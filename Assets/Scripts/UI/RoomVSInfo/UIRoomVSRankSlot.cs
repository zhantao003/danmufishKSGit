using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomVSRankSlot : MonoBehaviour
{
    public Text uiLabelName;
    public RawImage uiIconPlayer;

    public Text uiLabelCountLv1;
    public Text uiLabelCountLv2;
    public Text uiLabelCountLv3;
    public Text uiLabelCountLv4;

    public void SetInfo(CRankTreasureSlot info)
    {
        uiLabelName.text = info.szUserName;
        CAysncImageDownload.Ins.setAsyncImage(info.szHeadIcon, uiIconPlayer);

        uiLabelCountLv1.text = info.nLv1_count.ToString();
        uiLabelCountLv2.text = info.nLv2_count.ToString();
        uiLabelCountLv3.text = info.nLv3_count.ToString();
        uiLabelCountLv4.text = info.nLv4_count.ToString();
    }
}
