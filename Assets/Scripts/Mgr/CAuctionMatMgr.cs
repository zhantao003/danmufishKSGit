using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAuctionMatMgr : MonoBehaviour
{
    static CAuctionMatMgr ins = null;

    public static CAuctionMatMgr Ins
    {
        get
        {
            return ins;
        }
    }

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
    public ST_AuctionMaterialInfo curTreasureInfo;
    /// <summary>
    /// ��ǰ�ľ�������Ϣ
    /// </summary>
    public CPlayerBaseInfo pInfo;
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
    /// ��ǰ���ѵĲ���ID
    /// </summary>
    public int nPayMatID;
    /// <summary>
    /// ��ǰ�ɻ�ȡ�Ĳ���ID
    /// </summary>
    public int nGetMatID;
    /// <summary>
    /// ��ǰ�ɻ�ȡ�Ĳ���ͼƬ
    /// </summary>
    public string szGetMatIcon;

    /// <summary>
    /// ������Ϣ�ı�
    /// </summary>
    public DelegateSLFuncCall deleAuctionChgInfo;

    string szShowEff = "Effect/effMat_throw";

    private void Awake()
    {
        ins = this;
    }


    void GetInfoByAuction()
    {
        curTreasureInfo = null;
        pInfo = null;
        int nTotalWeight = 0;
        ///�������ı�����Ϣ
        List<ST_AuctionMaterialInfo> listInfos = CTBLHandlerAuctionMaterialInfo.Ins.GetInfos();
        for (int i = 0; i < listInfos.Count; i++)
        {
            nTotalWeight += listInfos[i].nWeight;
        }
        int nRandomValue = Random.Range(0, nTotalWeight + 1);
        for (int i = 0; i < listInfos.Count; i++)
        {
            if (listInfos[i].nWeight <= 0)
                continue;
            nRandomValue -= listInfos[i].nWeight;
            if (nRandomValue > 0)
                continue;
            curTreasureInfo = listInfos[i];
            break;
        }

        nNormalPrice = curTreasureInfo.GetBuyPrice();
        nBuyPrice = curTreasureInfo.GetPriceByBuyRange((int)nNormalPrice);
        nBaseCount = curTreasureInfo.nMatNum;
        emAuctionState = EMAuctionState.StartAuction;

        if(curTreasureInfo.emPayType == EMPayType.Mat)
        {
            nPayMatID = 0;
            List<ST_FishInfo> listFishInfo = CGameColorFishMgr.Ins.pMap.pTBLHandlerBoomFishInfo.GetInfos();
            for (int i = 0; i < listFishInfo.Count; i++)
            {
                if (listFishInfo[i].emItemType == EMItemType.FishMat)
                {
                    if (int.TryParse(listFishInfo[i].szIcon, out nPayMatID))
                    {
                        break;
                    }

                }
            }
        }

        if (curTreasureInfo.emMatType == EMMaterialType.Material)
        {
            nGetMatID = 0;
            List<ST_FishInfo> listFishInfo = CGameColorFishMgr.Ins.pMap.pTBLHandlerBoomFishInfo.GetInfos();
            for (int i = 0; i < listFishInfo.Count; i++)
            {
                if (listFishInfo[i].emItemType == EMItemType.FishMat)
                {
                    if (int.TryParse(listFishInfo[i].szIcon, out nGetMatID))
                    {
                        break;
                    }

                }
            }
        }
        else if (curTreasureInfo.emMatType == EMMaterialType.TargetMaterial)
        {
            nGetMatID = CGameColorFishMgr.Ins.pStaticConfig.GetInt("���������ض�����ID");
        }

        UIMatAuction auction = UIManager.Instance.GetUI(UIResType.MatAuction) as UIMatAuction;
        if (auction != null)
        {
            auction.SetInfo();
        }
    }

    public void SetPlayerByHttp(CPlayerBaseInfo baseInfo, long price)
    {
        if (curTreasureInfo == null)
            return;
        if(curTreasureInfo.emMatType == EMMaterialType.Role)
        {
            if (baseInfo.pAvatarPack.GetInfo(curTreasureInfo.nMatID) != null)
            {
                UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
                if (uiGameInfo == null || !uiGameInfo.IsOpen()) return;
                UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(baseInfo.uid);
                if (uiUnitInfo == null) return;
                uiUnitInfo.SetDmContent("��ӵ��,�޷�����");
                return; 
            }
        }
        else if(curTreasureInfo.emMatType == EMMaterialType.Boat)
        {
            if (baseInfo.pBoatPack.GetInfo(curTreasureInfo.nMatID) != null)
            {
                UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
                if (uiGameInfo == null || !uiGameInfo.IsOpen()) return;
                UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(baseInfo.uid);
                if (uiUnitInfo == null) return;
                uiUnitInfo.SetDmContent("��ӵ��,�޷�����");
                return;
            }
        }
        ///�ж��Ƿ�ﵽ��֧��������
        if (!CheckPlayerPayInfo(baseInfo, price))
        {
            return;
        }
        if (emAuctionState == EMAuctionState.Normal)
        {
            return;
        }
            

        CostPayInfo(baseInfo, price);
    }

    /// <summary>
    /// �ж��Ƿ�ﵽ��֧��������
    /// </summary>
    /// <param name="baseInfo"></param>
    /// <param name="price"></param>
    /// <returns></returns>
    public bool CheckPlayerPayInfo(CPlayerBaseInfo baseInfo, long price)
    {

        bool bEnough = true;
        ///��ȡ��ǰ������ϴ��ڵ�����
        long nlCurPlayerHavePayCount = 0;
        switch (curTreasureInfo.emPayType)
        {
            case EMPayType.FishCoin:
                {
                    nlCurPlayerHavePayCount = baseInfo.GameCoins;
                }
                break;
            case EMPayType.HaiDaoCoin:
                {
                    nlCurPlayerHavePayCount = baseInfo.TreasurePoint;
                }
                break;
            case EMPayType.Mat:
                {
                    nlCurPlayerHavePayCount = baseInfo.pMatPack.GetItem(nPayMatID);
                    if (nlCurPlayerHavePayCount < 0)
                        nlCurPlayerHavePayCount = 0;
                }
                break;
        }
        if(nlCurPlayerHavePayCount < price)
        {
            UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
            if (uiGameInfo == null || !uiGameInfo.IsOpen()) return false;
            UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(baseInfo.uid);
            if (uiUnitInfo == null) return false;
            switch (curTreasureInfo.emPayType)
            {
                case EMPayType.FishCoin:
                    {
                        uiUnitInfo.SetDmContent("���ֲ���");
                    }
                    break;
                case EMPayType.HaiDaoCoin:
                    {
                        uiUnitInfo.SetDmContent("������Ҳ���");
                    }
                    break;
                case EMPayType.Mat:
                    {
                        ST_FishMat fishMat = CTBLHandlerFishMaterial.Ins.GetInfo(CAuctionMatMgr.Ins.nPayMatID);
                        if(fishMat != null)
                        {
                            uiUnitInfo.SetDmContent(fishMat.szName + "����");
                        }
                    }
                    break;
            }
            bEnough = false;
        }
        else if (nBuyPrice >= price ||
            price - nBuyPrice < curTreasureInfo.nAddPrice)
        {
            UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
            if (uiGameInfo == null || !uiGameInfo.IsOpen()) return false;
            UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(baseInfo.uid);
            if (uiUnitInfo == null) return false;
            switch (curTreasureInfo.emPayType)
            {
                case EMPayType.FishCoin:
                    {
                        uiUnitInfo.SetDmContent("���۹���");
                    }
                    break;
                case EMPayType.HaiDaoCoin:
                    {
                        uiUnitInfo.SetDmContent("���۹���");
                    }
                    break;
                case EMPayType.Mat:
                    {
                        uiUnitInfo.SetDmContent("���۹���");
                    }
                    break;
            }
            bEnough = false;
        }
        else if (price - nBuyPrice > curTreasureInfo.nAddPrice * System.Convert.ToInt32(CGameColorFishMgr.Ins.pStaticConfig.GetInt("���Ӽ۱���") * 0.01f))
        {
            UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
            if (uiGameInfo == null || !uiGameInfo.IsOpen()) return false;
            UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(baseInfo.uid);
            if (uiUnitInfo == null) return false;
            switch (curTreasureInfo.emPayType)
            {
                case EMPayType.FishCoin:
                    {
                        uiUnitInfo.SetDmContent("���۲��ɳ���10���Ӽ�");
                    }
                    break;
                case EMPayType.HaiDaoCoin:
                    {
                        uiUnitInfo.SetDmContent("���۲��ɳ���10���Ӽ�");
                    }
                    break;
                case EMPayType.Mat:
                    {
                        uiUnitInfo.SetDmContent("���۲��ɳ���10���Ӽ�");
                    }
                    break;
            }
            bEnough = false;
        }

        return bEnough;
    }

    public void CostPayInfo(CPlayerBaseInfo baseInfo, long value)
    {
        switch (curTreasureInfo.emPayType)
        {
            case EMPayType.FishCoin:
                {
                    //��Ǯ
                    CPlayerNetHelper.AddFishCoin(baseInfo.uid,
                                                 -value,
                                                 EMFishCoinAddFunc.Auction,
                                                 true,
                                                 new HHandlerMatAuction(value));
                }
                break;
            case EMPayType.HaiDaoCoin:
                {
                    //���ػ���
                    CPlayerNetHelper.AddTreasureCoin(baseInfo.uid, -value, new HHandlerAddTreasurePointByAuction(value));
                }
                break;
            case EMPayType.Mat:
                {
                    //����
                    CPlayerNetHelper.AddFishMat(pInfo.uid, nPayMatID, -value, 0, new HHandlerAddFishMatByAuction(value));
                }
                break;
        }
    }

    public void SetPlayer(CPlayerBaseInfo baseInfo, long price)
    {
        ///�ж�֮ǰ�Ƿ��о�����
        if (pInfo != null)
        {
            switch (curTreasureInfo.emPayType)
            {
                case EMPayType.FishCoin:
                    {
                        //Ǯ
                        CPlayerNetHelper.AddFishCoin(pInfo.uid,
                                                     nBuyPrice,
                                                     EMFishCoinAddFunc.Auction,
                                                     false);
                    }
                    break;
                case EMPayType.HaiDaoCoin:
                    {
                        //���ػ���
                        CPlayerNetHelper.AddTreasureCoin(pInfo.uid, nBuyPrice);
                    }
                    break;
                case EMPayType.Mat:
                    {
                        //����
                        CPlayerNetHelper.AddFishMat(pInfo.uid, nPayMatID, nBuyPrice, 0);
                    }
                    break;
            }
            ////��Ǯ
            //CPlayerNetHelper.AddFishCoin(pInfo.uid,
            //                             nBuyPrice,
            //                             EMFishCoinAddFunc.Auction);
        }
        nBuyPrice = price;
        if (emAuctionState == EMAuctionState.StartAuction)
        {
            emAuctionState = EMAuctionState.Auctioning;
        }
        deleAuctionChgInfo?.Invoke(baseInfo.uid, price);
        if (CNPCMgr.Ins.pMatAuctionUnit.bCanAddStayTime())
        {
            CNPCMgr.Ins.pMatAuctionUnit.AddStayTime((float)CGameColorFishMgr.Ins.pStaticConfig.GetInt("��������ʱ��"));
        }
        pInfo = baseInfo;
    }

    /// <summary>
    /// ��ʼ����
    /// </summary>
    public void StartAuction()
    {
        UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
        UIMatAuction auction = UIManager.Instance.GetUI(UIResType.MatAuction) as UIMatAuction;
        if (roomInfo == null ||
            auction == null)
            return;

        GetInfoByAuction();

        roomInfo.HideSpecialty();
        auction.Show();
        bAuction = true;
    }

    public void AddReward(string uid)
    {
        if (curTreasureInfo == null) return;
        switch (curTreasureInfo.emMatType)
        {
            case EMMaterialType.Material:
                {
                    if (nGetMatID == 0)
                        return;
                    //CHttpParam pReqParams = new CHttpParam
                    //(
                    //   new CHttpParamSlot("uid", uid.ToString()),
                    //   new CHttpParamSlot("itemId", nGetMatID.ToString()),
                    //   new CHttpParamSlot("count", curTreasureInfo.nMatNum.ToString()),
                    //   new CHttpParamSlot("dailyGet", "0")
                    //);
                    //CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddUserFishMat, new HHandlerAddFishMat(), pReqParams, 0, true);
                    CPlayerNetHelper.AddFishMat(uid, nGetMatID, curTreasureInfo.nMatNum, 0, new HHandlerAddFishMat());
                }
                break;
            case EMMaterialType.TargetMaterial:
                {
                    if (nGetMatID == 0)
                        return;

                    CPlayerBaseInfo pPlayerInfo = CPlayerMgr.Ins.GetPlayer(uid);
                    if (pPlayerInfo == null) return;

                    if (pPlayerInfo.pBoatPack != null &&
                        pPlayerInfo.pBoatPack.GetInfo(CGameColorFishMgr.Ins.pStaticConfig.GetInt("�ض��һ���ID")) != null)
                    {
                        List<ST_FishInfo> listFishInfo = CGameColorFishMgr.Ins.pMap.pTBLHandlerBoomFishInfo.GetInfos();
                        for (int i = 0; i < listFishInfo.Count; i++)
                        {
                            if (listFishInfo[i].emItemType == EMItemType.FishMat)
                            {
                                if (int.TryParse(listFishInfo[i].szIcon, out nGetMatID))
                                {
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        nGetMatID = CGameColorFishMgr.Ins.pStaticConfig.GetInt("���������ض�����ID");
                    }
                    //CHttpParam pReqParams = new CHttpParam
                    //(
                    //   new CHttpParamSlot("uid", uid.ToString()),
                    //   new CHttpParamSlot("itemId", nGetMatID.ToString()),
                    //   new CHttpParamSlot("count", curTreasureInfo.nMatNum.ToString()),
                    //   new CHttpParamSlot("dailyGet", "0")
                    //);
                    //CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddUserFishMat, new HHandlerAddFishMat(), pReqParams, 0, true);
                    CPlayerNetHelper.AddFishMat(uid, nGetMatID, curTreasureInfo.nMatNum, 0, new HHandlerAddFishMat());
                }
                break;
            case EMMaterialType.FishPack:
                {
                    //CHttpParam pReqParams = new CHttpParam
                    //(
                    //   new CHttpParamSlot("uid", uid.ToString()),
                    //   new CHttpParamSlot("baitCount", curTreasureInfo.nMatNum.ToString()),
                    //   new CHttpParamSlot("ganCount", curTreasureInfo.nMatNum.ToString()),
                    //   new CHttpParamSlot("fupiaoCount", curTreasureInfo.nMatNum.ToString()),
                    //   new CHttpParamSlot("lunCount", "0"),
                    //   new CHttpParamSlot("time", CGameColorFishMgr.Ins.GetNowServerTime().ToString())
                    //);
                    //CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddFishItemPack, pReqParams, CHttpMgr.Instance.nReconnectTimes, true);
                    CPlayerNetHelper.AddFishItemPack(uid, curTreasureInfo.nMatNum, curTreasureInfo.nMatNum, curTreasureInfo.nMatNum, 0);
                }
                break;
            case EMMaterialType.FishLun:
                {
                    long curTimeStamp = CGameColorFishMgr.Ins.GetNowServerTime();
                    CHttpParam pReqParams = new CHttpParam
                    (
                        new CHttpParamSlot("uid", uid.ToString()),
                        new CHttpParamSlot("itemType", EMGiftType.fishLun.ToString()),
                        new CHttpParamSlot("count", curTreasureInfo.nMatNum.ToString()),
                        new CHttpParamSlot("time", curTimeStamp.ToString()),
                        new CHttpParamSlot("nonce", CEncryptHelper.AesEncrypt(uid.ToString() + CDanmuSDKCenter.Ins.szRoomId.ToString() + curTimeStamp))
                    );
                    CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddGiftCount, pReqParams, CHttpMgr.Instance.nReconnectTimes, true);
                }
                break;
            case EMMaterialType.BBBoom:
                {
                    CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
                    CPlayerBaseInfo baseInfo = CPlayerMgr.Ins.GetPlayer(uid);
                    if (pUnit == null)
                    {
                        pUnit = CPlayerMgr.Ins.GetActiveUnit(uid);
                        if (pUnit == null)
                        {
                            return;
                        }
                    }
                    pUnit.AddBoom(curTreasureInfo.nMatNum);
                    //if (baseInfo.CheckHaveGift())
                    //{
                    //    pUnit.ClearExitTick();
                    //}
                    //else
                    //{
                    //    pUnit.ResetExitTick();
                    //}
                }
                break;
            case EMMaterialType.HaiDaoGold:
                {
                    //���ӱ��ػ���
                    //CHttpParam pReqParams = new CHttpParam
                    //(
                    //    new CHttpParamSlot("uid", uid.ToString()),
                    //    new CHttpParamSlot("treasurePoint", curTreasureInfo.nMatNum.ToString())
                    //);
                    //CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddUserTreasurePoint, pReqParams, CHttpMgr.Instance.nReconnectTimes, true);

                    CPlayerNetHelper.AddTreasureCoin(uid, curTreasureInfo.nMatNum);
                }
                break;
            case EMMaterialType.Role:
                {
                    //�ӽ�ɫ
                    CHttpParam pReqParams = new CHttpParam
                      (
                         new CHttpParamSlot("uid", uid.ToString()),
                         new CHttpParamSlot("avatarId", curTreasureInfo.nMatID.ToString())
                      );

                    CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugSendAvatar, new HHandlerRefreshRolePack(), pReqParams);
                }
                break;
            case EMMaterialType.Boat:
                {
                    //�Ӵ�
                    CPlayerNetHelper.SendUserBoat(uid, curTreasureInfo.nMatID, new HHandlerRefreshBoatPack());
                }
                break;
        }
    }

    /// <summary>
    /// չʾ���Ļ�õı���
    /// </summary>
    void ShowBox()
    {
        Vector3 vShowPostion = CNPCMgr.Ins.pMatAuctionUnit.tranSelf.position;
        CEffectMgr.Instance.CreateEffSync(szShowEff, vShowPostion, Quaternion.identity, 0,
        delegate (GameObject value)
        {
            string uid = pInfo.uid;
            CEffectBeizier pEffBeizier = value.GetComponent<CEffectBeizier>();
            if (pEffBeizier == null) return;
            Transform tranTargetSlot = null;
            CPlayerUnit playerUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
            if (playerUnit == null)
            {
                tranTargetSlot = CGameColorFishMgr.Ins.pMap.GetRandomGachaPos();
            }
            else
            {
                tranTargetSlot = playerUnit.tranGachePos;
            }
            if (tranTargetSlot == null)
            {
                Debug.LogError("�쳣�ı����");
                return;
            }
            ///��ȡ��Ӧ���͵Ĳ��ʲ�����
            Texture texture = null;
            szGetMatIcon = string.Empty;
            if(curTreasureInfo.emMatType == EMMaterialType.Material ||
               curTreasureInfo.emMatType == EMMaterialType.TargetMaterial)
            {
                if (nGetMatID == 0)
                    return;
                ST_FishMat fishMat = CTBLHandlerFishMaterial.Ins.GetInfo(nGetMatID);
                if (fishMat == null)
                    return;
                szGetMatIcon = fishMat.szIcon;
            }
            else
            {
                szGetMatIcon = curTreasureInfo.szIcon;
            }
            texture = Resources.Load(szGetMatIcon) as Texture;
            //if (texture != null)
            //{
            //    Debug.LogError(texture.name + "===Texture");
            //}
            ///�Ѿ�����ָ���� Ĭ��ת��Ϊ��ͼ����
            if(curTreasureInfo.emMatType == EMMaterialType.TargetMaterial &&
               pInfo.pBoatPack.GetInfo(CGameColorFishMgr.Ins.pStaticConfig.GetInt("�ض��һ���ID")) != null)
            {
                nGetMatID = 0;
                List<ST_FishInfo> listFishInfo = CGameColorFishMgr.Ins.pMap.pTBLHandlerBoomFishInfo.GetInfos();
                for (int i = 0; i < listFishInfo.Count; i++)
                {
                    if (listFishInfo[i].emItemType == EMItemType.FishMat)
                    {
                        if (int.TryParse(listFishInfo[i].szIcon, out nGetMatID))
                        {
                            break;
                        }
                    }
                }
            }

            bool bMaterial = curTreasureInfo.emMatType == EMMaterialType.Material || curTreasureInfo.emMatType == EMMaterialType.TargetMaterial;
            ///չʾ��Ϣ
            MatAuctionCastInfo castInfo = new MatAuctionCastInfo();
            castInfo.playerInfo = pInfo;
            castInfo.emMatType = curTreasureInfo.emMatType;
            castInfo.nMatNum = curTreasureInfo.nMatNum;
            castInfo.nMatID = bMaterial ? nGetMatID : curTreasureInfo.nMatID;
            castInfo.szMatIcon = szGetMatIcon;
            castInfo.emPayType = curTreasureInfo.emPayType;
            castInfo.nPayNum = nBuyPrice;
            castInfo.nPayID = nPayMatID;
            /////�жϻ�ȡ����Ʒ�Ƿ�Ϊϡ������
            //UISpecialCast.ShowMatAuctionInfo(castInfo);

            pEffBeizier.SetRenderTexture(texture);
            pEffBeizier.SetTarget(vShowPostion + Vector3.up * 1.5F, tranTargetSlot, delegate ()
            {
                AddReward(uid);
                UIManager.Instance.OpenUI(UIResType.MatAuctionResult);
                UIMatAuctionResult auctionResult = UIManager.Instance.GetUI(UIResType.MatAuctionResult) as UIMatAuctionResult;
                if (auctionResult != null)
                {
                    auctionResult.SetInfo(castInfo);
                }
            });
        });

        //CResLoadMgr.Inst.SynLoad(szShowEff, CResLoadMgr.EM_ResLoadType.CanbeUnloadAssetbundle,
        //delegate (Object res, object data, bool bSuc)
        //{
        //    GameObject objBoxRoot = res as GameObject;
        //    if (objBoxRoot == null) return;
        //    GameObject objNewBox = GameObject.Instantiate(objBoxRoot) as GameObject;
        //    Transform tranNewBox = objNewBox.GetComponent<Transform>();
        //    Transform tranTargetSlot = null;
        //    CPlayerUnit playerUnit = CPlayerMgr.Ins.GetIdleUnit(pInfo.uid);
        //    if (playerUnit == null)
        //    {
        //        tranTargetSlot = CGameColorFishMgr.Ins.pMap.GetRandomGachaPos();
        //    }
        //    else
        //    {
        //        tranTargetSlot = playerUnit.tranGachePos;
        //    }

        //    if (tranTargetSlot == null)
        //    {
        //        Debug.LogError("�쳣�ı����");
        //        return;
        //    }
        //    tranNewBox.position = CNPCMgr.Ins.pAuctionUnit.tranGachePos.position;
        //    tranNewBox.SetParent(tranTargetSlot);

        //    tranNewBox.localScale = Vector3.one;
        //    tranNewBox.localRotation = Quaternion.identity;

        //    CAuctionBox pBox = objNewBox.GetComponent<CAuctionBox>();
        //    if (pBox == null)
        //    {
        //        Destroy(objNewBox);
        //        return;
        //    }

        //    ///���ӻ�ȡ����Դ
        //    if (playerUnit != null)
        //    {
        //        ///���
        //        playerUnit.AddCoinByHttp(auctionResultInfo.nlFishCoin, EMFishCoinAddFunc.Auction, false);
        //        ///��ɫ/��
        //        if (auctionResultInfo.dropInfo != null)
        //        {

        //            int nValue = auctionResultInfo.dropInfo.nValue;
        //            if (auctionResultInfo.dropInfo.nType == 0)
        //            {
        //                //�ӽ�ɫ
        //                CHttpParam pReqParams = new CHttpParam
        //                  (
        //                     new CHttpParamSlot("uid", playerUnit.uid.ToString()),
        //                     new CHttpParamSlot("avatarId", nValue.ToString())
        //                  );

        //                CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugSendAvatar, new HHandlerRefreshRolePack(), pReqParams);
        //            }
        //            else if (auctionResultInfo.dropInfo.nType == 1)
        //            {
        //                //�Ӵ�
        //                CPlayerNetHelper.SendUserBoat(playerUnit.uid, nValue, new HHandlerRefreshBoatPack());
        //            }
        //            else if (auctionResultInfo.dropInfo.nType == 2)
        //            {
        //                //�����
        //                CPlayerNetHelper.AddFishItemPack(pInfo.uid, nValue, nValue, nValue, 0);
        //            }
        //        }
        //    }

        //    pBox.tranRoot = tranTargetSlot;
        //    pBox.Init(listAuctionInfos, pInfo.uid, bCri);
        //    pBox.dlgOver = ShowResultUI;
        //});
    }

    void ShowResultUI()
    {
        UIManager.Instance.OpenUI(UIResType.AuctionResult);
        UIAuctionResult auctionResult = UIManager.Instance.GetUI(UIResType.AuctionResult) as UIAuctionResult;
        if (auctionResult != null)
        {
            //auctionResult.SetInfo(curTreasureInfo, auctionResultInfo);
        }
    }

    /// <summary>
    /// ��������
    /// </summary>
    public void StopAuction()
    {
        UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
        UIMatAuction auction = UIManager.Instance.GetUI(UIResType.MatAuction) as UIMatAuction;
        if (roomInfo == null ||
            auction == null)
            return;
        roomInfo.ShowSpecialty();
        auction.Hide();
        if (pInfo != null)
        {
            ShowBox();
        }
        bAuction = false;
        emAuctionState = EMAuctionState.Normal;
        pInfo = null;
    }


}