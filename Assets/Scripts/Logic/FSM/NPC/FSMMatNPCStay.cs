using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMMatNPCStay : FSMNPCBase
{
    public CPropertyTimer pStayTick;

    CPropertyTimer pShowTick;

    public override void OnBegin(object obj)
    {
        pNPCUnit.emCurState = CNPCUnit.EMState.Stay;
        pNPCUnit.PlayAnime(CUnitAnimeConst.Anime_Idle);
        CAuctionMatMgr.Ins.StartAuction();
        float fStayTime = CAuctionMatMgr.Ins.curTreasureInfo.nTime * 0.001f; // pNPCUnit.fStayTime;
        pStayTick = new CPropertyTimer();
        pStayTick.Value = fStayTime;
        pStayTick.FillTime();
        pNPCUnit.deleAuctionCountDown?.Invoke(fStayTime);
    }

    public override void OnUpdate(object obj, float delta)
    {
        if (pStayTick != null)
        {
            if (pStayTick.Tick(delta))
            {
                pStayTick = null;
                pNPCUnit.SetState(CNPCUnit.EMState.Exit);
            }
            else
            {
                pNPCUnit.deleAuctionCountDown?.Invoke(pStayTick.CurValue);
            }
        }
    }



}
