using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.AddWinnerInfo)]
public class HHandlerAddWinnerInfo : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string status = pMsg.GetString("status");
        if (!status.Equals("ok")) return;

        string uid = pMsg.GetString("uid");
        long ouhuang = pMsg.GetLong("ouhuang");
        long fishWinnerPoint = pMsg.GetLong("fishWinnerPoint");

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null) return;

        pPlayer.nWinnerOuhuang = ouhuang;
        pPlayer.nFishWinnerPoint = fishWinnerPoint;

        if(pPlayer.avatarId == 101)
        {
            pPlayer.RefreshBoatAvatar();
        }

        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if (pUnit == null) return;

        if (pUnit.pMapSlot == null ||
            pUnit.pMapSlot.emType != CMapSlot.EMType.Normal) return;

        pUnit.pMapSlot.SetBoat(pUnit.pInfo.nBoatAvatarId, pUnit, 0);
    }
}
