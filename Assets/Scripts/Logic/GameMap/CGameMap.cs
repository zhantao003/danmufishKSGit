using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMShowTypeByWait
{
    Battle,
    Auction
}

[System.Serializable]
public class CShowTimeWaitInfo
{
    [Header("出现信息")]
    public float fWaitTime;
    public EMShowTypeByWait emShowTypeByWait;
    public long[] value;
    public int[] nAuctionTypeWeight;
}

public class CGameMap : MonoBehaviour
{
    public float fAvatarScale;

    [Header("待机点位")]
    public List<CMapSlot> pMapSlots;
    //public CMapSlot[] pMapSlots;
    [Header("增加点位")]
    public List<CMapSlot> pAddMapSlots;
    [Header("欧皇台")]
    public CMapSlot pOuHuangSlot;
    [Header("贴贴台")]
    public CMapSlot pTietieSlot;

    public Transform tranBoundMin;
    public Transform tranBoundMax;
    public Vector3 vPosIdle;

    public string szFishPath;
    public string szBoomFishPath;
    public string szTreasureFishPath;
    public string szTreasureFishBoomPath;

    /// <summary>
    /// 普通鱼表
    /// </summary>
    public CTBLHandlerFishInfo pTBLHandlerFishInfo;
    /// <summary>
    /// 炸鱼表
    /// </summary>
    public CTBLHandlerFishInfo pTBLHandlerBoomFishInfo;
    /// <summary>
    /// 藏宝海湾表
    /// </summary>
    public CTBLHandlerFishInfo pTBLHandlerTreasureFishInfo;
    /// <summary>
    /// 藏宝海湾炸鱼表
    /// </summary>
    public CTBLHandlerFishInfo pTBLHandlerTreasureBombFishInfo;

    public Transform[] arrTranGachaRoots;
    List<Transform> listIdleGachaRoot = new List<Transform>();

	//普通小船
    public GameObject[] arrBoatCommon;
    public GameObject objBoartOuhuangRoot;

    [Header("当前决斗船")]
    public CDuelBoat pDuelBoat;
    [Header("鱼币决斗船")]
    public CDuelBoat pNormalBoat;
    [Header("海盗金币决斗船")]
    public CDuelBoat pSpecialBoat;
    [Header("送礼船")]
    public CDuelBoat pGiftsBoat;

    public Transform tranMapEffRoot;

    public int nYZMAnswerA;
    public int nYZMAnswerB;

    CPropertyTimer pRefreshYZMTick;
    public float fRefreshTime = 1800;

    public bool bAdd;

    CPropertyTimer pShowTick;
    public CShowTimeWaitInfo[] cShowTimeWaitInfos;

    public int nCurShowBattleIdx = 0;
    public int nCurBattlePriceType = -1;

    [Header("相机位移动画")]
    public UITweenPos uiCameraPos;
    public Vector3 vNormalPos;
    public Vector3 vBattlePos;

    [Header("自动决斗")]
    public bool bAutoBattle;

    public GameObject objMapSlotBase;
    public Transform tranMapSlotRoot;

    public void DoShowTick()
    {
        
        pShowTick = new CPropertyTimer();
        pShowTick.Value = cShowTimeWaitInfos[nCurShowBattleIdx].fWaitTime;
        pShowTick.FillTime();
    }

    public void DoCameraTween(bool bBattle)
    {
        if (uiCameraPos == null) return;
        if (bBattle)
        {
            uiCameraPos.from = vNormalPos;
            uiCameraPos.to = vBattlePos;
        }
        else
        {
            uiCameraPos.from = vBattlePos;
            uiCameraPos.to = vNormalPos;
        }
        uiCameraPos.Play();
    }

