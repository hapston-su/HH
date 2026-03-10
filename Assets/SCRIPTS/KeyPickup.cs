using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public PlayerInventory inventory;
    private GameManager gm;

    void Start()
    {
        gm = FindFirstObjectByType<GameManager>();
    }

    public void PickUpKey()
    {
        inventory.hasKey = true;
        Debug.Log("Key collected!");

        if (gm != null)
        {
            gm.PlayerGotKey();
        }

        Destroy(gameObject);
    }
}