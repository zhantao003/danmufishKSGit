using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILocalRankSlot : MonoBehaviour
{
    public GameObject objRankIdx;
    public GameObject objRankImg;
    public Image uiRankImg;
    public Image uiBG;
    public Sprite[] pRankImgs;
    public Sprite[] arrBGImgs;

    public RawImage uiPlayerIcon;

    public Text uiName;
    public Text uiRank;

    public Text uiCoinValue;
    public Text uiFishName;
    public Text uiFishSize;
    public Text uiFishPrice;
    public UIFishIconLoad uiFishIcon;

    public GameObject[] arrRankTypeTip;

    //string szNameContent;

    public void InitInfo(CPlayerLocalRankInfo localRankInfo,EMLocalRankType rankType)
    {
        if (localRankInfo == null)
            return;

        long nRank = localRankInfo.rank;
        bool bShowRankImg = (nRank <= 2 && nRank >= 0);
        objRankIdx.SetActive(!bShowRankImg);
        objRankImg.SetActive(bShowRankImg);
        if (bShowRankImg)
        {
            uiRankImg.sprite = pRankImgs[nRank];
        }

        if (nRank <= 2)
        {
            long nTargetBGIdx = nRank;
            if (nTargetBGIdx < 0)
            {
                uiBG.sprite = arrBGImgs[arrBGImgs.Length - 1];
            }
            else
            {
                uiBG.sprite = arrBGImgs[nTargetBGIdx];
            }
        }
        else
        {
            uiBG.sprite = arrBGImgs[arrBGImgs.Length - 1];
        }

        //szNameContent = localRankInfo.userName;
        //ShowNameLabel(false);
        uiName.text = localRankInfo.userName;
        uiRank.text = (localRankInfo.rank + 1).ToString();

        CAysncImageDownload.Ins.setAsyncImage(localRankInfo.headIcon, uiPlayerIcon);

        for (int i = 0; i < arrRankTypeTip.Length; i++)
        {
            arrRankTypeTip[i].SetActive(i == (int)rankType);
        }
        switch(rankType)
        {
            case EMLocalRankType.MapOuHuang:
                {
                    uiFishName.text = localRankInfo.szFishName;
                    uiFishSize.text = localRankInfo.fFishSize.ToString("f2") + "cm";
                    uiFishPrice.text = localRankInfo.value.ToString("f0");
                    uiFishIcon.IconLoad(localRankInfo.szFishIcon);
                }
                break;
            case EMLocalRankType.MapCain:
                {
                    uiCoinValue.text = localRankInfo.value.ToString("f0");
                }
                break;
        }

    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.F2))
    //    {
    //        ShowNameLabel(true);
    //    }

    //    if (Input.GetKeyUp(KeyCode.F2))
    //    {
    //        ShowNameLabel(false);
    //    }
    //}

    //void ShowNameLabel(bool show)
    //{
    //    if(show)
    //    {
    //        uiName.text = szNameContent;
    //    }
    //    else
    //    {
    //        uiName.text = "";
    //    }
    //}
}
