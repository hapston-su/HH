using UnityEngine;

public class VRMenuSpawner : MonoBehaviour
{
    public GameObject menuObject;
    private Transform playerCamera;

    [Header("Menu Settings")]
    public float distanceFromPlayer = 2f;
    public float followSpeed = 5f;        // Smoothing speed, higher = snappier
    public float angleTreshold = 30f;     // Only reposition if player looks away by this many degrees

    private Vector3 targetPosition;

    void Start()
    {
        playerCamera = Camera.main.transform;
        PositionMenuInFrontOfPlayer();
    }

    void Update()
    {
        if (menuObject == null || playerCamera == null) return;

        // Check the angle between where the menu is and where the player is looking
        Vector3 directionToMenu = menuObject.transform.position - playerCamera.position;
        float angle = Vector3.Angle(playerCamera.forward, directionToMenu);

        // Only reposition the menu if the player looks away past the threshold
        if (angle > angleTreshold)
        {
            PositionMenuInFrontOfPlayer();
        }

        // Smoothly move the menu to the target position
        menuObject.transform.position = Vector3.Lerp(
            menuObject.transform.position,
            targetPosition,
            Time.deltaTime * followSpeed
        );

        // Smoothly rotate menu to always face the player
        Quaternion targetRotation = Quaternion.LookRotation(
            menuObject.transform.position - playerCamera.position
        );
        menuObject.transform.rotation = Quaternion.Slerp(
            menuObject.transform.rotation,
            targetRotation,
            Time.deltaTime * followSpeed
        );
    }

    void PositionMenuInFrontOfPlayer()
    {
        // Calculates new target position in front of the player
        targetPosition = playerCamera.position + playerCamera.forward * distanceFromPlayer;
        targetPosition.y = playerCamera.position.y; // Keep at headset height
    }
}