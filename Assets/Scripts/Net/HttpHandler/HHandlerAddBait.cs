using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.AddBait)]
public class HHandlerAddBait : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        string uid = pMsg.GetString("uid");
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer != null)
        {
            pPlayer.nlBaitCount = 0;
            pPlayer.nlFreeBaitCount = 0;
        }

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

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string uid = pMsg.GetString("uid");
        long baitCount = pMsg.GetLong("fishItem");
        long freeFishBait = pMsg.GetLong("freeFishBait");

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        string szStatus = pMsg.GetString("status");
        if (szStatus == "error")
        {
            if (pPlayer != null)
            {
                pPlayer.nlBaitCount = 0;
                pPlayer.nlFreeBaitCount = 0;
            }
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
            return;
        }

        if (pPlayer != null)
        {
            pPlayer.nlBaitCount = baitCount;
            pPlayer.nlFreeBaitCount = freeFishBait;
        }

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
        //Debug.Log(uid + "=====Bait Count =====" + baitCount);

    }
}
