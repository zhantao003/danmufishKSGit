using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuCmdAttrite(CDanmuEventConst.TiePlayer)]
public class DCmdTiePlayer : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        if (CGameColorFishMgr.Ins.emCurGameType != CGameColorFishMgr.EMGameType.Normal &&
            CGameColorFishMgr.Ins.emCurGameType != CGameColorFishMgr.EMGameType.Cinema &&
            CGameColorFishMgr.Ins.emCurGameType != CGameColorFishMgr.EMGameType.TimeBattle) return;

        string uid = dm.uid.ToString();

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
        ///决斗中禁止贴贴
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

        //判断玩家
        CPlayerUnit pTargetUnit = null;
        List<CPlayerUnit> listPlayerUnits = CPlayerMgr.Ins.GetAllIdleUnit();// CPlayerMgr.Ins.pOwner.uid);
        List<CPlayerUnit> listRandomPlayerUnits = new List<CPlayerUnit>();
        for(int i= 0;i < listPlayerUnits.Count;i++)
        {
            if(listPlayerUnits[i].emCurState == CPlayerUnit.EMState.Battle ||
               listPlayerUnits[i].pShowFishEndEvent != null)
            {
                
            }
            else
            {
                listRandomPlayerUnits.Add(listPlayerUnits[i]);
            }
        }
        if (listRandomPlayerUnits.Count > 0)
        {
            ///获取除开呆在欧皇船上的人
            pTargetUnit = listRandomPlayerUnits[Random.Range(0, listRandomPlayerUnits.Count)];
        }
        if (pTargetUnit == null)
            return;

        //判断是否欧皇
        if (CGameColorFishMgr.Ins.nlCurOuHuangUID != uid ||
           CGameColorFishMgr.Ins.nlCurOuHuangUID == pTargetUnit.uid)
        {
            return;
        }

        ///决斗中禁止贴贴
        if (pTargetUnit.emCurState == CPlayerUnit.EMState.Battle ||
            pTargetUnit.pShowFishEndEvent != null)
        {
            return;
        }

        DoTieTie(pUnit, pTargetUnit);
    }

    public void DoTieTie(CPlayerUnit pUnit, CPlayerUnit pTargetUnit)
    {
        ///获取玩家单位
        if (pUnit == null)
            return;
        ///决斗中禁止贴贴
        if (pUnit.emCurState == CPlayerUnit.EMState.Battle ||
            pUnit.pShowFishEndEvent != null)
        {
            return;
        }

        //判断玩家目标
        if (pTargetUnit == null)
            return;

        ///决斗中禁止贴贴
        if (pTargetUnit.emCurState == CPlayerUnit.EMState.Battle ||
            pTargetUnit.pShowFishEndEvent != null)
        {
            return;
        }

        //判断欧皇是否需要跳
        if (CGameColorFishMgr.Ins.pMap.pOuHuangSlot.pBindUnit == null ||
           CGameColorFishMgr.Ins.pMap.pOuHuangSlot.pBindUnit.uid != pUnit.uid)
        {
            pUnit.JumpTarget(CGameColorFishMgr.Ins.pMap.pOuHuangSlot);
        }

        //判断玩家是否跳过去
        if (CGameColorFishMgr.Ins.pMap.pTietieSlot.pBindUnit == null)
        {
            pTargetUnit.dlgCallOnceJumpEvent = delegate ()
            {
                Vector3 vCenterPos = (CGameColorFishMgr.Ins.pMap.pOuHuangSlot.tranSelf.position + CGameColorFishMgr.Ins.pMap.pTietieSlot.tranSelf.position) * 0.5F;
                CEffectMgr.Instance.CreateEffSync("Effect/effHeartBoom", vCenterPos, Quaternion.identity, 0);
            };

            pTargetUnit.JumpTarget(CGameColorFishMgr.Ins.pMap.pTietieSlot);
        }
        else
        {
            if (CGameColorFishMgr.Ins.pMap.pTietieSlot.pBindUnit.uid != pTargetUnit.uid)
            {
                CPlayerUnit pOldUnit = CGameColorFishMgr.Ins.pMap.pTietieSlot.pBindUnit;
                CMapSlot pOwnerSlot = pTargetUnit.pMapSlot;

                pTargetUnit.dlgCallOnceJumpEvent = delegate ()
                {
                    Vector3 vCenterPos = (CGameColorFishMgr.Ins.pMap.pOuHuangSlot.tranSelf.position + CGameColorFishMgr.Ins.pMap.pTietieSlot.tranSelf.position) * 0.5F;
                    CEffectMgr.Instance.CreateEffSync("Effect/effHeartBoom", vCenterPos, Quaternion.identity, 0);
                };

                //交换位置
                pTargetUnit.JumpTarget(CGameColorFishMgr.Ins.pMap.pTietieSlot);
                pOldUnit.pMapSlot = null;
                if (pOwnerSlot != null)
                {
                    pOldUnit.JumpTarget(pOwnerSlot);
                }
            }
        }
    }

}
