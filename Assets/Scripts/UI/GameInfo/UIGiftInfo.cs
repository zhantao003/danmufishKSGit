using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGiftInfo : MonoBehaviour
{
    public ST_GiftMenuByBoat pTargetInfo;

    public GameObject objSelf;
    public Text uiName;
    public UIFishIconLoad iconLoad;
    public Text uiSize;

    public Image uiRareBG;
    public Sprite[] uiRareBGSprites;


    public void Init(ST_GiftMenuByBoat giftMenuByBoat)
    {
        pTargetInfo = giftMenuByBoat;

        string szName = string.Empty;
        string szIcon = string.Empty;
        switch (giftMenuByBoat.emGiftMenuType)
        {
            case EMGiftMenuType.FishCoin:
                {
                    szName = "积分";
                    szIcon = "Icon/Gift/FishCoin";
                }
                break;
            case EMGiftMenuType.FeiLun:
                {
                    szName = "初级卷轴";
                    szIcon = "Icon/Gift/FeiLun";
                }
                break;
            case EMGiftMenuType.FishPack:
                {
                    szName = "互动手柄";
                    szIcon = "Icon/Gift/FishPack";
                }
                break;
            case EMGiftMenuType.HaiDaoCoin:
                {
                    szName = "海盗金币";
                    szIcon = "Icon/Gift/Gold";
                }
                break;
        }
        if (uiName != null)
        {
            uiName.text = szName;
        }
        if (iconLoad != null)
        {
            iconLoad.IconLoad(szIcon);
        }
        if (uiSize != null)
        {
            uiSize.fontSize = 30;
            uiSize.text = "x" + pTargetInfo.nCount;
        }

        uiRareBG.sprite = uiRareBGSprites[(int)giftMenuByBoat.emGiftMenuRare];
    }

    public void SetActive(bool bActive)
    {
        if (objSelf == null) return;

        objSelf.SetActive(bActive);
    }

}
