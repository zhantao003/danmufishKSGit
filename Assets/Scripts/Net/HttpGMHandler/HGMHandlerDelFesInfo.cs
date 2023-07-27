using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.DebugRemoveFesInfo)]
public class HGMHandlerDelFesInfo : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
         
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string status = pMsg.GetString("status");
        if (!status.Equals("ok")) return;

        UIGMFesInfoConfig uiGMConsole = GameObject.FindObjectOfType<UIGMFesInfoConfig>();
        if (uiGMConsole == null) return;

        long packId = pMsg.GetLong("packId");
        int idx = pMsg.GetInt("idx");

        uiGMConsole.RemoveSlotByPackIdx(packId, idx);
    }
}
