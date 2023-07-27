using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.DebugGetFishGanInfoList)]
public class HGMHandlerGetFishGanInfoList : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        CLocalNetArrayMsg arrMsgList = pMsg.GetNetMsgArr("list");
        List<CGMFishGanInfo> listInfo = new List<CGMFishGanInfo>();

        for (int i = 0; i < arrMsgList.GetSize(); i++)
        {
            CLocalNetMsg msgSlot = arrMsgList.GetNetMsg(i);
            CGMFishGanInfo pInfo = new CGMFishGanInfo();
            pInfo.ganId = msgSlot.GetInt("ganId");
            pInfo.itemNum = msgSlot.GetLong("price");
            pInfo.itemId = msgSlot.GetInt("itemId");

            listInfo.Add(pInfo);
        }

        UIGMFishGanConfig uiBoat = GameObject.FindObjectOfType<UIGMFishGanConfig>();
        if (uiBoat != null)
        {
            uiBoat.Init(listInfo, 1f);
        }
    }
}
