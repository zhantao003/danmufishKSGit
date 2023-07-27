using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HHandlerAuction : INetEventHandler
{
    long needCostCoin;
    public HHandlerAuction(long price)
    {
        needCostCoin = price;
    }

    public void OnErrorCode(CLocalNetMsg pMsg)
    {

    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        //Debug.LogError(pMsg.GetData() + "==== Auction Data");
        if (pMsg.GetString("status") == "error")
        {

            //UIToast.Show("积分不够");
            string uid = pMsg.GetString("uid");
            UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
            if (uiGameInfo == null || !uiGameInfo.IsOpen()) return;

            UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(uid);
            if (uiUnitInfo == null) return;

            uiUnitInfo.SetDmContent("积分不够");
        }
        else
        {
            string uid = pMsg.GetString("uid");
            long gameCoin = pMsg.GetLong("fishCoin");

            CPlayerUnit player = CPlayerMgr.Ins.GetIdleUnit(uid);
            CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
            if (player == null || pPlayer == null)
                return;
            pPlayer.GameCoins = gameCoin;
            CAuctionMgr.Ins.SetPlayer(pPlayer, needCostCoin);

           
        }
    }
}
