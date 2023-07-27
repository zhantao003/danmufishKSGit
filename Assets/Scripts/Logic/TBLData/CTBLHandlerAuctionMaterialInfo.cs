using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 材料类型
/// </summary>
public enum EMMaterialType
{
    Material = 1,           ///当前地图材料
    FishPack,               ///渔具套装
    FishLun,                ///飞轮
    BBBoom,                 ///蹦蹦炸弹
    HaiDaoGold,             ///海盗金币
    Role,                   ///角色
    Boat,                   ///船
    TargetMaterial,         ///指定材料9901
}

/// <summary>
/// 付费类型
/// </summary>
public enum EMPayType
{
    FishCoin = 1,       ///鱼币
    HaiDaoCoin,     ///海盗金币
    Mat,            ///材料
}

public class CMaterialInfo
{
    public EMMaterialType emType;
    public long nlCount;

}

public class ST_AuctionMaterialInfo : CTBLConfigSlot
{
    public string szName;                   //名字
    public string szIcon;                   //图片
    public EMMaterialType emMatType;        //商品类型
    public int nMatNum;                     //商品数量
    public int nMatID;                      //商品ID
    public EMPayType emPayType;             //付费类型
    public int nPayID;                      //付费物品相关ID
    public string szBuyPrice;               //起拍价格系数（数量*系数=起拍价格）
    public string szBuyRange;               //起拍价格范围
    public int nWeight;                     //权重
    public int nTime;                       //竞拍时间
    public int nAddPrice;                   //单次加价

    public override void InitByLoader(CTBLLoader loader)
    {
        nID = loader.GetIntByName("id");
        szName = loader.GetStringByName("name");
        szIcon = loader.GetStringByName("icon");
        emMatType = (EMMaterialType)loader.GetIntByName("mattype");
        nMatID = loader.GetIntByName("matid");
        nMatNum = loader.GetIntByName("matnum");
        emPayType = (EMPayType)loader.GetIntByName("paytype");
        nPayID = loader.GetIntByName("payid");
        szBuyPrice = loader.GetStringByName("buyprice");
        szBuyRange = loader.GetStringByName("buyrange");
        nWeight = loader.GetIntByName("weight");
        nTime = loader.GetIntByName("time");
        nAddPrice = loader.GetIntByName("addprice");
    }

    /// <summary>
    /// 获取起拍价格
    /// </summary>
    /// <returns></returns>
    public int GetBuyPrice()
    {
        int buyPrice = 0;
        int nBuyPrice = 0;
        ///计算鱼/渔具的总数
        string[] szInfos = szBuyPrice.Split('|');
        int nMinCount = int.Parse(szInfos[0]);
        int nMaxCount = int.Parse(szInfos[1]);
        nBuyPrice = Random.Range(nMinCount, nMaxCount + 1);
        buyPrice = nBuyPrice;

        return buyPrice;
    }

    public int GetPriceByBuyRange(int price)
    {
        int nPrice = 0;

        string[] szInfos = szBuyRange.Split('|');
        int nMinCount = int.Parse(szInfos[0]);
        int nMaxCount = int.Parse(szInfos[1]);
        int nRangeValue = Random.Range(nMinCount, nMaxCount + 1);
        nPrice = System.Convert.ToInt32((float)price * (1f + (float)nRangeValue * 0.0001f));

        return nPrice;
    }


}

[CTBLConfigAttri("AuctionMaterialInfo")]
public class CTBLHandlerAuctionMaterialInfo : CTBLConfigBaseWithDic<ST_AuctionMaterialInfo>
{
}