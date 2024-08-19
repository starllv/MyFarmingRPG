using UnityEngine;
using UnityEngine.UI;

public class GridCursor : MonoBehaviour
{
    private Canvas canvas;
    private Grid grid;
    private Camera mainCamera;
    [SerializeField] private Image cursorImage = null;
    [SerializeField] private RectTransform cursorRectTransform = null;
    [SerializeField] private Sprite greenCursorSprite = null;
    [SerializeField] private Sprite redCursorSprite = null;

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

    private void SceneLoaded() {
        
        grid = GameObject.FindObjectOfType<Grid>();
    }

    private void OnEnable() {

        EventHandler.AfterSceneLoadEvent += SceneLoaded;
    }

    private void Start() {

        mainCamera = Camera.main;
        canvas = GetComponentInParent<Canvas>();
    }

    private void Update() {

        if (CursorIsEnabled) {

            DisplayCursor();
        }
    }

    private Vector3Int DisplayCursor() {

        if (grid != null) {

            Vector3Int gridPosition = GetGridPositionForCursor();

            Vector3Int playerGridPosition = GetGridPositionForPlayer();

            SetCursorValidity(gridPosition, playerGridPosition);

            cursorRectTransform.position = GetRectTransformPositionForCursor(gridPosition);

            return gridPosition;
        }
        else {

            return Vector3Int.zero;
        }
    }

    private void SetCursorValidity(Vector3Int cursorGridPosition, Vector3Int playerGridPosition) {

        SetCursorToValid();

        if (Mathf.Abs(cursorGridPosition.x - playerGridPosition.x) > ItemUseGridRadius ||
                Mathf.Abs(cursorGridPosition.y - playerGridPosition.y) > ItemUseGridRadius) {

            SetCursorToInvalid();
            return;
        }

        ItemDetails itemDetails = InventoryManager.Instance.GetSelectedInventoryItemDetails(InventoryLocation.player);

        if (itemDetails == null) {

            SetCursorToInvalid();
            return;
        }

        GridPropertyDetails gridPropertyDetails = GridPropertiesManager.Instance.GetGridPropertyDetails(cursorGridPosition.x, cursorGridPosition.y);

        if (gridPropertyDetails != null) {

            switch (itemDetails.ItemType) {

                case ItemType.Seed:
                    if (!IsCursorValidForSeed(gridPropertyDetails)) {

                        SetCursorToInvalid();
                        return;
                    }
                    break;
                case ItemType.Commodity:
                    if (!IsCursorValidForCommodity(gridPropertyDetails)) {

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

        return RectTransformUtility.PixelAdjustPoint(gridScreenPosition, cursorRectTransform, canvas);
    }

    private Vector3Int GetGridPositionForPlayer() {

        return grid.WorldToCell(Player.Instance.transform.position);
    }

    private Vector3Int GetGridPositionForCursor() {

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));

        return grid.WorldToCell(worldPosition);
    }
}
