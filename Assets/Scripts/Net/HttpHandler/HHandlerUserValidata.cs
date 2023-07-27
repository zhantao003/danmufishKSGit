using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.Validate)]
public class HHandlerUserValidata : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string szStatus = pMsg.GetString("status");
        if (!szStatus.Equals("ok")) return;

        string uid = pMsg.GetString("uid");
        int crePoint = pMsg.GetInt("crePoint");

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null) return;
        pPlayer.nCreditPoint = crePoint;
        Debug.Log(pPlayer.userName + "--当前验证码积分:" + crePoint);
        //处理逻辑

    }
}
