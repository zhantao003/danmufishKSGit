using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
//public class CGameVSGiftConfigSlot
//{
//    public int nGiftID; //奖品ID

//    [System.Serializable]
//    public class CGameVSGiftCurSlot
//    {
//        public float fCurRange;
//        public float fCreateTotalNum;
//    }

//    public CGameVSGiftCurSlot[] arrCutSlot; //数量-奖池区间 分布配置

//    //出现区间起点(百分比)
//    public bool bRareStartLock = false;
//    public float fRareStartPos = 0.4F;

//    //最后一个出现区间起点(百分比)
//    public bool bRareLastLock = false;
//    public float fRareLastPos = 0.8F;

//    //奖品单区间出现的范围(百分比)
//    public bool bConnectUnable  = true;   //是否允许连续出现
//    public Vector2Int vGiftCreateRange;

//    public string szLogColor;
//}

//[System.Serializable]
//public class CGameVSGiftInfo
//{
//    public int nIdx;    //出现次数
//    public int nFishId; //奖品ID
//    public bool bBianyi = false;
//    public string szLogColor = "";
//}

///// <summary>
///// 奖品随机信息
///// </summary>
//public class CGameVSGiftRandInfo
//{
//    public int nId;
//    public int nNum;
//    public int nBaseNum; //配置数量

//    public int nCurCutIdx = 0; //当前的切割索引
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
//            Debug.Log($"更新{nId}的Cut后剩余{nNum}个");

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

