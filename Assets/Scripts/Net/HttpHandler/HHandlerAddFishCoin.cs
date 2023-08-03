using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.AddFishCoin)]
public class HHandlerAddFishCoin : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {

    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string szStatus = pMsg.GetString("status");
        if (!szStatus.Equals("ok")) return;

        string uid = pMsg.GetString("uid");
        long gameCoin = pMsg.GetLong("fishCoin");
        bool isRoomOk = pMsg.GetBool("roomOk");

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer != null)
        {
            pPlayer.GameCoins = gameCoin;
        }

        //Debug.Log(uid + "=====Game Coin =====" + gameCoin);

        if(!isRoomOk)
        {
            CPlayerLogicHelper.CheckExit(uid);
        }
    }
}
