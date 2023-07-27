using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
//public class CGameVSGiftConfigSlot
//{
//    public int nGiftID; //��ƷID

//    [System.Serializable]
//    public class CGameVSGiftCurSlot
//    {
//        public float fCurRange;
//        public float fCreateTotalNum;
//    }

//    public CGameVSGiftCurSlot[] arrCutSlot; //����-�������� �ֲ�����

//    //�����������(�ٷֱ�)
//    public bool bRareStartLock = false;
//    public float fRareStartPos = 0.4F;

//    //���һ�������������(�ٷֱ�)
//    public bool bRareLastLock = false;
//    public float fRareLastPos = 0.8F;

//    //��Ʒ��������ֵķ�Χ(�ٷֱ�)
//    public bool bConnectUnable  = true;   //�Ƿ�������������
//    public Vector2Int vGiftCreateRange;

//    public string szLogColor;
//}

//[System.Serializable]
//public class CGameVSGiftInfo
//{
//    public int nIdx;    //���ִ���
//    public int nFishId; //��ƷID
//    public bool bBianyi = false;
//    public string szLogColor = "";
//}

///// <summary>
///// ��Ʒ�����Ϣ
///// </summary>
//public class CGameVSGiftRandInfo
//{
//    public int nId;
//    public int nNum;
//    public int nBaseNum; //��������

//    public int nCurCutIdx = 0; //��ǰ���и�����
//    public List<int> listCutNum = new List<int>();

//    public void InitCut(CGameVSGiftConfigSlot config)
//    {
//        listCutNum.Clear();
//        int nAddNum = 0;

//        if (config.arrCutSlot!=null &&
//            config.arrCutSlot.Length > 0)
//        {
            
//            for(int i=0; i<config.arrCutSlot.Length; i++)
//            {
//                int nCurCutNum = System.Convert.ToInt32(nBaseNum * config.arrCutSlot[i].fCreateTotalNum);
//                nAddNum += nCurCutNum;

//                listCutNum.Add(nCurCutNum); 
//            }
//        }

//        if(nBaseNum - nAddNum > 0)
//        {
//            listCutNum.Add(nBaseNum - nAddNum);
//        }

//        nCurCutIdx = 0;
//        nNum = listCutNum[0];
//    }

//    public void NextCut()
//    {
//        nCurCutIdx++;
//        if(nCurCutIdx< listCutNum.Count)
//        {
//            int nOldNum = nNum;
//            nNum = listCutNum[nCurCutIdx] + nOldNum;
//            Debug.Log($"����{nId}��Cut��ʣ��{nNum}��");

//        }
//    }

//    public EMRare emRare;
//}

[System.Serializable]
public class CGameVSGiftCountBaseConfig
{
    public int nTbid;
    public Vector2Int vCountRange;

    public int GetCount()
    {
        return Random.Range(vCountRange.x, vCountRange.y);
    }
}

public class CGameVSGiftCountSlot
{
    public int nTbid;
    public int nCount;
    public Vector2Int vCountRange;

    public bool Count(int num)
    {
        nCount -= num;

        if(nCount <=0)
        {
            nCount = Random.Range(vCountRange.x, vCountRange.y);
            return true;
        }

        return false;
    }

    public void Reset()
    {
        nCount = Random.Range(vCountRange.x, vCountRange.y);
    }
}

//��ҵļ�����
public class CGameVSGiftCountPlayerInfo
{
    public long uid;
    public CGameVSGiftCountSlot[] arrSlots;

    public CGameVSGiftCountSlot GetInfo(int tbid)
    {
        if (arrSlots == null) return null;
        for(int i=0; i<arrSlots.Length; i++)
        {
            if(arrSlots[i].nTbid == tbid)
            {
                return arrSlots[i];
            }
        }

        return null;
    }
}

public class CGameVSGiftPoolMgr : MonoBehaviour
{
    static CGameVSGiftPoolMgr ins = null;

    public static CGameVSGiftPoolMgr Ins
    {
        get
        {
            if(ins == null)
            {
                ins = FindObjectOfType<CGameVSGiftPoolMgr>();
                //ins.InitRandRange();
                //ins.InitGiftPool();
            }

            return ins;
        }
    }

    public enum EMState
    {
        Gameing,
        End,
    }

    [ReadOnly]
    public EMState emCurState = EMState.Gameing;

    public CGameVSGiftCountBaseConfig[] arrConfigs;

