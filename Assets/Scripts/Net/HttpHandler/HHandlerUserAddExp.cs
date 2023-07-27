using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.AddUserExp)]
public class HHandlerUserAddExp : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string szStatus = pMsg.GetString("status");
        if (!szStatus.Equals("ok")) return;

        string uid = pMsg.GetString("uid");
        long lv = pMsg.GetLong("lv");
        long exp = pMsg.GetLong("exp");

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if(pPlayer!=null)
        {
            pPlayer.SetUserLv(lv, exp);
        }
    }
}
