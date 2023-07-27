using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CDanmuGiftAttrite(CDanmuGiftConst.CardYuGan)]
public class DGiftCardFishingRod : CDanmuGiftAction
{
    public override void DoAction(CDanmuGift dm)
    {
        long nGiftNum = dm.giftNum;
        string uid = dm.uid.ToString();

        //if (CPlayerMgr.Ins.pOwner.uid == 38367534 &&
        //    nGiftNum >= CGameColorFishMgr.Ins.pStaticConfig.GetInt("送额外礼物的界限"))
        //{
        //    if (dm.guardLevel == 1)
        //    {
        //        nGiftNum = System.Convert.ToInt32(nGiftNum * (1 + CGameColorFishMgr.Ins.pStaticConfig.GetInt("送额外礼物的比率3") * 0.01f));
        //    }
        //    else if (dm.guardLevel == 2)
        //    {
        //        nGiftNum = System.Convert.ToInt32(nGiftNum * (1 + CGameColorFishMgr.Ins.pStaticConfig.GetInt("送额外礼物的比率2") * 0.01f));
        //    }
        //    else if (dm.guardLevel == 3)
        //    {
        //        nGiftNum = System.Convert.ToInt32(nGiftNum * (1 + CGameColorFishMgr.Ins.pStaticConfig.GetInt("送额外礼物的比率") * 0.01f));
        //    }
        //}

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null)
        {
            //先登录
            //CHttpParam pParamLogin = new CHttpParam
            //(
            //    new CHttpParamSlot("uid", uid.ToString()),
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
            //    new HHandlerLoginViewer(dm.userName, dm.userFace, "", null, delegate (CPlayerBaseInfo info)
            //    {
            //        GetFishGan(nGiftNum, info, EMGiftType.fishGan);
            //    }),
            //    pParamLogin);

            CPlayerNetHelper.Login(uid, dm.nickName, dm.headIcon, CPlayerBaseInfo.EMUserType.Guanzhong,
                                   dm.fanLv, dm.fanName, dm.fanEquip, dm.vipLv,
                                new HHandlerLoginViewer(dm.nickName, dm.headIcon, "", null, delegate (CPlayerBaseInfo info, CGameColorFishMgr.EMJoinType joinType)
                                {
                                    GetFishGan(nGiftNum, info, EMGiftType.fishGan);
                                }));
        }
        else
        {
            GetFishGan(nGiftNum, pPlayer, EMGiftType.fishGan);
        }

        ////给主播加鱼竿
        //if (CPlayerMgr.Ins.pOwner != null && CPlayerMgr.Ins.pOwner.uid != uid)
        //{
        //    GetFishGan(nGiftNum, CPlayerMgr.Ins.pOwner, EMGiftType.fishGan);
        //}
    }

    void GetFishGan(long num, CPlayerBaseInfo info, EMGiftType giftType)
    {
        CHttpParam pReqParams = new CHttpParam
        (
            new CHttpParamSlot("uid", info.uid.ToString()),
            new CHttpParamSlot("itemType", giftType.ToString()),
            new CHttpParamSlot("count", num.ToString())
        );
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddGiftCount, pReqParams, 10, true);
    }
}