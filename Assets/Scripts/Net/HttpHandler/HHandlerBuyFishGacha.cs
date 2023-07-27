using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.BuyFishGacha)]
public class HHandlerBuyFishGacha : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        throw new System.NotImplementedException();
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string szStatu = pMsg.GetString("status");
        if(szStatu.Equals("error"))
        {
            UIToast.Show("积分不足");
            return;
        }

        string uid = pMsg.GetString("uid");
        long avatarFragments = pMsg.GetLong("avatarFragments");
        long fishCoin = pMsg.GetLong("fishCoin");
        CLocalNetArrayMsg arrGachaContent = pMsg.GetNetMsgArr("gachaResult");

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer != null)
        {
            pPlayer.AvatarSuipian = avatarFragments;
            pPlayer.GameCoins = fishCoin;
        }

        CGachaInfo pGachaInfo = new CGachaInfo(uid, arrGachaContent);

        //检查是否有新角色
        if (pPlayer != null)
        {
            bool bRefreshAvatarPack = false;
            for (int i = 0; i < pGachaInfo.listGiftInfos.Count; i++)
            {
                CGachaGiftInfo pGiftInfo = pGachaInfo.listGiftInfos[i];

                if (pGiftInfo.resType != 1)
                    continue;

                pGiftInfo.isNew = (pGiftInfo.resType == 1);

                //加入背包
                CPlayerAvatarInfo pAvatarInfo = new CPlayerAvatarInfo();
                pAvatarInfo.nAvatarId = pGiftInfo.avatarId;
                pAvatarInfo.nPart = 0;
                pPlayer.pAvatarPack.AddInfo(pAvatarInfo);

                bRefreshAvatarPack = true;
            }

            if (bRefreshAvatarPack)
                pPlayer.pAvatarPack.SortAvatarPack();
        }

        if (CGameColorFishMgr.Ins.pGachaMgr != null)
        {
            CPlayerUnit playerUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
            CGameColorFishMgr.Ins.pGachaMgr.AddGachaInfo(pGachaInfo, playerUnit);
        }
    }
}
