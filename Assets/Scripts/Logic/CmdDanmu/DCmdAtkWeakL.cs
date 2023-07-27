using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuCmdAttrite(CDanmuEventConst.AtkWeakL)]
public class DCmdAtkWeakL : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        string uid = dm.uid.ToString();

        //如果是Boss战开始后不让上岸
        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
        {
            if (CGameBossMgr.Ins != null &&
            CGameBossMgr.Ins.emCurState != CGameBossMgr.EMState.Gaming)
            {
                return;
            }

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

            pUnit.nAtkIdx = 2;
        }

        return;
    }
}
