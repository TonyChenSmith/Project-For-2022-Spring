using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 鬼魂的行为模式。
 * @time 2022-4-10
 * @author 海中垂钓
 */
public class Ghost : MonoBehaviour
{
    //是否是超级鬼。
    internal static bool isSuperGhost;

    //出生点。
    private Vector2 born;

    //方向。
    private Vector2 direct;

    //上一次时间。
    private float previousTime;

    //可选路方向。
    private List<Vector2> road;

    //刚体对象。
    private Rigidbody2D body;

    //动画对象。
    private Animator anim;

    //倒计时次数。
    private int count1 = -1;

    //索敌次数。
    private int count2 = -1;

    //索敌限制上限。
    private int count2Limit = 10;

    //对象创建时调用。
    private void Start()
    {
        GlobalEnvironment.GHOST_GATE.Add(name, false);
        direct = Vector2.up;
        previousTime = 0;
        road = new List<Vector2>(4);
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        isSuperGhost = false;
        born = body.position;
    }

    //每次更新时调用。
    private void FixedUpdate()
    {
        float currentTime = Time.time;
        if(!GameListener.isStart)
        {
            body.velocity = Vector2.zero;
            return;
        }
        if (currentTime - GlobalEnvironment.GHOST_START_TIME[name] <= GlobalEnvironment.GHOST_STAND_TIME[name])
        {
            body.position = born;
            return;
        }
        else
        {
            if(GlobalEnvironment.GHOST_STAND_TIME[name]!=0f)
            {
                GlobalEnvironment.GHOST_STAND_TIME[name] = 0f;
                GlobalEnvironment.GHOST_GATE[name] = false;
            }
        }

        bool isPassGate = GlobalEnvironment.GHOST_GATE[name];
        if(currentTime-previousTime>GlobalEnvironment.GHOST_TIMESTAMP)
        {
            if(!isPassGate)
            {
                direct = Vector2.up;
                turnAndWalk();
            }
            else
            {
                if(count1==-1)
                {
                    count1 = 3;
                }
                else
                {
                    count1--;
                    return;
                }
                if(count2==-1)
                {
                    count2 = count2Limit;
                    count2Limit--;
                    if(count2Limit<-1)
                    {
                        count2Limit = -1;
                    }
                }
                else
                {
                    count2--;
                }
                road.Clear();
                checkWay();
                if (road.Count == 0)
                {
                    road.Add(Vector2.right);
                    road.Add(Vector2.left);
                    road.Add(Vector2.up);
                    road.Add(Vector2.down);
                    int index = GlobalEnvironment.RAND.Next(0, 3);
                    direct = road[index];
                }
                else
                {
                    int index = GlobalEnvironment.RAND.Next(0, road.Count - 1);
                    direct = road[index];
                }
                turnAndWalk();
            }
            previousTime = currentTime;
        }
    }

    //旋转动画方向并赋值速度。
    private void turnAndWalk()
    {
        body.velocity = new Vector2(direct.x * GlobalEnvironment.GHOST_VELOCITY, direct.y * GlobalEnvironment.GHOST_VELOCITY);
        anim.SetFloat("DirectX", body.velocity.x);
        anim.SetFloat("DirectY", body.velocity.y);
    }

