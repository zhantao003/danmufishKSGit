using SuperScrollView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CURAvatarSlotInfo
{
    public string name;
    public string desc;
    public ST_UnitAvatar info;
}

[System.Serializable]
public class CMingRenTangInfo
{
    [Header("名人堂人物信息")]
    public string szUID;
    public string szHeadIcon;
    public string szName;
    public string szDes;
}

[System.Serializable]
public class CMingRenTangInfoArr
{
    [Header("名人堂人物信息队列")]
    public CMingRenTangInfo[] pMingRenInfos;
    public string szTitle;
}


public class UIURAvatarInfo : UIBase
{
    [Header("欧皇名人")]
    public List<CMingRenTangInfoArr> showOuHuangInfos = new List<CMingRenTangInfoArr>();
    [Header("富豪名人")]
    public List<CMingRenTangInfoArr> showProfitInfos = new List<CMingRenTangInfoArr>();


    List<CMingRenTangInfoArr> curShowInfos = new List<CMingRenTangInfoArr>();     

    public LoopGridView pLoopGrid;

    public Button[] arrTagTogs;

    public int emCurTag;

    public bool bInit = false;

    public void Sort()
    {
        //avatarInfos.Sort((x, y) =>
        //{
        //    if (x.info == null && y.info == null) return 0;
        //    if (x.info == null) return 1;
        //    if (y.info == null) return -1;

        //    if (x.info.nID < y.info.nID)
        //    {
        //        return 1;
        //    }
        //    else
        //    {
        //        return -1;
        //    }
        //});
    }

    public override void OnOpen()
    {
        OnClickTag(0);
        //Refresh();
    }

    public void Refresh()
    {
        //avatarInfos.Clear();
        //List<ST_MingrenTang> mingRenTangInfos = CTBLHandlerMingrenTang.Ins.GetInfos();
        //for (int i = 0; i < mingRenTangInfos.Count; i++)
        //{
        //    if (mingRenTangInfos[i].emTag != emCurTag)
        //    {
        //        continue;
        //    }

        //    avatarInfos.Add(mingRenTangInfos[i]);
        //}
        curShowInfos.Clear();
        if (emCurTag == 0)
        {
            curShowInfos.AddRange(showOuHuangInfos);
        }
        else if(emCurTag == 1)
        {
            curShowInfos.AddRange(showProfitInfos);
        }
        Sort();
        //获取信息
        if (!bInit)
        {
            pLoopGrid.InitGridView(curShowInfos.Count, OnGetItemByIndex);
            bInit = true;
        }
        else
        {
            pLoopGrid.SetListItemCount(curShowInfos.Count);
            pLoopGrid.RefreshAllShownItem();
        }
    }

    LoopGridViewItem OnGetItemByIndex(LoopGridView gridView, int itemIndex, int row, int column)
    {
        LoopGridViewItem item = gridView.NewListViewItem("RoleRoot");

        UIURMingRenSlotArr itemScript = item.GetComponent<UIURMingRenSlotArr>();

        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
        }

        if (itemIndex < curShowInfos.Count && itemIndex >= 0)
        {
            itemScript.SetInfo(curShowInfos[itemIndex], emCurTag);
        }
        else
        {
            itemScript.SetInfo(curShowInfos[itemIndex], emCurTag);
        }
        return item;
    }

    public void OnClickTag(int tag)
    {
        emCurTag = tag;

        arrTagTogs[0].interactable = (emCurTag != 0);
        arrTagTogs[1].interactable = (emCurTag != 1);

        Refresh();
    }


    public void OnClickClose()
    {
        CloseSelf();
        UIManager.Instance.OpenUI(UIResType.RankList);
    }
}
