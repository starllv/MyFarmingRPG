using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletonMonoBehaviour<InventoryManager>
{
    private Dictionary<int, ItemDetails> itemDetailsDictionary;

    public List<InventoryItem>[] inventoryLists;

    [HideInInspector] public int[] inventoryListCapacityIntArray;

    [SerializeField] private SO_ItemList itemList = null;

    protected override void Awake() {

        base.Awake();

        CreateInventoryLists();

        CreateItemDetailsDictionary();
    }

    private void CreateInventoryLists() {
        inventoryLists = new List<InventoryItem>[(int)InventoryLocation.count];

        for (int i = 0; i < (int)InventoryLocation.count; i++) {
            inventoryLists[i] = new List<InventoryItem>();
        }

        inventoryListCapacityIntArray = new int[(int)InventoryLocation.count];

        inventoryListCapacityIntArray[(int)InventoryLocation.player] = Settings.playerInitialInventoryCapacity;
    }

    private void CreateItemDetailsDictionary() {
        itemDetailsDictionary = new Dictionary<int, ItemDetails>();

        foreach (ItemDetails itemDetails in itemList.itemDetails) {
            itemDetailsDictionary.Add(itemDetails.ItemCode, itemDetails);
        }
    }

    public void AddItem(InventoryLocation inventoryLocation, Item item) {

        int itemCode = item.ItemCode;
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];

        int itemPosition = FindItemInInventory(inventoryLocation, itemCode);

        if (itemPosition != -1) {
            AddItemAtPosition(inventoryList, itemCode, itemPosition);
        }
        else {
            AddItemAtPosition(inventoryList, itemCode);
        }

        EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);
    }

    public void AddItem(InventoryLocation inventoryLocation, Item item, GameObject gameObjectToDelete){
        
        AddItem(inventoryLocation, item);

        Destroy(gameObjectToDelete);
    }

    public int FindItemInInventory(InventoryLocation inventoryLocation, int itemCode) {
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];

        for (int i = 0; i < inventoryList.Count; i++) {
            if (inventoryList[i].itemCode == itemCode)
                return i;
        }

        return -1;
    } 

    private void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode) {

        InventoryItem inventoryItem = new InventoryItem();

        inventoryItem.itemCode = itemCode;
        inventoryItem.itemQuantity = 1;
        inventoryList.Add(inventoryItem);

        // DebugPrintInventoryList(inventoryList);
    }

    private void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode, int position) {

        InventoryItem inventoryItem = new InventoryItem();

        int quantity = inventoryList[position].itemQuantity + 1;
        inventoryItem.itemQuantity = quantity;
        inventoryItem.itemCode = itemCode;
        inventoryList[position] = inventoryItem;

        // Debug.ClearDeveloperConsole();
        // DebugPrintInventoryList(inventoryList);
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

    // private void DebugPrintInventoryList(List<InventoryItem> inventoryList) {

    //     foreach (InventoryItem inventoryItem in inventoryList) {
    //         Debug.Log("Item Description: " + InventoryManager.Instance.GetItemDetails(inventoryItem.itemCode).itemDescription + "    Item Quantity: " + inventoryItem.itemQuantity);
    //     }
    //     Debug.Log("***********************************************************************");
    // }
}
