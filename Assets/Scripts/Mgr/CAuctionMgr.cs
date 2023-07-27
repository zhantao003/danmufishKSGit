using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ɾͱ�������
/// </summary>
public enum EMAuctionBoxType
{
    Box1 = 1,
    Box2,
    Box3,
    Box4,
    Box5,

    Max,
}

public class CAuctionResultTreasureInfo
{
    public List<CPlayerBaseInfo> listPlayers;   //�������������
    public string nlPlayerUID;                //���UID
    public long nlBuyPrice;                 //���ļ۸�
    public List<CFishInfo> listFishInfo;    //����Ϣ
    public long nlFishCoin;                 //�����ֵ
    public COtherDropInfo dropInfo;         //���������Ϣ
}

public class CTreasureInfo
{
    public ST_AuctionTreasureInfo treasureInfo;

    public CTreasureInfo(ST_AuctionTreasureInfo info)
    {
        treasureInfo = info;
    }
}

public class CAuctionMgr : MonoBehaviour
{
    static CAuctionMgr ins = null;

    public static CAuctionMgr Ins
    {
        get
        {
            return ins;
        }
    }

    public EMAuctionLimitType curAuctionLimitType;

    /// <summary>
    /// ����״̬
    /// </summary>
    public enum EMAuctionState
    {
        Normal,             //Ĭ��
        StartAuction,       //��ʼ����
        Auctioning,         //������
    }

    /// <summary>
    /// ����״̬
    /// </summary>
    public EMAuctionState emAuctionState = EMAuctionState.Normal;
    /// <summary>
    /// �Ƿ���������
    /// </summary>
    public bool bAuction;
    /// <summary>
    /// ��ǰ�ı�����Ϣ
    /// </summary>
    public CTreasureInfo curTreasureInfo;
    /// <summary>
    /// ��ǰ�ľ�������Ϣ
    /// </summary>
    public CPlayerBaseInfo pCurAuctionPlayerInfo;

    public List<CPlayerBaseInfo> listAuctionPlayer = new List<CPlayerBaseInfo>();
    /// <summary>
    /// �����۸�
    /// </summary>
    public long nBuyPrice;
    /// <summary>
    /// �ο���ֵ
    /// </summary>
    public long nNormalPrice;
    /// <summary>
    /// ��������
    /// </summary>
    public int nBaseCount;
    /// <summary>
    /// ������ȡ�������
    /// </summary>
    int nAuctionCount;

    List<CAuctionInfo> listAuctionInfos = new List<CAuctionInfo>();

    /// <summary>
    /// ������Ϣ
    /// </summary>
    CAuctionResultTreasureInfo auctionResultInfo;

    bool bCri;

    /// <summary>
    /// ������Ϣ�ı�
    /// </summary>
    public DelegateSLFuncCall deleAuctionChgInfo;

    public CTBLHandlerFishInfo[] pAuctionFishByBox;

    public DelegateNFuncCall delShowBoxOverChg;

    string szFishPackBase = "paimai/";

    private void Awake()
    {
        ins = this;
        InitFishInfo();
    }

    public bool bAuctionByUID(string uid)
    {
        bool bAuction = false;
        if (curTreasureInfo == null) 
            return bAuction;
        if (curTreasureInfo.treasureInfo.emAuctionLimitType == EMAuctionLimitType.Multiple)
        {
            for (int i = 0; i < listAuctionPlayer.Count; i++)
            {
                if (listAuctionPlayer[i].uid == uid)
                {
                    bAuction = true;
                    break;
                }
            }
        }
        else if(curTreasureInfo.treasureInfo.emAuctionLimitType == EMAuctionLimitType.Single)
        {
            if(pCurAuctionPlayerInfo != null &&
               pCurAuctionPlayerInfo.uid.Equals(uid))
            {
                bAuction = true;
            }
        }
        
        return bAuction;
    }

