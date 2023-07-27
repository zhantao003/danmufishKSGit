using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.BuyTreasureShopInfo)]
public class HHandlerBuyTreasureItem : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
       
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string szStatus = pMsg.GetString("status");
        if (!szStatus.Equals("ok")) return;

        string uid = pMsg.GetString("uid");
        int itemType = pMsg.GetInt("itemType");
        int itemID = pMsg.GetInt("itemId");
        long treasurePoint = pMsg.GetLong("treasurePoint");

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null) return;

        pPlayer.nWinnerOuhuang = treasurePoint;

        if(itemType == (int)ST_ShopTreasure.EMItemType.Role)
        {
            CPlayerAvatarInfo pAvatarInfo = new CPlayerAvatarInfo();
            pAvatarInfo.nAvatarId = itemID;
            pAvatarInfo.nPart = 0;
            pPlayer.pAvatarPack.AddInfo(pAvatarInfo);
            pPlayer.pAvatarPack.SortAvatarPack();

            UIUserGetAvatar uiGetAvatar = UIManager.Instance.GetUI(UIResType.UserGetAvatarTip) as UIUserGetAvatar;
            if (uiGetAvatar != null)
            {
                uiGetAvatar.AddInfo(pPlayer, pAvatarInfo, UIUserGetAvatar.EMGetFunc.Exchange);
            }
        }
        else if(itemType == (int)ST_ShopTreasure.EMItemType.Boat)
        {
            CPlayerBoatInfo pBoatInfo = new CPlayerBoatInfo();
            pBoatInfo.nBoatId = itemID;
            pPlayer.pBoatPack.AddInfo(pBoatInfo);

            UIUserGetAvatar uiGetAvatar = UIManager.Instance.GetUI(UIResType.UserGetAvatarTip) as UIUserGetAvatar;
            if (uiGetAvatar != null)
            {
                uiGetAvatar.AddBoat(pPlayer, pBoatInfo, UIUserGetAvatar.EMGetFunc.Exchange);
            }    
        }
    }
}
