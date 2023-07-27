using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 道具类型
/// </summary>
public enum EMItemType
{
    Fish = 0,           //鱼
    Other = 1,          //其他（非鱼的）
    FishMat = 2,        //渔场特产材料
    RandomEvent = 3,    //随机事件
    FishItem = 4,       //材料

    BattleItem = 7,     //战斗道具
}

public enum EMRare
{
    Normal = 0,             //普通
    YouXiu,                 //优秀
    XiYou,                  //稀有
    Special,                //特级
    Shisi,                  //史诗
}

public class ST_FishInfo : CTBLConfigSlot
{
    public EMItemType emItemType;
   
    public int nNormalSize;
    public EMRare emRare;
    public int nTreasureLv;
    public int nTreasurePoint;
    public int nWeight;
    public int nPrice;
    public bool bCanBianYi;
    public bool bSpecial;
    public bool bRecord;
    public int nFesPack;
    public int nFesPoint;
    public int nAddExp;
    public string szName;
    public string szIcon;
    public string szSceneID;
    public int[] nSceneID;

    public override void InitByLoader(CTBLLoader loader)
    {
        nID = loader.GetIntByName("id");
        emItemType = (EMItemType)loader.GetIntByName("type");
        szName = loader.GetStringByName("name");
        nNormalSize = loader.GetIntByName("size");
        emRare = (EMRare)loader.GetIntByName("rare");
        nTreasureLv = loader.GetIntByName("treasureLv");
        nTreasurePoint = loader.GetIntByName("treasurePoint");
        nWeight = loader.GetIntByName("weight");
        nPrice = loader.GetIntByName("price");
        szIcon = loader.GetStringByName("icon");
        bCanBianYi = loader.GetIntByName("bianyi") > 0;
        bSpecial = loader.GetIntByName("special") > 0;
        szSceneID = loader.GetStringByName("sceneid");
        bRecord = loader.GetIntByName("record") > 0;
        nFesPack = loader.GetIntByName("fespack");
        nFesPoint = loader.GetIntByName("fespoint");
        nAddExp = loader.GetIntByName("exp");
        GetSceneID();

        //活动获得鱼
        if (!CFishFesInfoMgr.Ins.IsFesOn((int)CFishFesInfoMgr.EMFesType.RankRicher) &&
             nID == 9000001)
        {
            nWeight = 0;
        }
    }

    public void GetSceneID()
    {
        if (CHelpTools.IsStringEmptyOrNone(szSceneID)) return;
        string[] szInfos = szSceneID.Split('|');
        nSceneID = new int[szInfos.Length];
        for(int i = 0;i < szInfos.Length;i++)
        {
            nSceneID[i] = int.Parse(szInfos[i]);
        }
    }

    public bool InSceneID(int nID)
    {
        bool bIn = false;
        for(int i = 0;i < nSceneID.Length;i++)
        {
            if(nSceneID[i] == nID)
            {
                bIn = true;
                break;
            }
        }
        return bIn;
    }
}

public class CTBLHandlerFishInfo : CTBLConfigBaseWithDicNoIns<ST_FishInfo>
{
    /// <summary>
    /// 所有物体的总权重
    /// </summary>
    public int nMaxWeight;

    public Dictionary<EMItemType, List<ST_FishInfo>> dicFishInfos = new Dictionary<EMItemType, List<ST_FishInfo>>();

    public override void LoadInfo(CTBLLoader loader)
    {
        nMaxWeight = 0;
        for (int i = 0; i < loader.GetLineCount(); i++)
        {
            loader.GotoLineByIndex(i);

            ST_FishInfo pInfo = (ST_FishInfo)Activator.CreateInstance(typeof(ST_FishInfo), true);
            pInfo.InitByLoader(loader);
            nMaxWeight += pInfo.nWeight;
            dicInfos.Add(pInfo.nID, pInfo);

            List<ST_FishInfo> listFishs = null;
            if(!dicFishInfos.TryGetValue(pInfo.emItemType,out listFishs))
            {
                listFishs = new List<ST_FishInfo>();
                dicFishInfos.Add(pInfo.emItemType, listFishs);
            }
            listFishs.Add(pInfo);

            if (i == 0)
            {
                nMinId = pInfo.nID;
            }
            else
            {
                if (nMinId > pInfo.nID)
                {
                    nMinId = pInfo.nID;
                }
            }

            if (nMaxId < pInfo.nID)
            {
                nMaxId = pInfo.nID;
            }
        }
    }

    public List<ST_FishInfo> GetFishInfos(EMItemType emItemType)
    {
        List<ST_FishInfo> listFishs = null;

        dicFishInfos.TryGetValue(emItemType, out listFishs);

        return listFishs;
    }

