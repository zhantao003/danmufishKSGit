using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.AddSceneExp)]
public class HHandlerAddSceneExp : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {

    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string szState = pMsg.GetString("status");
        if (!szState.Equals("ok")) return;

        //Debug.Log("Add Scene Msg ====" +  pMsg.GetData());
        long nlExp = pMsg.GetLong("fishMapExp");
        int nLv = pMsg.GetInt("fishMapLv");
        if (CGameColorFishMgr.Ins == null) return;

        CGameColorFishMgr.Ins.nCurRateUpLv = nLv;
        CGameColorFishMgr.Ins.CurMapExp = nlExp;
        //long uid = pMsg.GetLong("uid");
        //long nlRoomID = pMsg.GetLong("roomId");
        Debug.Log("CurLv:" + nLv + "=====CurExp:" + nlExp);
        //CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        //if (pPlayer != null)
        //{
        //    pPlayer.GameCoins = gameCoin;
        //}

        //Debug.Log(uid + "=====Game Coin =====" + gameCoin);

    }
}
