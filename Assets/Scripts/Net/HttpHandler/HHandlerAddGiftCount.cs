using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.AddGiftCount)]
public class HHandlerAddGiftCount : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        string uid = pMsg.GetString("uid");
        string szItemType = pMsg.GetString("itemType");
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer != null)
        {
            if(szItemType == EMGiftType.fishGan.ToString())
            {
                pPlayer.nlBaitCount = 0;
            }
            else if (szItemType == EMGiftType.fishPiao.ToString())
            {
                pPlayer.nlBaitCount = 0;
            }
            else if (szItemType == EMGiftType.fishLun.ToString())
            {
                pPlayer.nlFeiLunCount = 0;
            }
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
        string szStatus = pMsg.GetString("status");
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if (szStatus == "error")
        {
            string szItemType = pMsg.GetString("itemType");
            if (pPlayer != null)
            {
                if (szItemType == EMGiftType.fishGan.ToString())
                {
                    pPlayer.nlBaitCount = 0;
                }
                else if (szItemType == EMGiftType.fishPiao.ToString())
                {
                    pPlayer.nlBaitCount = 0;
                }
                else if (szItemType == EMGiftType.fishLun.ToString())
                {
                    pPlayer.nlFeiLunCount = 0;
                }
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
        long fishGanCount = pMsg.GetLong("fishGan");
        long fishPiaoCount = pMsg.GetLong("fishPiao");
        long fishXianCount = pMsg.GetLong("fishXian");
        long fishLunCount = pMsg.GetLong("fishLun");
        if (pPlayer != null)
        {
            //pPlayer.nlBaitCount = fishGanCount;
            pPlayer.nlFeiLunCount = fishLunCount;
            //pPlayer.nlBuoyCount = fishPiaoCount;
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
        //Debug.Log(fishGanCount + "=====" + fishPiaoCount + "====" + fishLunCount);

    }
}
