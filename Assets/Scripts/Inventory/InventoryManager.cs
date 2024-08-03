using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletonMonoBehaviour<InventoryManager>
{
    private Dictionary<int, ItemDetails> itemDetailsDictionary;

    [SerializeField]
    private SO_ItemList itemList = null;

    private void Start() {
        CreateItemDetailsDictionary();
    }

    private void CreateItemDetailsDictionary() {
        itemDetailsDictionary = new Dictionary<int, ItemDetails>();

        foreach (ItemDetails itemDetails in itemList.itemDetails) {
            itemDetailsDictionary.Add(itemDetails.ItemCode, itemDetails);
        }
    }

    public ItemDetails GetItemDetails(int itemCode) {
        ItemDetails itemDetails;

        if (itemDetailsDictionary.TryGetValue(itemCode, out itemDetails)) {
            return itemDetails;
        }
        else {
            return null;
        }
    }
}
