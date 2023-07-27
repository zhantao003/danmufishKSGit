using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFishInfo : MonoBehaviour
{
    public CFishInfo pTargetInfo;

    public GameObject objSelf;
    public Text uiName;
    public UIFishIconLoad iconLoad;
    public Text uiSize;

    public GameObject objAddRoot;
    public Text uiAddSize;

    public Image pRareBG;
    public Sprite[] pRareBGSprite;


    public void Init(CFishInfo fishInfo)
    {
        pTargetInfo = fishInfo;

        //Debug.Log(fishInfo.szIcon + "=====" + fishInfo.szName + "=====" + fishInfo.szDes);
        if (uiName != null)
        {
            string szName = fishInfo.szName;
            if (fishInfo.emRare == EMRare.YouXiu)
            {
                szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishR"), "000000");
            }
            else if (fishInfo.emRare == EMRare.XiYou)
            {
                szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishSR"), "000000");
            }
            else if (fishInfo.emRare == EMRare.Special)
            {
                szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishSSR"), "000000");
            }
            else if (fishInfo.emRare == EMRare.Shisi)
            {
                szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishUR"), "000000");
            }
            //else if (fishInfo.emRare == EMRare.Normal)
            //{
            //    szName = "<color=#" + CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishUR") + ">" + szName + "</color>";
            //}
            uiName.text = szName;
        }

        if(iconLoad != null)
        {
            //Debug.Log("ÓãÓãID£º" + fishInfo.nTBID);
            iconLoad.IconLoad(fishInfo.szIcon);
        }

        

        if (uiSize != null)
        {
            if(fishInfo.emItemType == EMItemType.RandomEvent)
            {
                uiSize.fontSize = 30;
                uiSize.text = fishInfo.szDes;
            }
            else if(fishInfo.emItemType == EMItemType.FishMat)
            {
                uiSize.fontSize = 42;
                uiSize.text = "1.00cm";
            }
            else
            {
                uiSize.fontSize = 42;
                uiSize.text = fishInfo.fCurSize.ToString("f2") + "cm";
            }
        }

        
        if (fishInfo.fAddSize > 0 &&
            fishInfo.fAddSize > 0 &&
            fishInfo.emItemType == EMItemType.Fish)
        {
            if (objAddRoot != null)
                objAddRoot.SetActive(true);
            uiAddSize.text = "+" + fishInfo.fAddSize.ToString("f2") + "cm";
            uiSize.text = (fishInfo.fCurSize - fishInfo.fAddSize).ToString("f2") + "cm";
        }
        else
        {
            if (objAddRoot != null)
                objAddRoot.SetActive(false);
        }

        if (pRareBG != null)
        {
            pRareBG.sprite = pRareBGSprite[(int)fishInfo.emRare];
        }
    }

    public void SetActive(bool bActive)
    {
        if (objSelf == null) return;

        objSelf.SetActive(bActive);
    }

}
