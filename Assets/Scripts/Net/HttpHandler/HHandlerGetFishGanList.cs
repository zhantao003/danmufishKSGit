using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.GetFishGanList)]
public class HHandlerGetFishGanList : INetEventHandler
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

        pPlayer.pFishGanPack.Clear();

        if (szStatus.Equals("ok"))
        {
            CLocalNetArrayMsg arrList = pMsg.GetNetMsgArr("list");
            for (int i = 0; i < arrList.GetSize(); i++)
            {
                CLocalNetMsg msgAvatarSlot = arrList.GetNetMsg(i);
                int ganId = msgAvatarSlot.GetInt("ganId");
                int lv = msgAvatarSlot.GetInt("lv");
                int exp = msgAvatarSlot.GetInt("exp");
                EMAddUnitProType proType = (EMAddUnitProType)msgAvatarSlot.GetInt("proType");
                int proAdd = msgAvatarSlot.GetInt("proAdd");

                CPlayerFishGanInfo pGanInfo = new CPlayerFishGanInfo();
                pGanInfo.nGanId = ganId;
                pGanInfo.nLv = lv;
                pGanInfo.nExp = exp;
                pGanInfo.emPro = proType;
                pGanInfo.nProAdd = proAdd;

                pPlayer.pFishGanPack.AddInfo(pGanInfo);
            }

            pPlayer.pFishGanPack.SortGanPack();
            pPlayer.RefreshProAdd();
        }
        else
        {
            CPlayerFishGanInfo pGanInfo = new CPlayerFishGanInfo();
            pGanInfo.nGanId = 101;
            pGanInfo.nLv = 0;
            pGanInfo.nExp = 0;
            pGanInfo.emPro = 0;
            pGanInfo.nProAdd = 0;

            pPlayer.pFishGanPack.AddInfo(pGanInfo);
        }
    }
}
