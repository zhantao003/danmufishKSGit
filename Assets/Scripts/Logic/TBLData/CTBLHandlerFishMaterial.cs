using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ST_FishMat : CTBLConfigSlot
{
    public enum EMRare
    {
        R = 0,
        SR,
        SSR,
    }
    public EMRare emRare = EMRare.R;
    public long nDailyGetMax;
    public string szName;
    public string szIcon;
    public string szDesc;

    public override void InitByLoader(CTBLLoader loader)
    {
        emRare = (EMRare)loader.GetIntByName("rare");
        nDailyGetMax = loader.GetLongByName("dailyGet");
        szName = loader.GetStringByName("name");
        szIcon = loader.GetStringByName("icon");
        szDesc = loader.GetStringByName("desc");
    }
}

[CTBLConfigAttri("FishMaterial")]
public class CTBLHandlerFishMaterial : CTBLConfigBaseWithDic<ST_FishMat>
{
    
}
