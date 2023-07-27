using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ST_UnitFishGanLvInfo
{
    public int nLv;
    public int nExp;
    public string szPrefab;
}

public class ST_UnitFishGan : CTBLConfigSlot
{
    public enum EMRare
    {
        None = 0,

        R = 1,
        SR,
        SSR,
        UR,
    }

    public EMRare emRare = EMRare.R;

    public enum EMTag
    {
        Normal = 0,     //普通分页
        Diy,            //定制版
        Exchange,       //可兑换
        TreasureShop,   //宝藏商店
        BossDrop,       //Boss掉落
    }

    public EMTag emTag = EMTag.Normal;

    public int nItemId;
    public long nItemNum;
    public long nExpFishID;
    public EMAddUnitProType emProType;
    public int nProAddMin;
    public int nProAddMax;
    public string szName;
    public string szIcon;
    public string szPrefabPath;
    public string szDesc;
    public ST_UnitFishGanLvInfo[] arrLvInfo;

    public override void InitByLoader(CTBLLoader loader)
    {
        emRare = (EMRare)(loader.GetIntByName("rare"));
        emTag = (EMTag)(loader.GetIntByName("tag"));
        nItemId = loader.GetIntByName("itemId");
        nItemNum = loader.GetIntByName("itemNum");
        nExpFishID = loader.GetIntByName("expFish");
        emProType = (EMAddUnitProType)loader.GetIntByName("proType");
        nProAddMin = loader.GetIntByName("proAddMin");
        nProAddMax = loader.GetIntByName("proAddMax");

        szName = loader.GetStringByName("name");
        szIcon = loader.GetStringByName("icon");
        szPrefabPath = loader.GetStringByName("prefabPath");
        szDesc = loader.GetStringByName("desc");

        string szLvInfo = loader.GetStringByName("lvPrefab");
        CLocalNetArrayMsg arrContent = new CLocalNetArrayMsg(szLvInfo);
        if(arrContent!=null)
        {
            arrLvInfo = new ST_UnitFishGanLvInfo[arrContent.GetSize()];
            for(int i=0; i<arrContent.GetSize(); i++)
            {
                CLocalNetMsg msgContent = arrContent.GetNetMsg(i);
                arrLvInfo[i] = new ST_UnitFishGanLvInfo();
                arrLvInfo[i].nLv = msgContent.GetInt("lv");
                arrLvInfo[i].nExp = msgContent.GetInt("exp");
                arrLvInfo[i].szPrefab = msgContent.GetString("prefab");
            }
        }
    }

    //获取等级信息
    public ST_UnitFishGanLvInfo GetLv(int lv)
    {
        ST_UnitFishGanLvInfo res = null;
        for(int i=0; i<arrLvInfo.Length; i++)
        {
            if(arrLvInfo[i].nLv == lv)
            {
                res = arrLvInfo[i];
                break;
            }
        }

        return res;
    }
}

[CTBLConfigAttri("UnitFishGan")]
public class CTBLHandlerUnitFishGan : CTBLConfigBaseWithDic<ST_UnitFishGan>
{
    
}
