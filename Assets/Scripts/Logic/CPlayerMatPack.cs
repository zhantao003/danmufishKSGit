using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerMatPack
{
    public Dictionary<long, long> dicItemsPack = new Dictionary<long, long>();

    public void SetItem(long id, long num)
    {
        if (dicItemsPack.ContainsKey(id))
        {
            dicItemsPack[id] = num;
        }
        else
        {
            dicItemsPack.Add(id, num);
        }
    }

    public long GetItem(long id)
    {
        long nRes = -1;
        if(dicItemsPack.TryGetValue(id, out nRes))
        {

        }

        return nRes;
    }
}
