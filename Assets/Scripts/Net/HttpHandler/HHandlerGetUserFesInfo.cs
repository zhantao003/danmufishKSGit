using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.GetUserFesInfo)]
public class HHandlerGetUserFesInfo : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string szStatus = pMsg.GetString("status");
        if (!szStatus.Equals("ok")) return;

        string uid = pMsg.GetString("uid");
        long packId = pMsg.GetLong("pack");
        int curIdx = pMsg.GetInt("curIdx");
        long viewerPoint = pMsg.GetLong("viewerPoint");
        long vtbPoint = pMsg.GetLong("vtbPoint");

        CFishFesPlayerInfo pPlayerFesInfo = CFishFesInfoMgr.Ins.GetPlayerInfo(packId, uid);
        if (pPlayerFesInfo == null)
        {
            pPlayerFesInfo = new CFishFesPlayerInfo();
            pPlayerFesInfo.nUid = uid;
            pPlayerFesInfo.nCurIdx = curIdx;
            pPlayerFesInfo.nPlayerPoint = viewerPoint;
            pPlayerFesInfo.nVtbPoint = vtbPoint;
            CFishFesInfoMgr.Ins.AddFesPlayerInfo(packId, pPlayerFesInfo);
        }
        else
        {
            pPlayerFesInfo.nCurIdx = curIdx;
            pPlayerFesInfo.nPlayerPoint = viewerPoint;
            pPlayerFesInfo.nVtbPoint = vtbPoint;
        }
    }
}
