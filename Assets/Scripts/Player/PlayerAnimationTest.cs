using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTest : MonoBehaviour
{
    public float xInput;
    public float yInput;
    public bool isWalking;
    public bool isRunning;
    public bool isIdle;
    public bool isCarrying;
    public ToolEffect toolEffect;
    public bool isUsingToolLeft;
    public bool isUsingToolRight;
    public bool isUsingToolUp;
    public bool isUsingToolDown;
    public bool isLiftingToolLeft;
    public bool isLiftingToolRight;
    public bool isLiftingToolUp;
    public bool isLiftingToolDown;
    public bool isSwingingToolLeft;
    public bool isSwingingToolRight;
    public bool isSwingingToolUp;
    public bool isSwingingToolDown;
    public bool isPickingLeft;
    public bool isPickingRight;
    public bool isPickingUp;
    public bool isPickingDown;
    public bool idleLeft;
    public bool idleRight;
    public bool idleUp;
    public bool idleDown;

    private void Update()
    {
        EventHandler.CallMovementEvent(xInput, yInput, isWalking, 
                            isRunning, isIdle, isCarrying, 
                            toolEffect, 
                            isUsingToolLeft, isUsingToolRight, isUsingToolUp, isUsingToolDown, 
                            isLiftingToolLeft, isLiftingToolRight, isLiftingToolUp, isLiftingToolDown, 
                            isPickingLeft, isPickingRight, isPickingUp, isPickingDown, 
                            isSwingingToolLeft, isSwingingToolRight, isSwingingToolUp, isSwingingToolDown, 
                            idleLeft, idleRight, idleUp, idleDown);
    }
}
