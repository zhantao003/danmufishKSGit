using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShowText : MonoBehaviour
{
    public RectTransform rectSelf;
    public GameObject objSelf;
    public Text uiShowText;

    public void Init(SpecialCastInfo specialCastInfo)
    {
        if (!CHelpTools.IsStringEmptyOrNone(specialCastInfo.szShowText))
        {
            uiShowText.text = specialCastInfo.szShowText;
        }
        else
        {
            CFishInfo fishInfo = specialCastInfo.fishInfo;
            if (uiShowText != null)
            {
                ///判断该物品是否存在尺寸
                if (fishInfo.emItemType == EMItemType.Fish)
                {
                    uiShowText.text = "<color=#24CE38>[渔讯]</color>" +
                                      specialCastInfo.playerInfo.userName + "摸到了" +
                                      fishInfo.fCurSize.ToString("f2") + "cm的" +
                                      fishInfo.szName + ",获得了" +
                                      ((int)fishInfo.lPrice).ToString() + "积分";
                }
                else if(fishInfo.emItemType == EMItemType.FishMat)
                {
                    uiShowText.text = "<color=#24CE38>[渔讯]</color>" +
                                      specialCastInfo.playerInfo.userName + "摸到了" +
                                      fishInfo.szName;// fishInfo.fCurSize.ToString("f0") + "个" + fishInfo.szName;
                }
                else if(fishInfo.emItemType == EMItemType.RandomEvent)
                {
                    uiShowText.text = "<color=#FFEE2B>[幸运]</color>" +
                                   specialCastInfo.playerInfo.userName + "摸到了" +
                                   fishInfo.szName + "," + fishInfo.szDes;
                }
                else if(fishInfo.emItemType == EMItemType.BattleItem)
                {
                    uiShowText.text = "<color=#24CE38>[渔讯]</color>" +
                                      specialCastInfo.playerInfo.userName + "摸到了" +
                                      fishInfo.fCurSize.ToString("f2") + "cm的" +
                                      fishInfo.szName +",伤害值" + ((int)fishInfo.lPrice).ToString();
                }
                else
                {
                    uiShowText.text = "<color=#24CE38>[渔讯]</color>" +
                                     specialCastInfo.playerInfo.userName + "摸到了" +
                                     fishInfo.szName + ",获得了" +
                                     ((int)fishInfo.lPrice).ToString() + "积分";
                }
            }
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectSelf);
    }

    public void SetActive(bool bActive)
    {
        objSelf.SetActive(bActive);
    }

}
