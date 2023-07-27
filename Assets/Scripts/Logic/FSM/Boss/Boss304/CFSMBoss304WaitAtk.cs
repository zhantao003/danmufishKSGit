using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFSMBoss304WaitAtk : CFSMBossBase
{
    CBoss304 pBoss;
    CPropertyTimer pTimeTicker = new CPropertyTimer();

    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CBossBase.EMState.WaitAtk;
        pBoss = pUnit as CBoss304;
        if (pBoss == null) return;

        pBoss.uiTweenIdle.Stop();

        pTimeTicker.Value = pBoss.fTimeWaitAtk;
        pTimeTicker.FillTime();

        //确定路径
        pBoss.transform.position = pBoss.listAtkPaths[0].tranStartPos.position;

        //调整转向
        Vector3 vDir = pBoss.listAtkPaths[0].tranEndPos.position - pBoss.listAtkPaths[0].tranStartPos.position;
        vDir.y = 0F;
        vDir = vDir.normalized;
        pBoss.transform.forward = vDir;

        //打开显示面板
        UIBossBaseInfo uiBossInfo = UIManager.Instance.GetUI(UIResType.BossBaseInfo) as UIBossBaseInfo;
        if (uiBossInfo != null)
        {
            uiBossInfo.SetBoss304AtkTip(true);
        }

        for(int i = 0;i < pBoss.listAtkPaths.Count;i++)
        {
            int nIdx = i;
            Vector3 vAlertRangePos = (pBoss.listAtkPaths[nIdx].tranStartPos.position + pBoss.listAtkPaths[nIdx].tranEndPos.position) / 2f;
            CEffectMgr.Instance.CreateEffSync(pBoss.szAlertRangeEffect, vAlertRangePos, Quaternion.identity, 0, delegate (GameObject value)
            {
                CAlertRangeLine alertRangeLine = value.GetComponent<CAlertRangeLine>();
                if (alertRangeLine != null)
                {
                    alertRangeLine.SetAlertRange(pBoss.listAtkPaths[nIdx].tranStartPos, pBoss.listAtkPaths[nIdx].tranEndPos);
                }
            });
        }
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
        UIBossBaseInfo uiBossInfo = UIManager.Instance.GetUI(UIResType.BossBaseInfo) as UIBossBaseInfo;
        if (uiBossInfo != null)
        {
            uiBossInfo.SetBoss304AtkTip(false);
        }
    }
}
