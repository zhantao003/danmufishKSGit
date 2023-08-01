using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleModeInfo : UIBase
{
    public GameObject[] objTimes;

    [Header("ʱ�䵹��ʱ")]
    public Text uiLabelGameTime;
    [Header("ʱ�䵹��ʱ2")]
    public Text uiLabelGameTime2;
    [Header("����Root")]
    public UIBattleRankRoot battleRankRoot;

    public UITweenPos tweenPos;

    public void Init()
    {
        if (CBattleModeMgr.Ins != null)
        {
            CBattleModeMgr.Ins.deleValueChg = SetGameTime;
        }
        battleRankRoot.SetActive(false);
    }

    public void StartShow()
    {
        tweenPos.Play();
    }

    public void InitRankInfo(CGetChampionRule curChampionRuleByOuHuang, CGetChampionRule curChampionRuleByProfit)
    {
        battleRankRoot.ShowRankInfo(curChampionRuleByOuHuang, curChampionRuleByProfit);

        //CPlayerNetHelper.GetWinnerOuhuangRL("");
        //CPlayerNetHelper.GetWinnerRicherRL("");
    }

    public void SetGameTime(float value)
    {
        if (CBattleModeMgr.Ins == null ||
            uiLabelGameTime == null)
            return;

        objTimes[0].SetActive(CBattleModeMgr.Ins.emCurState == CBattleModeMgr.EMGameState.Ready);
        objTimes[1].SetActive(CBattleModeMgr.Ins.emCurState != CBattleModeMgr.EMGameState.Ready);

        if (CBattleModeMgr.Ins.emCurState == CBattleModeMgr.EMGameState.Ready)
        {
            uiLabelGameTime2.text = "<color=#FFFFFF>׼���׶�</color>\n<size=48>" + CHelpTools.GetTimeCounter((int)value) + "</size>";
            //uiLabelGameTime.text = "<color=#FFFFFF>�ȴ���ʼ</color>\n<size=36>" + CHelpTools.GetTimeCounter((int)value) + "</size>";
        }
        else if (CBattleModeMgr.Ins.emCurState == CBattleModeMgr.EMGameState.Gaming)
        {
            uiLabelGameTime.text = "<color=#FFFFFF>ʣ��ʱ��</color>\n<size=36>" + CHelpTools.GetTimeCounter((int)value) + "</size>";
        }
        else if (CBattleModeMgr.Ins.emCurState == CBattleModeMgr.EMGameState.End)
        {
            uiLabelGameTime.text = "<color=#FFFFFF>�ȴ�����</color>";// "��������ʱ " + CHelpTools.GetTimeCounter((int)value);
        }
    }

    public void OnClickStart()
    {
        CBattleModeMgr.Ins.QuickStart();
    }

}
