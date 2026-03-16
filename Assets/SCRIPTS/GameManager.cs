using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private WebSocketClient webSocketClient;
    [SerializeField] private GameObject electricTorch;
    private ElectricTorchOnOff torchScript;

    [Header("Exit UI")]
    [SerializeField] private GameObject exitCanvas;
    [SerializeField] private GameObject xrOrigin;

    [Header("Game Status")]
    [SerializeField] private bool gameStarted;
    [SerializeField] private bool exitedSuccessfully;
    [SerializeField] private bool gameOver;
    [SerializeField] private bool gotKey;
    [SerializeField] private bool gotMasterKey;
    [SerializeField] private bool needHelp;

    public bool GameStarted => gameStarted;
    public bool ExitedSuccessfully => exitedSuccessfully;
    public bool GameOver => gameOver;
    public bool GotKey => gotKey;
    public bool GotMasterKey => gotMasterKey;
    public bool NeedHelp => needHelp;

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

        if (electricTorch == null)
        {
            electricTorch = FindFirstObjectByType<ElectricTorchOnOff>();
        }
    }

    private void Start()
    {
        SyncAllStatusesToESP32();

        if (exitCanvas != null)
            exitCanvas.SetActive(false);
    }

    public void StartGame()
    {
        gameStarted = true;
        exitedSuccessfully = false;
        gameOver = false;
        gotKey = false;
        gotMasterKey = false;
        needHelp = false;

        SendAllStatuses();
        Debug.Log("Game Started");
    }

    public void ResetGameStatuses()
    {
        gameStarted = false;
        exitedSuccessfully = false;
        gameOver = false;
        gotKey = false;
        gotMasterKey = false;
        needHelp = false;

        SendAllStatuses();
        Debug.Log("Game statuses reset");
    }

    public void RestartGame()
    {
        Debug.Log("Restart requested");

        Time.timeScale = 1f;

        ResetGameStatuses();

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void PlayerExitedSuccessfully()
    {
        exitedSuccessfully = true;
        gameOver = false;
        needHelp = false;

        SendStatus("EXITED_SUCCESSFULLY", exitedSuccessfully);
        SendStatus("GAME_OVER", gameOver);
        SendStatus("NEED_HELP", needHelp);

        Debug.Log("Player Exited Successfully");

        ShowExitUI();
    }

    public void PlayerGameOver()
    {
        gameOver = true;
        exitedSuccessfully = false;

        SendStatus("GAME_OVER", gameOver);
        SendStatus("EXITED_SUCCESSFULLY", exitedSuccessfully);

        Debug.Log("Game Over");
    }

    void ShowExitUI()
    {
        Debug.Log("Showing Exit UI");

        // Pause gameplay
        Time.timeScale = 0f;

        // Disable player movement
        if (xrOrigin != null)
            xrOrigin.SetActive(false);

        // Show exit canvas
        if (exitCanvas != null)
            exitCanvas.SetActive(true);
    }

    public void PlayerGotKey()
    {
        if (gotKey)
            return;

        gotKey = true;
        SendStatus("GOT_KEY", gotKey);

        Debug.Log("Player Got Key");
    }

    public void PlayerLostKey()
    {
        if (!gotKey)
            return;

        gotKey = false;
        SendStatus("GOT_KEY", gotKey);

        Debug.Log("Player Lost Key");
    }

    public void PlayerGotMasterKey()
    {
        if (gotMasterKey)
            return;

        gotMasterKey = true;
        SendStatus("GOT_MASTER_KEY", gotMasterKey);

        Debug.Log("Player Got Master Key");
    }

    public void PlayerLostMasterKey()
    {
        if (!gotMasterKey)
            return;

        gotMasterKey = false;
        SendStatus("GOT_MASTER_KEY", gotMasterKey);

        Debug.Log("Player Lost Master Key");
    }

    public void SetNeedHelp(bool value)
    {
        needHelp = value;
        SendStatus("NEED_HELP", needHelp);

        Debug.Log("Need Help set to: " + value);
    }

    public void RequestHelp()
    {
        SetNeedHelp(true);
    }

    public void ClearHelpRequest()
    {
        SetNeedHelp(false);
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

    // ---------- TORCH CONTROL ----------

    public void ToggleTorch()
    {
        SetTorch(!torchOn);
    }

    public void ESP32_TorchButtonPressed()
    {
        Debug.Log("ESP32 Torch Button Pressed");
        ToggleTorch();
    }

    // ---------- ESP32 SYNC ----------

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
        SendStatus("NEED_HELP", needHelp);
    }

    private void SendStatus(string statusName, bool value)
    {
        if (webSocketClient != null)
        {
            webSocketClient.SendStatus(statusName, value);
        }
        else
        {
            Debug.LogWarning("WebSocketClient reference missing in GameManager.");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");

        Application.Quit();
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");

        Application.Quit();
    }
}