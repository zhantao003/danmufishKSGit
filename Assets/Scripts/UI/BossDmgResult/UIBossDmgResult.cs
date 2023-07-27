using SuperScrollView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRandDropPlayerInfo
{
    public string uid;
    public int weight;
}

public class UIBossDmgResult : UIBase
{
    public LoopGridView mLoopListView;

    public UIBossDmgResultSlot pSelfSlot;

    List<CGameBossRewardInfo> listCurInfos = new List<CGameBossRewardInfo>();

    public GameObject objVictory;
    public GameObject objFail;

    //掉落奖励
    CGameMapDropAvatarSlot pDropAvatarInfo;
    string lUidGetDropPlayer;

    public UIFreeDrawRoot freeDrawRoot;

    public bool bVictory;

    bool bInit = false;

    public void SetInfo(bool bVictory)
    {
        UISpecialCast specialCast = UIManager.Instance.GetUI(UIResType.SpecialCast) as UISpecialCast;
        if(specialCast != null)
        {
            specialCast.Clear();
        }
        List<CPlayerUnit> playerUnits = CPlayerMgr.Ins.GetAllIdleUnit();
        for (int i = 0; i < playerUnits.Count; i++)
        {
            if (playerUnits[i] == null) continue;
            playerUnits[i].SetState(CPlayerUnit.EMState.Idle);
        }
        objVictory.SetActive(bVictory);
        objFail.SetActive(!bVictory);
        if (CGameBossMgr.Ins.pBoss.emBossType == EMBossType.Normal)
        {
            if (bVictory)
            {
                CGameColorFishMgr.Ins.nSpecialBossRate = CGameColorFishMgr.Ins.pStaticConfig.GetInt("隐藏Boss出现概率");
            }
            else
            {
                CGameColorFishMgr.Ins.nSpecialBossRate = 0;
            }
        }
        else if (CGameBossMgr.Ins.pBoss.emBossType == EMBossType.Special)
        {
            CGameColorFishMgr.Ins.nSpecialBossRate = 0;
        }
        if (CGameBossMgr.Ins.pBoss.emBossType == EMBossType.Normal)
        {
            GetResultInfoByNormal(bVictory);
        }
        else if(CGameBossMgr.Ins.pBoss.emBossType == EMBossType.Special)
        {
            GetResultInfoBySpecial(bVictory);
        }
    }

