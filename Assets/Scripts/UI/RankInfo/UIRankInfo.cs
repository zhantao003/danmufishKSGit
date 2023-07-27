using Sirenix.OdinInspector;
using SuperScrollView;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRankInfo : UIBase
{
    public EMRankType emRankType = EMRankType.MapCain;

    [Header("循环列表")]
    public LoopListView2 mLoopListView;

    public UIRankSlot pSelfSlot;

    public UIRankTogType[] pTogs;

    public UIMingRenSlot mingRenSlot;

    #region 二级菜单鱼相关

    public int[] arrFishTag;

    public GameObject objFishTagSlotRoot;

    public RectTransform tranGirdFishTags;

    List<UIRankFishTag> listFishTag = new List<UIRankFishTag>();

    CTBLHandlerFishInfo pTBLHandlerFishInfo;

    bool bInitTBL = false;

    [ReadOnly]
    public int nCurFishID;

    #endregion

    public GameObject objOuHuangReward;
    public GameObject objProfitReward;

    int curIdx = -1;

    List<CPlayerRankInfo> listCurInfos = new List<CPlayerRankInfo>();

    bool bInit = false;

    protected override void OnStart()
    {
        InitTBLFishInfo();
    }

    void InitTBLFishInfo()
    {
        if (bInitTBL) return;

#if TBL_LOCAL
        CTBLInfo.Inst.LoadTBL("TBL/MainFish",
            delegate (CTBLLoader loader)
            {
                pTBLHandlerFishInfo = new CTBLHandlerFishInfo();
                pTBLHandlerFishInfo.LoadInfo(loader);
            });
#else
        CTBLInfo.Inst.LoadTBLByBundle(CTBLInfo.Inst.pTBLBundle, "MainFish",
        delegate (CTBLLoader loader)
        {
            pTBLHandlerFishInfo = new CTBLHandlerFishInfo();
            pTBLHandlerFishInfo.LoadInfo(loader);
        });
#endif

        //InitFishTags();

        bInitTBL = true;
    }

    void InitFishTags()
    {
        objFishTagSlotRoot.SetActive(false);

        for(int i=0; i<arrFishTag.Length; i++)
        {
            GameObject objNewSlot = GameObject.Instantiate(objFishTagSlotRoot) as GameObject;
            objNewSlot.SetActive(true);
            Transform tranNewSlot = objNewSlot.GetComponent<Transform>();
            tranNewSlot.SetParent(tranGirdFishTags);
            tranNewSlot.localPosition = Vector3.zero;
            tranNewSlot.localRotation = Quaternion.identity;
            tranNewSlot.localScale = Vector3.one;

            UIRankFishTag pNewSlot = objNewSlot.GetComponent<UIRankFishTag>();
            pNewSlot.SetInfo(pTBLHandlerFishInfo.GetInfo(arrFishTag[i]));

            listFishTag.Add(pNewSlot);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(tranGirdFishTags);
    }

    public override void OnOpen()
    {
        base.OnOpen();

        

        InitTBLFishInfo();

        OnClickRankType((int)emRankType);
    }
    
    public void SetRankType(EMRankType rankType)
    {
        emRankType = rankType;
        Debug.Log("排行榜类型：" + rankType.ToString());
        for(int i=0; i<pTogs.Length; i++)
        {
            pTogs[i].uiBtn.interactable = (pTogs[i].emRankType != rankType);
        }

        objOuHuangReward.SetActive(rankType == EMRankType.WinnerOuhuang);
        objProfitReward.SetActive(rankType == EMRankType.WinnerRicher);

        //特殊鱼有二级菜单
        if (rankType == EMRankType.RareFish)
        {
            tranGirdFishTags.gameObject.SetActive(true);
            SetFishRank(arrFishTag[0]);
        }
        else
        {
            tranGirdFishTags.gameObject.SetActive(false);
        }
    }

    public void SetFishRank(int tbid)
    {
        nCurFishID = tbid;
        for(int i=0; i<listFishTag.Count; i++)
        {
            listFishTag[i].SetSelect(tbid == listFishTag[i].nFishID);
        }
    }

    LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= listCurInfos.Count)
        {
            return null;
        }

        CPlayerRankInfo pRankInfo = listCurInfos[index];
        LoopListViewItem2 item = listView.NewListViewItem("RankSlot");
        UIRankSlot itemScript = item.GetComponent<UIRankSlot>();
        itemScript.SetInfo(pRankInfo, emRankType);

        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
        }

        return item;
    }

    public void RefreshList(List<CPlayerRankInfo> infos)
    {
        listCurInfos = infos;

        if (!bInit)
        {
            mLoopListView.InitListView(listCurInfos.Count, OnGetItemByIndex);
            bInit = true;
        }
        else
        {
            mLoopListView.SetForceListItemCount(listCurInfos.Count, true);
            //mLoopListView.RefreshAllShownItem();
        }
    }

    public void OnClickRankType(int rankType)
    {
        EMRankType emRankType = (EMRankType)rankType;
        SetRankType(emRankType);
        mingRenSlot.InitInfo(emRankType);
        InitRankInfo(emRankType, nCurFishID);
    }

    //请求排行榜
    public void InitRankInfo(EMRankType rankType, int value)
    {
        if(CWorldRankInfoMgr.Ins.IsRankNeedReq(rankType,value))
        {
            CWorldRankInfoMgr.Ins.ReqRankInfo(rankType, value, delegate ()
            {
                RefreshList(CWorldRankInfoMgr.Ins.GetRankInfo(rankType, value));
            });
        }
        else
        {
            RefreshList(CWorldRankInfoMgr.Ins.GetRankInfo(rankType, value));
        }
    }

    public void OnClickFishInfo(int fishId)
    {
        SetFishRank(fishId);

        InitRankInfo(emRankType, nCurFishID);
    }

    public void OnClickLocalRecord()
    {
        CloseSelf();
        UIManager.Instance.OpenUI(UIResType.LocalRankList);
    }

    public void OnClickExit()
    {
        CloseSelf();
    }

    public void OnClickMingRenTang()
    {
        CloseSelf();
        UIManager.Instance.OpenUI(UIResType.URAvatarInfo);
    }
    
}
