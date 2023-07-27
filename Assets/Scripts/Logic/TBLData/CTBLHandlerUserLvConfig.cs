using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ST_UserLvConfig : CTBLConfigSlot
{
    public long nExp;
    public int nShowLv;
    public int nTag;
    public string szName;

    public override void InitByLoader(CTBLLoader loader)
    {
        nExp = loader.GetLongByName("exp");
        nShowLv = loader.GetIntByName("tagLv");
        nTag = loader.GetIntByName("tag");
        szName = loader.GetStringByName("name");
    }
}

[CTBLConfigAttri("UserLvConfig")]
public class CTBLHandlerUserLvConfig : CTBLConfigBaseWithDic<ST_UserLvConfig>
{

}
