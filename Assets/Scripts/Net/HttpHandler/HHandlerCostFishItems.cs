using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.CostFishItems)]
public class HHandlerCostFishItems : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string uid = pMsg.GetString("uid");
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null) return;

        long fishBait = pMsg.GetLong("fishBait");
        long fishFreeBait = pMsg.GetLong("fishFreeBait");
        long fishGan = pMsg.GetLong("fishGan");
        long fishPiao = pMsg.GetLong("fishPiao");
        long fishLun = pMsg.GetLong("fishLun");

        pPlayer.nlBaitCount = fishBait;
        pPlayer.nlFreeBaitCount = fishFreeBait;
        //pPlayer.nlRobCount = fishGan;
        //pPlayer.nlBuoyCount = fishPiao;
        pPlayer.nlFeiLunCount = fishLun;

        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if (pUnit != null)
        {
            pUnit.dlgChgGift?.Invoke();
            if (pPlayer.CheckHaveGift())
            {
                pUnit.ClearExitTick();
            }
            else
            {
                pUnit.ResetExitTick();
            }
        }
    }
}
