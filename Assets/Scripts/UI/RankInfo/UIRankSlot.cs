using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRankSlot : MonoBehaviour
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

    public Text uiFishValue;
    public Text uiMapLvValue;
    public Text uiCoinValue;

    public Text uiVtbName;
    public Text uiVtbSize;
    public RawImage uiVtbIcon;

    public Text uiLabelOuhuang;
    public Text uiLabelFishCoin;

    public GameObject[] arrRankTypeTip;
    
    public void SetInfo(CPlayerRankInfo info, EMRankType rankType)
    {
        long nRank = info.rank;
        bool bShowRankImg = (nRank <= 3 && nRank >= 1);
        objRankIdx.SetActive(!bShowRankImg);
        objRankImg.SetActive(bShowRankImg);
        if (bShowRankImg)
        {
            uiRankImg.sprite = pRankImgs[nRank - 1];
        }

        if(nRank <= 3)
        {
            long nTargetBGIdx = nRank - 1;
            if(nTargetBGIdx < 0)
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

        uiName.text = info.userName;
        //uiGold.text = info.value.ToString();
        if(nRank <= 0)
        {
            uiRank.text = "Î´ÉÏ°ñ";
        }
        else
        {
            uiRank.text = info.rank.ToString();
        }

        CAysncImageDownload.Ins.setAsyncImage(info.headIcon, uiPlayerIcon);

        for(int i=0; i<arrRankTypeTip.Length; i++)
        {
            arrRankTypeTip[i].SetActive(i == (int)rankType);
        }

        switch(rankType)
        {
            case EMRankType.RareFish:
                {
                    if(info.value < 0)
                    {
                        uiFishValue.text = "";
                    }
                    else
                    {
                        uiFishValue.text = info.value.ToString() + "Ìõ";
                    }
                }
                break;
            case EMRankType.MapLv:
                {
                    uiMapLvValue.text = info.value.ToString() + "¼¶";
                }
                break;
            case EMRankType.MapCain:
                {
                    if(info.value < 0)
                    {
                        uiCoinValue.text = string.Format("{0:N0}", CRoomRecordInfoMgr.Ins.RoomGainCoin);
                    }
                    else
                    {
                        uiCoinValue.text = string.Format("{0:N0}", info.value);
                    } 
                }
                break;
            case EMRankType.FishVtb:
                {
                    bool show = (info.vtbUid > 0);
                    arrRankTypeTip[(int)rankType].SetActive(show);

                    if(show)
                    {
                        uiVtbName.text = info.vtbName;
                        uiVtbSize.text = string.Format("{0:N2}", (float)info.value * 0.01F) + "cm";
                        CAysncImageDownload.Ins.setAsyncImage(info.vtbIcon, uiVtbIcon);
                    }
                }
                break;
            case EMRankType.WinnerOuhuang:
                {
                    uiLabelOuhuang.text = info.value.ToString();
                }
                break;
            case EMRankType.WinnerRicher:
                {
                    uiLabelFishCoin.text = CHelpTools.GetGoldSZ(info.value);
                }
                break;
        }
    }
}
