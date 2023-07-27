using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CBomberCountConfigSlot
{
    public int nCount;
    public int nWeight;
}

[System.Serializable]
public class CBomberCountConfig
{
    public int nFishId;
    public bool bBianyi = false;
    public int nRandCount;
    public Vector2Int vRandomRange; //�������
    public string szName;

    public CBomberCountConfigSlot[] arrInfoRandPool;

    public int RandInfoCount()
    {
        //�����һ�����õ�����
        int nTotalWeight = 0;
        for (int slotIdx = 0; slotIdx < arrInfoRandPool.Length; slotIdx++)
        {
            nTotalWeight += arrInfoRandPool[slotIdx].nWeight;
        }

        int nCountRandWeight = Random.Range(1, nTotalWeight+1);
        int nInfoCount = 0;
        for (int slotIdx = 0; slotIdx < arrInfoRandPool.Length; slotIdx++)
        {
            if (nCountRandWeight <= arrInfoRandPool[slotIdx].nWeight)
            {
                nInfoCount = arrInfoRandPool[slotIdx].nCount;
                break;
            }
            else
            {
                nCountRandWeight -= arrInfoRandPool[slotIdx].nWeight;
            }
        }

        return nInfoCount;
    }
}

[System.Serializable]
public class CBomberCountInfo
{
    public int nIdx;
    public int nFishId;
    public string szName;
    public bool bBianyi = false;
}

public class CBomberCountMgr : MonoBehaviour
{
    static CBomberCountMgr ins = null;

    public static CBomberCountMgr Ins
    {
        get
        {
            return ins;
        }
    }

    //�����������
    public CBomberCountConfig[] arrConfigs;

    public int nRandomPool = 5000;  //���������

    [ReadOnly]
    public int nCurCount = 0;   //��ǰ����

    public Dictionary<int, CBomberCountInfo> dicInfos = new Dictionary<int, CBomberCountInfo>();

    private void Start()
    {
        ins = this;

        RandInfo();
        LogRandPool();
    }

    /// <summary>
    /// ��ʼ���漴��
    /// </summary>
    public void RandInfo()
    {
        nCurCount = 0;
        dicInfos.Clear();

        for(int i=0; i<arrConfigs.Length; i++)
        {
            int nAverageLerp = nRandomPool / arrConfigs[i].nRandCount;
            for (int idx = 0; idx < arrConfigs[i].nRandCount; idx++)
            {
                int nInfoNum = arrConfigs[i].RandInfoCount();

                for(int infoIdx =0; infoIdx< nInfoNum; infoIdx++)
                {
                    int nMinAdd = (int)(nAverageLerp * arrConfigs[i].vRandomRange.x * 0.01F);
                    int nMaxAdd = nAverageLerp - (int)(nAverageLerp * arrConfigs[i].vRandomRange.y * 0.01F);

                    int nFinalMin = 1 + idx * nAverageLerp + nMinAdd;
                    int nFinalMax = 1 + (idx + 1) * nAverageLerp - nMaxAdd;
                    //Debug.Log(arrConfigs[i].szName + $"   ��{idx}����� ���䣺" + nFinalMin + "  " + nFinalMax);
                    int nTargetIdx = Random.Range(nFinalMin, nFinalMax);
                    CBomberCountInfo pInfo = new CBomberCountInfo();
                    pInfo.nFishId = arrConfigs[i].nFishId;
                    pInfo.bBianyi = arrConfigs[i].bBianyi;
                    pInfo.szName = arrConfigs[i].szName;

                    AddInfo(nTargetIdx, pInfo);
                }
            }
        }
    }

    /// <summary>
    /// �����Ϣ
    /// </summary>
    void AddInfo(int idx, CBomberCountInfo info)
    {
        while(dicInfos.ContainsKey(idx))
        {
            if(idx >= nRandomPool)
            {
                idx = 0;
            }
            else
            {
                idx++;
            }
        }

        info.nIdx = idx;
        dicInfos.Add(idx, info);
    }

    ///����
    public static CBomberCountInfo Count(int count)
    {
        if (Ins == null) return null;

        CBomberCountInfo pRes = null;
        Ins.nCurCount++;

        if(Ins.dicInfos.TryGetValue(Ins.nCurCount, out pRes))
        {
            
        }

        //���ó���
        if(Ins.nCurCount >= Ins.nRandomPool)
        {
            Ins.RandInfo();
            Ins.LogRandPool();
        }

        return pRes;
    }

    public void LogRandPool()
    {
        string szContent = "";
        List<CBomberCountInfo> listInfos = new List<CBomberCountInfo>();
        listInfos.AddRange(dicInfos.Values);
        listInfos.Sort((x, y) =>
        {
            if(x.nIdx < y.nIdx)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        });
        foreach(CBomberCountInfo info in listInfos)
        {
            szContent += $"��<color=#FFD000>{info.nIdx}</color>�γ�    <color=#F02000>{info.szName}</color>\r\n";
        }

        Debug.Log(szContent);
    }

    [ContextMenu("Rand Info")]
    public void TestRandInfo()
    {
        RandInfo();
        LogRandPool();
    }

    [ContextMenu("Test Count")]
    public void TestCount()
    {
        for(int i=0; i<nRandomPool; i++)
        {
            CBomberCountInfo pRes = CBomberCountMgr.Count(1);

            if (pRes != null)
            {
                Debug.Log($"��{pRes.nIdx}��  ������<color=#F02000>{pRes.szName}</color>");
            }
            else
            {
                Debug.Log("û�г�������");
            }
        } 
    }
}
