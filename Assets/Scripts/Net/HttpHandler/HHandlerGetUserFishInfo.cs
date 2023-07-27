using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.GetUserFishInfo)]
public class HHandlerGetUserFishInfo : INetEventHandler
{
    public OnDeleagteLoginSuc callLoginOver;

    public HHandlerGetUserFishInfo() { }

    public HHandlerGetUserFishInfo(OnDeleagteLoginSuc call)
    {
        callLoginOver = call;
    }

    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string uid = pMsg.GetString("uid");
        long fishCoin = pMsg.GetLong("fishCoin");
        long fishBaits = pMsg.GetLong("fishBait");
        long freeFishBait = pMsg.GetLong("freeFishBait");
        //long fishGanCount = pMsg.GetLong("fishGan");
        //long fishPiaoCount = pMsg.GetLong("fishPiao");
        //long fishXianCount = pMsg.GetLong("fishXian");
        long fishLunCount = pMsg.GetLong("fishLun");
        int boatAvatar = pMsg.GetInt("boatAvatar");
        long vipPlayer = pMsg.GetLong("vipPlayer");
        long treasurePoint = pMsg.GetLong("treasurePoint");
        //long fishLv = pMsg.GetLong("fishLv");
        //long fishExp = pMsg.GetLong("fishExp");
        //long gachaCount = pMsg.GetLong("gachaCount");
        //int gameStarLv = pMsg.GetInt("gameStarLv");
        //int creditPoint = pMsg.GetInt("crePoint");
        long winnerOuhuang = pMsg.GetLong("winnerOuhuang");
        long winnerRicher = pMsg.GetLong("winnerRicher");
        long fishWinnerPoint = pMsg.GetLong("fishWinnerPoint");

        //基础信息同步
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null) return;
        pPlayer.GameCoins = fishCoin;
        pPlayer.nlBaitCount = fishBaits;
        pPlayer.nlFreeBaitCount = freeFishBait;
        //pPlayer.nlRobCount = fishGanCount;
        pPlayer.nlFeiLunCount = fishLunCount;
        //pPlayer.nlBuoyCount = fishPiaoCount;
        pPlayer.nVipPlayer = (CPlayerBaseInfo.EMVipLv)vipPlayer;
        pPlayer.TreasurePoint = treasurePoint;
        pPlayer.SetUserLv(1, 0);
        pPlayer.nGachaGiftCount =0;
        pPlayer.nGameStarLv = 0;
        pPlayer.nCreditPoint = 20;
        pPlayer.nWinnerOuhuang = winnerOuhuang;
        pPlayer.nWinnerRicher = winnerRicher;
        pPlayer.nFishWinnerPoint = fishWinnerPoint;

        //Debug.Log(pPlayer.userName + "--当前验证码积分:" + creditPoint);
        //加一个容错
        if (boatAvatar <= 0)
        {
            pPlayer.nBoatAvatarId = 101;
        }
        else
        {
            pPlayer.nBoatAvatarId = boatAvatar;
        }

        if(pPlayer.nBoatAvatarId == 101)
        {
            pPlayer.RefreshBoatAvatar();
        }
        
        if(pPlayer.avatarId == 101)
        {
            pPlayer.RefreshRoleAvatar();
        }

        //获取玩家的鱼竿ID
        int nFishGanID = pMsg.GetInt("ganAvatar");
        if(nFishGanID <=0)
        {
            nFishGanID = 101;
        }
        pPlayer.nFishGanAvatarId = nFishGanID;
        pPlayer.RefreshProAdd();

        //刷新玩家属性
        CPlayerMgr.Ins.InitPlayerInfo(uid);

        //获取房间鱼币总收获
        if (CPlayerMgr.Ins.pOwner!=null && 
            uid == CPlayerMgr.Ins.pOwner.uid)
        {
            CRoomRecordInfoMgr.Ins.RoomGainCoin = pMsg.GetLong("roomGainCoin");
        }

        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if (pUnit != null)
        {
            pUnit.dlgChgGift?.Invoke();
        }

        callLoginOver?.Invoke(pPlayer);
    }
}
