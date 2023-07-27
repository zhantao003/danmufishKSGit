using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ST_HelpDialog : CTBLConfigSlot
{
    public string szDialog;
    public int nSecenLv;

    public override void InitByLoader(CTBLLoader loader)
    {
        nID = loader.GetIntByName("id");
        nSecenLv = loader.GetIntByName("scenelv");
        szDialog = loader.GetStringByName("dialog");
    }

    public bool CheckSceneLv()
    {
        bool bInSceneLv = false;

        if(CGameColorFishMgr.Ins.nCurRateUpLv == nSecenLv)
        {
            bInSceneLv = true;
        }
        
        return bInSceneLv;
    }

}

[CTBLConfigAttri("HelpDialog")]
public class CTBLHandlerHelpDialog : CTBLConfigBaseWithDic<ST_HelpDialog>
{

}
