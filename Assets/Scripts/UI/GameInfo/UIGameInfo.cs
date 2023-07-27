using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIGameInfo : UIBase
{
    /// <summary>
    /// 电池目标
    /// </summary>
    public UIBatteryTarget uiBatteryTarget;

    /// <summary>
    /// 渔场等级
    /// </summary>
    public GameObject uiBoardMapLv;

    /// <summary>
    /// 炸鱼模式的倒计时面板
    /// </summary>
    public GameObject uiBoardGameVSTimeCounter;

    //抽奖盒子
    public Transform tranGachaBoxRoot;
    public GameObject objGachaBoxSlot;
    public Transform tranGachaGiftRoot;
    public GameObject objGachaGiftSlot;
    public List<UIGameGachaGiftInfo> listIdleGiftSlots = new List<UIGameGachaGiftInfo>();   //对象池

    public Transform tranBoomFishRoot;
    public GameObject objBoomFishSlot;
    public List<UIGameBoomFishInfo> listIdleBoomFishSlots = new List<UIGameBoomFishInfo>();   //对象池

    public Transform tranAuctionFishRoot;
    public GameObject objAuctionFishSlot;
    public List<UIGameAuctionInfo> listIdleAuctionFishSlots = new List<UIGameAuctionInfo>();   //对象池

    //盲盒
    public Transform tranGiftGachaBoxRoot;
    public GameObject objGiftGachaBoxSlot;
    public List<UIGameGiftGachaSlot> listIdleGiftGachaBoxSlots = new List<UIGameGiftGachaSlot>();   //对象池

    public Transform tranUnitInfoRoot;
    public GameObject objUnitInfoRoot;
    public List<UIShowUnitInfo> listShowSlots = new List<UIShowUnitInfo>();

    public Transform tranUnitDiaglogInfoRoot;
    public GameObject objUnitDiaglogInfoRoot;
    public List<UIShowUnitDialog> listShowDiaglogSlots = new List<UIShowUnitDialog>();

    public Transform tranUnitFishInfoInfoRoot;
    public GameObject objUnitFishInfoInfoRoot;
    public List<UIShowFishInfo> listShowFishInfoSlots = new List<UIShowFishInfo>();

    public Text uiLabelSceneDes;
    public Text uiLabelQuestBattery;
    public Text uiLabelSceneLevel;

    public GameObject objNormalRoot;
    public GameObject objMaxRoot;

    public UIShowNPCInfo showAuctionInfo;
    public UIShowNPCInfo showMatAuctionInfo;
    public UIShowNPCInfo showHelpInfo;
    public UIShowNPCInfo showGiftInfo;

    public Dictionary<long, GameObject> dicGachaBoxTips = new Dictionary<long, GameObject>();

    public UIShowUnitExit uiShowUnit;

    public UITweenPos uiBatteryTween;
    public Vector3 vShowPos;
    public Vector3 vHidePos;

    /// <summary>
    /// 展示对应面板
    /// </summary>
    /// <param name="tweenTarget"></param>
    /// <param name="fDelay"></param>
    /// <param name="fPlay"></param>
    public void ShowBatteryBoard(float fPlay, float fDelay)
    {
        uiBatteryTween.enabled = true;
        uiBatteryTween.from = vHidePos;
        uiBatteryTween.to = vShowPos;
        uiBatteryTween.delayTime = fDelay;
        uiBatteryTween.playTime = fPlay;
        uiBatteryTween.Play();
    }

    /// <summary>
    /// 隐藏对应面板
    /// </summary>
    /// <param name="tweenTarget"></param>
    /// <param name="fDelay"></param>
    /// <param name="fPlay"></param>
    public void HideBatteryBoard(float fPlay, float fDelay)
    {
        uiBatteryTween.enabled = true;
        uiBatteryTween.from = vShowPos;
        uiBatteryTween.to = vHidePos;
        uiBatteryTween.delayTime = fDelay;
        uiBatteryTween.playTime = fPlay;
        uiBatteryTween.Play();
    }


    protected override void OnStart()
    {
        objUnitInfoRoot.SetActive(false);
        objUnitDiaglogInfoRoot.SetActive(false);
        objUnitFishInfoInfoRoot.SetActive(false);

        objGachaBoxSlot.SetActive(false);
        objGachaGiftSlot.SetActive(false);
        objBoomFishSlot.SetActive(false);
        objAuctionFishSlot.SetActive(false);
        objGiftGachaBoxSlot.SetActive(false);

        uiShowUnit.SetActive(false);
    }

    /// <summary>
    /// 设置地图经验显示
    /// </summary>
    /// <param name="value"></param>
    public void SetMapExp(long value)
    {
        bool bMax = CGameColorFishMgr.Ins.nCurRateUpLv - 1 >= CGameColorFishMgr.Ins.pGameRoomConfig.nRateEvent.Length;
        objNormalRoot.SetActive(!bMax);
        objMaxRoot.SetActive(bMax);
        if (!bMax)
        {
            int nLvIdx = Mathf.Max(0, CGameColorFishMgr.Ins.nCurRateUpLv - 1);
            if(nLvIdx >=0 && nLvIdx < CGameColorFishMgr.Ins.pGameRoomConfig.nRateEvent.Length)
            {
                uiLabelQuestBattery.text = (CGameColorFishMgr.Ins.pGameRoomConfig.nRateEvent[nLvIdx] - value).ToString();
            }
            else
            {
                uiLabelQuestBattery.text = "0";
            }
        }
        ST_SceneRate mapRate = CTBLHandlerSceneRate.Ins.GetInfo(CGameColorFishMgr.Ins.nCurRateUpLv);
        if (mapRate != null)
        {
            uiLabelSceneDes.text = mapRate.szDes;
            uiLabelSceneLevel.text = mapRate.szName;
        }
    }

    /// <summary>
    /// 设置电池投喂目标显示
    /// </summary>
    /// <param name="value"></param>
    public void SetCurGameBattery(long value)
    {
        uiBatteryTarget.SetCurGameBattery(value);
    }


    public override void OnOpen()
    {
        if (CGameColorFishMgr.Ins == null) return;

        if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal)
        {
            //uiBoardMapLv.gameObject.SetActive(true);
            uiBoardGameVSTimeCounter.gameObject.SetActive(false);
            //uiBatteryTarget.SetActive(CGameColorFishMgr.Ins.pGameRoomConfig.nlBatteryTarget > 0);
            uiBatteryTarget.SetActive(false);
        }
        else if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
        {
            uiBoardMapLv.SetActive(false);
            //uiBoardMapLv.gameObject.SetActive(true);
            uiBoardGameVSTimeCounter.gameObject.SetActive(false);
            //uiBatteryTarget.SetActive(CGameColorFishMgr.Ins.pGameRoomConfig.nlBatteryTarget > 0);
            uiBatteryTarget.SetActive(false);
        }
        else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema)
        {
            //uiBoardMapLv.gameObject.SetActive(true);
            uiBoardGameVSTimeCounter.gameObject.SetActive(false);
            //uiBatteryTarget.SetActive(CGameColorFishMgr.Ins.pGameRoomConfig.nlBatteryTarget > 0);
            uiBatteryTarget.SetActive(false);
        }
        else if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive)
        {
            //uiBoardMapLv.gameObject.SetActive(false);
            uiBoardGameVSTimeCounter.gameObject.SetActive(false);
            uiBatteryTarget.SetActive(false);
        }
        else if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Battle)
        {
            //uiBoardMapLv.gameObject.SetActive(false);
            //uiBoardGameVSTimeCounter.gameObject.SetActive(true);
            uiBoardGameVSTimeCounter.gameObject.SetActive(false);
            uiBatteryTarget.SetActive(false);
        }
        else if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
        {
            //uiBoardMapLv.gameObject.SetActive(false);
            uiBoardMapLv.SetActive(false);
            uiBoardGameVSTimeCounter.gameObject.SetActive(false);
            uiBatteryTarget.SetActive(false);
        }
        
        SetMapExp(CGameColorFishMgr.Ins.CurMapExp);
        SetCurGameBattery(CGameColorFishMgr.Ins.CurGameBattery);

        CGameColorFishMgr.Ins.dlgChgMapExp += this.SetMapExp;
        CGameColorFishMgr.Ins.dlgChgCurGameBattery += this.SetCurGameBattery;


        
    }

    protected override void OnUpdate(float dt)
    {
        //if(Input.GetKeyDown(KeyCode.F8))
        //{
        //    CGameColorFishMgr.Ins.bShowPlayerInfo = !CGameColorFishMgr.Ins.bShowPlayerInfo;

        //    //刷新玩家名牌
        //    for(int i=0; i< listShowSlots.Count; i++)
        //    {
        //        listShowSlots[i].ShowNameLabel(CGameColorFishMgr.Ins.bShowPlayerInfo);
        //    }

        //    //刷新排行榜
        //    UIRoomInfo uiRoomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
        //    if(uiRoomInfo!=null)
        //    {
        //        uiRoomInfo.rankRoot.ShowAllPlayerName(CGameColorFishMgr.Ins.bShowPlayerInfo);
        //        uiRoomInfo.profitRoot.ShowAllPlayerName(CGameColorFishMgr.Ins.bShowPlayerInfo);
        //    }

        //    ////关闭鱼汛
        //    //if(CGameColorFishMgr.Ins.bShowPlayerInfo)
        //    //{
        //    //    UIManager.Instance.OpenUI(UIResType.ShowRoot);
        //    //}
        //    //else
        //    //{
        //    //    UIManager.Instance.CloseUI(UIResType.ShowRoot);
        //    //}
        //}

        //if(Input.GetKeyDown(KeyCode.G))
        //{
        //    CheckAuction(EMAuctionLimitType.Single);
        //}
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    CheckAuction(EMAuctionLimitType.Multiple);
        //}
        //CheckAuction(EMAuctionLimitType.Single);
        //CheckAuction(EMAuctionLimitType.Multiple);
    }

    void CheckAuction(EMAuctionLimitType limitType)
    {
        ST_AuctionTreasureInfo curTreasureInfo = null;
        int nTotalWeight = 0;
        ///获得随机的宝箱信息
        List<ST_AuctionTreasureInfo> listInfos = new List<ST_AuctionTreasureInfo>();
        listInfos.AddRange(CTBLHandlerAuctionTreasureInfo.Ins.GetInfos());
        for (int i = 0; i < listInfos.Count;)
        {
            if (listInfos[i].emAuctionLimitType != limitType)
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
            curTreasureInfo = listInfos[i];
            break;
        }
        if (curTreasureInfo == null)
        {
            Debug.LogError(limitType.ToString() + "Null Info");
        }
        else
        {
            Debug.Log(limitType.ToString() + "GetInfo ====" + curTreasureInfo.nID);
        }
    }

    public override void OnClose()
    {
        if (CGameColorFishMgr.Ins != null)
        {
            CGameColorFishMgr.Ins.dlgChgMapExp -= this.SetMapExp;
            CGameColorFishMgr.Ins.dlgChgCurGameBattery -= this.SetCurGameBattery;
        }
    }

    public void AddUnit(CPlayerUnit unit)
    {
        AddUnitSlot(unit);
        AddUnitDialogSlot(unit);
        AddUnitFishInfoSlot(unit);
    }

    public void ShowUnitExitInfo(CPlayerUnit unit)
    {
        if (unit.listNormalBooms.Count > 0 ||
            unit.listTreasureBooms.Count > 0) return;
        uiShowUnit.SetActive(true);

        uiShowUnit.pTargetUnit = unit;

        Vector3 vTargetScreenPos = Camera.main.WorldToScreenPoint(unit.tranSelf.position);
        vTargetScreenPos.z = 0F;

        Vector3 vSelfWorldPos = UIManager.Instance.uiCamDefault.uiCam.ScreenToWorldPoint(vTargetScreenPos);
        uiShowUnit.tranSelf.position = vSelfWorldPos;
        uiShowUnit.tranSelf.localPosition = new Vector3(uiShowUnit.tranSelf.localPosition.x, uiShowUnit.tranSelf.localPosition.y, 0F);
    }


    public void SetAuctionInfo(CNPCUnit unit)
    {
        if (showAuctionInfo == null) return;
        showAuctionInfo.SetUnit(unit);
    }

    public void SetMatAuctionInfo(CNPCUnit unit)
    {
        if (showMatAuctionInfo == null) return;
        showMatAuctionInfo.SetUnit(unit);
    }

    public void SetHelpInfo(CNPCUnit unit)
    {
        if (showHelpInfo == null) return;
        showHelpInfo.SetUnit(unit);
    }

    public void SetGiftInfo(CNPCUnit unit)
    {
        if (showGiftInfo == null) return;
        showGiftInfo.SetUnit(unit);
    }

    public void SetGiftDialog(string szInfo)
    {
        if (showGiftInfo == null) return;
        showGiftInfo.SetDmContent(szInfo);
    }


    #region UnitSlot

    public void AddUnitSlot(CPlayerUnit unit)
    {
        GameObject objNewInfoSlot = GameObject.Instantiate(objUnitInfoRoot) as GameObject;
        objNewInfoSlot.SetActive(true);

        Transform tranNewInfo = objNewInfoSlot.GetComponent<Transform>();
        tranNewInfo.SetParent(tranUnitInfoRoot);
        tranNewInfo.localScale = Vector3.one;
        tranNewInfo.localRotation = Quaternion.identity;
        tranNewInfo.localPosition = Vector3.zero;

        UIShowUnitInfo pNewSlot = objNewInfoSlot.GetComponent<UIShowUnitInfo>();

        pNewSlot.SetUnit(unit);

        listShowSlots.Add(pNewSlot);
    }

    public UIShowUnitInfo GetShowSlot(string uid)
    {
        UIShowUnitInfo unit = null;
        for (int i = 0; i < listShowSlots.Count; i++)
        {
            if (listShowSlots[i].lOwnerID.Equals(uid))
            {
                unit = listShowSlots[i];
                break;
            }
        }

        return unit;
    }

    public void RecycleUnitInfo(UIShowUnitInfo slot)
    {
        for (int i = 0; i < listShowSlots.Count; i++)
        {
            if (listShowSlots[i].lOwnerID == slot.lOwnerID)
            {
                Destroy(slot.gameObject);
                listShowSlots.RemoveAt(i);
                break;
            }
        }
    }

    public void ShowAllUnitInfo(bool value)
    {
        for (int i = 0; i < listShowSlots.Count; i++)
        {
            if (listShowSlots[i] == null) continue;
            listShowSlots[i].gameObject.SetActive(value);
        }
    }

    #endregion

    #region UnitDialog

    public void AddUnitDialogSlot(CPlayerUnit unit)
    {
        GameObject objNewInfoSlot = GameObject.Instantiate(objUnitDiaglogInfoRoot) as GameObject;
        objNewInfoSlot.SetActive(true);

        Transform tranNewInfo = objNewInfoSlot.GetComponent<Transform>();
        tranNewInfo.SetParent(tranUnitDiaglogInfoRoot);
        tranNewInfo.localScale = Vector3.one;
        tranNewInfo.localRotation = Quaternion.identity;
        tranNewInfo.localPosition = Vector3.zero;

        UIShowUnitDialog pNewSlot = objNewInfoSlot.GetComponent<UIShowUnitDialog>();

        pNewSlot.SetUnit(unit);

        listShowDiaglogSlots.Add(pNewSlot);
    }

    public UIShowUnitDialog GetShowDialogSlot(string uid)
    {
        UIShowUnitDialog unit = null;
        for (int i = 0; i < listShowDiaglogSlots.Count; i++)
        {
            if (listShowDiaglogSlots[i].lOwnerID.Equals(uid))
            {
                unit = listShowDiaglogSlots[i];
                break;
            }
        }

        return unit;
    }

    public void RecycleUnitDialogInfo(UIShowUnitDialog slot)
    {
        for (int i = 0; i < listShowDiaglogSlots.Count; i++)
        {
            if (listShowDiaglogSlots[i].lOwnerID == slot.lOwnerID)
            {
                Destroy(slot.gameObject);
                listShowDiaglogSlots.RemoveAt(i);
                break;
            }
        }
    }

    public void ShowAllUnitDialogInfo(bool value)
    {
        for (int i = 0; i < listShowDiaglogSlots.Count; i++)
        {
            if (listShowDiaglogSlots[i] == null) continue;
            listShowDiaglogSlots[i].gameObject.SetActive(value);
        }
    }

    #endregion

    #region UnitFishInfo

    public void AddUnitFishInfoSlot(CPlayerUnit unit)
    {
        GameObject objNewInfoSlot = GameObject.Instantiate(objUnitFishInfoInfoRoot) as GameObject;
        objNewInfoSlot.SetActive(true);

        Transform tranNewInfo = objNewInfoSlot.GetComponent<Transform>();
        tranNewInfo.SetParent(tranUnitFishInfoInfoRoot);
        tranNewInfo.localScale = Vector3.one;
        tranNewInfo.localRotation = Quaternion.identity;
        tranNewInfo.localPosition = Vector3.zero;

        UIShowFishInfo pNewSlot = objNewInfoSlot.GetComponent<UIShowFishInfo>();

        pNewSlot.SetUnit(unit);

        listShowFishInfoSlots.Add(pNewSlot);
    }

    public UIShowFishInfo GetShowFishInfoSlot(string uid)
    {
        UIShowFishInfo unit = null;
        for (int i = 0; i < listShowFishInfoSlots.Count; i++)
        {
            if (listShowFishInfoSlots[i].lOwnerID.Equals(uid))
            {
                unit = listShowFishInfoSlots[i];
                break;
            }
        }

        return unit;
    }

    public void RecycleUnitFishInfoInfo(UIShowFishInfo slot)
    {
        for (int i = 0; i < listShowFishInfoSlots.Count; i++)
        {
            if (listShowFishInfoSlots[i].lOwnerID == slot.lOwnerID)
            {
                Destroy(slot.gameObject);
                listShowFishInfoSlots.RemoveAt(i);
                break;
            }
        }
    }

    public void ShowAllUnitFishInfoInfo(bool value)
    {
        for (int i = 0; i < listShowFishInfoSlots.Count; i++)
        {
            if (listShowFishInfoSlots[i] == null) continue;
            listShowFishInfoSlots[i].gameObject.SetActive(value);
        }
    }

    #endregion

    #region 氪金宝箱相关

    public void AddGachaBox(CGachaBox box)
    {
        if (objGachaBoxSlot == null) return;
        GameObject objNewBoxTip = GameObject.Instantiate(objGachaBoxSlot) as GameObject;
        objNewBoxTip.SetActive(true);
        Transform tranNewBoxTip = objNewBoxTip.transform;
        tranNewBoxTip.SetParent(tranGachaBoxRoot);
        tranNewBoxTip.localScale = Vector3.one;
        tranNewBoxTip.localRotation = Quaternion.identity;
        tranNewBoxTip.localPosition = new Vector3(10000, 100000f, 0);

        UIGameGachaBoxInfo uiGachaInfo = objNewBoxTip.GetComponent<UIGameGachaBoxInfo>();
        uiGachaInfo.InitInfo(box.lOwnerUID, box.tranUIRoot);
        box.dlgOver += RemoveGachaBox;

        dicGachaBoxTips.Add(box.lGuid, objNewBoxTip);
    }

    void RemoveGachaBox(long guid)
    {
        GameObject objRes;
        if (dicGachaBoxTips.TryGetValue(guid, out objRes))
        {
            Destroy(objRes);
        }

        dicGachaBoxTips.Remove(guid);
    }

    public void AddGachaGift(CGachaGiftInfo msg, Vector3 pos)
    {
        UIGameGachaGiftInfo pNewSlot = null;
        if (listIdleGiftSlots.Count > 0)
        {
            pNewSlot = listIdleGiftSlots[0];
            listIdleGiftSlots.RemoveAt(0);
        }

        if (pNewSlot != null)
        {
            pNewSlot.Init(msg, pos);
        }
        else
        {
            GameObject objNewSlot = GameObject.Instantiate(objGachaGiftSlot) as GameObject;
            objNewSlot.SetActive(true);
            Transform tranNewSlot = objNewSlot.transform;
            tranNewSlot.SetParent(tranGachaGiftRoot);
            tranNewSlot.localPosition = Vector3.zero;
            tranNewSlot.localRotation = Quaternion.identity;
            tranNewSlot.localScale = Vector3.one;

            pNewSlot = objNewSlot.GetComponent<UIGameGachaGiftInfo>();
            pNewSlot.Init(msg, pos);
        }
    }

    public void RecycleGachaGift(UIGameGachaGiftInfo slot)
    {
        listIdleGiftSlots.Add(slot);
        slot.tranSelf.localPosition = new Vector3(10000F, 0F, 0F);
    }

    #endregion

    #region 深水炸弹相关

    public void AddBoomFish(string uid, CFishInfo msg, Vector3 pos, bool eff, DelegateNFuncCall pEvent)
    {
        UIGameBoomFishInfo pNewSlot = null;
        if (listIdleBoomFishSlots.Count > 0)
        {
            pNewSlot = listIdleBoomFishSlots[0];
            listIdleBoomFishSlots.RemoveAt(0);
        }
        if (pNewSlot != null)
        {
            pNewSlot.Init(uid, msg, pos, eff, pEvent);
        }
        else
        {
            GameObject objNewSlot = GameObject.Instantiate(objBoomFishSlot) as GameObject;
            objNewSlot.SetActive(true);
            Transform tranNewSlot = objNewSlot.transform;
            tranNewSlot.SetParent(tranGachaGiftRoot);
            tranNewSlot.localPosition = Vector3.zero;
            tranNewSlot.localRotation = Quaternion.identity;
            tranNewSlot.localScale = Vector3.one;
            pNewSlot = objNewSlot.GetComponent<UIGameBoomFishInfo>();
            pNewSlot.Init(uid, msg, pos, eff, pEvent);
        }
    }

    public void RecycleBoomFish(UIGameBoomFishInfo slot)
    {
        listIdleBoomFishSlots.Add(slot);
        slot.tranSelf.localPosition = new Vector3(10000F, 0F, 0F);
    }

    #endregion

    #region 拍卖相关

    public void AddAuctionFish(string uid, CAuctionInfo auctionInfo, Vector3 pos, bool eff, DelegateNFuncCall pEvent)
    {
        UIGameAuctionInfo pNewSlot = null;
        if (listIdleAuctionFishSlots.Count > 0)
        {
            pNewSlot = listIdleAuctionFishSlots[0];
            listIdleAuctionFishSlots.RemoveAt(0);
        }
        if (pNewSlot != null)
        {
            pNewSlot.Init(uid, auctionInfo, pos, eff, pEvent);
        }
        else
        {
            GameObject objNewSlot = GameObject.Instantiate(objAuctionFishSlot) as GameObject;
            objNewSlot.SetActive(true);
            Transform tranNewSlot = objNewSlot.transform;
            tranNewSlot.SetParent(tranAuctionFishRoot);
            tranNewSlot.localPosition = Vector3.zero;
            tranNewSlot.localRotation = Quaternion.identity;
            tranNewSlot.localScale = Vector3.one;
            pNewSlot = objNewSlot.GetComponent<UIGameAuctionInfo>();
            pNewSlot.Init(uid, auctionInfo, pos, eff, pEvent);
        }
    }
    
    public void RecycleAuctionFish(UIGameAuctionInfo slot)
    {
        listIdleAuctionFishSlots.Add(slot);
        slot.tranSelf.localPosition = new Vector3(10000F, 0F, 0F);
    }

    #endregion

    #region 盲盒相关

    public void AddGiftGachaSlot(CGiftGachaBoxInfo msg, Vector3 pos)
    {
        UIGameGiftGachaSlot pNewSlot = null;
        if (listIdleGiftGachaBoxSlots.Count > 0)
        {
            pNewSlot = listIdleGiftGachaBoxSlots[0];
            listIdleGiftGachaBoxSlots.RemoveAt(0);
        }

        if (pNewSlot != null)
        {
            pNewSlot.Init(msg, pos);
        }
        else
        {
            GameObject objNewSlot = GameObject.Instantiate(objGiftGachaBoxSlot) as GameObject;
            objNewSlot.SetActive(true);
            Transform tranNewSlot = objNewSlot.transform;
            tranNewSlot.SetParent(tranGiftGachaBoxRoot);
            tranNewSlot.localPosition = Vector3.zero;
            tranNewSlot.localRotation = Quaternion.identity;
            tranNewSlot.localScale = Vector3.one;

            pNewSlot = objNewSlot.GetComponent<UIGameGiftGachaSlot>();
            pNewSlot.Init(msg, pos);
        }
    }

    public void RecycleFishGiftGachaSlot(UIGameGiftGachaSlot slot)
    {
        listIdleGiftGachaBoxSlots.Add(slot);
        slot.tranSelf.localPosition = new Vector3(10000F, 0F, 0F);
    }

    #endregion

    #region 测试用

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        OnClickAddRobot();
    //        Debug.Log(CPlayerMgr.Ins.dicIdleUnits.Count + "=====当前玩家人数=====" + CPlayerMgr.Ins.dicActiveUnits.Count);
    //    }
    //    if (Input.GetKeyDown(KeyCode.D))
    //    {
    //        CPlayerMgr.Ins.RemoveRobot();
    //        Debug.Log(CPlayerMgr.Ins.dicIdleUnits.Count + "=====当前玩家人数=====" + CPlayerMgr.Ins.dicActiveUnits.Count);
    //    }
    //}

    //public void OnClickAddRobot()
    //{
    //    long nRandUID = CHelpTools.GenerateId();
    //    if (CPlayerMgr.Ins.GetPlayer(nRandUID.ToString()) != null) return;

    //    CPlayerBaseInfo pPlayerInfo = new CPlayerBaseInfo(nRandUID.ToString(), "机器人", "null", 0, "", false, 0, CDanmuSDKCenter.Ins.szRoomId, CPlayerBaseInfo.EMUserType.Guanzhong);
    //    pPlayerInfo.bIsRobot = true;

    //    pPlayerInfo.avatarId = Random.Range(101, 105);
    //    pPlayerInfo.nLv = 1;
    //    pPlayerInfo.GameCoins = 5000;
    //    pPlayerInfo.AvatarSuipian = 0;
    //    pPlayerInfo.nBattery = 0;

    //    CPlayerMgr.Ins.AddPlayer(pPlayerInfo);

    //    CGameColorFishMgr.Ins.JoinPlayer(pPlayerInfo, CGameColorFishMgr.EMJoinType.Normal);
    //}

    #endregion

}
