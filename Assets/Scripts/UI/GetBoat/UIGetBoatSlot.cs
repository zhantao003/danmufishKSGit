using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGetBoatSlot : MonoBehaviour
{
    public GameObject objActiveRoot;
    public GameObject objUnactiveRoot;
    public GameObject objHaveRoot;
    public GameObject objSeasonRoot;

    public Text uiName;
    public Text uiID;
    public Text uiDesc;

    public UIRawIconLoad uiAvatarIcon;

    public Image pImgBG;
    public Image pImgTip;

    public Sprite[] pColorBG;
    public Sprite[] pColorTip;

    public Text uiUnactiveName;
    public Text uiUnactiveDesc;

    public GameObject objBoardItem;
    public UIRawIconLoad uiIconItem;
    public Text uiLabelItemName;
    public Text uiLabelItemNum;

    public int nLimitYear;
    public int nLimitMonth;
    public int nLimitDay;


    public void SetInfo(ST_UnitFishBoat unitAvatar)
    {
        if (unitAvatar.nID == 0)
        {
            return;
        }
        else
        {
            objActiveRoot.SetActive(true);
            objUnactiveRoot.SetActive(false);
            objHaveRoot.SetActive(false);
            objSeasonRoot.SetActive(false);
        }

        if (unitAvatar != null)
        {
            pImgBG.sprite = pColorBG[(int)unitAvatar.emRare - 1];
            pImgTip.sprite = pColorTip[(int)unitAvatar.emRare - 1];
            uiName.text = unitAvatar.szName;

            uiID.text = "ID:" + unitAvatar.nID;
            //Debug.Log(unitAvatar.szDesc);
            uiDesc.text = unitAvatar.szDesc.Replace("\\r\\n", "\r\n");

            uiAvatarIcon.SetIconSync(unitAvatar.szIcon);

            ST_FishMat pTBLMatInfo = CTBLHandlerFishMaterial.Ins.GetInfo(unitAvatar.nItemId);
            if(pTBLMatInfo == null)
            {
                objBoardItem.SetActive(false);
            }
            else
            {
                objBoardItem.SetActive(true);
                uiIconItem.SetIconSync(pTBLMatInfo.szIcon);
                uiLabelItemName.text = pTBLMatInfo.szName;

                //≤ƒ¡œ∂“ªª¥Ú’€
                long nItemNum = unitAvatar.nItemNum;

                if (DateTime.Now.Year == nLimitYear &&
                    DateTime.Now.Month == nLimitMonth &&
                    DateTime.Now.Day == nLimitDay)
                {
                    nItemNum = System.Convert.ToInt64(nItemNum * CGameColorFishMgr.Ins.pStaticConfig.GetInt("≤ƒ¡œ∂“ªª¥Ú’€") * 0.01f);
                }

                uiLabelItemNum.text = nItemNum + "∏ˆ";
            }
            objHaveRoot.SetActive(false);
            objSeasonRoot.SetActive(unitAvatar.bIsSeason);

            //objHaveRoot.SetActive(CPlayerMgr.Ins.pOwner.pBoatPack.GetInfo(unitAvatar.nID) != null);
        }
        else
        {
            objBoardItem.SetActive(false);
        }
    }
}
