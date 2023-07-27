using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.SetUserBoat)]
public class HHandlerSetUserBoat : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string szStatus = pMsg.GetString("status");
        if (!szStatus.Equals("ok")) return;

        string uid = pMsg.GetString("uid");
        int boatId = pMsg.GetInt("boatId");

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null ||
            pPlayer.pBoatPack == null) return;

        //�ж��Ƿ񱳰���Ľ�ɫ
        CPlayerBoatInfo pAvatarInfo = pPlayer.pBoatPack.GetInfo(boatId);
        if (pAvatarInfo == null) return;

        pPlayer.nBoatAvatarId = boatId;

        //������Ϸ����ֱ�ӷ���
        if (CGameColorFishMgr.Ins.pMap == null) return;

        CPlayerUnit pPlayerUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if (pPlayerUnit == null)
        {
            pPlayerUnit = CPlayerMgr.Ins.GetActiveUnit(uid);
        }

        if (pPlayerUnit == null) return;

        if(pPlayerUnit.pMapSlot!=null)
        {
            pPlayerUnit.pMapSlot.SetBoat(pPlayer.nBoatAvatarId, pPlayerUnit, pPlayer.guardLevel, true);
        }
    }
}
