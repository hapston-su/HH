using UnityEngine;

public class CabinetDoorToggle : MonoBehaviour
{
    public Transform door;
    public Vector3 openRotation = new Vector3(0, 90, 0);
    public float speed = 2f;

    private bool isOpen = false;
    private Quaternion closedRot;
    private Quaternion openRot;

    void Start()
    {
        if (door == null)
            door = transform;

        closedRot = door.localRotation;
        openRot = Quaternion.Euler(door.localEulerAngles + openRotation);
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
    }

    void Update()
    {
        Quaternion targetRot = isOpen ? openRot : closedRot;
        door.localRotation = Quaternion.Slerp(door.localRotation, targetRot, Time.deltaTime * speed);
    }
}