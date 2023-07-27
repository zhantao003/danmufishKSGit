using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.StartKongtou)]
public class HHandlerStartKongTou : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string status = pMsg.GetString("status");
        if (!status.Equals("ok")) return;

        if (CCrazyTimeMgr.Ins != null)
        {
            CCrazyTimeMgr.Ins.AddCrazyTime(CCrazyTimeMgr.Ins.fCrazyTimeGacha);
            CCrazyTimeMgr.Ins.AddKongTouTime(CCrazyTimeMgr.Ins.fKongTouTimeGift);
        }
    }
}
