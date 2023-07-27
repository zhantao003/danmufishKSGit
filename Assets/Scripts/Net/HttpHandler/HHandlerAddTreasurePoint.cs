using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.AddUserTreasurePoint)]
public class HHandlerAddTreasurePoint : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string szStatus = pMsg.GetString("status");
        if(!szStatus.Equals("ok"))
        {
            return;
        }

        string uid = pMsg.GetString("uid");
        long treasurePoint = pMsg.GetLong("treasurePoint");

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null) return;

        pPlayer.TreasurePoint = treasurePoint;
    }
}