    void InitFishInfo()
    {
        List<ST_AuctionTreasureInfo> auctionTreasureInfos = CTBLHandlerAuctionTreasureInfo.Ins.GetInfos();

        pAuctionFishByBox = new CTBLHandlerFishInfo[auctionTreasureInfos.Count];
        for(int i = 0;i < pAuctionFishByBox.Length;i++)
        {
            int nBoxIdx = i;
#if TBL_LOCAL
            CTBLInfo.Inst.LoadTBL("TBL/" + szFishPackBase + auctionTreasureInfos[i].szTBLName,
                delegate (CTBLLoader loader)
                {
                    pAuctionFishByBox[nBoxIdx] = new CTBLHandlerFishInfo();
                    pAuctionFishByBox[nBoxIdx].LoadInfo(loader);
                });
#else
            CTBLInfo.Inst.LoadTBLByBundle(CTBLInfo.Inst.pTBLBundle, auctionTreasureInfos[i].szTBLName,
               delegate (CTBLLoader loader)
               {
                   pAuctionFishByBox[nBoxIdx] = new CTBLHandlerFishInfo();
                   pAuctionFishByBox[nBoxIdx].LoadInfo(loader);
               });
#endif
        }
    }

    public CTBLHandlerFishInfo GetFishHandler(int nBoxID)
    {
        return pAuctionFishByBox[nBoxID - 1];
    }

    void GetInfoByAuction()
    {
        delShowBoxOverChg = null;
        curTreasureInfo = null;
        pCurAuctionPlayerInfo = null;
        listAuctionPlayer.Clear();
        int nTotalWeight = 0;
        ///�������ı�����Ϣ
        List<ST_AuctionTreasureInfo> listInfos = new List<ST_AuctionTreasureInfo>();
        listInfos.AddRange(CTBLHandlerAuctionTreasureInfo.Ins.GetInfos());
        for (int i = 0; i < listInfos.Count;)
        {
            if (listInfos[i].emAuctionLimitType != curAuctionLimitType)
            {
                listInfos.RemoveAt(i);
            }
            else
            {
                nTotalWeight += listInfos[i].nWeight;
                i++;
            }
        }
        int nRandomValue = Random.Range(0, nTotalWeight + 1);
        for (int i = 0; i < listInfos.Count; i++)
        {
            if (listInfos[i].nWeight <= 0)
                continue;
            nRandomValue -= listInfos[i].nWeight;
            if (nRandomValue > 0)
                continue;
            curTreasureInfo = new CTreasureInfo(listInfos[i]);
            break;
        }

        nNormalPrice = curTreasureInfo.treasureInfo.GetBuyPrice(out nAuctionCount, out bCri);
        nBuyPrice = curTreasureInfo.treasureInfo.GetPriceByBuyRange((int)nNormalPrice);
        nBaseCount = curTreasureInfo.treasureInfo.GetCountByNormalRange(nAuctionCount);
        emAuctionState = EMAuctionState.StartAuction;

        UIAuction auction = UIManager.Instance.GetUI(UIResType.Auction) as UIAuction;
        if (auction != null)
        {
            auction.SetInfo();
        }
    }

