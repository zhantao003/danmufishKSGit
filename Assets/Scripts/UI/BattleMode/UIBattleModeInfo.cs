using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleModeInfo : UIBase
{
    public GameObject[] objTimes;

    [Header("时间倒计时")]
    public Text uiLabelGameTime;
    [Header("时间倒计时2")]
    public Text uiLabelGameTime2;
    [Header("排行Root")]
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
            uiLabelGameTime2.text = "<color=#FFFFFF>准备阶段</color>\n<size=48>" + CHelpTools.GetTimeCounter((int)value) + "</size>";
            //uiLabelGameTime.text = "<color=#FFFFFF>等待开始</color>\n<size=36>" + CHelpTools.GetTimeCounter((int)value) + "</size>";
        }
        else if (CBattleModeMgr.Ins.emCurState == CBattleModeMgr.EMGameState.Gaming)
        {
            uiLabelGameTime.text = "<color=#FFFFFF>剩余时间</color>\n<size=36>" + CHelpTools.GetTimeCounter((int)value) + "</size>";
        }
        else if (CBattleModeMgr.Ins.emCurState == CBattleModeMgr.EMGameState.End)
        {
            uiLabelGameTime.text = "<color=#FFFFFF>等待结算</color>";// "结束倒计时 " + CHelpTools.GetTimeCounter((int)value);
        }
    }

    public void OnClickStart()
    {
        CBattleModeMgr.Ins.QuickStart();
    }

}