    public void GetResultInfoByNormal(bool victory)
    {
        List<CPlayerUnit> listAllPlayers = CPlayerMgr.Ins.GetAllIdleUnit();
        List<CGameBossRewardInfo> listRewardInfo = new List<CGameBossRewardInfo>();
        
        bVictory = victory;
        //判断是否有掉落船
        pDropAvatarInfo = null;
        lUidGetDropPlayer = "";

        float nBaseRate = 0;

        //玩家按照伤害百分比排序
        listAllPlayers.Sort((x, y) =>
        {
            long nXDmg = 0;
            long nYDmg = 0;
            CGameBossDmgInfo bossXDmgInfo = CGameBossMgr.Ins.GetPlayerDmgInfo(x.uid);
            if (bossXDmgInfo != null)
            {
                nXDmg = bossXDmgInfo.nDmg;
            }
            CGameBossDmgInfo bossYDmgInfo = CGameBossMgr.Ins.GetPlayerDmgInfo(y.uid);
            if (bossYDmgInfo != null)
            {
                nYDmg = bossYDmgInfo.nDmg;
            }
            if (nYDmg < nXDmg)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        });

        if (bVictory) 
            //&& IsDropAvatar())
        {
            for(int i = 0;i < listAllPlayers.Count;i++)
            {
                if (listAllPlayers[i] == null) continue;
                CGameBossDmgInfo bossDmgInfo = CGameBossMgr.Ins.GetPlayerDmgInfo(listAllPlayers[i].uid);
                long nDmg = 0;
                if(bossDmgInfo != null)
                {
                    nDmg = bossDmgInfo.nDmg;
                }
                CGameMapDropAvatarSlot dropAvatarInfo = null;
                float fDmgRate = (float)nDmg / (float)CGameBossMgr.Ins.pBoss.nHPMax;
                if (fDmgRate >= 0.09F)
                {
                    nBaseRate = (float)CGameColorFishMgr.Ins.pStaticConfig.GetInt("玩家打Boss基础掉率");
                    //int nRateLerp = 0;
                    //if (fDmgRate > 0.09F)
                    //{
                    //    nRateLerp = (int)CHelpTools.FloatRound((fDmgRate - 0.1f) * 50f * 100f);
                    //    nBaseRate += nRateLerp;

                    //    if (nBaseRate > CGameColorFishMgr.Ins.pStaticConfig.GetInt("Boss基础最大掉率"))
                    //    {
                    //        nBaseRate = CGameColorFishMgr.Ins.pStaticConfig.GetInt("Boss基础最大掉率");
                    //    }
                    //}
                    //else
                    //{
                    //    nRateLerp = (int)CHelpTools.FloatRound((0.1f - fDmgRate) * 80f * 100f);
                    //    nBaseRate -= nRateLerp;
                    //    if (nBaseRate < 0)
                    //    {
                    //        nBaseRate = 0;
                    //    }
                    //}
                    int nBoomRate = 0;
                    if(listAllPlayers[i].nBoomAddRate >= 
                       CGameColorFishMgr.Ins.pStaticConfig.GetInt("炸弹必出货个数") * 
                       CGameColorFishMgr.Ins.pStaticConfig.GetInt("炸弹增加boss掉率"))
                    {
                        nBoomRate = 10000;
                    }
                    else
                    {
                        nBoomRate = listAllPlayers[i].nBoomAddRate;
                    }
                    
                    //炸弹增加boss掉率
                    nBaseRate += nBoomRate;
                    if (nBaseRate > 0)
                    {
                        int nRandomValue = Random.Range(0, 10001);
                        //Debug.LogError("伤害占比:" + (fDmgRate * 100f).ToString("f2") + "%基础权重:" + (nBaseRate - listAllPlayers[i].nBoomAddRate) + "炸弹权重:" + listAllPlayers[i].nBoomAddRate + "%随机获取值:" + nRandomValue);
                        //Debug.LogError("伤害占比:" + (fDmgRate * 100f).ToString("f2") + "%随机获取值:" + nRandomValue);
                        CPlayerBaseInfo playerGetDrop = CPlayerMgr.Ins.GetPlayer(listAllPlayers[i].uid);
                        if (nRandomValue <= nBaseRate)
                        {
                            if (playerGetDrop != null)
                            {
                                dropAvatarInfo = RandomDropAvatarByNormal(playerGetDrop);
                            }
                        }
                        if (dropAvatarInfo == null)
                        {
                        }
                        else
                        {
                            if (dropAvatarInfo.emDropType == EMDropType.Boat &&
                                playerGetDrop.pBoatPack.GetInfo(dropAvatarInfo.nId) == null)
                            {
                                CPlayerNetHelper.SendUserBoat(playerGetDrop.uid, dropAvatarInfo.nId, new HHandlerGetDropBoat());
                            }
                        }
                    }
                }
                CGameBossRewardInfo bossRewardInfo = new CGameBossRewardInfo();
                bossRewardInfo.nUid = listAllPlayers[i].uid;
                bossRewardInfo.nDailyGet = 1;
                bossRewardInfo.dropAvatarInfo = dropAvatarInfo;
                listRewardInfo.Add(bossRewardInfo);
            }
        }

        long nlBossMaxHP = 0;
        CBoss304 boss304 = CGameBossMgr.Ins.pBoss as CBoss304;
        if (boss304 != null)
        {
            nlBossMaxHP = boss304.nHPMax;
            for (int i = 0;i < boss304.pWeaks.Length;i++)
            {
                nlBossMaxHP += boss304.pWeaks[i].nHPMax;
            }
        }
        else
        {
            nlBossMaxHP = CGameBossMgr.Ins.pBoss.nHPMax;
        }

        bool bKillBoss = CGameBossMgr.Ins.CheckKillBoss();

        int nLastAddValue = 999;
        int nLastAddIndex = 0;
        int nTotalValue = 0;
        for (int i = 0; i < listAllPlayers.Count; i++)
        {
            if (listAllPlayers[i] == null) continue;
            float nlDmgRate = 0;
            CGameBossDmgInfo listBossDmgInfo = CGameBossMgr.Ins.GetPlayerDmgInfo(listAllPlayers[i].uid);
            if (listBossDmgInfo != null)
            {
                nlDmgRate = (float)listBossDmgInfo.nDmg / (float)nlBossMaxHP;
            }
            int showDmgRate = (int)CHelpTools.FloatRound(nlDmgRate * 10000f);
            if (i >= listRewardInfo.Count)
            {
                CGameBossRewardInfo bossRewardInfo = new CGameBossRewardInfo();
                bossRewardInfo.nUid = listAllPlayers[i].uid;
                bossRewardInfo.nItemId = 0;
                bossRewardInfo.nDailyGet = 1;
                bossRewardInfo.nShowDmgRate = showDmgRate;
                listRewardInfo.Add(bossRewardInfo);
            }
            else
            {
                listRewardInfo[i].nShowDmgRate = showDmgRate;
            }

            if (showDmgRate > 0 &&
                nLastAddValue > showDmgRate)
            {
                nLastAddIndex = showDmgRate;
                nLastAddIndex = i;
            }
            nTotalValue += showDmgRate;
            //listRewardInfo.Add(bossRewardInfo);
        }

        if (bVictory)
        {
            bool bAllDead = true;
            if (boss304 != null)
            {
                for (int i = 0; i < boss304.pWeaks.Length; i++)
                {
                    if (!boss304.pWeaks[i].bDead)
                    {
                        bAllDead = false;
                        break;
                    }
                }

            }
            if (bAllDead)
            {
                int nLerpValue = 10000 - nTotalValue;
                int nAddFirstValue = nLerpValue - 100 * listRewardInfo.Count;
                for (int i = 0; nLerpValue > 0; i++)
                {
                    if (i >= listRewardInfo.Count) continue;
                    if (nLerpValue > 100)
                    {
                        listRewardInfo[i].nShowDmgRate += 100;
                        nLerpValue -= 100;
                    }
                    else
                    {
                        listRewardInfo[i].nShowDmgRate += nLerpValue;
                        nLerpValue = 0;
                    }
                    if (i == 0 && nAddFirstValue > 0)
                    {
                        listRewardInfo[i].nShowDmgRate += nAddFirstValue;
                        nLerpValue -= nAddFirstValue;
                    }
                }
            }
        }

        if (listRewardInfo.Count > 0)
        {
            pSelfSlot.SetInfo(listRewardInfo[0], 0, listRewardInfo[0].nUid == lUidGetDropPlayer, pDropAvatarInfo, bVictory);
            if (bVictory)
            {
                CPlayerNetHelper.AddWinnerInfo(listRewardInfo[0].nUid, 1, 0);

                freeDrawRoot.Hide();
                CHttpParam pReqParams = new CHttpParam(
                    new CHttpParamSlot("uid", listRewardInfo[0].nUid.ToString()),
                    new CHttpParamSlot("modelId", "1"),
                    new CHttpParamSlot("gachaCount", "1")
                );
                CHttpMgr.Instance.SendHttpMsg(CHttpConst.BuyGiftGachaBox, new HHandlerShowFreeDraw(EMFreeDrawType.Boss), pReqParams, 10, true);
            }
            else
            {
                freeDrawRoot.Hide();
            }
            CGameBossMgr.Ins.SendRewardInfo(listRewardInfo);
            RefreshList(listRewardInfo);
        } 
    }

