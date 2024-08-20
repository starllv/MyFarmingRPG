using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridCursor : MonoBehaviour
{
    private Canvas canvas;             // 获取Cursor的父级Canvas组件
    private Grid grid;                 // 获取Grid网格对象
    private Camera mainCamera;
    [SerializeField] private Image cursorImage = null;        // 获取Cursor的Image组件
    [SerializeField] private RectTransform cursorRectTransform = null;      // 获取Cursor的RectTransform组件
    [SerializeField] private Sprite greenCursorSprite = null;               // 获取Cursor的绿色Sprite
    [SerializeField] private Sprite redCursorSprite = null;                // 获取Cursor的红色Sprite

    private bool _cursorPositionIsValid = false;
    public bool CursorPositionIsValid { get => _cursorPositionIsValid; set => _cursorPositionIsValid = value; }

    private int _itemUseGridRadius = 0;
    public int ItemUseGridRadius { get => _itemUseGridRadius; set => _itemUseGridRadius = value; }

    private ItemType _selectedItemType;
    public ItemType SelectedItemType { get => _selectedItemType; set => _selectedItemType = value; }

    private bool _cursorIsEnabled = false;
    public bool CursorIsEnabled { get => _cursorIsEnabled; set => _cursorIsEnabled = value; }

    private void OnDisable() {
        
        EventHandler.AfterSceneLoadEvent -= SceneLoaded;
    }


    private void OnEnable() {
        // 订阅场景加载完成事件，调用函数
        EventHandler.AfterSceneLoadEvent += SceneLoaded;
    }

    private void SceneLoaded() {
        // 获取场景的Grid网格对象
        grid = GameObject.FindObjectOfType<Grid>();
    }

    private void Start() {

        mainCamera = Camera.main;
        canvas = GetComponentInParent<Canvas>();
    }

    private void Update() {
        // 光标使能后，显示光标
        if (CursorIsEnabled) {

            DisplayCursor();
        }
    }

    private Vector3Int DisplayCursor() {

        if (grid != null) {
            // 获取光标在网格上的位置
            Vector3Int gridPosition = GetGridPositionForCursor();
            // 获取角色在网格上的位置
            Vector3Int playerGridPosition = GetGridPositionForPlayer();
            // 根据光标位置和角色位置，查看当前光标处是否可放置以及设置光标的颜色
            SetCursorValidity(gridPosition, playerGridPosition);
            // 将UI上的位置变换到光标在网格上的位置
            cursorRectTransform.position = GetRectTransformPositionForCursor(gridPosition);

            return gridPosition;
        }
        else {

            return Vector3Int.zero;
        }
    }

    private void SetCursorValidity(Vector3Int cursorGridPosition, Vector3Int playerGridPosition) {

        SetCursorToValid();
        // 当网格上光标和角色的距离大于设置的物品可使用的距离时，光标置为无效
        if (Mathf.Abs(cursorGridPosition.x - playerGridPosition.x) > ItemUseGridRadius ||
                Mathf.Abs(cursorGridPosition.y - playerGridPosition.y) > ItemUseGridRadius) {

            SetCursorToInvalid();
            return;
        }
        // 从玩家背包中获取物品的信息
        ItemDetails itemDetails = InventoryManager.Instance.GetSelectedInventoryItemDetails(InventoryLocation.player);

        if (itemDetails == null) {

            SetCursorToInvalid();
            return;
        }
        // 获取光标处网格的属性信息
        GridPropertyDetails gridPropertyDetails = GridPropertiesManager.Instance.GetGridPropertyDetails(cursorGridPosition.x, cursorGridPosition.y);

        if (gridPropertyDetails != null) {

            switch (itemDetails.ItemType) {

                case ItemType.Seed:
                    // 查看当前光标处是否可放置Seed
                    if (!IsCursorValidForSeed(gridPropertyDetails)) {

                        SetCursorToInvalid();
                        return;
                    }
                    break;
                case ItemType.Commodity:
                    // 查看当前光标处是否可放置Commodity
                    if (!IsCursorValidForCommodity(gridPropertyDetails)) {

                        SetCursorToInvalid();
                        return;
                    }
                    break;
                case ItemType.Hoeing_tool:
                    if (!IsCursorValidForTool(gridPropertyDetails,itemDetails)) {

                        SetCursorToInvalid();
                        return;
                    }
                    break;
                case ItemType.none:
                    break;
                case ItemType.count:
                    break;

                default:
                    break;
            }
        }
        else {

            SetCursorToInvalid();
            return;
        }

    }

    private bool IsCursorValidForTool(GridPropertyDetails gridPropertyDetails, ItemDetails itemDetails) {

        switch (itemDetails.ItemType) {

            case ItemType.Hoeing_tool:
                if (gridPropertyDetails.isDiggable && gridPropertyDetails.daysSinceDug == -1) {

                    #region 
                    Vector3 cursorWorldPosition = new Vector3(GetWorldPositionForCursor().x + 0.5f, GetWorldPositionForCursor().y + 0.5f, 0f);

                    List<Item> itemList = new List<Item>();

                    HelperMethods.GetComponentsAtBoxLocation<Item>(out itemList, cursorWorldPosition, Settings.cursorSize, 0f);
                    #endregion

                    bool foundReapable = false;

                    foreach (Item item in itemList) {

                        if(InventoryManager.Instance.GetItemDetails(item.ItemCode).ItemType == ItemType.Reapable_scenary) {

                            foundReapable = true;
                            break;
                        }
                    }

                    if (foundReapable) {

                        return false;
                    }
                    else {

                        return true;
                    }
                }
                else {

                    return false;
                }
            
            default:
                return false;
        }
    }

    private Vector3 GetWorldPositionForCursor() {

        return grid.CellToWorld(GetGridPositionForCursor());
    }

    private bool IsCursorValidForCommodity(GridPropertyDetails gridPropertyDetails) {

        return gridPropertyDetails.canDropItem;
    }

    private bool IsCursorValidForSeed(GridPropertyDetails gridPropertyDetails) {

        return gridPropertyDetails.canDropItem;
    }

    private void SetCursorToInvalid() {

        cursorImage.sprite = redCursorSprite;
        CursorPositionIsValid = false;
    }

    private void SetCursorToValid() {
        
        cursorImage.sprite = greenCursorSprite;
        CursorPositionIsValid = true;
    }

    public void DisableCursor() {
        // 光标图像设置为透明
        cursorImage.color = Color.clear;

        CursorIsEnabled = false;
    }

    public void EnableCursor() {

        cursorImage.color = new Color(1f, 1f, 1f, 1f);

        CursorIsEnabled = true;
    }

    private Vector2 GetRectTransformPositionForCursor(Vector3Int gridPosition) {

        Vector3 gridWorldPosition = grid.CellToWorld(gridPosition);
        Vector2 gridScreenPosition = mainCamera.WorldToScreenPoint(gridWorldPosition);
        // 将光标在屏幕空间的位置转换为canvas上的像素位置，cursorRectTransform是光标对象的RectTransform组件
        return RectTransformUtility.PixelAdjustPoint(gridScreenPosition, cursorRectTransform, canvas);
    }

    public Vector3Int GetGridPositionForPlayer() {

        return grid.WorldToCell(Player.Instance.transform.position);
    }

    public Vector3Int GetGridPositionForCursor() {
        // 从当前鼠标的位置获取世界坐标，并转换到Grid的格子位置
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));

        return grid.WorldToCell(worldPosition);
    }
}
