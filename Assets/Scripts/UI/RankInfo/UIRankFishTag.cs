using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRankFishTag : MonoBehaviour
{
    [ReadOnly]
    public int nFishID;

    public UIRawIconLoad uiIconLoad;
    public Text uiLabelName;
    public Button uiBtnTag;

    public void SetInfo(ST_FishInfo info)
    {
        if (info == null) return;

        nFishID = info.nID;
        if(!CHelpTools.IsStringEmptyOrNone(info.szIcon))
        {
            uiIconLoad.gameObject.SetActive(true);
            uiIconLoad.SetIconSync(info.szIcon);
        }
        else
        {
            uiIconLoad.gameObject.SetActive(false);
        }
        uiLabelName.text = info.szName;
    }

    public void SetSelect(bool active)
    {
        uiBtnTag.interactable = !active;
    }

    public void OnClickSlot()
    {
        UIRankInfo uiRank = UIManager.Instance.GetUI(UIResType.RankList) as UIRankInfo;
        if (uiRank == null) return;

        uiRank.OnClickFishInfo(nFishID);
    }
}
