using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HHandlerGetRLFishOuhuangBySlot : INetEventHandler
{
    public DelegateNFuncCall callSuc;

    public HHandlerGetRLFishOuhuangBySlot(DelegateNFuncCall call)
    {
        callSuc = call;
    }

    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        callSuc?.Invoke();
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        UIManager.Instance.CloseUI(UIResType.NetWait);

        string szStatus = pMsg.GetString("status");
        if (!szStatus.Equals("ok"))
        {
            callSuc?.Invoke();
            return;
        }

        CLocalNetArrayMsg arrRankContent = pMsg.GetNetMsgArr("list");
        List<CPlayerResRankInfo> listRankInfo = new List<CPlayerResRankInfo>();
        for (int i = 0; i < arrRankContent.GetSize(); i++)
        {
            CLocalNetMsg msgRankSlot = arrRankContent.GetNetMsg(i);
            CPlayerResRankInfo pRankInfo = new CPlayerResRankInfo();
            pRankInfo.uid = msgRankSlot.GetString("uid");
            pRankInfo.rank = msgRankSlot.GetLong("pos");
            pRankInfo.value = msgRankSlot.GetLong("token");

            listRankInfo.Add(pRankInfo);
        }

        CWorldRankInfoMgr.Ins.SetResRankSlotList(EMRankType.WinnerOuhuang, listRankInfo);

        callSuc?.Invoke();
    }
}
