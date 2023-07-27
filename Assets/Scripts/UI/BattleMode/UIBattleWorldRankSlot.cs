using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleWorldRankSlot : MonoBehaviour
{
    public GameObject objSelf;
    [Header("玩家头像")]
    public RawImage uiPlayerIcon;
    [Header("玩家名字")]
    public Text uiPlayerName;
    [Header("欧皇次数")]
    public Text uiOuHuangChance;

    public void SetInfo(CPlayerRankInfo rankInfo)
    {
        CAysncImageDownload.Ins.setAsyncImage(rankInfo.headIcon, uiPlayerIcon);
        uiPlayerName.text = rankInfo.userName;
        uiOuHuangChance.text = rankInfo.value.ToString();
    }

    public void SetActive(bool bActive)
    {
        if (objSelf == null) return;
        objSelf.SetActive(bActive);
    }

}
