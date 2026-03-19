using UnityEngine;

public class Room2Door : MonoBehaviour
{
    public PlayerInventory inventory;
    public Transform door;

    public GameObject escapeUI;   // UI that appears when player wins

    public float openAngle = 90f;
    public float speed = 2f;

    private bool opening = false;
    private Quaternion closedRot;
    private Quaternion openRot;

    void Start()
    {
        closedRot = door.localRotation;
        openRot = Quaternion.Euler(door.localEulerAngles + new Vector3(0, openAngle, 0));

        if (escapeUI != null)
            escapeUI.SetActive(false);
    }

    public void TryOpenDoor()
    {
        if (inventory.hasRoom2Key)
        {
            Debug.Log("Room 2 Door unlocked! You escaped!");
            opening = true;

            Invoke(nameof(ShowEscapeUI), 1.5f); // delay so door opens first
        }
        else
        {
            Debug.Log("You need the Room 2 key.");
        }
    }

    void Update()
    {
        if (opening)
        {
            door.localRotation = Quaternion.Slerp(
                door.localRotation,
                openRot,
                Time.deltaTime * speed
            );
        }
    }

    void ShowEscapeUI()
    {
        if (escapeUI == null)
        {
            Debug.LogWarning("Escape UI not assigned!");
            return;
        }

        // Place UI in front of player
        Transform cam = Camera.main.transform;

        escapeUI.transform.position = cam.position + cam.forward * 2f;
        escapeUI.transform.rotation = Quaternion.LookRotation(
            escapeUI.transform.position - cam.position
        );

        escapeUI.SetActive(true);

        Time.timeScale = 0f; // stop the game
    }
}