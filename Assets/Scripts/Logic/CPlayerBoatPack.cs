using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerBoatInfo
{
    public int nBoatId;
    public long nExTime;
    public bool bHave;

    public string GetExTime()
    {
        string szRes = "";
        long nLerp = nExTime - CTimeMgr.NowMillonsSec();
        if (nLerp <= 0)
        {
            szRes = "(剩7天)";
        }
        else
        {
            int nDay = (int)(nLerp / (24 * 60 * 60 * 1000)) + 1;
            nDay = Mathf.Clamp(1, 7, nDay);
            szRes = $"(剩{nDay}天)";
        }

        return szRes;
    }
}

public class CPlayerBoatPack
{
    public List<CPlayerBoatInfo> listInfos = new List<CPlayerBoatInfo>();

    public void AddInfo(CPlayerBoatInfo info)
    {
        RemoveInfo(info.nBoatId);

        listInfos.Add(info);
    }

    public CPlayerBoatInfo GetInfo(int id)
    {
        for (int i = 0; i < listInfos.Count; i++)
        {
            if (listInfos[i].nBoatId == id)
            {
                return listInfos[i];
            }
        }

        return null;
    }

    /// <summary>
    /// 获取一个随机的皮肤
    /// </summary>
    /// <param name="expectAvatarId"></param>
    /// <returns></returns>
    public CPlayerBoatInfo GetRandInfo(long expectBoatId = 0)
    {
        if (listInfos.Count <= 0) return null;

        List<CPlayerBoatInfo> listRand = new List<CPlayerBoatInfo>();
        listRand.AddRange(listInfos);
        for (int i = 0; i < listRand.Count;)
        {
            if (expectBoatId == listRand[i].nBoatId)
            {
                listRand.RemoveAt(i);
                break;
            }
            else
            {
                i++;
            }
        }

        return listRand[Random.Range(0, listRand.Count)];
    }

    /// <summary>
    /// 获取指定皮肤ID的下一个皮肤
    /// </summary>
    /// <param name="avatarId"></param>
    /// <returns></returns>
    public CPlayerBoatInfo GetNexBoat(long avatarId)
    {
        CPlayerBoatInfo pRes = null;
        if (listInfos.Count <= 0) return null;

        int nTargetIdx = -1;
        for (int i = 0; i < listInfos.Count; i++)
        {
            if (listInfos[i].nBoatId == avatarId)
            {
                nTargetIdx = i + 1;
                break;
            }
        }

        if (nTargetIdx > listInfos.Count - 1 || 
            nTargetIdx < 0)
        {
            nTargetIdx = 0;
        }

        pRes = listInfos[nTargetIdx];

        return pRes;
    }

    public void RemoveInfo(int id)
    {
        for (int i = 0; i < listInfos.Count; i++)
        {
            if (listInfos[i].nBoatId == id)
            {
                listInfos.RemoveAt(i);
                return;
            }
        }
    }

    public void SortBoatPack()
    {
        listInfos.Sort((x, y) =>
        {
            if (x == null) return -1;
            if (y == null) return 1;

            ST_UnitFishBoat pTBLX = CTBLHandlerUnitFishBoat.Ins.GetInfo(x.nBoatId);
            ST_UnitFishBoat pTBLY = CTBLHandlerUnitFishBoat.Ins.GetInfo(y.nBoatId);
            if (pTBLX == null) return -1;
            if (pTBLY == null) return 1;

            if (pTBLX.emRare < pTBLY.emRare)
            {
                return 1;
            }
            else if (pTBLX.emRare == pTBLY.emRare)
            {
                if (pTBLX.nID < pTBLY.nID)
                {
                    return 1;
                }
                else if (pTBLX.nID == pTBLY.nID)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }

            return 0;
        });
    }

    public void Clear()
    {
        listInfos.Clear();
    }
}
