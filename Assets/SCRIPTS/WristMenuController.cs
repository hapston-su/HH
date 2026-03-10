using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class WristMenuController : MonoBehaviour
{
    [Header("Canvas Reference")]
    public GameObject wristMenuCanvas;

    [Header("Attach Settings")]
    public Vector3 positionOffset = new Vector3(0f, 0.05f, 0f);
    public Vector3 rotationOffset = new Vector3(0f, 0f, 0f);

    [Header("Canvas Settings")]
    public Vector3 canvasScale = new Vector3(0.0003f, 0.0003f, 0.0003f);

    [Header("Tilt Settings")]
    public float tiltThreshold = 60f;
    public float hideDelay = 1.5f;

    private Transform leftControllerTransform;
    private bool menuVisible = false;
    private float hideTimer = 0f;

    void Start()
    {
        // Find left controller using XRBaseInputInteractor (Unity 6 approach)
        XRBaseInputInteractor[] interactors = FindObjectsByType<XRBaseInputInteractor>(FindObjectsSortMode.None);
        foreach (var interactor in interactors)
        {
            if (interactor.gameObject.name.ToLower().Contains("left"))
            {
                leftControllerTransform = interactor.transform;
                break;
            }
        }

        if (leftControllerTransform == null)
        {
            Debug.LogError("Left controller not found! Make sure your left controller object has 'left' in its name.");
        }

        if (wristMenuCanvas != null)
        {
            wristMenuCanvas.transform.localScale = canvasScale;
            wristMenuCanvas.SetActive(false);
        }
    }

    void Update()
    {
        if (leftControllerTransform == null || wristMenuCanvas == null) return;

        // Attach canvas to controller every frame
        wristMenuCanvas.transform.position = leftControllerTransform.position
            + leftControllerTransform.TransformDirection(positionOffset);

        wristMenuCanvas.transform.rotation = leftControllerTransform.rotation
            * Quaternion.Euler(rotationOffset);

        // Check tilt
        float tiltAngle = Vector3.Angle(leftControllerTransform.up, Vector3.up);

        if (tiltAngle > tiltThreshold)
        {
            menuVisible = true;
            hideTimer = hideDelay;
        }
        else
        {
            if (menuVisible)
            {
                hideTimer -= Time.deltaTime;
                if (hideTimer <= 0f)
                {
                    menuVisible = false;
                }
            }
        }

        wristMenuCanvas.SetActive(menuVisible);
    }
}