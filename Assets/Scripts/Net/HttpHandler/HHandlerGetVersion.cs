using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.GetVersion)]
public class HHandlerGetVersion : INetEventHandler
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

        UIMainMenu uiMainMenu = UIManager.Instance.GetUI(UIResType.MainMenu) as UIMainMenu;
        if(uiMainMenu!=null)
        {
            uiMainMenu.uiLabelVersionTip.text = szVersionTip;
        }

        UIRoomSetting uiRoomSetting = UIManager.Instance.GetUI(UIResType.RoomSetting) as UIRoomSetting;
        if(uiRoomSetting!=null)
        {
            uiRoomSetting.uiLabelVersionTip.text = szVersionTip;
        }
        
        //同步公告
        CFishFesInfoMgr.Ins.szBroadContent = szBroadContent;

        //UITreasureInfo uiTreasureInfo = UIManager.Instance.GetUI(UIResType.TreasureInfo) as UITreasureInfo;
        //if(uiTreasureInfo!=null)
        //{
        //    uiTreasureInfo.uiLabelBroadCast.text = CFishFesInfoMgr.Ins.szBroadContent;
        //}

        if (!szVersion.Equals(Application.version))
        {
            UIManager.Instance.OpenUI(UIResType.VersionTip);
            UIVersionTip uiVersion = UIManager.Instance.GetUI(UIResType.VersionTip) as UIVersionTip;
            if(uiVersion!=null)
            {
                uiVersion.SetType(0);
            }
        }
    }
}
