using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILocalRankRoot : MonoBehaviour
{
    public UILocalRankResRoot localRankResRoot;
    public UILocalProfitResRoot localProfitResRoot;

    public Text uiRankTitle;

    public Text uiBtnText;

    public GameObject[] objTips;
    public int nCurShowIdx;

    public void InitInfo(CGetChampionRule curChampionRule)
    {
        localRankResRoot.InitInfo(curChampionRule);
        localProfitResRoot.InitInfo(curChampionRule);
        ShowInfo();
    }

    public void RefreshRankInfo()
    {
        localRankResRoot.RefreshRankInfo();
        localProfitResRoot.RefreshRankInfo();
    }

    public void ShowInfo()
    {
        for (int i = 0; i < objTips.Length; i++)
        {
            objTips[i].SetActive(i == nCurShowIdx);
        }
        localRankResRoot.SetAcitve(nCurShowIdx == 0);
        localProfitResRoot.SetAcitve(nCurShowIdx == 1);
        if (nCurShowIdx == 0)
        {
            uiRankTitle.text = "本场欧皇榜";
            uiBtnText.text = "收益榜";
        }
        else
        {
            uiRankTitle.text = "本场收益榜";
            uiBtnText.text = "欧皇榜";
        }
        nCurShowIdx++;
        if (nCurShowIdx >= objTips.Length)
        {
            nCurShowIdx = 0;
        }

    }

}
