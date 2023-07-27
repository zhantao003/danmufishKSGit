using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuCmdAttrite(CDanmuEventConst.FaCai)]
public class DCmdFaCai : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        return;
        string uid = dm.uid.ToString();

        CPlayerBaseInfo pPlayerInfo = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayerInfo == null)
            return;

        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if (pUnit == null)
        {
            pUnit = CPlayerMgr.Ins.GetActiveUnit(uid);
            if (pUnit == null)
                return;
        }

        pUnit.PlayGiftEffectToTarget(1);

        return;
    }
}

