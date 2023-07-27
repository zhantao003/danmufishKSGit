using SuperScrollView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRoomVSRankList : MonoBehaviour
{
    public LoopListView2 uiGridList;

    List<CRankTreasureSlot> listCurInfos;

    bool bInit = false;

    private void Start()
    {
        CRankTreasureMgr.Ins.dlgRefreshInfo += this.RefreshList;
    }

    public void Init()
    {
        RefreshList();
    }

    void RefreshList()
    {
        RefreshList(CRankTreasureMgr.Ins.GetRankInfos());
    }

    public void Reset()
    {
        listCurInfos.Clear();
        RefreshList(listCurInfos);

        uiGridList.MovePanelToItemIndex(0, 0);
    }

    public void RefreshList(List<CRankTreasureSlot> infos)
    {
        listCurInfos = infos;

        if (!bInit)
        {
            uiGridList.InitListView(listCurInfos.Count, OnGetItemByIndex);
            bInit = true;
        }
        else
        {
            uiGridList.SetForceListItemCount(listCurInfos.Count);
            //uiGridList.RefreshAllShownItem();
        }
    }

    LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= listCurInfos.Count)
        {
            return null;
        }

        CRankTreasureSlot pInfo = listCurInfos[index];

        LoopListViewItem2 item = null;

        item = listView.NewListViewItem("RankSlot");
        UIRoomVSRankSlot itemScript = item.GetComponent<UIRoomVSRankSlot>();
        itemScript.SetInfo(pInfo);

        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
        }

        return item;
    }
}
