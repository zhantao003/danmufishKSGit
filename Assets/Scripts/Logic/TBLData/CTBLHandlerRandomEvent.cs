using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ST_RandomEvent : CTBLConfigSlot
{
    public int nRate;
    public bool bIsGood;
    public string szTitle;
    public string szDesc;

    public override void InitByLoader(CTBLLoader loader)
    {
        nRate = loader.GetIntByName("rate");
        bIsGood = (loader.GetIntByName("good") == 1);
        szTitle = loader.GetStringByName("title");
        szDesc = loader.GetStringByName("desc");
    }
}

[CTBLConfigAttri("RandomEvent")]
public class CTBLHandlerRandomEvent : CTBLConfigBaseWithDic<ST_RandomEvent>
{

}
