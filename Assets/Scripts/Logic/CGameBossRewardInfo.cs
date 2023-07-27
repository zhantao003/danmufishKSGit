using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BossÕ½½±Àø·â×°
/// </summary>
public class CGameBossRewardInfo
{
    public string nUid;
    public long nItemId;
    public long nAddNum;
    public int nDailyGet;
    public int nShowDmgRate;
    public long nlGetFishCoin;
    public CGameMapDropAvatarSlot dropAvatarInfo;

    public CLocalNetMsg ToMsg()
    {
        CLocalNetMsg msgRes = new CLocalNetMsg();
        msgRes.SetString("uid", nUid);
        msgRes.SetLong("itemId", nItemId);
        msgRes.SetLong("count", nAddNum);
        msgRes.SetInt("dailyGet", nDailyGet);

        return msgRes;
    }
}
