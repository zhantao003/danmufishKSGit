using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRoomRecordInfoMgr : CSingleMgrBase<CRoomRecordInfoMgr>
{
    /// <summary>
    /// �������������
    /// </summary>
    long nRoomGainCoin = 0;
    public DelegateLFuncCall dlgChgRoomGainCoin;
    public long RoomGainCoin
    {
        get
        {
            return nRoomGainCoin;
        }
        set
        {
            nRoomGainCoin = value;

        }
    }

    /// <summary>
    /// �����¼
    /// </summary>
    Dictionary<int, long> dicRareFishInfo = new Dictionary<int, long>();

    public void Init()
    {
        dicRareFishInfo.Clear();
    }

    public void SetFishInfo(int id, long count)
    {
        if(dicRareFishInfo.ContainsKey(id))
        {
            dicRareFishInfo[id] = count;
        }
        else
        {
            dicRareFishInfo.Add(id, count);
        }
    }

    public long GetFishCount(int id)
    {
        long num = 0;
        if(dicRareFishInfo.TryGetValue(id, out num))
        {
            
        }

        return num;
    }
}

