using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.DebugRemoveFishGanInfo)]
public class HGMHandlerDelFishGanInfo : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string status = pMsg.GetString("status");
        if (!status.Equals("ok")) return;

        int ganId = pMsg.GetInt("ganId");
        UIGMFishGanConfig uiAvatar = GameObject.FindObjectOfType<UIGMFishGanConfig>();
        if (uiAvatar != null)
        {
            uiAvatar.DeleteSlot(ganId);
        }
    }
}
