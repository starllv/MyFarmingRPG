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
    // 交换物品栏中的物品位置
    // inventoryLocation：指示是那个物品栏
    // fromItem：要交换的物品位置
    // toItem：最终要要换到的位置
    public void SwapInventoryItems(InventoryLocation inventoryLocation, int fromItem, int toItem) {
        // fromItem和toItem要小于当前物品栏的存储的个数，即不能拖到空的物品栏，且两参数不能相等或小于0
        if (fromItem < inventoryLists[(int)inventoryLocation].Count && toItem < inventoryLists[(int)inventoryLocation].Count
            && fromItem != toItem && fromItem >= 0 && toItem >= 0) {
            // 获取物品栏中两个物品
            InventoryItem fromInventoryItem = inventoryLists[(int)inventoryLocation][fromItem];
            InventoryItem toInventoryItem = inventoryLists[(int)inventoryLocation][toItem];
            // 交换位置
            inventoryLists[(int)inventoryLocation][fromItem] = toInventoryItem;
            inventoryLists[(int)inventoryLocation][toItem] = fromInventoryItem;
            // 发出事件，停止物品栏UI进行更新
            EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);  
        }
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

    private void RemoveItemAtPosition(List<InventoryItem> inventoryList, int itemCode, int position) {

        InventoryItem inventoryItem = new InventoryItem();

        int quantity = inventoryList[position].itemQuantity - 1;

        if (quantity > 0) {

            inventoryItem.itemQuantity = quantity;
            inventoryItem.itemCode = itemCode;
            inventoryList[position] = inventoryItem;
        }
        else {

            inventoryList.RemoveAt(position);
        }
    }

    public void RemoveItem(InventoryLocation inventoryLocation, int itemCode) {

        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];

        int itemPosition = FindItemInInventory(inventoryLocation, itemCode);

        if (itemPosition != -1) {

            RemoveItemAtPosition(inventoryList, itemCode, itemPosition);
        }

        EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);
    }
    // private void DebugPrintInventoryList(List<InventoryItem> inventoryList) {

    //     foreach (InventoryItem inventoryItem in inventoryList) {
    //         Debug.Log("Item Description: " + InventoryManager.Instance.GetItemDetails(inventoryItem.itemCode).itemDescription + "    Item Quantity: " + inventoryItem.itemQuantity);
    //     }
    //     Debug.Log("***********************************************************************");
    // }
}