    public void SetPlayerByHttp(CPlayerBaseInfo baseInfo)
    {
        long price = 0;
        if(curTreasureInfo.treasureInfo.emAuctionLimitType == EMAuctionLimitType.Single)
        {
            price = nBuyPrice + curTreasureInfo.treasureInfo.nAddPrice;
        }
        else if (curTreasureInfo.treasureInfo.emAuctionLimitType == EMAuctionLimitType.Multiple)
        {
            price = nBuyPrice;
            for (int i = 0; i < listAuctionPlayer.Count; i++)
            {
                if (listAuctionPlayer[i].uid == baseInfo.uid)
                    return;
            }
        }

        if (nBuyPrice > baseInfo.GameCoins)
        {
            UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
            if (uiGameInfo == null || !uiGameInfo.IsOpen()) return;
            UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(baseInfo.uid);
            if (uiUnitInfo == null) return;
            uiUnitInfo.SetDmContent("���ֲ���");
            return;
        }

        if (curTreasureInfo.treasureInfo.emAuctionLimitType == EMAuctionLimitType.Single)
        {
            if (nBuyPrice >= price ||
                price - nBuyPrice < curTreasureInfo.treasureInfo.nAddPrice)
            {
                UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
                if (uiGameInfo == null || !uiGameInfo.IsOpen()) return;
                UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(baseInfo.uid);
                if (uiUnitInfo == null) return;
                uiUnitInfo.SetDmContent("���۹���");
                return;
            }
            if (price - nBuyPrice > curTreasureInfo.treasureInfo.nAddPrice * System.Convert.ToInt32(CGameColorFishMgr.Ins.pStaticConfig.GetInt("���Ӽ۱���") * 0.01f))
            {
                UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
                if (uiGameInfo == null || !uiGameInfo.IsOpen()) return;
                UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(baseInfo.uid);
                if (uiUnitInfo == null) return;
                uiUnitInfo.SetDmContent("���۲��ɳ���10���Ӽ�");
                return;
            }
        }
        ///�ж�ʱ���Ƿ���5s�ڣ�5s�ڲ��ɾ���
        if(!CNPCMgr.Ins.pAuctionUnit.bCanPaiMaiTime())
        {
            return;
        }

        if (emAuctionState == EMAuctionState.Normal)
            return;

        //��Ǯ
        bool bVtb = baseInfo.emUserType == CPlayerBaseInfo.EMUserType.Zhubo;

        CPlayerNetHelper.AddFishCoin(baseInfo.uid,
                                     -nBuyPrice,
                                     EMFishCoinAddFunc.Auction,
                                     false,
                                     new HHandlerAuction(price));
    }

    public void SetPlayer(CPlayerBaseInfo baseInfo, long price)
    {
        //if (nBuyPrice >= price ||
        //    price - nBuyPrice < curTreasureInfo.nAddPrice)
        //{
        //    UIToast.Show("���۹���");
        //    return;
        //}

        if(curTreasureInfo.treasureInfo.emAuctionLimitType == EMAuctionLimitType.Single)
        {
            ///�ж�֮ǰ�Ƿ��о�����
            if (pCurAuctionPlayerInfo != null)
            {
                //��Ǯ
                bool bAddVtb = pCurAuctionPlayerInfo.emUserType == CPlayerBaseInfo.EMUserType.Zhubo;

                CPlayerNetHelper.AddFishCoin(pCurAuctionPlayerInfo.uid,
                                             nBuyPrice,
                                             EMFishCoinAddFunc.Auction,
                                             false);
            }
            pCurAuctionPlayerInfo = baseInfo;
        }
        else if (curTreasureInfo.treasureInfo.emAuctionLimitType == EMAuctionLimitType.Multiple)
        {
            for (int i = 0; i < listAuctionPlayer.Count; i++)
            {
                if (listAuctionPlayer[i].uid == baseInfo.uid)
                    return;
            }
            listAuctionPlayer.Add(baseInfo);
        }

        nBuyPrice = price;
        if (emAuctionState == EMAuctionState.StartAuction)
        {
            emAuctionState = EMAuctionState.Auctioning;
        }
        deleAuctionChgInfo?.Invoke(baseInfo.uid, price);
        //if (CNPCMgr.Ins.pAuctionUnit.bCanAddStayTime())
        //{
        //    CNPCMgr.Ins.pAuctionUnit.AddStayTime((float)CGameColorFishMgr.Ins.pStaticConfig.GetInt("��������ʱ��"));
        //}
        //pInfo = baseInfo;
    }

    /// <summary>
    /// ��ʼ����
    /// </summary>
    public void StartAuction()
    {
        UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
        UIAuction auction = UIManager.Instance.GetUI(UIResType.Auction) as UIAuction;
        if (roomInfo == null ||
            auction == null)
            return;
        ///��ȡ������Ϣ
        GetInfoByAuction();

        //roomInfo.HideSpecialty();
        ///����ui����
        auction.Show();
        roomInfo.HideBoard(roomInfo.uiTopBtnTween, 0.5f, 0f);
        UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (gameInfo != null)
        {
            gameInfo.HideBatteryBoard(0.5f, 0f);
        }
        UICrazyGiftTip crazyGiftTip = UIManager.Instance.GetUI(UIResType.CrazyGiftTip) as UICrazyGiftTip;
        if (crazyGiftTip != null)
        {
            crazyGiftTip.ShowPosByAuction(0.5f);
        }
        UICrazyTime crazyTime = UIManager.Instance.GetUI(UIResType.CrazyTime) as UICrazyTime;
        if (crazyTime != null)
        {
            crazyTime.ShowPosByAuction(0.5f);
        }
        bAuction = true;
    }

