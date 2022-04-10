using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * 图片。
 * @time 2022-4-10
 * @author 海中垂钓。
 */
public class Image : MonoBehaviour
{
    //倒计时文本。
    private static string[] text1 = new string[] { "3", "2", "1", "GO!" };

    //结果文本。
    private static string[] text2 = new string[] { "Win!", "Game\nOver" };

    //文本组件。
    private Text content;

    //倒计时文本索引。
    private int count;

    //开始记录的时间。
    private float previousTime;

    //是否加载。
    private bool isLoad;

    //创造对象时调用。
    private void Start()
    {
        count = 0;
        content = GetComponent<Text>();
        content.fontSize = 50;
        content.text = text1[count];
        isLoad = false;
        previousTime = Time.time;
    }

    //启动更新。
    private void FixedUpdate()
    {
        if(Time.time-previousTime>1f&&!isLoad)
        {
            content.text=text1[++count];
            previousTime = Time.time;
            if(count==3)
            {
                isLoad = true;
                GameListener.isStart = true;
            }
        }

        if(GlobalEnvironment.isWin||GlobalEnvironment.isOver)
        {
            content.fontSize = 40;
            if(GlobalEnvironment.isWin)
            {
                content.text = text2[0];
                GlobalEnvironment.isWin = false;
            }
            else
            {
                content.text = text2[1];
                GlobalEnvironment.isOver = false;
            }
            GameListener.isEnd = true;
            previousTime = Time.time;
        }

        if(GameListener.isEnd&&Time.time-previousTime>15f)
        {
            Application.Quit();
        }
    }
}
