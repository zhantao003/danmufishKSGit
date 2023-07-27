using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAuctionWaitPlayer : MonoBehaviour
{
    public GameObject objSelf;
    public RawImage uiPlayerIcon;

    public void SetActive(bool bActive)
    {
        if (objSelf == null) return;
        objSelf.SetActive(bActive);
    }

    public void SetIcon(string szIcon)
    {
        if (uiPlayerIcon == null) return;
        CAysncImageDownload.Ins.setAsyncImage(szIcon, uiPlayerIcon);
    }


}