    /// <summary>
    /// չʾ���Ļ�õı���
    /// </summary>
    void ShowBox()
    {
        CResLoadMgr.Inst.SynLoad(curTreasureInfo.treasureInfo.szPrefab, CResLoadMgr.EM_ResLoadType.CanbeUnloadAssetbundle,
        delegate (Object res, object data, bool bSuc)
        {
            GameObject objBoxRoot = res as GameObject;
            if (objBoxRoot == null) return;
            GameObject objNewBox = GameObject.Instantiate(objBoxRoot) as GameObject;
            Transform tranNewBox = objNewBox.GetComponent<Transform>();

            ///���ñ���Ŀ���
            Vector3 vTarget = Vector3.zero;
            if(curTreasureInfo.treasureInfo.emAuctionLimitType == EMAuctionLimitType.Single)
            {
                CPlayerUnit playerUnit = CPlayerMgr.Ins.GetIdleUnit(pCurAuctionPlayerInfo.uid);// CPlayerMgr.Ins.GetIdleUnit(pInfo.uid);\
                if (playerUnit == null)
                {
                    vTarget = CGameColorFishMgr.Ins.pMap.GetRandomGachaPos().position;
                }
                else
                {
                    vTarget = playerUnit.tranGachePos.position;
                }
            }
            else if (curTreasureInfo.treasureInfo.emAuctionLimitType == EMAuctionLimitType.Multiple)
            {
                vTarget = CNPCMgr.Ins.pAuctionUnit.tranStayPos.position;
            }

            tranNewBox.SetParent(null);
            tranNewBox.position = CNPCMgr.Ins.pAuctionUnit.tranGachePos.position;
            tranNewBox.localScale = Vector3.one;
            tranNewBox.localRotation = Quaternion.identity;

            CAuctionBox pBox = objNewBox.GetComponent<CAuctionBox>();
            if (pBox == null)
            {
                Destroy(objNewBox);
                return;
            }

            //����¼�
            List<CPlayerBaseInfo> listAddPointPlayer = new List<CPlayerBaseInfo>();
            listAddPointPlayer.AddRange(listAuctionPlayer);
            CPlayerBaseInfo pCurBaseInfo = pCurAuctionPlayerInfo;
            delShowBoxOverChg = delegate ()
            {
                ///���ӻ���
                if (curTreasureInfo.treasureInfo.emAuctionLimitType == EMAuctionLimitType.Single &&
                    pCurBaseInfo != null)
                {
                    string uid = pCurBaseInfo.uid;
                    CPlayerNetHelper.AddFishCoin(uid,
                                                 auctionResultInfo.nlFishCoin,
                                                 EMFishCoinAddFunc.Auction,
                                                 false);
                    //׬�˾ͼ�¼һ�¸�������
                    long nCoinLerp = auctionResultInfo.nlFishCoin - nBuyPrice;
                    if (nCoinLerp > 0)
                    {
                        CPlayerNetHelper.AddWinnerInfo(uid, 0, nCoinLerp);
                    }
                    CFishRecordMgr.Ins.AddRecord(auctionResultInfo.nlFishCoin - nBuyPrice, pCurBaseInfo);
                }
                else if (curTreasureInfo.treasureInfo.emAuctionLimitType == EMAuctionLimitType.Multiple &&
                         listAddPointPlayer != null)
                {
                    for (int i = 0; i < listAddPointPlayer.Count; i++)
                    {
                        if (listAddPointPlayer[i] == null) continue;
                        string uid = listAddPointPlayer[i].uid;
                        CPlayerNetHelper.AddFishCoin(uid,
                                                     auctionResultInfo.nlFishCoin,
                                                     EMFishCoinAddFunc.Auction,
                                                     false);
                        //׬�˾ͼ�¼һ�¸�������
                        long nCoinLerp = auctionResultInfo.nlFishCoin - nBuyPrice;
                        if (nCoinLerp > 0)
                        {
                            CPlayerNetHelper.AddWinnerInfo(uid, 0, nCoinLerp);
                        }
                        CFishRecordMgr.Ins.AddRecord(auctionResultInfo.nlFishCoin - nBuyPrice, listAddPointPlayer[i]);
                    }
                }
            };

            ///��ȡ���⽱��
            CPlayerBaseInfo pInfo = CPlayerMgr.Ins.GetPlayer(auctionResultInfo.nlPlayerUID);
            if (auctionResultInfo.dropInfo != null &&
                pInfo != null)
            {
                int nValue = auctionResultInfo.dropInfo.nValue;
                if (auctionResultInfo.dropInfo.nType == 0)
                {
                    //�ӽ�ɫ
                    CHttpParam pReqParams = new CHttpParam
                      (
                         new CHttpParamSlot("uid", pInfo.uid.ToString()),
                         new CHttpParamSlot("avatarId", nValue.ToString())
                      );

                    CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugSendAvatar, new HHandlerRefreshRolePack(), pReqParams);
                }
                else if (auctionResultInfo.dropInfo.nType == 1)
                {
                    //�Ӵ�
                    CPlayerNetHelper.SendUserBoat(pInfo.uid, nValue, new HHandlerRefreshBoatPack());
                }
                else if (auctionResultInfo.dropInfo.nType == 2)
                {
                    //�����
                    CPlayerNetHelper.AddFishItemPack(pInfo.uid, nValue, nValue, nValue, 0);
                }
                else if (auctionResultInfo.dropInfo.nType == 5)
                {
                    //�ӷ���
                    long curTimeStamp = CGameColorFishMgr.Ins.GetNowServerTime();
                    CHttpParam pReqParams = new CHttpParam
                    (
                        new CHttpParamSlot("uid", pInfo.uid.ToString()),
                        new CHttpParamSlot("itemType", EMGiftType.fishLun.ToString()),
                        new CHttpParamSlot("count", nValue.ToString()),
                        new CHttpParamSlot("time", curTimeStamp.ToString()),
                        new CHttpParamSlot("nonce", CEncryptHelper.AesEncrypt(pInfo.uid.ToString() + CDanmuSDKCenter.Ins.szRoomId.ToString() + curTimeStamp) + CHelpTools.GetRandomString(8, true, true, true, false, ""))
                    );

                    CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddGiftCount, pReqParams, CHttpMgr.Instance.nReconnectTimes, true);
                }
                else if (auctionResultInfo.dropInfo.nType == 3)
                {
                    //�Ӳ���
                    int nMatID = 0;
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
                    if (nMatID != 0)
                    {
                        CPlayerNetHelper.AddFishMat(pInfo.uid, nMatID, nValue, 0, new HHandlerAddFishMat());
                    }
                }
                else if (auctionResultInfo.dropInfo.nType == 4)
                {
                    //��ָ������
                    int nMatID = 0;
                    ///�Ѿ�����ָ���� Ĭ��ת��Ϊ��ͼ����
                    if (pInfo.pBoatPack.GetInfo(CGameColorFishMgr.Ins.pStaticConfig.GetInt("�ض��һ���ID")) != null)
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
                        nMatID = CGameColorFishMgr.Ins.pStaticConfig.GetInt("���������ض�����ID");
                    }
                    if (nMatID != 0)
                    {
                        CPlayerNetHelper.AddFishMat(pInfo.uid, nMatID, nValue, 0, new HHandlerAddFishMat());
                    }
                }
            }

