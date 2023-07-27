using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ST_MingrenTang : CTBLConfigSlot
{
    public enum EMMinRenType
    {
        Role = 0,
        Boat,
    }

    public enum EMMinRenTag
    {
        Normal,
        Diy
    }

    public EMMinRenType emType;
    public EMMinRenTag emTag;
    public int nContentID;
    public string szIcon;
    public string szName;
    public string szDesc;

    public override void InitByLoader(CTBLLoader loader)
    {
        emType = (EMMinRenType)(loader.GetIntByName("type"));
        emTag = (EMMinRenTag)(loader.GetIntByName("tag"));
        nContentID = loader.GetIntByName("contentid");
        szIcon = loader.GetStringByName("icon");
        szName = loader.GetStringByName("name");
        szDesc = loader.GetStringByName("desc");
    }
}

[CTBLConfigAttri("MinRenTang")]
public class CTBLHandlerMingrenTang : CTBLConfigBaseWithDic<ST_MingrenTang>
{
    
}
