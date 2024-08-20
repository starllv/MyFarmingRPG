using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonMonoBehaviour<Player>
{
    private WaitForSeconds useToolAnimationPause;
    private WaitForSeconds afterUseToolAnimationPause;
    private AnimationOverrides animationOverrides;
    private GridCursor gridCursor;

    // Movement Parameters
    private float xInput;
    private float yInput;
    private bool isWalking;
    private bool isRunning;
    private bool isIdle;
    private bool isCarrying = false;
    private ToolEffect toolEffect = ToolEffect.none;
    private bool isUsingToolLeft;
    private bool isUsingToolRight;
    private bool isUsingToolUp;
    private bool isUsingToolDown;
    private bool isLiftingToolLeft;
    private bool isLiftingToolRight;
    private bool isLiftingToolUp;
    private bool isLiftingToolDown;
    private bool isSwingingToolLeft;
    private bool isSwingingToolRight;
    private bool isSwingingToolUp;
    private bool isSwingingToolDown;
    private bool isPickingLeft;
    private bool isPickingRight;
    private bool isPickingUp;
    private bool isPickingDown;
    private bool idleLeft;
    private bool idleRight;
    private bool idleUp;
    private bool idleDown;
    private Camera mainCamera;
    private bool playerToolUseDisabled = false;

    private List<CharacterAttribute> characterAttributeCustomisationList;

    [Tooltip("Should be populated in the prefab with the equipped item sprite renderer")]
    [SerializeField] private SpriteRenderer equippedItemSpriteRenderer = null;

    private CharacterAttribute armsCharacterAttribute;
    private CharacterAttribute toolCharacterAttribute;


    private Rigidbody2D rigidBody2D;
    private Direction playerDirection;
    private float movementSpeed;
    private bool _playerInputIsDisabled = false;

    public bool PlayerInputIsDisabled { get => _playerInputIsDisabled; set => _playerInputIsDisabled = value; }

    protected override void Awake()
    {
        base.Awake();

        rigidBody2D = GetComponent<Rigidbody2D>();

        animationOverrides = GetComponentInChildren<AnimationOverrides>();

        armsCharacterAttribute = new CharacterAttribute(CharacterPartAnimator.arms, PartVariantColour.none, PartVariantType.none);

        characterAttributeCustomisationList = new List<CharacterAttribute>();

        mainCamera = Camera.main;
    }

    private void Start() {

        gridCursor = FindObjectOfType<GridCursor>();
        useToolAnimationPause = new WaitForSeconds(Settings.useToolAnimationPause);
        afterUseToolAnimationPause = new WaitForSeconds(Settings.afterUseToolAnimationPause);
    }

    private void Update()
    {
        #region Player Input

        if (!PlayerInputIsDisabled) {

            ResetAnimationTriggers();

            PlayerMovementInput();

            PlayerWalkInput();
            
            PlayerClickInput();

            PlayerTestInput();


            // Send event to any listeners for player movement input
            EventHandler.CallMovementEvent(xInput, yInput, isWalking, 
                                isRunning, isIdle, isCarrying, 
                                toolEffect, 
                                isUsingToolLeft, isUsingToolRight, isUsingToolUp, isUsingToolDown, 
                                isLiftingToolLeft, isLiftingToolRight, isLiftingToolUp, isLiftingToolDown, 
                                isPickingLeft, isPickingRight, isPickingUp, isPickingDown, 
                                isSwingingToolLeft, isSwingingToolRight, isSwingingToolUp, isSwingingToolDown, 
                                false, false, false, false);
        }

        #endregion
    }

    private void FixedUpdate() 
    {
        PlayerMovement();    
    }

    private void ResetAnimationTriggers()
    {
         ToolEffect toolEffect = ToolEffect.none;
         isUsingToolLeft = false;
         isUsingToolRight = false;
         isUsingToolUp = false;
         isUsingToolDown = false;
         isLiftingToolLeft = false;
         isLiftingToolRight = false;
         isLiftingToolUp = false;
         isLiftingToolDown = false;
         isSwingingToolLeft = false;
         isSwingingToolRight = false;
         isSwingingToolUp = false;
         isSwingingToolDown = false;
         isPickingLeft = false;
         isPickingRight = false;
         isPickingUp = false;
         isPickingDown = false;
    }

    private void PlayerMovementInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        if (xInput != 0 && yInput != 0)
        {
            xInput = xInput * 0.71f;
            yInput = yInput * 0.71f;
        }

        if (xInput != 0 || yInput != 0)
        {
            isRunning = true;
            isWalking = false;
            isIdle = false;
            movementSpeed = Settings.runningSpeed;

            if (xInput < 0)
            {
                playerDirection = Direction.left;
            }
            else if (xInput > 0)
            {
                playerDirection = Direction.right;
            }
            else if (yInput < 0)
            {
                playerDirection = Direction.down;
            }
            else
            {
                playerDirection = Direction.up;
            }
        }
        else if (xInput == 0 && yInput == 0)
        {
            isRunning = false;
            isWalking = false;
            isIdle = true;

        }
    }

    private void PlayerWalkInput()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            isRunning = false;
            isWalking = true;
            isIdle = false;
            movementSpeed = Settings.walkingSpeed;
        }
        else
        {
            isRunning = true;
            isWalking = false;
            isIdle = false;
            movementSpeed = Settings.runningSpeed;
        }
    }

    private void PlayerMovement()
    {
        Vector2 move = new Vector2(xInput * movementSpeed * Time.deltaTime, 
                                    yInput * movementSpeed * Time.deltaTime);
        rigidBody2D.MovePosition(rigidBody2D.position + move);
    }

    private void ResetMovement() {

        xInput = 0f;
        yInput = 0f;
        isRunning = false;
        isWalking = false;
        isIdle = true;
    }

    public void DisablePlayerInputAndResetMovement() {

        DisablePlayerInput();
        ResetMovement();

        EventHandler.CallMovementEvent(xInput, yInput, isWalking, 
                                isRunning, isIdle, isCarrying, 
                                toolEffect, 
                                isUsingToolLeft, isUsingToolRight, isUsingToolUp, isUsingToolDown, 
                                isLiftingToolLeft, isLiftingToolRight, isLiftingToolUp, isLiftingToolDown, 
                                isPickingLeft, isPickingRight, isPickingUp, isPickingDown, 
                                isSwingingToolLeft, isSwingingToolRight, isSwingingToolUp, isSwingingToolDown, 
                                false, false, false, false);
    }

    public void DisablePlayerInput() {

        PlayerInputIsDisabled = true;
    }

    public void EnablePlayerInput() {

        PlayerInputIsDisabled = false;
    }

    public Vector3 GetPlayerViewportPosition() {

        return mainCamera.WorldToViewportPoint(transform.position);
    }

    public void ShowCarriedItem(int iteCode) {

        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(iteCode);
        if (itemDetails != null) {

            equippedItemSpriteRenderer.sprite = itemDetails.itemSprite;
            equippedItemSpriteRenderer.color = new Color(1f, 1f, 1f, 1f);

            armsCharacterAttribute.partVariantType = PartVariantType.carry;
            characterAttributeCustomisationList.Clear();
            characterAttributeCustomisationList.Add(armsCharacterAttribute);
            animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomisationList);

            isCarrying = true;
        }
    }

    public void ClearCarriedItem() {

        equippedItemSpriteRenderer.sprite = null;
        equippedItemSpriteRenderer.color = new Color(1f, 1f, 1f, 0f);

        armsCharacterAttribute.partVariantType = PartVariantType.none;
        characterAttributeCustomisationList.Clear();
        characterAttributeCustomisationList.Add(armsCharacterAttribute);
        animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomisationList);

        isCarrying = false;
    }

    private void PlayerTestInput() {

        if (Input.GetKey(KeyCode.T)) {

            TimeManager.Instance.TestAdvanceGameMinute();
        }

        if (Input.GetKeyDown(KeyCode.G)) {

            TimeManager.Instance.TestAdvanceGameDay();
        }
        // 测试场景切换功能
        if (Input.GetKeyDown(KeyCode.L)) {

            SceneControllerManager.Instance.FadeAndLoadScene(SceneName.Scene1_Farm.ToString(), transform.position);
        }
    }

    private void PlayerClickInput() {

        if (playerToolUseDisabled)
            return;

        if (Input.GetMouseButton(0)) {


            if (gridCursor.CursorIsEnabled) {

                Vector3Int cursorGridPosition = gridCursor.GetGridPositionForCursor();

                Vector3Int playerGridPosition = gridCursor.GetGridPositionForPlayer();
                
                ProcessPlayerClickInput(cursorGridPosition, playerGridPosition);
            }
        }
    }

    private void ProcessPlayerClickInput(Vector3Int cursorGridPosition, Vector3Int playerGridPosition) {
        
        ResetMovement();

        Vector3Int playerDirection = GetPlayerClickDirection(cursorGridPosition, playerGridPosition);

        GridPropertyDetails gridPropertyDetails = GridPropertiesManager.Instance.GetGridPropertyDetails(cursorGridPosition.x, cursorGridPosition.y);

        ItemDetails itemDetails = InventoryManager.Instance.GetSelectedInventoryItemDetails(InventoryLocation.player);

        if (itemDetails != null) {

            switch (itemDetails.ItemType) {

                case ItemType.Seed:
                    if (Input.GetMouseButtonDown(0)) {

                        ProcessPlayerClickInputSeed(itemDetails);
                    }
                    break;
                case ItemType.Commodity:
                    if (Input.GetMouseButtonDown(0)) {
                        ProcessPlayerClickInputCommodity(itemDetails);
                    }
                    break;
                case ItemType.Hoeing_tool:
                    ProcessPlayerClickInputTool(gridPropertyDetails, itemDetails, playerDirection);
                    break;
                case ItemType.none:
                    break;
                case ItemType.count:
                    break;
                    
                default:
                    break;
            }
        }
    }

    private Vector3Int GetPlayerClickDirection(Vector3Int cursorGridPosition, Vector3Int playerGridPosition) {

        if (cursorGridPosition.x > playerGridPosition.x) {

            return Vector3Int.right;
        }
        else if (cursorGridPosition.x < playerGridPosition.x) {
        
            return Vector3Int.left;
        }
        else if (cursorGridPosition.y > playerGridPosition.y) {

            return Vector3Int.up;
        }
        else {

            return Vector3Int.down;
        }
    }

    private void ProcessPlayerClickInputTool(GridPropertyDetails gridPropertyDetails, ItemDetails itemDetails, Vector3Int playerDirection) {

        switch (itemDetails.ItemType) {

            case ItemType.Hoeing_tool:
                if (gridCursor.CursorPositionIsValid) {

                    HoeGroundAtCursor(gridPropertyDetails, playerDirection);
                }
                break;

            default:
                break;
        }
    }

    private void HoeGroundAtCursor(GridPropertyDetails gridPropertyDetails, Vector3Int playerDirection) {

        StartCoroutine(HoeGroundAtCursorRoutine(playerDirection, gridPropertyDetails));
    }

    private IEnumerator HoeGroundAtCursorRoutine(Vector3Int playerDirection, GridPropertyDetails gridPropertyDetails) {

        PlayerInputIsDisabled = true;
        playerToolUseDisabled = true;

        toolCharacterAttribute.partVariantType = PartVariantType.hoe;
        characterAttributeCustomisationList.Clear();
        characterAttributeCustomisationList.Add(toolCharacterAttribute);
        animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeCustomisationList);

        if (playerDirection == Vector3Int.right) {

            isUsingToolRight = true;
        }
        else if (playerDirection == Vector3Int.left) {

            isUsingToolLeft = true;
        }
        else if (playerDirection == Vector3Int.up) {

            isUsingToolUp = true;
        }
        else if (playerDirection == Vector3Int.down) {

            isUsingToolDown = true;
        }

        yield return useToolAnimationPause;

        if (gridPropertyDetails.daysSinceDug == -1) {

            gridPropertyDetails.daysSinceDug = 0;
        }

        GridPropertiesManager.Instance.SetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY, gridPropertyDetails);

        yield return afterUseToolAnimationPause;

        playerToolUseDisabled = false;
        PlayerInputIsDisabled = false;
    }

    private void ProcessPlayerClickInputSeed(ItemDetails itemDetails) {

        if (itemDetails.canBeDropped && gridCursor.CursorPositionIsValid) {

            EventHandler.CallDropSelectedItemEvent();
        }
    }

    private void ProcessPlayerClickInputCommodity(ItemDetails itemDetails) {

        if (itemDetails.canBeDropped && gridCursor.CursorPositionIsValid) {

            EventHandler.CallDropSelectedItemEvent();
        }
    }
}