            pBox.Init(listAuctionInfos, 
                      curTreasureInfo.treasureInfo.emAuctionLimitType == EMAuctionLimitType.Single ? auctionResultInfo.nlPlayerUID : "", 
                      bCri,
                      vTarget);
            pBox.dlgOver = ShowResultUI;
        });
    }

    void ShowResultUI(long guid)
    {
        //delShowBoxOverChg?.Invoke();
        UIManager.Instance.OpenUI(UIResType.AuctionResult);
        UIAuctionResult auctionResult = UIManager.Instance.GetUI(UIResType.AuctionResult) as UIAuctionResult;
        if(auctionResult != null)
        {
            auctionResult.SetInfo(curTreasureInfo.treasureInfo, auctionResultInfo, delShowBoxOverChg);
        }
    }

    /// <summary>
    /// ��ȡ���Ľ�Ʒ��Ϣ
    /// </summary>
    public void GetAuctionInfo()
    {
        auctionResultInfo = new CAuctionResultTreasureInfo();
        bCri = false;
        listAuctionInfos.Clear();

        List<CPlayerBaseInfo> listPool = new List<CPlayerBaseInfo>();
        auctionResultInfo.nlPlayerUID = string.Empty;
        auctionResultInfo.nlBuyPrice = nBuyPrice;
        auctionResultInfo.nlFishCoin = 0;
        auctionResultInfo.listFishInfo = new List<CFishInfo>();
        auctionResultInfo.dropInfo = null;
        auctionResultInfo.listPlayers = new List<CPlayerBaseInfo>();
        if (curTreasureInfo.treasureInfo.emAuctionLimitType == EMAuctionLimitType.Single)
        {
            auctionResultInfo.nlPlayerUID = pCurAuctionPlayerInfo.uid;
        }
        else if (curTreasureInfo.treasureInfo.emAuctionLimitType == EMAuctionLimitType.Multiple)
        {
            listPool.AddRange(listAuctionPlayer);
            auctionResultInfo.listPlayers.AddRange(listAuctionPlayer);
        }

        UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (gameInfo != null)
        {
            ///��ȡ�����Ϣ
            List<CFishInfo> fishInfos = curTreasureInfo.treasureInfo.GetFish(nAuctionCount);
            for (int i = 0; i < fishInfos.Count; i++)
            {
                if (fishInfos[i] == null) continue;
                auctionResultInfo.listFishInfo.Add(fishInfos[i]);
                auctionResultInfo.nlFishCoin += fishInfos[i].lPrice;
                CAuctionInfo auctionInfo = new CAuctionInfo();
                auctionInfo.fishInfo = fishInfos[i];
                listAuctionInfos.Add(auctionInfo);
            }
            bool bGetOther = curTreasureInfo.treasureInfo.CheckGetOther();
            ///��ȡ��ɫ/������Ϣ
            if (bGetOther)
            {
                CAuctionInfo auctionInfo = new CAuctionInfo();
                COtherDropInfo dropInfo = curTreasureInfo.treasureInfo.GetOther();
                auctionInfo.otherDropInfo = dropInfo;
                if (dropInfo.nType == 0)
                {
                    if (curTreasureInfo.treasureInfo.emAuctionLimitType == EMAuctionLimitType.Multiple)
                    {
                        for (int i = 0; i < listPool.Count;)
                        {
                            if (listPool[i] == null ||
                                listPool[i].pAvatarPack.GetInfo(dropInfo.nValue) != null)
                            {
                                listPool.RemoveAt(0);
                            }
                            else
                            {
                                i++;
                            }
                        }
                        if (listPool.Count > 0)
                        {
                            int nRandomIdx = Random.Range(0, listPool.Count);
                            auctionResultInfo.nlPlayerUID = listPool[nRandomIdx].uid;
                            auctionResultInfo.dropInfo = dropInfo;
                        }
                    }
                    else if (curTreasureInfo.treasureInfo.emAuctionLimitType == EMAuctionLimitType.Single)
                    {
                        if (pCurAuctionPlayerInfo.pAvatarPack.GetInfo(dropInfo.nValue) == null)
                        {
                            auctionResultInfo.dropInfo = dropInfo;
                        }
                    }
                }
                else if (dropInfo.nType == 1)
                {
                    if (curTreasureInfo.treasureInfo.emAuctionLimitType == EMAuctionLimitType.Multiple)
                    {
                        for (int i = 0; i < listPool.Count;)
                        {
                            if (listPool[i] == null ||
                                listPool[i].pBoatPack.GetInfo(dropInfo.nValue) != null)
                            {
                                listPool.RemoveAt(0);
                            }
                            else
                            {
                                i++;
                            }
                        }
                        if (listPool.Count > 0)
                        {
                            int nRandomIdx = Random.Range(0, listPool.Count);
                            auctionResultInfo.nlPlayerUID = listPool[nRandomIdx].uid;
                            auctionResultInfo.dropInfo = dropInfo;
                        }
                    }
                    else if (curTreasureInfo.treasureInfo.emAuctionLimitType == EMAuctionLimitType.Single)
                    {
                        if (pCurAuctionPlayerInfo.pBoatPack.GetInfo(dropInfo.nValue) == null)
                        {
                            auctionResultInfo.dropInfo = dropInfo;
                        }
                    }
                }
                else if (dropInfo.nType == 2 ||
                         dropInfo.nType == 3 ||
                         dropInfo.nType == 4 ||
                         dropInfo.nType == 5)
                {
                    if (curTreasureInfo.treasureInfo.emAuctionLimitType == EMAuctionLimitType.Multiple)
                    {
                        if (listPool.Count > 0)
                        {
                            int nRandomIdx = Random.Range(0, listPool.Count);
                            auctionResultInfo.nlPlayerUID = listPool[nRandomIdx].uid;
                        }
                    }
                    auctionResultInfo.dropInfo = dropInfo;
                }
            }
            /////չʾ��Ϣ
            //AuctionCastInfo castInfo = new AuctionCastInfo();
            //castInfo.playerInfo = pInfo;
            //castInfo.nlTotalReward = nBuyPrice;
            /////�жϻ�ȡ����Ʒ�Ƿ�Ϊϡ������
            //UISpecialCast.ShowAuctionInfo(castInfo);

        }
        if (auctionResultInfo.nlFishCoin < 0)
        {
            auctionResultInfo.nlFishCoin = 0;
        }

    }

    /// <summary>
    /// ��������
    /// </summary>
    public void StopAuction()
    {
        UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
        UIAuction auction = UIManager.Instance.GetUI(UIResType.Auction) as UIAuction;
        if (roomInfo == null ||
            auction == null)
            return;
        //roomInfo.ShowSpecialty();
        //ui��������
        auction.Hide();
        roomInfo.ShowBoard(roomInfo.uiTopBtnTween, 0.5f, 0.5f);
        UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (gameInfo != null)
        {
            gameInfo.ShowBatteryBoard(0.5f, 0.5f);
        }
        UICrazyGiftTip crazyGiftTip = UIManager.Instance.GetUI(UIResType.CrazyGiftTip) as UICrazyGiftTip;
        if (crazyGiftTip != null)
        {
            crazyGiftTip.BackOriPos(0f);
        }
        UICrazyTime crazyTime = UIManager.Instance.GetUI(UIResType.CrazyTime) as UICrazyTime;
        if (crazyTime != null)
        {
            crazyTime.BackOriPos(0f);
        }
        ///չʾ��Ϣ
        bool bShowInfo = false;
        if (curTreasureInfo.treasureInfo.emAuctionLimitType == EMAuctionLimitType.Single &&
            pCurAuctionPlayerInfo != null)
        {
            bShowInfo = true;
        }
        else if (curTreasureInfo.treasureInfo.emAuctionLimitType == EMAuctionLimitType.Multiple && 
                 listAuctionPlayer.Count > 0)
        {
            bShowInfo = true;
        }
        if(bShowInfo)
        {
            GetAuctionInfo();
            ShowBox();
        }
        ///����״̬
        bAuction = false;
        emAuctionState = EMAuctionState.Normal;
        listAuctionPlayer.Clear();
        pCurAuctionPlayerInfo = null;
    }


}
