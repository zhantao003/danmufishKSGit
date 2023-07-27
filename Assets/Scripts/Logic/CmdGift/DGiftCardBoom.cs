using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuGiftAttrite(CDanmuGiftConst.CardBoom)]
public class DGiftCardBoom : CDanmuGiftAction
{
    public override void DoAction(CDanmuGift dm)
    {
        long nGiftNum = dm.giftNum * CGameColorFishMgr.Ins.pStaticConfig.GetInt("�ı�ը�����ӱ���"); 
        string uid = dm.uid.ToString();

        //ը���ӽ��
        //long nGetCoin = CGameColorFishMgr.Ins.pStaticConfig.GetInt("ը����ȡ���") * nGiftNum;
        //AddCoin(uid, nGetCoin);
       
        if (CGameColorFishMgr.Ins.pMap == null) return;

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if(pPlayer==null)
        {
            CPlayerNetHelper.Login(uid, dm.nickName, dm.headIcon, CPlayerBaseInfo.EMUserType.Guanzhong,
                                   dm.fanLv, dm.fanName, dm.fanEquip, dm.vipLv,
                                 new HHandlerLoginViewer(dm.nickName, dm.headIcon, "", null, delegate (CPlayerBaseInfo info, CGameColorFishMgr.EMJoinType joinType)
                                 {
                                     GetBomber(nGiftNum, info);
                                 }));
        }
        else
        {
            GetBomber(nGiftNum, pPlayer);
        }
    }

    void GetBomber(long num, CPlayerBaseInfo info)
    {
        //CFishFesInfoMgr.Ins.AddFesPoint(1, num, info);

        ////����ͬ����û���
        //if(CPlayerMgr.Ins.pOwner!=null)
        //{
        //    CFishFesInfoMgr.Ins.AddFesPoint(1, num, CPlayerMgr.Ins.pOwner);
        //}
        long nlGiftCount = num / CGameColorFishMgr.Ins.pStaticConfig.GetInt("�ı�ը�����ӱ���");
        UIUserGetAvatar uiGetAvatar = UIManager.Instance.GetUI(UIResType.UserGetAvatarTip) as UIUserGetAvatar;
        if (uiGetAvatar != null)
        {
            CSpecialGiftInfo specialGiftInfo = new CSpecialGiftInfo();
            specialGiftInfo.emType = CSpecialGiftInfo.EMGiftType.Item1;
            specialGiftInfo.count = nlGiftCount;
            uiGetAvatar.AddSpecialGiftSlot(info, specialGiftInfo);
        }
        //CPlayerNetHelper.AddWinnerInfo(info.uid, nlGiftCount * CGameColorFishMgr.Ins.pStaticConfig.GetInt("ը�����׻ʹ�����"), 0);
        ///����uid��ȡ��ҵ�λ
        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(info.uid);
        if (pUnit == null)
        {
            //Bossս����ֱ���ϴ�
            if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
            {
                CGameBossMgr.Ins.AddWaitPlayer(info);

                ////��������ը��
                //CPlayerUnit pUnitOwner = CPlayerMgr.Ins.GetIdleUnit(CPlayerMgr.Ins.pOwner.uid);
                //if (pUnitOwner != null)
                //{
                //    CGameBossMgr.Ins.AddPlayerBomber(CPlayerMgr.Ins.pOwner.uid, num);

                //    pUnitOwner.AddBoom((int)num);
                //}

                return;
            }

            //if (CPlayerMgr.Ins.dicIdleUnits.Count >= CGameColorFishMgr.Ins.nMaxPlayerNum ||
            //    CGameColorFishMgr.Ins.pMap.GetRandIdleRoot() == null)
            //{
            //    UIToast.Show("��������");
            //    return;
            //}

            CGameColorFishMgr.Ins.JoinPlayer(info, CGameColorFishMgr.EMJoinType.Gift);

            pUnit = CPlayerMgr.Ins.GetIdleUnit(info.uid);
            if (pUnit != null)
            {
                if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
                {
                    CGameBossMgr.Ins.AddPlayerBomber(info.uid, num);
                }

                pUnit.AddBoom((int)num);
                pUnit.nBoomAddRate += (int)num / CGameColorFishMgr.Ins.pStaticConfig.GetInt("�ı�ը�����ӱ���") * CGameColorFishMgr.Ins.pStaticConfig.GetInt("ը������boss����");
            }
        }
        else
        {
            if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
            {
                CGameBossMgr.Ins.AddPlayerBomber(info.uid, num);
            }

            pUnit.AddBoom((int)num);
            pUnit.nBoomAddRate += (int)num / CGameColorFishMgr.Ins.pStaticConfig.GetInt("�ı�ը�����ӱ���") * CGameColorFishMgr.Ins.pStaticConfig.GetInt("ը������boss����");
        }

        if (info.CheckHaveGift())
        {
            pUnit.ClearExitTick();
        }
        else
        {
            pUnit.ResetExitTick();
        }
    }

    void AddCoin(string uid, long add)
    {
        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
            return;
        CPlayerNetHelper.AddFishCoin(uid,
                                     add,
                                     EMFishCoinAddFunc.Pay,
                                     true);
    }
}
