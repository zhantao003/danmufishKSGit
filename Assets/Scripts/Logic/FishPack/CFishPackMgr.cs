using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFishPackMgr : MonoBehaviour
{
    /// <summary>
    /// 鱼群随机数量
    /// </summary>
    public Vector2Int vRangeCount;

    /// <summary>
    /// 创建等待时间
    /// </summary>
    public Vector2 vRangeCreateTime;

    /// <summary>
    /// 鱼群对象根节点
    /// </summary>
    public GameObject objFishRoot;

    /// <summary>
    /// 出生点
    /// </summary>
    public Transform[] arrRandStartPos;

    CPropertyTimer pTickerCreate = new CPropertyTimer();

    private void Start()
    {
        Refresh(0.4F);
    }

    private void Update()
    {
        if (pTickerCreate.Tick(CTimeMgr.DeltaTime))
        {
            //CreateFish(GetRandMapSlots(Random.Range(vRangeCount.x, vRangeCount.y + 1)));

            CreateFish(GetPlayerUnitsSlots(Random.Range(vRangeCount.x, vRangeCount.y + 1)));

            Refresh(1F);
        }
    }

    List<CMapSlot> GetRandMapSlots(int num)
    {
        if (CGameColorFishMgr.Ins.pMap == null) return null;
        List<CMapSlot> listMapSlots = CGameColorFishMgr.Ins.pMap.GetRandIdleRootWithoutFish(num);

        if (CGameColorFishMgr.Ins.pMap.pOuHuangSlot.pFishPack == null)
        {
            listMapSlots.Add(CGameColorFishMgr.Ins.pMap.pOuHuangSlot);
        }

        if (CGameColorFishMgr.Ins.pMap.pTietieSlot.pFishPack == null)
        {
            listMapSlots.Add(CGameColorFishMgr.Ins.pMap.pTietieSlot);
        }

        return listMapSlots;
    }

    List<CMapSlot> GetPlayerUnitsSlots(int num)
    {
        List<CMapSlot> listSlots = new List<CMapSlot>();

        List<CPlayerUnit> listUnits = CPlayerMgr.Ins.GetAllIdleUnit();
       
        for (int i = 0; i < num; i++)
        {
            if (i >= listUnits.Count) break;

            if (listUnits[i].pMapSlot == null ||
               listUnits[i].pMapSlot.emType == CMapSlot.EMType.Ouhuang)
            {
                listUnits.RemoveAt(i);
                continue;
            }

            listSlots.Add(listUnits[i].pMapSlot);
        }

        return listSlots;
    }

    void CreateFish(List<CMapSlot> listMapSlots)
    {
        if (listMapSlots == null ||
            listMapSlots.Count <= 0) return;

        for (int i=0; i<listMapSlots.Count; i++)
        {
            if(listMapSlots[i].pFishPack!=null)
            {
                listMapSlots[i].pFishPack.RefreshTime();
            }
            else
            {
                GameObject objNewFish = GameObject.Instantiate(objFishRoot) as GameObject;
                Transform tranNewFish = objNewFish.transform;
                tranNewFish.SetParent(null);
                tranNewFish.position = arrRandStartPos[Random.Range(0, arrRandStartPos.Length)].position;
                tranNewFish.localRotation = Quaternion.identity;
                tranNewFish.localScale = Vector3.one;

                CFishPack pFishPack = objNewFish.GetComponent<CFishPack>();
                pFishPack.SetTarget(listMapSlots[i]);
            }
        }

        UIManager.Instance.OpenUI(UIResType.FishPackWarning);
    }

    [ContextMenu("创建于")]
    void TestCreate()
    {
        CreateFish(GetPlayerUnitsSlots(Random.Range(vRangeCount.x, vRangeCount.y + 1)));
    }

    private void Refresh(float lerp)
    {
        pTickerCreate.Value = Random.Range(vRangeCreateTime.x, vRangeCreateTime.y) * lerp;
        pTickerCreate.FillTime();
    }
}
