using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAuctionShowInfo : MonoBehaviour
{
    public GameObject objSelf;
    public Text uiName;
    public Text uiPrice;

    public void SetInfo(CFishInfo fishInfo)
    {
        if (fishInfo == null) return;
        if(uiName != null)
        {
            uiName.text = fishInfo.szName;
        }
        if(uiPrice != null)
        {
            uiPrice.text = fishInfo.lPrice.ToString();
        }
    }

    public void SetActive(bool bActive)
    {
        if (objSelf == null) return;
        objSelf.SetActive(bActive);
    }

}