    //寻向函数。
    private void checkWay()
    {
        //右 左 上 下
        bool[] checkDirect = new bool[4];

        RaycastHit2D ray0 = Physics2D.Raycast(new Vector2(body.position.x + 0.8f, body.position.y + 0.8f), Vector2.right);
        RaycastHit2D ray1 = Physics2D.Raycast(new Vector2(body.position.x + 0.8f, body.position.y), Vector2.right);
        RaycastHit2D ray2 = Physics2D.Raycast(new Vector2(body.position.x + 0.8f, body.position.y - 0.8f), Vector2.right);
        if (ray1.distance>=GlobalEnvironment.GHOST_DISTANCE||ray1.collider.gameObject.name.StartsWith("Pacdot")||ray1.collider.gameObject.name.StartsWith("Pacman"))
        {
            if (ray0.distance >= GlobalEnvironment.GHOST_DISTANCE || ray0.collider.gameObject.name.StartsWith("Pacdot") || ray0.collider.gameObject.name.StartsWith("Pacman"))
            {
                if (ray2.distance >= GlobalEnvironment.GHOST_DISTANCE || ray2.collider.gameObject.name.StartsWith("Pacdot") || ray2.collider.gameObject.name.StartsWith("Pacman"))
                {
                    checkDirect[0] = true;
                }
            }
        }

        ray0 = Physics2D.Raycast(new Vector2(body.position.x - 0.8f, body.position.y + 0.8f), Vector2.left);
        ray1 = Physics2D.Raycast(new Vector2(body.position.x - 0.8f, body.position.y), Vector2.left);
        ray2 = Physics2D.Raycast(new Vector2(body.position.x - 0.8f, body.position.y - 0.8f), Vector2.left);
        if (ray1.distance >= GlobalEnvironment.GHOST_DISTANCE || ray1.collider.gameObject.name.StartsWith("Pacdot") || ray1.collider.gameObject.name.StartsWith("Pacman"))
        {
            if (ray0.distance >= GlobalEnvironment.GHOST_DISTANCE || ray0.collider.gameObject.name.StartsWith("Pacdot") || ray0.collider.gameObject.name.StartsWith("Pacman"))
            {
                if (ray2.distance >= GlobalEnvironment.GHOST_DISTANCE || ray2.collider.gameObject.name.StartsWith("Pacdot") || ray2.collider.gameObject.name.StartsWith("Pacman"))
                {
                    checkDirect[1] = true;
                }
            }
        }

        ray0 = Physics2D.Raycast(new Vector2(body.position.x + 0.8f, body.position.y + 0.8f), Vector2.up);
        ray1 = Physics2D.Raycast(new Vector2(body.position.x, body.position.y + 0.8f), Vector2.up);
        ray2 = Physics2D.Raycast(new Vector2(body.position.x - 0.8f, body.position.y + 0.8f), Vector2.up);
        if (ray1.distance >= GlobalEnvironment.GHOST_DISTANCE || ray1.collider.gameObject.name.StartsWith("Pacdot") || ray1.collider.gameObject.name.StartsWith("Pacman"))
        {
            if (ray0.distance >= GlobalEnvironment.GHOST_DISTANCE || ray0.collider.gameObject.name.StartsWith("Pacdot") || ray0.collider.gameObject.name.StartsWith("Pacman"))
            {
                if (ray2.distance >= GlobalEnvironment.GHOST_DISTANCE || ray2.collider.gameObject.name.StartsWith("Pacdot") || ray2.collider.gameObject.name.StartsWith("Pacman"))
                {
                    checkDirect[2] = true;
                }
            }
        }

        ray0 = Physics2D.Raycast(new Vector2(body.position.x + 0.8f, body.position.y - 0.8f), Vector2.down);
        ray1 = Physics2D.Raycast(new Vector2(body.position.x, body.position.y - 0.8f), Vector2.down);
        ray2 = Physics2D.Raycast(new Vector2(body.position.x - 0.8f, body.position.y - 0.8f), Vector2.down);
        if (ray1.distance >= GlobalEnvironment.GHOST_DISTANCE || ray1.collider.gameObject.name.StartsWith("Pacdot") || ray1.collider.gameObject.name.StartsWith("Pacman"))
        {
            if (ray0.distance >= GlobalEnvironment.GHOST_DISTANCE || ray0.collider.gameObject.name.StartsWith("Pacdot") || ray0.collider.gameObject.name.StartsWith("Pacman"))
            {
                if (ray2.distance >= GlobalEnvironment.GHOST_DISTANCE || ray2.collider.gameObject.name.StartsWith("Pacdot") || ray2.collider.gameObject.name.StartsWith("Pacman"))
                {
                    checkDirect[3] = true;
                }
            }
        }

        FindWay(checkDirect);
    }

