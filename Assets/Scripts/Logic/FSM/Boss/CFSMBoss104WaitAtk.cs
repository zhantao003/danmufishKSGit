using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFSMBoss104WaitAtk : CFSMBossBase
{
    CBoss104 pBoss;
    CPropertyTimer pTimeTicker = new CPropertyTimer();

    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CBossBase.EMState.WaitAtk;
        pBoss = pUnit as CBoss104;
        if (pBoss == null) return;

        pBoss.uiTweenIdle.Stop();

        pTimeTicker.Value = pBoss.fTimeWaitAtk;
        pTimeTicker.FillTime();

        //确定路径
        pBoss.emAtkPath = (CBoss104.EMAtkPath)Random.Range(0, (int)CBoss104.EMAtkPath.Max);
        pBoss.transform.position = pBoss.GetAtkPath(pBoss.emAtkPath)[0].position;

        //调整转向
        Vector3 vDir = pBoss.GetAtkPath(pBoss.emAtkPath)[1].position - pBoss.GetAtkPath(pBoss.emAtkPath)[0].position;
        vDir.y = 0F;
        vDir = vDir.normalized;
        pBoss.transform.forward = vDir;

        //打开显示面板
        UIBossBaseInfo uiBossInfo = UIManager.Instance.GetUI(UIResType.BossBaseInfo) as UIBossBaseInfo;
        if(uiBossInfo != null)
        {
            uiBossInfo.SetBoss104AtkTip(pBoss.emAtkPath);
        }
    }

    public override void OnUpdate(object obj, float delta)
    {
        if (pBoss == null) return;
        if(pTimeTicker.Tick(delta))
        {
            pBoss.SetState(CBossBase.EMState.Attack);
        }
    }

    public override void OnEnd(object obj)
    {
        UIBossBaseInfo uiBossInfo = UIManager.Instance.GetUI(UIResType.BossBaseInfo) as UIBossBaseInfo;
        if (uiBossInfo != null)
        {
            uiBossInfo.SetBoss104AtkTip(CBoss104.EMAtkPath.Max);
        }
    }
}
