using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ST_RicherBoat : CTBLConfigSlot
{
    public int count;
    public int roleId;

    public override void InitByLoader(CTBLLoader loader)
    {
        count = loader.GetIntByName("count");
        roleId = loader.GetIntByName("roleId");
    }
}

[CTBLConfigAttri("RicherBoat")]
public class CTBLHandlerRicherBoat : CTBLConfigBaseWithDic<ST_RicherBoat>
{
    
}
