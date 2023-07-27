using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;



public class CGameColorFishMgr : CSingleCompBase<CGameColorFishMgr>
{
    public enum EMJoinType
    {
        Normal,
        Gift,
    }

    public enum EMGameType
    {
        Normal,     //休闲模式
        Battle,     //竞赛模式
        Boss,       //Boss战

        Cinema,     //影院模式
        Survive,    //生存模式
        TimeBattle, //时间竞速
    }

    public EMGameType emCurGameType = EMGameType.Normal;
    public int nMaxPlayerNum;
    public float fAvatarScale;

    [Header("游戏配置")]
    public CConfigColorFish pStaticConfig;
    [Header("游戏地图")]
    public CGameMap pMap;
    [Header("广播配置")]
    public CConfigFishBroad pAudioBroad;

    public CGachaMgr pGachaMgr; //氪金管理器

    public CRandomEventHandler pRandomEventMgr = new CRandomEventHandler(); //随机事件管理器

    /// <summary>
    /// 全场概率提升等级
    /// </summary>
    public int nCurRateUpLv;

    //当前场次游戏收获的总电池数量
    [ReadOnly]
    long lCurGameBattery;
    public DelegateLFuncCall dlgChgCurGameBattery;
    public long CurGameBattery
    {
        get
        {
            return lCurGameBattery;
        }
        set
        {
            lCurGameBattery = value;

            dlgChgCurGameBattery?.Invoke(lCurGameBattery);
        }
    }

    /// <summary>
    /// 当前欧皇UID
    /// </summary>
    public string nlCurOuHuangUID;

    //当前地图经验统计
    long lCurMapExp;
    public DelegateLFuncCall dlgChgMapExp;
    public long CurMapExp
    {
        get
        {
            return lCurMapExp;
        }
        set
        {
            lCurMapExp = value;
            dlgChgMapExp?.Invoke(lCurMapExp);
        }
    }

    //房间设置
    public CGameColorFishNormalRoomConfig pGameRoomConfig;
    public CGameColorFishVSRoomConfig pGameRoomVSConfig;

    //地图设置
    public ST_GameMap pMapConfig;
    public ST_GameVSMap pMapVSConfig;

    public int nCurVtuberCount;     //主播已经出现的数量

    public bool bShowPlayerInfo;    //是否显示玩家信息

    public int nBossDropAdd = 0;    //Boss掉落加成

	public int nSpecialBossRate = 0;        //特殊Boss出现概率
    public int nSpecialBossDropAdd = 0;     //特殊Boss掉落概率

   

    public int nTimeHeartBeat;      //心跳包间隔时间
    public CGameHeartBeatInfo pHeartInfo = null;
    [HideInInspector]
    public CPropertyTimer pTickerHeart = null;    
    
    bool bInited = false;

    public void Init()
    {
        fAvatarScale = 1F;
        bShowPlayerInfo = true;

       

        if (pMap != null)
        {
            pMap.Init();
            fAvatarScale = pMap.fAvatarScale;
        }

        nCurVtuberCount = 0;
        lCurGameBattery = 0;
        if (bInited)
        {
            return;
        }
        //初始化配置
        //bool bSaveFile = false;
        if (!LoadRoomConfig(CAppPathMgr.LOCALSAVEDATA_DIR + CAppPathMgr.SaveFile_RoomConfig))
        {
            pGameRoomConfig = new CGameColorFishNormalRoomConfig();
            SaveRoomConfig(EMGameType.Normal);
        }

        if(!LoadVSRoomConfig(CAppPathMgr.LOCALSAVEDATA_DIR + CAppPathMgr.SaveFile_RoomVSConfig))
        {
            pGameRoomVSConfig = new CGameColorFishVSRoomConfig();
            SaveRoomConfig(EMGameType.Battle);
        }

        bInited = true;
    }

    public bool CheckMaxVtuberCount()
    {
        bool bMax = false;

        if (pGameRoomConfig.nMaxVtuberCount > 0 &&
            nCurVtuberCount >= pGameRoomConfig.nMaxVtuberCount)
        {
            bMax = true;
        }

        return bMax;
    }

    public void AddVtuberCount()
    {
        if (pGameRoomConfig.nMaxVtuberCount <= 0) return;
        nCurVtuberCount++;
    }

