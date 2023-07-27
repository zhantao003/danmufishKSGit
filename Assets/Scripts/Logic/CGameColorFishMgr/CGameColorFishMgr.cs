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
        Normal,     //����ģʽ
        Battle,     //����ģʽ
        Boss,       //Bossս

        Cinema,     //ӰԺģʽ
        Survive,    //����ģʽ
        TimeBattle, //ʱ�侺��
    }

    public EMGameType emCurGameType = EMGameType.Normal;
    public int nMaxPlayerNum;
    public float fAvatarScale;

    [Header("��Ϸ����")]
    public CConfigColorFish pStaticConfig;
    [Header("��Ϸ��ͼ")]
    public CGameMap pMap;
    [Header("�㲥����")]
    public CConfigFishBroad pAudioBroad;

    public CGachaMgr pGachaMgr; //봽������

    public CRandomEventHandler pRandomEventMgr = new CRandomEventHandler(); //����¼�������

    /// <summary>
    /// ȫ�����������ȼ�
    /// </summary>
    public int nCurRateUpLv;

    //��ǰ������Ϸ�ջ���ܵ������
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
    /// ��ǰŷ��UID
    /// </summary>
    public string nlCurOuHuangUID;

    //��ǰ��ͼ����ͳ��
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

    //��������
    public CGameColorFishNormalRoomConfig pGameRoomConfig;
    public CGameColorFishVSRoomConfig pGameRoomVSConfig;

    //��ͼ����
    public ST_GameMap pMapConfig;
    public ST_GameVSMap pMapVSConfig;

    public int nCurVtuberCount;     //�����Ѿ����ֵ�����

    public bool bShowPlayerInfo;    //�Ƿ���ʾ�����Ϣ

    public int nBossDropAdd = 0;    //Boss����ӳ�

	public int nSpecialBossRate = 0;        //����Boss���ָ���
    public int nSpecialBossDropAdd = 0;     //����Boss�������

   

    public int nTimeHeartBeat;      //���������ʱ��
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
        //��ʼ������
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
            Debug.Log("�����ظ�����");
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
             
             //������ҵĴ�
             tranIdleSlot.SetBoat(pNewUnit.pInfo.nBoatAvatarId, pNewUnit, pNewUnit.pInfo.guardLevel, true);

             CControlerSlotByBoss cControlerSlot = tranIdleSlot.GetComponent<CControlerSlotByBoss>();
             if (CControlerSlotMgr.Ins != null &&
                 cControlerSlot != null)
             {
                 CControlerSlotMgr.Ins.AddSlot(cControlerSlot, pPlayerInfo.uid);
             }

             pNewUnit.SetState(CPlayerUnit.EMState.Born);

             //���������
             CPlayerMgr.Ins.AddIdleUnit(pNewUnit);

             //���뵥λ��UI��Ϣ
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

             //�Զ�ǩ��
             CHttpParam pParamSign = new CHttpParam
             (
                 new CHttpParamSlot("uid", pPlayerInfo.uid.ToString())
             );
             CHttpMgr.Instance.SendHttpMsg(CHttpConst.ViewerSignIn, pParamSign);

             ////����ǩ��
             ////���������ƣ���ʱֻ���Ųʸ緿����
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
    /// ���ط��������ļ�
    /// </summary>
    /// <returns></returns>
    bool LoadRoomConfig(string strPath)
    {
        if (!File.Exists(strPath))
        {
            Debug.LogError("�ļ������ڣ�" + strPath);
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
            Debug.LogError("��ȡ���������쳣��" + e.Message);
            return false;
        }
    }

    bool LoadVSRoomConfig(string strPath)
    {
        if (!File.Exists(strPath))
        {
            Debug.LogError("�ļ������ڣ�" + strPath);
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
            Debug.LogError("��ȡVS���������쳣��" + e.Message);
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
    ///// ���ط��������ļ�
    ///// </summary>
    ///// <returns></returns>
    //bool LoadRoomConfig(string strPath)
    //{
    //    if (!File.Exists(strPath))
    //    {
    //        Debug.LogError("�ļ������ڣ�" + strPath);
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
    //        Debug.LogError("��ȡ���������쳣��" + e.Message);
    //        return false;
    //    }
    //}

    /// <summary>
    /// ִ������¼�
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
    /// ִ������¼�
    /// </summary>
    /// <param name="pEvent"></param>
    /// <param name="player"></param>
    public void DoRandomEvent(int nRandomID, CPlayerBaseInfo player)
    {
        //��ȡAction���
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

        //��������������
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
