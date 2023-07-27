using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HGMHandlerSetVersion : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        UIGMConsole uiGMConsole = GameObject.FindObjectOfType<UIGMConsole>();
        if (uiGMConsole == null) return;

        uiGMConsole.uiLabelContent.text = pMsg.GetData();

        //string status = pMsg.GetString("status");
        //if (!status.Equals("ok")) return;

        //string szVersion = pMsg.GetString("version");

        //if(!szVersion.Equals(Application.version))
        //{
        //    UIMsgBox.Show("", "游戏有版本更新，建议更新后再游玩", UIMsgBox.EMType.YesNo, delegate ()
        //    {
        //        Application.OpenURL("https://play-live.bilibili.com/details/1659814658645");
        //    });
        //}
    }
}
