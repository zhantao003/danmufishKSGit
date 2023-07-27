using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ST_UnitAvatar : CTBLConfigSlot
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
        Normal, //普通分页
        Diy,    //定制版
        Fes,    //活动限定
        Auction,    //拍卖宝箱
    }

    public EMTag emTag = EMTag.Normal;
    
    public int nPrice;
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
        nPrice = loader.GetIntByName("price");

        szName = loader.GetStringByName("name");
        szIcon = loader.GetStringByName("icon");
        szPrefab = loader.GetStringByName("prefab");
        szDesc = loader.GetStringByName("desc");
    }
}

[CTBLConfigAttri("UnitAvatar")]
public class CTBLHandlerUnitAvatar : CTBLConfigBaseWithDic<ST_UnitAvatar>
{
    
}
