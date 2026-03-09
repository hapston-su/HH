using UnityEngine;

public class CreepyDollController : MonoBehaviour
{
    public Transform player;
    public Light flashlight;

    public float activationDelay = 5f;
    public float teleportInterval = 3f;
    public float teleportDistance = 2f;

    private bool active = false;
    private float teleportTimer;

    void Start()
    {
        Invoke(nameof(ActivateDoll), activationDelay);
    }

    public void ActivateDoll()
    {
        active = true;
    }

    void Update()
    {
        if (!active) return;

        LookAtPlayer();

        if (FlashlightHit())
            return;

        teleportTimer += Time.deltaTime;

        if (teleportTimer >= teleportInterval)
        {
            TeleportCloser();
            teleportTimer = 0;
        }
    }

    void LookAtPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    bool FlashlightHit()
    {
        Ray ray = new Ray(flashlight.transform.position, flashlight.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, flashlight.range))
        {
            if (hit.collider.gameObject == gameObject)
                return true;
        }

        return false;
    }

    void TeleportCloser()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        // Stop teleporting if already close to player
        if (dist < 1.5f)
            return;
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 targetPos = transform.position + direction * teleportDistance;

        float checkRadius = 0.4f;

        if (!Physics.CheckSphere(targetPos, checkRadius))
        {
            transform.position = targetPos;
        }
    }
}