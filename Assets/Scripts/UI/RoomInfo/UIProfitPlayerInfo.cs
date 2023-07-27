using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProfitPlayerInfo : MonoBehaviour
{
    public GameObject objSelf;
    [Header("�������")]
    public Text uiName;
    [Header("�ܼ�")]
    public Text uiPrice;

    string szNameContent;

    public void Init(ProfitRankInfo recordInfo)
    {
        if (recordInfo == null)
        {
            uiName.text = "���޸���";
            if (uiPrice != null)
                uiPrice.text = "";
            return;
        }
        szNameContent = recordInfo.szUserName;

        //if (recordInfo == null)
        //{
        //    if (uiName != null)
        //        uiName.text = "?";
        //    if (uiPrice != null)
        //        uiPrice.text = "?";
        //    return;
        //}

        ShowNameLabel(CGameColorFishMgr.Ins.bShowPlayerInfo);

        if (uiPrice != null)
            uiPrice.text = (recordInfo.nlTotalCoin).ToString();
    }


    public void SetActive(bool bActive)
    {
        objSelf.SetActive(bActive);
    }

    public void ShowNameLabel(bool show)
    {
        if (uiName == null) return;

        if (show)
        {
            uiName.text = szNameContent;
        }
        else
        {
            uiName.text = "";
        }
    }
}