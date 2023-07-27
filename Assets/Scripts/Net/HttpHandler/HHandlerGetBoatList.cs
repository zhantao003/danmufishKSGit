using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.GetBoatList)]
public class HHandlerGetBoatList : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string szStatus = pMsg.GetString("status");
        string uid = pMsg.GetString("uid");

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null) return;

        pPlayer.pBoatPack.Clear();

        if (!szStatus.Equals("empty"))
        {
            CLocalNetArrayMsg arrList = pMsg.GetNetMsgArr("list");
            for (int i = 0; i < arrList.GetSize(); i++)
            {
                CLocalNetMsg msgAvatarSlot = arrList.GetNetMsg(i);
                int boatId = msgAvatarSlot.GetInt("boatId");
                long exTime = msgAvatarSlot.GetInt("exTime");

                CPlayerBoatInfo pBoatInfo = new CPlayerBoatInfo();
                pBoatInfo.nBoatId = boatId;
                pBoatInfo.nExTime = exTime;

                pPlayer.pBoatPack.AddInfo(pBoatInfo);
            }

            pPlayer.pBoatPack.SortBoatPack();
        }
        else
        {
            CPlayerBoatInfo pBoatInfo = new CPlayerBoatInfo();
            pBoatInfo.nBoatId = 101;
            pBoatInfo.nExTime = 0;

            pPlayer.pBoatPack.AddInfo(pBoatInfo);
        }
    }
}
