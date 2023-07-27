using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HHandlerCreatSolo : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {

    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
        if (pMsg.GetString("status") == "error")
        {
            UIToast.Show("»ý·Ö²»¹»");
            if (roomInfo == null)
                return;

            if (roomInfo.emCurShowType != ShowBoardType.Battle)
            {
                roomInfo.battleRoot.nlCurPrice = 0;
            }
        }
        else
        {
            if (roomInfo == null)
                return;
            string uid = pMsg.GetString("uid");
            CPlayerUnit player = CPlayerMgr.Ins.GetIdleUnit(uid);
            CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
            if (player == null || pPlayer == null)
                return;
            SpecialCastInfo castInfo = new SpecialCastInfo();
            castInfo.playerInfo = pPlayer;
            castInfo.fishInfo = null;

            roomInfo.ShowBattle();
            roomInfo.battleRoot.nMaxCount = 2;
            roomInfo.battleRoot.AddPlayerInfo(castInfo);

            player.SetState(CPlayerUnit.EMState.Battle);
            player.ResetDuelCD();
        }
    }
}
