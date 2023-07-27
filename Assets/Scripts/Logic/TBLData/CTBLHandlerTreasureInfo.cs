using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ST_TreasureInfo : CTBLConfigSlot
{
    public int nType;
    public int nTargetID;

    public override void InitByLoader(CTBLLoader loader)
    {
        nID = loader.GetIntByName("id");
        nType = loader.GetIntByName("type");
        nTargetID = loader.GetIntByName("targetid");
    }
}

[CTBLConfigAttri("TreasureInfo")]
public class CTBLHandlerTreasureInfo : CTBLConfigBaseWithDic<ST_TreasureInfo>
{

}
