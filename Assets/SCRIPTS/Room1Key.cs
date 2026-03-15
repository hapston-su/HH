using UnityEngine;

public class Room1Key : MonoBehaviour
{
    public PlayerInventory inventory;

    public void PickupKey()
    {
        inventory.hasRoom1Key = true;

        Debug.Log("Room 1 key collected");

        Destroy(gameObject);
    }
}