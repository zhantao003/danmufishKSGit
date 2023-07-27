using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBattleShow : MonoBehaviour
{
    public GameObject objSelf;

    public UIBattlePlayerInfo[] playerInfos;

    public void SetActive(bool bActive)
    {
        objSelf.SetActive(bActive);
    }

    /// <summary>
    /// Ë¢ÐÂ
    /// </summary>
    public void RefreshPlayerInfo(List<SpecialCastInfo> listShowInfos)
    {
        if (!objSelf.activeSelf) return;
        for (int i = 0; i < listShowInfos.Count; i++)
        {
            if (i >= playerInfos.Length) break;
            playerInfos[i].SetActive(true);
            playerInfos[i].Init(listShowInfos[i],false);
        }
        for (int i = listShowInfos.Count; i < playerInfos.Length; i++)
        {
            playerInfos[i].SetActive(false);
        }
    }

}