    /**
     * 寻路。
     */
    private void FindWay(bool[] direct)
    {
        bool north = direct[2];
        bool south = direct[3];
        bool west = direct[1];
        bool east = direct[0];

        if (north && !south && !west && !east)
        {
            //下停。
            road.Add(Vector2.up);
        }
        else if (!north && south && !west && !east)
        {
            //上停。
            road.Add(Vector2.down);
        }
        else if (!north && !south && west && !east)
        {
            //右停。
            road.Add(Vector2.left); ;
        }
        else if (!north && !south && !west && east)
        {
            //左停。
            road.Add(Vector2.right);
        }
        else if (north && south && !west && !east)
        {
            //上下通路。
            if(body.velocity.y>0)
            {
                road.Add(Vector2.up);
            }
            else
            {
                road.Add(Vector2.down);
            }
        }
        else if (!north && !south && west && east)
        {
            //左右通路。
            if (body.velocity.x > 0)
            {
                road.Add(Vector2.right);
            }
            else
            {
                road.Add(Vector2.left);
            }
        }
        else if (north && !south && west && !east)
        {
            //左上拐角。
            if (body.velocity.x > 0)
            {
                road.Add(Vector2.up);
            }
            else
            {
                road.Add(Vector2.left);
            }
        }
        else if (north && !south && !west && east)
        {
            //右上拐角。
            if (body.velocity.x < 0)
            {
                road.Add(Vector2.up);
            }
            else
            {
                road.Add(Vector2.right);
            }
        }
        else if (!north && south && west && !east)
        {
            //左下拐角。
            if (body.velocity.x > 0)
            {
                road.Add(Vector2.down);
            }
            else
            {
                road.Add(Vector2.left);
            }
        }
        else if (!north && south && !west && east)
        {
            //右下拐角。
            if (body.velocity.x < 0)
            {
                road.Add(Vector2.down);
            }
            else
            {
                road.Add(Vector2.right);
            }
        }
        else if (north && south && west && !east)
        {
            //上下左T。
            if (body.velocity.y > 0)
            {
                //从下方来。
                chooseMinDistance(true, false, false, true);
            }
            else if (body.velocity.x > 0)
            {
                //从左方来。
                chooseMinDistance(true, false, true, false);
            }
            else
            {
                //从上方来。
                chooseMinDistance(false, false, true, true);
            }
        }
        else if (north && south && !west && east)
        {
            //上下右T。
            if (body.velocity.y > 0)
            {
                //从下方来。
                chooseMinDistance(true, true, false, false);
            }
            else if (body.velocity.x < 0)
            {
                //从右方来。
                chooseMinDistance(true, false, true, false);
            }
            else
            {
                //从上方来。
                chooseMinDistance(false, true, true, false);
            }
        }
        else if (north && !south && west && east)
        {
            //上左右T。
            if (body.velocity.y < 0)
            {
                //从上方来。
                chooseMinDistance(false, true, false, true);
            }
            else if (body.velocity.x > 0)
            {
                //从左方来。
                chooseMinDistance(true, true, false, false);
            }
            else
            {
                //从右方来。
                chooseMinDistance(true, false, false, true);
            }
        }
        else if (!north && south && west && east)
        {
            //下左右T。
            if (body.velocity.y > 0)
            {
                //从下方来。
                chooseMinDistance(false, true, false, true);
            }
            else if (body.velocity.x > 0)
            {
                //从左方来。
                chooseMinDistance(false, true, true, false);
            }
            else
            {
                //从右方来。
                chooseMinDistance(false, false, true, true);
            }
        }
        else
        {
            //全向。
            if (body.velocity.y > 0)
            {
                //向上。
                chooseMinDistance(true, true, false, true);
            }
            else if (body.velocity.x > 0)
            {
                //向右。
                chooseMinDistance(true, true, true, false);
            }
            else if (body.velocity.y < 0)
            {
                //向下。
                chooseMinDistance(false, true, true, true);
            }
            else
            {
                //向左。
                chooseMinDistance(true, false, true, true);
            }
        }
    }

