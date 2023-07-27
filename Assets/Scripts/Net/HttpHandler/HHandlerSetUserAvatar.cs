using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.SetUserAvatar)]
public class HHandlerSetUserAvatar : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        int code = pMsg.GetInt("errcode");
        if (code == 1)
        {
            //UIToast.Show("数据异常");
        }
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string uid = pMsg.GetString("uid");
        int avatarId = pMsg.GetInt("avatarId");

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null ||
            pPlayer.pAvatarPack == null) return;

        //判断是否背包里的角色
        CPlayerAvatarInfo pAvatarInfo = pPlayer.pAvatarPack.GetInfo(avatarId);
        if (pAvatarInfo == null) return;

        pPlayer.avatarId = avatarId;

        CPlayerUnit pPlayerUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if (pPlayerUnit == null)
        {
            pPlayerUnit = CPlayerMgr.Ins.GetActiveUnit(uid);
        }

        if (pPlayerUnit != null)
        {
            Quaternion pBodyRot = pPlayerUnit.tranBody.localRotation;
            pPlayerUnit.InitAvatar();
            pPlayerUnit.tranBody.localRotation = pBodyRot;
        }

        //if (CGameColorFishMgr.Ins.nCurRateUpLv == 1 &&
        //    UIHelp.emHelpLv == EMHelpLev.Lev4_Chg &&
        //    pPlayerUnit.pInfo.emUserType == CPlayerBaseInfo.EMUserType.Zhubo)
        //{
        //    UIHelp.GoNextHelpLv();
        //}

        //判断是否刷新UI
        if (pPlayer.emUserType == CPlayerBaseInfo.EMUserType.Zhubo)
        {
            UIVtuberAvatar uiVAvatar = UIManager.Instance.GetUI(UIResType.VtuberAvatar) as UIVtuberAvatar;
            if (uiVAvatar != null &&
                uiVAvatar.IsOpen())
            {
                uiVAvatar.RefreshInfo();
            }

            UIRoomSetting uiRoomInfo = UIManager.Instance.GetUI(UIResType.RoomSetting) as UIRoomSetting;
            if (uiRoomInfo != null &&
                uiRoomInfo.IsOpen())
            {
                uiRoomInfo.uiAvatarInfo.SetAvatar(CPlayerMgr.Ins.pOwner.avatarId);
            }
        }
    }
}
