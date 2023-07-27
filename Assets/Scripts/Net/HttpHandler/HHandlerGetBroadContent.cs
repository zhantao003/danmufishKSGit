using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.GetBroadContent)]
public class HHandlerGetBroadContent : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string status = pMsg.GetString("status");
        if (!status.Equals("ok")) return;

        string szBroadContent = pMsg.GetString("broadContent");
        CFishFesInfoMgr.Ins.szBroadContent = szBroadContent;

        UITreasureInfo uiTreasureInfo = UIManager.Instance.GetUI(UIResType.TreasureInfo) as UITreasureInfo;
        if (uiTreasureInfo != null)
        {
            uiTreasureInfo.uiLabelBroadCast.text = CFishFesInfoMgr.Ins.szBroadContent;
        }
    }
}
