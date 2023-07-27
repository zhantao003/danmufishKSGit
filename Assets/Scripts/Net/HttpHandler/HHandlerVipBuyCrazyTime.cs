using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HHandlerVipBuyCrazyTime : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
         
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string uid = pMsg.GetString("uid");
        long nlRoomID = pMsg.GetLong("roomId");
        bool bVtb = pMsg.GetInt("isVtb") > 0;
        long gameCoin = pMsg.GetLong("fishCoin");

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer != null)
        {
            pPlayer.GameCoins = gameCoin;
        }

        //启动急速
        if(CCrazyTimeMgr.Ins!=null)
        {
            CCrazyTimeMgr.Ins.AddCrazyTime(CGameColorFishMgr.Ins.pStaticConfig.GetInt("总督急速时间"));
        }
    }
}
