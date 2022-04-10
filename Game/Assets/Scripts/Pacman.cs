using UnityEngine;

/**
 * 吃豆人类。
 * @time 2022-4-10
 * @author 海中垂钓
 */
public class Pacman : MonoBehaviour
{
    //是否是超级吃豆人。
    internal static bool IS_SUPER_PACMAN;

    //X轴速度占比。
    private float xSpeed;

    //Y轴速度占比。
    private float ySpeed;

    //刚体组件。
    private Rigidbody2D body;

    //动画组件。
    private Animator anim;

    //游戏对象创建时调用。
    private void Start()
    {
        IS_SUPER_PACMAN  = false;
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    //游戏刷新时调用。
    private void FixedUpdate()
    {
        if (!GameListener.isStart)
        {
            return;
        }
        GlobalEnvironment.PACMAN_POSITION = body.position;
        float currentTime = Time.time;
        if(currentTime-GlobalEnvironment.START_TIME<=GlobalEnvironment.STAND_TIME)
        {
            body.velocity = Vector2.zero;
            return;
        }
        else
        {
            Ghost.isSuperGhost = false;
            GlobalEnvironment.STAND_TIME = 0f;
        }

        xSpeed = Input.GetAxis("Horizontal")*GlobalEnvironment.PACMAN_VELOCITY;
        ySpeed = Input.GetAxis("Vertical")*GlobalEnvironment.PACMAN_VELOCITY;
        //Debug.Log("X:" + xSpeed + ",Y:" + ySpeed+",C:"+GlobalEnvironment.PACDOT_COUNT+",S:"+GlobalEnvironment.SCORE+",C:"+GlobalEnvironment.PACDOT_LIST.Count);

        body.velocity = new Vector2(xSpeed, ySpeed);
        
        if(Mathf.Abs(xSpeed)>Mathf.Abs(ySpeed)&&Mathf.Abs(xSpeed)>0.1f)
        {
            anim.SetFloat("DirectX", xSpeed);
            anim.SetFloat("DirectY", 0.0f);
        }
        else if(Mathf.Abs(ySpeed)>Mathf.Abs(xSpeed)&&Mathf.Abs(ySpeed)>0.1f)
        {
            anim.SetFloat("DirectX", 0.0f);
            anim.SetFloat("DirectY", ySpeed);
        }
        else
        {
            anim.SetFloat("DirectX", 0.0f);
            anim.SetFloat("DirectY", 0.0f);
        }
    }

    //碰撞时调用，指鬼与吃豆人。
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name.Equals("MazeWall"))
        {
            return;
        }
        if(Ghost.isSuperGhost)
        {
            if (GlobalEnvironment.PACDOT_LIST.Count > 50)
            {
                //回家扣分操作。
                GlobalEnvironment.SCORE = GlobalEnvironment.SCORE - 400;
                collision.gameObject.GetComponent<Rigidbody2D>().position = body.position;
                body.position = new Vector2(2, 2);
            }
            else
            {
                //游戏结束。
                GlobalEnvironment.isOver = true;
                collision.gameObject.GetComponent<Rigidbody2D>().position = body.position;
                body.position = new Vector2(2, 2);
            }
        }
        else if(GlobalEnvironment.PACDOT_LIST.Count <= 50)
        {
            //游戏结束。
            GlobalEnvironment.isOver = true;
            collision.gameObject.GetComponent<Rigidbody2D>().position = body.position;
            body.position = new Vector2(2, 2);
        }
        else
        {
            //鬼魂罚站
            body.position = collision.gameObject.GetComponent<Rigidbody2D>().position;
            GlobalEnvironment.GHOST_STAND_TIME[collision.gameObject.name] = 3f;
            GlobalEnvironment.GHOST_START_TIME[collision.gameObject.name] = Time.time;
        }
    }
}
