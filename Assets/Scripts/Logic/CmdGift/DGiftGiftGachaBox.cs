using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuGiftAttrite(CDanmuGiftConst.GiftGachaBox)]
public class DGiftGiftGachaBox : CDanmuGiftAction
{
    public override void DoAction(CDanmuGift dm)
    {
        string uid = dm.uid.ToString();
        long nGiftNum = dm.giftNum;//dm.giftNum * CGameColorFishMgr.Ins.pStaticConfig.GetInt("炫彩音符增加比例");

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if(pPlayer == null)
        {
            CPlayerNetHelper.Login(uid, dm.nickName, dm.headIcon, CPlayerBaseInfo.EMUserType.Guanzhong,
                                   dm.fanLv, dm.fanName, dm.fanEquip, dm.vipLv,
                    new HHandlerLoginViewer(dm.nickName, dm.headIcon, "", null, delegate (CPlayerBaseInfo info, CGameColorFishMgr.EMJoinType joinType)
                    {
                        BuyGachaBox(nGiftNum, info);
                    }));
        }
        else
        {
            BuyGachaBox(nGiftNum, pPlayer);
        }
    }

    void BuyGachaBox(long num, CPlayerBaseInfo info)
    {
        //Debug.Log("抽奖次数:" + num);
        //CHttpParam pReqParams = new CHttpParam(
        //   new CHttpParamSlot("uid", info.uid.ToString()),
        //   new CHttpParamSlot("modelId", "1"),
        //   new CHttpParamSlot("gachaCount", num.ToString())
        //);

        UIUserGetAvatar uiGetAvatar = UIManager.Instance.GetUI(UIResType.UserGetAvatarTip) as UIUserGetAvatar;
        if (uiGetAvatar != null)
        {
            CSpecialGiftInfo specialGiftInfo = new CSpecialGiftInfo();
            specialGiftInfo.emType = CSpecialGiftInfo.EMGiftType.Item2;
            specialGiftInfo.count = num;
            uiGetAvatar.AddSpecialGiftSlot(info, specialGiftInfo);
        }

        //加入玩家单位
        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(info.uid);
        if (pUnit == null)
        {
            CGameColorFishMgr.Ins.JoinPlayer(info, CGameColorFishMgr.EMJoinType.Gift);

            pUnit = CPlayerMgr.Ins.GetIdleUnit(info.uid);
            if (pUnit == null) return;
        }

        int nSpecFishGanID = 502;
        pUnit.pInfo.nFishGanAvatarId = nSpecFishGanID;
        //强行加入背包
        if(pUnit.pInfo.pFishGanPack.GetInfo(nSpecFishGanID) == null)
        {
            ST_UnitFishGan pTBLGanInfo = CTBLHandlerUnitFishGan.Ins.GetInfo(nSpecFishGanID);

            CPlayerFishGanInfo pGanInfo = new CPlayerFishGanInfo();
            pGanInfo.nGanId = nSpecFishGanID;
            pGanInfo.emPro = pTBLGanInfo.emProType;
            pGanInfo.nProAdd = pTBLGanInfo.nProAddMax;
            pUnit.pInfo.pFishGanPack.AddInfo(pGanInfo);
        }

        pUnit.pInfo.RefreshProAdd();

        //不在游戏界面直接返回
        if (CGameColorFishMgr.Ins.pMap == null) return;

        pUnit.pSpecMgr.AddFishGanTime((int)(CGameColorFishMgr.Ins.pStaticConfig.GetInt("特殊鱼竿时间") * num));
        pUnit.InitFishGan(pUnit.pInfo.nFishGanAvatarId);
    }
}
