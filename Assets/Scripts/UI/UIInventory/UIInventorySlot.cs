using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

// 继承IBeginDragHandler, IDragHandler, IEndDragHandler接口类，用于当拖动事件发生时，能够实现功能；需使用UnityEngine.EventSystems命名空间
// 使用IPointerEnterHandler, IPointerExitHandler接口，当鼠标点到物品的时候，触发回调函数
public class UIInventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Camera mainCamera;      // 获取主摄像机，用于将获取的屏幕输入位置转换到3维世界空间的坐标
    private Canvas parentCanvas;    // 获取物品槽的父级的Canvas组件
    private Transform parentItem;   // 用于保存Items的父对象的变换组件
    private GameObject draggedItem; // 当武平被拖出UI物品栏时，生成新的拖出物品

    public Image inventorySlotHighlight; // 公开字段，用于设置物品槽高亮显示的图片
    public Image inventorySlotImage;     // 该字段用于设置存放在物品槽物品的图像
    public TextMeshProUGUI textMeshProUGUI;  // 此处用于设置物品槽显示文字的字体

    [HideInInspector] public ItemDetails itemDetails;    // 物品槽存放物品的详细信息，在Unity中的Inspector中隐藏
    [HideInInspector] public int itemQuantity;           // 物品槽存放该物品的数量，在Unity中的Inspector中隐藏

    [SerializeField] private UIInventoryBar inventoryBar = null;  // 用于设置物品栏，即物品槽的父级，因为添加了SerializeField参数，所以在unity中的Inspector中可见
    [SerializeField] private GameObject itemPrefab = null;       // 用于设置实例化物品时，使用的物品预制件，因为添加了SerializeField参数，所以在unity中的Inspector中可见
    [SerializeField] private int slotNumber = 0;                // 记录当前槽位是第几个槽位，因为添加了SerializeField参数，所以在unity中的Inspector中可见
    [SerializeField] private GameObject inventoryTextBoxPrefab = null; // 用来保存文字框预制件，因为添加了SerializeField参数，所以在unity中的Inspector中可见

    private void Awake() {
        // 用来获取parentCanvas，即父级MainGameUICanvas的Canvas组件
        parentCanvas = GetComponentInParent<Canvas>();
    }
    // 此函数在脚本刚开始运行时运行一次，且运行在Update函数前，即一帧更新之前
    private void Start() {
        // 初始化，获取主摄像机
        mainCamera = Camera.main;
        // 用Tag标签获取游戏中Items的父对象
        parentItem = GameObject.FindGameObjectWithTag(Tags.ItemsParentTransform).transform;
    }
    // 在画面上鼠标位置放置所选的物品
    private void DropSelectedItemAtMousePosition() {
        // 此处用于判断鼠标选择的物体是否存在，若是空物品槽，则不能放置
        if (itemDetails != null) {
            // 屏幕空间到世界空间的转换，参数是输入的鼠标的x，y坐标，及主摄像机的z坐标的负值，输出的是屏幕空间的点在3维空间中的位置
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
            // 实例化要放置的物品，将其放置在获取的3维世界的位置，第一个参数是要实例化的物品，此处使用预制件，第二个参数指定位置，
            // 第三个参数指定旋转，Quaternion.identity代表无旋转， 第四个参数指定其父级
            GameObject itemGameObject = Instantiate(itemPrefab, worldPosition, Quaternion.identity, parentItem);
            // 获取物体的Item组件，即自己写的Item脚本
            Item item = itemGameObject.GetComponent<Item>();
            // 将item组件的ItemCode参数设置为放置物品的ItemCode
            item.ItemCode = itemDetails.ItemCode;
            // 物品管理器移除player的物品栏中的物品，因为此物品已被拖出外面
            InventoryManager.Instance.RemoveItem(InventoryLocation.player, item.ItemCode);
        }
    }
    // 此方法在类继承IBeginDragHandler后的回调函数，当unity输入系统检测到拖动开始前调用
    public void OnBeginDrag(PointerEventData eventData) {
        // 此处用于判断鼠标选择的物体是否存在，若是空物品槽，则不能拖动
        if (itemDetails != null) {
            // 拖动开始时，禁止角色移动且重置角色动作，在物品拖动过程中，角色无法移动
            Player.Instance.DisablePlayerInputAndResetMovement();
            // 实列化拖出物品，生成新的物品，第一个参数指定要copy的对象，此处设置为inventroyBar的公开字段inventoryBarDraggedItem，可在unity中设置，
            // 第二个参数指定要赋给生成对象的父对象
            draggedItem = Instantiate(inventoryBar.inventoryBarDraggedItem, inventoryBar.transform);
            // 获取draggedItem的子对象的组件，设置为要拖出生成物品的图像
            Image draggedItemImage = draggedItem.GetComponentInChildren<Image>();
            draggedItemImage.sprite = inventorySlotImage.sprite;
        }
    }
    // 此方法在类继承IDragHandler后的回调函数，当unity输入系统检测到拖动过程中调用
    public void OnDrag(PointerEventData eventData) {

        if (draggedItem != null) {
            // 拖动物体存在时，将根据鼠标的位置改变该物体的位置，实现拖动物体跟随鼠标运动
            draggedItem.transform.position = Input.mousePosition;
        }
    }
    // 此方法在类继承IEndDragHandler后的回调函数，当unity输入系统检测到拖动结束后调用
    public void OnEndDrag(PointerEventData eventData) {

        if (draggedItem != null) {
            // 拖动完成后，删除拖动过程中生成的物体，因为接下来会将其放到地图中
            Destroy(draggedItem);
            // 当拖动结束时，判断鼠标指向的位置是否有游戏中物体，且该物体存在UIInventorySlot组件，即指向的物体是物品栏UI中的物品槽，此处用于交换物品栏中物品的位置
            if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.GetComponent<UIInventorySlot>() != null) {
                // 获取当前鼠标指向的槽位
                int toSlotNumber = eventData.pointerCurrentRaycast.gameObject.GetComponent<UIInventorySlot>().slotNumber;
                // 调用InventoryManager中的函数交换当前槽位的物品到鼠标指向的位置
                InventoryManager.Instance.SwapInventoryItems(InventoryLocation.player, slotNumber, toSlotNumber);                
                // 删除弹出的物品信息文本框，因为在拖动物品的时候，会生成文本框，在释放物品后，需要删除
                DestroyInventoryTextBox();
            }
            else {
                // 若鼠标指向的位置没有物体，且拖动的物品可以放下，则在鼠标指向的位置放置物体
                if (itemDetails.canBeDropped) {

                    DropSelectedItemAtMousePosition();
                }
            }
            // 激活角色，可以接受输入
            Player.Instance.EnablePlayerInput();
        }
    }
    // 当鼠标进入UI物品栏范围时触发的函数，需继承IPointerEnterHandler接口
    public void OnPointerEnter(PointerEventData eventData) {
        // 当所指向的物品的数量大于零时执行，若物品为0，则当前物品栏没有物品
        if (itemQuantity != 0) {
            // 在inventoryBar下生成文本框的游戏物体，使用的模板是外部设置的inventoryTextBoxPrefab，位置是当前物品槽的位置，即在所指物品栏的位置生成文本框，旋转设置为单位旋转，即不旋转
            inventoryBar.inventoryTextBoxGameObject = Instantiate(inventoryTextBoxPrefab, transform.position, Quaternion.identity);
            // 将文本框物体的父级设置为之前获取的存在Canvas组件的游戏物体
            inventoryBar.inventoryTextBoxGameObject.transform.SetParent(parentCanvas.transform, false);
            // 获取该文本框物体的UIInventoryTextBox组件，即我们写的脚本
            UIInventoryTextBox inventoryTextBox = inventoryBar.inventoryTextBoxGameObject.GetComponent<UIInventoryTextBox>();
            // 从物品管理器获取当前物品的类型描述字符串
            string itemTypeDescription = InventoryManager.Instance.GetItemTypeDescription(itemDetails.ItemType);
            // 使用物品信息设置文本框的文字部分
            inventoryTextBox.SetTextboxText(itemDetails.itemDescription, itemTypeDescription, "", itemDetails.itemLongDescription, "", "");
            // 判断物品栏是在底部和底部，因为文本框要根据物品栏的位置改变显示的位置
            if (inventoryBar.IsInventoryBarPositionBottom) {
                // 物品栏在底部时，设置文本框的锚点，并设置其位置
                inventoryBar.inventoryTextBoxGameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);
                inventoryBar.inventoryTextBoxGameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 50f, transform.position.z);
            }
            else {
                // 物品栏在顶部时，设置文本框的锚点，并设置其位置
                inventoryBar.inventoryTextBoxGameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);
                inventoryBar.inventoryTextBoxGameObject.transform.position = new Vector3(transform.position.x, transform.position.y - 50f, transform.position.z);
            }
        }
    }
    // 当鼠标离开UI物品栏时调用
    public void OnPointerExit(PointerEventData eventData) {
        // 删除生成的文本框
        DestroyInventoryTextBox();
    }
    // 删除物品文本框的函数
    public void DestroyInventoryTextBox() {

        if (inventoryBar.inventoryTextBoxGameObject != null) {

            Destroy(inventoryBar.inventoryTextBoxGameObject);
        }
    }
}
