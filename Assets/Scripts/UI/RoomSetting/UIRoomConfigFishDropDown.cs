using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomConfigFishDropDown : MonoBehaviour
{
    public int nMapId;
    public Dropdown uiDropList;
    public int nFishID;

    public Text uiLabelName;
    public int nConfigNum;

    [ReadOnly]
    public List<int> listConfigValue = new List<int>();

    bool bInited = false;

    public void InitInfo(CFishInfo info, int defValue)
    {
        nFishID = info.nTBID;
        uiLabelName.text = info.szName;

        int nSelectIdx = 0;

        ST_GameVSMap pTBLMapInfo = CTBLHandlerGameVSMap.Ins.GetInfo(nMapId);
        if (pTBLMapInfo == null) return;

        ST_GameVSMapTreasureConfig pTreasureConfig = pTBLMapInfo.GetTreasureConfig(nFishID);
        if (pTreasureConfig == null) return;

        nConfigNum = pTreasureConfig.nMax - pTreasureConfig.nMin;
        List<string> listValues = new List<string>();
        for (int i=0; i <= nConfigNum; i++)
        {
            listConfigValue.Add(pTreasureConfig.nMin + i);
            listValues.Add(pTreasureConfig.nMin + i + "¸ö");

            if((pTreasureConfig.nMin + i) == defValue)
            {
                nSelectIdx = i;
            }
        }

        uiDropList.AddOptions(listValues);
        uiDropList.onValueChanged.AddListener(OnDropValue);
        uiDropList.value = nSelectIdx;

        CGameColorFishMgr.Ins.pGameRoomVSConfig.SetFishInfo(nMapId, nFishID, defValue);
    }

    void OnDropValue(int idx)
    {
        int nFishNum = listConfigValue[idx];

        CGameColorFishMgr.Ins.pGameRoomVSConfig.SetFishInfo(nMapId, nFishID, nFishNum);
    }
}
