using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ؼ�¼������Ϣ
/// </summary>
public class CPlayerLocalRankInfo 
{
    ///ͨ������
    public long uid;
    public long rank;
    public long value;
    public string userName;
    public string headIcon;

    ///
    public string szFishIcon;
    public string szFishName;
    public float fFishSize;


    public EMLocalRankType emRankType = EMLocalRankType.Max;

    public void LoadMsg(CLocalNetMsg localNetMsg,int nRank)
    {
        uid = localNetMsg.GetLong("useruid");
        userName = localNetMsg.GetString("username");
        headIcon = localNetMsg.GetString("headicon");
        rank = nRank;
        if(emRankType == EMLocalRankType.MapOuHuang)
        {
            value = localNetMsg.GetLong("fishprice");
            szFishIcon = localNetMsg.GetString("fishicon");
            szFishName = localNetMsg.GetString("fishname");
            fFishSize = localNetMsg.GetFloat("fishsize");
        }
        else if(emRankType == EMLocalRankType.MapCain)
        {
            value = localNetMsg.GetLong("totalprice");
        }
    }
}
