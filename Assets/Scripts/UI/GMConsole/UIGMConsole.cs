using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGMConsole : MonoBehaviour
{
    public InputField uiInputUrl;
    public InputField uiInputPort;
    public InputField uiInputUID;
    public InputField uiInputRoom;
    public Toggle uiToggleIsVtb;
    public InputField uiInputAddCoin;
    public InputField uiInputAddExp;
    public InputField uiInputAddSuipian;
    public InputField uiInputSetDailySignAdd;
    public InputField uiInputAvatarId;
    public InputField uiInputFishBoatId;
    public InputField uiInputBaitCount;
    public InputField uiInputFishGanCount;
    public InputField uiInputFishPiaoCount;
    public InputField uiInputFishLunCount;
    public InputField uiInputAddMatID;
    public InputField uiInputAddMatNum;
    public InputField uiInputFesID;
    public InputField uiInputFesPoint;
    public InputField uiInputGachaGift;
    public InputField uiInputTreasurePoint;
    public InputField uiInputExp;
    public InputField uiInputRareFish;
    public InputField uiInputFishGanId;

    public GameObject objBoardAvatarInfo;
    public GameObject objBoardGachaPool;
    public GameObject objBoardMapExp;
    public GameObject objBoardFishBoat;
    public GameObject objBoardFishGan;
    public GameObject objBoardFesInfo;
    public GameObject objBoardMatInfo;
    public GameObject objBoardTreasureInfo;
    public GameObject objBoardFishGachaBoxInfo;
    public GameObject objBoardUserLvConfig;
    public GameObject objBoardVersionConfig;

    public Text uiLabelContent;

    // Start is called before the first frame update
    void Start()
    {
        AssetBundle.SetAssetBundleDecryptKey(CEncryptHelper.ASSETKEY);

        CNetConfigMgr.Ins.Init();
        CHttpMgr.Instance.Init();
        CTBLInfo.Inst.Init();
        uiInputUrl.text = CNetConfigMgr.Ins.GetHttpServerIP();
        uiInputPort.text = CNetConfigMgr.Ins.GetHttpServerPort().ToString();

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetVersion, new HGMHandlerGetVersion());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshUrl()
    {
        CHttpMgr.Instance.szUrl = uiInputUrl.text;
        CHttpMgr.Instance.nPort = int.Parse(uiInputPort.text);
    }

    public void OnClickAddCoin()
    {
        //if (uiToggleIsVtb.isOn) return;

        RefreshUrl();

        CHttpParam pReqParams = new CHttpParam
        (
            new CHttpParamSlot("uid", uiInputUID.text),
            new CHttpParamSlot("add", uiInputAddCoin.text),
            new CHttpParamSlot("time", CGameColorFishMgr.Ins.GetNowServerTime().ToString())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugAddFishCoin, new HGMHandlerAddCoin(), pReqParams, 0, true);
    }

    public void OnClickAddRoomGain()
    {
        RefreshUrl();

        CHttpParam pReqAddCoinParams = new CHttpParam(
            new CHttpParamSlot("uid", uiInputUID.text),
            new CHttpParamSlot("count", uiInputAddCoin.text)
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddRoomFishGainCoin, new HGMHandlerAddCoin(), pReqAddCoinParams);
    }

    public void OnClickAddExp()
    {
        //CHttpParam pAddSceneExp = new CHttpParam
        //(
        //    new CHttpParamSlot("uid", uiInputUID.text),
        //    new CHttpParamSlot("roomId", uiInputRoom.text),
        //    new CHttpParamSlot("exp", uiInputAddExp.text)
        //    //new CHttpParamSlot("isPay", "1")
        //);

        //CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddSceneExp, new HGMHandlerAddCoin(), pAddSceneExp, 0, true);

        CPlayerNetHelper.AddSceneExp(uiInputUID.text, uiInputRoom.text, long.Parse(uiInputAddExp.text), false, new HGMHandlerAddCoin());
    }

    public void OnClickAddSuipian()
    {
        RefreshUrl();

        CHttpParam pReqParams = new CHttpParam
        (
            new CHttpParamSlot("uid", uiInputUID.text),
            new CHttpParamSlot("roomId", uiInputRoom.text),
            new CHttpParamSlot("isVtb", uiToggleIsVtb.isOn?"1":"0"),
            new CHttpParamSlot("avatarFragments", uiInputAddSuipian.text),
            new CHttpParamSlot("time", CTimeMgr.NowMillonsSec().ToString())
        );
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugViewerAddAvatarSuipian, new HGMHandlerAddAvatarSuipian(), pReqParams, 0, true);
    }

    public void OnClickSetDailySign()
    {
        RefreshUrl();

        CHttpParam pReqParams = new CHttpParam
        (
           new CHttpParamSlot("addFishItem", uiInputSetDailySignAdd.text)
        );
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugSetDailySignAdd, new HGMHandlerAddCoin(), pReqParams);
    }

    public void OnClickSendAvatar()
    {
        RefreshUrl();

        string szAvatarId = uiInputAvatarId.text;
        CHttpParam pReqParams = new CHttpParam
        (
           new CHttpParamSlot("uid", uiInputUID.text),
           new CHttpParamSlot("avatarId", szAvatarId)
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugSendAvatar, new HGMHandlerSendAvatar(), pReqParams);
    }

    public void OnClickDelUserAvatar()
    {
        RefreshUrl();

        string szAvatarId = uiInputAvatarId.text;
        CHttpParam pReqParams = new CHttpParam
        (
           new CHttpParamSlot("uid", uiInputUID.text),
           new CHttpParamSlot("avatarId", szAvatarId)
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugDelAvatar, new HGMHandlerSendAvatar(), pReqParams);
    }

    public void OnClickSendFishBoat()
    {
        RefreshUrl();

        CPlayerNetHelper.SendUserBoat(uiInputUID.text, int.Parse(uiInputFishBoatId.text), new HGMHandlerSendAvatar());
    }

    public void OnClickRemoveFishBoat()
    {
        RefreshUrl();

        CHttpParam pReqParams = new CHttpParam
       (
          new CHttpParamSlot("uid", uiInputUID.text),
          new CHttpParamSlot("boatId", uiInputFishBoatId.text)
       );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugRemoveFishBoat, new HGMHandlerSendAvatar(), pReqParams);
    }

    public void OnClickAddMaterials()
    {
        RefreshUrl();

        //CHttpParam pReqParams = new CHttpParam
        //(
        //    new CHttpParamSlot("uid", uiInputUID.text),
        //    new CHttpParamSlot("itemId", uiInputAddMatID.text),
        //    new CHttpParamSlot("count", uiInputAddMatNum.text),
        //    new CHttpParamSlot("dailyGet", "0")
        //);

        //CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddUserFishMat, new HGMHandlerSendAvatar(), pReqParams, 0, true);

        CPlayerNetHelper.AddFishMat(uiInputUID.text, 
                                    int.Parse(uiInputAddMatID.text), 
                                    long.Parse(uiInputAddMatNum.text), 0, 
                                    new HGMHandlerSendAvatar());
    }

    #region 送礼物

    public void OnClickSendBait()
    {
        RefreshUrl();
        long curTimeStamp = CGameColorFishMgr.Ins.GetNowServerTime();
        string szBaitCount = uiInputBaitCount.text;
        CHttpParam pReqParams = new CHttpParam
        (
            new CHttpParamSlot("uid", uiInputUID.text),
            new CHttpParamSlot("fishItem", szBaitCount.ToString()),
            new CHttpParamSlot("time", curTimeStamp.ToString()),
            new CHttpParamSlot("nonce", CEncryptHelper.AesEncrypt(uiInputUID.text + uiInputRoom.text + curTimeStamp))
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddBait, new HGMHandlerSendBait(), pReqParams, 0, true);
    }

    public void OnClickSendYugan()
    {
        long curTimeStamp = CGameColorFishMgr.Ins.GetNowServerTime();
        CHttpParam pReqParams = new CHttpParam
        (
           new CHttpParamSlot("uid", uiInputUID.text),
           new CHttpParamSlot("itemType", "fishGan"),
           new CHttpParamSlot("count", uiInputFishGanCount.text),
           new CHttpParamSlot("time", curTimeStamp.ToString()),
           new CHttpParamSlot("nonce", CEncryptHelper.AesEncrypt(uiInputUID.text + uiInputRoom.text + curTimeStamp))
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddGiftCount, new HGMHandlerSendBait(), pReqParams, 0, true);
    }
    public void OnClickSendFupiao()
    {
        long curTimeStamp = CGameColorFishMgr.Ins.GetNowServerTime();
        CHttpParam pReqParams = new CHttpParam
        (
           new CHttpParamSlot("uid", uiInputUID.text),
           new CHttpParamSlot("itemType", "fishPiao"),
           new CHttpParamSlot("count", uiInputFishPiaoCount.text),
           new CHttpParamSlot("time", curTimeStamp.ToString()),
           new CHttpParamSlot("nonce", CEncryptHelper.AesEncrypt(uiInputUID.text + uiInputRoom.text + curTimeStamp))
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddGiftCount, new HGMHandlerSendBait(), pReqParams, 0, true);
    }

    public void OnClickSendFeilun()
    {
        long curTimeStamp = CGameColorFishMgr.Ins.GetNowServerTime();
        CHttpParam pReqParams = new CHttpParam
        (
           new CHttpParamSlot("uid", uiInputUID.text),
           new CHttpParamSlot("itemType", "fishLun"),
           new CHttpParamSlot("count", uiInputFishLunCount.text),
           new CHttpParamSlot("time", curTimeStamp.ToString()),
           new CHttpParamSlot("nonce", CEncryptHelper.AesEncrypt(uiInputUID.text + uiInputRoom.text + curTimeStamp))
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddGiftCount, new HGMHandlerSendBait(), pReqParams, 0, true);
    }


    #endregion

    public void OnClickClearTotalBattery()
    {
        RefreshUrl();

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugClearTotalBattery);
    }

    public void OnClickClearCallPoint()
    {
        RefreshUrl();

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugClearCallCoin);
    }

    public void OnClickClearFishVtbRanklist()
    {
        RefreshUrl();

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugClearFishVtbRanklist);
    }

    public void OnClickEditAvatarInfo()
    {
        RefreshUrl();

        objBoardAvatarInfo.SetActive(true);

        //请求数据
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugGetAvatarInfoList);
    }

    public void OnClickEditGachaPool()
    {
        RefreshUrl();

        objBoardGachaPool.SetActive(true);

        UIGMGachaPool uiGachaPool = GameObject.FindObjectOfType<UIGMGachaPool>();
        if (uiGachaPool == null) return;

        uiGachaPool.Init(1, 200, 2500, 1, 1, new List<CGMGachaInfo>());
    }

    public void OnClickEditMapExp()
    {
        RefreshUrl();

        objBoardMapExp.SetActive(true);

        //请求数据
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugGetMapExpInfo);
    }

    public void OnClickEditFishBoatInfo()
    {
        RefreshUrl();

        objBoardFishBoat.SetActive(true);

        //请求数据
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugGetFishBoatInfoList);
    }

    public void OnClickEditFishGanInfo()
    {
        RefreshUrl();

        objBoardFishGan.SetActive(true);

        //请求数据
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugGetFishGanInfoList);
    }

    public void OnClickEditFesInfo()
    {
        RefreshUrl();

        objBoardFesInfo.SetActive(true);

        UIGMFesInfoConfig uiFesInfoConfigBoard = objBoardFesInfo.GetComponent<UIGMFesInfoConfig>();
        uiFesInfoConfigBoard.Init();
    }

    public void OnClickEditMatInfo()
    {
        RefreshUrl();

        objBoardMatInfo.SetActive(true);

        //请求数据
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugLoadMatInfo, new HGMHandlerGetFishMatInfoArr());
    }

    public void OnClickCombineFishCoin()
    {
        RefreshUrl();

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugCombineFishCoin);
    }

    public void OnClickCombineFishBait()
    {
        RefreshUrl();

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugCombineFishBait);
    }

    public void OnClickSetVip(int vip)
    {
        RefreshUrl();

        CHttpParam pReqParams = new CHttpParam
        (
            new CHttpParamSlot("uid", uiInputUID.text),
            new CHttpParamSlot("vip", vip.ToString())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugSetVip, new HGMHandlerSendAvatar(), pReqParams);
    }

    public void OnClickChaxunBoat()
    {
        RefreshUrl();

        //继续获取角色背包
        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("uid", uiInputUID.text),
            new CHttpParamSlot("roomId", uiInputRoom.text),
            new CHttpParamSlot("isVtb", "0")
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetBoatList, new HGMHandlerSendAvatar(), pReqParams);
    }

    public void OnClickChaxunRole()
    {
        RefreshUrl();

        //继续获取角色背包
        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("uid", uiInputUID.text),
            new CHttpParamSlot("roomId", uiInputRoom.text),
            new CHttpParamSlot("isVtb", "0")
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetAvatarList, new HGMHandlerSendAvatar(), pReqParams);
    }

    public void OnClickChaxunFishGan()
    {
        RefreshUrl();

        CHttpParam pReqParamsOnlyUid = new CHttpParam(
           new CHttpParamSlot("uid", uiInputUID.text)
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetFishGanList, new HGMHandlerSendAvatar(), pReqParamsOnlyUid);
    }

    public void OnClickAddFesPoint()
    {
        RefreshUrl();

        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("uid", uiInputUID.text),
            new CHttpParamSlot("packId", uiInputFesID.text),
            new CHttpParamSlot("point", uiInputFesPoint.text),
            new CHttpParamSlot("isVtb", "0"),
            new CHttpParamSlot("time", CTimeMgr.NowMillonsSec().ToString())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddFesPoint, new HGMHandlerSendAvatar(), pReqParams, 10, true);
    }

    public void OnClickTreasureShopConfig()
    {
        RefreshUrl();

        objBoardTreasureInfo.SetActive(true);

        //请求数据
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugLoadTreasureShopList, new HGMHandlerGetTreasureShopList());
    }

    public void OnClickGiftGachaBoxConfig()
    {
        RefreshUrl();

        objBoardFishGachaBoxInfo.SetActive(true);

        UIGMGiftGachaBoxConfig uiGachaPool = objBoardFishGachaBoxInfo.GetComponent<UIGMGiftGachaBoxConfig>();
        if (uiGachaPool == null) return;

        uiGachaPool.Init(1, 200, new List<CGMGiftGachaBoxConfigInfo>());
    }

    public void OnClickGachaGift()
    {
        RefreshUrl();

        CHttpParam pReqParams = new CHttpParam(
         new CHttpParamSlot("uid", uiInputUID.text),
         new CHttpParamSlot("modelId", "1"),
         new CHttpParamSlot("gachaCount", uiInputGachaGift.text)
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.BuyGiftGachaBox, new HGMHandlerGachaGiftBox(), pReqParams, 0, true);
    }

    public void OnClickAddTreasurePoint()
    {
        RefreshUrl();

        //CHttpParam pReqParams = new CHttpParam
        //(
        //    new CHttpParamSlot("uid", uiInputUID.text),
        //    new CHttpParamSlot("treasurePoint", uiInputTreasurePoint.text)
        //);

        //CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddUserTreasurePoint, new HGMHandlerSendAvatar(), pReqParams, 10, true);
        CPlayerNetHelper.AddTreasureCoin(uiInputUID.text, long.Parse(uiInputTreasurePoint.text), new HGMHandlerSendAvatar());
    }

    public void OnClickEditUserExpConfig()
    {
        RefreshUrl();

        objBoardUserLvConfig.SetActive(true);

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugGetFishUserExpConfig, new HGMHandlerGetUserExpConfigList());
    }

    public void OnClickAddUserExp()
    {
        RefreshUrl();

        CHttpParam pReqParams = new CHttpParam
        (
           new CHttpParamSlot("uid", uiInputUID.text),
           new CHttpParamSlot("exp", uiInputExp.text)
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddUserExp, new HGMHandlerSendAvatar(), pReqParams, 10, true);
    }

    public void OnClickResetMapExp()
    {
        RefreshUrl();

        CHttpParam pReqParams = new CHttpParam
        (
          new CHttpParamSlot("uid", uiInputUID.text),
          new CHttpParamSlot("roomId", uiInputRoom.text),
          new CHttpParamSlot("level", "1"),
          new CHttpParamSlot("exp", "0")
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugSetUserMapLv, new HGMHandlerSendAvatar(), pReqParams);
    }

    public void OnClickAddRareFish()
    {
        RefreshUrl();

        CHttpParam pReqRoomParams = new CHttpParam(
            new CHttpParamSlot("uid", uiInputUID.text),
            new CHttpParamSlot("roomId", uiInputRoom.text),
            new CHttpParamSlot("fishId", uiInputRareFish.text),
            new CHttpParamSlot("count", 1.ToString())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddRoomRareFishRecord, new HGMHandlerSendAvatar(), pReqRoomParams, 0, true);
    }

    public void OnClickBanUser(int code)
    {
        RefreshUrl();

        CHttpParam pReqRoomParams = new CHttpParam(
          new CHttpParamSlot("uid", uiInputUID.text),
          new CHttpParamSlot("banLv", code.ToString())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugBanUser, new HGMHandlerSendAvatar(), pReqRoomParams);
    }

    public void OnClickVersionConfig()
    {
        objBoardVersionConfig.SetActive(true);
    }

    public void OnClickSendFishGan()
    {
        RefreshUrl();

        CPlayerNetHelper.SendUserFisnGan(uiInputUID.text, 
            int.Parse(uiInputFishGanId.text), 
            new HGMHandlerSendAvatar());
    }

    public void OnClickRemoveFishGan()
    {
        RefreshUrl();

        CHttpParam pReqParams = new CHttpParam
      (
         new CHttpParamSlot("uid", uiInputUID.text),
         new CHttpParamSlot("ganId", uiInputFishGanId.text)
      );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugRemoveFishGan, new HGMHandlerSendAvatar(), pReqParams);
    }

    public void OnClickGetVersion()
    {
        RefreshUrl();

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetVersion, new HGMHandlerGetVersion());
    }
}
