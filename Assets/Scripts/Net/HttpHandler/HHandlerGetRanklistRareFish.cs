using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HHandlerGetRanklistRareFish : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        UIManager.Instance.CloseUI(UIResType.NetWait);

        string szStatus = pMsg.GetString("status");
        if (!szStatus.Equals("ok")) return;

        int fishId = pMsg.GetInt("fishId");
        string uid = pMsg.GetString("uid");
        long roomId = pMsg.GetLong("roomId");
        long pos = pMsg.GetLong("pos");
        long value = pMsg.GetLong("token");

        CPlayerRankInfo pSelfInfo = new CPlayerRankInfo();
        pSelfInfo.uid = uid;
        pSelfInfo.roomId = roomId;
        pSelfInfo.userName = CPlayerMgr.Ins.pOwner.userName;
        pSelfInfo.headIcon = CPlayerMgr.Ins.pOwner.userFace;
        pSelfInfo.rank = pos;
        pSelfInfo.value = value;
        pSelfInfo.emRankType = EMRankType.RareFish;

        string szRankContent = pMsg.GetString("list").Replace("\\", "");
        CLocalNetArrayMsg arrRankContent = new CLocalNetArrayMsg(szRankContent);
        List<CPlayerRankInfo> listRankInfo = new List<CPlayerRankInfo>();
        for (int i = 0; i < arrRankContent.GetSize(); i++)
        {
            CLocalNetMsg msgRankSlot = arrRankContent.GetNetMsg(i);
            CPlayerRankInfo pRankInfo = new CPlayerRankInfo();
            pRankInfo.emRankType = EMRankType.RareFish;
            pRankInfo.InitMsg(msgRankSlot);

            listRankInfo.Add(pRankInfo);
        }

        CWorldRankInfoMgr.Ins.SetFishRankInfo(fishId, listRankInfo);

        //UIRankInfo uiRank = UIManager.Instance.GetUI(UIResType.RankList) as UIRankInfo;
        //if (uiRank != null && uiRank.IsOpen())
        //{
        //    uiRank.SetFishRankInfo(fishId, listRankInfo);

        //    if (uiRank.emRankType == EMRankType.RareFish &&
        //        uiRank.nCurFishID == fishId)
        //    {
        //        uiRank.RefreshList(listRankInfo);
        //        uiRank.SetSelfFishInfo(fishId, pSelfInfo);
        //    }
        //}
    }
}
