using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomFishList : MonoBehaviour
{
    //地图配置相关
    public ScrollRect uiNormalFishList;
    public RectTransform uiGridNormalFishList;
    public ScrollRect uiSpecialFishList;
    public RectTransform uiGridSpecialFishList;
    public GameObject objFishInfoRoot;
    [ReadOnly]
    public List<UIFishInfo> listNormalFishInfo = new List<UIFishInfo>();
    [ReadOnly]
    public List<UIFishInfo> listSpecialFishInfo = new List<UIFishInfo>();

    public CTBLHandlerFishInfo pTBLHandlerFishInfo;

    List<CFishInfo> listAllFishInfo = new List<CFishInfo>();

    public void Init()
    {
        if (pTBLHandlerFishInfo == null)
        {
#if TBL_LOCAL
           CTBLInfo.Inst.LoadTBL("TBL/MainFish", delegate (CTBLLoader loader)
                    {
                        pTBLHandlerFishInfo = new CTBLHandlerFishInfo();
                        pTBLHandlerFishInfo.LoadInfo(loader);

                        List<ST_FishInfo> fishInfos = pTBLHandlerFishInfo.GetInfos();
                        listAllFishInfo = new List<CFishInfo>();
                        for (int i = 0; i < fishInfos.Count; i++)
                        {
                            CFishInfo fishInfo = new CFishInfo(fishInfos[i]);
                            listAllFishInfo.Add(fishInfo);
                        }
                        listAllFishInfo.Sort((a, b) =>
                        {
                            if (a.emRare == b.emRare)
                            {
                                return b.nTBID.CompareTo(a.nTBID);
                            }
                            else
                            {
                                return b.emRare.CompareTo(a.emRare);
                            }
                        });
                    });
#else
            CTBLInfo.Inst.LoadTBLByBundle(CTBLInfo.Inst.pTBLBundle, "MainFish", delegate (CTBLLoader loader)
            {
                pTBLHandlerFishInfo = new CTBLHandlerFishInfo();
                pTBLHandlerFishInfo.LoadInfo(loader);

                List<ST_FishInfo> fishInfos = pTBLHandlerFishInfo.GetInfos();
                listAllFishInfo = new List<CFishInfo>();
                for (int i = 0; i < fishInfos.Count; i++)
                {
                    CFishInfo fishInfo = new CFishInfo(fishInfos[i]);
                    listAllFishInfo.Add(fishInfo);
                }
                listAllFishInfo.Sort((a, b) =>
                {
                    if (a.emRare == b.emRare)
                    {
                        return b.nTBID.CompareTo(a.nTBID);
                    }
                    else
                    {
                        return b.emRare.CompareTo(a.emRare);
                    }
                });
            });
#endif
        }
    }

    public void InitFishInfo(int nID)
    {
        Debug.Log("初始化iD：" + nID);
        InitNormalFishInfo(nID);
        InitSpecialFishInfo(nID);
    }

    /// <summary>
    /// 初始化普通鱼信息
    /// </summary>
    /// <param name="id"></param>
    public void InitNormalFishInfo(int id)
    {
        for (int i = 0; i < listNormalFishInfo.Count; i++)
        {
            Destroy(listNormalFishInfo[i].objSelf);
        }
        listNormalFishInfo.Clear();

        for (int i = 0; i < listAllFishInfo.Count; i++)
        {
            //判断是否为该场景ID的鱼信息且不为特产
            if (!listAllFishInfo[i].InSceneID(id))
                continue;
            if (listAllFishInfo[i].emItemType != EMItemType.Fish &&
                listAllFishInfo[i].emItemType != EMItemType.BattleItem)
                continue;
            if (listAllFishInfo[i].bSpecial)
                continue;

            ///活动获得鱼
            if (!CFishFesInfoMgr.Ins.IsFesOn((int)CFishFesInfoMgr.EMFesType.RankRicher) &&
               listAllFishInfo[i].nTBID == 9000001)
            {
                continue;
            }

            GameObject objNewSlot = GameObject.Instantiate(objFishInfoRoot) as GameObject;
            objNewSlot.SetActive(true);
            Transform tranNewSlot = objNewSlot.GetComponent<Transform>();
            tranNewSlot.SetParent(uiGridNormalFishList);
            tranNewSlot.localPosition = Vector3.zero;
            tranNewSlot.localRotation = Quaternion.identity;
            tranNewSlot.localScale = Vector3.one;

            UIFishInfo pNewSlot = objNewSlot.GetComponent<UIFishInfo>();
            pNewSlot.Init(listAllFishInfo[i]);
            listNormalFishInfo.Add(pNewSlot);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(uiGridNormalFishList);
        uiNormalFishList.verticalNormalizedPosition = 0.9F;
    }

    /// <summary>
    /// 初始化特产信息
    /// </summary>
    /// <param name="id"></param>
    public void InitSpecialFishInfo(int id)
    {
        for (int i = 0; i < listSpecialFishInfo.Count; i++)
        {
            Destroy(listSpecialFishInfo[i].objSelf);
        }
        listSpecialFishInfo.Clear();

        for (int i = 0; i < listAllFishInfo.Count; i++)
        {
            //判断是否为该场景ID的特产信息
            if (!listAllFishInfo[i].InSceneID(id))
                continue;
            if (!listAllFishInfo[i].bSpecial)
                continue;

            GameObject objNewSlot = GameObject.Instantiate(objFishInfoRoot) as GameObject;
            objNewSlot.SetActive(true);
            Transform tranNewSlot = objNewSlot.GetComponent<Transform>();
            tranNewSlot.SetParent(uiGridSpecialFishList);
            tranNewSlot.localPosition = Vector3.zero;
            tranNewSlot.localRotation = Quaternion.identity;
            tranNewSlot.localScale = Vector3.one;

            UIFishInfo pNewSlot = objNewSlot.GetComponent<UIFishInfo>();
            pNewSlot.Init(listAllFishInfo[i]);
            listSpecialFishInfo.Add(pNewSlot);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(uiGridSpecialFishList);
        uiSpecialFishList.verticalNormalizedPosition = 1F;
    }

}
