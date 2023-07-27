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

        Girl1,  //���
        Girl2,  //¹¹��
        Girl3,  //������
        Girl4,  //����
        Girl5,  //���ɽ�

        Max,
    }

    public enum EMBroadType
    {
        UltraFish = 0,  //��ϡ����
        PlayerFish,     //�������
        Suipian100,     //100��Ƭ
        PlayerFishBY,   //����������
        FishBY,         //ϡ������
        Duck,           //����ϡ����Ʒ
        GachaSSR,       //����SSR

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
