using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerAvatarInfo
{
    public int nAvatarId;
    public int nPart;
    public long nExTime;
    public bool bHave = true;

    public string GetExTime()
    {
        string szRes = "";
        long nLerp = nExTime - CTimeMgr.NowMillonsSec();
        if(nLerp <= 0)
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

/// <summary>
/// 玩家背包
/// </summary>
public class CPlayerAvatarPack
{
    public List<CPlayerAvatarInfo> listAvatars = new List<CPlayerAvatarInfo>();

    public void AddInfo(CPlayerAvatarInfo info)
    {
        RemoveInfo(info.nAvatarId);

        listAvatars.Add(info);
    }

    public CPlayerAvatarInfo GetInfo(int id)
    {
        for(int i=0; i<listAvatars.Count; i++)
        {
            if(listAvatars[i].nAvatarId == id)
            {
                return listAvatars[i];
            }
        }

        return null;
    }

    /// <summary>
    /// 获取一个随机的皮肤
    /// </summary>
    /// <param name="expectAvatarId"></param>
    /// <returns></returns>
    public CPlayerAvatarInfo GetRandInfo(long expectAvatarId = 0)
    {
        if (listAvatars.Count <= 0) return null;

        List<CPlayerAvatarInfo> listRand = new List<CPlayerAvatarInfo>();
        listRand.AddRange(listAvatars);
        for(int i=0; i<listRand.Count; )
        {
            if(expectAvatarId == listRand[i].nAvatarId)
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
    public CPlayerAvatarInfo GetNextAvatar(long avatarId)
    {
        CPlayerAvatarInfo pRes = null;
        if (listAvatars.Count <= 0) return null;

        int nTargetIdx = -1;
        for (int i = 0; i < listAvatars.Count; i++)
        {
            if(listAvatars[i].nAvatarId == avatarId)
            {
                nTargetIdx = i + 1;
                break;
            }
        }

        if(nTargetIdx > listAvatars.Count -1)
        {
            nTargetIdx = 0;
        }

        pRes = listAvatars[nTargetIdx];

        return pRes;
    }

    public void RemoveInfo(int id)
    {
        for (int i = 0; i < listAvatars.Count; i++)
        {
            if (listAvatars[i].nAvatarId == id)
            {
                listAvatars.RemoveAt(i);
                return;
            }
        }
    }

    public void SortAvatarPack()
    {
        listAvatars.Sort((x, y) =>
        {
            ST_UnitAvatar pTBLX = CTBLHandlerUnitAvatar.Ins.GetInfo(x.nAvatarId);
            ST_UnitAvatar pTBLY = CTBLHandlerUnitAvatar.Ins.GetInfo(y.nAvatarId);
            if (pTBLX == null) return -1;
            if (pTBLY == null) return 1;

            if (pTBLX.emRare < pTBLY.emRare)
            {
                return 1;
            }
            else if (pTBLX.emRare == pTBLY.emRare)
            {
                if(pTBLX.nID < pTBLY.nID)
                {
                    return 1;
                }
                else if(pTBLX.nID == pTBLY.nID)
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
        });
    }

    public void Clear()
    {
        listAvatars.Clear();
    }
}