    //选择最近方向。
    private void chooseMinDistance(bool north, bool east, bool south, bool west)
    {
        if (count2 == count2Limit)
        {
            //索敌模式
            float distanceX = GlobalEnvironment.PACMAN_POSITION.x - body.position.x;
            float distanceY = GlobalEnvironment.PACMAN_POSITION.y - body.position.y;
            if(Mathf.Abs(distanceX)>Mathf.Abs(distanceY))
            {
                if(distanceX>0)
                {
                    if(east)
                    {
                        road.Add(Vector2.right);
                    }
                    else if(distanceY>0)
                    {
                        if(north)
                        {
                            road.Add(Vector2.up);
                        }
                        else
                        {
                            if (west)
                            {
                                road.Add(Vector2.left);
                            }
                            if (south)
                            {
                                road.Add(Vector2.down);
                            }
                        }
                    }
                    else
                    {
                        if (south)
                        {
                            road.Add(Vector2.down);
                        }
                        else
                        {
                            if (west)
                            {
                                road.Add(Vector2.left);
                            }
                            if (north)
                            {
                                road.Add(Vector2.up);
                            }
                        }
                    }
                }
                else
                {
                    if(west)
                    {
                        road.Add(Vector2.left);
                    }
                    else if (distanceY > 0)
                    {
                        if (north)
                        {
                            road.Add(Vector2.up);
                        }
                        else
                        {
                            if (east)
                            {
                                road.Add(Vector2.right);
                            }
                            if (south)
                            {
                                road.Add(Vector2.down);
                            }
                        }
                    }
                    else
                    {
                        if (south)
                        {
                            road.Add(Vector2.down);
                        }
                        else
                        {
                            if (east)
                            {
                                road.Add(Vector2.right);
                            }
                            if (north)
                            {
                                road.Add(Vector2.up);
                            }
                        }
                    }
                }
            }
            else
            {
                if(distanceY>0)
                {
                    if(north)
                    {
                        road.Add(Vector2.up);
                    }
                    else if(distanceX>0)
                    {
                        if(east)
                        {
                            road.Add(Vector2.right);
                        }
                        else
                        {
                            if (west)
                            {
                                road.Add(Vector2.left);
                            }
                            if (south)
                            {
                                road.Add(Vector2.down);
                            }
                        }
                    }
                    else
                    {
                        if(west)
                        {
                            road.Add(Vector2.left);
                        }
                        else
                        {
                            if (east)
                            {
                                road.Add(Vector2.right);
                            }
                            if (south)
                            {
                                road.Add(Vector2.down);
                            }
                        }
                    }
                }
                else
                {
                    if(south)
                    {
                        road.Add(Vector2.down);
                    }
                    else if (distanceX > 0)
                    {
                        if (east)
                        {
                            road.Add(Vector2.right);
                        }
                        else
                        {
                            if (west)
                            {
                                road.Add(Vector2.left);
                            }
                            if (north)
                            {
                                road.Add(Vector2.up);
                            }
                        }
                    }
                    else
                    {
                        if (west)
                        {
                            road.Add(Vector2.left);
                        }
                        else
                        {
                            if (east)
                            {
                                road.Add(Vector2.right);
                            }
                            if (north)
                            {
                                road.Add(Vector2.up);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            //随机模式
            if(north)
            {
                road.Add(Vector2.up);
            }
            if(east)
            {
                road.Add(Vector2.right);
            }
            if(west)
            {
                road.Add(Vector2.left);
            }
            if(south)
            {
                road.Add(Vector2.down);
            }
        }
    }
}
