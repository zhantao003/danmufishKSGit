using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ST_MapExp : CTBLConfigSlot
{
    public int nExp;

    public override void InitByLoader(CTBLLoader loader)
    {
        nID = loader.GetIntByName("id");
        nExp = loader.GetIntByName("exp");
    }
}

[CTBLConfigAttri("MapExp")]
public class CTBLHandlerMapExp : CTBLConfigBaseWithDic<ST_MapExp>
{

}