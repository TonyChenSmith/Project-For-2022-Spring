using UnityEngine;

/**
 * 游戏监听器。
 * @time 2022-4-10
 * @author 海中垂钓
 */
public class GameListener : MonoBehaviour
{
    //倒计数。
    private int count;

    //开始状态。
    internal static bool isStart = false;

    //开始状态。
    internal static bool isEnd = false;

    //是否开始音乐。
    private bool isAudio;

    //创造对象时调用。
    private void Start()
    {
        isAudio = false;
        count = 300;
        GlobalEnvironment.startGame();
        GetComponent<AudioSource>().Stop();
    }

    //被销毁时调用。
    private void OnDestroy()
    {
        GlobalEnvironment.endGame();
    }

    //更新超级豆。
    private void Update()
    {
        if(!isStart)
        {
            return;
        }
        else
        {
            if(!isAudio)
            {
                GetComponent<AudioSource>().Play();
                isAudio = true;
            }
        }
        if(isEnd)
        {
            GetComponent<AudioSource>().Stop();
            isStart = false;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GlobalEnvironment.isOver = true;
            return;
        }

        if(GlobalEnvironment.PACDOT_LIST.Count<=0)
        {
            if(GlobalEnvironment.SCORE>0)
            {
                GlobalEnvironment.isWin = true;
            }
            else
            {
                GlobalEnvironment.isOver = true;
            }
            return;
        }

        if(count<=0)
        {
            count = 300;
            if(GlobalEnvironment.PACDOT_LIST.Count>0)
            {
                int index = GlobalEnvironment.RAND.Next(0, GlobalEnvironment.PACDOT_LIST.Count - 1);
                Pacdot superDot = GlobalEnvironment.PACDOT_MAP[GlobalEnvironment.PACDOT_LIST[index]];
                if(!superDot.isSuperPacdot)
                {
                    superDot.transform.localScale = new Vector3(2, 2, 1);
                    superDot.isSuperPacdot = true;
                }
            }
        }
        else
        {
            count--;
        }
    }
}
