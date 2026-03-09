using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    public PlayerInventory inventory;
    public Transform door;

    public float openAngle = 90f;
    public float speed = 2f;

    public CreepyDollController doll;   // reference to the doll

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
        if (inventory.hasKey)
        {
            Debug.Log("Door unlocked!");
            opening = true;

            if (doll != null)
            {
                Invoke(nameof(ActivateDoll), 5f); // wait 5 seconds
            }
        }
        else
        {
            Debug.Log("Door is locked. Find the key.");
        }
    }

    void ActivateDoll()
    {
        doll.ActivateDoll();
    }

    void Update()
    {
        if (opening)
        {
            door.localRotation = Quaternion.Slerp(door.localRotation, openRot, Time.deltaTime * speed);
        }
    }
}