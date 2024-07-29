public delegate void MovementDelegate(float inputX, float inputY, bool isWalking, 
                                        bool isRunning, bool isIdle, bool isCarrying, 
                                        ToolEffect toolEffect, 
                                        bool isUsingToolLeft, bool isUsingToolRight, bool isUsingToolUp, bool isUsingToolDown, 
                                        bool isLiftingToolLeft, bool isLiftingToolRight, bool isLiftingToolUp, bool isLiftingToolDown, 
                                        bool isPickingLeft, bool isPickingRight, bool isPickingUp, bool isPickingDown, 
                                        bool isSwingingToolLeft, bool isSwingingToolRight, bool isSwingingToolUp, bool isSwingingToolDown, 
                                        bool idleLeft, bool idleRight, bool idleUp, bool idleDown);

public static class EventHandler
{
    // Movement Event
    public static event MovementDelegate MovementEvent;

    // Movement Event Call For Publishers
    public static void CallMovementEvent(float inputX, float inputY, bool isWalking, 
                                        bool isRunning, bool isIdle, bool isCarrying, 
                                        ToolEffect toolEffect, 
                                        bool isUsingToolLeft, bool isUsingToolRight, bool isUsingToolUp, bool isUsingToolDown, 
                                        bool isLiftingToolLeft, bool isLiftingToolRight, bool isLiftingToolUp, bool isLiftingToolDown, 
                                        bool isPickingLeft, bool isPickingRight, bool isPickingUp, bool isPickingDown, 
                                        bool isSwingingToolLeft, bool isSwingingToolRight, bool isSwingingToolUp, bool isSwingingToolDown, 
                                        bool idleLeft, bool idleRight, bool idleUp, bool idleDown)
    {
        if(MovementEvent != null)
            MovementEvent(inputX, inputY, isWalking, 
                            isRunning, isIdle, isCarrying, 
                            toolEffect, 
                            isUsingToolLeft, isUsingToolRight, isUsingToolUp, isUsingToolDown, 
                            isLiftingToolLeft, isLiftingToolRight, isLiftingToolUp, isLiftingToolDown, 
                            isPickingLeft, isPickingRight, isPickingUp, isPickingDown, 
                            isSwingingToolLeft, isSwingingToolRight, isSwingingToolUp, isSwingingToolDown, 
                            idleLeft, idleRight, idleUp, idleDown);
                                    
    }
}
