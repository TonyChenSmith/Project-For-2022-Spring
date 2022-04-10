using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 重修版吃豆人的全局环境类。
 * @time 2022-4-10
 * @author 海中垂钓
 */
class GlobalEnvironment
{
    //吃豆人速度。
    internal static readonly float PACMAN_VELOCITY = 3f;

    //鬼魂速度。
    internal static readonly float GHOST_VELOCITY = 3.3f;

    //鬼魂反应时间。
    internal static readonly float GHOST_TIMESTAMP = 0.1f;

    //鬼魂反应距离。
    internal static readonly float GHOST_DISTANCE = 0.5f;

    //所有豆子名称与游戏对象的映射。每次重新打开游戏界面都会重新刷新。
    internal static Dictionary<string, Pacdot> PACDOT_MAP = new Dictionary<string,Pacdot>();

    //一个记录所有豆子的游戏对象名。
    internal static List<string> PACDOT_LIST = new List<string>();

    //记录鬼魂是否过门。
    internal static Dictionary<string, bool> GHOST_GATE = new Dictionary<string, bool>();

    //随机数对象。
    internal static System.Random RAND = new System.Random();

    //吃豆人位置。
    internal static Vector2 PACMAN_POSITION = new Vector2(2,2);

    //分数。
    internal static int SCORE;

    //已经吃的豆子。
    internal static int PACDOT_COUNT;

    //罚站时间。
    internal static float STAND_TIME;

    //鬼魂罚站时间。
    internal static Dictionary<string, float> GHOST_STAND_TIME = new Dictionary<string, float>();

    //鬼魂开始罚站时间。
    internal static Dictionary<string, float> GHOST_START_TIME = new Dictionary<string, float>();

    //开始罚站时间。
    internal static float START_TIME;

    //是否赢。
    internal static bool isWin;

    //是否输。
    internal static bool isOver;

    //启动游戏时调用。
    internal static void startGame()
    {
        SCORE = 3000;
        PACDOT_COUNT = 0;
        STAND_TIME = 0;
        START_TIME = Time.time;
        GHOST_STAND_TIME.Add("Blinky", 0f);
        GHOST_STAND_TIME.Add("Clyde", 0f);
        GHOST_STAND_TIME.Add("Inky", 0f);
        GHOST_STAND_TIME.Add("Pinky", 0f);
        GHOST_START_TIME.Add("Blinky", START_TIME);
        GHOST_START_TIME.Add("Clyde", START_TIME);
        GHOST_START_TIME.Add("Inky", START_TIME);
        GHOST_START_TIME.Add("Pinky", START_TIME);
        isWin = false;
        isOver = false;
    }

    //游戏结束时调用。
    internal static void endGame()
    {
        PACDOT_LIST.Clear();
        PACDOT_MAP.Clear();
    }
}
