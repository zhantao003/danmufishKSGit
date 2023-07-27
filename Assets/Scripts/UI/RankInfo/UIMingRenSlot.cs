using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMingRenSlot : MonoBehaviour
{
    public CMingRenTangInfo pOuHuangInfo;
    public CMingRenTangInfo pProfitInfo;

    public GameObject objIcon;
    public RawImage uiPlayrIcon;
    public Text uiPlayerName;
    public Text uiType;

    public void InitInfo(EMRankType emRankType)
    {
        CMingRenTangInfo setInfo = null;
        if(emRankType == EMRankType.WinnerOuhuang)
        {
            setInfo = pOuHuangInfo;
            uiType.text = "…œºæ≈∑ª ";
        }
        else
        {
            setInfo = pProfitInfo;
            uiType.text = "…œºæ∏ª∫¿";
        }
        if (uiPlayrIcon != null)
        {
            if (CHelpTools.IsStringEmptyOrNone(setInfo.szHeadIcon))
            {
                objIcon.SetActive(false);
            }
            else
            {
                objIcon.SetActive(true);
                CAysncImageDownload.Ins.downloadImageAction(setInfo.szHeadIcon, uiPlayrIcon);
            }
        }
        if(uiPlayerName != null)
        {
            uiPlayerName.text = setInfo.szName;
        }
    }
}
