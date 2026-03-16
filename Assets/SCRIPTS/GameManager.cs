using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private WebSocketClient webSocketClient;
    [SerializeField] private ElectricTorchOnOff electricTorch;
    [SerializeField] private PlayerInventory playerInventory;

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
            webSocketClient = FindFirstObjectByType<WebSocketClient>();

        if (electricTorch == null)
            electricTorch = FindFirstObjectByType<ElectricTorchOnOff>();

        if (playerInventory == null)
            playerInventory = FindFirstObjectByType<PlayerInventory>();
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
        needHelp = false;

        ResetInventoryKeys();

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

        ResetInventoryKeys();

        SendAllStatuses();
        Debug.Log("Game statuses reset");
    }

    public void RestartGame()
    {
        Debug.Log("Restart requested from ESP32");
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

        if (playerInventory != null)
        {
            playerInventory.hasRoom1Key = true;
            playerInventory.hasRoom2Key = true;
        }
        else
        {
            Debug.LogWarning("PlayerInventory reference missing in GameManager.");
        }

        SendStatus("GOT_MASTER_KEY", gotMasterKey);

        Debug.Log("Player Got Master Key");
    }

    public void PlayerLostMasterKey()
    {
        if (!gotMasterKey)
            return;

        gotMasterKey = false;

        if (playerInventory != null)
        {
            playerInventory.hasRoom1Key = false;
            playerInventory.hasRoom2Key = false;
        }
        else
        {
            Debug.LogWarning("PlayerInventory reference missing in GameManager.");
        }

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

        if (playerInventory != null)
        {
            playerInventory.hasRoom1Key = value;
            playerInventory.hasRoom2Key = value;
        }
        else
        {
            Debug.LogWarning("PlayerInventory reference missing in GameManager.");
        }

        SendStatus("GOT_MASTER_KEY", gotMasterKey);
    }

    // -------- INVENTORY HELPERS --------

    public void GiveMasterKeyToInventory()
    {
        if (playerInventory == null)
        {
            Debug.LogWarning("PlayerInventory reference missing in GameManager.");
            return;
        }

        playerInventory.hasRoom1Key = true;
        playerInventory.hasRoom2Key = true;

        gotMasterKey = true;
        SendStatus("GOT_MASTER_KEY", gotMasterKey);

        Debug.Log("Master Key applied: Room1Key = true, Room2Key = true");
    }

    public void RemoveMasterKeyFromInventory()
    {
        if (playerInventory == null)
        {
            Debug.LogWarning("PlayerInventory reference missing in GameManager.");
            return;
        }

        playerInventory.hasRoom1Key = false;
        playerInventory.hasRoom2Key = false;

        gotMasterKey = false;
        SendStatus("GOT_MASTER_KEY", gotMasterKey);

        Debug.Log("Master Key removed: Room1Key = false, Room2Key = false");
    }

    private void ResetInventoryKeys()
    {
        if (playerInventory == null)
            return;

        playerInventory.hasRoom1Key = false;
        playerInventory.hasRoom2Key = false;
    }

    // -------- TORCH CONTROL --------

    public void ToggleTorch()
    {
        if (electricTorch == null)
        {
            Debug.LogWarning("ElectricTorchOnOff reference missing in GameManager.");
            return;
        }

        electricTorch.ToggleTorch();
        Debug.Log("Torch toggled from GameManager");
    }

    public void TorchOn()
    {
        SetTorch(true);
    }

    public void TorchOff()
    {
        SetTorch(false);
    }

    public void SetTorch(bool state)
    {
        if (electricTorch == null)
        {
            Debug.LogWarning("ElectricTorchOnOff reference missing in GameManager.");
            return;
        }

        electricTorch.SetTorch(state);
        Debug.Log("Torch set to: " + state);
    }

    // -------- ESP32 SYNC --------

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
}