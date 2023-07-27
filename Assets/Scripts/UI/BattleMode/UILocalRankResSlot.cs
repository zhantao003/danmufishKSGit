using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILocalRankResSlot : MonoBehaviour
{
    public GameObject objRoot; 
    public RawImage uiIconPlayer;
    public UIFishIconLoad uiIconFish;

    public Text uiPlayerName;
    //public Text uiFishName;
    public Text uiFishPrice;
    public Text uiFishSize;
    public Text uiChampionCount;
    public Text uiOuHuangRank;
    public GameObject objBoom;
    public GameObject objBianYiTIip;
    public GameObject objChampion;
    public Text uiAddChampionCount;

    string szBindUID;


    public void InitInfo(OuHuangRankInfo ouHuangInfo, int nGetChampionCount)
    {
        if (ouHuangInfo == null) return;

        CAysncImageDownload.Ins.setAsyncImage(ouHuangInfo.szHeadIcon, uiIconPlayer);
        uiPlayerName.text = ouHuangInfo.szUserName;

        string szName = ouHuangInfo.szFishName.Replace("[±äÒì]", "");

        if (objBianYiTIip != null)
            objBianYiTIip.SetActive(ouHuangInfo.bBianYi);

        uiFishPrice.text = ((int)ouHuangInfo.nlFishPrice).ToString();
        uiFishSize.text = ouHuangInfo.fFishSize.ToString("f1") + "cm";
        if (uiIconFish != null)
        {
            uiIconFish.IconLoad(ouHuangInfo.szFishIcon);
        }
        objBoom.SetActive(ouHuangInfo.bBoom);

        //CWorldRankInfoMgr.Ins.InitRankInfo( )
        //UIRankInfo rankInfo = UIManager.Instance.GetUI(UIResType.RankList) as UIRankInfo;
        //if(rankInfo != null)
        //{
        //    rankInfo.InitRankInfo(EMRankType.WinnerOuhuang, 0);
        //    rankInfo.InitRankInfo(EMRankType.WinnerRicher, 0);
        //}

        uiOuHuangRank.text = "";
        uiChampionCount.text = "";
        szBindUID = ouHuangInfo.nlUserUID;
        
        if (objChampion != null)
        {
            objChampion.SetActive(nGetChampionCount > 0);
            uiAddChampionCount.text = "+" + nGetChampionCount;
        }

    }

    public void RefreshRankInfo()
    {
        if (CHelpTools.IsStringEmptyOrNone(szBindUID))
            return;
        CPlayerResRankInfo ouHuangRankInfo = CWorldRankInfoMgr.Ins.GetPlayerResRankSlotInWorld(szBindUID, EMRankType.WinnerOuhuang);
        if (ouHuangRankInfo != null)
        {
            uiOuHuangRank.text = ouHuangRankInfo.rank.ToString();
            uiChampionCount.text = ouHuangRankInfo.value.ToString();
        }
        else
        {
            uiOuHuangRank.text = "100+";
            CPlayerBaseInfo baseInfo = CPlayerMgr.Ins.GetPlayer(szBindUID);
            if (baseInfo != null)
            {
                uiChampionCount.text = baseInfo.nFishWinnerPoint.ToString();
            }
            else
            {
                uiChampionCount.text = "???";
            }
        }
    }

}
