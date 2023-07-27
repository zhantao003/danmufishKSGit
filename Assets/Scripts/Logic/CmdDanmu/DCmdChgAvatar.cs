using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuCmdAttrite(CDanmuEventConst.ChgAvatar)]
public class DCmdChgAvatar : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        string uid = dm.uid.ToString();
        string szContent = dm.content.Trim();
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null || pPlayer.pAvatarPack == null) return;

        SetAvatar(szContent, pPlayer);
    }


    void SetAvatar(string content, CPlayerBaseInfo player)
    {
        try
        {
            int nLen = content.Length;
            int nCmdIdx = content.IndexOf(CDanmuEventConst.ChgAvatar) + CDanmuEventConst.ChgAvatar.Length;
            //if (content.IndexOf(CDanmuEventConst.ChgAvatar) == -1)
            //{
            //    nCmdIdx = content.IndexOf(CDanmuEventConst.ChgAvatar2) + CDanmuEventConst.ChgAvatar2.Length;
            //}
            content = content.Substring(nCmdIdx, nLen - nCmdIdx).Trim();

            int nAvatarId = 0;
            if (CHelpTools.IsStringEmptyOrNone(content))
            {
                CPlayerAvatarInfo pAvatarInfo = player.pAvatarPack.GetNextAvatar(player.avatarId);
                if (pAvatarInfo == null) return;
                nAvatarId = pAvatarInfo.nAvatarId;
            }
            else
            {
                if (!int.TryParse(content, out nAvatarId))
                {
                    Debug.Log("异常换装角色ID：" + content);
                    return;
                }
            }
            if (nAvatarId <= 0) return;

            Debug.Log("请求更换角色:" + nAvatarId);

            CHttpParam pReqParams = new CHttpParam(
                new CHttpParamSlot("uid", player.uid.ToString()),
                new CHttpParamSlot("isVtb", ((player.emUserType == CPlayerBaseInfo.EMUserType.Zhubo) ? 1 : 0).ToString()),
                new CHttpParamSlot("roomId", CDanmuSDKCenter.Ins.szRoomId.ToString()),
                new CHttpParamSlot("avatarId", nAvatarId.ToString())
            );

            CHttpMgr.Instance.SendHttpMsg(CHttpConst.SetUserAvatar, pReqParams);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}
