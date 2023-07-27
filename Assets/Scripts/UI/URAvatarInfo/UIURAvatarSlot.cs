using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIURAvatarSlot : MonoBehaviour
{
    public GameObject objActiveRoot;
    public GameObject objUnactiveRoot;

    public Text uiName;
    public Text uiID;
    public Text uiDesc;

    public UIRawIconLoad uiAvatarIcon;

    public Image pImgBG;
    public Image pImgTip;

    public Sprite[] pColorBG;
    public Sprite[] pColorTip;
    
    public UIRawIconLoad uiUnactiveAvatarIcon;
    public Text uiUnactiveName;
    public Text uiUnactiveDesc;

    //public void SetInfo(CMingRenTangInfo cMingRenTangInfoInfo)
    //{
    //    if (cMingRenTangInfoInfo == null)
    //    {
    //        uiUnactiveName.text = "";
    //        uiUnactiveDesc.text = "";
    //        uiUnactiveAvatarIcon.SetIconSync("Icon/MinrenTang/MRTDef");

    //        objActiveRoot.SetActive(false);
    //        objUnactiveRoot.SetActive(true);
    //    }
    //    else
    //    {

    //        objActiveRoot.SetActive(true);
    //        objUnactiveRoot.SetActive(false);


    //        if (!CHelpTools.IsStringEmptyOrNone(cMingRenTangInfoInfo.szName))
    //        {
    //            uiName.text = cMingRenTangInfoInfo.szName;
    //        }
    //        else
    //        {
    //            uiName.text = "";
    //        }

    //        uiID.text = "ID:" + cMingRenTangInfoInfo.szUID;
    //        uiDesc.text = cMingRenTangInfoInfo.szDes;

    //        CAysncImageDownload.Ins.setAsyncImage(cMingRenTangInfoInfo.szHeadIcon, uiAvatarIcon.uiIcon);
    //    }
    //    int nRareIdx = 3;

    //    pImgBG.sprite = pColorBG[nRareIdx];
    //    pImgTip.sprite = pColorTip[nRareIdx];
    //}

    public void SetInfo(ST_MingrenTang avatarInfo)
    {
        if (avatarInfo.nContentID == 0)
        {
            uiUnactiveName.text = avatarInfo.szName;
            uiUnactiveDesc.text = avatarInfo.szDesc;
            Debug.Log("touxiang:" + avatarInfo.szIcon);
            if (!CHelpTools.IsStringEmptyOrNone(avatarInfo.szIcon))
            {
                uiUnactiveAvatarIcon.SetIconSync(avatarInfo.szIcon);
            }
            else
            {
                uiUnactiveAvatarIcon.SetIconSync("Icon/MinrenTang/MRTDef");
            }

            objActiveRoot.SetActive(false);
            objUnactiveRoot.SetActive(true);
        }

        objActiveRoot.SetActive(true);
        objUnactiveRoot.SetActive(false);

        if (avatarInfo.emType == ST_MingrenTang.EMMinRenType.Role)
        {
            ST_UnitAvatar unitAvatar = CTBLHandlerUnitAvatar.Ins.GetInfo(avatarInfo.nContentID);
            if (unitAvatar == null)
            {
                uiUnactiveName.text = avatarInfo.szName;
                uiUnactiveDesc.text = avatarInfo.szDesc;

                objActiveRoot.SetActive(false);
                objUnactiveRoot.SetActive(true);
            }
            else
            {
                if (!CHelpTools.IsStringEmptyOrNone(avatarInfo.szName))
                {
                    uiName.text = avatarInfo.szName;
                }
                else
                {
                    uiName.text = unitAvatar.szName;
                }

                uiID.text = "ID:" + unitAvatar.nID;
                uiDesc.text = unitAvatar.szDesc;

                uiAvatarIcon.SetIconSync(unitAvatar.szIcon);

                int nRareIdx = (int)unitAvatar.emRare - 1;

                pImgBG.sprite = pColorBG[nRareIdx];
                pImgTip.sprite = pColorTip[nRareIdx];
            }
        }
        else if (avatarInfo.emType == ST_MingrenTang.EMMinRenType.Boat)
        {
            ST_UnitFishBoat unitAvatar = CTBLHandlerUnitFishBoat.Ins.GetInfo(avatarInfo.nContentID);
            if (unitAvatar == null)
            {
                uiUnactiveName.text = avatarInfo.szName;
                uiUnactiveDesc.text = avatarInfo.szDesc;

                objActiveRoot.SetActive(false);
                objUnactiveRoot.SetActive(true);
            }
            else
            {
                if (!CHelpTools.IsStringEmptyOrNone(avatarInfo.szName))
                {
                    uiName.text = avatarInfo.szName;
                }
                else
                {
                    uiName.text = unitAvatar.szName + "(´¬)";
                }

                uiID.text = "ID:" + unitAvatar.nID;
                uiDesc.text = avatarInfo.szDesc;

                uiAvatarIcon.SetIconSync(unitAvatar.szIcon);

                int nRareIdx = (int)unitAvatar.emRare - 1;

                pImgBG.sprite = pColorBG[nRareIdx];
                pImgTip.sprite = pColorTip[nRareIdx];
            }
        }
    }
}
