using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuCmdAttrite(CDanmuEventConst.GetOut)]
public class DCmdGetOut : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        if (CGameColorFishMgr.Ins.emCurGameType != CGameColorFishMgr.EMGameType.Normal &&
            CGameColorFishMgr.Ins.emCurGameType != CGameColorFishMgr.EMGameType.Cinema &&
            CGameColorFishMgr.Ins.emCurGameType != CGameColorFishMgr.EMGameType.TimeBattle) return;

        string uid = dm.uid.ToString();

        //�ж��Ƿ�ŷ��
        if (!CGameColorFishMgr.Ins.nlCurOuHuangUID.Equals(uid) ||
            CGameColorFishMgr.Ins.pMap.pTietieSlot.pBindUnit == null)
        {
            return;
        }

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
        ///�����н�ֹ
        if (pUnit.emCurState == CPlayerUnit.EMState.Battle ||
            pUnit.pShowFishEndEvent != null)
        {
            return;
        }

        CPlayerUnit playerUnit = CGameColorFishMgr.Ins.pMap.pTietieSlot.pBindUnit;
        //��λ��
        CMapSlot tranIdleSlot = CGameColorFishMgr.Ins.pMap.GetRandIdleRoot();
        if (tranIdleSlot == null)
        {
            UIToast.Show("û��λ�ÿ��Ի���");
            return;
        }
        playerUnit.JumpTarget(tranIdleSlot);
    }
}