    public void GetResultInfoBySpecial(bool victory)
    {
        List<CPlayerUnit> listAllPlayers = CPlayerMgr.Ins.GetAllIdleUnit();
        List<CGameBossRewardInfo> listRewardInfo = new List<CGameBossRewardInfo>();
        int nGetItemID = 0;
        ST_GameMap pTBLMapInfo = CGameColorFishMgr.Ins.pMapConfig;
        for (int i = 0; i < pTBLMapInfo.arrDroppack.GetSize(); i++)
        {
            CLocalNetMsg msgDrop = pTBLMapInfo.arrDroppack.GetNetMsg(i);
            EMDropType emDropType = (EMDropType)msgDrop.GetInt("type");

            if (emDropType == EMDropType.Item)
            {
                int nId = msgDrop.GetInt("id");
                ST_FishMat fishMat = CTBLHandlerFishMaterial.Ins.GetInfo(nId);
                if (fishMat != null)
                {
                    nGetItemID = nId;
                    break;
                }
            }
        }
        bVictory = victory;
        //玩家按照伤害百分比排序
        listAllPlayers.Sort((x, y) =>
        {
            long nXDmg = 0;
            long nYDmg = 0;
            CGameBossDmgInfo bossXDmgInfo = CGameBossMgr.Ins.GetPlayerDmgInfo(x.uid);
            if (bossXDmgInfo != null)
            {
                nXDmg = bossXDmgInfo.nDmg;
            }
            CGameBossDmgInfo bossYDmgInfo = CGameBossMgr.Ins.GetPlayerDmgInfo(y.uid);
            if (bossYDmgInfo != null)
            {
                nYDmg = bossYDmgInfo.nDmg;
            }
            if (nYDmg < nXDmg)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        });

        //判断是否有掉落船
        pDropAvatarInfo = null;
        lUidGetDropPlayer = "";
        long nlBossMaxHP = 0;
        CBoss304 boss304 = CGameBossMgr.Ins.pBoss as CBoss304;
        if (boss304 != null)
        {
            nlBossMaxHP = boss304.nHPMax;
            for (int i = 0; i < boss304.pWeaks.Length; i++)
            {
                nlBossMaxHP += boss304.pWeaks[i].nHPMax;
            }
        }
        else
        {
            nlBossMaxHP = CGameBossMgr.Ins.pBoss.nHPMax;
        }
        if (bVictory)
        {
            if (IsDropAvatar())
            {
                RandomGetSpecialDropPlayer(listAllPlayers);
                CPlayerBaseInfo pPlayerGetDrop = CPlayerMgr.Ins.GetPlayer(lUidGetDropPlayer);
                if (pPlayerGetDrop != null)
                {
                    CGameBossDmgInfo listBossDmgInfo = CGameBossMgr.Ins.GetPlayerDmgInfo(pPlayerGetDrop.uid);
                    if (listBossDmgInfo != null &&
                        listBossDmgInfo.nDmg >= (int)(nlBossMaxHP * 0.04F))
                    {
                        Debug.Log("掉落玩家：" + pPlayerGetDrop.userName + "   uid:" + lUidGetDropPlayer);
                        pDropAvatarInfo = RandomDropAvatarBySpecial(pPlayerGetDrop);
                        if (pDropAvatarInfo != null)
                        {
                            SendDropAvatarReq(pPlayerGetDrop);

                            //重置Boss掉落加成
                            CGameColorFishMgr.Ins.nBossDropAdd = 0;
                        }
                        else
                        {
                            pDropAvatarInfo = null;
                            lUidGetDropPlayer = "";
                        }
                    }
                    else
                    {
                        pDropAvatarInfo = null;
                        lUidGetDropPlayer = "";
                    }
                }
                else
                {
                    Debug.LogWarning("没有人可以掉落船");
                    pDropAvatarInfo = null;
                    lUidGetDropPlayer = "";
                }
            }
            else
            {
                pDropAvatarInfo = null;
                lUidGetDropPlayer = "";
                //listAllPlayers[0].pInfo.uid;
                //CPlayerBaseInfo pPlayerGetDrop = CPlayerMgr.Ins.GetPlayer(lUidGetDropPlayer);
                //if (pPlayerGetDrop != null)
                //{
                //    pDropAvatarInfo = new CGameMapDropAvatarSlot();
                //    pDropAvatarInfo.emDropType = EMDropType.FishCoin;
                //    pDropAvatarInfo.nlValue = CGameColorFishMgr.Ins.pStaticConfig.GetInt("隐藏Boss安慰奖鱼币第一名");
                //    CPlayerNetHelper.AddFishCoin(lUidGetDropPlayer, pDropAvatarInfo.nlValue, EMFishCoinAddFunc.Pay);
                //}
            }
            
        }
        

        int nTotalChip = CGameBossMgr.Ins.GetTotalChipCount();
        bool bKillBoss = CGameBossMgr.Ins.CheckKillBoss();
        if (nGetItemID > 0)
        {
            int nLastAddValue = 999;
            int nLastAddIndex = 0;
            int nTotalValue = 0;
            for (int i = 0; i < listAllPlayers.Count; i++)
            {
                if (listAllPlayers[i] == null) continue;
                float nlDmgRate = 0;
                float nlChipRate = 0;
                CGameBossDmgInfo listBossDmgInfo = CGameBossMgr.Ins.GetPlayerDmgInfo(listAllPlayers[i].uid);
                if (listBossDmgInfo != null)
                {
                    nlDmgRate = (float)listBossDmgInfo.nDmg / (float)nlBossMaxHP;
                    nlChipRate = (float)listBossDmgInfo.nDmg / (float)(nlBossMaxHP - CGameBossMgr.Ins.pBoss.nCurHp);
                }
                long nGetChipByRate = (long)CHelpTools.FloatRound(nTotalChip * nlChipRate);//  (long)(nTotalChip * nlDmgRate);
                CGameBossRewardInfo bossRewardInfo = new CGameBossRewardInfo();
                bossRewardInfo.nUid = listAllPlayers[i].uid;
                bossRewardInfo.nItemId = nGetItemID;
                bossRewardInfo.nAddNum = nGetChipByRate;
                if (bKillBoss)
                {
                    bossRewardInfo.nAddNum += CGameBossMgr.Ins.GetChipByRank(i);
                }
                if(CGameBossMgr.Ins.pBoss.emBossType == EMBossType.Special)
                {
                    bossRewardInfo.nAddNum = System.Convert.ToInt32((float)bossRewardInfo.nAddNum * ((float)CGameColorFishMgr.Ins.pStaticConfig.GetInt("隐藏Boss芯片数量加成") * 0.01f));
                }
                bossRewardInfo.nDailyGet = 1;
                bossRewardInfo.nShowDmgRate = (int)CHelpTools.FloatRound(nlDmgRate * 10000f);

                if (bossRewardInfo.nShowDmgRate > 0 &&
                    nLastAddValue > bossRewardInfo.nShowDmgRate)
                {
                    nLastAddIndex = bossRewardInfo.nShowDmgRate;
                    nLastAddIndex = i;
                }
                nTotalValue += bossRewardInfo.nShowDmgRate;
                listRewardInfo.Add(bossRewardInfo);
            }

            if (bVictory)
            {
                bool bAllDead = true;
                if (boss304 != null)
                {
                    for (int i = 0; i < boss304.pWeaks.Length; i++)
                    {
                        if (!boss304.pWeaks[i].bDead)
                        {
                            bAllDead = false;
                            break;
                        }
                    }

                }
                if (bAllDead)
                {
                    int nLerpValue = 10000 - nTotalValue;
                    int nAddFirstValue = nLerpValue - 100 * listRewardInfo.Count;
                    for (int i = 0; nLerpValue > 0; i++)
                    {
                        if (i >= listRewardInfo.Count) continue;
                        if (nLerpValue > 100)
                        {
                            listRewardInfo[i].nShowDmgRate += 100;
                            nLerpValue -= 100;
                        }
                        else
                        {
                            listRewardInfo[i].nShowDmgRate += nLerpValue;
                            nLerpValue = 0;
                        }
                        if (i == 0 && nAddFirstValue > 0)
                        {
                            listRewardInfo[i].nShowDmgRate += nAddFirstValue;
                            nLerpValue -= nAddFirstValue;
                        }
                    }
                }
                //送鱼币
                for (int i = 0; i < listAllPlayers.Count; i++)
                {
                    if (i > 5) break;
                    if (listAllPlayers[i].uid == lUidGetDropPlayer)
                        continue;

                    long nlFishReward = (int)(((((float)listRewardInfo[i].nShowDmgRate * 0.0001f) * (float)nlBossMaxHP) / 16000f) * 1.2f) * 50000;
                    listRewardInfo[i].nlGetFishCoin = nlFishReward;
                    CPlayerNetHelper.AddFishCoin(listAllPlayers[i].uid, nlFishReward, EMFishCoinAddFunc.Pay, true);
                }
            }
        }
        if (listRewardInfo.Count > 0)
        {
            pSelfSlot.SetInfo(listRewardInfo[0], 0, listRewardInfo[0].nUid == lUidGetDropPlayer, pDropAvatarInfo, bVictory);
            CGameBossMgr.Ins.SendRewardInfo(listRewardInfo);
            RefreshList(listRewardInfo);
        }
    }


