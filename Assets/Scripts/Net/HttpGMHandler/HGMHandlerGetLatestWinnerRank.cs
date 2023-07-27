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

        Debug.Log("ŷ�����У�" + listRankOuInfo[0].rank);
        Debug.Log("�������У�" + listRankRicherInfo[0].rank);

        string szContent = "ŷ�ʰ�\r\n";
        for(int i=0; i<3; i++)
        {
            szContent += $"��{listRankOuInfo[i].rank}��  " + "UID��" + listRankOuInfo[i].uid + "  �ǳƣ�" + listRankOuInfo[i].userName + "  ͷ��" + listRankOuInfo[i].headIcon + "\r\n";
        }

        szContent += "������\r\n";
        for (int i = 0; i < 3; i++)
        {
            szContent += $"��{listRankRicherInfo[i].rank}��  " + "UID��" + listRankRicherInfo[i].uid + "  �ǳƣ�" + listRankRicherInfo[i].userName + "  ͷ��" + listRankRicherInfo[i].headIcon + "\r\n";
        }

        //�����ļ�
        LocalFileManage.ExportFile(szContent, "���а�����");
    }
}
