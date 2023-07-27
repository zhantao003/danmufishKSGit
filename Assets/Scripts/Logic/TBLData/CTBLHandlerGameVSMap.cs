using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ST_GameVSMapTreasureConfig
{
    public int nID;
    public int nMin;
    public int nMax;
}

public class ST_GameVSMap : CTBLConfigSlot
{
    public string szName;
    public string szSize;
    public string szIcon;
    public string szScene;
    public string szDesc;
    public long nUnlockFishCoin;
    public List<ST_GameVSMapTreasureConfig> listTreasure = new List<ST_GameVSMapTreasureConfig>();
    public CLocalNetArrayMsg arrUnlockFishInfo;

    public override void InitByLoader(CTBLLoader loader)
    {
        szName = loader.GetStringByName("name");
        szSize = loader.GetStringByName("size");
        szIcon = loader.GetStringByName("icon");
        szScene = loader.GetStringByName("scene");
        szDesc = loader.GetStringByName("desc");
        nUnlockFishCoin = loader.GetLongByName("unlockCoin");

        string szUnlockFishcontent = loader.GetStringByName("unlockFish");
        if (!CHelpTools.IsStringEmptyOrNone(szUnlockFishcontent))
        {
            CLocalNetMsg pUnlockFishContent = new CLocalNetMsg(szUnlockFishcontent);
            arrUnlockFishInfo = pUnlockFishContent.GetNetMsgArr("data");
        }
        else
        {
            arrUnlockFishInfo = null;
        }

        //∂¡»°±¶≤ÿ≈‰÷√
        listTreasure.Clear();
        string szTreasureConfigContent = loader.GetStringByName("treasureConfig");
        if(!CHelpTools.IsStringEmptyOrNone(szTreasureConfigContent))
        {
            CLocalNetMsg msgTreasureConfig = new CLocalNetMsg(szTreasureConfigContent);
            CLocalNetArrayMsg arrTreasureConfigs = msgTreasureConfig.GetNetMsgArr("data");
            for (int i = 0; i< arrTreasureConfigs.GetSize(); i++)
            {
                CLocalNetMsg msgConfigSlot = arrTreasureConfigs.GetNetMsg(i);
                ST_GameVSMapTreasureConfig pTConfigSlot = new ST_GameVSMapTreasureConfig();
                pTConfigSlot.nID = msgConfigSlot.GetInt("id");
                pTConfigSlot.nMin = msgConfigSlot.GetInt("min");
                pTConfigSlot.nMax = msgConfigSlot.GetInt("max"); 

                listTreasure.Add(pTConfigSlot);
            }
        }
    }

    public ST_GameVSMapTreasureConfig GetTreasureConfig(int fishId)
    {
        ST_GameVSMapTreasureConfig pRes = null;
        for(int i=0; i<listTreasure.Count; i++)
        {
            if (listTreasure[i].nID == fishId)
            {
                pRes = listTreasure[i];
            }
        }

        return pRes;
    }

    public List<ST_GameVSMapTreasureConfig> GetAllTreasureConfig()
    {
        return listTreasure;
    }
}

[CTBLConfigAttri("GameVSMap")]
public class CTBLHandlerGameVSMap : CTBLConfigBaseWithDic<ST_GameVSMap>
{

}
