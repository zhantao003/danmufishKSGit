using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.AddRoomRareFishRecord)]
public class HHandlerSetRoomRareFish : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
       
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string szCode = pMsg.GetString("status");
        if (!szCode.Equals("ok")) return;

        int fishId = pMsg.GetInt("fishId");
        long count = pMsg.GetLong("count");

        CRoomRecordInfoMgr.Ins.SetFishInfo(fishId, count);
    }
}
