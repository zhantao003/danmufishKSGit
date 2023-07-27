using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CHttpEvent(CHttpConst.ViewerVipSignIn)]
public class HHandlerVipSignDay : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string szStatus = pMsg.GetString("status");
        if (!szStatus.Equals("ok")) return;

        string uid = pMsg.GetString("uid");
        int itemPack = pMsg.GetInt("add");
        int addTreasurePoint = pMsg.GetInt("addTreasure");
        long fishGan = pMsg.GetLong("fishGan");
        long fishPiao = pMsg.GetLong("fishPiao");
        long fishLun = pMsg.GetLong("fishLun");
        long fishBait = pMsg.GetLong("fishBait");
        long treasurePoint = pMsg.GetLong("treasurePoint");

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer != null)
        {
            pPlayer.nlBaitCount = fishBait;
            //pPlayer.nlRobCount = fishGan;
            //pPlayer.nlBuoyCount = fishPiao;
            pPlayer.nlFeiLunCount = fishLun;
            pPlayer.TreasurePoint = treasurePoint;
        }
        else
        {
            return;
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

        UIUserGetAvatar uiGetAvatar = UIManager.Instance.GetUI(UIResType.UserGetAvatarTip) as UIUserGetAvatar;
        uiGetAvatar.AddVipDailySign(pPlayer, itemPack, addTreasurePoint, UIUserGetAvatar.EMGetFunc.Sign);
    }
}
