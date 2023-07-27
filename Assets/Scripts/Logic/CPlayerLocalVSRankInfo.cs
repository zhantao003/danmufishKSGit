using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerLocalVSRankInfo
{
    ///通用数据
    public long uid;
    public long rank;
    public string userName;
    public string headIcon;

    public long bomberCount;
    public int countLv1;
    public int countLv2;
    public int countLv3;
    public int countLv4;

    public void LoadMsg(CLocalNetMsg localNetMsg, int nRank)
    {
        uid = localNetMsg.GetLong("useruid");
        bomberCount = localNetMsg.GetLong("bomber");
        userName = localNetMsg.GetString("username");
        headIcon = localNetMsg.GetString("headicon");
        rank = nRank;

        countLv1 = localNetMsg.GetInt("lv1count");
        countLv2 = localNetMsg.GetInt("lv2count");
        countLv3 = localNetMsg.GetInt("lv3count");
        countLv4 = localNetMsg.GetInt("lv4count");
    }
}
