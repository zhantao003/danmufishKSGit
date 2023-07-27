using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILocalTreasureSlot : MonoBehaviour
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

    public Text uiLabelLv1;
    public Text uiLabelLv2;
    public Text uiLabelLv3;
    public Text uiLabelLv4;

    public void InitInfo(CPlayerLocalVSRankInfo localRankInfo)
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

        uiName.text = localRankInfo.userName;
        uiRank.text = (localRankInfo.rank + 1).ToString();

        CAysncImageDownload.Ins.setAsyncImage(localRankInfo.headIcon, uiPlayerIcon);

        uiLabelLv1.text = localRankInfo.countLv1.ToString();
        uiLabelLv2.text = localRankInfo.countLv2.ToString();
        uiLabelLv3.text = localRankInfo.countLv3.ToString();
        uiLabelLv4.text = localRankInfo.countLv4.ToString();
    }

    public void InitInfoByRankInfo(int rank, CRankTreasureSlot localRankInfo)
    {
        if (localRankInfo == null)
            return;

        long nRank = rank;
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

        uiName.text = localRankInfo.szUserName;
        uiRank.text = (nRank + 1).ToString();

        CAysncImageDownload.Ins.setAsyncImage(localRankInfo.szHeadIcon, uiPlayerIcon);

        //uiFishName.text = localRankInfo.szFishName;
        //uiFishIcon.IconLoad(localRankInfo.szFishIcon);
    }    
}

