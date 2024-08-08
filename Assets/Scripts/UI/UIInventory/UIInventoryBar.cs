using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryBar : MonoBehaviour
{
    [SerializeField] private Sprite blank16x16Sprite = null;          // 空白精灵，用于设置空物品槽，可在Inspector中指定
    [SerializeField] private UIInventorySlot[] inventorySlot = null;  // 物品栏的所以物品槽的列表，可在Inspector中指定
    public GameObject inventoryBarDraggedItem;                        // 用于获取当拖动时生成物品的预制件，可在Inspector中指定
    [HideInInspector] public GameObject inventoryTextBoxGameObject;   // 显示物品属性的文本框，在Inspector中隐藏
    private RectTransform rectTransform;                              // 获取物品栏的矩形变换组件，用于之后改变位置
    private bool _isInventoryBarPositionBottom = true;                // 物品栏状态，是否位于底部，默认在底部
    public bool IsInventoryBarPositionBottom { get => _isInventoryBarPositionBottom; set => _isInventoryBarPositionBottom = value; }

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }
    // 脚本激活时调用
    private void OnEnable() {
        // 订阅物品更新事件
        EventHandler.InventoryUpdatedEvent += InventoryUpdated;
    }
    // 脚本退出时调用
    private void OnDisable() {
        // 取消订阅物品更新事件
        EventHandler.InventoryUpdatedEvent -= InventoryUpdated;
    }

    private void Update() {
        // 每一帧都调用，满足条件则改变物品栏位置
        SwitchInventoryBarPosition();
    }
    // 清空物品栏的函数，即把每一个物品槽设置为空白，物品个数设置为0
    private void ClearInventorySlots() {

        if (inventorySlot.Length > 0) {

            for (int i = 0; i < inventorySlot.Length; i++) {

                inventorySlot[i].inventorySlotImage.sprite = blank16x16Sprite;
                inventorySlot[i].textMeshProUGUI.text = "";
                inventorySlot[i].itemDetails = null;
                inventorySlot[i].itemQuantity = 0;

                SetHighlightedInventorySlots(i);
            }
        }
    }
    // 物品栏更新函数，更新UI
    private void InventoryUpdated(InventoryLocation inventoryLocation, List<InventoryItem> inventoryList) {
        // 如果当前物品栏是角色的物品栏
        if (inventoryLocation == InventoryLocation.player) {

            ClearInventorySlots();
            // 判断物品槽个数是否大于零且传入的物品列表的个数大于零
            if (inventorySlot.Length > 0 && inventoryList.Count > 0) {
                // 遍历物品槽列表，更新每一个物品槽
                for (int i = 0; i < inventorySlot.Length; i++) {
                    // 判断有没有将所以物品列表中的物品添加到物品槽
                    if (i < inventoryList.Count) {

                        int itemCode = inventoryList[i].itemCode;
                        // 根据itemCode从物品管理器中获取物品详情
                        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCode);
                        // 若物品存在，更新UI的物品槽
                        if (itemDetails != null) {

                            inventorySlot[i].inventorySlotImage.sprite = itemDetails.itemSprite;
                            inventorySlot[i].textMeshProUGUI.text = inventoryList[i].itemQuantity.ToString();
                            inventorySlot[i].itemDetails = itemDetails;
                            inventorySlot[i].itemQuantity = inventoryList[i].itemQuantity;

                            SetHighlightedInventorySlots(i);
                        }
                    }
                    else {

                        break;
                    }
                }
            }
        }
    }
    // 根据角色在屏幕中的位置，更改物品栏是在屏幕下面还是上面显示，防止物品栏挡住角色
    private void SwitchInventoryBarPosition() {
        // 获取角色在屏幕中的位置，位置的左下角坐标为（0，0），右上角坐标为（1，1）
        Vector3 playerViewportPosition = Player.Instance.GetPlayerViewportPosition();
        // 若角色比较靠上而物品栏位于屏幕顶部时，将物品栏移到屏幕底部
        if (playerViewportPosition.y > 0.3f && IsInventoryBarPositionBottom == false) {

            rectTransform.pivot = new Vector2(0.5f, 0f);
            rectTransform.anchorMin = new Vector2(0.5f, 0f);
            rectTransform.anchorMax = new Vector2(0.5f, 0f);
            rectTransform.anchoredPosition = new Vector2(0f, 2.5f);
            // 更改物品栏状态
            IsInventoryBarPositionBottom = true;
        }
        // 若角色比较靠下而物品栏位于屏幕底部时，将物品栏移到屏幕顶部
        else if (playerViewportPosition.y <= 0.3f && IsInventoryBarPositionBottom == true) {

            rectTransform.pivot = new Vector2(0.5f, 1f);
            rectTransform.anchorMin = new Vector2(0.5f, 1f);
            rectTransform.anchorMax = new Vector2(0.5f, 1f);
            rectTransform.anchoredPosition = new Vector2(0f, -2.5f);
            // 更改物品栏状态
            IsInventoryBarPositionBottom = false;
        }
    }
    // 清除物品槽的高亮效果
    public void ClearHighlightOnInventorySlots() {

        if (inventorySlot.Length > 0) {
            // 遍历所以物品槽
            for (int i = 0; i < inventorySlot.Length; i++) {
                // 若物品槽被选中，则清除选择，将高亮显示框设为透明，然后通知仓库管理器
                if (inventorySlot[i].isSelected) {

                    inventorySlot[i].isSelected = false;
                    inventorySlot[i].inventorySlotHighlight.color = new Color(0f, 0f, 0f, 0f);

                    InventoryManager.Instance.ClearSelectedInventoryItem(InventoryLocation.player);
                }
            }
        }
    }
    // 设置物品槽高亮
    public void SetHighlightedInventorySlots() {

        if (inventorySlot.Length > 0) {

            for (int i = 0; i < inventorySlot.Length; i++) {

                SetHighlightedInventorySlots(i);
            }
        }
    }
    // 将给定位置的物品槽设置为高亮
    public void SetHighlightedInventorySlots(int itemPosition) {

        if (inventorySlot.Length > 0 && inventorySlot[itemPosition].itemDetails != null) {

            if (inventorySlot[itemPosition].isSelected) {

                inventorySlot[itemPosition].inventorySlotHighlight.color = new Color(1f, 1f, 1f, 1f);
                // 更新库存管理器，使物品被选中
                InventoryManager.Instance.SetSelectedInventoryItem(InventoryLocation.player, inventorySlot[itemPosition].itemDetails.ItemCode);
            }
        }
    }
}
