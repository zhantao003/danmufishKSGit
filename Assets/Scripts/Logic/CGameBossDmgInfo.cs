using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家打Boss伤害统计
/// </summary>
public class CGameBossDmgInfo
{
    public string nUid;
    public long nDmg;
    public long nBomberCount;
    public string szName;

    //public DelegateLLFuncCall dlgChgPoint;

    public void AddDmg(long dmg)
    {
        nDmg += dmg;

        //dlgChgPoint?.Invoke(nUid, nDmg);
    }

    public void AddBomber(long num)
    {
        nBomberCount += num;
    }
}
