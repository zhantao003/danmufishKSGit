using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerFishGanInfo
{
    public int nGanId;
    public int nLv;     //当前等级
    public int nExp;    //当前经验值
    public EMAddUnitProType emPro;
    public int nProAdd;

    public bool bHave;
}

public class CPlayerFishGanPack
{
    public List<CPlayerFishGanInfo> listInfos = new List<CPlayerFishGanInfo>();

    public void AddInfo(CPlayerFishGanInfo info)
    {
        RemoveInfo(info.nGanId);

        listInfos.Add(info);
    }

    public CPlayerFishGanInfo GetInfo(int id)
    {
        for (int i = 0; i < listInfos.Count; i++)
        {
            if (listInfos[i].nGanId == id)
            {
                return listInfos[i];
            }
        }

        return null;
    }

    /// <summary>
    /// 获取指定皮肤ID的下一个皮肤
    /// </summary>
    /// <param name="avatarId"></param>
    /// <returns></returns>
    public CPlayerFishGanInfo GetNexBoat(long avatarId)
    {
        CPlayerFishGanInfo pRes = null;
        if (listInfos.Count <= 0) return null;

        int nTargetIdx = -1;
        for (int i = 0; i < listInfos.Count; i++)
        {
            if (listInfos[i].nGanId == avatarId)
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

    public void SortGanPack()
    {
        listInfos.Sort((x, y) =>
        {
            if (x == null) return -1;
            if (y == null) return 1;

            ST_UnitFishGan pTBLX = CTBLHandlerUnitFishGan.Ins.GetInfo(x.nGanId);
            ST_UnitFishGan pTBLY = CTBLHandlerUnitFishGan.Ins.GetInfo(y.nGanId);
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

    public void RemoveInfo(int id)
    {
        for (int i = 0; i < listInfos.Count; i++)
        {
            if (listInfos[i].nGanId == id)
            {
                listInfos.RemoveAt(i);
                return;
            }
        }
    }

    public void Clear()
    {
        listInfos.Clear();
    }
}
