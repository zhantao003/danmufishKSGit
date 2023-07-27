using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HHandlerRefreshRolePack : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {

    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string szStatus = pMsg.GetString("status");
        if (!szStatus.Equals("ok")) return;

        string uid = pMsg.GetString("uid");
        int avatarId = pMsg.GetInt("avatarId");
        int partId = pMsg.GetInt("partId");
        long exTime = pMsg.GetInt("exTime");

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null) return;
        CPlayerAvatarInfo pAvatarInfo = new CPlayerAvatarInfo();
        pAvatarInfo.nAvatarId = avatarId;
        pAvatarInfo.nPart = partId;
        pAvatarInfo.nExTime = exTime;
        pPlayer.pAvatarPack.AddInfo(pAvatarInfo);
        pPlayer.pAvatarPack.SortAvatarPack();

    }
}

