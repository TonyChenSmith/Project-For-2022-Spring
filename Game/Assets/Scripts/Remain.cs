using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * 剩余板。
 * @time 2022-4-10
 * @author 海中垂钓
 */
public class Remain : MonoBehaviour
{
    //文字组件。
    private Text content;

    //开始界面。
    private void Start()
    {
        content = GetComponent<Text>();
    }

    //更新分数。
    private void Update()
    {
        if (!GameListener.isStart)
        {
            return;
        }
        content.text = "Remain:" + GlobalEnvironment.PACDOT_LIST.Count;
    }
}
