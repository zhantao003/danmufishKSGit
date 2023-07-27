using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAuctionResultOtherRoot : MonoBehaviour
{
    public GameObject objSelf;

    public UITweenScale uiTweenScale;

    [Header("展示图片")]
    public UIRawIconLoad uiShowImg;
    [Header("展示文本")]
    public Text uiShowText;

    public void SetIcon(string szIcon)
    {
        if (uiShowImg == null) return;
        uiShowImg.SetIconSync(szIcon);
    }

    public void SetIconSize(Vector2 vSize)
    {
        if (uiShowImg == null) return;
        uiShowImg.uiIcon.GetComponent<RectTransform>().sizeDelta = vSize;
    }

    public void SetIconNative()
    {
        if (uiShowImg == null) return;
        uiShowImg.uiIcon.SetNativeSize();
    }

    public void SetShowText(string szShow)
    {
        if (uiShowText == null) return;
        uiShowText.text = szShow;
    }

    public void SetActive(bool bActive)
    {
        if (objSelf == null) return;
        objSelf.SetActive(bActive);
    }

    public void PlayTween()
    {
        if (uiTweenScale == null) return;
        uiTweenScale.Play();
    }

}
