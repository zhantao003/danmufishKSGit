using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILocalProfitResSlot : MonoBehaviour
{
    public GameObject objRoot;
    public RawImage uiPlayerIcon;
    public Text uiPlayerName;
    public Text uiFishPrice;
    public GameObject objChampion;
    public Text uiAddChampionCount;

    public Text uiPoint;
    public Text uiProfitRank;

    string szBindUID;

    public void InitInfo(ProfitRankInfo profitRankInfo,int nGetChampionCount)
    {
        if (profitRankInfo == null) return;

        CAysncImageDownload.Ins.setAsyncImage(profitRankInfo.szHeadIcon, uiPlayerIcon);
        uiPlayerName.text = profitRankInfo.szUserName;
        uiFishPrice.text = CHelpTools.GetGoldSZ(profitRankInfo.nlTotalCoin);

        if (objChampion != null)
        {
            objChampion.SetActive(nGetChampionCount > 0);
            uiAddChampionCount.text = "+" + nGetChampionCount;
        }

        szBindUID = profitRankInfo.nlUserUID;

        uiProfitRank.text = "";
        uiPoint.text = "";
        
    }

    public void RefreshRankInfo()
    {
        if (CHelpTools.IsStringEmptyOrNone(szBindUID))
            return;

        CPlayerResRankInfo rankInfo = CWorldRankInfoMgr.Ins.GetPlayerResRankSlotInWorld(szBindUID, EMRankType.WinnerRicher);
        if (rankInfo != null)
        {
            uiProfitRank.text = rankInfo.rank.ToString();
            uiPoint.text = CHelpTools.GetGoldSZ(rankInfo.value);
        }
        else
        {
            uiProfitRank.text = "100+";
            CPlayerBaseInfo baseInfo = CPlayerMgr.Ins.GetPlayer(szBindUID);
            if (baseInfo != null)
            {
                uiPoint.text = CHelpTools.GetGoldSZ(baseInfo.nWinnerRicher);
            }
            else
            {
                uiPoint.text = "???";
            }
        }
    }

}