    Dictionary<string, CGameVSGiftCountPlayerInfo> dicPlayerCounts = new Dictionary<string, CGameVSGiftCountPlayerInfo>();

    //�����������
    //public CGameVSGiftConfigSlot[] arrRandRangeConfigs;

    //[ReadOnly]
    //public int nFinalRandRange; //�����������
    //[ReadOnly]
    //public int nAverageRange;  //�������

    //[ReadOnly]
    //public int nCurCount;   //��ǰ����

    ////��Ϸ����ʱ
    //[ReadOnly]
    //public CPropertyTimer pGameTimeTicker = new CPropertyTimer();

    //[HideInInspector]
    //public List<CGameVSGiftRandInfo> listAllGiftBaseInfos = new List<CGameVSGiftRandInfo>();    //��Ʒ������Ϣ
    //[HideInInspector]
    //public List<CGameVSGiftInfo> listGiftPools = new List<CGameVSGiftInfo>();  //����

    ////��һ������Ľ�Ʒ
    //int nPreGiftId;

    ///// <summary>
    ///// ��õ���ί��
    ///// </summary>
    //public DelegateIFuncCall dlgCountGift;

    ///// <summary>
    ///// ��Ϸ����
    ///// </summary>
    //public DelegateNFuncCall dlgPoolEnd;

    public bool bDevTest;

    ///// <summary>
    ///// ��ʼ���������������
    ///// </summary>
    //void InitRandRange()
    //{
    //    int nMapID = CGameColorFishMgr.Ins.pMapVSConfig.nID;

    //    int nMapFishNum = 0;
    //    CGameColorFishVSRoomConfigInfoSlot pMapConfigSlot = CGameColorFishMgr.Ins.pGameRoomVSConfig.GetFishInfo(nMapID);
    //    if(pMapConfigSlot!=null)
    //    {
    //        nMapFishNum = CGameColorFishMgr.Ins.pGameRoomVSConfig.GetFishInfo(nMapID).GetAllFishNum();
    //    }

    //    nFinalRandRange = CGameColorFishMgr.Ins.pGameRoomVSConfig.nBomberCount;

    //    nAverageRange = nFinalRandRange / nMapFishNum;

    //    Debug.Log("��Ʒ������" + nMapFishNum + "---�����������" + nFinalRandRange + "----���ص����䣺" + nAverageRange);
    //}

    ///// <summary>
    ///// �������
    ///// </summary>
    //public void InitGiftPool()
    //{
    //    nCurCount = 0;
    //    nPreGiftId = 0;

    //    pGameTimeTicker.Value = CGameColorFishMgr.Ins.pGameRoomVSConfig.nGameVSTime;
    //    pGameTimeTicker.FillTime();

    //    listGiftPools.Clear();
    //    listAllGiftBaseInfos.Clear();

    //    int nMapID = CGameColorFishMgr.Ins.pMapVSConfig.nID;
    //    CGameColorFishVSRoomConfigInfoSlot pMapConfigSlot = CGameColorFishMgr.Ins.pGameRoomVSConfig.GetFishInfo(nMapID);
    //    if (pMapConfigSlot == null) return;

    //    CTBLLoader loader = new CTBLLoader();
    //    loader.LoadFromFile($"TBL/{"MainFish"}");
    //    CTBLHandlerFishInfo pTBLHandlerFishInfo = new CTBLHandlerFishInfo();
    //    pTBLHandlerFishInfo.LoadInfo(loader);

    //    //��Ʒ�����
    //    int nGiftTotalNum = 0;
    //    List<CGameVSGiftRandInfo> listGiftInfos = new List<CGameVSGiftRandInfo>();
    //    foreach (int key in pMapConfigSlot.dicFishInfos.Keys)
    //    {
    //        CGameVSGiftRandInfo pGiftInfo = new CGameVSGiftRandInfo();
    //        pGiftInfo.nId = key;
    //        pGiftInfo.nNum = pMapConfigSlot.dicFishInfos[key];
    //        pGiftInfo.nBaseNum = pMapConfigSlot.dicFishInfos[key];
    //        pGiftInfo.nCurCutIdx = 0;
    //        pGiftInfo.InitCut(GetGiftConfigSlot(key));

    //        CGameVSGiftRandInfo pBaseGiftInfo = new CGameVSGiftRandInfo();
    //        pBaseGiftInfo.nId = key;
    //        pBaseGiftInfo.nNum = pMapConfigSlot.dicFishInfos[key];
    //        pBaseGiftInfo.nBaseNum = pMapConfigSlot.dicFishInfos[key];
    //        pGiftInfo.nCurCutIdx = 0;

