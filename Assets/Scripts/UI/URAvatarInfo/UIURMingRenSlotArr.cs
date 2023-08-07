using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UIURMingRenSlotArr : MonoBehaviour
{
    public Text uiTitle;
    public UIURMingRenSlot[] pSlots;
    public GameObject objOuHuang;
    public GameObject objProfit;

    public void SetInfo(CMingRenTangInfoArr mingRenTangInfoArr,int nIdx)
    {
        if (mingRenTangInfoArr == null)
            return;
        bool bOuHuang = (nIdx == 0);
        objOuHuang.SetActive(bOuHuang);
        objProfit.SetActive(!bOuHuang);

        
        if (bOuHuang)
        {
            uiTitle.text = $"第{mingRenTangInfoArr.nSeason}赛季欧皇";// mingRenTangInfoArr.szTitle;
            for (int i = 0; i < mingRenTangInfoArr.pMingRenInfosByOuHuang.Length; i++)
            {
                if (i >= pSlots.Length)
                    break;
                pSlots[i].SetActive(true);
                pSlots[i].InitInfo(mingRenTangInfoArr.pMingRenInfosByOuHuang[i]);
            }
            for (int i = mingRenTangInfoArr.pMingRenInfosByOuHuang.Length; i < pSlots.Length; i++)
            {
                pSlots[i].SetActive(false);

            }
        }
        else
        {
            uiTitle.text = $"第{mingRenTangInfoArr.nSeason}赛季富豪";
            for (int i = 0; i < mingRenTangInfoArr.pMingRenInfosByProfit.Length; i++)
            {
                if (i >= pSlots.Length)
                    break;
                pSlots[i].SetActive(true);
                pSlots[i].InitInfo(mingRenTangInfoArr.pMingRenInfosByProfit[i]);
            }
            for (int i = mingRenTangInfoArr.pMingRenInfosByProfit.Length; i < pSlots.Length; i++)
            {
                pSlots[i].SetActive(false);

            }
        }
    }
}