//玩家的计数器
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

    //随机区间配置
    //public CGameVSGiftConfigSlot[] arrRandRangeConfigs;

    //[ReadOnly]
    //public int nFinalRandRange; //最终随机区间
    //[ReadOnly]
    //public int nAverageRange;  //随机区间

    //[ReadOnly]
    //public int nCurCount;   //当前计数

    ////游戏倒计时
    //[ReadOnly]
    //public CPropertyTimer pGameTimeTicker = new CPropertyTimer();

    //[HideInInspector]
    //public List<CGameVSGiftRandInfo> listAllGiftBaseInfos = new List<CGameVSGiftRandInfo>();    //奖品基础信息
    //[HideInInspector]
    //public List<CGameVSGiftInfo> listGiftPools = new List<CGameVSGiftInfo>();  //奖池

    ////上一次随机的奖品
    //int nPreGiftId;

    ///// <summary>
    ///// 获得道具委托
    ///// </summary>
    //public DelegateIFuncCall dlgCountGift;

    ///// <summary>
    ///// 游戏结束
    ///// </summary>
    //public DelegateNFuncCall dlgPoolEnd;

    public bool bDevTest;

    ///// <summary>
    ///// 初始化奖池总随机区间
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

    //    Debug.Log("奖品数量：" + nMapFishNum + "---奖池随机数：" + nFinalRandRange + "----奖池单区间：" + nAverageRange);
    //}

    ///// <summary>
    ///// 随机奖池
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

    //    //奖品随机库
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

    //        //统计总数
    //        nGiftTotalNum += pMapConfigSlot.dicFishInfos[key];

    //        ST_FishInfo pFishInfo = pTBLHandlerFishInfo.GetInfo(key);
    //        if (pFishInfo != null)
    //        {
    //            pGiftInfo.emRare = pFishInfo.emRare;
    //        }

    //        listGiftInfos.Add(pGiftInfo);
    //        listAllGiftBaseInfos.Add(pBaseGiftInfo);
    //    }

    //    //先把奖池排序随机好
    //    string szLogRes = $"总次数：{nFinalRandRange}   奖池信息：\r\n";
    //    List<CGameVSGiftRandInfo> listTmpRandGiftPool;
    //    for (int i=0; i<nGiftTotalNum; i++)
    //    {
    //        listTmpRandGiftPool = GetTmpRandGiftPool(i + 1, nGiftTotalNum, ref listGiftInfos);
    //        if(listTmpRandGiftPool.Count > 0)
    //        {
    //            //随机
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

    //            //计算出现的次数
    //            CGameVSGiftConfigSlot pConfigSlot = GetGiftConfigSlot(nTargetGiftId);

    //            int nCreatePos = 0;
    //            int nMinAdd = (int)(nAverageRange * pConfigSlot.vGiftCreateRange.x * 0.01F);
    //            int nMaxAdd = nAverageRange - (int)(nAverageRange * pConfigSlot.vGiftCreateRange.y * 0.01F);
    //            int nFinalRangeMin = 0;
    //            int nFinalRangeMax = 0;

    //            //Debug.Log("开始插值：" + nMinAdd + "   结束插值：" + nMaxAdd);

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

    //            //加入奖池
    //            listGiftPools.Add(new CGameVSGiftInfo() { 
    //                nIdx = nCreatePos,
    //                nFishId = nTargetGiftId,
    //                szLogColor = pConfigSlot.szLogColor,
    //            });

    //            nPreGiftId = nTargetGiftId;

    //            //计数
    //            for (int j = 0; j < listGiftInfos.Count; j++)
    //            {
    //                if (listGiftInfos[j].nId == nTargetGiftId)
    //                {
    //                    listGiftInfos[j].nNum--;
    //                    break;
    //                }
    //            }

    //            //日志内容
    //            string szColor = pConfigSlot.szLogColor; //(pFishInfo.emRare >= EMRare.Shisi) ? "ff50ff" : "ffffff";

    //            if(i<nGiftTotalNum - 1)
    //            {
    //                szLogRes += $"区间[{1 + i * nAverageRange}-{(i + 1) * nAverageRange}]   第<color=#ffff00>{nCreatePos}</color>次  出现奖品{i + 1}---" + $"<color=#{szColor}>{listGiftPools[i].nFishId}</color>" + "\r\n";
    //            }
    //            else
    //            {
    //                szLogRes += $"区间[{1 + i * nAverageRange}-{nFinalRandRange}]   第<color=#ffff00>{nCreatePos}</color>次  出现奖品{i + 1}---" + $"<color=#{szColor}>{listGiftPools[i].nFishId}</color>" + "\r\n";
    //            }
    //        }
    //    }

    //    //重置游戏状态
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

    //        //判断是否更新cut
    //        if (config.arrCutSlot != null &&
    //            config.arrCutSlot.Length > 0 &&
    //            listInfos[i].nCurCutIdx < config.arrCutSlot.Length)
    //        { 
    //            float fRate = (float)idx / (float)max;
    //            if (fRate > config.arrCutSlot[listInfos[i].nCurCutIdx].fCurRange)
    //            {
    //                Debug.Log($"第{idx}次 更新{listInfos[i].nId}的Cut   比例情况：{fRate}:{config.arrCutSlot[listInfos[i].nCurCutIdx].fCurRange}");

    //                listInfos[i].NextCut();
    //            }
    //        }

    //        if (listInfos[i].nNum > 0)
    //        {
    //            //判断有没有cut
    //            if(config.arrCutSlot != null &&
    //               config.arrCutSlot.Length > 0 &&
    //               listInfos[i].nCurCutIdx < config.arrCutSlot.Length)
    //            {
    //                float fCurRate = (float)idx / (float)max;
    //                if(fCurRate <= config.arrCutSlot[listInfos[i].nCurCutIdx].fCurRange)
    //                {
    //                    //判断剩余池子段位不够填充
    //                    int nBoard = System.Convert.ToInt32(max * config.arrCutSlot[listInfos[i].nCurCutIdx].fCurRange);
    //                    if (idx - 1 + listInfos[i].nNum == nBoard)
    //                    {
    //                        Debug.Log($"第{idx}次 填充剩下{listInfos[i].nNum}次  界限：{nBoard}");
    //                        listRes.Clear();
    //                        listRes.Add(listInfos[i]);
    //                        return listRes;
    //                    }
    //                }
    //            }

    //            //最后一个 直接给了
    //            if(idx == max)
    //            {
    //                bAdd = true;
    //            }
    //            else
    //            {
    //                //检查是否有最终限制
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
    //                    //判断是否有初始位置限制
    //                    if(config.bRareStartLock)
    //                    {
    //                        int nTmpCheck = System.Convert.ToInt32(max * config.fRareStartPos);
    //                        if (idx >= nTmpCheck)
    //                        {
    //                            //判断是否有连续限制
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
    //                        //判断是否有连续限制
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

    //    //容错强制添加
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
    ///// 获取指定奖品的随机配置
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
                Debug.Log($"第{pInfo.arrSlots[i].nCount}次出现<color=#F02000>{pInfo.arrSlots[i].nTbid}</color>");
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

        //#region 测试接口

        //if (!bDevTest) return;

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    CGameVSGiftInfo pRes = Count(1);
        //    if (pRes != null)
        //    {
        //        Debug.Log($"{nCurCount}次  出现奖品:" + pRes.nFishId);
        //    }
        //    else
        //    {
        //        Debug.Log($"{nCurCount}次  无奖品");
        //    }
        //}

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    InitGiftPool();
        //}

        //#endregion
    }

    [ContextMenu("测试奖池")]
    public void TestRandGift()
    {
        //InitGiftPool();
    }
}
