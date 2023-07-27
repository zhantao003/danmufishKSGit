using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HGMHandlerGetActiveRoomList : INetEventHandler
{
    public int nPage;

    public HGMHandlerGetActiveRoomList(int page)
    {
        nPage = page;
    }

    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        CLocalNetArrayMsg arrInfos = pMsg.GetNetMsgArr("list");

        UIGMSearchRoom uiRoom = GameObject.FindObjectOfType<UIGMSearchRoom>();
        if(uiRoom!=null)
        {
            uiRoom.AddInfo(nPage, arrInfos);
            uiRoom.RefreshPage();
        }
    }
}
