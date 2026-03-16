using UnityEngine;

public class Room1Door : MonoBehaviour
{
    public PlayerInventory inventory;
    public Transform door;
    private AudioSource doorAudio;

    public float openAngle = 90f;
    public float speed = 2f;

    private bool opening = false;
    private Quaternion closedRot;
    private Quaternion openRot;

    void Start()
    {
        closedRot = door.localRotation;
        openRot = Quaternion.Euler(door.localEulerAngles + new Vector3(0, openAngle, 0));
        doorAudio = GetComponent<AudioSource>();
    }

    public void TryOpenDoor()
{
    if (inventory.hasRoom1Key)
    {
        Debug.Log("Door unlocked!");
        opening = true;

        if (doorAudio != null && !doorAudio.isPlaying)
        {
            doorAudio.Play();
        }
    }
    else
    {
        Debug.Log("Door is locked. Find the key.");
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