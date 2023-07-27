using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HGMHandlerGetUserExpConfigList : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        UIGMFishUserLvConfig uiConfig = GameObject.FindObjectOfType<UIGMFishUserLvConfig>();
        if (uiConfig == null) return;

        List<CGMFishUserLvConfig> listInfos = new List<CGMFishUserLvConfig>();
        CLocalNetArrayMsg arrContent = pMsg.GetNetMsgArr("list");
        for (int i = 0; i < arrContent.GetSize(); i++)
        {
            CLocalNetMsg msgContent = arrContent.GetNetMsg(i);
            long lv = msgContent.GetLong("level");
            long exp = msgContent.GetLong("exp");

            CGMFishUserLvConfig pInfo = new CGMFishUserLvConfig();
            pInfo.lv = lv;
            pInfo.exp = exp;
            listInfos.Add(pInfo);
        }

        listInfos.Sort((x, y) => x.lv.CompareTo(y.lv));
        uiConfig.Init(listInfos);
    }
}
