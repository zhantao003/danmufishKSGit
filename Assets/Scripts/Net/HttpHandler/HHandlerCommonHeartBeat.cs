using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HHandlerCommonHeartBeat : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string szStatus = pMsg.GetString("status");
        if (!szStatus.Equals("ok")) return;

        string szVersion = pMsg.GetString("version");
        string szBroadContent = pMsg.GetString("broadcontent");
        long serverTimeStamp = pMsg.GetLong("timestamp");

        CGameColorFishMgr.Ins.pHeartInfo = new CGameHeartBeatInfo();
        CGameColorFishMgr.Ins.pHeartInfo.szVersion = szVersion;
        CGameColorFishMgr.Ins.pHeartInfo.szBroadContent = szBroadContent;
        CGameColorFishMgr.Ins.pHeartInfo.nServerTimeStamp = serverTimeStamp;
        CGameColorFishMgr.Ins.pHeartInfo.nRecordTimeStamp = CTimeMgr.NowMillonsSec();

        if(!CGameColorFishMgr.Ins.pHeartInfo.szVersion.Equals(Application.version))
        {
            UIManager.Instance.OpenUI(UIResType.VersionMsgBox);
        }

        //CFishFesInfoMgr.Ins.szBroadContent = CGameColorFishMgr.Ins.pHeartInfo.szBroadContent;

        //UITreasureInfo uiTreasureInfo = UIManager.Instance.GetUI(UIResType.TreasureInfo) as UITreasureInfo;
        //if (uiTreasureInfo != null)
        //{
        //    uiTreasureInfo.uiLabelBroadCast.text = CFishFesInfoMgr.Ins.szBroadContent;
        //}

        //if (!CGameColorFishMgr.Ins.pHeartInfo.szVersion.Equals(Application.version))
        //{
        //    UIManager.Instance.OpenUI(UIResType.VersionTip);
        //    UIVersionTip uiVersion = UIManager.Instance.GetUI(UIResType.VersionTip) as UIVersionTip;
        //    if (uiVersion != null)
        //    {
        //        uiVersion.SetType(1);
        //    }

        //    return;
        //}
    }
}
