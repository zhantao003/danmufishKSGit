using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CGetChampionRule
{
    public Vector2 vCountRange;
    public int[] nGetChampion;
}

public class CChampionMgr : MonoBehaviour
{
    static CChampionMgr ins = null;

    public static CChampionMgr Ins
    {
        get
        {
            return ins;
        }
    }

    public CGetChampionRule[] getChampionRules;

    private void Awake()
    {
        ins = this;
    }

    public CGetChampionRule GetCurRule()
    {
        CGetChampionRule curRule = null;

        int nPlayerCount = CPlayerMgr.Ins.GetAllIdleUnit().Count;

        for(int i =0;i < getChampionRules.Length;i++)
        {
            if(nPlayerCount >= getChampionRules[i].vCountRange.x &&
               nPlayerCount < getChampionRules[i].vCountRange.y)
            {
                curRule = getChampionRules[i];
            }
        }

        return curRule;
    }
}
