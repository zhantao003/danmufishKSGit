using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpecialInfo
{
    public CSceneFactory.EMSceneType emSceneType;

    public int[] fishID;
}

public class UITodaySpecialty : MonoBehaviour
{
    public GameObject objSelf;

    public UIFishInfo[] fishInfos;

    public SpecialInfo[] specialInfos;

    public void Init()
    {
        for (int i = 0; i < specialInfos.Length; i++)
        {
            if (specialInfos[i].emSceneType == CSceneMgr.Instance.m_objCurScene.emSceneType)
            {
                for (int j = 0; j < specialInfos[i].fishID.Length; j++)
                {
                    if (j >= fishInfos.Length) continue;
                    ST_FishInfo fishInfo = CGameColorFishMgr.Ins.pMap.pTBLHandlerFishInfo.GetInfo(specialInfos[i].fishID[j]);
                    if (fishInfo == null)
                    {
                        fishInfo = CGameColorFishMgr.Ins.pMap.pTBLHandlerBoomFishInfo.GetInfo(specialInfos[i].fishID[j]);
                        if (fishInfo == null)
                        {
                            continue;
                        }
                    }
                    CFishInfo info = new CFishInfo(fishInfo);
                    fishInfos[j].SetActive(true);
                    fishInfos[j].Init(info);
                }
                for (int j = specialInfos[i].fishID.Length; j < fishInfos.Length; j++)
                {
                    fishInfos[j].SetActive(false);
                }
                break;
            }
        }
    }

    public void SetActive(bool bActive)
    {
        if (objSelf == null) return;
        objSelf.SetActive(bActive);
    }

}
