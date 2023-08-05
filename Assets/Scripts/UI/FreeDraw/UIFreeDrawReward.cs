using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFreeDrawReward : MonoBehaviour
{
    public GameObject objSelf;
    public Text uiRewardName;
    public UIRawIconLoad uiRewardImg;

    public void SetInfo(CDrawRewardInfo rewardInfo)
    {
        string szName = string.Empty;
        string szIcon = string.Empty;
        
        switch (rewardInfo.emDrawRewardType)
        {
            case CGiftGachaBoxInfo.EMGiftType.Yuju:
                {
                    szName = "互动手柄*" + rewardInfo.nlRewardCount;
                    szIcon = "Icon/Gift/FishPack";
                }
                break;
            case CGiftGachaBoxInfo.EMGiftType.Feilun:
                {
                    szName = "初级卷轴*" + rewardInfo.nlRewardCount;
                    szIcon = "Icon/Gift/FeiLun";
                }
                break;
            case CGiftGachaBoxInfo.EMGiftType.FishCoin:
                {
                    szName = "积分" + rewardInfo.nlRewardCount;
                    szIcon = "Icon/Gift/FishCoin";
                }
                break;
            case CGiftGachaBoxInfo.EMGiftType.Role:
                {
                    ST_UnitAvatar unitAvatar = CTBLHandlerUnitAvatar.Ins.GetInfo(rewardInfo.nRewardID);
                    if(unitAvatar != null)
                    {
                        szName = unitAvatar.szName + rewardInfo.nRewardID;
                        szIcon = unitAvatar.szIcon;
                    }
                }
                break;
            case CGiftGachaBoxInfo.EMGiftType.Boat:
                {
                    ST_UnitFishBoat unitFishBoat = CTBLHandlerUnitFishBoat.Ins.GetInfo(rewardInfo.nRewardID);
                    if(unitFishBoat != null)
                    {
                        szName = unitFishBoat.szName + rewardInfo.nRewardID;
                        szIcon = unitFishBoat.szIcon;
                    }
                }
                break;
        }


        if (uiRewardImg != null)
        {
            uiRewardImg.SetIconSync(szIcon);

            uiRewardImg.uiIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(100f, 100f);
        }
        if(uiRewardName != null)
        {
            uiRewardName.text = szName;
        }
    }


    public void SetActive(bool bActive)
    {
        if (objSelf == null) return;

        objSelf.SetActive(bActive);
    }

}
