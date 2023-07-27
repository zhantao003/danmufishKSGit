using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CRandomEventAttri(3)]
public class REventSuipianPack : CRandomEventAction
{
    public override void DoAction(CPlayerBaseInfo player)
    {
        if (player == null) return;

        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(player.uid);
        if (pUnit == null) return;
        pUnit.AddPifuChip(5);
    }
}
