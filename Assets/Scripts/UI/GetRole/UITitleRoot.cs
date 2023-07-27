using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITitleRoot : MonoBehaviour
{
    public GameObject objSelf;

    public Image uiImgTip;

    public Sprite[] pColorTip;

    public void SetInfo(ST_UnitAvatar.EMRare emRare)
    {
        uiImgTip.sprite = pColorTip[(int)emRare - 1];
    }

    public void SetActive(bool bActive)
    {
        objSelf.SetActive(bActive);
    }

}
