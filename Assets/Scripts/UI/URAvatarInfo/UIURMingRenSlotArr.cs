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

        objOuHuang.SetActive(nIdx == 0);
        objProfit.SetActive(nIdx == 1);

        uiTitle.text = mingRenTangInfoArr.szTitle;
        for(int i = 0;i < mingRenTangInfoArr.pMingRenInfos.Length;i++)
        {
            if (i >= pSlots.Length)
                break;
            pSlots[i].SetActive(true);
            pSlots[i].InitInfo(mingRenTangInfoArr.pMingRenInfos[i]);
        }
        for (int i = mingRenTangInfoArr.pMingRenInfos.Length; i < pSlots.Length; i++)
        {
            pSlots[i].SetActive(false);

        }

    }
}
