using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HGMHandlerLoadFesInfo : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        UIGMFesInfoConfig uiGMConsole = GameObject.FindObjectOfType<UIGMFesInfoConfig>();
        if (uiGMConsole == null) return;

        long nPackID = long.Parse(uiGMConsole.uiInputFieldModelId.text);
        List<CGMFesInfo> listInfos = new List<CGMFesInfo>();
        CLocalNetArrayMsg msgListContent = pMsg.GetNetMsgArr("list");
        for(int i=0; i<msgListContent.GetSize(); i++)
        {
            CLocalNetMsg msgSlot = msgListContent.GetNetMsg(i);
            long packId = msgSlot.GetLong("packId");
            if (packId != nPackID) continue;

            CGMFesInfo pInfo = new CGMFesInfo();
            pInfo.nPackId = packId;
            pInfo.nIdx = msgSlot.GetInt("idx");
            pInfo.nPointVtb = msgSlot.GetInt("needVtbPoint");
            pInfo.nPointPlayer = msgSlot.GetInt("needViewerPoint");

            string szGiftContent = msgSlot.GetString("giftDetail").Replace("\\", ""); ;
            CLocalNetArrayMsg arrGiftContent = new CLocalNetArrayMsg(szGiftContent);
            if(arrGiftContent.GetSize() > 0)
            {
                CLocalNetMsg msgGiftContent = arrGiftContent.GetNetMsg(0);
                pInfo.nType = msgGiftContent.GetInt("giftType");
                pInfo.nID = msgGiftContent.GetInt("giftId");
            }
            else
            {
                pInfo.nType = 2;
                pInfo.nID = 10000;
            }

            listInfos.Add(pInfo);
        }

        uiGMConsole.ClearSlots();
        listInfos.Sort((x, y) => x.nIdx.CompareTo(y.nIdx));
        for(int i=0; i<listInfos.Count; i++)
        {
            uiGMConsole.AddNewSlot(listInfos[i], i == (listInfos.Count - 1));
        }
    }
}
