using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAuctionInfo
{
    public CFishPackInfo fishPackInfo;
    public CFishInfo fishInfo;
    public COtherDropInfo otherDropInfo;
}

public enum EMAuctionLimitType
{
    Single,
    Multiple
}

public class COtherDropInfo
{
    /// <summary>
    /// 0 -- ��ɫ / 1 -- ��
    /// </summary>
    public int nType;
    public int nValue;
    public int nWeight;
}

public class CFishPackInfo
{
    public int nCount;
}

public class ST_AuctionTreasureInfo : CTBLConfigSlot
{
    public string szName;                   //����
    public string szIcon;                   //ͼƬ
    public EMAuctionLimitType emAuctionLimitType; //������������
    public EMAuctionBoxType nType;          //����
    public string szTBLName;                //��ȡ�ı���
    public string szPrefab;                 //Ԥ����
    public string szBuyPrice;                   //���ļ۸�ϵ��������*ϵ��=���ļ۸�
    public string szNum;                    //����ص�����
    public string szNormalRange;            //��ֵ��Χ
    public string szBuyRange;               //���ļ۸�Χ
    public int nOtherRate;                  //��ɫ/���ĸ���
    public string szOtherDrop;              //��ɫ/���ĵ����
    public int nWeight;                     //Ȩ��
    public int nTime;                       //����ʱ��
    public int nAddPrice;                   //���μӼ�
    public int nCri;                        //�������ʣ����������������

    public CLocalNetArrayMsg arrOtherDrop;

    public override void InitByLoader(CTBLLoader loader)
    {
        nID = loader.GetIntByName("id");
        szName = loader.GetStringByName("name");
        szIcon = loader.GetStringByName("icon");
        nTime = loader.GetIntByName("time");
        emAuctionLimitType = (EMAuctionLimitType)loader.GetIntByName("type");
        nType = (EMAuctionBoxType)loader.GetIntByName("lev");
        szTBLName = loader.GetStringByName("tblname");
        szPrefab = loader.GetStringByName("prefab");
        nAddPrice = loader.GetIntByName("addprice");
        nCri = loader.GetIntByName("cri");
        szNormalRange = loader.GetStringByName("normalrange");
        szBuyRange = loader.GetStringByName("buyrange");
        szBuyPrice = loader.GetStringByName("buyprice");
        szNum = loader.GetStringByName("num");
        nOtherRate = loader.GetIntByName("otherrate");
        szOtherDrop = loader.GetStringByName("otherdrop");
        nWeight = loader.GetIntByName("weight");

        if (!CHelpTools.IsStringEmptyOrNone(szOtherDrop))
        {
            CLocalNetMsg pDropPackMsg = new CLocalNetMsg(szOtherDrop);
           
            arrOtherDrop = pDropPackMsg.GetNetMsgArr("data");
        }
        else
        {
            arrOtherDrop = null;
        }
    }

    /// <summary>
    /// ��ȡ���ļ۸�
    /// </summary>
    /// <returns></returns>
    public int GetBuyPrice(out int nCount,out bool bCri)
    {
        int buyPrice = 0;
        int nBuyPrice = 0;

        ///������/��ߵ�����
        string[] szInfos = szBuyPrice.Split('|');
        int nMinCount = int.Parse(szInfos[0]);
        int nMaxCount = int.Parse(szInfos[1]);
        nBuyPrice = Random.Range(nMinCount, nMaxCount + 1);
        ///������/��ߵ�����
        szInfos = szNum.Split('|');
        nMinCount = int.Parse(szInfos[0]);
        nMaxCount = int.Parse(szInfos[1]);
        nCount = Random.Range(nMinCount, nMaxCount + 1);
        bCri = false;
        int nRandomValue = Random.Range(0, 10001);
        if (nRandomValue <= nCri)
        {
            bCri = true;
        }
        if (bCri)
        {
            nCount = nCount * 2;
        }
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

    public int GetCountByNormalRange(int price)
    {
        int nPrice = 0;

        string[] szInfos = szNormalRange.Split('|');
        int nMinCount = int.Parse(szInfos[0]);
        int nMaxCount = int.Parse(szInfos[1]);
        int nRangeValue = Random.Range(nMinCount, nMaxCount + 1);
        nPrice = System.Convert.ToInt32((float)price * (1f + nRangeValue * 0.0001f));

        return nPrice;
    }

    /// <summary>
    /// ��ȡ�����Ϣ
    /// </summary>
    /// <returns></returns>
    public List<CFishInfo> GetFish(int nCount)
    {
        ///��ȡ��/��ߵ���Ϣ
        List<CFishInfo> listFishInfo = new List<CFishInfo>();
        for (int i = 0; i < nCount; i++)
        {
            CFishInfo fishInfo = new CFishInfo(CAuctionMgr.Ins.GetFishHandler(nID).GetTargetFishInfoByType(), 0);
            //CFishInfo fishInfo = new CFishInfo(CGameColorFishMgr.Ins.pMap.pTBLHandlerFishInfo.GetTargetFishInfoByType(), 0);
            listFishInfo.Add(fishInfo);
        }

        return listFishInfo;
    }

    /// <summary>
    /// �ж��Ƿ��ȡ��ɫ/������Ϣ
    /// </summary>
    /// <returns></returns>
    public bool CheckGetOther()
    {
        bool bGetOther = false;

        int nRandomValue = Random.Range(0, 10001);
        if(nRandomValue <= nOtherRate)
        {
            bGetOther = true;
        }

        return bGetOther;
    }

    /// <summary>
    /// ��ȡ��ɫ/������Ϣ
    /// </summary>
    /// <returns></returns>
    public COtherDropInfo GetOther()
    {
        COtherDropInfo pGetInfo = null;
        int nTotalWeight = 0;
        List<COtherDropInfo> listDropInfos = new List<COtherDropInfo>();
        for (int i = 0; i < arrOtherDrop.GetSize(); i++)
        {
            CLocalNetMsg localNetMsg = arrOtherDrop.GetNetMsg(i);
            int nType = localNetMsg.GetInt("type");
            int nValue = localNetMsg.GetInt("value");
            int nWeight = localNetMsg.GetInt("weight");
            COtherDropInfo otherDropInfo = new COtherDropInfo();
            otherDropInfo.nType = nType;
            otherDropInfo.nValue = nValue;
            otherDropInfo.nWeight = nWeight;
            listDropInfos.Add(otherDropInfo);
            nTotalWeight += nWeight;
        }

        int nRandomWeight = Random.Range(0, nTotalWeight + 1);

        for(int i = 0;i < listDropInfos.Count;i++)
        {
            if (listDropInfos[i].nWeight <= 0)
                continue;
            nRandomWeight -= listDropInfos[i].nWeight;
            if (nRandomWeight > 0)
                continue;
            
            pGetInfo = listDropInfos[i];
            break;
        }

        return pGetInfo;
    }

}

[CTBLConfigAttri("AuctionTreasureInfo")]
public class CTBLHandlerAuctionTreasureInfo : CTBLConfigBaseWithDic<ST_AuctionTreasureInfo>
{
}