    public ST_FishInfo GetTargetFishInfoByType()
    {
        ST_FishInfo fishInfo = null;
        List<ST_FishInfo> listFishInfos = GetInfos();// GetFishInfos(EMItemType.Fish);
        if (listFishInfos == null) return null;
        int nTotalWeight = 0;
        for (int i = 0; i < listFishInfos.Count; i++)
        {
            ///检测主播出现数量是否满了
            if (CGameColorFishMgr.Ins.CheckMaxVtuberCount() && listFishInfos[i].nID == 1000001)
                continue;
            int nWeight = listFishInfos[i].nWeight;
            
            nTotalWeight += nWeight;
        }
        int nRandomWeight = UnityEngine.Random.Range(0, nTotalWeight + 1);
        for (int i = 0; i < listFishInfos.Count; i++)
        {
            ///检测主播出现数量是否满了
            if (CGameColorFishMgr.Ins.CheckMaxVtuberCount() && listFishInfos[i].nID == 1000001)
                continue;
            int nWeight = listFishInfos[i].nWeight;
            
            nRandomWeight -= nWeight;
            if (nRandomWeight <= 0)
            {
                fishInfo = listFishInfos[i];
                break;
            }
        }
        if (fishInfo.nID == 1000001)
        {
            CGameColorFishMgr.Ins.AddVtuberCount();
        }

        return fishInfo;
    }

    public ST_FishInfo GetRandomFishInfo(EMItemType emItemType,int nAddFishRareRate, int nAddZawuRareRate, int nAddFishMatRareRate,float nAddBaseRate = 1f, EMRare emGetRare = EMRare.Normal,bool goldBoom = false)
    {
        ST_FishInfo fishInfo = null;
        List<ST_FishInfo> listFishInfos = GetFishInfos(emItemType);
        if (listFishInfos == null ||
            listFishInfos.Count <= 0)
        {
            emItemType = EMItemType.Fish;
            listFishInfos = GetFishInfos(emItemType);
            if (listFishInfos == null ||
                listFishInfos.Count <= 0)
            {
                return null;
            }
        }
        if(goldBoom && emItemType == EMItemType.FishMat)
        {
            for(int i = 0;i < listFishInfos.Count;i++)
            {
                if(listFishInfos[i].emItemType == emItemType &&
                    listFishInfos[i].nNormalSize == 1)
                {
                    fishInfo = listFishInfos[i];
                    break;
                }
            }
            return fishInfo;
        }
        int nTotalWeight = 0;
        for(int i = 0;i < listFishInfos.Count;i++)
        {
            ///检测主播出现数量是否满了
            if (CGameColorFishMgr.Ins.CheckMaxVtuberCount() && listFishInfos[i].nID == 1000001)
                continue;
            if (listFishInfos[i].emRare < emGetRare)
                continue;

            int nWeight = listFishInfos[i].nWeight;
            if ((int)listFishInfos[i].emRare >= (int)EMRare.Special)
            {
                nWeight = System.Convert.ToInt32(nWeight * nAddBaseRate);
            }
            if ((int)listFishInfos[i].emRare >= (int)EMRare.XiYou)
            {
                if(listFishInfos[i].emItemType == EMItemType.Fish ||
                   listFishInfos[i].emItemType == EMItemType.BattleItem)
                {
                    nWeight += System.Convert.ToInt32(nWeight * nAddFishRareRate * 0.01f);
                }
                else if (listFishInfos[i].emItemType == EMItemType.Other)
                {
                    nWeight += System.Convert.ToInt32(nWeight * nAddZawuRareRate * 0.01f);
                }
                else if (listFishInfos[i].emItemType == EMItemType.FishMat)
                {
                    nWeight += System.Convert.ToInt32(nWeight * nAddFishMatRareRate * 0.01f);
                }
            }
            nTotalWeight += nWeight;
        }
        int nRandomWeight = UnityEngine.Random.Range(0, nTotalWeight + 1);
        for (int i = 0; i < listFishInfos.Count; i++)
        {
            ///检测主播出现数量是否满了
            if (CGameColorFishMgr.Ins.CheckMaxVtuberCount() && listFishInfos[i].nID == 1000001)
                continue;
            if (listFishInfos[i].emRare < emGetRare)
                continue;
            int nWeight = listFishInfos[i].nWeight;
            if ((int)listFishInfos[i].emRare >= (int)EMRare.Special)
            {
                nWeight = System.Convert.ToInt32(nWeight * nAddBaseRate);
            }

            if ((int)listFishInfos[i].emRare >= (int)EMRare.XiYou)
            {
                if (listFishInfos[i].emItemType == EMItemType.Fish ||
                    listFishInfos[i].emItemType == EMItemType.BattleItem)
                {
                    nWeight += System.Convert.ToInt32(nWeight * nAddFishRareRate * 0.01f);
                }
                else if (listFishInfos[i].emItemType == EMItemType.Other)
                {
                    nWeight += System.Convert.ToInt32(nWeight * nAddZawuRareRate * 0.01f);
                }
                else if (listFishInfos[i].emItemType == EMItemType.FishMat)
                {
                    nWeight += System.Convert.ToInt32(nWeight * nAddFishMatRareRate * 0.01f);
                }
            }
            nRandomWeight -= nWeight;
            if(nRandomWeight <= 0)
            {
                fishInfo = listFishInfos[i];
                break;
            }
        }
        if(fishInfo.nID == 1000001)
        {
            CGameColorFishMgr.Ins.AddVtuberCount();
        }

        return fishInfo;
    }

