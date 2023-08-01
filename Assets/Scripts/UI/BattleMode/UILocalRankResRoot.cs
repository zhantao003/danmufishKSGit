using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILocalRankResRoot : MonoBehaviour
{
    public UILocalRankResSlot[] arrRankSlots;

    

    public GameObject objSelf;

    public void SetAcitve(bool bActive)
    {
        if (objSelf == null) return;
        objSelf.SetActive(bActive);
    }

    public void InitInfo(CGetChampionRule curChampionRule)
    {
        List<OuHuangRankInfo> listShowInfos = COuHuangRankMgr.Ins.GetRankInfos();

        for(int i=0; i< arrRankSlots.Length; i++)
        {
            int index = i;// i + 1;
            int nGetChampionCount = 0;
            if (curChampionRule != null &&
                i < curChampionRule.nGetChampion.Length)
            {
                nGetChampionCount = curChampionRule.nGetChampion[i];
            }
            if (index >= listShowInfos.Count)
            {
                arrRankSlots[i].objRoot.SetActive(false);
            }
            else
            {
                arrRankSlots[i].objRoot.SetActive(true);
                arrRankSlots[i].InitInfo(listShowInfos[index], nGetChampionCount);
            }
        }
    }

    public void RefreshRankInfo()
    {
        for (int i = 0; i < arrRankSlots.Length; i++)
        {
            arrRankSlots[i].RefreshRankInfo();
        }
    }

}