    bool IsDropAvatar()
    {
        List<CPlayerUnit> listAllPlayers = CPlayerMgr.Ins.GetAllIdleUnit();
        ST_GameMap pTBLMapInfo = CGameColorFishMgr.Ins.pMapConfig;
        if (CGameBossMgr.Ins.pBoss.emBossType == EMBossType.Normal)
        {
            if (pTBLMapInfo.listDropAvatarSlots.Count <= 0)
            {
                Debug.Log("没有掉落");
                return false;
            }
        }
        else if (CGameBossMgr.Ins.pBoss.emBossType == EMBossType.Special)
        {
            if (pTBLMapInfo.listSpecialDropAvatarSlots.Count <= 0)
            {
                Debug.Log("没有掉落");
                return false;
            }
        }

        //1:先判断是否有掉落船
        int nRateDropAble = Random.Range(0, 10000);
        int nTotalDropWeight = 0;
        if (CGameBossMgr.Ins.pBoss.emBossType == EMBossType.Normal)
        {
            nTotalDropWeight = pTBLMapInfo.nDropAvatarWeight;
            ////TODO:活动掉率翻倍
            //if (CFishFesInfoMgr.Ins.IsFesOn(1))
            //{
            //    nTotalDropWeight = nTotalDropWeight * 2;
            //}

            ///每多一个在场的人加一定数值的概率
            nTotalDropWeight += CGameColorFishMgr.Ins.pStaticConfig.GetInt("每个角色加成的掉落概率") * (listAllPlayers.Count - 1);

            nTotalDropWeight += +CGameColorFishMgr.Ins.nBossDropAdd;
        }
        else if(CGameBossMgr.Ins.pBoss.emBossType == EMBossType.Special)
        {
            nTotalDropWeight = pTBLMapInfo.nSpecialDropAvatarWeight;
            ////TODO:活动掉率翻倍
            //if (CFishFesInfoMgr.Ins.IsFesOn(1))
            //{
            //    nTotalDropWeight = nTotalDropWeight * 2;
            //}

            ///每多一个在场的人加一定数值的概率
            nTotalDropWeight += CGameColorFishMgr.Ins.pStaticConfig.GetInt("每个角色加成的掉落概率") * (listAllPlayers.Count - 1);
            
            nTotalDropWeight += +CGameColorFishMgr.Ins.nSpecialBossDropAdd;
        }
        ///判断是否有弱点击破 增加对应的掉落概率
        CBoss304 cBoss = CGameBossMgr.Ins.pBoss as CBoss304;
        if (cBoss != null)
        {
            nTotalDropWeight += cBoss.GetWeakAddRate();
        }

        Debug.Log("掉落权重：" + nRateDropAble + "   总权重：" + nTotalDropWeight);
        if (nRateDropAble > nTotalDropWeight)
        {
            ////没有掉落就提高Boss掉率
            //if (CGameColorFishMgr.Ins.nBossDropAdd < CGameColorFishMgr.Ins.pStaticConfig.GetInt("Boss加成上限"))
            //{
            //    //CGameColorFishMgr.Ins.nBossDropAdd += CGameColorFishMgr.Ins.pStaticConfig.GetInt("Boss每局加成");
            //}
            //else
            //{
            //    CGameColorFishMgr.Ins.nBossDropAdd = CGameColorFishMgr.Ins.pStaticConfig.GetInt("Boss加成上限");
            //}

            return false;
        }

        return true;
    }

