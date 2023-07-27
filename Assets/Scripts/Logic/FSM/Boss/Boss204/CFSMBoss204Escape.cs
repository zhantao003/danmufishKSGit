using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFSMBoss204Escape : CFSMBossBase
{
    CBoss204 pBoss;
    CPropertyTimer pTimeTicker = new CPropertyTimer();

    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CBossBase.EMState.Escape;
        pBoss = pUnit as CBoss204;

        CGameBossMgr.Ins.emCurState = CGameBossMgr.EMState.End;

        if (pBoss == null) return;

        pTimeTicker.Value = pBoss.fTimeSavePower;
        pTimeTicker.FillTime();
    }

    public override void OnFixedUpdate(object obj, float delta)
    {
        if (!pTimeTicker.Tick(delta))
        {
            pBoss.transform.position = Vector3.Lerp(pBoss.vPosReady, pBoss.vGameStartPos, 1f - pTimeTicker.GetTimeLerp());
        }
        else
        {
            pUnit.transform.position = pBoss.vGameStartPos;

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
