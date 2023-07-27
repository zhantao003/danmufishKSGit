using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.GetRankListMapLv)]
public class HHandlerGetRanklistMapLv : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        UIManager.Instance.CloseUI(UIResType.NetWait);

        string szStatus = pMsg.GetString("status");
        if (!szStatus.Equals("ok")) return;

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
        pSelfInfo.emRankType = EMRankType.MapLv;

        string szRankContent = pMsg.GetString("list").Replace("\\", "");
        CLocalNetArrayMsg arrRankContent = new CLocalNetArrayMsg(szRankContent);
        List<CPlayerRankInfo> listRankInfo = new List<CPlayerRankInfo>();
        for (int i = 0; i < arrRankContent.GetSize(); i++)
        {
            CLocalNetMsg msgRankSlot = arrRankContent.GetNetMsg(i);
            CPlayerRankInfo pRankInfo = new CPlayerRankInfo();
            pRankInfo.emRankType = EMRankType.MapLv;
            pRankInfo.InitMsg(msgRankSlot);

            listRankInfo.Add(pRankInfo);
        }

        CWorldRankInfoMgr.Ins.SetCommonRankInfo(EMRankType.MapLv, listRankInfo);

        //UIRankInfo uiRank = UIManager.Instance.GetUI(UIResType.RankList) as UIRankInfo;
        //if (uiRank != null && uiRank.IsOpen())
        //{
        //    uiRank.SetCommonRankInfo(EMRankType.MapLv, listRankInfo);

        //    if (uiRank.emRankType == EMRankType.MapLv)
        //    {
        //        uiRank.RefreshList(listRankInfo);
        //        uiRank.SetSelfCommonInfo(EMRankType.MapLv, pSelfInfo);
        //    }
        //}
    }
}
