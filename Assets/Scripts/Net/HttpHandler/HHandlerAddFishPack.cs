using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.AddFishItemPack)]
public class HHandlerAddFishPack : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        string szStatus = pMsg.GetString("status");
        if (!szStatus.Equals("ok")) return;

        string uid = pMsg.GetString("uid");
        long baitCount = pMsg.GetLong("baitCount");
        long fishGan = pMsg.GetLong("fishGan");
        long fishPiao = pMsg.GetLong("fishPiao");
        long fishLun = pMsg.GetLong("fishLun");
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer != null)
        {
            //pPlayer.nlBaitCount = baitCount;
            //pPlayer.nlRobCount = fishGan;
            //pPlayer.nlBuoyCount = fishPiao;
            pPlayer.nlBaitCount = baitCount;
            pPlayer.nlFeiLunCount = fishLun;
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
        long baitCount = pMsg.GetLong("baitCount");
        long fishGan = pMsg.GetLong("fishGan");
        long fishPiao = pMsg.GetLong("fishPiao");
        long fishLun = pMsg.GetLong("fishLun");

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        string szStatus = pMsg.GetString("status");
        if (szStatus == "error")
        {
            if (pPlayer != null)
            {
                //pPlayer.nlBaitCount = 0;
                //pPlayer.nlFreeBaitCount = 0;
                //pPlayer.nlRobCount = 0;
                pPlayer.nlBaitCount = 0;
                pPlayer.nlFeiLunCount = 0;
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
            //pPlayer.nlRobCount = fishGan;
            //pPlayer.nlBuoyCount = fishPiao;
            pPlayer.nlFeiLunCount = fishLun;
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
