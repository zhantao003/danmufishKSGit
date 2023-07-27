using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.AddFreeBait)]
public class HHandlerAddFreeBait : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string uid = pMsg.GetString("uid");
        long baitCount = pMsg.GetLong("fishItem");
        long freeFishBait = pMsg.GetLong("freeFishBait");

        CPlayerBaseInfo pPlayerInfo = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayerInfo == null) return;

        pPlayerInfo.nlBaitCount = baitCount;
        pPlayerInfo.nlFreeBaitCount = freeFishBait;

        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if (pUnit != null)
        {
            pUnit.dlgChgGift?.Invoke();
        }
    }
}
