using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFSMBoss104EndAtk : CFSMBossBase
{
    CBoss104 pBoss;
    CPropertyTimer pTimeTicker = new CPropertyTimer();

    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CBossBase.EMState.EndAtk;
        pBoss = pUnit as CBoss104;
        if (pBoss == null) return;

        pBoss.transform.position = pBoss.vPosReady;
        pBoss.transform.localEulerAngles = pBoss.vAngelIdleSelf;

        pTimeTicker.Value = 1.5F;
        pTimeTicker.FillTime();
    }

    public override void OnUpdate(object obj, float delta)
    {
        if(pTimeTicker.Tick(delta))
        {
            pBoss.SetState(CBossBase.EMState.Born);
        }
    }
}
