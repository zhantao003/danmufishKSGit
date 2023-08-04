using SuperScrollView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAvatarInfo : UIBase
{
    public enum EMType
    {
        OuHuang,
        Richer,
    }

    public EMType emCurType = EMType.OuHuang;

    public Text uiLabelTip;

    public Button[] arrBtnType;

    public LoopGridView uiGrid;

    bool bInited = false;

    //bool bInitWinnerOuhuang = false;
    //public List<CGMFesInfo> listWinnerOuhuangInfos = new List<CGMFesInfo>();

    //bool bInitWinnerRicher = false;
    //public List<CGMFesInfo> listWinnerRicherInfos = new List<CGMFesInfo>();

    List<CFishFesInfoSlot> listInfos = new List<CFishFesInfoSlot>();

    public override void OnOpen()
    {
        SetType(EMType.OuHuang);
    }

    public void OnClickTog(int value)
    {
        SetType((EMType)value);
    }

    void RefreshInfoOuHuang(List<CFishFesInfoSlot> infos)
    {
        listInfos.Clear();
        listInfos.AddRange(infos);

        if (!bInited)
        {
            uiGrid.InitGridView(listInfos.Count, OnGetItemByIndex);
            bInited = true;
        }
        else
        {
            uiGrid.SetListItemCount(listInfos.Count);
            uiGrid.RefreshAllShownItem();
        }
    }

    void RefreshInfoRicher(List<CFishFesInfoSlot> infos)
    {
        listInfos.Clear();
        listInfos.AddRange(infos);

        if (!bInited)
        {
            uiGrid.InitGridView(listInfos.Count, OnGetItemByIndex);
            bInited = true;
        }
        else
        {
            uiGrid.SetListItemCount(listInfos.Count);
            uiGrid.RefreshAllShownItem();
        }
    }

    LoopGridViewItem OnGetItemByIndex(LoopGridView gridView, int itemIndex, int row, int column)
    {
        LoopGridViewItem item = gridView.NewListViewItem("RoleRoot");

        UIAvatarInfoSlot itemScript = item.GetComponent<UIAvatarInfoSlot>();

        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
        }

        if (emCurType == EMType.OuHuang)
        {
            if (itemIndex < listInfos.Count && itemIndex >= 0)
            {
                itemScript.SetInfo(listInfos[itemIndex], EMType.OuHuang);
            }
            else
            {
                itemScript.SetInfo(listInfos[itemIndex], EMType.OuHuang);
            }
        }
        else if (emCurType == EMType.Richer)
        {
            if (itemIndex < listInfos.Count && itemIndex >= 0)
            {
                itemScript.SetInfo(listInfos[itemIndex], EMType.Richer);
            }
            else
            {
                itemScript.SetInfo(listInfos[itemIndex], EMType.Richer);
            }
        }

        return item;
    }

    public void SetType(EMType togType)
    {
        emCurType = togType;

        for (int i = 0; i < arrBtnType.Length; i++)
        {
            arrBtnType[i].interactable = !((int)togType == i);
        }

        if(emCurType == EMType.OuHuang)
        {
            uiLabelTip.text = "※皇冠达到指定数量自动获得奖励，每赛季重置※";

            RefreshInfoOuHuang(CFishFesInfoMgr.Ins.GetFesPack((long)CFishFesInfoMgr.EMFesType.RankOuhuang));
        }
        else if(emCurType == EMType.Richer)
        {
            uiLabelTip.text = "※皇冠达到指定数量自动获得奖励，每赛季重置※";

            RefreshInfoRicher(CFishFesInfoMgr.Ins.GetFesPack((long)CFishFesInfoMgr.EMFesType.RankRicher));
        }
    }
}
