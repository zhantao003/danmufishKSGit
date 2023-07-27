using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuGiftAttrite(CDanmuGiftConst.CardFeiLun)]
public class DGiftCardFeiLun : CDanmuGiftAction
{
    public override void DoAction(CDanmuGift dm)
    {
        long nGiftNum = dm.giftNum;
        nGiftNum = nGiftNum * CGameColorFishMgr.Ins.pStaticConfig.GetInt("�������ӱ���");
        string uid = dm.uid.ToString();

        //if (CPlayerMgr.Ins.pOwner.uid == 38367534 &&
        //    nGiftNum >= CGameColorFishMgr.Ins.pStaticConfig.GetInt("�Ͷ�������Ľ���"))
        //{
        //    if (dm.guardLevel == 1)
        //    {
        //        nGiftNum = System.Convert.ToInt32(nGiftNum * (1 + CGameColorFishMgr.Ins.pStaticConfig.GetInt("�Ͷ�������ı���3") * 0.01f));
        //    }
        //    else if (dm.guardLevel == 2)
        //    {
        //        nGiftNum = System.Convert.ToInt32(nGiftNum * (1 + CGameColorFishMgr.Ins.pStaticConfig.GetInt("�Ͷ�������ı���2") * 0.01f));
        //    }
        //    else if (dm.guardLevel == 3)
        //    {
        //        nGiftNum = System.Convert.ToInt32(nGiftNum * (1 + CGameColorFishMgr.Ins.pStaticConfig.GetInt("�Ͷ�������ı���") * 0.01f));
        //    }
        //}

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null)
        {
            CPlayerNetHelper.Login(uid, dm.nickName, dm.headIcon, CPlayerBaseInfo.EMUserType.Guanzhong,
                                   dm.fanLv, dm.fanName, dm.fanEquip, dm.vipLv,
                                 new HHandlerLoginViewer(dm.nickName, dm.headIcon, "", null, delegate (CPlayerBaseInfo info, CGameColorFishMgr.EMJoinType joinType)
                                 {
                                     GetFishLun(nGiftNum, info, EMGiftType.fishLun);
                                 }));
        }
        else
        {
            GetFishLun(nGiftNum, pPlayer, EMGiftType.fishLun);
        }

        ////�������ӷ���
        //if (CPlayerMgr.Ins.pOwner != null && CPlayerMgr.Ins.pOwner.uid != uid)
        //{
        //    GetFishLun((long)(nGiftNum * 0.5F), CPlayerMgr.Ins.pOwner, EMGiftType.fishLun);
        //}
    }

    void GetFishLun(long num, CPlayerBaseInfo info, EMGiftType giftType)
    {
        UIUserGetAvatar uiGetAvatar = UIManager.Instance.GetUI(UIResType.UserGetAvatarTip) as UIUserGetAvatar;
        if (uiGetAvatar != null)
        {
            CSpecialGiftInfo specialGiftInfo = new CSpecialGiftInfo();
            specialGiftInfo.emType = CSpecialGiftInfo.EMGiftType.Item5;
            specialGiftInfo.count = num / CGameColorFishMgr.Ins.pStaticConfig.GetInt("�������ӱ���");
            uiGetAvatar.AddSpecialGiftSlot(info, specialGiftInfo);
        }

        long curTimeStamp = CGameColorFishMgr.Ins.GetNowServerTime();
        CHttpParam pReqParams = new CHttpParam
        (
            new CHttpParamSlot("uid", info.uid.ToString()),
            new CHttpParamSlot("itemType", giftType.ToString()),
            new CHttpParamSlot("count", num.ToString()),
            new CHttpParamSlot("time", curTimeStamp.ToString()),
            new CHttpParamSlot("nonce", CEncryptHelper.AesEncrypt(info.uid.ToString() + CDanmuSDKCenter.Ins.szRoomId.ToString() + curTimeStamp) + CHelpTools.GetRandomString(8, true, true, true, false, ""))
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddGiftCount, pReqParams, CHttpMgr.Instance.nReconnectTimes, true);
    }
}