    public void DoEvent()
    {
        if (cShowTimeWaitInfos[nCurShowBattleIdx].emShowTypeByWait == EMShowTypeByWait.Battle)
        {
            UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
            if (roomInfo != null)
            {
                long price = 0;
                if (nCurBattlePriceType < 0 &&
                    cShowTimeWaitInfos[nCurShowBattleIdx].value.Length > 0)
                {
                    int nRandomValue = Random.Range(0, cShowTimeWaitInfos[nCurShowBattleIdx].value.Length);
                    price = cShowTimeWaitInfos[nCurShowBattleIdx].value[nRandomValue];
                }
                else
                {
                    if(nCurBattlePriceType >= cShowTimeWaitInfos[nCurShowBattleIdx].value.Length)
                    {
                        nCurBattlePriceType = cShowTimeWaitInfos[nCurShowBattleIdx].value.Length - 1;
                    }
                    price = cShowTimeWaitInfos[nCurShowBattleIdx].value[nCurBattlePriceType];
                }
                roomInfo.battleRoot.nlCurPrice = price;
                roomInfo.battleRoot.nMaxCount = 10;
                roomInfo.ShowBattle();
                DoCameraTween(true);
                nCurBattlePriceType = -1;
            }
        }
        else if (cShowTimeWaitInfos[nCurShowBattleIdx].emShowTypeByWait == EMShowTypeByWait.Auction)
        {
            EMAuctionLimitType randomLimitType = EMAuctionLimitType.Single;
            int nTotalWeight = 0;
            for(int i = 0;i < cShowTimeWaitInfos[nCurShowBattleIdx].nAuctionTypeWeight.Length;i++)
            {
                nTotalWeight += cShowTimeWaitInfos[nCurShowBattleIdx].nAuctionTypeWeight[i];
            }
            int nRandomWeight = Random.Range(0, nTotalWeight + 1);
            for (int i = 0; i < cShowTimeWaitInfos[nCurShowBattleIdx].nAuctionTypeWeight.Length; i++)
            {
                if (cShowTimeWaitInfos[nCurShowBattleIdx].nAuctionTypeWeight[i] <= 0) continue;
                nRandomWeight -= cShowTimeWaitInfos[nCurShowBattleIdx].nAuctionTypeWeight[i];
                if(nRandomWeight <= 0)
                {
                    randomLimitType = (EMAuctionLimitType)i;
                    break;
                }
            }
            CAuctionMgr.Ins.curAuctionLimitType = randomLimitType;
            CNPCMgr.Ins.ShowAuction();
        }
        if (nCurShowBattleIdx < cShowTimeWaitInfos.Length - 1)
        {
            nCurShowBattleIdx++;
            DoShowTick();
        }
        else
        {
            pShowTick = null;
        }
        
    }

    public void EndBattleTick()
    {
        pShowTick = null;
        nCurShowBattleIdx = 0;
    }

    private void Update()
    {
        if (pRefreshYZMTick != null &&
            pRefreshYZMTick.Tick(CTimeMgr.DeltaTime))
        {
            RefreshYZMAnswer(true);
        }
        if (pShowTick != null &&
            pShowTick.Tick(CTimeMgr.DeltaTime))
        {
            pShowTick = null;
            DoEvent();
        }
        ClickChgBattlePrice();
    }

