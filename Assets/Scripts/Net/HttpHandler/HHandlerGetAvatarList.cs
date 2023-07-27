using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.GetAvatarList)]
public class HHandlerGetAvatarList : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string uid = pMsg.GetString("uid");
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null) return;

        pPlayer.pAvatarPack.Clear();
        CLocalNetArrayMsg arrList = pMsg.GetNetMsgArr("list");
        for(int i=0; i<arrList.GetSize(); i++)
        {
            CLocalNetMsg msgAvatarSlot = arrList.GetNetMsg(i);
            int avatarId = msgAvatarSlot.GetInt("avatarId");
            int partId = msgAvatarSlot.GetInt("partId");
            long exTime = msgAvatarSlot.GetLong("extime");

            CPlayerAvatarInfo pAvatarInfo = new CPlayerAvatarInfo();
            pAvatarInfo.nAvatarId = avatarId;
            pAvatarInfo.nPart = partId;
            pAvatarInfo.nExTime = exTime;

            pPlayer.pAvatarPack.AddInfo(pAvatarInfo);
        }

        pPlayer.pAvatarPack.SortAvatarPack();
    }
}
