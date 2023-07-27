using Crosstales.FB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class HGMHandlerGetLatestWinnerRank : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg) 
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string rankOu = pMsg.GetString("rankOu").Replace("\\", "");
        CLocalNetArrayMsg arrRankOuContent = new CLocalNetArrayMsg(rankOu);

        string rankRicher = pMsg.GetString("rankRicher").Replace("\\", "");
        CLocalNetArrayMsg arrRankRicherContent = new CLocalNetArrayMsg(rankRicher);

        List<CPlayerRankInfo> listRankOuInfo = new List<CPlayerRankInfo>();
        for (int i = 0; i < arrRankOuContent.GetSize(); i++)
        {
            CLocalNetMsg msgRankSlot = arrRankOuContent.GetNetMsg(i);
            CPlayerRankInfo pRankInfo = new CPlayerRankInfo();
            pRankInfo.emRankType = EMRankType.WinnerOuhuang;
            pRankInfo.InitMsg(msgRankSlot);

            listRankOuInfo.Add(pRankInfo);
        }

        List<CPlayerRankInfo> listRankRicherInfo = new List<CPlayerRankInfo>();
        for (int i = 0; i < arrRankRicherContent.GetSize(); i++)
        {
            CLocalNetMsg msgRankSlot = arrRankRicherContent.GetNetMsg(i);
            CPlayerRankInfo pRankInfo = new CPlayerRankInfo();
            pRankInfo.emRankType = EMRankType.WinnerRicher;
            pRankInfo.InitMsg(msgRankSlot);

            listRankRicherInfo.Add(pRankInfo);
        }

        listRankOuInfo.Sort((x, y) => x.rank.CompareTo(y.rank));
        listRankRicherInfo.Sort((x, y) => x.rank.CompareTo(y.rank));

        Debug.Log("欧皇排行：" + listRankOuInfo[0].rank);
        Debug.Log("富豪排行：" + listRankRicherInfo[0].rank);

        string szContent = "欧皇榜单\r\n";
        for(int i=0; i<3; i++)
        {
            szContent += $"第{listRankOuInfo[i].rank}名  " + "UID：" + listRankOuInfo[i].uid + "  昵称：" + listRankOuInfo[i].userName + "  头像：" + listRankOuInfo[i].headIcon + "\r\n";
        }

        szContent += "富豪榜单\r\n";
        for (int i = 0; i < 3; i++)
        {
            szContent += $"第{listRankRicherInfo[i].rank}名  " + "UID：" + listRankRicherInfo[i].uid + "  昵称：" + listRankRicherInfo[i].userName + "  头像：" + listRankRicherInfo[i].headIcon + "\r\n";
        }

        //导出文件
        LocalFileManage.ExportFile(szContent, "排行榜数据");
    }
}
