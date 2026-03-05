using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorToggle : MonoBehaviour
{
    public Transform door;
    public float openAngle = 90f;
    public float speed = 2f;

    private bool isOpen = false;
    private Quaternion closedRot;
    private Quaternion openRot;

    void Start()
    {
        closedRot = door.localRotation;
        openRot = Quaternion.Euler(door.localEulerAngles + new Vector3(0, openAngle, 0));
        UnityEngine.Debug.Log("Doorcode2");
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
    }

    void Update()
    {
        UnityEngine.Debug.Log("Doorcode2");
        if (isOpen)
            door.localRotation = Quaternion.Lerp(door.localRotation, openRot, Time.deltaTime * speed);
        else
            door.localRotation = Quaternion.Lerp(door.localRotation, closedRot, Time.deltaTime * speed);
    }
}