using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMUnitBossWaitShow : FSMUnitBase
{
    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CPlayerUnit.EMState.BossWait;
        //pTimeTicker.Value = CGameBossMgr.Ins.fBossEatTime;
        //pTimeTicker.FillTime();

        SetUnitInfo(false);

        //UIBossBaseInfo uiBossInfo = UIManager.Instance.GetUI(UIResType.BossBaseInfo) as UIBossBaseInfo;
        //if (uiBossInfo != null)
        //{
        //    uiSlot = uiBossInfo.AddEatPlayer(pUnit);
        //}
    }

    public override void OnEnd(object obj)
    {
        SetUnitInfo(true);
    }

    void SetUnitInfo(bool value)
    {
        UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (uiGameInfo == null) return;

        UIShowUnitInfo uiUnitInfo = uiGameInfo.GetShowSlot(pUnit.uid);
        if (uiUnitInfo == null) return;

        uiUnitInfo.gameObject.SetActive(value);
    }
}
