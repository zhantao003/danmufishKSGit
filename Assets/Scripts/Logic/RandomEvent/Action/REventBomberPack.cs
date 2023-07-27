using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CRandomEventAttri(4)]
public class REventBomberPack : CRandomEventAction
{
    public override void DoAction(CPlayerBaseInfo player)
    {
        if (player == null) return;

        ///根据uid获取玩家单位
        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(player.uid);
        if (pUnit == null)
        {
            pUnit = CPlayerMgr.Ins.GetActiveUnit(player.uid);
            if (pUnit == null)
                return;
        }

        pUnit.AddBoom(1, false);
    }
}
