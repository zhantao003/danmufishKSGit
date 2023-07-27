using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFSMBoss304Escape : CFSMBossBase
{
    CBoss304 pBoss;
    CPropertyTimer pTimeTicker = new CPropertyTimer();

    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CBossBase.EMState.Escape;
        pBoss = pUnit as CBoss304;

        CGameBossMgr.Ins.emCurState = CGameBossMgr.EMState.End;

        if (pBoss == null) return;

        pTimeTicker.Value = pBoss.fTimeSavePower;
        pTimeTicker.FillTime();
    }

    public override void OnFixedUpdate(object obj, float delta)
    {
        if (!pTimeTicker.Tick(delta))
        {
            pBoss.transform.position = Vector3.Lerp(pBoss.vPosIdle, pBoss.vPosExit, 1f - pTimeTicker.GetTimeLerp());
        }
        else
        {
            pUnit.transform.position = pBoss.vPosExit;

            UIManager.Instance.OpenUI(UIResType.BossDmgResult);
            UIBossDmgResult uiBossRes = UIManager.Instance.GetUI(UIResType.BossDmgResult) as UIBossDmgResult;
            if (uiBossRes != null)
            {
                uiBossRes.SetInfo(false);
            }
            pBoss.SetState(CBossBase.EMState.Ready);
        }
    }
}
