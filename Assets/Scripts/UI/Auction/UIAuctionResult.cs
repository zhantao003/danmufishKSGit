using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UIAuctionResult : UIBase
{
    enum EMAuctionState
    {
        Normal,
        Show,
        Over,
    }

    public GameObject objInfoRoot;

    public Animator anima;
    [Header("背景图片")]
    public Image uiImgBG;
    public Vector2 vecSingleBGSize;
    public Vector2 vecMultipleBGSize;
    [Header("宝箱图片")]
    public UIRawIconLoad rawIconLoad;
    [Header("宝箱名字")]
    public Text uiName;
    [Header("玩家名字")]
    public Text[] uiPlayerName;
    [Header("渔获数量")]
    public Text uiFishCount;
    [Header("起拍价")]
    public Text uiPrice;
    [Header("总价值")]
    public Text uiFishCoin;
    [Header("总价值Tween效果")]
    public UITweenScale uiFishCoinTween;
    [Header("模型位移")]
    public UITweenPos uiModelTween;
    [Header("展示Root")]
    public UIAuctionShowRoot auctionShowRoot;
    [Header("状态文本")]
    public UIRawIconLoad uiState;
    [Header("单人额外奖励")]
    public UIAuctionResultOtherRoot pSingleOther;
    [Header("多人额外奖励")]
    public UIAuctionResultOtherRoot pMultipleOther;
    [Header("拍卖玩家头像")]
    public UIAuctionWaitPlayer[] uiAuctionImgs;

    EMAuctionState emAuctionState = EMAuctionState.Normal;
    /// <summary>
    /// 当前增加展示物品的下标
    /// </summary>
    int nCurShowIdx;
    /// <summary>
    /// 总收益
    /// </summary>
    long nlTotalPrice;

    CPropertyTimer pAddTicker;
    public float fAddTime;
    public float fOverShowTime;

    public Transform tranAvatarRoot;
    [ReadOnly]
    public GameObject objAvatarShow;

    public UITweenPos tweenPos;
    public Vector3 vecStartPos;
    public Vector3 vecShowPos;
    public Vector3 vecHidePos;

    public GameObject objSingleRoot;
    public GameObject objMultipleRoot;

    ST_AuctionTreasureInfo curTreasureInfo;

    CAuctionResultTreasureInfo resultInfo;

    DelegateNFuncCall delShowBoxOverChg;

    void Hide()
    {
        tweenPos.from = vecShowPos;
        tweenPos.to = vecHidePos;
        tweenPos.Play(delegate()
        {
            emAuctionState = EMAuctionState.Normal;
            if (objAvatarShow != null)
            {
                Destroy(objAvatarShow);
                objAvatarShow = null;
            }
            CloseSelf();
        });
    }

    private void Update()
    {
        if (pAddTicker != null &&
           pAddTicker.Tick(CTimeMgr.DeltaTime))
        {
            if (emAuctionState == EMAuctionState.Show)
            {
                AddNewShowInfo();
            }
            else if(emAuctionState == EMAuctionState.Over)
            {
                delShowBoxOverChg?.Invoke();
                Hide();
                pAddTicker = null;
            }
        }
    }

    public void SetInfo(ST_AuctionTreasureInfo auctionTreasureInfo,CAuctionResultTreasureInfo auctionResultInfo, DelegateNFuncCall showBoxOverChg)
    {
        delShowBoxOverChg = showBoxOverChg;
        nlTotalPrice = 0;
        nCurShowIdx = 0;
        auctionShowRoot.Reset();
        objInfoRoot.SetActive(false);
        curTreasureInfo = auctionTreasureInfo;
        pSingleOther.SetActive(false);
        pMultipleOther.SetActive(false);
        resultInfo = auctionResultInfo;
        if (curTreasureInfo != null)
        {
            if (curTreasureInfo.emAuctionLimitType == EMAuctionLimitType.Single)
            {
                uiModelTween.tranTarget.localPosition = uiModelTween.to;
                uiImgBG.GetComponent<RectTransform>().sizeDelta = vecSingleBGSize;
            }
            else if (curTreasureInfo.emAuctionLimitType == EMAuctionLimitType.Multiple)
            {
                uiModelTween.tranTarget.localPosition = uiModelTween.from;
                uiImgBG.GetComponent<RectTransform>().sizeDelta = vecMultipleBGSize;
                if (resultInfo != null)
                {
                    for (int i = 0; i < resultInfo.listPlayers.Count; i++)
                    {
                        if (i >= uiAuctionImgs.Length) break;
                        string szHeadIcon = resultInfo.listPlayers[i].userFace;
                        uiAuctionImgs[i].SetActive(true);
                        uiAuctionImgs[i].SetIcon(szHeadIcon);
                    }
                    if (resultInfo.listPlayers.Count < uiAuctionImgs.Length)
                    {
                        for (int i = resultInfo.listPlayers.Count; i < uiAuctionImgs.Length; i++)
                        {
                            uiAuctionImgs[i].SetActive(false);
                        }
                    }
                }
            }
        }
        
        ///播放面板位移动画
        tweenPos.from = vecStartPos;
        tweenPos.to = vecShowPos;
        tweenPos.Play(delegate()
        {
            objInfoRoot.SetActive(true);
            if (curTreasureInfo != null)
            {
                if (rawIconLoad != null)
                {
                    rawIconLoad.SetIconSync(curTreasureInfo.szIcon);
                }
                if (uiName != null)
                {
                    uiName.text = curTreasureInfo.szName;
                }
                objSingleRoot.SetActive(curTreasureInfo.emAuctionLimitType == EMAuctionLimitType.Single);
                objMultipleRoot.SetActive(curTreasureInfo.emAuctionLimitType == EMAuctionLimitType.Multiple);
            }
            if (resultInfo != null)
            {
                
                if (uiPrice != null)
                {
                    uiPrice.text = resultInfo.nlBuyPrice.ToString();
                }

                SetTotalPrice(-resultInfo.nlBuyPrice, false);

                ///获取随机奖励的玩家
                CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(resultInfo.nlPlayerUID);
                if (pPlayer != null)
                {
                    SetAvatar(pPlayer.avatarId);
                    for(int i = 0;i < uiPlayerName.Length;i++)
                    {
                        if (uiPlayerName[i] == null) continue;
                        uiPlayerName[i].text = pPlayer.userName;
                    }
                }
                if (resultInfo.dropInfo != null)
                {
                    string szDropName = string.Empty;
                    string szIcon = string.Empty;
                    //角色
                    if (resultInfo.dropInfo.nType == 0)
                    {
                        ST_UnitAvatar pTBLAvatarInfo = CTBLHandlerUnitAvatar.Ins.GetInfo(resultInfo.dropInfo.nValue);
                        if (pTBLAvatarInfo != null)
                        {
                            szDropName = pTBLAvatarInfo.szName + "#" + resultInfo.dropInfo.nValue;
                        }
                        szIcon = pTBLAvatarInfo.szIcon;
                    }
                    //船
                    else if (resultInfo.dropInfo.nType == 1)
                    {
                        ST_UnitFishBoat pTBLBoatInfo = CTBLHandlerUnitFishBoat.Ins.GetInfo(resultInfo.dropInfo.nValue);
                        if (pTBLBoatInfo != null)
                        {
                            szDropName = pTBLBoatInfo.szName + "#" + resultInfo.dropInfo.nValue;
                        }
                        szIcon = pTBLBoatInfo.szIcon;
                    }
                    //渔具
                    else if (resultInfo.dropInfo.nType == 2)
                    {
                        szIcon = "Icon/Gift/FishPack";
                        szDropName = "互动手柄:" + resultInfo.dropInfo.nValue + "套";

                    }
                    //飞轮
                    else if (resultInfo.dropInfo.nType == 5)
                    {
                        szIcon = "Icon/Gift/FeiLun";
                        szDropName = "初级卷轴:" + resultInfo.dropInfo.nValue + "套";

                    }
                    //材料
                    else if(resultInfo.dropInfo.nType == 3)
                    {
                        //加材料
                        int nMatID = 0;
                        List<ST_FishInfo> listFishInfo = CGameColorFishMgr.Ins.pMap.pTBLHandlerBoomFishInfo.GetInfos();
                        for (int i = 0; i < listFishInfo.Count; i++)
                        {
                            if (listFishInfo[i].emItemType == EMItemType.FishMat)
                            {
                                if (int.TryParse(listFishInfo[i].szIcon, out nMatID))
                                {
                                    ST_FishMat fishMat = CTBLHandlerFishMaterial.Ins.GetInfo(nMatID);
                                    if(fishMat != null)
                                    {
                                        szIcon = fishMat.szIcon;
                                        szDropName = fishMat.szName + ":" + resultInfo.dropInfo.nValue + "个";
                                    }
                                    break;
                                }
                            }
                        }

                    }//指定材料
                    else if (resultInfo.dropInfo.nType == 4)
                    {
                        int nMatID =0;
                        //加材料
                        if (pPlayer.pBoatPack.GetInfo(CGameColorFishMgr.Ins.pStaticConfig.GetInt("特定兑换船ID")) != null)
                        {
                            List<ST_FishInfo> listFishInfo = CGameColorFishMgr.Ins.pMap.pTBLHandlerBoomFishInfo.GetInfos();
                            for (int i = 0; i < listFishInfo.Count; i++)
                            {
                                if (listFishInfo[i].emItemType == EMItemType.FishMat)
                                {
                                    if (int.TryParse(listFishInfo[i].szIcon, out nMatID))
                                    {
                                        break;
                                    }

                                }
                            }
                        }
                        else
                        {
                            nMatID = CGameColorFishMgr.Ins.pStaticConfig.GetInt("云游商人特定材料ID");
                        }
                        ST_FishMat fishMat = CTBLHandlerFishMaterial.Ins.GetInfo(nMatID);
                        if (fishMat != null)
                        {
                            szIcon = fishMat.szIcon;
                            szDropName = fishMat.szName + ":" + resultInfo.dropInfo.nValue + "个";
                        }
                    }
                    ///设置对应的额外奖励信息
                    if (curTreasureInfo.emAuctionLimitType == EMAuctionLimitType.Single)
                    {
                        pSingleOther.SetIcon(szIcon);
                        pSingleOther.SetIconSize(new Vector2(100f, 100f));
                        pSingleOther.SetShowText(szDropName);
                    }
                    else if (curTreasureInfo.emAuctionLimitType == EMAuctionLimitType.Multiple)
                    {
                        pMultipleOther.SetIcon(szIcon);
                        pMultipleOther.SetIconSize(new Vector2(100f, 100f));
                        pMultipleOther.SetShowText(szDropName);
                    }
                }
            }
            emAuctionState = EMAuctionState.Show;
            AddNewShowInfo();
        });
    }

    /// <summary>
    /// 设置角色
    /// </summary>
    /// <param name="tbid"></param>
    public void SetAvatar(int avatarId)
    {
        if (objAvatarShow != null)
        {
            Destroy(objAvatarShow);
        }

        ST_UnitAvatar pAvatarInfo = CTBLHandlerUnitAvatar.Ins.GetInfo(avatarId);
        if (pAvatarInfo == null)
        {
            Debug.LogError("错误的皮肤信息：" + avatarId);
            return;
        }

        CResLoadMgr.Inst.SynLoad(pAvatarInfo.szPrefab, CResLoadMgr.EM_ResLoadType.Role,
        delegate (Object res, object data, bool bSuc)
        {
            GameObject objNewAvatarRoot = res as GameObject;
            if (objNewAvatarRoot == null)
            {
                Debug.LogError($"{avatarId} 没有皮肤预制体：{pAvatarInfo.szPrefab}");
                return;
            }

            objAvatarShow = GameObject.Instantiate(objNewAvatarRoot) as GameObject;
            Transform tranAvatarShow = objAvatarShow.GetComponent<Transform>();
            tranAvatarShow.SetParent(tranAvatarRoot);
            tranAvatarShow.localPosition = Vector3.zero;
            tranAvatarShow.localRotation = Quaternion.identity;
            tranAvatarShow.localScale = Vector3.one;

            CPlayerAvatar pNewAvatar = objAvatarShow.GetComponent<CPlayerAvatar>();
            if (pNewAvatar != null)
            {
                pNewAvatar.InitJumpEff();
                pNewAvatar.PlayJumpEff(false);
                pNewAvatar.ShowHandObj(false);
                CTimeTickMgr.Inst.PushTicker(1.1F, delegate (object[] values)
                {
                    if (pNewAvatar != null)
                    {
                        pNewAvatar.PlayAnime(CUnitAnimeConst.Anime_Win);
                    }
                });
            }

            CHelpTools.SetLayer(tranAvatarShow, LayerMask.NameToLayer("show"));
        });
    }


    void SetTotalPrice(long price,bool bPlayTween)
    {
        nlTotalPrice += price;
        uiFishCoin.text = nlTotalPrice.ToString();
        if(bPlayTween)
        {
            uiFishCoinTween.Play();
        }
        CheckState();
    }

    void CheckState()
    {
        
        List<ST_AuctionRate> auctionTreasureRates = CTBLHandlerAuctionTreasureRate.Ins.GetInfos();
        for(int i = 0;i < auctionTreasureRates.Count;i++)
        {
            if(auctionTreasureRates[i].CheckInRange(nlTotalPrice, resultInfo.nlBuyPrice))
            {
                //Debug.LogError("Cur State ====" + auctionTreasureRates[i].szDes);
                uiState.SetIconSync(auctionTreasureRates[i].szIcon);
                break;
            }
        }
    }

    void AddNewShowInfo()
    {
        if (nCurShowIdx >= resultInfo.listFishInfo.Count)
        {
            emAuctionState = EMAuctionState.Over;
            if (resultInfo.dropInfo != null)
            {
                pAddTicker = new CPropertyTimer();
                pAddTicker.Value = fOverShowTime + 3F;
                pAddTicker.FillTime();
                CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(resultInfo.nlPlayerUID);
                UIUserGetAvatar uiGetAvatar = UIManager.Instance.GetUI(UIResType.UserGetAvatarTip) as UIUserGetAvatar;
                
               
                //anima.Play("show");
                if(curTreasureInfo.emAuctionLimitType == EMAuctionLimitType.Single)
                {
                    pSingleOther.SetActive(true);
                    pSingleOther.PlayTween();
                }
                else if(curTreasureInfo.emAuctionLimitType == EMAuctionLimitType.Multiple)
                {
                    uiModelTween.Play();
                    pMultipleOther.SetActive(true);
                    pMultipleOther.PlayTween();
                }
                int nCheckType = 0;
                if (resultInfo.dropInfo.nType == 0)
                {
                    nCheckType = 1;
                    /////展示飘动的信息
                    //if (uiGetAvatar != null)
                    //{
                    //    CPlayerAvatarInfo pAvatarInfo = new CPlayerAvatarInfo();
                    //    pAvatarInfo.nAvatarId = resultInfo.dropInfo.nValue;
                    //    uiGetAvatar.AddInfo(pPlayer, pAvatarInfo, UIUserGetAvatar.EMGetFunc.Auction);
                    //}
                }
                else if (resultInfo.dropInfo.nType == 1)
                {
                    nCheckType = 1;
                    /////展示飘动的信息
                    //if (uiGetAvatar != null)
                    //{
                    //    CPlayerBoatInfo pBoatInfo = new CPlayerBoatInfo();
                    //    pBoatInfo.nBoatId = resultInfo.dropInfo.nValue;
                    //    uiGetAvatar.AddBoat(pPlayer, pBoatInfo, UIUserGetAvatar.EMGetFunc.Auction);
                    //}
                }
                else if (resultInfo.dropInfo.nType == 2)
                {
                    nCheckType = 2;
                    /////展示飘动的信息
                    //if (uiGetAvatar != null)
                    //{
                    //    uiGetAvatar.AddVipDailySign(pPlayer, resultInfo.dropInfo.nValue, UIUserGetAvatar.EMGetFunc.Auction);
                    //}
                }
                else if (resultInfo.dropInfo.nType == 5)
                {
                    nCheckType = 2;
                    /////展示飘动的信息
                    //if (uiGetAvatar != null)
                    //{
                    //    uiGetAvatar.AddVipDailySign(pPlayer, resultInfo.dropInfo.nValue, UIUserGetAvatar.EMGetFunc.Auction);
                    //}
                }
                else if (resultInfo.dropInfo.nType == 3)
                {
                    nCheckType = 2;
                    /////展示飘动的信息
                    //if (uiGetAvatar != null)
                    //{
                    //    uiGetAvatar.AddVipDailySign(pPlayer, resultInfo.dropInfo.nValue, UIUserGetAvatar.EMGetFunc.Auction);
                    //}
                }
                else if (resultInfo.dropInfo.nType == 4)
                {
                    nCheckType = 2;
                    /////展示飘动的信息
                    //if (uiGetAvatar != null)
                    //{
                    //    uiGetAvatar.AddVipDailySign(pPlayer, resultInfo.dropInfo.nValue, UIUserGetAvatar.EMGetFunc.Auction);
                    //}
                }
                List<ST_AuctionRate> auctionTreasureRates = CTBLHandlerAuctionTreasureRate.Ins.GetInfos();
                for (int i = 0; i < auctionTreasureRates.Count; i++)
                {
                    if (auctionTreasureRates[i].nType == nCheckType)
                    {
                        uiState.SetIconSync(auctionTreasureRates[i].szIcon);
                        break;
                    }
                }
            }
            else
            {
                pAddTicker = new CPropertyTimer();
                pAddTicker.Value = fOverShowTime;
                pAddTicker.FillTime();
            }

        }
        else
        {
            pAddTicker = new CPropertyTimer();
            pAddTicker.Value = fAddTime;
            pAddTicker.FillTime();
            auctionShowRoot.AddNewInfo(resultInfo.listFishInfo[nCurShowIdx]);
            SetTotalPrice(resultInfo.listFishInfo[nCurShowIdx].lPrice, true);
            nCurShowIdx++;
            uiFishCount.text = nCurShowIdx + "个";
        }
    }

}
