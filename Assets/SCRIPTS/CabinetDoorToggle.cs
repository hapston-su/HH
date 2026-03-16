using UnityEngine;

public class CabinetDoorToggle : MonoBehaviour
{
    public Transform door;
    public Vector3 openRotation = new Vector3(0, 90, 0);
    public float speed = 2f;

    [Header("Audio")]
    public AudioSource doorAudio;

    private bool isOpen = false;
    private Quaternion closedRot;
    private Quaternion openRot;

    void Start()
    {
        if (door == null)
            door = transform;

        closedRot = door.localRotation;
        openRot = Quaternion.Euler(door.localEulerAngles + openRotation);

        // Automatically grab AudioSource if not assigned
        if (doorAudio == null)
            doorAudio = GetComponent<AudioSource>();
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;

        // Play creak sound when door starts moving
        if (doorAudio != null)
        {
            doorAudio.pitch = Random.Range(0.9f, 1.1f); // slight variation
            doorAudio.Play();
        }
    }

    void Update()
    {
        Quaternion targetRot = isOpen ? openRot : closedRot;
        door.localRotation = Quaternion.Slerp(door.localRotation, targetRot, Time.deltaTime * speed);
    }
}