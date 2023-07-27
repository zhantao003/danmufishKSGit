using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CDanmuCmdAttrite(CDanmuEventConst.Chaxun_Suipian)]
public class DCmdChaxunSuipian : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        return;
        string uid = dm.uid.ToString();
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null)
        {
            //ÏÈµÇÂ¼
            //CHttpParam pParamLogin = new CHttpParam
            //(
            //    new CHttpParamSlot("uid", dm.uid.ToString()),
            //    new CHttpParamSlot("nickName", System.Net.WebUtility.UrlEncode(dm.userName)),
            //    new CHttpParamSlot("headIcon", System.Net.WebUtility.UrlEncode(dm.userFace)),
            //    new CHttpParamSlot("userType", ((int)CPlayerBaseInfo.EMUserType.Guanzhong).ToString()),
            //    new CHttpParamSlot("fansMedalLevel", dm.fansMedalLevel.ToString()),
            //    new CHttpParamSlot("fansMedalName", dm.fansMedalName),
            //    new CHttpParamSlot("fansMedalWearingStatus", dm.fansMedalWearingStatus.ToString()),
            //    new CHttpParamSlot("guardLevel", dm.guardLevel.ToString())
            //);

            //CHttpMgr.Instance.SendHttpMsg(
            //    CHttpConst.LoginViewer,
            //    new HHandlerLoginViewer(dm.userName, dm.userFace, "", null,
            //    delegate (CPlayerBaseInfo info)
            //    {
            //        UIBroadCast.AddCastInfo($"{info.userName}Ê£Óà½ÇÉ«ËéÆ¬\r\n{info.AvatarSuipian}");
            //        //UIBroadCast.AddCastInfo($"<color=#f000ff>{info.userName}</color>Ê£Óà½ÇÉ«ËéÆ¬\r\n<color=#fff000>{info.AvatarSuipian}</color>");
            //    }),
            //    pParamLogin);

            CPlayerNetHelper.Login(uid, dm.nickName, dm.headIcon, CPlayerBaseInfo.EMUserType.Guanzhong,
                                   dm.fanLv, dm.fanName, dm.fanEquip, dm.vipLv,
                                  new HHandlerLoginViewer(dm.nickName, dm.headIcon, "", null,
                delegate (CPlayerBaseInfo info, CGameColorFishMgr.EMJoinType joinType)
                {
                    UIBroadCast.AddCastInfo($"{info.userName}Ê£Óà½ÇÉ«ËéÆ¬\r\n{info.AvatarSuipian}");
                    //UIBroadCast.AddCastInfo($"<color=#f000ff>{info.userName}</color>Ê£Óà½ÇÉ«ËéÆ¬\r\n<color=#fff000>{info.AvatarSuipian}</color>");
                }));
        }
        else
        {
            UIBroadCast.AddCastInfo($"{pPlayer.userName}Ê£Óà½ÇÉ«ËéÆ¬\r\n{pPlayer.AvatarSuipian}");
            //UIBroadCast.AddCastInfo($"<color=#f000ff>{pPlayer.userName}</color>Ê£Óà½ÇÉ«ËéÆ¬\r\n<color=#fff000>{pPlayer.AvatarSuipian}</color>");
        }

        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if (pUnit == null)
        {
            pUnit = CPlayerMgr.Ins.GetActiveUnit(uid);
        }

        if (pUnit != null)
        {
            UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
            if (uiGameInfo == null || !uiGameInfo.IsOpen()) return;

            UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(pUnit.uid);
            if (uiUnitInfo == null) return;

            uiUnitInfo.SetDmContent($"ÎÒ»¹ÓÐ<color=#24CE38>{pPlayer.AvatarSuipian}</color>¸ö×°°çËéÆ¬£¡");
        }
    }
}
