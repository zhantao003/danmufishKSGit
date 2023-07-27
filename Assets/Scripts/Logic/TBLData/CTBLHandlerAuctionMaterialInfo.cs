using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������
/// </summary>
public enum EMMaterialType
{
    Material = 1,           ///��ǰ��ͼ����
    FishPack,               ///�����װ
    FishLun,                ///����
    BBBoom,                 ///�ı�ը��
    HaiDaoGold,             ///�������
    Role,                   ///��ɫ
    Boat,                   ///��
    TargetMaterial,         ///ָ������9901
}

/// <summary>
/// ��������
/// </summary>
public enum EMPayType
{
    FishCoin = 1,       ///���
    HaiDaoCoin,     ///�������
    Mat,            ///����
}

public class CMaterialInfo
{
    public EMMaterialType emType;
    public long nlCount;

}

public class ST_AuctionMaterialInfo : CTBLConfigSlot
{
    public string szName;                   //����
    public string szIcon;                   //ͼƬ
    public EMMaterialType emMatType;        //��Ʒ����
    public int nMatNum;                     //��Ʒ����
    public int nMatID;                      //��ƷID
    public EMPayType emPayType;             //��������
    public int nPayID;                      //������Ʒ���ID
    public string szBuyPrice;               //���ļ۸�ϵ��������*ϵ��=���ļ۸�
    public string szBuyRange;               //���ļ۸�Χ
    public int nWeight;                     //Ȩ��
    public int nTime;                       //����ʱ��
    public int nAddPrice;                   //���μӼ�

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
    /// ��ȡ���ļ۸�
    /// </summary>
    /// <returns></returns>
    public int GetBuyPrice()
    {
        int buyPrice = 0;
        int nBuyPrice = 0;
        ///������/��ߵ�����
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