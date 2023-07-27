using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFSMBoss104SavePower : CFSMBossBase
{
    CBoss104 pBoss;
    CPropertyTimer pTimeTicker = new CPropertyTimer();

    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CBossBase.EMState.SavePower;
        pBoss = pUnit as CBoss104;
        if (pBoss == null) return;

        pTimeTicker.Value = pBoss.fTimeSavePower;
        pTimeTicker.FillTime();
    }

    public override void OnFixedUpdate(object obj, float delta)
    {
        if (!pTimeTicker.Tick(delta))
        {
            pBoss.transform.position = Vector3.Lerp(pBoss.vPosIdle, pBoss.vPosReady, 1f - pTimeTicker.GetTimeLerp());
        }
        else
        {
            pUnit.transform.position = pBoss.vPosReady;
            pBoss.SetState(CBossBase.EMState.WaitAtk);
        }
    }
}
