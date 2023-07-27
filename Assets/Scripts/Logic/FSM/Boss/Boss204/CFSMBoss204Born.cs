using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFSMBoss204Born : CFSMBossBase
{
    CBoss204 pBoss;
    CPropertyTimer pTimeTicker = new CPropertyTimer();

    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CBossBase.EMState.Born;
        pBoss = pUnit as CBoss204;
        if (pBoss == null) return;

        //pBoss.transform.position = pBoss.vGameStartPos;

        pTimeTicker.Value = pBoss.fTimeBorn;
        pTimeTicker.FillTime();
    }

    public override void OnFixedUpdate(object obj, float delta)
    {
        if (!pTimeTicker.Tick(delta))
        {
            //pBoss.transform.position = Vector3.Lerp(pBoss.vGameStartPos, pBoss.vPosReady, 1f - pTimeTicker.GetTimeLerp());
        }
        else
        {
            //pUnit.transform.position = pBoss.vPosReady;
            pBoss.SetState(CBossBase.EMState.Idle);
        }
    }
}

