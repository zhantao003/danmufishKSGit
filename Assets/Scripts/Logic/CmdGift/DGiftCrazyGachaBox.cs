using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuGiftAttrite(CDanmuGiftConst.CrazyGachaBox)]
public class DGiftCrazyGachaBox : CDanmuGiftAction
{
    public override void DoAction(CDanmuGift dm)
    {
        long nGiftNum = dm.giftNum;
        string uid = dm.uid.ToString();
        if (CCrazyTimeMgr.Ins != null)
        {
            CCrazyTimeMgr.Ins.AddCrazyTime(nGiftNum * CCrazyTimeMgr.Ins.fCrazyTimeGacha);
            CCrazyTimeMgr.Ins.AddKongTouTime(nGiftNum * CCrazyTimeMgr.Ins.fKongTouTimeGift);
        }

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null)
        {
            CPlayerNetHelper.Login(uid, dm.nickName, dm.headIcon, CPlayerBaseInfo.EMUserType.Guanzhong,
                                   dm.fanLv, dm.fanName, dm.fanEquip, dm.vipLv,
                                   new HHandlerLoginViewer(dm.nickName, dm.headIcon, "", null, delegate (CPlayerBaseInfo info, CGameColorFishMgr.EMJoinType joinType)
                                   {
                                       ShowCast(nGiftNum, info);
                                   }));
        }
        else
        {
            ShowCast(nGiftNum, pPlayer);
        }
    }

    public void ShowCast(long num, CPlayerBaseInfo info)
    {
        int nChouJiangNum = 0;
        for (int i = 0; i < num; i++)
        {
            if (Random.Range(1, 101) <= CGameColorFishMgr.Ins.pStaticConfig.GetInt("³é½±¸ÅÂÊ2"))
            {
                nChouJiangNum++;
            }
        }
        if (nChouJiangNum > 0)
        {
            CHttpParam pReqParams2 = new CHttpParam(
               new CHttpParamSlot("uid", info.uid.ToString()),
               new CHttpParamSlot("modelId", "3"),
               new CHttpParamSlot("gachaCount", (nChouJiangNum * 10).ToString())
            );
            CHttpMgr.Instance.SendHttpMsg(CHttpConst.BuyGiftGachaBox, new HHandlerShowDraw(EMDrawType.SuperKongTou), pReqParams2, 10, true);
        }

        CSpecialGift specialGift = new CSpecialGift();
        CSpecialGiftInfo specialGiftInfo = new CSpecialGiftInfo();
        specialGiftInfo.count = num;
        specialGiftInfo.emType = CSpecialGiftInfo.EMGiftType.Item3;
        specialGift.baseInfo = info;
        specialGift.giftInfo = specialGiftInfo;
        UICrazyGiftTip.ShowInfo(specialGift);

        //¼Ó·ÉÂÖ
        long curTimeStamp = CGameColorFishMgr.Ins.GetNowServerTime();
        CHttpParam pReqParams = new CHttpParam
        (
            new CHttpParamSlot("uid", info.uid.ToString()),
            new CHttpParamSlot("itemType", "fishLun"),
            new CHttpParamSlot("count", (num * 180).ToString()),
            new CHttpParamSlot("time", curTimeStamp.ToString()),
            new CHttpParamSlot("nonce", CEncryptHelper.AesEncrypt(info.uid.ToString() + CDanmuSDKCenter.Ins.szRoomId.ToString() + curTimeStamp) + CHelpTools.GetRandomString(8, true, true, true, false, ""))
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddGiftCount, pReqParams, CHttpMgr.Instance.nReconnectTimes, true);
    }

}
