using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������Ϣ
/// </summary>
[System.Serializable]
public class AddInfo
{
    public int nAddFishRate;                //������ĸ���
    public int nAddBigFishRate;             //��������ĸ���
    public int nAddFishRareRate;            //����ϡ�������͵ĸ���
    public int nAddZawuRareRate;            //����ϡ���������͵ĸ���
    public int nAddFishMatRareRate;            //����ϡ���泡�ز����͵ĸ���
    public int nAddFishSpeed;               //�һ�������ٶ�
    public int nAddFishMatRate;             //�����泡�ز��ĸ���
    public int nAddRandomRate;                 //��������¼��ĸ���

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
    fishBait = 101,     //���
    fishGan = 201,      //���
    fishPiao = 301,     //��Ư
    fishLun = 401,      //����
    fishXian = 501,     //����
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