    /// <summary>
    /// 随机掉落
    /// </summary>
    CGameMapDropAvatarSlot RandomDropAvatarByNormal(CPlayerBaseInfo pPlayer)
    {
        CGameMapDropAvatarSlot pRes = null;
        ST_GameMap pTBLMapInfo = CGameColorFishMgr.Ins.pMapConfig;
        if (pTBLMapInfo.listDropAvatarSlots.Count <= 0) return null;
        //2.掉落哪一艘
        List<CGameMapDropAvatarSlot> listRandPools = new List<CGameMapDropAvatarSlot>();
        int nTotalDropAvatarWeight = 0;
        for(int i=0; i<pTBLMapInfo.listDropAvatarSlots.Count; i++)
        {
            if(pTBLMapInfo.listDropAvatarSlots[i].emDropType == EMDropType.Boat)
            {
                if (pPlayer.pBoatPack.GetInfo(pTBLMapInfo.listDropAvatarSlots[i].nId) != null)
                {
                    continue;
                }
            }
            else if(pTBLMapInfo.listDropAvatarSlots[i].emDropType == EMDropType.Role)
            {
                if (pPlayer.pAvatarPack.GetInfo(pTBLMapInfo.listDropAvatarSlots[i].nId) != null)
                {
                    continue;
                }
            }
            nTotalDropAvatarWeight += pTBLMapInfo.listDropAvatarSlots[i].nWeight;
            listRandPools.Add(pTBLMapInfo.listDropAvatarSlots[i]);
        }

        if(listRandPools.Count <= 0)
        {
            return null;
        }
        int nRateDropAvatar = Random.Range(0, nTotalDropAvatarWeight + 1);
        for (int i = 0; i < listRandPools.Count; i++)
        {
            if(nRateDropAvatar <= listRandPools[i].nWeight)
            {
                pRes = listRandPools[i];
                break;
            }
            else
            {
                nRateDropAvatar -= listRandPools[i].nWeight;
            }
        }

        Debug.Log("掉落船：" + pRes.nId);

        return pRes;
    }

