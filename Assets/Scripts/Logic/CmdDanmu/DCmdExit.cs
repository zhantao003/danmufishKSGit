using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuCmdAttrite(CDanmuEventConst.ExitGame)]
public class DCmdExit : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        string uid = dm.uid.ToString();

        //Boss场景主播不让上岸
        //if (uid == CPlayerMgr.Ins.pOwner.uid &&
        //if (CGameColorFishMgr.Ins.pMapConfig != null)
        //{
        //    if (CGameColorFishMgr.Ins.pMapConfig.nID == 204 ||
        //        CGameColorFishMgr.Ins.pMapConfig.nID == 104)
        //    {
        //        return;
        //    }
        //}

        //如果是Boss战开始后不让上岸
        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
        {
            if(CGameBossMgr.Ins != null && 
               CGameBossMgr.Ins.emCurState == CGameBossMgr.EMState.Gaming)
            {
                return;
            }
        }
        else if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive)
        {
            //生存模式游戏开始后不让上岸
            //生存模式进入房间后不让上岸
            if (CGameSurviveMap.Ins.emCurState != CGameSurviveMap.EMGameState.Ready)
                return;
        }

        CPlayerUnit playerUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if (playerUnit != null &&
            (playerUnit.emCurState == CPlayerUnit.EMState.Battle ||
            playerUnit.emCurState == CPlayerUnit.EMState.HitDrop ||
            playerUnit.listNormalBooms.Count > 0 ||
            playerUnit.listTreasureBooms.Count > 0))
            return;

        ///正在拍卖的人无法上岸
        if(CHelpTools.CheckAuctionByUID(uid))
            return;
        //if (CAuctionMgr.Ins != null &&
        //    CAuctionMgr.Ins.bAuctionByUID(uid))
        //    return;
        //if (CAuctionMatMgr.Ins != null &&
        //    CAuctionMatMgr.Ins.pInfo != null &&
        //    CAuctionMatMgr.Ins.pInfo.uid == uid) 
        //    return;
        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive)
        {
            CGameSurviveMap.Ins.RemovePlayerInRoom(uid);
        }
        //if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle &&
        //   CPlayerMgr.Ins.GetAllIdleUnit().Count < CGameColorFishMgr.Ins.pStaticConfig.GetInt("可踢人判断数量"))
        //{
        //    CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        //    if (pUnit == null)
        //    {
        //        pUnit = CPlayerMgr.Ins.GetActiveUnit(uid);
        //        if (pUnit == null)
        //            return;
        //    }
        //    pUnit.ResetExitTick();

        //    return;
        //}

        //看看是不是在游戏队列
        CPlayerBaseInfo pActivePlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pActivePlayer != null)
        {
            CPlayerMgr.Ins.RemoveActivePlayer(pActivePlayer);
            UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
            if(roomInfo != null)
            {
                roomInfo.battleRoot.RemovePlayerInfo(uid);
            }
            if (pActivePlayer.emUserType == CPlayerBaseInfo.EMUserType.Guanzhong)
            {
                CPlayerMgr.Ins.RemovePlayer(uid);
            }
            if(CControlerSlotMgr.Ins != null)
            {
                CControlerSlotMgr.Ins.RecycleSlot(uid);
            }

            return;
        }
    }
}
