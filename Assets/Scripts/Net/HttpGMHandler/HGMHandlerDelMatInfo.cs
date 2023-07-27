using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HGMHandlerDelMatInfo : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
       
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string szStatus = pMsg.GetString("status");
        if (!szStatus.Equals("ok")) return;

        int nItemId = pMsg.GetInt("itemId");

        UIGMFishMatConfig uiMatConfig = GameObject.FindObjectOfType<UIGMFishMatConfig>();
        if (uiMatConfig != null)
        {
            uiMatConfig.DeleteSlot(nItemId);
        }
    }
}
