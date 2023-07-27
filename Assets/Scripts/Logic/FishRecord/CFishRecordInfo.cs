using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFishRecordSlot
{
    public int nFishId;    //鱼的ID
    public long nCount;    //数量
    public float fMaxSize = 0F; //最大尺寸
    public float fMinSize = 0F; //最小尺寸
}

public class CFishRecordInfo
{
    public string uid;
    public string userName;
    public string headIcon;
    public long guardianLv; //舰长等级

    public long nTotalCoin; //本场总营收

    public CFishRecordInfo(CPlayerBaseInfo player)
    {
        uid = player.uid;
        userName = player.userName;
        headIcon = player.userFace;
        guardianLv = player.guardLevel;
        nTotalCoin = 0;
    }

    Dictionary<int, CFishRecordSlot> dicRecords = new Dictionary<int, CFishRecordSlot>();

    public CFishRecordSlot GetRecord(int tbid)
    {
        CFishRecordSlot pRes = null;
        if(dicRecords.TryGetValue(tbid, out pRes))
        {

        }

        return pRes;
    }

    public void AddRecord(CFishRecordSlot record)
    {
        if(dicRecords.ContainsKey(record.nFishId))
        {
            dicRecords[record.nFishId] = record;
        }
        else
        {
            dicRecords.Add(record.nFishId, record);
        }
    }

    /// <summary>
    /// 获取所有钓到的鱼的讯息
    /// </summary>
    /// <returns></returns>
    public List<CFishRecordSlot> GetAllRecords()
    {
        List<CFishRecordSlot> listSlots = new List<CFishRecordSlot>();
        listSlots.AddRange(dicRecords.Values);

        return listSlots;
    }
}
