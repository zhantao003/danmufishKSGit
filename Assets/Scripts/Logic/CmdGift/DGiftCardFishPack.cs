using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuGiftAttrite(CDanmuGiftConst.CardFishPack)]
public class DGiftCardFishPack : CDanmuGiftAction
{
    public override void DoAction(CDanmuGift dm)
    {
        long nGiftNum = dm.giftNum * CGameColorFishMgr.Ins.pStaticConfig.GetInt("渔具套装增加比例");
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
            CPlayerNetHelper.Login(uid, dm.nickName, dm.headIcon, CPlayerBaseInfo.EMUserType.Guanzhong,
                                   dm.fanLv, dm.fanName, dm.fanEquip, dm.vipLv,
                                new HHandlerLoginViewer(dm.nickName, dm.headIcon, "", null, delegate (CPlayerBaseInfo info, CGameColorFishMgr.EMJoinType joinType)
                                {
                                    GetFishPack(nGiftNum, info);
                                }));
        }
        else
        {
            GetFishPack(nGiftNum, pPlayer);
        }

        ////给主播加鱼饵
        //if (CPlayerMgr.Ins.pOwner != null && CPlayerMgr.Ins.pOwner.uid != uid)
        //{
        //    GetFishPack(nGiftNum, CPlayerMgr.Ins.pOwner);
        //}
    }

    void GetFishPack(long num, CPlayerBaseInfo info)
    {
        UIUserGetAvatar uiGetAvatar = UIManager.Instance.GetUI(UIResType.UserGetAvatarTip) as UIUserGetAvatar;
        if (uiGetAvatar != null)
        {
            CSpecialGiftInfo specialGiftInfo = new CSpecialGiftInfo();
            specialGiftInfo.emType = CSpecialGiftInfo.EMGiftType.Item4;
            specialGiftInfo.count = num / CGameColorFishMgr.Ins.pStaticConfig.GetInt("渔具套装增加比例");
            uiGetAvatar.AddSpecialGiftSlot(info, specialGiftInfo);
        }
        CPlayerNetHelper.AddFishItemPack(info.uid, num, num, num, 0);
    }
}
