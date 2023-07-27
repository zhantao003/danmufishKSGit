using SuperScrollView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomBossDmgRankboard : MonoBehaviour
{
    public LoopListView2 mLoopListView;

    List<CGameBossDmgInfo> listCurInfos = new List<CGameBossDmgInfo>();

    List<UIRoomBossDmgRankSlot> listSlots = new List<UIRoomBossDmgRankSlot>();

    public UIRoomBossDmgRankSlot dmgRankSlot;

    public GameObject[] objShowRank;
    public GameObject[] objHideRank;

    public UITweenPos uiRankTween;
    public Vector3 vRankShow;
    public Vector3 vRankHide;
    public bool bShowRank = true;
    public Image pArrowImg;

    bool bInit = false;

    public void RegistEvent()
    {
        CGameBossMgr.Ins.callDmgListRefresh = RefreshInfo;
        CGameBossMgr.Ins.callDmgSlotRefresh = RefreshTargetInfo;
        RefreshList(new List<CGameBossDmgInfo>());
    }

    public void RefreshTargetInfo(string uid,long dmg)
    {
        UIRoomBossDmgRankSlot bossDmgRankSlot = listSlots.Find(x => x.nlUID.Equals(uid));
        if(bossDmgRankSlot != null)
        {
            bossDmgRankSlot.RefreshDmgInfo(dmg);
        }
    }

    public void RefreshInfo()
    {
        List<CGameBossDmgInfo> listBossDmgInfos = CGameBossMgr.Ins.GetPlayerDmgList();
        RefreshList(listBossDmgInfos);
    }

    public void RefreshList(List<CGameBossDmgInfo> infos)
    {
        listCurInfos = infos;
        listSlots.Clear();
        if(listCurInfos.Count > 0)
        {
            dmgRankSlot.SetInfo(listCurInfos[0]);
        }
        else
        {
            dmgRankSlot.SetInfo(null);
        }
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

        CGameBossDmgInfo pRankInfo = listCurInfos[index];
        LoopListViewItem2 item = listView.NewListViewItem("RankSlot");
        UIRoomBossDmgRankSlot itemScript = item.GetComponent<UIRoomBossDmgRankSlot>();
        itemScript.SetInfo(pRankInfo);

        if (item.IsInitHandlerCalled == false)
        {
            item.IsInitHandlerCalled = true;
        }
        listSlots.Add(itemScript);

        return item;
    }

    public void ShowRank()
    {
        if (uiRankTween == null) return;
        uiRankTween.from = vRankHide;
        uiRankTween.to = vRankShow;
        uiRankTween.delayTime = 0f;
        uiRankTween.Play();
        bShowRank = true;
        for (int i = 0; i < objShowRank.Length; i++)
        {
            objShowRank[i].SetActive(true);
        }
        for (int i = 0; i < objHideRank.Length; i++)
        {
            objHideRank[i].SetActive(false);
        }
        if (pArrowImg == null) return;
        pArrowImg.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
    }

    public void HideRank()
    {
        if (uiRankTween == null) return;
        uiRankTween.from = vRankShow;
        uiRankTween.to = vRankHide;
        uiRankTween.delayTime = 0f;
        uiRankTween.Play();
        bShowRank = false;
        for (int i = 0; i < objShowRank.Length; i++)
        {
            objShowRank[i].SetActive(false);
        }
        for (int i = 0; i < objHideRank.Length; i++)
        {
            objHideRank[i].SetActive(true);
        }
        if (pArrowImg == null) return;
        pArrowImg.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
    }

    public void OnClickShowRank()
    {
        if (bShowRank)
        {
            HideRank();
        }
        else
        {
            ShowRank();
        }
    }

}
