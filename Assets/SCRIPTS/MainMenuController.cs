using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject menuCanvas;

    void Start()
    {
        Time.timeScale = 0f;   // pause the game
        menuCanvas.SetActive(true);
    }

    public void StartGame()
    {
        menuCanvas.SetActive(false);
        Time.timeScale = 1f;   // resume the game
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game");

        Application.Quit();
    }
}