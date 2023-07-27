using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 增益信息
/// </summary>
[System.Serializable]
public class AddInfo
{
    public int nAddFishRate;                //钓到鱼的概率
    public int nAddBigFishRate;             //钓到大鱼的概率
    public int nAddFishRareRate;            //钓到稀有鱼类型的概率
    public int nAddZawuRareRate;            //钓到稀有杂物类型的概率
    public int nAddFishMatRareRate;            //钓到稀有渔场特产类型的概率
    public int nAddFishSpeed;               //挂机钓鱼的速度
    public int nAddFishMatRate;             //钓到渔场特产的概率
    public int nAddRandomRate;                 //钓到随机事件的概率

    public void Clear()
    {
        nAddFishRate = 0;
        nAddBigFishRate = 0;
        nAddFishRareRate = 0;
        nAddZawuRareRate = 0;
        nAddFishMatRareRate = 0;
        nAddFishSpeed = 0;
        nAddFishMatRate = 0;
        nAddRandomRate = 0;
    }

    public void AddNewInfo(AddInfo info)
    {
        nAddFishRate += info.nAddFishRate;
        nAddBigFishRate += info.nAddBigFishRate;
        nAddFishRareRate += info.nAddFishRareRate;
        nAddZawuRareRate += info.nAddZawuRareRate;
        nAddFishMatRareRate += info.nAddFishMatRareRate;
        nAddFishSpeed += info.nAddFishSpeed;
        nAddFishMatRate += info.nAddFishMatRate;
    }

}

public enum EMGiftType
{
    fishBait = 101,     //鱼饵
    fishGan = 201,      //鱼竿
    fishPiao = 301,     //浮漂
    fishLun = 401,      //飞轮
    fishXian = 501,     //鱼线
}

public class ST_GiftInfo : CTBLConfigSlot
{
    public string szName;
    public AddInfo pAddInfo;
    public string szDes;
    public EMGiftType emGiftType;

    public override void InitByLoader(CTBLLoader loader)
    {
        nID = loader.GetIntByName("id");
        emGiftType = (EMGiftType)nID;
        szName = loader.GetStringByName("name");
        pAddInfo = new AddInfo();
        pAddInfo.nAddFishRate = loader.GetIntByName("addfishrate");
        pAddInfo.nAddBigFishRate = loader.GetIntByName("addbigfishrate");
        pAddInfo.nAddFishRareRate = loader.GetIntByName("addfishrarerate");
        pAddInfo.nAddZawuRareRate = loader.GetIntByName("addzawurarerate");
        pAddInfo.nAddFishMatRareRate = loader.GetIntByName("addfishmatrarerate");
        pAddInfo.nAddFishSpeed = loader.GetIntByName("addfishspeed");
        pAddInfo.nAddFishMatRate = loader.GetIntByName("addfishmatrate");
        szDes = loader.GetStringByName("des");
    }
}

[CTBLConfigAttri("GiftInfo")]
public class CTBLHandlerGiftInfo : CTBLConfigBaseWithDic<ST_GiftInfo>
{

}