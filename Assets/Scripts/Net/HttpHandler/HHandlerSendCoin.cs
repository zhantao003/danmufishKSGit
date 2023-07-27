using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HHandlerSendCoin : INetEventHandler
{
    public long nTotalCoin;

    public HHandlerSendCoin(long value)
    {
        nTotalCoin = value;
    }

    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        if (pMsg.GetString("status") == "error")
        {
            UIToast.Show("积分不够,撒不了");
            return;
        }

        string uid = pMsg.GetString("uid");
        long nlRoomID = pMsg.GetLong("roomId");
        bool bVtb = pMsg.GetInt("isVtb") > 0;
        long gameCoin = pMsg.GetLong("fishCoin");

        //刷新玩家的钱
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer != null)
        {
            pPlayer.GameCoins = gameCoin;
        }

        CPlayerUnit pPlayerUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if(pPlayerUnit == null)
        {
            return;
        }

        long nAverageAdd = 0;
        int nPlayerNum = 0;
        List<CPlayerUnit> listAllPlayers = CPlayerMgr.Ins.GetAllIdleUnit();
        listAllPlayers.RemoveAll(x => x.uid == uid);
        if(listAllPlayers.Count >= 5)
        {
            nPlayerNum = 5;
            nAverageAdd = nTotalCoin / nPlayerNum;
        }
        else if(listAllPlayers.Count >= 1)
        {
            nPlayerNum = listAllPlayers.Count;
            nAverageAdd = nTotalCoin / nPlayerNum;
        }

        if (nAverageAdd <= 0) return;

        for(int i=0; i<nPlayerNum; i++)
        {
            if (listAllPlayers.Count <= 0) break;

            int nIdx = Random.Range(0, listAllPlayers.Count);

            CPlayerUnit pTargetUnit = listAllPlayers[nIdx];
            listAllPlayers.RemoveAt(nIdx);
            if (pTargetUnit == null) continue;
            
            pPlayerUnit.PlayGoldEffectToTarget(pTargetUnit.tranSelf, delegate ()
            {
                if (pTargetUnit == null) return;

                pTargetUnit.AddCoinByHttp(nAverageAdd, EMFishCoinAddFunc.Pay, false, false);
                pTargetUnit.dlgOnlySetAddCoin?.Invoke(nAverageAdd, 0);
                pTargetUnit.dlgShowAddCoin?.Invoke();
            });
        }
    }
}
