using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.ExchangeAvatar)]
public class HHandlerExchangeAvatar : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        int code = pMsg.GetInt("errcode");
        if(code == 1)
        {
            Debug.Log("碎片不足");
        }
        else if(code == 2)
        {
            Debug.Log("角色不存在");
        }
        else if(code ==3 )
        {
            Debug.Log("已经兑换过了");
        }
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string uid = pMsg.GetString("uid");
        long avatarFragments = pMsg.GetLong("avatarFragments");
        int avatarId = pMsg.GetInt("avatarId");
        int partId = pMsg.GetInt("partId");
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null) return;
        pPlayer.AvatarSuipian = avatarFragments;

        CPlayerAvatarInfo pAvatarInfo = new CPlayerAvatarInfo();
        pAvatarInfo.nAvatarId = avatarId;
        pAvatarInfo.nPart = partId;
        pPlayer.pAvatarPack.AddInfo(pAvatarInfo);
        pPlayer.pAvatarPack.SortAvatarPack();

        UIUserGetAvatar uiGetAvatar = UIManager.Instance.GetUI(UIResType.UserGetAvatarTip) as UIUserGetAvatar;
        if (uiGetAvatar != null)
        {
            uiGetAvatar.AddInfo(pPlayer, pAvatarInfo, UIUserGetAvatar.EMGetFunc.Exchange);
        }

        if(pPlayer.emUserType == CPlayerBaseInfo.EMUserType.Zhubo)
        {
            if (uid != CPlayerMgr.Ins.pOwner.uid) return;

            //判断主播是否打开兑换UI
            UIGetRole getRole = UIManager.Instance.GetUI(UIResType.GetRole) as UIGetRole;
            if(getRole != null)
            {
                getRole.Refresh();
            }
        }
    }
}
