using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITreasureItemSlot : MonoBehaviour
{
    public Text uiName;
    public Text uiID;
    public Text uiDesc;
    public UIRawIconLoad uiAvatarIcon;

    public Image pImgBG;
    public Image pImgTip;
    public Sprite[] pColorBG;
    public Sprite[] pColorTip;

    public Text uiLabelItemNum;

    public void SetInfo(ST_ShopTreasure shopInfo)
    {
        if (shopInfo == null) return;

        if(shopInfo.emType == ST_ShopTreasure.EMItemType.Boat)
        {
            ST_UnitFishBoat unitAvatar = CTBLHandlerUnitFishBoat.Ins.GetInfo(shopInfo.nContentID);

            pImgBG.sprite = pColorBG[(int)unitAvatar.emRare - 1];
            pImgTip.sprite = pColorTip[(int)unitAvatar.emRare - 1];
            uiName.text = unitAvatar.szName;

            uiID.text = "¥¨ID£∫" + shopInfo.nContentID;
            uiDesc.text = unitAvatar.szDesc.Replace("\\r\\n", "\r\n");
            uiAvatarIcon.SetIconSync(unitAvatar.szIcon);
        }
        else if(shopInfo.emType == ST_ShopTreasure.EMItemType.Role)
        {
            ST_UnitAvatar unitAvatar = CTBLHandlerUnitAvatar.Ins.GetInfo(shopInfo.nContentID);

            pImgBG.sprite = pColorBG[(int)unitAvatar.emRare - 1];
            pImgTip.sprite = pColorTip[(int)unitAvatar.emRare - 1];
            uiName.text = unitAvatar.szName;

            uiID.text = "»ÀID£∫" + shopInfo.nContentID;
            uiDesc.text = unitAvatar.szDesc.Replace("\\r\\n", "\r\n");
            uiAvatarIcon.SetIconSync(unitAvatar.szIcon);
        }

        uiLabelItemNum.text = shopInfo.nPrice + "";
    }
}
