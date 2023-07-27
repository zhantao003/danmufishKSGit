using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ST_AuctionRate : CTBLConfigSlot
{
    public int nType;          //ÀàÐÍ
    public string szIcon;
    public string szDes;
    public string szRange;
    public float fMin;
    public float fMax;


    public override void InitByLoader(CTBLLoader loader)
    {
        nID = loader.GetIntByName("id");
        nType = loader.GetIntByName("type");
        szDes = loader.GetStringByName("DES");
        szIcon = loader.GetStringByName("Icon");
        szRange = loader.GetStringByName("Range");
        if(!CHelpTools.IsStringEmptyOrNone(szRange))
        {
            string[] szInfos = szRange.Split('|');
            fMin = int.Parse(szInfos[1]) * 0.0001f;
            fMax = int.Parse(szInfos[0]) * 0.0001f;
        }
    }

    public bool CheckInRange(long nlPrice,long basePrice)
    {
        bool bInRange = false;
        //Debug.LogError("==========================================================================================================================");
        if(nType == 0)
        {
            float fCurLerp = (float)nlPrice / (float)basePrice;
            //Debug.LogError(nlPrice + "=====" + basePrice + "=====" + fCurLerp + "====" + fMin + "====" + fMax);
            if(fCurLerp >= fMin &&
                fCurLerp < fMax)
            {
                bInRange = true;
            }
        }

        return bInRange;
    }

}

[CTBLConfigAttri("AuctionTreasureRate")]
public class CTBLHandlerAuctionTreasureRate : CTBLConfigBaseWithDic<ST_AuctionRate>
{
}
