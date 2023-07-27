using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ST_WinnerBoat : CTBLConfigSlot
{
    public int count;
    public int boat;

    public override void InitByLoader(CTBLLoader loader)
    {
        count = loader.GetIntByName("count");
        boat = loader.GetIntByName("boatId");
    }
}

[CTBLConfigAttri("WinnerBoat")]
public class CTBLHandlerWinnerBoat : CTBLConfigBaseWithDic<ST_WinnerBoat>
{
    
}
