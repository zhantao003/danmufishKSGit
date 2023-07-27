using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFSMBoss204EndAtk : CFSMBossBase
{
    CBoss204 pBoss;
    CPropertyTimer pTimeTicker = new CPropertyTimer();

    CPropertyTimer pWaitTicker = new CPropertyTimer();

    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CBossBase.EMState.EndAtk;
        pBoss = pUnit as CBoss204;
        if (pBoss == null) return;

        pBoss.transform.position = pBoss.vGameStartPos;
        //pBoss.transform.position = pBoss.vPosReady;
        //pBoss.transform.localEulerAngles = pBoss.vAngelIdleSelf;

        pWaitTicker = new CPropertyTimer();
        pWaitTicker.Value = pBoss.fTimeWaitTentacle;
        pWaitTicker.FillTime();

        pTimeTicker = null;

       
    }

    public override void OnUpdate(object obj, float delta)
    {
        if (pWaitTicker != null)
        {
            if (pWaitTicker.Tick(delta))
            {
                pWaitTicker = null;
                pTimeTicker = new CPropertyTimer();
                pTimeTicker.Value = pBoss.fTimeShow;
                pTimeTicker.FillTime();
            }
        }
        if (pTimeTicker != null)
        {
            if (pTimeTicker.Tick(delta))
            {
                pBoss.transform.position = pBoss.vPosReady;
                pBoss.SetState(CBossBase.EMState.Born);
                pTimeTicker = null;
            }
            else
            {
                pBoss.transform.position = Vector3.Lerp(pBoss.vGameStartPos, pBoss.vPosReady, 1f - pTimeTicker.GetTimeLerp());
            }
        }
    }
}