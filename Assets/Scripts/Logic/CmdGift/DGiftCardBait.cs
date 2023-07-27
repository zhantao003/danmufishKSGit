using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CDanmuGiftAttrite(CDanmuGiftConst.CardBait)]
public class DGiftCardBait : CDanmuGiftAction
{
    public override void DoAction(CDanmuGift dm)
    {
        long nGiftNum = dm.giftNum;
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
                                     GetFishBait(nGiftNum, info);
                                 }));
        }
        else
        {
            GetFishBait(nGiftNum, pPlayer);
        }
        
        ////�����������
        //if(CPlayerMgr.Ins.pOwner!= null && CPlayerMgr.Ins.pOwner.uid != uid)
        //{
        //    GetFishBait(nGiftNum, CPlayerMgr.Ins.pOwner);
        //}
    }

    void GetFishBait(long num, CPlayerBaseInfo info)
    {
        //CHttpParam pReqParams = new CHttpParam
        //(
        //    new CHttpParamSlot("uid", info.uid.ToString()),
        //    new CHttpParamSlot("fishItem", num.ToString())
        //);
        //CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddBait, pReqParams, 10, true);
    }
}