    public ST_FishInfo GetRandomVSFishInfo(EMItemType emItemType, int nAddFishRareRate, int nAddZawuRareRate, int nAddFishMatRareRate, float nAddBaseRate = 1f, bool bMustGetChengSe = false)
    {
        ST_FishInfo fishInfo = null;
        List<ST_FishInfo> listFishInfos = GetFishInfos(emItemType);
        if (listFishInfos == null ||
            listFishInfos.Count <= 0)
        {
            emItemType = EMItemType.Fish;
            listFishInfos = GetFishInfos(emItemType);
            if (listFishInfos == null ||
                listFishInfos.Count <= 0)
            {
                return null;
            }
        }
        int nTotalWeight = 0;
        for (int i = 0; i < listFishInfos.Count; i++)
        {
            ////炸鱼模式下，不能直接摸到特级以上
            //if (listFishInfos[i].emRare >= EMRare.Special)
            //    continue;

            int nWeight = listFishInfos[i].nWeight;
            //if ((int)listFishInfos[i].emRare >= (int)EMRare.Special)
            //{
            //    nWeight = System.Convert.ToInt32(nWeight * nAddBaseRate);
            //}

            if ((int)listFishInfos[i].emRare >= (int)EMRare.XiYou)
            {
                if (listFishInfos[i].emItemType == EMItemType.Fish)
                {
                    nWeight += System.Convert.ToInt32(nWeight * nAddFishRareRate * 0.01f);
                }
                else if (listFishInfos[i].emItemType == EMItemType.Other)
                {
                    nWeight += System.Convert.ToInt32(nWeight * nAddZawuRareRate * 0.01f);
                }
                else if (listFishInfos[i].emItemType == EMItemType.FishMat)
                {
                    nWeight += System.Convert.ToInt32(nWeight * nAddFishMatRareRate * 0.01f);
                }
            }

            nTotalWeight += nWeight;
        }

        int nRandomWeight = UnityEngine.Random.Range(0, nTotalWeight + 1);

        //Debug.Log("总权重：" + nTotalWeight + "   随机权重：" + nRandomWeight);

        for (int i = 0; i < listFishInfos.Count; i++)
        {
            ////炸鱼模式下，不能直接摸到特级以上
            //if (listFishInfos[i].emRare >= EMRare.Special)
            //    continue;

            int nWeight = listFishInfos[i].nWeight;
            //if ((int)listFishInfos[i].emRare >= (int)EMRare.Special)
            //{
            //    nWeight = System.Convert.ToInt32(nWeight * nAddBaseRate);
            //}

            if ((int)listFishInfos[i].emRare >= (int)EMRare.XiYou)
            {
                if (listFishInfos[i].emItemType == EMItemType.Fish)
                {
                    nWeight += System.Convert.ToInt32(nWeight * nAddFishRareRate * 0.01f);
                }
                else if (listFishInfos[i].emItemType == EMItemType.Other)
                {
                    nWeight += System.Convert.ToInt32(nWeight * nAddZawuRareRate * 0.01f);
                }
                else if (listFishInfos[i].emItemType == EMItemType.FishMat)
                {
                    nWeight += System.Convert.ToInt32(nWeight * nAddFishMatRareRate * 0.01f);
                }
            }

            nRandomWeight -= nWeight;
            if (nRandomWeight <= 0)
            {
                fishInfo = listFishInfos[i];
                break;
            }
        }

        return fishInfo;
    }

    public ST_FishInfo GetRandomFish(float fAddGetRate)
    {
        ST_FishInfo fishInfo = null;
        ST_FishInfo preFishInfo = null;
        int nWeight = 0;
        int nRandomWeight = UnityEngine.Random.Range(0, nMaxWeight + 1);
        List<ST_FishInfo> listFishInfos = GetInfos();
        for(int i = 0;i < listFishInfos.Count;i++)
        {
            if (fAddGetRate > 0)
            {
                nWeight = (int)((float)listFishInfos[i].nWeight * (1f - fAddGetRate));
            }
            else
            {
                nWeight = listFishInfos[i].nWeight;
            }
            nRandomWeight -= nWeight;
            if(nWeight > 0 &&
               nRandomWeight <= 0)
            {
                fishInfo = listFishInfos[i];
                break;
            }
            if(nWeight > 0)
            {
                preFishInfo = listFishInfos[i];
            }
            if(i == listFishInfos.Count - 1)
            {
                fishInfo = preFishInfo;
                break;
            }
        }
        return fishInfo;
    }
}