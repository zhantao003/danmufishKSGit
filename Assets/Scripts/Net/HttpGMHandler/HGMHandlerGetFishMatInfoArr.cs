using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HGMHandlerGetFishMatInfoArr : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        CLocalNetArrayMsg arrMsgContent = pMsg.GetNetMsgArr("list");
        if (arrMsgContent == null) return;

        List<CGMFishMatInfo> listMatInfos = new List<CGMFishMatInfo>();
        for(int i=0; i<arrMsgContent.GetSize(); i++)
        {
            CLocalNetMsg msgInfo = arrMsgContent.GetNetMsg(i);
            CGMFishMatInfo pInfo = new CGMFishMatInfo();
            pInfo.nId = msgInfo.GetInt("itemId");
            pInfo.nDailyMax = msgInfo.GetLong("maxCount");
            listMatInfos.Add(pInfo);
        }

        UIGMFishMatConfig uiMatConfig = GameObject.FindObjectOfType<UIGMFishMatConfig>();
        if(uiMatConfig!=null)
        {
            uiMatConfig.Init(listMatInfos);
        }
    }
}
