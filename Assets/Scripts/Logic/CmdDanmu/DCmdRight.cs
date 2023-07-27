using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuCmdAttrite(CDanmuEventConst.Right)]
public class DCmdRight : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        string uid = dm.uid.ToString();

        //如果是Boss战开始后不让上岸
        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
        {
            if (CGameBossMgr.Ins != null &&
            CGameBossMgr.Ins.emCurState != CGameBossMgr.EMState.Gaming)
            {
                return;
            }

            CPlayerBaseInfo pPlayerInfo = CPlayerMgr.Ins.GetPlayer(uid);
            if (pPlayerInfo == null)
                return;

            CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
            if (pUnit == null)
            {
                pUnit = CPlayerMgr.Ins.GetActiveUnit(uid);
                if (pUnit == null)
                    return;
            }

            if (pUnit.emCurState == CPlayerUnit.EMState.BossEat ||
                pUnit.emCurState == CPlayerUnit.EMState.BossWait ||
                pUnit.emCurState == CPlayerUnit.EMState.BossReturn)
                return;

            CControlerSlotByBoss slot = CControlerSlotMgr.Ins.GetSlot(uid);
            if (slot == null)
                return;

            slot.Move(CControlerSlotByBoss.SlotDirection.Right);
        }
        else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal ||
                 CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
        {
            CPlayerUnit pPlayerUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
            if (pPlayerUnit == null) return;

            CBoatMoveCtrl slot = pPlayerUnit.pMapSlot.gameObject.GetComponent<CBoatMoveCtrl>();
            if (slot == null) return;

            slot.Move(CBoatMoveCtrl.SlotDirection.Right);
        }
        else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive)
        {
            if (CGameSurviveMap.Ins.emCurState != CGameSurviveMap.EMGameState.Ready ||
                CGameSurviveMap.Ins.IsPlayerInRoom(uid))
                return;

            CPlayerUnit pPlayerUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
            if (pPlayerUnit == null) return;

            CBoatMoveCtrl slot = pPlayerUnit.pMapSlot.gameObject.GetComponent<CBoatMoveCtrl>();
            if (slot == null) return;

            slot.Move(CBoatMoveCtrl.SlotDirection.Right);
        }

        return;
    }
}