    public void ClickChgBattlePrice()
    {
        if(Input.GetKeyDown(KeyCode.Keypad0))
        {
            nCurBattlePriceType = 0;
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            nCurBattlePriceType = 1;
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            nCurBattlePriceType = 2;
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            nCurBattlePriceType = 3;
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            nCurBattlePriceType = 4;
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            nCurBattlePriceType = 5;
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            nCurBattlePriceType = 6;
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            nCurBattlePriceType = 7;
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            nCurBattlePriceType = 8;
        }
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            nCurBattlePriceType = 9;
        }
    }

    public void RefreshYZMAnswer(bool bRefreshTick)
    {
        nYZMAnswerA = Random.Range(1, 100);
        nYZMAnswerB = Random.Range(1, 100);

        UITreasureInfo.SetYZM();

        UIGameInfo uiShow = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (uiShow == null)
            return;

        List<CPlayerUnit> playerUnits = CPlayerMgr.Ins.GetAllIdleUnit();
        for(int i = 0;i < playerUnits.Count;i++)
        {
            if(playerUnits[i].bCheckYZM)
            {
                UIShowUnitDialog uiUnitInfo = uiShow.GetShowDialogSlot(playerUnits[i].uid);
                if (uiUnitInfo == null)
                    continue;
                ///刷新验证码信息
                uiUnitInfo.RefreshCheckYZM();
            }
        }
        if (bRefreshTick)
        {
            pRefreshYZMTick.FillTime();
        }
    }

    public void Init()
    {
        LoadFishInfo(szFishPath);
        LoadBoomFishInfo(szBoomFishPath);
        LoadTreasureFishInfo(szTreasureFishPath);
        LoadTreasureBombFishInfo(szTreasureFishBoomPath);

        listIdleGachaRoot.Clear();
        listIdleGachaRoot.AddRange(arrTranGachaRoots);

        RefreshYZMAnswer(false);
        pRefreshYZMTick = new CPropertyTimer();
        pRefreshYZMTick.Value = fRefreshTime;
        pRefreshYZMTick.FillTime();

        bAdd = CSystemInfoMgr.Inst.GetBool(CSystemInfoConst.ADDSEAT);
        if (bAdd)
        {
            CSceneRoot pSceneRoot = FindObjectOfType<CSceneRoot>();
            if (pSceneRoot != null)
            {
                pSceneRoot.ShowScene(false);
            }
            pMapSlots.AddRange(pAddMapSlots);
            CGameColorFishMgr.Ins.nMaxPlayerNum += pAddMapSlots.Count;
        }
        else
        {
            for(int i = 0;i < pAddMapSlots.Count;i++)
            {
                pAddMapSlots[i].gameObject.SetActive(false);
            }
        }

        if (pNormalBoat!=null)
        {
            pNormalBoat.Init();
        }
        if(pSpecialBoat != null)
        {
            pSpecialBoat.Init();
        }
        if (pGiftsBoat != null)
        {
            pGiftsBoat.Init();
        }

    }

    void LoadFishInfo(string szFishPack)
    {
#if TBL_LOCAL
        CTBLInfo.Inst.LoadTBL("TBL/" + szFishPack,
                delegate (CTBLLoader loader)
                {
                    pTBLHandlerFishInfo = new CTBLHandlerFishInfo();
                    pTBLHandlerFishInfo.LoadInfo(loader);
                });
#else
        CTBLInfo.Inst.LoadTBLByBundle(CTBLInfo.Inst.pTBLBundle, szFishPack,
            delegate (CTBLLoader loader)
            {
                pTBLHandlerFishInfo = new CTBLHandlerFishInfo();
                pTBLHandlerFishInfo.LoadInfo(loader);
            });
#endif

    }

    void LoadBoomFishInfo(string szFishPack)
    {
#if TBL_LOCAL
        CTBLInfo.Inst.LoadTBL("TBL/" + szFishPack,
                        delegate (CTBLLoader loader)
                        {
                            pTBLHandlerBoomFishInfo = new CTBLHandlerFishInfo();
                            pTBLHandlerBoomFishInfo.LoadInfo(loader);
                        });
#else
        CTBLInfo.Inst.LoadTBLByBundle(CTBLInfo.Inst.pTBLBundle, szFishPack,
          delegate (CTBLLoader loader)
          {
              pTBLHandlerBoomFishInfo = new CTBLHandlerFishInfo();
              pTBLHandlerBoomFishInfo.LoadInfo(loader);
          });
#endif
    }

    void LoadTreasureFishInfo(string szFishPack)
    {
#if TBL_LOCAL
        CTBLInfo.Inst.LoadTBL("TBL/" + szFishPack,
                        delegate (CTBLLoader loader)
                        {
                            pTBLHandlerTreasureFishInfo = new CTBLHandlerFishInfo();
                            pTBLHandlerTreasureFishInfo.LoadInfo(loader);
                        });
#else
        CTBLInfo.Inst.LoadTBLByBundle(CTBLInfo.Inst.pTBLBundle, szFishPack,
          delegate (CTBLLoader loader)
          {
              pTBLHandlerTreasureFishInfo = new CTBLHandlerFishInfo();
              pTBLHandlerTreasureFishInfo.LoadInfo(loader);
          });
#endif
    }

    void LoadTreasureBombFishInfo(string szFishPack)
    {
#if TBL_LOCAL
        CTBLInfo.Inst.LoadTBL("TBL/" + szFishPack,
                        delegate (CTBLLoader loader)
                        {
                            pTBLHandlerTreasureBombFishInfo = new CTBLHandlerFishInfo();
                            pTBLHandlerTreasureBombFishInfo.LoadInfo(loader);
                        });
#else
        CTBLInfo.Inst.LoadTBLByBundle(CTBLInfo.Inst.pTBLBundle, szFishPack,
          delegate (CTBLLoader loader)
          {
              pTBLHandlerTreasureBombFishInfo = new CTBLHandlerFishInfo();
              pTBLHandlerTreasureBombFishInfo.LoadInfo(loader);
          });
#endif
    }

    /// <summary>
    /// 获取一个随机的Idle站位点
    /// </summary>
    /// <returns></returns>
    public CMapSlot GetRandIdleRoot()
    {
        List<CMapSlot> listRand = new List<CMapSlot>();
        for (int i = 0; i < pMapSlots.Count; i++)
        {
            if (pMapSlots[i].pBindUnit == null)
                listRand.Add(pMapSlots[i]);
        }
        
        if (listRand.Count <= 0 &&
            CGameColorFishMgr.Ins.emCurGameType != CGameColorFishMgr.EMGameType.Boss)
        {
            CMapSlot mapSlot = null;
            GameObject objNewSlot = GameObject.Instantiate(objMapSlotBase) as GameObject;
            objNewSlot.SetActive(true);

            Transform tranNewInfo = objNewSlot.GetComponent<Transform>();
            tranNewInfo.SetParent(tranMapSlotRoot);
            tranNewInfo.localScale = Vector3.one;
            tranNewInfo.localRotation = Quaternion.identity;
            tranNewInfo.localPosition = new Vector3(100,0,0);

            mapSlot = objNewSlot.GetComponent<CMapSlot>();
            pMapSlots.Add(mapSlot);
            return mapSlot;
        }

        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss &&
            listRand.Count <= 0)
        {
            return null;
        }

        return listRand[Random.Range(0, listRand.Count)];
    }

    /// <summary>
    /// 随机获得N个没有鱼群的点
    /// </summary>
    /// <returns></returns>
    public List<CMapSlot> GetRandIdleRootWithoutFish(int num)
    {
        List<CMapSlot> listRand = new List<CMapSlot>();
        for (int i = 0; i < pMapSlots.Count; i++)
        {
            if (pMapSlots[i].pFishPack == null)
                listRand.Add(pMapSlots[i]);
        }

        ////欧皇台是否有鱼群
        //if(pOuHuangSlot!= null &&
        //   pOuHuangSlot.pFishPack == null)
        //{
        //    listRand.Add(pOuHuangSlot);
        //}

        if (listRand.Count <= 0) return null;
        List<CMapSlot> listRes = new List<CMapSlot>();
        for(int i=0; i<num; i++)
        {
            if (listRand.Count <= 0) break;
            int nRandIdx = Random.Range(0, listRand.Count);
            listRes.Add(listRand[nRandIdx]);

            listRand.RemoveAt(nRandIdx);
        }

        return listRes;
    }

    /// <summary>
    /// 获取随机氪金盒子位置
    /// </summary>
    /// <returns></returns>
    public Transform GetRandomGachaPos()
    {
        if (listIdleGachaRoot.Count <= 0)
            return null;

        int nRandIdx = Random.Range(0, listIdleGachaRoot.Count);
        Transform tranRes = listIdleGachaRoot[nRandIdx];
        listIdleGachaRoot.RemoveAt(nRandIdx);

        if(listIdleGachaRoot.Count <= 0)
        {
            listIdleGachaRoot.AddRange(arrTranGachaRoots);
        }

        return tranRes;
    }

    /// <summary>
    /// 获得通用小船
    /// </summary>
    /// <param name="guardian"></param>
    /// <returns></returns>
    public GameObject GetBoatCommon(long guardian)
    {
        return arrBoatCommon[0];

        //if (guardian >= 36)   // 总督
        //{
        //    return arrBoatCommon[3];
        //}
        //else if(guardian >=21) //提督
        //{
        //    return arrBoatCommon[2];
        //}
        //else if(guardian >=6) // 舰长
        //{
        //    return arrBoatCommon[1];
        //}
        //else
        //{
        //    return arrBoatCommon[0];
        //}
    }

    //获得随机点
    public Vector3 GetRandomPos()
    {
        return new Vector3(Random.Range(tranBoundMin.position.x, tranBoundMax.position.x),
                           tranBoundMin.position.y,
                           Random.Range(tranBoundMin.position.z, tranBoundMax.position.z));
    }
}
