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

        //判断是否欧皇
        if (!CGameColorFishMgr.Ins.nlCurOuHuangUID.Equals(uid) ||
            CGameColorFishMgr.Ins.pMap.pTietieSlot.pBindUnit == null)
        {
            return;
        }

        ///根据uid获取玩家单位
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
        ///决斗中禁止
        if (pUnit.emCurState == CPlayerUnit.EMState.Battle ||
            pUnit.pShowFishEndEvent != null)
        {
            return;
        }

        CPlayerUnit playerUnit = CGameColorFishMgr.Ins.pMap.pTietieSlot.pBindUnit;
        //换位置
        CMapSlot tranIdleSlot = CGameColorFishMgr.Ins.pMap.GetRandIdleRoot();
        if (tranIdleSlot == null)
        {
            UIToast.Show("没有位置可以换了");
            return;
        }
        playerUnit.JumpTarget(tranIdleSlot);
    }
}
