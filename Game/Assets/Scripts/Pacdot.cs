using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 海中垂钓重构的吃豆人豆子类。
 * 对豆子的碰撞判定。
 * @time 2022-4-10
 * @author 海中垂钓
 */
public class Pacdot : MonoBehaviour
{
    //是否是超级豆。
    internal bool isSuperPacdot = false;

    //对象创造时调用。
    private void Start()
    {
        string name = gameObject.name;
        GlobalEnvironment.PACDOT_MAP.Add(name, this);
        GlobalEnvironment.PACDOT_LIST.Add(name);
    }

    //碰撞判定时调用。
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject aim = collision.gameObject;
        if(aim.name.Equals("Pacman"))
        {
            if(this.isSuperPacdot)
            {
                Pacman.IS_SUPER_PACMAN = true;
            }
            GlobalEnvironment.SCORE = GlobalEnvironment.SCORE + 100;
            GlobalEnvironment.PACDOT_COUNT++;
            Destroy(gameObject);
        }
        else if(aim.name.Equals("Blinky") || aim.name.Equals("Clyde") || aim.name.Equals("Inky") || aim.name.Equals("Pinky"))
        {
            if(this.isSuperPacdot)
            {
                Ghost.isSuperGhost = true;
                GlobalEnvironment.STAND_TIME = 7f;
                GlobalEnvironment.START_TIME = Time.time;
            }
            GlobalEnvironment.SCORE = GlobalEnvironment.SCORE - 50;
            Destroy(gameObject);
        }
    }

    //在游戏对象销毁时调用。
    private void OnDestroy()
    {
        GlobalEnvironment.PACDOT_MAP.Remove(name);
        GlobalEnvironment.PACDOT_LIST.Remove(name);
    }
}
