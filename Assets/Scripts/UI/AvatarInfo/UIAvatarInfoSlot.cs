using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAvatarInfoSlot : MonoBehaviour
{
    public Text uiLabelName;
    public Text uiLabelName2;
    public Text uiLabelID;
    public Text uiLabelCount;
    public Text uiLabelTip;
    public UIRawIconLoad uiAvatarIcon;

    public Image pImgBG;
    public Image pImgTip;

    public Sprite[] pColorBG;
    public Sprite[] pColorTip;
    public GameObject[] arrObjTips;

    public GameObject objIconBoard;
    public GameObject objYubiBoard;
    public GameObject objFishPackBoard;
    public GameObject objFishLunBoard;

    public void SetInfo(CFishFesInfoSlot info, UIAvatarInfo.EMType emType)
    {
        int giftType = info.nType;
        int giftID = info.nID;
        if (giftType == 0)   //��
        {
            ST_UnitFishBoat pTBLBoatInfo = CTBLHandlerUnitFishBoat.Ins.GetInfo(giftID);
            if (pTBLBoatInfo == null)
            {
                objIconBoard.SetActive(false);
                objYubiBoard.SetActive(false);
                objFishPackBoard.SetActive(false);
                objFishLunBoard.SetActive(false);
                uiLabelName.text = "";
                uiLabelName2.text = "";
            }
            else
            {
                objIconBoard.SetActive(true);
                objYubiBoard.SetActive(false);
                objFishPackBoard.SetActive(false);
                objFishLunBoard.SetActive(false);

                uiLabelName.text = pTBLBoatInfo.szName;
                uiLabelID.text = "";
                uiAvatarIcon.SetIconSync(pTBLBoatInfo.szIcon);
                uiLabelName2.text = "";

                int nRareIdx = (int)pTBLBoatInfo.emRare - 1;
                pImgBG.sprite = pColorBG[nRareIdx];
                pImgTip.sprite = pColorTip[nRareIdx];

                uiLabelTip.text = "���Ի�ô�ֻ";
            }
        }
        else if (giftType == 1)  //��ɫ
        {
            ST_UnitAvatar pTBLAvatarInfo = CTBLHandlerUnitAvatar.Ins.GetInfo(giftID);
            if (pTBLAvatarInfo == null)
            {
                objIconBoard.SetActive(false);
                objYubiBoard.SetActive(false);
                objFishPackBoard.SetActive(false);
                objFishLunBoard.SetActive(false);
                uiLabelName.text = "";
                uiLabelName2.text = "";
            }
            else
            {
                objIconBoard.SetActive(true);
                objYubiBoard.SetActive(false);
                objFishPackBoard.SetActive(false);
                objFishLunBoard.SetActive(false);

                uiLabelName.text = pTBLAvatarInfo.szName;
                uiLabelID.text = "";
                uiAvatarIcon.SetIconSync(pTBLAvatarInfo.szIcon);
                uiLabelName2.text = "";

                int nRareIdx = (int)pTBLAvatarInfo.emRare - 1;
                pImgBG.sprite = pColorBG[nRareIdx];
                pImgTip.sprite = pColorTip[nRareIdx];

                uiLabelTip.text = "���Ի������";
            }
        }
        else if (giftType == 2)  //���
        {
            objIconBoard.SetActive(false);
            objYubiBoard.SetActive(true);
            objFishPackBoard.SetActive(false);
            objFishLunBoard.SetActive(false);

            uiLabelName2.text = CHelpTools.GetGoldSZ(giftID);
            uiLabelTip.text = "���Ի��";
        }
        else if (giftType == 3)  //�����װ
        {
            objIconBoard.SetActive(false);
            objYubiBoard.SetActive(false);
            objFishPackBoard.SetActive(true);
            objFishLunBoard.SetActive(false);

            uiLabelName2.text = giftID.ToString() + "��";
            uiLabelTip.text = "���Ի��";
        }
        else if (giftType == 4) //����
        {
            objIconBoard.SetActive(false);
            objYubiBoard.SetActive(false);
            objFishPackBoard.SetActive(false);
            objFishLunBoard.SetActive(true);

            uiLabelName2.text = giftID.ToString() + "��";
            uiLabelTip.text = "���Ի��";
        }

        if(emType == UIAvatarInfo.EMType.OuHuang)
        {
            uiLabelCount.text = info.nPointPlayer.ToString();
        }
        else
        {
            uiLabelCount.text = CHelpTools.GetGoldSZ(info.nPointPlayer);
        }
        
        for (int i=0; i<arrObjTips.Length; i++)
        {
            arrObjTips[i].SetActive((int)emType == i);
        }
    }
}
