using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.SetUserFishGan)]
public class HHandlerSetFishGan : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
       
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string szStatus = pMsg.GetString("status");
        if (!szStatus.Equals("ok")) return;

        string uid = pMsg.GetString("uid");
        int ganId = pMsg.GetInt("ganId");

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null ||
            pPlayer.pFishGanPack == null) return;

        pPlayer.nFishGanAvatarId = ganId;
        pPlayer.RefreshProAdd();

        //不在游戏界面直接返回
        if (CGameColorFishMgr.Ins.pMap == null) return;

        CPlayerUnit pPlayerUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if (pPlayerUnit == null)
        {
            pPlayerUnit = CPlayerMgr.Ins.GetActiveUnit(uid);
        }

        if (pPlayerUnit == null) return;

        pPlayerUnit.InitFishGan(pPlayer.nFishGanAvatarId);
    }
}
