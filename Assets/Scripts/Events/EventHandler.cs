using System;
using System.Collections.Generic;

public delegate void MovementDelegate(float inputX, float inputY, bool isWalking, 
                                        bool isRunning, bool isIdle, bool isCarrying, 
                                        ToolEffect toolEffect, 
                                        bool isUsingToolLeft, bool isUsingToolRight, bool isUsingToolUp, bool isUsingToolDown, 
                                        bool isLiftingToolLeft, bool isLiftingToolRight, bool isLiftingToolUp, bool isLiftingToolDown, 
                                        bool isPickingLeft, bool isPickingRight, bool isPickingUp, bool isPickingDown, 
                                        bool isSwingingToolLeft, bool isSwingingToolRight, bool isSwingingToolUp, bool isSwingingToolDown, 
                                        bool idleLeft, bool idleRight, bool idleUp, bool idleDown);
// 事件管理器
public static class EventHandler
{
    // 仓库更新事件
    public static event Action<InventoryLocation, List<InventoryItem>> InventoryUpdatedEvent;
    public static void CallInventoryUpdatedEvent(InventoryLocation inventoryLocation, List<InventoryItem> inventoryList) {
        if (InventoryUpdatedEvent != null)
            InventoryUpdatedEvent(inventoryLocation, inventoryList);
    }

    // 角色移动事件
    public static event MovementDelegate MovementEvent;
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

    // 时间事件
    // 分钟事件
    public static event Action<int, Season, int, string, int, int, int> AdvanceGameMinuteEvent;
    public static void CallAdvanceGameMinuteEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, 
        int gameHour, int gameMinute, int gameSecond) {

            if (AdvanceGameMinuteEvent != null) {

                AdvanceGameMinuteEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
            }
    }
    // 小时事件
    public static event Action<int, Season, int, string, int, int, int> AdvanceGameHourEvent;
    public static void CallAdvanceGameHourEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, 
        int gameHour, int gameMinute, int gameSecond) {

            if (AdvanceGameHourEvent != null) {

                AdvanceGameHourEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
            }
    }
    // 天事件
    public static event Action<int, Season, int, string, int, int, int> AdvanceGameDayEvent;
    public static void CallAdvanceGameDayEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, 
        int gameHour, int gameMinute, int gameSecond) {

            if (AdvanceGameDayEvent != null) {

                AdvanceGameDayEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
            }
    }
    // 季节事件
    public static event Action<int, Season, int, string, int, int, int> AdvanceGameSeasonEvent;
    public static void CallAdvanceGameSeasonEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, 
        int gameHour, int gameMinute, int gameSecond) {

            if (AdvanceGameSeasonEvent != null) {

                AdvanceGameSeasonEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
            }
    }
    // 年事件
    public static event Action<int, Season, int, string, int, int, int> AdvanceGameYearEvent;
    public static void CallAdvanceGameYearEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, 
        int gameHour, int gameMinute, int gameSecond) {

            if (AdvanceGameYearEvent != null) {

                AdvanceGameYearEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
            }
    }


}
