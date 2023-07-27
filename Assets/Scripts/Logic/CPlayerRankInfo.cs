using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerRankInfo 
{
    public string uid;
    public long rank;
    public long value;
    public long roomId;
    public string userName;
    public string headIcon;
    public EMRankType emRankType = EMRankType.Max;

    //额外信息
    public long vtbUid = -1;
    public string vtbName;
    public string vtbIcon;

    public void InitMsg(CLocalNetMsg msg)
    {
        uid = msg.GetString("uid");
        rank = msg.GetLong("pos");
        roomId = msg.GetLong("roomId");
        userName = msg.GetString("nickName");
        headIcon = msg.GetString("headIcon");
        value = msg.GetLong("token");

        //额外信息
        vtbUid = msg.GetLong("vtbuid");
        vtbName = msg.GetString("vtbName");
        vtbIcon = msg.GetString("vtbIcon");
    }
}

//玩家结算排名信息
public class CPlayerResRankInfo
{
    public string uid;
    public long rank;
    public long value;
}