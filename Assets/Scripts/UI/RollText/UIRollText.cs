using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRollText : UIBase
{
    [Header("��ʼλ��")]
    public Vector3 vecStartPos;
    [Header("�������")]
    public float fRollWidth;
    [Header("�����ı����")]
    public float fGapValue;
    [Header("�����ٶ�")]
    public Vector3 fRollSpeed;
    [Header("�����ı�")]
    public List<UIRollTextRoot> listRollTexts;
    [Header("������Ϣ")]
    public List<string> listSaveInfo;
    [Header("��ʱʹ�õ���Ϣ�±�")]
    public int nCurUseInfoIdx;
    [Header("��ǰ����չʾ�ı����±�")]
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
