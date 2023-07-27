using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomSettingBoardVS : MonoBehaviour
{
    //地图配置相关
    public ScrollRect uiScrollMapList;
    public RectTransform uiGridMapList;
    public GameObject objMapInfoRoot;

    [Header("对应地图的鱼信息")]
    public UIRoomFishList roomFishList;

    [ReadOnly]
    public List<UIRoomMapVSSlot> listMapSlotInfo = new List<UIRoomMapVSSlot>();
    [ReadOnly]
    public UIRoomMapVSSlot curSelectMap;

    //房间基础配置
    public UIRoomConfigDropDown[] arrRoomBaseConfig;

    //鱼的配置
    public GameObject objConfigFishRoot;
    public RectTransform tranConfigFishGrid;
    protected List<UIRoomConfigFishDropDown> listFishConfigs = new List<UIRoomConfigFishDropDown>();

    //海盗金币价值榜
    public GameObject objSlotTreasurePointRoot;
    public RectTransform tranTreasurePointGrid;
    public List<UIRoomTreasurePointSlot> listTreasurePointSlots = new List<UIRoomTreasurePointSlot>();

    private void Start()
    {
        objMapInfoRoot.SetActive(false);
        objConfigFishRoot.SetActive(false);
        objSlotTreasurePointRoot.SetActive(false);

        for (int i = 0; i < arrRoomBaseConfig.Length; i++)
        {
            arrRoomBaseConfig[i].Init();
        }
    }

    public void OnOpen()
    {
        roomFishList.Init();

        InitMapInfo();

        InitTreasurePointList();
    }

    /// <summary>
    /// 初始化地图信息
    /// </summary>
    public void InitMapInfo()
    {
        for (int i = 0; i < listMapSlotInfo.Count; i++)
        {
            Destroy(listMapSlotInfo[i].gameObject);
        }
        listMapSlotInfo.Clear();

        List<ST_GameVSMap> listMapInfo = CTBLHandlerGameVSMap.Ins.GetInfos();
        for (int i = 0; i < listMapInfo.Count; i++)
        {
            int nTBID = listMapInfo[i].nID;
            GameObject objNewSlot = GameObject.Instantiate(objMapInfoRoot) as GameObject;
            objNewSlot.SetActive(true);
            Transform tranNewSlot = objNewSlot.GetComponent<Transform>();
            tranNewSlot.SetParent(uiGridMapList);
            tranNewSlot.localPosition = Vector3.zero;
            tranNewSlot.localRotation = Quaternion.identity;
            tranNewSlot.localScale = Vector3.one;

            UIRoomMapVSSlot pNewSlot = objNewSlot.GetComponent<UIRoomMapVSSlot>();
            pNewSlot.Init(listMapInfo[i]);
            pNewSlot.uiBtnMap.onClick.AddListener(delegate ()
            {
                SelectMap(nTBID);
            });

            listMapSlotInfo.Add(pNewSlot);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(uiGridMapList);
        uiScrollMapList.horizontalNormalizedPosition = 0F;

        SelectMap(listMapInfo[0].nID);
    }

    /// <summary>
    /// 选择地图
    /// </summary>
    public void SelectMap(int id)
    {
        Debug.Log("选择地图");
        if (curSelectMap != null && curSelectMap.nTBID != id)
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

        for (int i = 0; i < listFishConfigs.Count; i++)
        {
            listFishConfigs[i].nMapId = curSelectMap.nTBID;
        }

        //初始化房间选项
        //InitFishConfig();
    }

    public void InitFishConfig()
    {
        bool bNew = false;

        if (CGameColorFishMgr.Ins.pGameRoomVSConfig.GetFishInfo(curSelectMap.nTBID) == null)
        {
            bNew = true;
        }

        for (int i = 0; i < listFishConfigs.Count; i++)
        {
            Destroy(listFishConfigs[i].gameObject);
        }
        listFishConfigs.Clear();

        List<UIFishInfo> listSpecFishInfos = roomFishList.listSpecialFishInfo;

        //listSpecFishInfos.Sort((x, y) =>
        //{
        //    if(x.pTargetInfo.nTBID < y.pTargetInfo.nTBID)
        //    {
        //        return -1;
        //    }
        //    else if (x.pTargetInfo.nTBID == y.pTargetInfo.nTBID)
        //    {
        //        return 0;
        //    }
        //    else
        //    {
        //        return 1;
        //    }
        //});

        ST_GameVSMap pTBLMapInfo = CTBLHandlerGameVSMap.Ins.GetInfo(curSelectMap.nTBID);

        for (int i = 0; i < listSpecFishInfos.Count; i++)
        {
            UIFishInfo uiFishInfo = listSpecFishInfos[i];
            if (uiFishInfo.pTargetInfo.emItemType != EMItemType.Fish)
                continue;

            if (!uiFishInfo.pTargetInfo.bSpecial)
                continue;

            GameObject objNewSlot = GameObject.Instantiate(objConfigFishRoot) as GameObject;
            objNewSlot.SetActive(true);
            Transform tranNewSlot = objNewSlot.GetComponent<Transform>();
            tranNewSlot.SetParent(tranConfigFishGrid);
            tranNewSlot.localPosition = Vector3.zero;
            tranNewSlot.localRotation = Quaternion.identity;
            tranNewSlot.localScale = Vector3.one;

            UIRoomConfigFishDropDown pNewSlot = objNewSlot.GetComponent<UIRoomConfigFishDropDown>();
            if (curSelectMap != null)
            {
                pNewSlot.nMapId = curSelectMap.nTBID;
            }

            int nDefValue = 0;
            //判断是否有默认值
            if (bNew)
            {
                nDefValue = pTBLMapInfo.GetTreasureConfig(uiFishInfo.pTargetInfo.nTBID).nMin;
                CGameColorFishMgr.Ins.pGameRoomVSConfig.SetFishInfo(curSelectMap.nTBID, uiFishInfo.pTargetInfo.nTBID, 1);
            }
            else
            {
                CGameColorFishVSRoomConfigInfoSlot pFishConfigSlot = CGameColorFishMgr.Ins.pGameRoomVSConfig.GetFishInfo(curSelectMap.nTBID);
                if (pFishConfigSlot != null)
                {
                    nDefValue = pFishConfigSlot.GetFishNum(uiFishInfo.pTargetInfo.nTBID);
                }
            }

            pNewSlot.InitInfo(listSpecFishInfos[i].pTargetInfo, nDefValue);

            listFishConfigs.Add(pNewSlot);
        }
    }

    public void InitTreasurePointList()
    {
#if TBL_LOCAL
        CTBLInfo.Inst.LoadTBL("TBL/BoomFish501", delegate (CTBLLoader loader)
        {
            CTBLHandlerFishInfo pTBLHandlerFishInfo = new CTBLHandlerFishInfo();
            pTBLHandlerFishInfo.LoadInfo(loader);
            List<ST_FishInfo> fishInfos = pTBLHandlerFishInfo.GetInfos();

            for (int i = 0; i < listTreasurePointSlots.Count; i++)
            {
                Destroy(listTreasurePointSlots[i].gameObject);
            }
            listTreasurePointSlots.Clear();

            for (int i = fishInfos.Count - 1; i >= 0; i--)
            {
                if (fishInfos[i].nTreasurePoint > 0)
                {
                    GameObject objSlot = GameObject.Instantiate(objSlotTreasurePointRoot) as GameObject;
                    objSlot.SetActive(true);

                    Transform tranSlot = objSlot.GetComponent<Transform>();
                    tranSlot.SetParent(tranTreasurePointGrid);
                    tranSlot.localScale = Vector3.one;
                    tranSlot.localPosition = Vector3.zero;
                    tranSlot.localRotation = Quaternion.identity;

                    UIRoomTreasurePointSlot pSlot = objSlot.GetComponent<UIRoomTreasurePointSlot>();
                    pSlot.SetInfo(fishInfos[i]);

                    listTreasurePointSlots.Add(pSlot);
                }
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(tranTreasurePointGrid);
        });
#else
        CTBLInfo.Inst.LoadTBLByBundle(CTBLInfo.Inst.pTBLBundle, "BoomFish501", delegate (CTBLLoader loader)
        {
            CTBLHandlerFishInfo pTBLHandlerFishInfo = new CTBLHandlerFishInfo();
            pTBLHandlerFishInfo.LoadInfo(loader);
            List<ST_FishInfo> fishInfos = pTBLHandlerFishInfo.GetInfos();

            for (int i = 0; i < listTreasurePointSlots.Count; i++)
            {
                Destroy(listTreasurePointSlots[i].gameObject);
            }
            listTreasurePointSlots.Clear();

            for (int i = fishInfos.Count - 1; i >= 0; i--)
            {
                if (fishInfos[i].nTreasurePoint > 0)
                {
                    GameObject objSlot = GameObject.Instantiate(objSlotTreasurePointRoot) as GameObject;
                    objSlot.SetActive(true);

                    Transform tranSlot = objSlot.GetComponent<Transform>();
                    tranSlot.SetParent(tranTreasurePointGrid);
                    tranSlot.localScale = Vector3.one;
                    tranSlot.localPosition = Vector3.zero;
                    tranSlot.localRotation = Quaternion.identity;

                    UIRoomTreasurePointSlot pSlot = objSlot.GetComponent<UIRoomTreasurePointSlot>();
                    pSlot.SetInfo(fishInfos[i]);

                    listTreasurePointSlots.Add(pSlot);
                }
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(tranTreasurePointGrid);
        });
#endif
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

        UIMsgBox.Show("藏宝模式", "该模式只有用炸弹才有概率获得宝藏!\r\n【谨慎使用】", UIMsgBox.EMType.YesNo,
            delegate ()
            {
                CGameColorFishMgr.Ins.pGameRoomVSConfig.bActiveDuel = false;
                CGameColorFishMgr.Ins.pGameRoomVSConfig.nAutoExitTime = 3600;
                Debug.Log(CGameColorFishMgr.Ins.pGameRoomVSConfig.GetLogFishInfo());
                CGameColorFishMgr.Ins.SaveRoomConfig(CGameColorFishMgr.EMGameType.Battle);

                UIManager.Instance.OpenUI(UIResType.Loading);
                UIManager.Instance.CloseUI(UIResType.RoomSetting);
                CSceneMgr.Instance.LoadScene((CSceneFactory.EMSceneType)CGameColorFishMgr.Ins.pMapVSConfig.nID);
            });
    }
}
