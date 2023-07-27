using SuperScrollView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EMLocalRankType
{
    MapOuHuang = 0,         //渔场欧皇
    MapCain,                //渔场收益

    Max,
}

public class UILocalRankList : UIBase
{
    [Header("循环列表")]
    public LoopListView2 mLoopListView;

    public Button[] pTogs;

    public Dropdown uiLocalRecordDropDown;

    public List<CPlayerLocalRankInfo> listCurInfos = new List<CPlayerLocalRankInfo>();

    bool bInit = false;

    public override void OnOpen()
    {
        base.OnOpen();

        InitDropDown();
    }

    public void InitDropDown()
    {
        CLocalRankInfoMgr.Ins.CheckLocalFile();
        uiLocalRecordDropDown.onValueChanged.AddListener(ChgLocalRecord);
        List<string> listStrReslution = new List<string>();
        for (int i = 0; i < CLocalRankInfoMgr.Ins.listRankFileInfos.Count; i++)
        {
            if (CLocalRankInfoMgr.Ins.listRankFileInfos[i] == null)
                continue;
            listStrReslution.Add(CLocalRankInfoMgr.Ins.listRankFileInfos[i].GetTitle());
        }
        uiLocalRecordDropDown.ClearOptions();
        uiLocalRecordDropDown.AddOptions(listStrReslution);
        uiLocalRecordDropDown.SetValueWithoutNotify(0);
        ChgLocalRecord(0);
    }

    /// <summary>
    /// 选择本地排行记录
    /// </summary>
    /// <param name="nIdx"></param>
    public void ChgLocalRecord(int nIdx)
    {
        CLocalRankInfoMgr.Ins.LoadMsg(nIdx, delegate ()
         {
             OnClickShowRank((int)EMLocalRankType.MapOuHuang);
         });
    }

    public void ShowRankInfo(EMLocalRankType localRankType)
    {
        string szGetInfoValue = string.Empty;
        if(localRankType == EMLocalRankType.MapOuHuang)
        {
            szGetInfoValue = "RankInfos";
        }
        else if(localRankType == EMLocalRankType.MapCain)
        {
            szGetInfoValue = "ProfitInfos";
        }
        CLocalNetArrayMsg arrayMsg = CLocalRankInfoMgr.Ins.GetArrayMsg(szGetInfoValue);
        if (arrayMsg == null) return;
        List<CPlayerLocalRankInfo> infos = new List<CPlayerLocalRankInfo>();
        int nSize = arrayMsg.GetSize();
        for(int i = 0;i < nSize;i++)
        {
            CLocalNetMsg msgInfo = arrayMsg.GetNetMsg(i);
            CPlayerLocalRankInfo localRankInfo = new CPlayerLocalRankInfo();
            localRankInfo.emRankType = localRankType;
            localRankInfo.LoadMsg(msgInfo, i);
            infos.Add(localRankInfo);
        }
        RefreshList(infos);

    }

    public void RefreshList(List<CPlayerLocalRankInfo> infos)
    {
        listCurInfos = infos;

        if (!bInit)
        {
            mLoopListView.InitListView(listCurInfos.Count, OnGetItemByIndex);
            bInit = true;
        }
        else
        {
            mLoopListView.SetForceListItemCount(listCurInfos.Count);
            //mLoopListView.RefreshAllShownItem();
        }
    }

    LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= listCurInfos.Count)
        {
            return null;
        }

        CPlayerLocalRankInfo pRankInfo = listCurInfos[index];
        LoopListViewItem2 item = listView.NewListViewItem("RankSlot");
        UILocalRankSlot itemScript = item.GetComponent<UILocalRankSlot>();
        itemScript.InitInfo(pRankInfo, pRankInfo.emRankType);

        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
        }

        return item;
    }

    public void OnClickShowRank(int nIdx)
    {
        ShowRankInfo((EMLocalRankType)nIdx);
        for (int i = 0; i < pTogs.Length; i++)
        {
            pTogs[i].interactable = (i != nIdx);
        }
    }

    public void OnClickClose()
    {
        CloseSelf();
        UIManager.Instance.OpenUI(UIResType.RankList);
    }

}
