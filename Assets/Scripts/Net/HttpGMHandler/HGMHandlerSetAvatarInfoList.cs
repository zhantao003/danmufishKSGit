using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.DebugSetAvatarInfoList)]
public class HGMHandlerSetAvatarInfoList : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        CLocalNetArrayMsg arrMsgList = pMsg.GetNetMsgArr("list");
        List<CGMAvatarInfo> listInfo = new List<CGMAvatarInfo>();
        for (int i = 0; i < arrMsgList.GetSize(); i++)
        {
            CLocalNetMsg msgSlot = arrMsgList.GetNetMsg(i);
            CGMAvatarInfo pInfo = new CGMAvatarInfo();
            pInfo.avatarId = msgSlot.GetInt("avatarId");
            pInfo.partId = msgSlot.GetInt("partId");
            pInfo.price = msgSlot.GetLong("price");

            listInfo.Add(pInfo);
        }

        UIGMAvatarConfig uiAvatar = GameObject.FindObjectOfType<UIGMAvatarConfig>();
        if (uiAvatar != null)
        {
            uiAvatar.Init(listInfo);
        }
    }
}