    //        //ͳ������
    //        nGiftTotalNum += pMapConfigSlot.dicFishInfos[key];

    //        ST_FishInfo pFishInfo = pTBLHandlerFishInfo.GetInfo(key);
    //        if (pFishInfo != null)
    //        {
    //            pGiftInfo.emRare = pFishInfo.emRare;
    //        }

    //        listGiftInfos.Add(pGiftInfo);
    //        listAllGiftBaseInfos.Add(pBaseGiftInfo);
    //    }

    //    //�Ȱѽ������������
    //    string szLogRes = $"�ܴ�����{nFinalRandRange}   ������Ϣ��\r\n";
    //    List<CGameVSGiftRandInfo> listTmpRandGiftPool;
    //    for (int i=0; i<nGiftTotalNum; i++)
    //    {
    //        listTmpRandGiftPool = GetTmpRandGiftPool(i + 1, nGiftTotalNum, ref listGiftInfos);
    //        if(listTmpRandGiftPool.Count > 0)
    //        {
    //            //���
    //            int nTmpAllWeight = 0;
    //            for(int r = 0; r<listTmpRandGiftPool.Count; r++)
    //            {
    //                nTmpAllWeight += listTmpRandGiftPool[r].nNum;
    //            }

    //            int nTargetGiftId = 0;
    //            int nTmpRandWeight = Random.Range(1, nTmpAllWeight + 1);
    //            for (int r = 0; r < listTmpRandGiftPool.Count; r++)
    //            {
    //                if(nTmpRandWeight <= listTmpRandGiftPool[r].nNum)
    //                {
    //                    nTargetGiftId = listTmpRandGiftPool[r].nId;
    //                    break;
    //                }
    //                else
    //                {
    //                    nTmpRandWeight -= listTmpRandGiftPool[r].nNum;
    //                }
    //            }

    //            //������ֵĴ���
    //            CGameVSGiftConfigSlot pConfigSlot = GetGiftConfigSlot(nTargetGiftId);

    //            int nCreatePos = 0;
    //            int nMinAdd = (int)(nAverageRange * pConfigSlot.vGiftCreateRange.x * 0.01F);
    //            int nMaxAdd = nAverageRange - (int)(nAverageRange * pConfigSlot.vGiftCreateRange.y * 0.01F);
    //            int nFinalRangeMin = 0;
    //            int nFinalRangeMax = 0;

    //            //Debug.Log("��ʼ��ֵ��" + nMinAdd + "   ������ֵ��" + nMaxAdd);

    //            if (i < nGiftTotalNum - 1)
    //            {
    //                nFinalRangeMin = 1 + i * nAverageRange + nMinAdd;
    //                nFinalRangeMax = (i + 1) * nAverageRange - nMaxAdd;
    //            }
    //            else
    //            {
    //                nFinalRangeMin = 1 + i * nAverageRange + nMinAdd;
    //                nFinalRangeMax = nFinalRandRange + 1;
    //            }

    //            nCreatePos = Random.Range(nFinalRangeMin, nFinalRangeMax);

    //            //���뽱��
    //            listGiftPools.Add(new CGameVSGiftInfo() { 
    //                nIdx = nCreatePos,
    //                nFishId = nTargetGiftId,
    //                szLogColor = pConfigSlot.szLogColor,
    //            });

    //            nPreGiftId = nTargetGiftId;

    //            //����
    //            for (int j = 0; j < listGiftInfos.Count; j++)
    //            {
    //                if (listGiftInfos[j].nId == nTargetGiftId)
    //                {
    //                    listGiftInfos[j].nNum--;
    //                    break;
    //                }
    //            }

    //            //��־����
    //            string szColor = pConfigSlot.szLogColor; //(pFishInfo.emRare >= EMRare.Shisi) ? "ff50ff" : "ffffff";

    //            if(i<nGiftTotalNum - 1)
    //            {
    //                szLogRes += $"����[{1 + i * nAverageRange}-{(i + 1) * nAverageRange}]   ��<color=#ffff00>{nCreatePos}</color>��  ���ֽ�Ʒ{i + 1}---" + $"<color=#{szColor}>{listGiftPools[i].nFishId}</color>" + "\r\n";
    //            }
    //            else
    //            {
    //                szLogRes += $"����[{1 + i * nAverageRange}-{nFinalRandRange}]   ��<color=#ffff00>{nCreatePos}</color>��  ���ֽ�Ʒ{i + 1}---" + $"<color=#{szColor}>{listGiftPools[i].nFishId}</color>" + "\r\n";
    //            }
    //        }
    //    }

