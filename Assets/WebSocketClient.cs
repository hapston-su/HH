using UnityEngine;
using System;
using NativeWS = NativeWebSocket.WebSocket;

public class WebSocketClient : MonoBehaviour
{
    private NativeWS socket;

    [Header("Server Settings")]
    [SerializeField] private string serverIP = "10.204.0.50";
    [SerializeField] private int serverPort = 8081;

    public bool IsConnected => socket != null && socket.State == NativeWebSocket.WebSocketState.Open;

    private async void Start()
    {
        socket = new NativeWS("ws://" + serverIP + ":" + serverPort);

        socket.OnOpen += async () =>
        {
            Debug.Log("Connected to WebSocket server");

            string uuid = SystemInfo.deviceUniqueIdentifier;
            string deviceMessage = "Device (Unity): " + SystemInfo.deviceName + " | UUID: " + uuid;

            await socket.SendText(deviceMessage);
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

    public async void SendStatus(string statusName, bool value)
    {
        if (socket != null && socket.State == NativeWebSocket.WebSocketState.Open)
        {
            string message = $"{statusName.ToUpperInvariant()}:{value.ToString().ToLower()}";
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
        if (socket != null && socket.State == NativeWebSocket.WebSocketState.Open)
        {
            await socket.SendText(message);
            Debug.Log("Sent raw: " + message);
        }
        else
        {
            Debug.LogWarning("WebSocket not connected");
        }
    }

    private void IncomingMessageParser(string msg)
    {
        int separatorIndex = msg.IndexOf(":");

        if (separatorIndex < 0)
        {
            Debug.Log("Invalid message format: " + msg);
            return;
        }

        string type = msg.Substring(0, separatorIndex).Trim().ToLower();
        string value = msg.Substring(separatorIndex + 1).Trim().ToLower();

        if (type == "restart_button" && value == "1")
        {
            Debug.Log("ESP32 Restart Button Pressed");

            if (GameManager.Instance != null)
                GameManager.Instance.RestartGame();
        }
        else if (type == "key_button" && value == "1")
        {
            Debug.Log("ESP32 Key Button Pressed");

            if (GameManager.Instance != null)
                GameManager.Instance.PlayerGotMasterKey();
        }
        else if (type == "need_help_clear" && value == "1")
        {
            Debug.Log("ESP32 Help Clear Button Pressed");

            if (GameManager.Instance != null)
                GameManager.Instance.ClearHelpRequest();
        }
        else if (type == "torch_on" && value == "1")
        {
            Debug.Log("ESP32 Torch ON");

            if (GameManager.Instance != null)
                GameManager.Instance.PlayerGotMasterKey();
        }
        else if (type == "torch_off" && value == "1")
        {
            Debug.Log("ESP32 Torch OFF");

            if (GameManager.Instance != null)
                GameManager.Instance.PlayerGotMasterKey();
        }
        else if (type == "torch_toggle" && value == "1")
        {
            Debug.Log("ESP32 Torch TOGGLE");

            if (GameManager.Instance != null)
                GameManager.Instance.ToggleTorch();
        }
        else if (type == "button")
        {
            if (value == "1")
            {
                Debug.Log("ESP32 Generic Button Pressed");
            }
            else if (value == "0")
            {
                Debug.Log("ESP32 Generic Button Released");
            }
        }
        else
        {
            Debug.Log("Unknown message received: " + msg);
        }
    }
}