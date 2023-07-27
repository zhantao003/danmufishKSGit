using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFSMBoss204WaitAtk : CFSMBossBase
{
    CBoss204 pBoss;
    CPropertyTimer pTimeTicker = new CPropertyTimer();

    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CBossBase.EMState.WaitAtk;
        pBoss = pUnit as CBoss204;
        if (pBoss == null) return;

        pBoss.uiTweenIdle.Stop();

        pTimeTicker.Value = pBoss.fTimeWaitAtk;
        pTimeTicker.FillTime();

        //确定路径
        //pBoss.emAtkPath = (CBoss104.EMAtkPath)Random.Range(0, (int)CBoss104.EMAtkPath.Max);
        //pBoss.transform.position = pBoss.GetAtkPath(pBoss.emAtkPath)[0].position;

        //调整转向
        //Vector3 vDir = pBoss.GetAtkPath(pBoss.emAtkPath)[1].position - pBoss.GetAtkPath(pBoss.emAtkPath)[0].position;
        //vDir.y = 0F;
        //vDir = vDir.normalized;
        //pBoss.transform.forward = vDir;

        
    }

    public override void OnUpdate(object obj, float delta)
    {
        if (pBoss == null) return;
        if (pTimeTicker.Tick(delta))
        {
            pBoss.SetState(CBossBase.EMState.Attack);
        }
    }

    public override void OnEnd(object obj)
    {
        
    }
}

