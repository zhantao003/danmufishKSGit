using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFSMBoss304OnHit : CFSMBossBase
{
    CBoss304 pBoss;
    CPropertyTimer pTimeTickerOnHit = new CPropertyTimer();

    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CBossBase.EMState.OnHit;
        pBoss = pUnit as CBoss304;
        if (pBoss == null) return;
        pUnit.pAnimeCtrl.Play(CBossAnimeConst.Anime_OnHit);
        pBoss.uiTweenIdle.Stop();

        pTimeTickerOnHit.Value = 1f;
        pTimeTickerOnHit.FillTime();

        pBoss.CheckWeak();
    }

    public override void OnUpdate(object obj, float delta)
    {
        if (pTimeTickerOnHit.Tick(delta))
        {
            pBoss.SetState(CBossBase.EMState.SavePower);
        }
    }

    public override void OnEnd(object obj)
    {

        //pBoss.uiTweenIdle.Stop();
    }
}
