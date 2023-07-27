using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CDanmuCmdAttrite(CDanmuEventConst.FesSign)] 
public class DCmdFesSign : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        ////限定炫彩哥直播间
        //if (CPlayerMgr.Ins.pOwner.uid != 38367534)
        //{
        //    return;
        //}

        string uid = dm.uid.ToString();

        if (!CFishFesInfoMgr.Ins.IsFesOn((int)CFishFesInfoMgr.EMFesType.FesDaySign))
        {
            return;
        }

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
            //    new HHandlerLoginViewer(dm.userName, dm.userFace, "", null, OnGetFesGift),
            //    pParamLogin);

            CPlayerNetHelper.Login(uid, dm.nickName, dm.headIcon, CPlayerBaseInfo.EMUserType.Guanzhong,
                                   dm.fanLv, dm.fanName, dm.fanEquip, dm.vipLv,
                                 new HHandlerLoginViewer(dm.nickName, dm.headIcon, "", null, OnGetFesGift));
        }
        else
        {
            OnGetFesGift(pPlayerInfo, CGameColorFishMgr.EMJoinType.Normal);
        }
    }

    void OnGetFesGift(CPlayerBaseInfo player, CGameColorFishMgr.EMJoinType joinType)
    {
        //5是签到活动
        if (!CFishFesInfoMgr.Ins.IsFesOn((int)CFishFesInfoMgr.EMFesType.FesDaySign))
        {
            return;
        }

        CHttpParam pReqParams = new CHttpParam
        (
            new CHttpParamSlot("uid", player.uid.ToString())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.FesDailySignIn, new HHandlerFesSignGift(), pReqParams);
    }
}
