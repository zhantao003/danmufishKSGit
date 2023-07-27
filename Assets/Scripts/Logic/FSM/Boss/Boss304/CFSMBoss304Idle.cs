using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFSMBoss304Idle : CFSMBossBase
{
    CBoss304 pBoss;
    CPropertyTimer pTimeTickerAtk = new CPropertyTimer();

    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CBossBase.EMState.Idle;
        pBoss = pUnit as CBoss304;
        if (pBoss == null) return;
        UIBossBaseInfo bossBaseInfo = UIManager.Instance.GetUI(UIResType.BossBaseInfo) as UIBossBaseInfo;
        if (bossBaseInfo != null)
        {
            bossBaseInfo.ActiveBossWeakBar(true);
        }
        pBoss.uiTweenIdle.Play();

        float fAtkCD = Random.Range(pBoss.vAtkCDRange.x, pBoss.vAtkCDRange.y);
        Debug.Log("Boss下次攻击等待时间：" + fAtkCD);
        pTimeTickerAtk.Value = fAtkCD;
        pTimeTickerAtk.FillTime();
    }

    public override void OnUpdate(object obj, float delta)
    {
        if (pTimeTickerAtk.Tick(delta))
        {
            pBoss.SetState(CBossBase.EMState.SavePower);
        }
    }

    public override void OnEnd(object obj)
    {
        //pBoss.uiTweenIdle.Stop();
    }
}