    /// <summary>
    /// 随机掉落
    /// </summary>
    CGameMapDropAvatarSlot RandomDropAvatarBySpecial(CPlayerBaseInfo pPlayer)
    {
        CGameMapDropAvatarSlot pRes = null;
        ST_GameMap pTBLMapInfo = CGameColorFishMgr.Ins.pMapConfig;
        if (pTBLMapInfo.listSpecialDropAvatarSlots.Count <= 0) return null;

        //2.掉落哪一艘
        List<CGameMapDropAvatarSlot> listRandPools = new List<CGameMapDropAvatarSlot>();
        int nTotalDropAvatarWeight = 0;
        for (int i = 0; i < pTBLMapInfo.listSpecialDropAvatarSlots.Count; i++)
        {
            if (pTBLMapInfo.listSpecialDropAvatarSlots[i].emDropType == EMDropType.Boat)
            {
                if (pPlayer.pBoatPack.GetInfo(pTBLMapInfo.listSpecialDropAvatarSlots[i].nId) != null) continue;
            }
            else if (pTBLMapInfo.listSpecialDropAvatarSlots[i].emDropType == EMDropType.Role)
            {
                if (pPlayer.pAvatarPack.GetInfo(pTBLMapInfo.listSpecialDropAvatarSlots[i].nId) != null) continue;
            }

            nTotalDropAvatarWeight += pTBLMapInfo.listSpecialDropAvatarSlots[i].nWeight;
            listRandPools.Add(pTBLMapInfo.listSpecialDropAvatarSlots[i]);
        }

        if (listRandPools.Count <= 0)
        {
            return null;
        }

        int nRateDropAvatar = Random.Range(0, nTotalDropAvatarWeight + 1);
        for (int i = 0; i < listRandPools.Count; i++)
        {
            if (nRateDropAvatar <= listRandPools[i].nWeight)
            {
                pRes = listRandPools[i];
                break;
            }
            else
            {
                nRateDropAvatar -= listRandPools[i].nWeight;
            }
        }

        Debug.Log("掉落船：" + pRes.nId);

        return pRes;
    }

