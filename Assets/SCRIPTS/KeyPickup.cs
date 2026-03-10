using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public PlayerInventory inventory;

    public void PickUpKey()
    {
        inventory.hasKey = true;
        Debug.Log("Key collected!");

        Destroy(gameObject);
    }
}