using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.DebugDelAvatarInfo)]
public class HGMHandlerDelAvatarInfo : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
       
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string status = pMsg.GetString("status");
        if (!status.Equals("ok")) return;

        int avatarId = pMsg.GetInt("avatarId");
        UIGMAvatarConfig uiAvatar = GameObject.FindObjectOfType<UIGMAvatarConfig>();
        if (uiAvatar != null)
        {
            uiAvatar.DeleteSlot(avatarId);
        }
    }
}
