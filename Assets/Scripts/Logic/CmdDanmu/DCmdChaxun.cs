using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuCmdAttrite(CDanmuEventConst.Chaxun)]
public class DCmdChaxun : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        if (CBattleModeMgr.Ins != null &&
            CBattleModeMgr.Ins.emCurState == CBattleModeMgr.EMGameState.End)
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
            //        UICheckIn.ShowInfo(info);
            //    }),
            //    pParamLogin);

            CPlayerNetHelper.Login(uid, dm.nickName, dm.headIcon, CPlayerBaseInfo.EMUserType.Guanzhong,
                                   dm.fanLv, dm.fanName, dm.fanEquip, dm.vipLv,
                                   new HHandlerLoginViewer(dm.nickName, dm.headIcon, "", null,
                                    delegate (CPlayerBaseInfo info, CGameColorFishMgr.EMJoinType joinType)
                                    {
                                        UICheckIn.ShowInfo(info);
                                    }));
        }
        else
        {
            UICheckIn.ShowInfo(pPlayer);
        }
    }
}
