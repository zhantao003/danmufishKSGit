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
                ///�жϸ���Ʒ�Ƿ���ڳߴ�
                if (fishInfo.emItemType == EMItemType.Fish)
                {
                    uiShowText.text = "<color=#24CE38>[��Ѷ]</color>" +
                                      specialCastInfo.playerInfo.userName + "������" +
                                      fishInfo.fCurSize.ToString("f2") + "cm��" +
                                      fishInfo.szName + ",�����" +
                                      ((int)fishInfo.lPrice).ToString() + "����";
                }
                else if(fishInfo.emItemType == EMItemType.FishMat)
                {
                    uiShowText.text = "<color=#24CE38>[��Ѷ]</color>" +
                                      specialCastInfo.playerInfo.userName + "������" +
                                      fishInfo.szName;// fishInfo.fCurSize.ToString("f0") + "��" + fishInfo.szName;
                }
                else if(fishInfo.emItemType == EMItemType.RandomEvent)
                {
                    uiShowText.text = "<color=#FFEE2B>[����]</color>" +
                                   specialCastInfo.playerInfo.userName + "������" +
                                   fishInfo.szName + "," + fishInfo.szDes;
                }
                else if(fishInfo.emItemType == EMItemType.BattleItem)
                {
                    uiShowText.text = "<color=#24CE38>[��Ѷ]</color>" +
                                      specialCastInfo.playerInfo.userName + "������" +
                                      fishInfo.fCurSize.ToString("f2") + "cm��" +
                                      fishInfo.szName +",�˺�ֵ" + ((int)fishInfo.lPrice).ToString();
                }
                else
                {
                    uiShowText.text = "<color=#24CE38>[��Ѷ]</color>" +
                                     specialCastInfo.playerInfo.userName + "������" +
                                     fishInfo.szName + ",�����" +
                                     ((int)fishInfo.lPrice).ToString() + "����";
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
