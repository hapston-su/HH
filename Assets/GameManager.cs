using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private WebSocketClient webSocketClient;

    [Header("Game Status")]
    [SerializeField] private bool gameStarted = false;
    [SerializeField] private bool exitedSuccessfully = false;
    [SerializeField] private bool gameOver = false;
    [SerializeField] private bool gotKey = false;
    [SerializeField] private bool gotMasterKey = false;

    public bool GameStarted => gameStarted;
    public bool ExitedSuccessfully => exitedSuccessfully;
    public bool GameOver => gameOver;
    public bool GotKey => gotKey;
    public bool GotMasterKey => gotMasterKey;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (webSocketClient == null)
        {
            webSocketClient = FindFirstObjectByType<WebSocketClient>();
        }
    }

    private void Start()
    {
        SyncAllStatusesToESP32();
    }

    public void StartGame()
    {
        gameStarted = true;
        exitedSuccessfully = false;
        gameOver = false;
        gotKey = false;
        gotMasterKey = false;

        SendAllStatuses();
        Debug.Log("Game Started");
    }

    public void PlayerExitedSuccessfully()
    {
        exitedSuccessfully = true;
        gameOver = false;

        SendStatus("EXITED_SUCCESSFULLY", exitedSuccessfully);
        SendStatus("GAME_OVER", gameOver);

        Debug.Log("Player Exited Successfully");
    }

    public void PlayerGameOver()
    {
        gameOver = true;
        exitedSuccessfully = false;

        SendStatus("GAME_OVER", gameOver);
        SendStatus("EXITED_SUCCESSFULLY", exitedSuccessfully);

        Debug.Log("Game Over");
    }

    public void PlayerGotKey()
    {
        if (gotKey)
            return;

        gotKey = true;
        SendStatus("GOT_KEY", gotKey);

        Debug.Log("Player Got Key");
    }

    public void PlayerGotMasterKey()
    {
        if (gotMasterKey)
            return;

        gotMasterKey = true;
        SendStatus("GOT_MASTER_KEY", gotMasterKey);

        Debug.Log("Player Got Master Key");
    }

    public void ResetGameStatuses()
    {
        gameStarted = false;
        exitedSuccessfully = false;
        gameOver = false;
        gotKey = false;
        gotMasterKey = false;

        SendAllStatuses();
        Debug.Log("Game statuses reset");
    }

    public void SetGameStarted(bool value)
    {
        gameStarted = value;
        SendStatus("GAME_STARTED", gameStarted);
    }

    public void SetExitedSuccessfully(bool value)
    {
        exitedSuccessfully = value;
        SendStatus("EXITED_SUCCESSFULLY", exitedSuccessfully);
    }

    public void SetGameOver(bool value)
    {
        gameOver = value;
        SendStatus("GAME_OVER", gameOver);
    }

    public void SetGotKey(bool value)
    {
        gotKey = value;
        SendStatus("GOT_KEY", gotKey);
    }

    public void SetGotMasterKey(bool value)
    {
        gotMasterKey = value;
        SendStatus("GOT_MASTER_KEY", gotMasterKey);
    }

    public void SyncAllStatusesToESP32()
    {
        SendAllStatuses();
    }

    private void SendAllStatuses()
    {
        SendStatus("GAME_STARTED", gameStarted);
        SendStatus("EXITED_SUCCESSFULLY", exitedSuccessfully);
        SendStatus("GAME_OVER", gameOver);
        SendStatus("GOT_KEY", gotKey);
        SendStatus("GOT_MASTER_KEY", gotMasterKey);
    }

    private void SendStatus(string statusName, bool value)
    {
        if (webSocketClient != null)
        {
            webSocketClient.SetStatus(statusName, value);
        }
        else
        {
            Debug.LogWarning("WebSocketClient reference missing in GameManager");
        }
    }
}