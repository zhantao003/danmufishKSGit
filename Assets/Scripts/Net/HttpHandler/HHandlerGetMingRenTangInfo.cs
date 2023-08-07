using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class HHandlerGetMingRenTangInfo : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {

    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        
        string szStatus = pMsg.GetString("status");
        if (!szStatus.Equals("ok")) return;

        CLocalNetArrayMsg arrRankOuContent = pMsg.GetNetMsgArr("list");
        Debug.LogError("Count ===" + arrRankOuContent.GetSize());
        List<CMingRenTangInfoArr> listMingRenTangInfos = new List<CMingRenTangInfoArr>();
        for (int i = 0; i < arrRankOuContent.GetSize(); i++)
        {
            CLocalNetMsg msgRankSlot = arrRankOuContent.GetNetMsg(i);
            CMingRenTangInfoArr cMingRenTangInfo = new CMingRenTangInfoArr();
            cMingRenTangInfo.nSeason = msgRankSlot.GetInt("seasonId");
            //欧皇信息
            cMingRenTangInfo.pMingRenInfosByOuHuang = new CMingRenTangInfo[3];
            cMingRenTangInfo.pMingRenInfosByOuHuang[0] = new CMingRenTangInfo();
            cMingRenTangInfo.pMingRenInfosByOuHuang[0].szUID = msgRankSlot.GetString("ouUid1");
            cMingRenTangInfo.pMingRenInfosByOuHuang[0].szName = msgRankSlot.GetString("ouUser1Name");
            cMingRenTangInfo.pMingRenInfosByOuHuang[0].szHeadIcon = msgRankSlot.GetString("ouUser1HeadIcon");
            cMingRenTangInfo.pMingRenInfosByOuHuang[1] = new CMingRenTangInfo();
            cMingRenTangInfo.pMingRenInfosByOuHuang[1].szUID = msgRankSlot.GetString("ouUid2");
            cMingRenTangInfo.pMingRenInfosByOuHuang[1].szName = msgRankSlot.GetString("ouUser2Name");
            cMingRenTangInfo.pMingRenInfosByOuHuang[1].szHeadIcon = msgRankSlot.GetString("ouUser2HeadIcon");
            cMingRenTangInfo.pMingRenInfosByOuHuang[2] = new CMingRenTangInfo();
            cMingRenTangInfo.pMingRenInfosByOuHuang[2].szUID = msgRankSlot.GetString("ouUid3");
            cMingRenTangInfo.pMingRenInfosByOuHuang[2].szName = msgRankSlot.GetString("ouUser3Name");
            cMingRenTangInfo.pMingRenInfosByOuHuang[2].szHeadIcon = msgRankSlot.GetString("ouUser3HeadIcon");
            //富豪信息
            cMingRenTangInfo.pMingRenInfosByProfit = new CMingRenTangInfo[3];
            cMingRenTangInfo.pMingRenInfosByProfit[0] = new CMingRenTangInfo();
            cMingRenTangInfo.pMingRenInfosByProfit[0].szUID = msgRankSlot.GetString("richUid1");
            cMingRenTangInfo.pMingRenInfosByProfit[0].szName = msgRankSlot.GetString("richUser1Name");
            cMingRenTangInfo.pMingRenInfosByProfit[0].szHeadIcon = msgRankSlot.GetString("richUser1HeadIcon");
            cMingRenTangInfo.pMingRenInfosByProfit[1] = new CMingRenTangInfo();
            cMingRenTangInfo.pMingRenInfosByProfit[1].szUID = msgRankSlot.GetString("richUid2");
            cMingRenTangInfo.pMingRenInfosByProfit[1].szName = msgRankSlot.GetString("richUser2Name");
            cMingRenTangInfo.pMingRenInfosByProfit[1].szHeadIcon = msgRankSlot.GetString("richUser2HeadIcon");
            cMingRenTangInfo.pMingRenInfosByProfit[2] = new CMingRenTangInfo();
            cMingRenTangInfo.pMingRenInfosByProfit[2].szUID = msgRankSlot.GetString("richUid3");
            cMingRenTangInfo.pMingRenInfosByProfit[2].szName = msgRankSlot.GetString("richUser3Name");
            cMingRenTangInfo.pMingRenInfosByProfit[2].szHeadIcon = msgRankSlot.GetString("richUser3HeadIcon");

            listMingRenTangInfos.Add(cMingRenTangInfo);
        }
        listMingRenTangInfos.Sort((x, y) => x.nSeason.CompareTo(y.nSeason));

        UIURAvatarInfo uiURAvatarInfo = UIManager.Instance.GetUI(UIResType.URAvatarInfo) as UIURAvatarInfo;
        if(uiURAvatarInfo != null)
        {
            uiURAvatarInfo.InitInfo(listMingRenTangInfos);
        }
    }
}
