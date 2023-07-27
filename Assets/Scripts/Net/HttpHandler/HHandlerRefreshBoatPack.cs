using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HHandlerRefreshBoatPack : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {

    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string szStatus = pMsg.GetString("status");
        if (!szStatus.Equals("ok")) return;

        string uid = pMsg.GetString("uid");
        int boatId = pMsg.GetInt("boatId");
        long exTime = pMsg.GetInt("exTime");

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null) return;

        CPlayerBoatInfo pBoatInfo = new CPlayerBoatInfo();
        pBoatInfo.nBoatId = boatId;
        pBoatInfo.nExTime = exTime;
        pPlayer.pBoatPack.AddInfo(pBoatInfo);
        pPlayer.pBoatPack.SortBoatPack();
    }
}
