using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuCmdAttrite(CDanmuEventConst.ChgPos)]
public class DCmdChgPos : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        //����ģʽ������
        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive) return;

        //204����������
        if(CGameColorFishMgr.Ins.pMapConfig != null)
        {
            if(CGameColorFishMgr.Ins.pMapConfig.nID == 204)
            {
                return;
            }
        }

        string uid = dm.uid.ToString();
        ///����uid��ȡ��ҵ�λ
        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if (pUnit == null)
        {
            pUnit = CPlayerMgr.Ins.GetActiveUnit(uid);
            if (pUnit == null)
                return;
        }
        if (CGameColorFishMgr.Ins.emCurGameType != CGameColorFishMgr.EMGameType.Battle &&
            CGameColorFishMgr.Ins.emCurGameType != CGameColorFishMgr.EMGameType.Boss)
        {
            if (pUnit.bCheckYZM ||
            pUnit.pInfo.CheckIsGrayName())
            {
                return;
            }
        }

        ///��ֹ��λ�õ�״̬
        if (pUnit.emCurState == CPlayerUnit.EMState.Battle ||
            pUnit.emCurState == CPlayerUnit.EMState.BossEat ||
            pUnit.emCurState == CPlayerUnit.EMState.BossReturn ||
            pUnit.emCurState == CPlayerUnit.EMState.BossWait ||
            pUnit.emCurState == CPlayerUnit.EMState.GunShootBoss ||
            pUnit.emCurState == CPlayerUnit.EMState.RPGShootBoss ||
            pUnit.emCurState == CPlayerUnit.EMState.HitDrop ||
            pUnit.pShowFishEndEvent != null)
        {
            return;
        }

        //��λ��
        CMapSlot tranIdleSlot = CGameColorFishMgr.Ins.pMap.GetRandIdleRoot();
        if (tranIdleSlot == null)
        {
            UIToast.Show("û��λ�ÿ��Ի���");
            return;
        }
        pUnit.JumpTarget(tranIdleSlot);

    }
}
