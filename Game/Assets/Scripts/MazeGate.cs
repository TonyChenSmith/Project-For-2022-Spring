using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 迷宫大门触发器。
 * @time 2022-4-10
 * @author 海中垂钓
 */
public class MazeGate : MonoBehaviour
{
    //触发器响应。
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject aim = collision.gameObject;
        Rigidbody2D body = aim.GetComponent<Rigidbody2D>();
        if (aim.name.Equals("Blinky")|| aim.name.Equals("Clyde")|| aim.name.Equals("Inky")|| aim.name.Equals("Pinky"))
        {
            body.position = new Vector2(body.position.x, 20);
            GlobalEnvironment.GHOST_GATE[aim.name] = true;
        }
    }
}
