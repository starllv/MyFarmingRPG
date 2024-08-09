using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : SingletonMonoBehaviour<TimeManager>
{
    private int gameYear = 1;
    private Season gameSeason = Season.Spring;
    private int gameDay = 1;
    private int gameHour = 6;
    private int gameMinute = 30;
    private int gameSecond = 0;
    private string gameDayOfWeek = "Mon";

    private bool gameClockPaused = false;

    private float gameTick = 0f;

    private void Start() {

        EventHandler.CallAdvanceGameMinuteEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
    }

    private void Update() {
        
        if (!gameClockPaused) {

            GameTick();
        }
    }

    private void GameTick() {
        
        gameTick += Time.deltaTime;

        if (gameTick >= Settings.secondsPerGameSecond) {

            gameTick -= Settings.secondsPerGameSecond;
            UpdateGameSecond();
        }
    }

    private void UpdateGameSecond() {
        
        gameSecond++;

        if (gameSecond > 59) {

            gameSecond = 0;
            gameMinute++;

            if (gameMinute > 59) {

                gameMinute = 0;
                gameHour++;

                if (gameHour > 23) {

                    gameHour = 0;
                    gameDay++;

                    if (gameDay > 30) {

                        gameDay = 1;

                        int gs = (int)gameSeason;
                        gs++;

                        gameSeason = (Season)gs;

                        if (gs > 3) {

                            gs = 0;
                            gameSeason = (Season)gs;

                            gameYear++;

                            EventHandler.CallAdvanceGameYearEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
                        }

                        EventHandler.CallAdvanceGameSeasonEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
                    }

                    gameDayOfWeek = GetDayOfWeek();
                    EventHandler.CallAdvanceGameDayEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
                }

                EventHandler.CallAdvanceGameHourEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
            }

            EventHandler.CallAdvanceGameMinuteEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
            Debug.Log("游戏时间    年：" + gameYear + "    季节：" + gameSeason + "    日：" +
                gameDay + "    小时：" + gameHour + "    分钟：" + gameMinute);
        }
    }

    private string GetDayOfWeek() {

        int totalDays = (((int)gameSeason) * 30) + gameDay;
        int dayOfWeek = totalDays % 7;

        switch (dayOfWeek) {

            case 1:
                return "Mon";

            case 2:
                return "Tue";

            case 3:
                return "Wed";

            case 4:
                return "Thu";

            case 5:
                return "Fri";

            case 6:
                return "Sat";

            case 0:
                return "Sun";

            default:
                return "";
        }
    }
}
