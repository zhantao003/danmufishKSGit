using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.ChaxunDailyLimit)]
public class HHandlerChaxunDailyLimit : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string szStatus = pMsg.GetString("status");
        if (!szStatus.Equals("ok")) return;

        string uid = pMsg.GetString("uid");
        long maxCoin = pMsg.GetLong("maxCoin");
        long curCoin = pMsg.GetLong("curCoin");
        long maxExp = pMsg.GetLong("maxExp");
        long curExp = pMsg.GetLong("curExp");

        CPlayerUnit pPlayerUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if (pPlayerUnit == null) return;

        UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (uiGameInfo == null || !uiGameInfo.IsOpen()) return;

        UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(uid);
        if (uiUnitInfo == null) return;

        string szContent = $"剩余免费可摸积分:{maxCoin - curCoin}";
        uiUnitInfo.SetDmContent(szContent);
    }
}
