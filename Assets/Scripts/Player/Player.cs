using UnityEngine;

public class Player : SingletonMonoBehaviour<Player>
{
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

    private Rigidbody2D rigidBody2D;

    private Direction playerDirection;

    private float movementSpeed;

    private bool _playerInputIsDisabled = false;

    public bool PlayerInputIsDisabled { get => _playerInputIsDisabled; set => _playerInputIsDisabled = value; }

    protected override void Awake()
    {
        base.Awake();

        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        #region Player Input

        ResetAnimationTriggers();

        PlayerMovementInput();

        PlayerWalkInput();

        // Send event to any listeners for player movement input
        EventHandler.CallMovementEvent(xInput, yInput, isWalking, 
                            isRunning, isIdle, isCarrying, 
                            toolEffect, 
                            isUsingToolLeft, isUsingToolRight, isUsingToolUp, isUsingToolDown, 
                            isLiftingToolLeft, isLiftingToolRight, isLiftingToolUp, isLiftingToolDown, 
                            isPickingLeft, isPickingRight, isPickingUp, isPickingDown, 
                            isSwingingToolLeft, isSwingingToolRight, isSwingingToolUp, isSwingingToolDown, 
                            false, false, false, false);

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
}
