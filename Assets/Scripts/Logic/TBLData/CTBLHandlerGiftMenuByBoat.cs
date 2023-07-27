using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMGiftMenuType
{
    FishCoin = 0,
    FeiLun,
    FishPack,
    HaiDaoCoin,
}

public enum EMGiftMenuRare
{
    Normal = 0,
    Special,
}

public class ST_GiftMenuByBoat : CTBLConfigSlot
{
    public EMGiftMenuType emGiftMenuType;     
    public int nCount;
    public EMGiftMenuRare emGiftMenuRare;
    public int nWeight;

    public override void InitByLoader(CTBLLoader loader)
    {
        nID = loader.GetIntByName("id");
        emGiftMenuType = (EMGiftMenuType)loader.GetIntByName("type");
        nCount = loader.GetIntByName("count");
        emGiftMenuRare = (EMGiftMenuRare)loader.GetIntByName("rare");
        nWeight = loader.GetIntByName("weight");
    }

}

[CTBLConfigAttri("GiftMenuByBoat")]
public class CTBLHandlerGiftMenuByBoat : CTBLConfigBaseWithDic<ST_GiftMenuByBoat>
{
    public static CTBLHandlerGiftMenuByBoat pIns = null;

    public Dictionary<EMGiftMenuRare, List<ST_GiftMenuByBoat>> dicInfoByMenuRare = new Dictionary<EMGiftMenuRare, List<ST_GiftMenuByBoat>>();

    public List<ST_GiftMenuByBoat> GetInfosByRare(EMGiftMenuRare giftMenuRare)
    {
        List<ST_GiftMenuByBoat> listInfos = null;
        dicInfoByMenuRare.TryGetValue(giftMenuRare, out listInfos);
        return listInfos;
    }

    public ST_GiftMenuByBoat GetRandomInfoByRare(EMGiftMenuRare giftMenuRare)
    {
        ST_GiftMenuByBoat info = null;
        List<ST_GiftMenuByBoat> listInfos = GetInfosByRare(giftMenuRare);
        if(listInfos != null)
        {
            int nTotalWeight = 0;
            for (int i = 0; i < listInfos.Count;i++)
            {
                nTotalWeight += listInfos[i].nWeight;
            }
            int nRandomWeight = UnityEngine.Random.Range(0, nTotalWeight + 1);
            for (int i = 0; i < listInfos.Count; i++)
            {
                if (listInfos[i].nWeight <= 0) continue;
                nRandomWeight -= listInfos[i].nWeight;
                if (nRandomWeight <= 0)
                {
                    info = listInfos[i];
                    break;
                }
            }
        }

        return info;
    }

    public override void LoadInfo(CTBLLoader loader)
    {
        pIns = this;
        Ins = this;

        for (int i = 0; i < loader.GetLineCount(); i++)
        {
            loader.GotoLineByIndex(i);

            ST_GiftMenuByBoat pInfo = (ST_GiftMenuByBoat)Activator.CreateInstance(typeof(ST_GiftMenuByBoat), true);
            pInfo.nID = loader.GetIntByName("id");
            pInfo.InitByLoader(loader);
            dicInfos.Add(pInfo.nID, pInfo);

            List<ST_GiftMenuByBoat> listInfos = null;
            dicInfoByMenuRare.TryGetValue(pInfo.emGiftMenuRare, out listInfos);
            if (listInfos == null)
            {
                listInfos = new List<ST_GiftMenuByBoat>();
                listInfos.Add(pInfo);
                dicInfoByMenuRare.Add(pInfo.emGiftMenuRare, listInfos);
            }
            else
            {
                listInfos.Add(pInfo);
            }

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

}
