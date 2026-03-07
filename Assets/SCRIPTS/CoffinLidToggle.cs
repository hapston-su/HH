using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CoffinLidToggle : MonoBehaviour
{
    public Transform lid;
    public float openAngle = 110f;
    public float speed = 2f;

    private bool isOpen = false;
    private Quaternion closedRot;
    private Quaternion openRot;

    void Start()
    {
        closedRot = lid.localRotation;
        openRot = Quaternion.Euler(lid.localEulerAngles + new Vector3(0, 0,-openAngle ));
    }

    public void ToggleLid()
    {
        isOpen = !isOpen;
    }

    void Update()
    {
        if (isOpen)
        {
            lid.localRotation = Quaternion.Lerp(lid.localRotation, openRot, Time.deltaTime * speed);
        }
        else
        {
            lid.localRotation = Quaternion.Lerp(lid.localRotation, closedRot, Time.deltaTime * speed);
        }
    }
}