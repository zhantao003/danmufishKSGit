using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomSettingBoardNormal : MonoBehaviour
{
    [Header("对应地图的鱼信息")]
    public UIRoomFishList roomFishList;
    [Header("电池目标输入框")]
    public InputField uiBatteryInput;
    [Header("决斗开关Tog")]
    public Toggle uiActiveDuel;
    [Header("排行榜是否去重")]
    public Toggle uiActiveRankRepeat;
    [Header("加座模式Tog")]
    public Toggle uiTogAddSeat;

    public UIRoomConfigDropDown[] arrRoomConfigRoot;

    //地图配置相关
    public ScrollRect uiScrollMapList;
    public RectTransform uiGridMapList;
    public GameObject objMapInfoRoot;
    public GameObject objMapYugaoRoot;
    [ReadOnly]
    public List<UIRoomMapSlot> listMapSlotInfo = new List<UIRoomMapSlot>();
    protected UIRoomMapSlot curSelectMap;

    //地图章节
    public Button[] arrBtnChapter;

    bool bSetInfo;

    private void Start()
    {
        objMapInfoRoot.SetActive(false);
        if(objMapYugaoRoot!=null)
        {
            objMapYugaoRoot.SetActive(false);
        }
    }

    /// <summary>
    /// 初始化地图信息
    /// </summary>
    public void InitMapInfo()
    {
        ST_GameMap pTBLMapInfo = CTBLHandlerGameMap.Ins.GetInfo(CGameColorFishMgr.Ins.pGameRoomConfig.nSelectMapID);
        if(pTBLMapInfo == null)
        {
            SelectChapter(1);
        }
        else
        {
            SelectChapter(pTBLMapInfo.nChapter);
        }

        //for (int i = 0; i < listMapSlotInfo.Count; i++)
        //{
        //    Destroy(listMapSlotInfo[i].gameObject);
        //}
        //listMapSlotInfo.Clear();

        //List<ST_GameMap> listMapInfo = CTBLHandlerGameMap.Ins.GetInfos();
        //for (int i = 0; i < listMapInfo.Count; i++)
        //{
        //    int nTBID = listMapInfo[i].nID;
        //    GameObject objNewSlot = GameObject.Instantiate(objMapInfoRoot) as GameObject;
        //    objNewSlot.SetActive(true);
        //    Transform tranNewSlot = objNewSlot.GetComponent<Transform>();
        //    tranNewSlot.SetParent(uiGridMapList);
        //    tranNewSlot.localPosition = Vector3.zero;
        //    tranNewSlot.localRotation = Quaternion.identity;
        //    tranNewSlot.localScale = Vector3.one;

        //    UIRoomMapSlot pNewSlot = objNewSlot.GetComponent<UIRoomMapSlot>();
        //    pNewSlot.Init(listMapInfo[i]);
        //    pNewSlot.uiBtnMap.onClick.AddListener(delegate ()
        //    {
        //        SelectMap(nTBID);
        //    });

        //    listMapSlotInfo.Add(pNewSlot);
        //}

        //////添加预告
        ////if (objMapYugaoRoot != null)
        ////{
        ////    GameObject objYugaoSlot = GameObject.Instantiate(objMapYugaoRoot) as GameObject;
        ////    objYugaoSlot.SetActive(true);
        ////    Transform tranYugaoSlot = objYugaoSlot.GetComponent<Transform>();
        ////    tranYugaoSlot.SetParent(uiGridMapList);
        ////    tranYugaoSlot.localPosition = Vector3.zero;
        ////    tranYugaoSlot.localRotation = Quaternion.identity;
        ////    tranYugaoSlot.localScale = Vector3.one;
        ////    UIRoomMapSlot pYugaoSlot = objYugaoSlot.GetComponent<UIRoomMapSlot>();
        ////    listMapSlotInfo.Add(pYugaoSlot);
        ////}

        //LayoutRebuilder.ForceRebuildLayoutImmediate(uiGridMapList);
        //uiScrollMapList.horizontalNormalizedPosition = 0F;

        //ST_GameMap gameMap = GetMapInfo(CGameColorFishMgr.Ins.pGameRoomConfig.nSelectMapID, listMapInfo);
        //if(gameMap == null)
        //{
        //    gameMap = listMapInfo[0];
        //}
        //SelectMap(gameMap.nID);
    }

    public ST_GameMap GetMapInfo(int nID, List<ST_GameMap> listMapInfo)
    {
        ST_GameMap mapInfo = null;

        mapInfo = listMapInfo.Find(x => x.nID == nID);

        return mapInfo;
    }

    /// <summary>
    /// 选择地图
    /// </summary>
    public void SelectMap(int id)
    {
        Debug.Log("选择地图");
        if (curSelectMap != null && 
            curSelectMap.nTBID != id)
        {
            curSelectMap.Select(false);
        }

        for (int i = 0; i < listMapSlotInfo.Count; i++)
        {
            if (listMapSlotInfo[i].nTBID == id)
            {
                curSelectMap = listMapSlotInfo[i];
                curSelectMap.Select(true);
                roomFishList.InitFishInfo(id);
                break;
            }
        }
    }

    public void SelectChapter(int chapterId)
    {
        for(int i=0; i<arrBtnChapter.Length; i++)
        {
            arrBtnChapter[i].interactable = (chapterId != (i + 1));
        }

        for (int i = 0; i < listMapSlotInfo.Count; i++)
        {
            Destroy(listMapSlotInfo[i].gameObject);
        }
        listMapSlotInfo.Clear();

        List<ST_GameMap> listMapInfo = CTBLHandlerGameMap.Ins.GetInfos();
        for (int i = 0; i < listMapInfo.Count; i++)
        {
            if (listMapInfo[i].nChapter != chapterId) continue;

            int nTBID = listMapInfo[i].nID;
            GameObject objNewSlot = GameObject.Instantiate(objMapInfoRoot) as GameObject;
            objNewSlot.SetActive(true);
            Transform tranNewSlot = objNewSlot.GetComponent<Transform>();
            tranNewSlot.SetParent(uiGridMapList);
            tranNewSlot.localPosition = Vector3.zero;
            tranNewSlot.localRotation = Quaternion.identity;
            tranNewSlot.localScale = Vector3.one;

            UIRoomMapSlot pNewSlot = objNewSlot.GetComponent<UIRoomMapSlot>();
            pNewSlot.Init(listMapInfo[i]);
            pNewSlot.uiBtnMap.onClick.AddListener(delegate ()
            {
                SelectMap(nTBID);
            });

            listMapSlotInfo.Add(pNewSlot);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(uiGridMapList);
        uiScrollMapList.horizontalNormalizedPosition = 0F;

        ST_GameMap gameMap = GetMapInfo(CGameColorFishMgr.Ins.pGameRoomConfig.nSelectMapID, listMapInfo);
        if (gameMap == null)
        {
            gameMap = listMapInfo[0];
        }
        SelectMap(gameMap.nID);
    }

    public void OnOpen()
    {
        objMapInfoRoot.SetActive(false);

        roomFishList.Init();

        for (int i = 0; i < arrRoomConfigRoot.Length; i++)
        {
            arrRoomConfigRoot[i].Init();
        }

        InitMapInfo();
        bSetInfo = false;
        uiBatteryInput.text = CGameColorFishMgr.Ins.pGameRoomConfig.nlBatteryTarget.ToString();

        //uiActiveDuel.isOn = CGameColorFishMgr.Ins.pGameRoomConfig.bActiveDuel;

        uiActiveRankRepeat.isOn = CGameColorFishMgr.Ins.pGameRoomConfig.bActiveRankRepeat;

        //if(CGameColorFishMgr.Ins.nCurRateUpLv >= CGameColorFishMgr.Ins.pStaticConfig.GetInt("加座位解锁等级"))
        //{
        //    uiTogAddSeat.gameObject.SetActive(true);
        //    uiTogAddSeat.onValueChanged.AddListener(SetAddSeat);
        //    uiTogAddSeat.isOn = CSystemInfoMgr.Inst.GetBool(CSystemInfoConst.ADDSEAT);
        //}
        //else
        //{
        //    uiTogAddSeat.gameObject.SetActive(false);
        //}
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame()
    {
        if (curSelectMap.bLock)
        {
            UIToast.Show("未解锁");
            return;
        }
        
        if (bSetInfo)
        {
            CSystemInfoMgr.Inst.SaveFile();
        }

        //CGameColorFishMgr.Ins.pGameRoomConfig.nlBatteryTarget = int.Parse(uiBatteryInput.text);
        if (CHelpTools.IsStringEmptyOrNone(uiBatteryInput.text) ||
           CTBLHandlerFucAble.Ins.GetInfo((int)EMFuncAbleType.BattryTarget).CheckSceneLv())
        {
            CGameColorFishMgr.Ins.pGameRoomConfig.nlBatteryTarget = 0;
        }
        else
        {
            CGameColorFishMgr.Ins.pGameRoomConfig.nlBatteryTarget = long.Parse(uiBatteryInput.text);
        }

        //CGameColorFishMgr.Ins.pGameRoomConfig.bActiveDuel = uiActiveDuel.isOn;
        CGameColorFishMgr.Ins.pGameRoomConfig.bActiveRankRepeat = uiActiveRankRepeat.isOn;
        CGameColorFishMgr.Ins.pGameRoomConfig.nSelectMapID = CGameColorFishMgr.Ins.pMapConfig.nID;

        CGameColorFishMgr.Ins.SaveRoomConfig(CGameColorFishMgr.EMGameType.Normal);
        if(CGameColorFishMgr.Ins.pMapConfig.emType == ST_GameMap.EMType.Normal)
        {
            CGameColorFishMgr.Ins.emCurGameType = CGameColorFishMgr.EMGameType.TimeBattle;
        }
        else if(CGameColorFishMgr.Ins.pMapConfig.emType == ST_GameMap.EMType.Boss)
        {
            CGameColorFishMgr.Ins.emCurGameType = CGameColorFishMgr.EMGameType.Boss;
        }

        UIManager.Instance.OpenUI(UIResType.Loading);
        UIManager.Instance.CloseUI(UIResType.RoomSetting);
        CSceneMgr.Instance.LoadScene((CSceneFactory.EMSceneType)CGameColorFishMgr.Ins.pMapConfig.nID);
    }

    public void SetAddSeat(bool value)
    {
        bSetInfo = true;
        CSystemInfoMgr.Inst.SetBool(CSystemInfoConst.ADDSEAT, value);
        
    }

}
