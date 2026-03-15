using UnityEngine;

public class Room1Door : MonoBehaviour
{
    public PlayerInventory inventory;
    public Transform door;

    public float openAngle = 90f;
    public float speed = 2f;

    private bool opening = false;
    private Quaternion closedRot;
    private Quaternion openRot;

    void Start()
    {
        closedRot = door.localRotation;
        openRot = Quaternion.Euler(door.localEulerAngles + new Vector3(0, openAngle, 0));
    }

    public void TryOpenDoor()
    {
        if (inventory.hasRoom1Key)
        {
            Debug.Log("Room 1 Door unlocked!");
            opening = true;
        }
        else
        {
            Debug.Log("You need the Room 1 key.");
        }
    }

    void Update()
    {
        if (opening)
        {
            door.localRotation = Quaternion.Slerp(door.localRotation, openRot, Time.deltaTime * speed);
        }
    }
}