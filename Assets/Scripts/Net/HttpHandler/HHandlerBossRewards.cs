using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HHandlerBossRewards : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        CLocalNetArrayMsg arrMsgContent = pMsg.GetNetMsgArr("resData");
        if (arrMsgContent == null) return;

        for(int i=0; i<arrMsgContent.GetSize(); i++)
        {
            CLocalNetMsg msgContent = arrMsgContent.GetNetMsg(i);
            string uid = pMsg.GetString("uid");
            long itemId = msgContent.GetLong("itemId");
            long count = msgContent.GetLong("count");
            long getCount = msgContent.GetLong("getCount");

            CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
            if (pPlayer == null) continue;

            pPlayer.pMatPack.SetItem(itemId, count);
        }
    }
}
