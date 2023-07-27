using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HHandlerAddDuel : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {

    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        //Debug.LogError("Enter Msg");
        if (pMsg.GetString("status") == "error")
        {
            UIToast.Show("积分不够");
        }
        else
        {
            string uid = pMsg.GetString("uid");
            long gameCoin = pMsg.GetLong("fishCoin");
            CPlayerUnit player = CPlayerMgr.Ins.GetIdleUnit(uid);
            CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);

            if (player == null || pPlayer == null)
                return;
            
            UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
            if (roomInfo == null)
                return;

            SpecialCastInfo castInfo = new SpecialCastInfo();
            castInfo.playerInfo = pPlayer;
            castInfo.fishInfo = null;
            ///判断该玩家是否已经加入了决斗
            if(roomInfo.battleRoot.bHavePlayer(uid))
            {
                return;
            }
            if (CGameColorFishMgr.Ins.pMap.pDuelBoat.emCurState == CDuelBoat.EMState.Show)   //船正在展示阶段
            {
                if (!roomInfo.battleRoot.AddPlayerInfo(castInfo))
                    return;
                //CFishRecordMgr.Ins.AddRecord(roomInfo.battleRoot.nlCurPrice * -1, pPlayer);
                pPlayer.GameCoins = gameCoin;
                player.SetState(CPlayerUnit.EMState.Battle);
            }
            else if (CGameColorFishMgr.Ins.pMap.pDuelBoat.emCurState == CDuelBoat.EMState.Stay) //船正在停留阶段
            {
                CMapSlot mapSlot = CGameColorFishMgr.Ins.pMap.pDuelBoat.GetIdleSlot();
                if (mapSlot == null) return;
                if(!roomInfo.battleRoot.AddPlayerInfo(castInfo))
                {
                    return;
                }
                //CFishRecordMgr.Ins.AddRecord(roomInfo.battleRoot.nlCurPrice * -1, pPlayer);
                pPlayer.GameCoins = gameCoin;
                player.JumpTarget(mapSlot, false);
                mapSlot.BindPlayer(player);
                //player.SetState(CPlayerUnit.EMState.Battle);
            }
            else if (CGameColorFishMgr.Ins.pMap.pDuelBoat.emCurState == CDuelBoat.EMState.Hide) //船正在隐藏阶段
            {

            }
            
            

            //CMapSlot mapSlot = CGameColorFishMgr.Ins.pMap.pDuelBoat.GetIdleSlot();
            //player.JumpTarget(mapSlot, false);
            //mapSlot.pBindUnit = player;
        }
    }
}

