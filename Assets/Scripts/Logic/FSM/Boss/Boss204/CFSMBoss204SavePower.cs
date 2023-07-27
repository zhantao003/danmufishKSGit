using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFSMBoss204SavePower : CFSMBossBase
{
    CBoss204 pBoss;
    CPropertyTimer pTimeTicker = new CPropertyTimer();

    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CBossBase.EMState.SavePower;
        pBoss = pUnit as CBoss204;
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
            pBoss.SetState(CBossBase.EMState.WaitAtk);
        }
    }
}

