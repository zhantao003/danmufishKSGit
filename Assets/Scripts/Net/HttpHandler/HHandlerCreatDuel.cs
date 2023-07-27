using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HHandlerCreatDuel : INetEventHandler
{
    long nlCostPrice;
    public HHandlerCreatDuel(long costPrice)
    {
        nlCostPrice = costPrice;
    }

    public void OnErrorCode(CLocalNetMsg pMsg)
    {

    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
        if (roomInfo.emCurShowType == ShowBoardType.Battle ||
            roomInfo.emCurShowType == ShowBoardType.SpecialBattle)
        {
            string uid = pMsg.GetString("uid");
            CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);

            //同步玩家鱼币
            CPlayerNetHelper.AddFishCoin(uid,
                                         nlCostPrice,
                                         EMFishCoinAddFunc.Duel,
                                         false);

            return;
        }

        if (pMsg.GetString("status") == "error")
        {
            UIToast.Show("积分不够");
            if (roomInfo == null)
                return;

            if (roomInfo.emCurShowType != ShowBoardType.Battle &&
                roomInfo.emCurShowType != ShowBoardType.SpecialBattle)
            {
                roomInfo.battleRoot.nlCurPrice = 0;
            }
        }
        else
        {
            if (roomInfo == null)
                return;
            string uid = pMsg.GetString("uid");
            long gameCoin = pMsg.GetLong("fishCoin");
            CPlayerUnit player = CPlayerMgr.Ins.GetIdleUnit(uid);
            CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
            if (player == null || pPlayer == null)
                return;
            pPlayer.GameCoins = gameCoin;
            SpecialCastInfo castInfo = new SpecialCastInfo();
            castInfo.playerInfo = pPlayer;
            castInfo.fishInfo = null;
            roomInfo.ShowBattle();
            roomInfo.battleRoot.nMaxCount = 10;
            roomInfo.battleRoot.AddPlayerInfo(castInfo);

            player.SetState(CPlayerUnit.EMState.Battle);
            player.ResetDuelCD();
        }
    }
}
