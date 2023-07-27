using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UIURMingRenSlot : MonoBehaviour
{
    public GameObject objSelf;
    public RawImage uiPlayerIcon;
    public GameObject objPlayerIcon;
    public Text uiPlayerName;

    public void InitInfo(CMingRenTangInfo cMingRenTangInfoInfo)
    {
        if (CHelpTools.IsStringEmptyOrNone(cMingRenTangInfoInfo.szHeadIcon))
        {
            objPlayerIcon.SetActive(false);
        }
        else
        {
            objPlayerIcon.SetActive(true);
            CAysncImageDownload.Ins.setAsyncImage(cMingRenTangInfoInfo.szHeadIcon, uiPlayerIcon);
        }
        uiPlayerName.text = cMingRenTangInfoInfo.szName;
    }

    public void SetActive(bool bActive)
    {
        if (objSelf == null) return;
        objSelf.SetActive(bActive);
    }

}
