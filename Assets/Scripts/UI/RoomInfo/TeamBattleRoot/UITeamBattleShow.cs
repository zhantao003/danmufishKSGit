using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITeamBattleShow : MonoBehaviour
{
    public GameObject objSelf;

    public UIBattlePlayerInfo[] teamAPlayerInfos;
    public UIBattlePlayerInfo[] teamBPlayerInfos;

    public void SetActive(bool bActive)
    {
        objSelf.SetActive(bActive);
    }

    /// <summary>
    /// Ë¢ÐÂ
    /// </summary>
    public void RefreshPlayerInfo(List<SpecialCastInfo> listShowInfos,EMTeamBattleCamp battleCamp)
    {
        if (!objSelf.activeSelf) return;

        UIBattlePlayerInfo[] battlePlayerInfos = null;
        if(battleCamp == EMTeamBattleCamp.Red)
        {
            battlePlayerInfos = teamAPlayerInfos;
        }
        else if(battleCamp == EMTeamBattleCamp.Blue)
        {
            battlePlayerInfos = teamBPlayerInfos;
        }

        for (int i = 0; i < listShowInfos.Count; i++)
        {
            if (i >= battlePlayerInfos.Length) break;
            battlePlayerInfos[i].SetActive(true);
            battlePlayerInfos[i].Init(listShowInfos[i], false);
        }
        for (int i = listShowInfos.Count; i < battlePlayerInfos.Length; i++)
        {
            battlePlayerInfos[i].SetActive(false);
        }
    }

}
