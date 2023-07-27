using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ST_ShopTreasure : CTBLConfigSlot
{
    public enum EMItemType
    {
        Role = 0,
        Boat,
    }

    public EMItemType emType;
    public int nContentID;
    public int nPrice;

    public override void InitByLoader(CTBLLoader loader)
    {
        emType = (EMItemType)(loader.GetIntByName("type"));
        nContentID = loader.GetIntByName("contentid");
        nPrice = loader.GetIntByName("price");
    }
}

[CTBLConfigAttri("ShopTreasure")]
public class CTBLHandlerTreasureShop : CTBLConfigBaseWithDic<ST_ShopTreasure>
{
    
}
