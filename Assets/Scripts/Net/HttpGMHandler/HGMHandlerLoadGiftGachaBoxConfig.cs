using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.DebugGetFishGachaBoxConfig)]
public class HGMHandlerLoadGiftGachaBoxConfig : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        UIGMGiftGachaBoxConfig uiConfig = GameObject.FindObjectOfType<UIGMGiftGachaBoxConfig>();
        if (uiConfig == null) return;

        CLocalNetArrayMsg arrContent = pMsg.GetNetMsgArr("list");
        for(int i=0; i<arrContent.GetSize(); i++)
        {
            CLocalNetMsg msgContent = arrContent.GetNetMsg(i);
            int modelId = msgContent.GetInt("modelId");
            long baseCount = msgContent.GetLong("baseCount");
            if(modelId == int.Parse(uiConfig.uiInputModelId.text))
            {
                string szGachaList = msgContent.GetString("gachaContent").Replace("\\", "");
                CLocalNetArrayMsg arrGachaList = new CLocalNetArrayMsg(szGachaList);
                List<CGMGiftGachaBoxConfigInfo> listGachaInfos = new List<CGMGiftGachaBoxConfigInfo>();

                if (arrGachaList != null && arrGachaList.GetSize() > 0)
                {
                    for (int j = 0; j < arrGachaList.GetSize(); j++)
                    {
                        CLocalNetMsg msgSlot = arrGachaList.GetNetMsg(j);
                        CGMGiftGachaBoxConfigInfo pInfo = new CGMGiftGachaBoxConfigInfo()
                        {
                            nShopId = msgSlot.GetInt("shopId"),
                            nItemType = msgSlot.GetInt("itemType"),
                            nItemId = msgSlot.GetLong("itemId"),
                            nWeight = msgSlot.GetLong("weight"),
                            nIsRare = msgSlot.GetInt("rare")
                        };
                        listGachaInfos.Add(pInfo);
                    }
                }

                uiConfig.Init(modelId, baseCount, listGachaInfos);
            }
        }
    }
}
