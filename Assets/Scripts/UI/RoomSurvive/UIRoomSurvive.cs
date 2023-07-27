using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomSurvive : UIBase
{
    public Text uiLabelGameTime;
    public Color[] arrColorGameTime;

    CGameSurviveMap.EMGameState emCurState;

    public GameObject objRoomTip;
    public GameObject objDangerTip;
    public CAudioMgr.CAudioSlottInfo pAudioDanger;

    public override void OnOpen()
    {
        CGameSurviveMap.Ins.dlgRefrehsState = SetGameState;
        CGameSurviveMap.Ins.dlgRefreshStateTime = SetGameTime;

        SetGameState((int)CGameSurviveMap.Ins.emCurState);
        SetGameTime(CGameSurviveMap.Ins.pTickerState.CurValue);
    }

    public void SetGameTime(float value)
    {
        if (emCurState == CGameSurviveMap.EMGameState.Ready)
        {
            uiLabelGameTime.text = "等待开始 " + CHelpTools.GetTimeCounter((int)value);
        }
        else if (emCurState == CGameSurviveMap.EMGameState.Gaming)
        {
            uiLabelGameTime.text = "剩余时间 " + CHelpTools.GetTimeCounter((int)value);
        } 
    }

    public void SetGameState(int state)
    {
        emCurState = (CGameSurviveMap.EMGameState)state;
        if(emCurState == CGameSurviveMap.EMGameState.Ready)
        {
            objRoomTip.SetActive(true);
            uiLabelGameTime.color = arrColorGameTime[0];
        }
        else if(emCurState == CGameSurviveMap.EMGameState.WaitStart)
        {
            objRoomTip.SetActive(false);
            objDangerTip.SetActive(true);
            CAudioMgr.Ins.PlaySoundBySlot(pAudioDanger, Vector3.zero);
        }
        else if(emCurState == CGameSurviveMap.EMGameState.Gaming)
        {
            uiLabelGameTime.color = arrColorGameTime[1];

            objDangerTip.SetActive(false);
        }
    }
}