    /// <summary>
    /// 随机获得掉落的玩家
    /// </summary>
    void RandomGetDropPlayer(List<CPlayerUnit> listUnits)
    {
        ST_GameMap pTBLMapInfo = CGameColorFishMgr.Ins.pMapConfig;
        if (pTBLMapInfo.listDropAvatarSlots.Count <= 0)
        {
            lUidGetDropPlayer = "";
            return;
        }

        int nTotalWeight = 0;
        List<CRandDropPlayerInfo> listRankPool = new List<CRandDropPlayerInfo>();
        for (int i=0; i< listUnits.Count; i++)
        {
            if (listUnits[i].pInfo == null) continue;

            CGameBossDmgInfo listBossDmgInfo = CGameBossMgr.Ins.GetPlayerDmgInfo(listUnits[i].pInfo.uid);
            if (listBossDmgInfo != null &&
                listBossDmgInfo.nDmg >= (int)(CGameBossMgr.Ins.pBoss.nHPMax * 0.09F))
            {
                //判断是否所有船都有了
                bool bDropAble = false;
                for (int d = 0; d < pTBLMapInfo.listDropAvatarSlots.Count; d++)
                {
                    if (listUnits[i].pInfo.pBoatPack.GetInfo(pTBLMapInfo.listDropAvatarSlots[d].nId) == null)
                    {
                        bDropAble = true;
                        break;
                    }
                }

                if (!bDropAble) continue; 

                CRandDropPlayerInfo pRandPlayerInfo = new CRandDropPlayerInfo();
                pRandPlayerInfo.uid = listUnits[i].pInfo.uid;
                float nlDmgRate = (float)listBossDmgInfo.nDmg / (float)CGameBossMgr.Ins.pBoss.nHPMax;
                pRandPlayerInfo.weight = (int)CHelpTools.FloatRound(nlDmgRate * 10000f) + (int)(listBossDmgInfo.nBomberCount * 200);
                nTotalWeight += pRandPlayerInfo.weight;

                listRankPool.Add(pRandPlayerInfo);
            }
        }

        if (listRankPool.Count <= 0)
        {
            Debug.Log("没有玩家");
            lUidGetDropPlayer = "";
            return;
        }

        Debug.Log("随机玩家池子：" + listRankPool.Count);
        long nCurWeight = Random.Range(1, nTotalWeight + 1);
        for(int i=0; i<listRankPool.Count; i++)
        {
            if(nCurWeight <= listRankPool[i].weight)
            {
                lUidGetDropPlayer = listRankPool[i].uid;
                break;
            }
            else
            {
                nCurWeight -= listRankPool[i].weight;
            }
        }
    }

