using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomBossDmgRankSlot : MonoBehaviour
{
    public GameObject objSelf;
    public string nlUID;
    public Text uiPlayerName;
    public Text uiPlayerDmg;

    public void SetInfo(CGameBossDmgInfo bossDmgInfo)
    {
        if(bossDmgInfo == null)
        {
            uiPlayerName.text = "ÔÝÎÞ¼ÇÂ¼";
            uiPlayerDmg.text = "";
            return;
        }
        CPlayerBaseInfo playerBaseInfo = CPlayerMgr.Ins.GetPlayer(bossDmgInfo.nUid);
        if (playerBaseInfo == null)
            return;
        objSelf.SetActive(true);
        nlUID = playerBaseInfo.uid;
        uiPlayerName.text = playerBaseInfo.userName;
        uiPlayerDmg.text = bossDmgInfo.nDmg.ToString();
    }

    public void RefreshDmgInfo(long nlDmg)
    {
        uiPlayerDmg.text = nlDmg.ToString();
    }
}
