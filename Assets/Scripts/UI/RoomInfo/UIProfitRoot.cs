using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIProfitRoot : MonoBehaviour
{
    public GameObject objSelf;

    public UIProfitPlayerInfo[] playerInfos;

    public UIProfitPlayerInfo uiCamption;

    public int nMaxCount;

    public string szTopID;

    bool bFirst = true;

    public void InitInfo()
    {
        Refresh();
    }

    public void AddPlayerInfo(CFishRecordInfo recordInfo)
    {
        bool bRefresh = CProfitRankMgr.Ins.AddInfo(recordInfo,nMaxCount);
        if (bRefresh)
        {
            CProfitRankMgr.Ins.Sort();
            Refresh();
        }
    }

    public void Refresh()
    {
        List<ProfitRankInfo> listShowInfos = CProfitRankMgr.Ins.GetRankInfos();
        if (listShowInfos.Count > 0)
        {
            uiCamption.Init(listShowInfos[0]);
        }
        else
        {
            uiCamption.Init(null);
        }

        for (int i = 0; i < listShowInfos.Count; i++)
        {
            if (i >= playerInfos.Length) break;
            playerInfos[i].SetActive(true);
            playerInfos[i].Init(listShowInfos[i]);
        }

        for (int i = listShowInfos.Count; i < playerInfos.Length; i++)
        {
            playerInfos[i].SetActive(false);
        }

        if (listShowInfos.Count > 0)
        {
            
            if (bFirst)
            {
                bFirst = false;
            }
            else if(szTopID != listShowInfos[0].nlUserUID)
            {
                CPlayerBaseInfo baseInfo = CPlayerMgr.Ins.GetPlayer(listShowInfos[0].nlUserUID);
                CRankChgInfo rankChgInfo = new CRankChgInfo();
                rankChgInfo.pPlayerInfo = baseInfo;
                rankChgInfo.emRankChgType = EMRankChgType.Profit;
                rankChgInfo.nRank = 1;

                UIRankChg.ShowInfo(rankChgInfo);
                CPlayerUnit prePlayerUnit = CPlayerMgr.Ins.GetIdleUnit(szTopID);
                if (prePlayerUnit != null)
                {
                    prePlayerUnit.objEffRich.SetActive(false);
                }
            }
            szTopID = listShowInfos[0].nlUserUID;

            CPlayerUnit playerUnit = CPlayerMgr.Ins.GetIdleUnit(szTopID);
            if(playerUnit != null)
            {
                playerUnit.objEffRich.SetActive(true);
            }
        }
    }

    public void ShowAllPlayerName(bool show)
    {
        for (int i = 0; i < playerInfos.Length; i++)
        {
            playerInfos[i].ShowNameLabel(show);
        }
    }

    public void OnClickReset()
    {
        UIMsgBox.Show("重置收益榜", "是否重置收益排行榜，请谨慎操作", UIMsgBox.EMType.YesNo, delegate ()
        {
            Clear();
        });
    }

    public void Clear()
    {
        bFirst = true;
        CProfitRankMgr.Ins.Clear();
        if(!CHelpTools.IsStringEmptyOrNone(szTopID))
        {
            CPlayerUnit playerUnit = CPlayerMgr.Ins.GetIdleUnit(szTopID);
            if (playerUnit != null)
            {
                playerUnit.objEffRich.SetActive(false);
            }
        }
        szTopID = string.Empty;
        Refresh();
    }

}