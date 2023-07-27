using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGiftInfo
{
    public string szName;
    public AddInfo pAddInfo;
    public string szDes;

    public CGiftInfo()
    {

    }

    public CGiftInfo(ST_GiftInfo info)
    {
        szName = info.szName;
        pAddInfo = info.pAddInfo;
        szDes = info.szDes;
    }

}