    public void JoinPlayer(CPlayerBaseInfo pPlayerInfo, EMJoinType emJoinType)
    {
        if (CPlayerMgr.Ins.GetIdleUnit(pPlayerInfo.uid) != null)
        {
            Debug.Log("不能重复加入");
            return;
        }

        //Debug.Log(pPlayerInfo.emUserType + "=== UserType");
        CMapSlot tranIdleSlot = pMap.GetRandIdleRoot();
        if (tranIdleSlot == null) return;

        if(pPlayerInfo.avatarId == 101)
        {
            pPlayerInfo.RefreshRoleAvatar();
        }

        CResLoadMgr.Inst.SynLoad("Unit/PlayerUnit", CResLoadMgr.EM_ResLoadType.Role,
         delegate (Object res, object data, bool bSuc)
         {
             GameObject objRoleRoot = res as GameObject;
             if (objRoleRoot == null) return;
             GameObject objNewRole = GameObject.Instantiate(objRoleRoot) as GameObject;
             Transform tranNewRole = objNewRole.GetComponent<Transform>();
             tranNewRole.SetParent(tranIdleSlot.tranSelf);
             tranNewRole.localScale = Vector3.one;
             tranNewRole.localPosition = Vector3.zero;
             tranNewRole.localRotation = Quaternion.identity;
             
             CPlayerUnit pNewUnit = objNewRole.GetComponent<CPlayerUnit>(); 
             pNewUnit.Init(pPlayerInfo);
             pNewUnit.SetMapSlot(tranIdleSlot);
             
             //设置玩家的船
             tranIdleSlot.SetBoat(pNewUnit.pInfo.nBoatAvatarId, pNewUnit, pNewUnit.pInfo.guardLevel, true);

             CControlerSlotByBoss cControlerSlot = tranIdleSlot.GetComponent<CControlerSlotByBoss>();
             if (CControlerSlotMgr.Ins != null &&
                 cControlerSlot != null)
             {
                 CControlerSlotMgr.Ins.AddSlot(cControlerSlot, pPlayerInfo.uid);
             }

             pNewUnit.SetState(CPlayerUnit.EMState.Born);

             //加入管理器
             CPlayerMgr.Ins.AddIdleUnit(pNewUnit);

             //加入单位的UI信息
             UIGameInfo uiShow = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
             if (uiShow != null)
             {
                 uiShow.AddUnit(pNewUnit);
             }

             //if (emJoinType == EMJoinType.Normal)
             //{
             //    pNewUnit.CheckYZM();
             //}

             UIRoomBossInfo roomBossInfo = UIManager.Instance.GetUI(UIResType.RoomBossInfo) as UIRoomBossInfo;
             if (roomBossInfo != null)
             {
                 roomBossInfo.RefreshDropRate();
             }

             //自动签到
             CHttpParam pParamSign = new CHttpParam
             (
                 new CHttpParamSlot("uid", pPlayerInfo.uid.ToString())
             );
             CHttpMgr.Instance.SendHttpMsg(CHttpConst.ViewerSignIn, pParamSign);

             ////舰长签到
             ////白名单限制：暂时只有炫彩哥房间有
             //if (pPlayerInfo.guardLevel > 0)
             //{
             //    CHttpParam pParamVipSign = new CHttpParam(
             //       new CHttpParamSlot("uid", pPlayerInfo.uid.ToString()),
             //       new CHttpParamSlot("roomId", CDanmuSDKCenter.Ins.szRoomId.ToString()),
             //       new CHttpParamSlot("guardianLv", pPlayerInfo.guardLevel.ToString())
             //    );
             //    CHttpMgr.Instance.SendHttpMsg(CHttpConst.ViewerVipSignIn, pParamVipSign, 0, true);
             //}

             if (CGamePlayerBornMgr.Ins!=null)
             {
                 CGamePlayerBornMgr.Ins.CreateBornEff(pPlayerInfo);
             }
         });
    }

    /// <summary>
    /// 加载房间配置文件
    /// </summary>
    /// <returns></returns>
    bool LoadRoomConfig(string strPath)
    {
        if (!File.Exists(strPath))
        {
            Debug.LogError("文件不存在：" + strPath);
            return false;
        }

        try
        {
            string strText = LocalFileManage.Ins.LoadFileInfo(strPath);
            CLocalNetMsg pData = new CLocalNetMsg(strText);

            pGameRoomConfig = new CGameColorFishNormalRoomConfig();
            pGameRoomConfig.LoadByMsg(pData);

            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("读取房间配置异常：" + e.Message);
            return false;
        }
    }

    bool LoadVSRoomConfig(string strPath)
    {
        if (!File.Exists(strPath))
        {
            Debug.LogError("文件不存在：" + strPath);
            return false;
        }

        try
        {
            string strText = LocalFileManage.Ins.LoadFileInfo(strPath);
            CLocalNetMsg pData = new CLocalNetMsg(strText);

            pGameRoomVSConfig = new CGameColorFishVSRoomConfig();
            pGameRoomVSConfig.LoadByMsg(pData);

            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("读取VS房间配置异常：" + e.Message);
            return false;
        }
    }

