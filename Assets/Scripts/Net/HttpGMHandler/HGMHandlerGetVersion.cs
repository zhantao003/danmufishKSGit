using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HGMHandlerGetVersion : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string status = pMsg.GetString("status");
        if (!status.Equals("ok")) return;

        string szVersion = pMsg.GetString("version");
        string szVersionTip = pMsg.GetString("versionTip");
        string szBroadContent = pMsg.GetString("broadContent");
        CEncryptHelper.KEY = pMsg.GetString("broadKey");
        CEncryptHelper.IV = pMsg.GetString("broadIv");
    }
}
