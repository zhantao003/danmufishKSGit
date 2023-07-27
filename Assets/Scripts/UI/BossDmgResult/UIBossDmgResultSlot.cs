using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBossDmgResultSlot : MonoBehaviour
{
    public GameObject objRankIdx;
    public GameObject objRankImg;
    public RawImage uiPlayerIcon;
    public Text uiPlayerName;
    public Text uiPlayerDmg;
    public Text uiRank;
    public Text uiReward;

    public Image uiRankImg;
    public Image uiBG;
    public Sprite[] pRankImgs;
    public Sprite[] arrBGImgs;

    public UIRawIconLoad iconLoad;

    public GameObject objShowIcon;
    public GameObject objShowText;
    public GameObject objReward;

    public GameObject objCamption;

    public Text uiShowText;

    public GameObject objDropAvatarRoot;
    public UIRawIconLoad uiDropAvatarIcon;

    public void SetInfo(CGameBossRewardInfo bossRewardInfo,int rank, bool dropAble, CGameMapDropAvatarSlot dropInfo,bool bVictory)
    {
        CPlayerBaseInfo playerBaseInfo = CPlayerMgr.Ins.GetPlayer(bossRewardInfo.nUid);
        
        if (playerBaseInfo == null) return;

        long nRank = rank;// localRankInfo.rank;
        bool bShowRankImg = (nRank <= 2 && nRank >= 0);
        objRankIdx.SetActive(!bShowRankImg);
        objRankImg.SetActive(bShowRankImg);
        if (bShowRankImg)
        {
            uiRankImg.sprite = pRankImgs[nRank];
        }

        if (objCamption != null)
        {
            if (nRank == 0 && bVictory)
            {
                objCamption.SetActive(true);
            }
            else
            {
                objCamption.SetActive(false);
            }
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
        uiRank.text = (nRank + 1).ToString();

        if (uiPlayerIcon != null)
        {
            CAysncImageDownload.Ins.setAsyncImage(playerBaseInfo.userFace, uiPlayerIcon);
        }

        if (uiPlayerName != null)
        {
            uiPlayerName.text = playerBaseInfo.userName;
        }

        if(uiPlayerDmg != null)
        {
            uiPlayerDmg.text = "造成伤害" + ((float)bossRewardInfo.nShowDmgRate * 0.01f).ToString("f2") + "%";
        }

        if (bossRewardInfo.nItemId > 0)
        {
            objReward.SetActive(true);
            if (uiReward != null)
            {
                ST_FishMat fishMat = CTBLHandlerFishMaterial.Ins.GetInfo((int)bossRewardInfo.nItemId);
                if (fishMat != null)
                {
                    iconLoad.SetIconSync(fishMat.szIcon);
                    uiReward.text = "*" + bossRewardInfo.nAddNum; //fishMat.szName +
                }
                else
                {
                    objReward.SetActive(false);
                }
            }
        }
        else
        {
            objReward.SetActive(false);
        }


        //判断是否有凋落物
        if(dropAble && dropInfo!=null)
        {
            objDropAvatarRoot.SetActive(true);
            if (objShowIcon != null)
                objShowIcon.SetActive(true);
            if (objShowText != null)
                objShowText.SetActive(false);
            if (dropInfo.emDropType == EMDropType.Role)
            {
                ST_UnitAvatar pTBLAvatar = CTBLHandlerUnitAvatar.Ins.GetInfo(dropInfo.nId);
                if (pTBLAvatar != null)
                {
                    uiDropAvatarIcon.SetIconSync(pTBLAvatar.szIcon);
                }
            }
            else if(dropInfo.emDropType == EMDropType.Boat)
            {
                ST_UnitFishBoat pTBLBoat = CTBLHandlerUnitFishBoat.Ins.GetInfo(dropInfo.nId);
                if(pTBLBoat != null)
                {
                    uiDropAvatarIcon.SetIconSync(pTBLBoat.szIcon);
                } 
            }
        }
        else if(bossRewardInfo.dropAvatarInfo != null)
        {
            objDropAvatarRoot.SetActive(true);
            if (objShowIcon != null)
                objShowIcon.SetActive(true);
            if (objShowText != null)
                objShowText.SetActive(false);
            if (bossRewardInfo.dropAvatarInfo.emDropType == EMDropType.Role)
            {
                ST_UnitAvatar pTBLAvatar = CTBLHandlerUnitAvatar.Ins.GetInfo(bossRewardInfo.dropAvatarInfo.nId);
                if (pTBLAvatar != null)
                {
                    uiDropAvatarIcon.SetIconSync(pTBLAvatar.szIcon);
                }
            }
            else if (bossRewardInfo.dropAvatarInfo.emDropType == EMDropType.Boat)
            {
                ST_UnitFishBoat pTBLBoat = CTBLHandlerUnitFishBoat.Ins.GetInfo(bossRewardInfo.dropAvatarInfo.nId);
                if (pTBLBoat != null)
                {
                    uiDropAvatarIcon.SetIconSync(pTBLBoat.szIcon);
                }
            }
        }
        else
        {
            if (bVictory &&
                bossRewardInfo.nlGetFishCoin > 0)
            {
                objDropAvatarRoot.SetActive(true);
                if (objShowIcon != null)
                    objShowIcon.SetActive(false);
                if (objShowText != null)
                    objShowText.SetActive(true);
                if (uiShowText != null)
                    uiShowText.text = (bossRewardInfo.nlGetFishCoin * 0.0001f).ToString() + "万\n鱼币"; 
            }
            else
            {
                if (objShowIcon != null)
                    objShowIcon.SetActive(true);
                if (objShowText != null)
                    objShowText.SetActive(false);
                objDropAvatarRoot.SetActive(false);
            }
        }
    }
}
