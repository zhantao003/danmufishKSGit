using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ҵ�Boss�˺�ͳ��
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
