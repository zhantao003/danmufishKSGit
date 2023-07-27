using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ST_UnitFishBoat : CTBLConfigSlot
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
    public string szName;
    public string szIcon;
    public string szPrefab;
    public string szDesc;

    //是否当前赛季的
    public bool bIsSeason = false;

    public override void InitByLoader(CTBLLoader loader)
    {
        emRare = (EMRare)(loader.GetIntByName("rare"));
        emTag = (EMTag)(loader.GetIntByName("tag"));
        nItemId = loader.GetIntByName("itemId");
        nItemNum = loader.GetIntByName("itemNum");

        szName = loader.GetStringByName("name");
        szIcon = loader.GetStringByName("icon");
        szPrefab = loader.GetStringByName("prefab");
        szDesc = loader.GetStringByName("desc");
    }
}

[CTBLConfigAttri("UnitFishBoat")]
public class CTBLHandlerUnitFishBoat : CTBLConfigBaseWithDic<ST_UnitFishBoat>
{
    
}
