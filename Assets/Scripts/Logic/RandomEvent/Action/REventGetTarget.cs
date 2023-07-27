using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CRandomEventAttri(5)]
public class REventGetTarget : CRandomEventAction
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

        long nlCoin = 0;
        ST_FishInfo fishInfo = CGameColorFishMgr.Ins.pMap.pTBLHandlerFishInfo.GetInfo(4000001);
        if (fishInfo == null)
            return;
        nlCoin = fishInfo.nPrice;
        if (nlCoin >= 0)
        {
            pUnit.AddCoinByHttp(nlCoin, EMFishCoinAddFunc.Free, false, false);
        }
    }
}
