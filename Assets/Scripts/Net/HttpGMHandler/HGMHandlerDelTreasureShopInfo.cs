using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.DebugRemoveTreasureShopInfo)]
public class HGMHandlerDelTreasureShopInfo : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        long itemId = pMsg.GetLong("itemId");
        int itemType = pMsg.GetInt("itemType");

        UIGMTreasureShopConfig uiConfig = GameObject.FindObjectOfType<UIGMTreasureShopConfig>();
        if (uiConfig == null) return;

        uiConfig.RemoveSlot(itemId, itemType);
    }
}
