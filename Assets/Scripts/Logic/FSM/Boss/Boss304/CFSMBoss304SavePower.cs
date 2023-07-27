using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFSMBoss304SavePower : CFSMBossBase
{
    CBoss304 pBoss;
    CPropertyTimer pTimeTicker = new CPropertyTimer();

    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CBossBase.EMState.SavePower;
        pBoss = pUnit as CBoss304;
        if (pBoss == null) return;
        pUnit.pAnimeCtrl.Play(CBossAnimeConst.Anime_Idle);
        
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
            //È·¶¨Â·¾¶
            pBoss.GetAtkPathByNormal();
            pBoss.SetState(CBossBase.EMState.WaitAtk);
        }
    }
}
