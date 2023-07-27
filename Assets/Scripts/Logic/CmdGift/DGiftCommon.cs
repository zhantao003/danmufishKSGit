using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuGiftAttrite(CDanmuGiftConst.CommonGift)]
public class DGiftCommon : CDanmuGiftAction
{
    public override void DoAction(CDanmuGift dm)
    {
        string uid = dm.uid.ToString();
        long nBattery = dm.price;
        long nGiftNum = dm.giftNum;

        //string nVtuberUID = "";
        //if (CPlayerMgr.Ins.pOwner != null)
        //{
        //    nVtuberUID = CPlayerMgr.Ins.pOwner.uid;
        //}

        //判断是否收费礼物
        if (!dm.paid)
        {
            //CHttpMgr.Instance.SendHttpMsg(CHttpConst.GiftFree, pReqParams);

            return;
        }
        else
        {
            long nlAddExp = nBattery;
            bool bIsGameGift = false;

            //记录当前局游戏的电池数(只限于游戏道具)
            Debug.Log("礼物名字：" + dm.giftName);
            if (dm.giftName.Equals(CDanmuGiftConst.CardFeiLun)  ||
                dm.giftName.Equals(CDanmuGiftConst.CardBoom) ||
                dm.giftName.Equals(CDanmuGiftConst.CardFishPack) ||
                dm.giftName.Equals(CDanmuGiftConst.GiftGachaBox) ||
                dm.giftName.Equals(CDanmuGiftConst.CrazyGachaBox))
            {
                bIsGameGift = true;

                //统计礼物流水
                CHttpParam pReqRecordParams = new CHttpParam(
                    new CHttpParamSlot("roomId", CDanmuSDKCenter.Ins.szRoomId),
                    new CHttpParamSlot("giftPrice", nBattery.ToString())
                );

                CHttpMgr.Instance.SendHttpMsg(CHttpConst.RecordGiftInfo, pReqRecordParams);

                CGameColorFishMgr.Ins.CurGameBattery += nlAddExp;

                if (CNPCMgr.Ins != null &&
                    CNPCMgr.Ins.pAuctionUnit != null)
                {
                    //普通拍卖
                    if(CAuctionMgr.Ins != null &&
                       CAuctionMgr.Ins.emAuctionState != CAuctionMgr.EMAuctionState.Normal &&
                       CAuctionMgr.Ins.bAuctionByUID(uid))
                    {
                        CNPCMgr.Ins.pAuctionUnit.AddStayTime((float)CGameColorFishMgr.Ins.pStaticConfig.GetInt("每一个电池减少拍卖时间") * (float)nlAddExp);
                    }
                    //材料拍卖
                    else if (CAuctionMatMgr.Ins != null &&
                             CAuctionMatMgr.Ins.pInfo != null &&
                             CAuctionMatMgr.Ins.emAuctionState != CAuctionMatMgr.EMAuctionState.Normal &&
                             CAuctionMatMgr.Ins.pInfo.uid == uid)
                    {

                    }
                    //CNPCMgr.Ins.pAuctionUnit.AddStayTime((float)CGameColorFishMgr.Ins.pStaticConfig.GetInt("每一个电池减少拍卖时间") * (float)nlAddExp);
                }

                //if (CNPCMgr.Ins != null &&
                //    CNPCMgr.Ins.pMatAuctionUnit != null)
                //{
                //    CNPCMgr.Ins.pMatAuctionUnit.AddStayTime((float)CGameColorFishMgr.Ins.pStaticConfig.GetInt("每一个电池减少拍卖时间") * (float)nlAddExp);
                //}

                UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
                if(gameInfo != null)
                {
                    gameInfo.uiBatteryTarget.AddBatteryByUID(uid, nlAddExp, dm.nickName);
                }

                CPlayerUnit pPlayerUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
                CPlayerBaseInfo playerInfo = CPlayerMgr.Ins.GetPlayer(uid);
                if (playerInfo != null && 
                    pPlayerUnit != null)
                {
                    pPlayerUnit.PlayGiftEffectToTarget((int)nBattery);
                }

                if(!dm.giftName.Equals(CDanmuGiftConst.CrazyGachaBox))
                {
                    if (CCrazyTimeMgr.Ins != null)
                    {
                        CCrazyTimeMgr.Ins.AddBattery(nlAddExp);
                    }
                }
            }
            //else if (CPlayerMgr.Ins.pOwner.uid == 38367534) //炫彩哥直播间所有礼物有效
            //{
            //    if (CCrazyTimeMgr.Ins != null)
            //    {
            //        CCrazyTimeMgr.Ins.AddBattery(nlAddExp);
            //    }
            //}

            //if (CGameColorFishMgr.Ins.pGameConfig.bJoinOwner &&
            //    CGameColorFishMgr.Ins.emState == CGameColorFishMgr.EMState.Gaming)
            //{
            //CGameColorFishMgr.Ins.CurQuestBattery += nlAddExp;

            //Debug.Log("Cur Battery =====" + CGameColorFishMgr.Ins.CurQuestBattery);
            //}

            //玩家增加助力值
            CPlayerBaseInfo pPlayerInfo = CPlayerMgr.Ins.GetPlayer(uid);
            if (pPlayerInfo == null)
            {
                CPlayerNetHelper.AddSceneExp(CDanmuSDKCenter.Ins.szRoomId,
                                                       CDanmuSDKCenter.Ins.szRoomId.ToString(),
                                                       nlAddExp,
                                                       true);

                //CPlayerNetHelper.Login(uid, dm.nickName, dm.headIcon, CPlayerBaseInfo.EMUserType.Guanzhong,
                //                   dm.fanLv, dm.fanName, dm.fanEquip, dm.vipLv,
                //    new HHandlerLoginViewer(dm.nickName, dm.headIcon, "", null, 
                //    delegate (CPlayerBaseInfo info, CGameColorFishMgr.EMJoinType joinType)
                //    {
                //        bool bAddSceneExp = false;
                //        //if (CGameColorFishMgr.Ins.nCurRateUpLv > 1)
                //        //{
                //            if (CGameColorFishMgr.Ins.nCurRateUpLv >= 10)
                //            {
                //                if (bIsGameGift)
                //                {
                //                    bAddSceneExp = true;
                //                }
                //            }
                //            else
                //            {
                //                bAddSceneExp = true;
                //            }
                //        //}

                //        if (bAddSceneExp)
                //        {
                //            CPlayerNetHelper.AddSceneExp(CDanmuSDKCenter.Ins.szRoomId,
                //                                       CDanmuSDKCenter.Ins.szRoomId.ToString(),
                //                                       nlAddExp,
                //                                       true);
                //        }

                //        if (bIsGameGift)
                //        {
                //            CPlayerNetHelper.AddUserExp(uid, nlAddExp * 2);
                //        }
                //    }));
            }
            else
            {
                bool bAddSceneExp = false;

                if (CGameColorFishMgr.Ins != null)
                {
                    if (CGameColorFishMgr.Ins.nCurRateUpLv >= 10)
                    {
                        if (bIsGameGift)
                        {
                            bAddSceneExp = true;
                        }
                    }
                    else
                    {
                        bAddSceneExp = true;
                    }
                }

                if (bAddSceneExp)
                {
                    CPlayerNetHelper.AddSceneExp(CDanmuSDKCenter.Ins.szRoomId,
                                                 CDanmuSDKCenter.Ins.szRoomId.ToString(),
                                                 nlAddExp,
                                                 true);
                }

                //添加用户经验
                if (bIsGameGift)
                {
                    CPlayerNetHelper.AddUserExp(uid, nlAddExp * 2);
                    ///刷新挂机状态
                    CPlayerUnit pPlayerUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
                    if(pPlayerUnit != null)
                    {
                        pPlayerUnit.SetActiveState(true);
                    }
                }
            }

            //创建特效
            //CreateGiftEff(dm.giftName, uid);

            return;
        }
    }

