using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuController : MonoBehaviour
{
    public GameObject menuCanvas;
    public InputActionReference startActionReference;
    private GameManager gm;


    void OnEnable()
    {
        startActionReference.action.performed += ctx => StartGame();
    }

    void OnDisable()
    {
        startActionReference.action.performed -= ctx => StartGame();
    }

    void Start()
    {
        Time.timeScale = 0f;   // pause the game
        menuCanvas.SetActive(true);
        gm = FindFirstObjectByType<GameManager>();
    }

    public void StartGame()
    {
        menuCanvas.SetActive(false);
        Time.timeScale = 1f;   // resume the game
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game");
        gm.RestartGame();

        Application.Quit();
    }
}