    /// <summary>
    /// 随机获得掉落的玩家
    /// </summary>
    void RandomGetSpecialDropPlayer(List<CPlayerUnit> listUnits)
    {
        ST_GameMap pTBLMapInfo = CGameColorFishMgr.Ins.pMapConfig;
        if (pTBLMapInfo.listSpecialDropAvatarSlots.Count <= 0)
        {
            lUidGetDropPlayer = "";
            return;
        }

        for (int i = 0; i < listUnits.Count; i++)
        {
            if (listUnits[i].pInfo == null) continue;

            CGameBossDmgInfo listBossDmgInfo = CGameBossMgr.Ins.GetPlayerDmgInfo(listUnits[i].pInfo.uid);
            if (listBossDmgInfo != null &&
                listBossDmgInfo.nDmg >= (int)(CGameBossMgr.Ins.pBoss.nHPMax * 0.09F))
            {
                //判断是否所有船都有了
                bool bDropAble = false;
                for (int d = 0; d < pTBLMapInfo.listSpecialDropAvatarSlots.Count; d++)
                {
                    if (listUnits[i].pInfo.pBoatPack.GetInfo(pTBLMapInfo.listSpecialDropAvatarSlots[d].nId) == null)
                    {
                        bDropAble = true;
                        break;
                    }
                }

                if (!bDropAble) continue;

                lUidGetDropPlayer = listUnits[i].pInfo.uid;
                break;
            }
        }
    }

    /// <summary>
    /// 发起获得掉落的请求
    /// </summary>
    void SendDropAvatarReq(CPlayerBaseInfo player)
    {
        if (pDropAvatarInfo == null) return;

        if(pDropAvatarInfo.emDropType == EMDropType.Boat)
        {
            if (player.pBoatPack.GetInfo(pDropAvatarInfo.nId) != null)
            {
                return;
            }

            CPlayerNetHelper.SendUserBoat(player.uid, pDropAvatarInfo.nId, new HHandlerGetDropBoat());
        }
        else if(pDropAvatarInfo.emDropType == EMDropType.Role)
        {
            if (player.pAvatarPack.GetInfo(pDropAvatarInfo.nId) != null)
            {
                return;
            }
        }
    }

    public void RefreshList(List<CGameBossRewardInfo> infos)
    {
        listCurInfos = infos;
        listCurInfos.RemoveAt(0);
        //获取信息
        if (!bInit)
        {
            mLoopListView.InitGridView(listCurInfos.Count, OnGetItemByIndex);
            bInit = true;
        }
        else
        {
            mLoopListView.SetListItemCount(listCurInfos.Count);
            mLoopListView.RefreshAllShownItem();
        }
    }

    LoopGridViewItem OnGetItemByIndex(LoopGridView listView, int index, int row, int column)
    {
        if (index < 0 || index >= listCurInfos.Count)
        {
            return null;
        }

        CGameBossRewardInfo pRankInfo = listCurInfos[index];
        LoopGridViewItem item = listView.NewListViewItem("RankSlot");
        UIBossDmgResultSlot itemScript = item.GetComponent<UIBossDmgResultSlot>();
        itemScript.SetInfo(pRankInfo, index + 1, pRankInfo.nUid == lUidGetDropPlayer, pDropAvatarInfo, bVictory);

        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
        }

        return item;
    }

    public void OnClickClose()
    {
        CPlayerMgr.Ins.ClearAllActiveUnit();
        CPlayerMgr.Ins.ClearAllIdleUnit();
        CPlayerMgr.Ins.ClearActivePlayer();
        //CPlayerMgr.Ins.ClearAllPlayerInfos();

        CloseSelf();

        CSceneMainMenu.emOpenUI = UIResType.RoomInfo;
        UIManager.Instance.OpenUI(UIResType.Loading);
        CSceneMgr.Instance.LoadScene(CSceneFactory.EMSceneType.MainMenu);
    }

}
