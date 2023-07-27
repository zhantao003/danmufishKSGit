using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.ViewerSignIn)]
public class HHandlerViewerSignDay : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {

    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string uid = pMsg.GetString("uid");
        long fishCoin = pMsg.GetLong("fishCoin");
        long baitCount = pMsg.GetLong("fishItem");
        long freeFishBait = pMsg.GetLong("freeFishBait");

        CPlayerBaseInfo pPlayerInfo = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayerInfo == null) return;

        pPlayerInfo.nlBaitCount = baitCount;
        pPlayerInfo.nlFreeBaitCount = freeFishBait;
        pPlayerInfo.GameCoins = fishCoin;

        Debug.Log($"玩家：{pPlayerInfo.userName}  签到成功，当前鱼饵数量：{pPlayerInfo.nlBaitCount + +pPlayerInfo.nlFreeBaitCount}");
        UIBroadCast.AddCastInfo($"{pPlayerInfo.userName} 签到成功");
        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if (pUnit != null)
        {
            pUnit.dlgChgGift?.Invoke();
        }

        UIUserGetAvatar uiGetAvatar = UIManager.Instance.GetUI(UIResType.UserGetAvatarTip) as UIUserGetAvatar;
        uiGetAvatar.AddDailySign(pPlayerInfo);

        //UIBroadCast.AddCastInfo($"<color=#f000ff>{pPlayerInfo.userName}</color> 签到成功");
    }
}
