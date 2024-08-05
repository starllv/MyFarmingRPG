using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        Item item = other.GetComponent<Item>();

        if (item != null) {
            ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(item.ItemCode);

            if (itemDetails.canBePickUp == true) {

                InventoryManager.Instance.AddItem(InventoryLocation.player, item, other.gameObject);
            }
        }
    }
}
