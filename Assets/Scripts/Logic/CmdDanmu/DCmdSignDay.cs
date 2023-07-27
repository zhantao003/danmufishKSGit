using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CDanmuCmdAttrite(CDanmuEventConst.SignDay)]
public class DCmdSignDay : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        string uid = dm.uid.ToString();

        CPlayerBaseInfo pPlayerInfo = CPlayerMgr.Ins.GetPlayer(uid);

        if (pPlayerInfo == null)
        {
            //先登录
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
            //    new HHandlerLoginViewer(dm.userName, dm.userFace, "", null, OnLoginSuc),
            //    pParamLogin);

            CPlayerNetHelper.Login(uid, dm.nickName, dm.headIcon, CPlayerBaseInfo.EMUserType.Guanzhong,
                                   dm.fanLv, dm.fanName, dm.fanEquip, dm.vipLv,
                                   new HHandlerLoginViewer(dm.nickName, dm.headIcon, "", null, OnLoginSuc));
        }
        else
        {
            OnLoginSuc(pPlayerInfo, CGameColorFishMgr.EMJoinType.Normal);
        }
    }

    void OnLoginSuc(CPlayerBaseInfo info, CGameColorFishMgr.EMJoinType joinType)
    {
        //普通签到
        CHttpParam pParamSign = new CHttpParam
        (
            new CHttpParamSlot("uid", info.uid.ToString())
        );
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.ViewerSignIn, pParamSign);

        //舰长签到
        //白名单限制：暂时只有炫彩哥房间有
        if (info.guardLevel > 0)
        {
            CHttpParam pParamVipSign = new CHttpParam(
               new CHttpParamSlot("uid", info.uid.ToString()),
               new CHttpParamSlot("roomId", CDanmuSDKCenter.Ins.szRoomId.ToString()),
               new CHttpParamSlot("guardianLv", info.guardLevel.ToString())
            );
            CHttpMgr.Instance.SendHttpMsg(CHttpConst.ViewerVipSignIn, pParamVipSign, 0, true);
        }
    }
}