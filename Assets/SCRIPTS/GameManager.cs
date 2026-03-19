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

    // -------- GAME CONTROL --------

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

    // -------- KEY STATUS --------

    public void PlayerGotKey()
    {
        if (gotKey) return;

        gotKey = true;
        SendStatus("GOT_KEY", gotKey);

        Debug.Log("Player Got Key");
    }

    public void PlayerLostKey()
    {
        if (!gotKey) return;

        gotKey = false;
        SendStatus("GOT_KEY", gotKey);

        Debug.Log("Player Lost Key");
    }

    public void PlayerGotMasterKey()
    {
        //if (gotMasterKey) return;

       // gotMasterKey = true;

       // if (playerInventory != null)
       // {
       //     playerInventory.hasRoom1Key = true;
       //     playerInventory.hasRoom2Key = true;
       // }

        //SendStatus("GOT_MASTER_KEY", gotMasterKey);
        ToggleTorch();
        Debug.Log("Player Got Master Key");
    }

    public void PlayerLostMasterKey()
    {
        if (!gotMasterKey) return;

        gotMasterKey = false;

        if (playerInventory != null)
        {
            playerInventory.hasRoom1Key = false;
            playerInventory.hasRoom2Key = false;
        }

        SendStatus("GOT_MASTER_KEY", gotMasterKey);

        Debug.Log("Player Lost Master Key");
    }

    // -------- HELP STATUS --------

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

    // -------- INVENTORY HELPERS --------

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
            Debug.LogWarning("ElectricTorchOnOff reference missing.");
            return;
        }

        electricTorch.ToggleTorch();
        Debug.Log("Torch toggled from GameManager");
    }

    public void TorchOn()
    {
        Debug.Log("Torch ON command received");
        SetTorch(true);
    }

    public void TorchOff()
    {
        Debug.Log("Torch OFF command received");
        SetTorch(false);
    }

    public void SetTorch(bool state)
    {
        if (electricTorch == null)
        {
            Debug.LogWarning("ElectricTorchOnOff reference missing.");
            return;
        }

        bool torchCurrentlyOn = IsTorchOn();

        if (state && !torchCurrentlyOn)
            electricTorch.ToggleTorch();

        if (!state && torchCurrentlyOn)
            electricTorch.ToggleTorch();

        Debug.Log("Torch state set to: " + state);
    }

    private bool IsTorchOn()
    {
        Light torchLight = electricTorch.GetComponent<Light>();

        if (torchLight == null)
            return false;

        return torchLight.intensity > 0;
    }

    // -------- ESP32 STATUS SYNC --------

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
            Debug.LogWarning("WebSocketClient reference missing.");
        }
    }
}