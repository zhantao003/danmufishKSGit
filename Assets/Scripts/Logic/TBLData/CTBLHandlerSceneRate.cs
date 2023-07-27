using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ST_SceneRate : CTBLConfigSlot
{
    public int nAddFishRate;
    public int nAddBigFishRate;
    public int nAddRareRate;
    public string szName;
    public string szDes;

    public override void InitByLoader(CTBLLoader loader)
    {
        nID = loader.GetIntByName("id");
        nAddFishRate = loader.GetIntByName("addfishrate");
        nAddBigFishRate = loader.GetIntByName("addbigfishrate");
        nAddRareRate = loader.GetIntByName("addrarerate");
        szName = loader.GetStringByName("name");
        szDes = loader.GetStringByName("des");
    }
}

[CTBLConfigAttri("SceneRate")]
public class CTBLHandlerSceneRate : CTBLConfigBaseWithDic<ST_SceneRate>
{

}