    //    //������Ϸ״̬
    //    emCurState = EMState.Gameing;

    //    Debug.Log(szLogRes);
    //}

    //List<CGameVSGiftRandInfo> GetTmpRandGiftPool(int idx, int max, ref List<CGameVSGiftRandInfo> listInfos)
    //{
    //    List<CGameVSGiftRandInfo> listRes = new List<CGameVSGiftRandInfo>();

    //    for(int i=0; i< listInfos.Count; i++)
    //    {
    //        bool bAdd = false;
    //        CGameVSGiftConfigSlot config = GetGiftConfigSlot(listInfos[i].nId);

    //        //�ж��Ƿ����cut
    //        if (config.arrCutSlot != null &&
    //            config.arrCutSlot.Length > 0 &&
    //            listInfos[i].nCurCutIdx < config.arrCutSlot.Length)
    //        { 
    //            float fRate = (float)idx / (float)max;
    //            if (fRate > config.arrCutSlot[listInfos[i].nCurCutIdx].fCurRange)
    //            {
    //                Debug.Log($"��{idx}�� ����{listInfos[i].nId}��Cut   ���������{fRate}:{config.arrCutSlot[listInfos[i].nCurCutIdx].fCurRange}");

    //                listInfos[i].NextCut();
    //            }
    //        }

    //        if (listInfos[i].nNum > 0)
    //        {
    //            //�ж���û��cut
    //            if(config.arrCutSlot != null &&
    //               config.arrCutSlot.Length > 0 &&
    //               listInfos[i].nCurCutIdx < config.arrCutSlot.Length)
    //            {
    //                float fCurRate = (float)idx / (float)max;
    //                if(fCurRate <= config.arrCutSlot[listInfos[i].nCurCutIdx].fCurRange)
    //                {
    //                    //�ж�ʣ����Ӷ�λ�������
    //                    int nBoard = System.Convert.ToInt32(max * config.arrCutSlot[listInfos[i].nCurCutIdx].fCurRange);
    //                    if (idx - 1 + listInfos[i].nNum == nBoard)
    //                    {
    //                        Debug.Log($"��{idx}�� ���ʣ��{listInfos[i].nNum}��  ���ޣ�{nBoard}");
    //                        listRes.Clear();
    //                        listRes.Add(listInfos[i]);
    //                        return listRes;
    //                    }
    //                }
    //            }

    //            //���һ�� ֱ�Ӹ���
    //            if(idx == max)
    //            {
    //                bAdd = true;
    //            }
    //            else
    //            {
    //                //����Ƿ�����������
    //                if (listInfos[i].nNum == 1 &&
    //                    config.bRareLastLock &&
    //                    listInfos[i].nBaseNum > 1)
    //                {
    //                    int nTmpCheck = System.Convert.ToInt32(max * config.fRareLastPos);
    //                    if (idx >= nTmpCheck)
    //                    {
    //                        bAdd = true;
    //                    }
    //                }
    //                else
    //                {
    //                    //�ж��Ƿ��г�ʼλ������
    //                    if(config.bRareStartLock)
    //                    {
    //                        int nTmpCheck = System.Convert.ToInt32(max * config.fRareStartPos);
    //                        if (idx >= nTmpCheck)
    //                        {
    //                            //�ж��Ƿ�����������
    //                            if(config.bConnectUnable)
    //                            {
    //                                if(nPreGiftId != listInfos[i].nId)
    //                                {
    //                                    bAdd = true;
    //                                }
    //                            }
    //                            else
    //                            {
    //                                bAdd = true;
    //                            }
    //                        }
    //                    }
    //                    else
    //                    {
    //                        //�ж��Ƿ�����������
    //                        if (config.bConnectUnable)
    //                        {
    //                            if (nPreGiftId != listInfos[i].nId)
    //                            {
    //                                bAdd = true;
    //                            }
    //                        }
    //                        else
    //                        {
    //                            bAdd = true;
    //                        }
    //                    }
    //                }
    //            }
    //        }

    //        if(bAdd)
    //        {
    //            listRes.Add(listInfos[i]);
    //        }
    //    }

