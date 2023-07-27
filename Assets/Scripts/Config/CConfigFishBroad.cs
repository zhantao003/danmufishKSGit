using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[UnityEngine.CreateAssetMenu(fileName = "AudioBroad", menuName = "GameConfig/AudioBroad")]
public class CConfigFishBroad : SerializedScriptableObject
{
    public enum EMBroadModel
    {
        None = 0,

        Girl1,  //南瞑酱
        Girl2,  //鹿鹿酱
        Girl3,  //咕咕咩
        Girl4,  //污喵
        Girl5,  //豆椛姐

        Max,
    }

    public enum EMBroadType
    {
        UltraFish = 0,  //最稀有鱼
        PlayerFish,     //钓起玩家
        Suipian100,     //100碎片
        PlayerFishBY,   //钓起变异玩家
        FishBY,         //稀变异鱼
        Duck,           //钓起稀有物品
        GachaSSR,       //出货SSR

        Max,
    }

    public List<string> listBroadModelName = new List<string>();

    public Dictionary<EMBroadModel, CConfigFishBroadInfo> dicInfos = new Dictionary<EMBroadModel, CConfigFishBroadInfo>();

    public CAudioMgr.CAudioSourcePlayer Play(EMBroadModel modelId, EMBroadType emType)
    {
        CConfigFishBroadInfo pInfo = null;
        if(dicInfos.TryGetValue(modelId, out pInfo))
        {
            return pInfo.Play(emType);
        }

        return null;
    }

    public CAudioMgr.CAudioSlottInfo GetRandSlot(EMBroadModel modelId, EMBroadType emType)
    {
        CConfigFishBroadInfo pInfo = null;
        if (dicInfos.TryGetValue(modelId, out pInfo))
        {
            List<CAudioMgr.CAudioSlottInfo> listRes = null;
            if (pInfo.dicConfigs.TryGetValue(emType, out listRes))
            {
                if (listRes.Count <= 0) return null;

                int nRandIdx = Random.Range(0, listRes.Count);
                return listRes[nRandIdx];
            }
        }

        return null;
    }
}

public class CConfigFishBroadInfo
{
    public Dictionary<CConfigFishBroad.EMBroadType, List<CAudioMgr.CAudioSlottInfo>> dicConfigs = new Dictionary<CConfigFishBroad.EMBroadType, List<CAudioMgr.CAudioSlottInfo>>();

    public CAudioMgr.CAudioSourcePlayer Play(CConfigFishBroad.EMBroadType emType)
    {
        List<CAudioMgr.CAudioSlottInfo> listRes = null;
        if (dicConfigs.TryGetValue(emType, out listRes))
        {
            if (listRes.Count <= 0) return null;

            int nRandIdx = Random.Range(0, listRes.Count);
            return CAudioMgr.Ins.PlaySoundBySlot(listRes[nRandIdx], Vector3.zero);
        }

        return null;
    }
}

public class CConfigFishBroadSlot
{
    public CAudioMgr.CAudioSlottInfo[] arrAudios;

    public void Play()
    {
        if (arrAudios == null || arrAudios.Length <= 0) return;

    }
}
