using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HGMHandlerGetTreasureShopList : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
       
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        CLocalNetArrayMsg arrMsgContent = pMsg.GetNetMsgArr("list");
        if (arrMsgContent == null) return;

        List<CGMTreasuresShopBaseInfo> listTreasureShopInfos = new List<CGMTreasuresShopBaseInfo>();
        for (int i = 0; i < arrMsgContent.GetSize(); i++)
        {
            CLocalNetMsg msgInfo = arrMsgContent.GetNetMsg(i);
            CGMTreasuresShopBaseInfo pInfo = new CGMTreasuresShopBaseInfo();
            pInfo.nPrice = msgInfo.GetLong("price");
            pInfo.nItemType = msgInfo.GetInt("itemType");
            pInfo.nItemId = msgInfo.GetInt("itemId");
            listTreasureShopInfos.Add(pInfo);
        }

        UIGMTreasureShopConfig uiTreasureShopConfig = GameObject.FindObjectOfType<UIGMTreasureShopConfig>();
        if (uiTreasureShopConfig != null)
        {
            uiTreasureShopConfig.Init(listTreasureShopInfos);
        }
    }
}
