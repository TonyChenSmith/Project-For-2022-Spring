﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * 得分板。
 * @time 2022-4-10
 * @author 海中垂钓。
 */
public class Score : MonoBehaviour
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
        content.text = "Score:" + GlobalEnvironment.SCORE;
    }
}
