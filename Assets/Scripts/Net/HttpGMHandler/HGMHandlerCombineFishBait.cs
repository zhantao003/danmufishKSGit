using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HGMHandlerCombineFishBait : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        UIGMConsole uiGMConsole = GameObject.FindObjectOfType<UIGMConsole>();
        if (uiGMConsole == null) return;

        uiGMConsole.uiLabelContent.text = pMsg.GetData();
    }
}
