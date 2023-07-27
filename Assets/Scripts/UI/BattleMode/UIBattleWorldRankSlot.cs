using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleWorldRankSlot : MonoBehaviour
{
    public GameObject objSelf;
    [Header("���ͷ��")]
    public RawImage uiPlayerIcon;
    [Header("�������")]
    public Text uiPlayerName;
    [Header("ŷ�ʴ���")]
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
