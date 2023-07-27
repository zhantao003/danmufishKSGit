using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITreasureSlot : MonoBehaviour
{
    public UIRawIconLoad pImg;
    public Image pImgBG;
    public Image pImgTip;

    public Text uiName;
    public Text uiID;

    public Sprite[] pColorBG;
    public Sprite[] pColorTip;

    public void SetInfo(CGiftGachaBoxInfo treasureInfo)
    {
        int nRareIdx = 0;
        int nID = 0;
        string szName = string.Empty;
        string szImg = string.Empty;
        if (treasureInfo.emType == CGiftGachaBoxInfo.EMGiftType.Role)     //½ÇÉ«
        {
            ST_UnitAvatar unitAvatar = CTBLHandlerUnitAvatar.Ins.GetInfo(treasureInfo.nItemID);
            if (unitAvatar != null)
            {
                szName = unitAvatar.szName;
                nID = unitAvatar.nID;
                nRareIdx = ((int)unitAvatar.emRare - 1);
                szImg = unitAvatar.szIcon;
            }
        }
        else if (treasureInfo.emType == CGiftGachaBoxInfo.EMGiftType.Boat)       //´¬
        {
            ST_UnitFishBoat unitFishBoat = CTBLHandlerUnitFishBoat.Ins.GetInfo(treasureInfo.nItemID);
            if(unitFishBoat != null)
            {
                szName = unitFishBoat.szName;
                nID = unitFishBoat.nID;
                nRareIdx = (int)(unitFishBoat.emRare) - 1;
                szImg = unitFishBoat.szIcon;
            }
        }
        else if(treasureInfo.emType == CGiftGachaBoxInfo.EMGiftType.FishGan)    //Óã¸Í
        {
            ST_UnitFishGan unitFishGan = CTBLHandlerUnitFishGan.Ins.GetInfo(treasureInfo.nItemID);
            if (unitFishGan != null)
            {
                szName = unitFishGan.szName;
                nID = unitFishGan.nID;
                nRareIdx = (int)(unitFishGan.emRare) - 1;
                szImg = unitFishGan.szIcon;
            }
        }

        if (uiName != null)
        {
            uiName.text = szName;
        }
        if (uiID != null)
        {
            uiID.text = "ID:" + nID;
        }
        if (pImgBG != null)
        {
            if (nRareIdx >= 0 && nRareIdx < pColorBG.Length)
                pImgBG.sprite = pColorBG[nRareIdx];
        }
        if (pImgTip != null)
        {
            if(nRareIdx >= 0 && nRareIdx < pColorTip.Length)
                pImgTip.sprite = pColorTip[nRareIdx];
        }
        if (pImg != null)
        {
            pImg.SetIconSync(szImg);
        }
    }

}
