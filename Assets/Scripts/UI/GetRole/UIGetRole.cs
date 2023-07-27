using SuperScrollView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetRoleInfo
{
    public List<ST_UnitAvatar> avatarInfos;     //列表信息

    public ST_UnitAvatar.EMRare emRare;             //标题类型

    public GetRoleInfo()
    {
        avatarInfos = new List<ST_UnitAvatar>();
        emRare = ST_UnitAvatar.EMRare.None;
    }

    public void Sort()
    {
        avatarInfos.Sort((x, y) => {
            if(x.nID < y.nID)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        });
    }
}


public class UIGetRole : UIBase
{
    public List<GetRoleInfo> listData = new List<GetRoleInfo>();
    [Header("循环列表")]
    public LoopListView2 mLoopListView;

    public UIRoomAvatarInfo roomAvatarInfo;

    int nHorMaxCount = 5;

    public Text uiLabelSuipian;

    public ST_UnitAvatar.EMTag emCurTag = ST_UnitAvatar.EMTag.Normal;

    public Button[] arrTagTogs;

    public bool bInit = false;



    public override void OnOpen()
    {
        base.OnOpen();
        Init();
        if (bInit)
        {
            mLoopListView.MovePanelToItemIndex(0, 0);
        }
    }

    public override void OnClose()
    {
        base.OnClose();

        UnregistDlg();
    }

    public void Init()
    {
        //刷新UI
        OnClickTag(0);

        uiLabelSuipian.text = CPlayerMgr.Ins.pOwner.AvatarSuipian.ToString();
        RegistDlg();

        CPlayerBaseInfo playerBaseInfo = CPlayerMgr.Ins.pOwner;
        if (playerBaseInfo == null)
            return;
        roomAvatarInfo.SetAvatar(playerBaseInfo.avatarId);
    }

    void RegistDlg()
    {
        if (CPlayerMgr.Ins.pOwner == null) return;

        CPlayerMgr.Ins.pOwner.dlgChgAvatarSuipian += this.SetAvatarSuipian;
    }

    void UnregistDlg()
    {
        if (CPlayerMgr.Ins.pOwner == null) return;

        CPlayerMgr.Ins.pOwner.dlgChgAvatarSuipian -= this.SetAvatarSuipian;
    }

    void SetAvatarSuipian(long value)
    {
        uiLabelSuipian.text = value.ToString();
    }

    void GetInfo()
    {
        listData.Clear();

        ST_UnitAvatar.EMRare emRare = ST_UnitAvatar.EMRare.None;

        List<ST_UnitAvatar> unitAvatars = new List<ST_UnitAvatar>();
        unitAvatars.AddRange(CTBLHandlerUnitAvatar.Ins.GetInfos());

        ////临时去掉tag
        //unitAvatars.RemoveAll(x =>  x.emTag == ST_UnitAvatar.EMTag.Diy);

        unitAvatars.Sort((x, y) => {
            if (x.nID < y.nID)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        });
        GetRoleInfo getRoleInfo = new GetRoleInfo();
        List<ST_UnitAvatar> saveAvatars = new List<ST_UnitAvatar>();
        for (int i = 0;i < unitAvatars.Count;i++)
        {
            if (unitAvatars[i].emTag != emCurTag)
            {
                if(i == unitAvatars.Count - 1 &&
                    getRoleInfo != null &&
                    getRoleInfo.avatarInfos.Count > 0)
                {
                    listData.Add(getRoleInfo);
                    getRoleInfo = new GetRoleInfo();
                }
                continue;
            }
            ///判断是否需要增加标题信息
            if (emRare != unitAvatars[i].emRare)
            {
                if(getRoleInfo != null && getRoleInfo.avatarInfos.Count > 0)
                {
                    listData.Add(getRoleInfo);
                    getRoleInfo = new GetRoleInfo();
                }
                //判断是否存在该类型的标题
                emRare = unitAvatars[i].emRare;
                GetRoleInfo getTitleInfo = new GetRoleInfo();
                getTitleInfo.emRare = emRare;
                listData.Add(getTitleInfo);
            }
            if(getRoleInfo.avatarInfos.Count < nHorMaxCount)
            {
                getRoleInfo.avatarInfos.Add(unitAvatars[i]);
                if (getRoleInfo.avatarInfos.Count >= nHorMaxCount)
                {
                    listData.Add(getRoleInfo);
                    getRoleInfo = new GetRoleInfo();
                }
                else if (i == unitAvatars.Count - 1)
                {
                    listData.Add(getRoleInfo);
                    getRoleInfo = new GetRoleInfo();
                }
            }

        }
        //for(int i = 0;i < listData.Count;i++)
        //{
        //    Debug.Log(listData[i].avatarInfos.Count + "====== Count =====" + i);
        //}
        //Debug.Log(listData.Count + "====== End Info =====" + unitAvatars.Count);
    }


    public void Refresh()
    {
        //获取信息
        GetInfo();
        if (bInit)
        {
            mLoopListView.SetListItemCount(listData.Count, false);
            mLoopListView.RefreshAllShownItem();
        }
        else
        {
            mLoopListView.InitListView(listData.Count, OnGetItemByIndex);
            bInit = true;
        }
    }

    public void OnClickTag(int tag)
    {
        emCurTag = (ST_UnitAvatar.EMTag)tag;

        arrTagTogs[0].interactable = (emCurTag != ST_UnitAvatar.EMTag.Normal);
        arrTagTogs[1].interactable = (emCurTag != ST_UnitAvatar.EMTag.Fes);
        arrTagTogs[2].interactable = (emCurTag != ST_UnitAvatar.EMTag.Diy);

        Refresh();
    }


    LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= listData.Count)
        {
            return null;
        }

        GetRoleInfo getRoleInfo = listData[index];
        LoopListViewItem2 item = null;
        if(getRoleInfo.avatarInfos.Count > 0)
        {
            item = listView.NewListViewItem("RoleRoot");
            UIListRole itemScript = item.GetComponent<UIListRole>();
            itemScript.SetInfo(getRoleInfo.avatarInfos);
        }
        else
        {
            item = listView.NewListViewItem("Title");
            UITitleRoot itemScript = item.GetComponent<UITitleRoot>();
            itemScript.SetInfo(getRoleInfo.emRare);
        }
        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
        }
        
        return item;
    }

    public void OnClickUR()
    {
        CloseSelf();
        UIManager.Instance.OpenUI(UIResType.URAvatarInfo);
    }
}
