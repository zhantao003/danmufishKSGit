using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HHandlerAddFishMatByAuction : INetEventHandler
{
    long needCostCoin;
    public HHandlerAddFishMatByAuction(long price)
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

            uiUnitInfo.SetDmContent("²ÄÁÏ²»¹»");
        }
        else
        {
            string uid = pMsg.GetString("uid");
            int itemId = pMsg.GetInt("itemId");
            long count = pMsg.GetLong("count");
            CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
            if (pPlayer == null) return;

            pPlayer.pMatPack.SetItem(itemId, count);
            CAuctionMatMgr.Ins.SetPlayer(pPlayer, needCostCoin);

        }
    }
}


