using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFSMBoss304Born : CFSMBossBase
{
    CBoss304 pBoss;
    CPropertyTimer pTimeTicker = new CPropertyTimer();

    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CBossBase.EMState.Born;
        pBoss = pUnit as CBoss304;
        if (pBoss == null) return;

        pBoss.transform.position = pBoss.vPosReady;
        pBoss.transform.localEulerAngles = pBoss.vAngelIdleSelf;
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
