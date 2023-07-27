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
            Debug.Log("��Ƭ����");
        }
        else if(code == 2)
        {
            Debug.Log("��ɫ������");
        }
        else if(code ==3 )
        {
            Debug.Log("�Ѿ��һ�����");
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

            //�ж������Ƿ�򿪶һ�UI
            UIGetRole getRole = UIManager.Instance.GetUI(UIResType.GetRole) as UIGetRole;
            if(getRole != null)
            {
                getRole.Refresh();
            }
        }
    }
}
