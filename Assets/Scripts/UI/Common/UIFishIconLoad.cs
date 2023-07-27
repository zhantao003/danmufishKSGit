using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFishIconLoad : MonoBehaviour
{
    public Image pImg;
    public RawImage uiPlayerIcon;


    public void IconLoad(string szIcon)
    {
        //Debug.Log("Load Icon Name:" + szIcon);
        Sprite sprite = Resources.Load<Sprite>(szIcon);
        if (sprite == null)
        {
            if (pImg != null)
            {
                pImg.gameObject.SetActive(false);
            }

            if (uiPlayerIcon != null)
            {
                uiPlayerIcon.gameObject.SetActive(true);
                CAysncImageDownload.Ins.setAsyncImage(szIcon, uiPlayerIcon);
            }
        }
        else
        {
            if (uiPlayerIcon != null)
            {
                uiPlayerIcon.gameObject.SetActive(false);
            }
            if (pImg != null)
            {
                pImg.gameObject.SetActive(true);
                pImg.sprite = sprite;
            }
        }
    }


}