    void CreateGiftEff(string giftName, string uid)
    {
        string szEffPrefab = "";
        bool bToOwner = true;
        //if (giftName.Equals(CDanmuGiftConst.CardDaoluan))
        //{
        //    szEffPrefab = CDanmuGiftEffect.CardDaoluan;
        //}
        //else if (giftName.Equals(CDanmuGiftConst.CardZhichi))
        //{
        //    szEffPrefab = CDanmuGiftEffect.CardZhichi;
        //}
        //else if (giftName.Equals(CDanmuGiftConst.CardGold))
        //{
        //    szEffPrefab = CDanmuGiftEffect.CardGold;
        //    bToOwner = false;
        //}
        //else if (giftName.Equals(CDanmuGiftConst.CardRocket))
        //{
        //    szEffPrefab = CDanmuGiftEffect.CardRocket;
        //    bToOwner = false;
        //}
        //else if (giftName.Equals(CDanmuGiftConst.CardFlg))
        //{
        //    szEffPrefab = CDanmuGiftEffect.CardFlg;
        //}

        if (CHelpTools.IsStringEmptyOrNone(szEffPrefab))
        {
            return;
        }

        //该玩家自己在场上时
        CPlayerUnit pPlayerUnit = null;
        if (CPlayerMgr.Ins.GetActivePlayer(uid) != null)
        {
            pPlayerUnit = CPlayerMgr.Ins.GetActiveUnit(uid);
        }

        if (pPlayerUnit == null)
        {
            pPlayerUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        }

        if (pPlayerUnit != null)
        {
            if (bToOwner)
            {
                if (CPlayerMgr.Ins.pOwner != null)
                {
                    CPlayerUnit pPlayerOwner = CPlayerMgr.Ins.GetActiveUnit(CPlayerMgr.Ins.pOwner.uid);
                    if (pPlayerOwner != null)
                    {
                        CEffectMgr.Instance.CreateEffSync(szEffPrefab + "_throw", pPlayerUnit.tranSelf.position, Quaternion.identity, 0,
                        delegate (GameObject value)
                        {
                            CEffectBeizier pEffBeizier = value.GetComponent<CEffectBeizier>();
                            if (pEffBeizier == null) return;

                            pEffBeizier.SetTarget(pPlayerUnit.tranSelf.position + Vector3.up * 1.5F, pPlayerOwner.tranSelf);
                        });
                    }
                }
            }
            else
            {
                CEffectMgr.Instance.CreateEffSync(szEffPrefab + "_drop", pPlayerUnit.tranSelf.position, Quaternion.identity, 0);
            }
        }
        else
        {
            if (bToOwner)
            {
                if (CPlayerMgr.Ins.pOwner != null)
                {
                    CPlayerUnit pPlayerOwner = CPlayerMgr.Ins.GetActiveUnit(CPlayerMgr.Ins.pOwner.uid);
                    if (pPlayerOwner != null)
                    {
                        CEffectMgr.Instance.CreateEffSync(szEffPrefab + "_drop", pPlayerOwner.tranSelf.position, Quaternion.identity, 0);
                    }
                }
            }
        }
    }
}
