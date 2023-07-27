using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HHandlerUserExchangeBoat : INetEventHandler
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
        int itemId = pMsg.GetInt("itemId");
        long count = pMsg.GetLong("count");

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null) return;

        CPlayerBoatInfo pBoatInfo = new CPlayerBoatInfo();
        pBoatInfo.nBoatId = boatId;
        pPlayer.pBoatPack.AddInfo(pBoatInfo);

        pPlayer.pMatPack.SetItem(itemId, count);

        UIUserGetAvatar uiGetAvatar = UIManager.Instance.GetUI(UIResType.UserGetAvatarTip) as UIUserGetAvatar;
        uiGetAvatar.AddBoat(pPlayer, pBoatInfo, UIUserGetAvatar.EMGetFunc.Exchange);
    }
}
