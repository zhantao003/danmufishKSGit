using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFSMBoss104Born : CFSMBossBase
{
    CBoss104 pBoss;
    CPropertyTimer pTimeTicker = new CPropertyTimer();

    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CBossBase.EMState.Born;
        pBoss = pUnit as CBoss104;
        if (pBoss == null) return;

        pBoss.transform.position = pBoss.vPosReady;

        pTimeTicker.Value = pBoss.fTimeBorn;
        pTimeTicker.FillTime();
    }

    public override void OnFixedUpdate(object obj, float delta)
    {
        if (pBoss == null) return;
        if (!pTimeTicker.Tick(delta))
        {
            pBoss.transform.position = Vector3.Lerp(pBoss.vPosReady, pBoss.vPosIdle, 1f - pTimeTicker.GetTimeLerp());
        }
        else
        {
            pUnit.transform.position = pBoss.vPosIdle;
            pBoss.SetState(CBossBase.EMState.Idle);
        }
    }
}
