using UnityEngine;

public class Room2Key : MonoBehaviour
{
    public PlayerInventory inventory;

    public void PickupKey()
    {
        inventory.hasRoom2Key = true;

        Debug.Log("Room 2 key collected");

        Destroy(gameObject);
    }
}