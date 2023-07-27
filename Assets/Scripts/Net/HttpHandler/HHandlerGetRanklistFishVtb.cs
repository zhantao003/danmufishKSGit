using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class HHandlerGetRanklistFishVtb : INetEventHandler
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

        long vtbUid = pMsg.GetLong("vtbUid");
        long vtbSize = pMsg.GetLong("size");
        string szVtbName = pMsg.GetString("vtbName");
        string szVtbIcon = pMsg.GetString("vtbIcon");

        CPlayerRankInfo pSelfInfo = new CPlayerRankInfo();
        pSelfInfo.uid = uid;
        pSelfInfo.roomId = roomId;
        pSelfInfo.userName = CPlayerMgr.Ins.pOwner.userName;
        pSelfInfo.headIcon = CPlayerMgr.Ins.pOwner.userFace;
        pSelfInfo.rank = pos;
        pSelfInfo.value = vtbSize;
        pSelfInfo.vtbUid = vtbUid;
        pSelfInfo.vtbIcon = szVtbIcon;
        pSelfInfo.vtbName = szVtbName;
        pSelfInfo.emRankType = EMRankType.FishVtb;

        string szRankContent = pMsg.GetString("list").Replace("\\", "");
        CLocalNetArrayMsg arrRankContent = new CLocalNetArrayMsg(szRankContent);
        List<CPlayerRankInfo> listRankInfo = new List<CPlayerRankInfo>();
        for (int i = 0; i < arrRankContent.GetSize(); i++)
        {
            CLocalNetMsg msgRankSlot = arrRankContent.GetNetMsg(i);
            CPlayerRankInfo pRankInfo = new CPlayerRankInfo();
            pRankInfo.emRankType = EMRankType.MapCain;
            pRankInfo.InitMsg(msgRankSlot);

            listRankInfo.Add(pRankInfo);
        }

        CWorldRankInfoMgr.Ins.SetCommonRankInfo(EMRankType.FishVtb, listRankInfo);

        //UIRankInfo uiRank = UIManager.Instance.GetUI(UIResType.RankList) as UIRankInfo;
        //if (uiRank != null && uiRank.IsOpen())
        //{
        //    uiRank.SetCommonRankInfo(EMRankType.FishVtb, listRankInfo);

        //    if (uiRank.emRankType == EMRankType.FishVtb)
        //    {
        //        uiRank.RefreshList(listRankInfo);
        //        uiRank.SetSelfCommonInfo(EMRankType.FishVtb, pSelfInfo);
        //    }
        //}
    }
}
