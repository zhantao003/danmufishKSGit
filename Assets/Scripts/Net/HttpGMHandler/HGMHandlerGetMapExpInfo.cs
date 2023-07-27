using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.DebugGetMapExpInfo)]
public class HGMHandlerGetMapExpInfo : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {

    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        CLocalNetArrayMsg arrMsgList = pMsg.GetNetMsgArr("list");
        List<CMapExpInfo> listInfo = new List<CMapExpInfo>();
        for (int i = 0; i < arrMsgList.GetSize(); i++)
        {
            CLocalNetMsg msgSlot = arrMsgList.GetNetMsg(i);
            CMapExpInfo pInfo = new CMapExpInfo(msgSlot.GetInt("level"), msgSlot.GetInt("exp"));

            listInfo.Add(pInfo);
        }

        UIGMMapExp uiMapExp = GameObject.FindObjectOfType<UIGMMapExp>();
        if (uiMapExp == null) return;
        uiMapExp.Init(listInfo);

    }
}
