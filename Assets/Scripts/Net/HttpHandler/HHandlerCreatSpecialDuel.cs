using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HHandlerCreatSpecialDuel : INetEventHandler
{
    long nlCostPrice;
    public HHandlerCreatSpecialDuel(long costPrice)
    {
        nlCostPrice = costPrice;
    }

    public void OnErrorCode(CLocalNetMsg pMsg)
    {

    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
        if (roomInfo.emCurShowType == ShowBoardType.Battle ||
            roomInfo.emCurShowType == ShowBoardType.SpecialBattle)
        {
            string uid = pMsg.GetString("uid");
            CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);

            ////同步宝藏积分
            //CHttpParam pReqParams = new CHttpParam
            //(
            //    new CHttpParamSlot("uid", uid.ToString()),
            //    new CHttpParamSlot("treasurePoint", nlCostPrice.ToString())
            //);
            //CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddUserTreasurePoint, pReqParams, CHttpMgr.Instance.nReconnectTimes, true);

            CPlayerNetHelper.AddTreasureCoin(uid, nlCostPrice);

            return;
        }

        if (pMsg.GetString("status") == "error")
        {
            UIToast.Show("海盗金币不够");
            if (roomInfo == null)
                return;

            if (roomInfo.emCurShowType != ShowBoardType.Battle &&
                roomInfo.emCurShowType != ShowBoardType.SpecialBattle)
            {
                roomInfo.battleRoot.nlCurPrice = 0;
            }
        }
        else
        {
            if (roomInfo == null)
                return;

            string uid = pMsg.GetString("uid");
            long treasurePoint = pMsg.GetLong("treasurePoint");

            CPlayerUnit player = CPlayerMgr.Ins.GetIdleUnit(uid);
            CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
            if (player == null || pPlayer == null)
                return;
            pPlayer.TreasurePoint = treasurePoint;
            SpecialCastInfo castInfo = new SpecialCastInfo();
            castInfo.playerInfo = pPlayer;
            castInfo.fishInfo = null;
            roomInfo.ShowSpecialBattle();
            roomInfo.battleRoot.nMaxCount = 10;
            roomInfo.battleRoot.AddPlayerInfo(castInfo);

            player.SetState(CPlayerUnit.EMState.Battle);
            player.ResetDuelCD();
        }
    }
}
