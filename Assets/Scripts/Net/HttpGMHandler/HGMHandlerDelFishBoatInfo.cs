using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.DebugDelFishBoatInfo)]
public class HGMHandlerDelFishBoatInfo : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string status = pMsg.GetString("status");
        if (!status.Equals("ok")) return;

        int boatId = pMsg.GetInt("boatId");
        UIGMBoatConfig uiAvatar = GameObject.FindObjectOfType<UIGMBoatConfig>();
        if (uiAvatar != null)
        {
            uiAvatar.DeleteSlot(boatId);
        }
    }
}
