using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.GetUserFishMat)]
public class HHandlerGetUserFishMats : INetEventHandler
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
        CLocalNetArrayMsg arrContents = pMsg.GetNetMsgArr("list");
        if (arrContents == null ||
            arrContents.GetSize() <= 0)
            return;

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null) return;

        for(int i=0; i< arrContents.GetSize(); i++)
        {
            CLocalNetMsg msgSlot = arrContents.GetNetMsg(i);
            long itemId = msgSlot.GetLong("itemId");
            long count = msgSlot.GetLong("count");

            pPlayer.pMatPack.SetItem(itemId, count);
        }
    }
}
