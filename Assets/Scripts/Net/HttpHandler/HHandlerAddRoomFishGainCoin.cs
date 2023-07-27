using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.AddRoomFishGainCoin)]
public class HHandlerAddRoomFishGainCoin : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        if (!pMsg.GetString("status").Equals("ok")) return;

        string uid = pMsg.GetString("uid");
        long fishCoin = pMsg.GetLong("count");

        if (CPlayerMgr.Ins.pOwner == null ||
            !CPlayerMgr.Ins.pOwner.uid.Equals(uid)) return;

        CRoomRecordInfoMgr.Ins.RoomGainCoin = (fishCoin > 0 ? fishCoin : 0);
    }
}
