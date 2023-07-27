using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomTreasurePointSlot : MonoBehaviour
{
    public UIRawIconLoad uiIcon;
    public Text uiLabelPoint;

    public void SetInfo(ST_FishInfo fishInfo)
    {
        uiIcon.SetIconSync(fishInfo.szIcon);
        uiLabelPoint.text = $"=<color=#ffff00>{fishInfo.nTreasurePoint}</color>º£µÁ½ð±Ò";
    }
}
