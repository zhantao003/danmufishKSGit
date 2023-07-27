using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.AddAvatarSuipianArr)]
public class HHandlerAddAvatarSuipianArr : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        throw new System.NotImplementedException();
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        CLocalNetArrayMsg arrData = pMsg.GetNetMsgArr("resData");
        for (int i = 0; i < arrData.GetSize(); i++)
        {
            CLocalNetMsg msgData = arrData.GetNetMsg(i);
            string uid = msgData.GetString("uid");
            long avatarSuipian = msgData.GetLong("avatarFragments");

            CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
            if (pPlayer == null) continue;
            pPlayer.AvatarSuipian = avatarSuipian;
        }
    }
}
