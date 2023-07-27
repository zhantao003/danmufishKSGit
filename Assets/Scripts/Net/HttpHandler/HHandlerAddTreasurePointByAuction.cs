using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HHandlerAddTreasurePointByAuction : INetEventHandler
{
    long needCostCoin;
    public HHandlerAddTreasurePointByAuction(long price)
    {
        needCostCoin = price;
    }

    public void OnErrorCode(CLocalNetMsg pMsg)
    {

    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string szStatus = pMsg.GetString("status");
        

        
        //Debug.LogError(pMsg.GetData() + "==== Auction Data");
        if (szStatus == "error")
        {
            string uid = pMsg.GetString("uid");
            UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
            if (uiGameInfo == null || !uiGameInfo.IsOpen()) return;

            UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(uid);
            if (uiUnitInfo == null) return;

            uiUnitInfo.SetDmContent("º£µÁ½ð±Ò²»¹»");
        }
        else
        {
            string uid = pMsg.GetString("uid");
            long treasurePoint = pMsg.GetLong("treasurePoint");

            CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
            if (pPlayer == null) return;

            pPlayer.TreasurePoint = treasurePoint;
            CAuctionMatMgr.Ins.SetPlayer(pPlayer, needCostCoin);

        }
    }
}