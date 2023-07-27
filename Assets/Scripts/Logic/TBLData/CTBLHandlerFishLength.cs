using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ST_FishLength : CTBLConfigSlot
{
    public int nMinValue;
    public int nMaxValue;
    public int nWeight;

    public override void InitByLoader(CTBLLoader loader)
    {
        nID = loader.GetIntByName("id");
        nMinValue = loader.GetIntByName("minvalue");
        nMaxValue = loader.GetIntByName("maxvalue");
        nWeight = loader.GetIntByName("weight");
    }
}

[CTBLConfigAttri("FishLength")]
public class CTBLHandlerFishLength : CTBLConfigBaseWithDic<ST_FishLength>
{

}