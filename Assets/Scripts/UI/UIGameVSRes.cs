using SuperScrollView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameVSRes : UIBase
{
    public LoopListView2 uiGridList;

    public CPropertyTimer pTimeTicker = new CPropertyTimer();

    public Text uiLabelCount;

    List<CRankTreasureSlot> listCurInfos;

    bool bInit = false;

    public override void OnOpen()
    {
        //��ʼ������
        pTimeTicker.FillTime();

        RefreshInfo();
    }

    void RefreshInfo()
    {
        listCurInfos = CRankTreasureMgr.Ins.GetRankInfos();

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
        UILocalTreasureSlot itemScript = item.GetComponent<UILocalTreasureSlot>();
        itemScript.InitInfoByRankInfo(index, pInfo);

        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
        }

        return item;
    }

    private void Update()
    {
        if(pTimeTicker.Tick(CTimeMgr.DeltaTime))
        {
            uiLabelCount.text = "0����Զ���ʼ��һ����Ϸ";
            OnClickOK();
        }
        else
        {
            uiLabelCount.text = $"{(int)pTimeTicker.CurValue}����Զ���ʼ��һ����Ϸ";
        }
    }

    public void OnClickOK()
    {
        CloseSelf();

        //CGameVSGiftPoolMgr.Ins.InitGiftPool();

        //ˢ�����а���Ϣ
        CLocalRankInfoMgr.Ins.SaveVSInfo();
        CRankTreasureMgr.Ins.Clear();
        CRankTreasureMgr.Ins.dlgRefreshInfo?.Invoke();
        CLocalRankInfoMgr.Ins.RefreshVSFileName();

        //��ֵ�������
        UIRoomVSInfo uiRoomVSInfo = UIManager.Instance.GetUI(UIResType.RoomVSInfo) as UIRoomVSInfo;
        if(uiRoomVSInfo!=null)
        {
            uiRoomVSInfo.uiGiftPool.Init();
        }
    }
}
