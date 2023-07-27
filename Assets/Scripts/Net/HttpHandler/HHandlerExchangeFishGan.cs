using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HHandlerExchangeFishGan : INetEventHandler
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
        int lv = pMsg.GetInt("lv");
        int exp = pMsg.GetInt("exp");
        int proType = pMsg.GetInt("proType");
        int proAdd = pMsg.GetInt("proAdd");
        int itemId = pMsg.GetInt("itemId");
        long count = pMsg.GetLong("count");

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null) return;

        CPlayerFishGanInfo pGanInfo = new CPlayerFishGanInfo();
        pGanInfo.nGanId = ganId;
        pGanInfo.nLv = lv;
        pGanInfo.nExp = exp;
        pGanInfo.emPro = (EMAddUnitProType)proType;
        pGanInfo.nProAdd = proAdd;
        pPlayer.pFishGanPack.AddInfo(pGanInfo);

        pPlayer.pMatPack.SetItem(itemId, count);

        UIUserGetAvatar uiGetAvatar = UIManager.Instance.GetUI(UIResType.UserGetAvatarTip) as UIUserGetAvatar;
        uiGetAvatar.AddFishGan(pPlayer, pGanInfo, UIUserGetAvatar.EMGetFunc.Exchange);
    }
}
