using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HHandlerAddFishMat : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string szStatus = pMsg.GetString("status");
        if (!szStatus.Equals("ok")) return;

        string uid = pMsg.GetString("uid"); ;
        int itemId = pMsg.GetInt("itemId");
        long count = pMsg.GetLong("count");
        long getCount = pMsg.GetLong("getCount");

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null) return;

        pPlayer.pMatPack.SetItem(itemId, count);
    }
}
