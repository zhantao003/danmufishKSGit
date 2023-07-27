using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CDanmuCmdAttrite(CDanmuEventConst.TieTie)]
public class DCmdTieTie : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        return;
        if (CGameColorFishMgr.Ins.emCurGameType != CGameColorFishMgr.EMGameType.Normal &&
            CGameColorFishMgr.Ins.emCurGameType != CGameColorFishMgr.EMGameType.Cinema &&
            CGameColorFishMgr.Ins.emCurGameType != CGameColorFishMgr.EMGameType.TimeBattle) return;

        string uid = dm.uid.ToString();

        //�ж��Ƿ�ŷ��
        if (!CGameColorFishMgr.Ins.nlCurOuHuangUID.Equals(uid))// ||
             //CGameColorFishMgr.Ins.nlCurOuHuangUID.Equals(CPlayerMgr.Ins.pOwner.uid))
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
        ///�����н�ֹ����
        if (pUnit.emCurState == CPlayerUnit.EMState.Battle ||
            pUnit.pShowFishEndEvent != null)
        {
            return;
        }

        if (CGameColorFishMgr.Ins.pMap.pTietieSlot != null &&
            CGameColorFishMgr.Ins.pMap.pTietieSlot.pBindUnit != null)
        {
            if (CGameColorFishMgr.Ins.pMap.pTietieSlot.pBindUnit.emCurState == CPlayerUnit.EMState.Battle ||
                CGameColorFishMgr.Ins.pMap.pTietieSlot.pBindUnit.pShowFishEndEvent != null)
            {
                return;
            }
        }

        //�ж�����
        CPlayerUnit pOwnerUnit = CPlayerMgr.Ins.GetIdleUnit(CPlayerMgr.Ins.pOwner.uid);
        if (pOwnerUnit == null)
        {
            pOwnerUnit = CPlayerMgr.Ins.GetActiveUnit(CPlayerMgr.Ins.pOwner.uid);
            if (pOwnerUnit == null)
                return;
        }

        ///�����н�ֹ����
        if (pOwnerUnit.emCurState == CPlayerUnit.EMState.Battle ||
            pOwnerUnit.pShowFishEndEvent != null)
        {
            return;
        }

        UIRoomInfo uiRoomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
        if (uiRoomInfo == null) return;
        //uiRoomInfo.uiTieTieRoot.Show(true);
        uiRoomInfo.uiTieTieRoot.DoTitTie();
    }
}
