using UnityEngine;
using System;
using System.Collections.Generic;
using NativeWebSocket;
using System.Net.WebSockets;
using NativeWS = NativeWebSocket.WebSocket;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WebSocketClient : MonoBehaviour
{
    private NativeWS socket;

    [Header("Server Settings")]
    [SerializeField] private string serverIP = "10.204.0.50";
    [SerializeField] private int serverPort = 8081;

    [Header("Game Status")]
    [SerializeField] private bool gameStarted = false;
    [SerializeField] private bool exitedSuccessfully = false;
    [SerializeField] private bool gameOver = false;
    [SerializeField] private bool gotKey = false;
    [SerializeField] private bool gotMasterKey = false;

    private readonly Dictionary<string, bool> statusMap = new Dictionary<string, bool>();

    public bool IsConnected => socket != null && socket.State == WebSocketState.Open;

    private async void Start()
    {
        statusMap["GAME_STARTED"] = gameStarted;
        statusMap["EXITED_SUCCESSFULLY"] = exitedSuccessfully;
        statusMap["GAME_OVER"] = gameOver;
        statusMap["GOT_KEY"] = gotKey;
        statusMap["GOT_MASTER_KEY"] = gotMasterKey;

        socket = new NativeWS("ws://" + serverIP + ":" + serverPort);

        socket.OnOpen += async () =>
        {
            Debug.Log("Connected to WebSocket server");

            string uuid = SystemInfo.deviceUniqueIdentifier;
            string deviceMessage = "Device (Unity): " + SystemInfo.deviceName + " | UUID: " + uuid;

            await socket.SendText(deviceMessage);
            await SendAllStatuses();
        };

        socket.OnMessage += (bytes) =>
        {
            string message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("Received: " + message);
            IncomingMessageParser(message);
        };

        socket.OnClose += (code) =>
        {
            Debug.Log("WebSocket closed with code: " + code);
        };

        socket.OnError += (errorMsg) =>
        {
            Debug.LogError("WebSocket error: " + errorMsg);
        };

        await socket.Connect();
    }

    private void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        socket?.DispatchMessageQueue();
#endif
    }

    private async void OnDestroy()
    {
        if (socket != null)
        {
            await socket.Close();
        }
    }

    public async void SetStatus(string statusName, bool value)
    {
        statusName = statusName.ToUpperInvariant();

        if (!statusMap.ContainsKey(statusName))
        {
            Debug.LogWarning("Unknown status: " + statusName);
            return;
        }

        statusMap[statusName] = value;
        SyncInspectorValues();

        if (socket != null && socket.State == WebSocketState.Open)
        {
            string message = $"{statusName}:{value.ToString().ToLower()}";
            await socket.SendText(message);
            Debug.Log("Sent: " + message);
        }
        else
        {
            Debug.LogWarning("WebSocket not connected. Could not send: " + statusName);
        }
    }

    public async void SendRawMessage(string message)
    {
        if (socket != null && socket.State == WebSocketState.Open)
        {
            await socket.SendText(message);
            Debug.Log("Sent raw: " + message);
        }
        else
        {
            Debug.LogWarning("WebSocket not connected");
        }
    }

    private async System.Threading.Tasks.Task SendAllStatuses()
    {
        if (socket == null || socket.State != WebSocketState.Open)
            return;

        foreach (var item in statusMap)
        {
            string message = $"{item.Key}:{item.Value.ToString().ToLower()}";
            await socket.SendText(message);
            Debug.Log("Sent on connect: " + message);
        }
    }

    private void SyncInspectorValues()
    {
        gameStarted = statusMap["GAME_STARTED"];
        exitedSuccessfully = statusMap["EXITED_SUCCESSFULLY"];
        gameOver = statusMap["GAME_OVER"];
        gotKey = statusMap["GOT_KEY"];
        gotMasterKey = statusMap["GOT_MASTER_KEY"];
    }

    public void SetGameStarted(bool value) => SetStatus("GAME_STARTED", value);
    public void SetExitedSuccessfully(bool value) => SetStatus("EXITED_SUCCESSFULLY", value);
    public void SetGameOver(bool value) => SetStatus("GAME_OVER", value);
    public void SetGotKey(bool value) => SetStatus("GOT_KEY", value);
    public void SetGotMasterKey(bool value) => SetStatus("GOT_MASTER_KEY", value);

    public void IncomingMessageParser(string msg)
    {
        int separatorIndex = msg.IndexOf(":");
        if (separatorIndex < 0)
            return;

        string type = msg.Substring(0, separatorIndex);
        string value = msg.Substring(separatorIndex + 1);

        if (type.Equals("button", StringComparison.OrdinalIgnoreCase))
        {
            if (value == "1")
            {
                Debug.Log("ESP32 Button Pressed");
            }
            else if (value == "0")
            {
                Debug.Log("ESP32 Button Released");
            }
        }
    }
}