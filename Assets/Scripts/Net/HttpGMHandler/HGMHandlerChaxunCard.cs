using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst_Debug.DEBUG_GetCardStatus)]
public class HGMHandlerChaxunCard : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        UIGMConsoleCard.SetResContent(pMsg.GetData());
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        UIGMConsoleCard.SetResContent(pMsg.GetData());
    }
}
