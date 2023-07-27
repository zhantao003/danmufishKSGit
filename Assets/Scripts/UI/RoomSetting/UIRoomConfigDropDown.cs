using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomConfigDropDown : MonoBehaviour
{
    public Dropdown uiDropList;
    public EMRoomConfigType emConfigType;

    public string szConfinKey;
    public string szAdd;
    public int[] arrConfigValue;

    [ReadOnly]
    public List<int> listConfigValue = new List<int>();

    bool bInited = false;

    public void Init()
    {
        if (bInited)
        {
            return;
        }

        listConfigValue.Clear();
        uiDropList.ClearOptions();
        
        //初始化数据列表
        int nSelectIdx = 0;
        List<string> listValues = new List<string>();

        for (int i = 0; i < arrConfigValue.Length; i++)
        {
            bool bSelected = false;

            if (emConfigType == EMRoomConfigType.MaxVtuberCount)
            {
                int nValue = i;
                if(nValue == 0)
                {
                    listValues.Add("无限制");
                }
                else
                {
                    nValue = arrConfigValue[i];
                    listValues.Add(nValue.ToString());
                }
                bSelected = (nValue == CGameColorFishMgr.Ins.pGameRoomConfig.nMaxVtuberCount);
                listConfigValue.Add(nValue);

                if (bSelected)
                {
                    nSelectIdx = i;
                }
            }
            else if(emConfigType == EMRoomConfigType.AutoExitTime)
            {
                int nValue = arrConfigValue[i];
                listValues.Add(nValue.ToString() + szAdd);
                bSelected = (nValue * 60 == CGameColorFishMgr.Ins.pGameRoomConfig.nAutoExitTime);
                listConfigValue.Add(nValue);

                if (bSelected)
                {
                    nSelectIdx = i;
                }
            }
            else if(emConfigType == EMRoomConfigType.TreasureBomber)
            {
                int nValue = arrConfigValue[i];
                listValues.Add(nValue.ToString() + szAdd);
                bSelected = (nValue == CGameColorFishMgr.Ins.pGameRoomVSConfig.nBomberCount);
                listConfigValue.Add(nValue);

                if (bSelected)
                {
                    nSelectIdx = i;
                }
            }
            else if (emConfigType == EMRoomConfigType.VSTimeCount)
            {
                int nValue = arrConfigValue[i];
                listValues.Add(nValue.ToString() + szAdd);
                bSelected = (nValue * 60 == CGameColorFishMgr.Ins.pGameRoomVSConfig.nGameVSTime);
                listConfigValue.Add(nValue);

                if (bSelected)
                {
                    nSelectIdx = i;
                }
            }
        }

        uiDropList.AddOptions(listValues);
        uiDropList.onValueChanged.AddListener(OnDropValue);
        uiDropList.value = nSelectIdx;

        bInited = true;
    }
              
    void OnDropValue(int idx)
    {
        if (idx < 0 || idx >= listConfigValue.Count) return;

        switch (emConfigType)
        {
            case EMRoomConfigType.MaxVtuberCount:
                {
                    CGameColorFishMgr.Ins.pGameRoomConfig.nMaxVtuberCount = listConfigValue[idx];
                }
                break;
            case EMRoomConfigType.AutoExitTime:
                {
                    CGameColorFishMgr.Ins.pGameRoomConfig.nAutoExitTime = listConfigValue[idx] * 60;
                    List<CPlayerUnit> playerUnits = CPlayerMgr.Ins.GetAllIdleUnit();
                    for(int i = 0;i < playerUnits.Count;i++)
                    {
                        if (playerUnits[i] == null) return;
                        playerUnits[i].SetExitTick(CGameColorFishMgr.Ins.pGameRoomConfig.nAutoExitTime);
                    }
                }
                break;
            case EMRoomConfigType.TreasureBomber:
                {
                    CGameColorFishMgr.Ins.pGameRoomVSConfig.nBomberCount = listConfigValue[idx];
                }
                break;
            case EMRoomConfigType.VSTimeCount:
                {
                    CGameColorFishMgr.Ins.pGameRoomVSConfig.nGameVSTime = listConfigValue[idx] * 60;
                }
                break;
        }
    }
}
