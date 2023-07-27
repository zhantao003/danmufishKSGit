using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRollText : UIBase
{
    [Header("起始位置")]
    public Vector3 vecStartPos;
    [Header("滚动宽度")]
    public float fRollWidth;
    [Header("滚动文本间隔")]
    public float fGapValue;
    [Header("滚动速度")]
    public Vector3 fRollSpeed;
    [Header("滚动文本")]
    public List<UIRollTextRoot> listRollTexts;
    [Header("滚动信息")]
    public List<string> listSaveInfo;
    [Header("当时使用的信息下标")]
    public int nCurUseInfoIdx;
    [Header("当前用于展示文本的下标")]
    public int nCurShowTextIdx;

    public UITweenPos tweenPos;

    public Vector3 vecShowPos;
    public Vector3 vecHidePos;


    public override void OnOpen()
    {
        base.OnOpen();
        SetRollInfo();
    }

    public void ShowInfo(float fDelay = 0f)
    {
        tweenPos.enabled = true;
        tweenPos.from = vecHidePos;
        tweenPos.to = vecShowPos;
        tweenPos.delayTime = fDelay;
        tweenPos.Play();
    }

    public void HideInfo(float fDelay = 0f)
    {
        tweenPos.enabled = true;
        tweenPos.from = vecShowPos;
        tweenPos.to = vecHidePos;
        tweenPos.delayTime = fDelay;
        tweenPos.Play();
    }

    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.S))
    //    {
    //        SetRollInfo();
    //    }
    //}

    void SetRollInfo()
    {
        listRollTexts[nCurShowTextIdx].StartRoll(listSaveInfo[nCurUseInfoIdx], fGapValue, vecStartPos, fRollWidth, fRollSpeed, SetRollInfo);
        AddShowTextIdx();
        AddUseInfoIdx();
    }

    void AddShowTextIdx()
    {
        nCurShowTextIdx++;
        if(nCurShowTextIdx >= listRollTexts.Count)
        {
            nCurShowTextIdx = 0;
        }
    }

    void AddUseInfoIdx()
    {
        nCurUseInfoIdx++;
        if (nCurUseInfoIdx >= listSaveInfo.Count)
        {
            nCurUseInfoIdx = 0;
        }
    }

}
