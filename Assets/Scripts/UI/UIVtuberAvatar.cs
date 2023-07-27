using SuperScrollView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVtuberAvatar : UIBase
{
    public LoopGridView listAvatars;

    int nCurAvatarId;

    List<CPlayerAvatarInfo> listInfos = new List<CPlayerAvatarInfo>();

    bool bInited = false;

    protected override void OnStart()
    {

    }

    public override void OnOpen()
    {
        Init();
        ///Ω¯»Îƒ¨»œ÷√∂•
        listAvatars.MovePanelToItemByIndex(0,0);
    }

    public override void OnClose()
    {
       
    }

    public void Init()
    {
        if (CPlayerMgr.Ins.pOwner == null ||
            CPlayerMgr.Ins.pOwner.pAvatarPack == null) return;

        nCurAvatarId = CPlayerMgr.Ins.pOwner.avatarId;

        RefreshInfo();
    }

    public void RefreshInfo()
    {
        listInfos.Clear();
        listInfos.AddRange(CPlayerMgr.Ins.pOwner.pAvatarPack.listAvatars);

        if (!bInited)
        {
            listAvatars.InitGridView(listInfos.Count, OnHelperInfoByIndex);
            bInited = true;
        }
        else
        {
            listAvatars.SetListItemCount(listInfos.Count);
            listAvatars.RefreshAllShownItem();
        }
    }

    LoopGridViewItem OnHelperInfoByIndex(LoopGridView gridView, int itemIndex, int row, int column)
    {
        LoopGridViewItem item = gridView.NewListViewItem("AvatarSlot");

        UIVutberAvatarSlot itemScript = item.GetComponent<UIVutberAvatarSlot>();

        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
        }

        if (itemIndex < listInfos.Count && itemIndex >= 0)
        {
            itemScript.InitInfo(listInfos[itemIndex]);
        }
        else
        {
            itemScript.InitInfo(listInfos[itemIndex]);
        }
        return item;
    }

    public void OnClickClose()
    {
        CloseSelf();
    }
}
