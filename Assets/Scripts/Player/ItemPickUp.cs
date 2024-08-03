using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        Item item = other.GetComponent<Item>();

        if (item != null) {
            ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(item.ItemCode);

            Debug.Log(itemDetails.itemDescription);
        }
    }
}