    //    //�ݴ�ǿ�����
    //    if(listRes.Count <= 0)
    //    {
    //        for (int i = 0; i < listInfos.Count; i++)
    //        {
    //            if (listInfos[i].nNum > 0)
    //            {
    //                listRes.Add(listInfos[i]);
    //            }
    //        }
    //    }

    //    return listRes;
    //}

    ///// <summary>
    ///// ��ȡָ����Ʒ���������
    ///// </summary>
    ///// <param name="id"></param>
    ///// <returns></returns>
    //CGameVSGiftConfigSlot GetGiftConfigSlot(int id)
    //{
    //    CGameVSGiftConfigSlot pRes = null;
    //    for(int i=0; i< arrRandRangeConfigs.Length; i++)
    //    {
    //        if(arrRandRangeConfigs[i].nGiftID == id)
    //        {
    //            pRes = arrRandRangeConfigs[i];
    //            break;
    //        }
    //    }

    //    return pRes;
    //}

    //public static CGameVSGiftInfo Count(int count)
    //{
    //    if (Ins == null ||
    //        Ins.emCurState == EMState.End) return null;

    //    CGameVSGiftInfo pRes = null;
    //    Ins.nCurCount++;

    //    if(Ins.listGiftPools.Count > 0)
    //    {
    //        if (Ins.nCurCount >= Ins.listGiftPools[0].nIdx)
    //        {
    //            pRes = Ins.listGiftPools[0];
    //            Ins.listGiftPools.RemoveAt(0);

    //            Ins.dlgCountGift?.Invoke(pRes.nFishId);
    //        }
    //    }

    //    if(Ins.listGiftPools.Count <= 0)
    //    {
    //        Ins.emCurState = EMState.End;
    //        Ins.dlgPoolEnd?.Invoke();
    //    }

    //    return pRes;
    //}

    public int Count(string uid, int num)
    {
        int nRes = 0;
        CGameVSGiftCountPlayerInfo pInfo = null;
        if(!dicPlayerCounts.TryGetValue(uid, out pInfo))
        {
            pInfo = new CGameVSGiftCountPlayerInfo();
            pInfo.arrSlots = new CGameVSGiftCountSlot[arrConfigs.Length];
            for (int i = 0; i < pInfo.arrSlots.Length; i++)
            {
                pInfo.arrSlots[i] = new CGameVSGiftCountSlot();
                pInfo.arrSlots[i].nTbid = arrConfigs[i].nTbid;
                pInfo.arrSlots[i].nCount = arrConfigs[i].GetCount();
                pInfo.arrSlots[i].vCountRange = new Vector2Int(arrConfigs[i].vCountRange.x, arrConfigs[i].vCountRange.y);
                Debug.Log($"��{pInfo.arrSlots[i].nCount}�γ���<color=#F02000>{pInfo.arrSlots[i].nTbid}</color>");
            }

            dicPlayerCounts.Add(uid, pInfo);
        }

        if (pInfo == null) return 0;

        for(int i=0; i<pInfo.arrSlots.Length; i++)
        {
            if(pInfo.arrSlots[i].Count(num))
            {
                nRes = pInfo.arrSlots[i].nTbid;
                break;
            }
        }

        return nRes;
    }

    public CGameVSGiftCountPlayerInfo GetPlayerCountInfo(string uid)
    {
        CGameVSGiftCountPlayerInfo pInfo = null;
        if (dicPlayerCounts.TryGetValue(uid, out pInfo))
        {

        }
        return pInfo;
    }

    private void Update()
    {
        //if(emCurState == EMState.Gameing)
        //{
        //    if(pGameTimeTicker.Tick(CTimeMgr.DeltaTime))
        //    {
        //        emCurState = EMState.End;
        //        dlgPoolEnd?.Invoke();
        //    }
        //}

        //#region ���Խӿ�

        //if (!bDevTest) return;

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    CGameVSGiftInfo pRes = Count(1);
        //    if (pRes != null)
        //    {
        //        Debug.Log($"{nCurCount}��  ���ֽ�Ʒ:" + pRes.nFishId);
        //    }
        //    else
        //    {
        //        Debug.Log($"{nCurCount}��  �޽�Ʒ");
        //    }
        //}

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    InitGiftPool();
        //}

        //#endregion
    }

    [ContextMenu("���Խ���")]
    public void TestRandGift()
    {
        //InitGiftPool();
    }
}