    public void SaveRoomConfig(EMGameType gameType)
    {
        if(gameType == EMGameType.Normal ||
           gameType == EMGameType.Boss)
        {
            CLocalNetMsg pData = pGameRoomConfig.ToMsg();
            LocalFileManage.Ins.SaveFileAsyc(CAppPathMgr.SaveFile_RoomConfig, pData.GetData(), CAppPathMgr.LOCALSAVEDATA_DIR);
        }
        else if(gameType == EMGameType.Battle)
        {
            CLocalNetMsg pData = pGameRoomVSConfig.ToMsg();
            LocalFileManage.Ins.SaveFileAsyc(CAppPathMgr.SaveFile_RoomVSConfig, pData.GetData(), CAppPathMgr.LOCALSAVEDATA_DIR);
        }
    }

    ///// <summary>
    ///// 加载房间配置文件
    ///// </summary>
    ///// <returns></returns>
    //bool LoadRoomConfig(string strPath)
    //{
    //    if (!File.Exists(strPath))
    //    {
    //        Debug.LogError("文件不存在：" + strPath);
    //        return false;
    //    }

    //    try
    //    {
    //        string strText = LocalFileManage.Ins.LoadFileInfo(strPath);
    //        CLocalNetMsg pData = new CLocalNetMsg(strText);

    //        pGameConfig = new CGameColorFishRoomConfig();

    //        return true;
    //    }
    //    catch (System.Exception e)
    //    {
    //        Debug.LogError("读取房间配置异常：" + e.Message);
    //        return false;
    //    }
    //}

    /// <summary>
    /// 执行随机事件
    /// </summary>
    public ST_RandomEvent RandomEvent()
    {
        ST_RandomEvent pEvent = null;
        List<ST_RandomEvent> listEvents = CTBLHandlerRandomEvent.Ins.GetInfos();
        int nTotalWeights = 0;
        for (int i = 0; i < listEvents.Count; i++)
        {
            nTotalWeights += listEvents[i].nRate;
        }

        int nCurRand = Random.Range(1, nTotalWeights + 1);
        int nAddRate = 0;
        int nRes = -1;
        for (int i = 0; i < listEvents.Count; i++)
        {
            if (nCurRand <= listEvents[i].nRate + nAddRate)
            {
                nRes = i;
                break;
            }
            else
            {
                nAddRate += listEvents[i].nRate;
            }
        }

        if (nRes >= 0)
        {
            pEvent = listEvents[nRes];
            return pEvent;
        }

        return null;
    }

    /// <summary>
    /// 执行随机事件
    /// </summary>
    /// <param name="pEvent"></param>
    /// <param name="player"></param>
    public void DoRandomEvent(int nRandomID, CPlayerBaseInfo player)
    {
        //获取Action句柄
        CRandomEventAction pAction = null;
        if (pRandomEventMgr.dicRandomEventAct.TryGetValue(nRandomID, out pAction))
        {
            pAction.DoAction(player);
        }
    }

    public void Reset()
    {
        //emState = EMState.Idle;
        //lCurGameBattery = 0;
        CurMapExp = 0;
        //szCurGameID = "";
        //pRoundInfo = null;

        //if (pMap != null)
        //{
        //    pMap.RefreshMap();
        //}
    }

    void Update()
    {
        UpdateHeartBeat();
    }

    void UpdateHeartBeat()
    {
        if (pHeartInfo == null) return;
        if (pTickerHeart == null) return;

        if(pTickerHeart.Tick(CTimeMgr.DeltaTime))
        {
            SendHeartBeat(new HHandlerCommonHeartBeat());
            pTickerHeart.FillTime();
        }
    }

    public void SendHeartBeat(INetEventHandler handler = null)
    {
        //if (CDanmuSDKCenter.Ins.emPlatform != CDanmuSDKCenter.EMPlatform.Bilibili) return;

        //发送心跳包请求
        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("uid", CDanmuSDKCenter.Ins.szRoomId),
            new CHttpParamSlot("roomId", CDanmuSDKCenter.Ins.szRoomId)
        );

        if(handler == null)
        {
            CHttpMgr.Instance.SendHttpMsg(CHttpConst.HeartBeat, pReqParams);
        }
        else
        {
            CHttpMgr.Instance.SendHttpMsg(CHttpConst.HeartBeat, handler, pReqParams);
        }
    }

    public long GetNowServerTime()
    {
        if(pHeartInfo == null)
        {
            return CTimeMgr.NowMillonsSec();
        }

        return (CTimeMgr.NowMillonsSec() - pHeartInfo.nRecordTimeStamp) + pHeartInfo.nServerTimeStamp;
    }
}
