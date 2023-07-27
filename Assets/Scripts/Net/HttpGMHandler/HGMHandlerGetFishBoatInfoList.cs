using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.DebugGetFishBoatInfoList)]
public class HGMHandlerGetFishBoatInfoList : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        CLocalNetArrayMsg arrMsgList = pMsg.GetNetMsgArr("list");
        List<CGMFishBoatInfo> listInfo = new List<CGMFishBoatInfo>();

        for (int i = 0; i < arrMsgList.GetSize(); i++)
        {
            CLocalNetMsg msgSlot = arrMsgList.GetNetMsg(i);
            CGMFishBoatInfo pInfo = new CGMFishBoatInfo();
            pInfo.boatId = msgSlot.GetInt("boatId");
            pInfo.itemNum = msgSlot.GetLong("price");
            pInfo.itemId = msgSlot.GetInt("itemId");

            listInfo.Add(pInfo);
        }

        UIGMBoatConfig uiBoat = GameObject.FindObjectOfType<UIGMBoatConfig>();
        if (uiBoat != null)
        {
            uiBoat.Init(listInfo, 1f);
        }
    }
}
