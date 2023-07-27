using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.DebugAddGachaInfo)]
public class HGMHandlerAddGachaInfo : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        UIGMConsole uiGMConsole = GameObject.FindObjectOfType<UIGMConsole>();
        if (uiGMConsole == null) return;

        long modelId = pMsg.GetLong("modelId");

        uiGMConsole.uiLabelContent.text = $"设定 抽奖池{modelId} 成功";
    }
}
