using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HGMHandlerGetLatestWinnerRankSlot : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        CLocalNetArrayMsg arrRankOuContent = pMsg.GetNetMsgArr("rankOu");
        List<CPlayerRankInfo> listRankOuInfo = new List<CPlayerRankInfo>();
        for (int i = 0; i < arrRankOuContent.GetSize(); i++)
        {
            CLocalNetMsg msgRankSlot = arrRankOuContent.GetNetMsg(i);
            CPlayerRankInfo pRankInfo = new CPlayerRankInfo();
            pRankInfo.emRankType = EMRankType.WinnerOuhuang;
            pRankInfo.InitMsg(msgRankSlot);

            listRankOuInfo.Add(pRankInfo);
        }

        CLocalNetArrayMsg arrRankRicherContent = pMsg.GetNetMsgArr("rankRicher");
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
        for (int i = 0; i < 3; i++)
        {
            szContent += $"第{listRankOuInfo[i].rank}名  " + "UID：" + listRankOuInfo[i].uid + "  昵称：" + listRankOuInfo[i].userName + "  头像：" + listRankOuInfo[i].headIcon + "\r\n";
        }

        szContent += "富豪榜单\r\n";
        for (int i = 0; i < 3; i++)
        {
            szContent += $"第{listRankRicherInfo[i].rank}名  " + "UID：" + listRankRicherInfo[i].uid + "  昵称：" + listRankRicherInfo[i].userName + "  头像：" + listRankRicherInfo[i].headIcon + "\r\n";
        }

        //初始化名人堂设定的UI
        UIGMConsoleCard uiCard = GameObject.FindObjectOfType<UIGMConsoleCard>();
        if(uiCard!=null)
        {
            UIGMMingrentang uiGMMingrentang = uiCard.objMingrentang.GetComponent<UIGMMingrentang>();
            if(uiGMMingrentang!=null)
            {
                uiGMMingrentang.uiInputOuUID1.text = listRankOuInfo[0].uid;
                uiGMMingrentang.uiInputOuUID2.text = listRankOuInfo[1].uid;
                uiGMMingrentang.uiInputOuUID3.text = listRankOuInfo[2].uid;

                uiGMMingrentang.uiInputOuName1.text = listRankOuInfo[0].userName;
                uiGMMingrentang.uiInputOuName2.text = listRankOuInfo[1].userName;
                uiGMMingrentang.uiInputOuName3.text = listRankOuInfo[2].userName;

                uiGMMingrentang.uiInputOuHeadIcon1.text = listRankOuInfo[0].headIcon;
                uiGMMingrentang.uiInputOuHeadIcon2.text = listRankOuInfo[1].headIcon;
                uiGMMingrentang.uiInputOuHeadIcon3.text = listRankOuInfo[2].headIcon;

                uiGMMingrentang.uiInputRichUID1.text = listRankRicherInfo[0].uid;
                uiGMMingrentang.uiInputRichUID2.text = listRankRicherInfo[1].uid;
                uiGMMingrentang.uiInputRichUID3.text = listRankRicherInfo[2].uid;

                uiGMMingrentang.uiInputRichName1.text = listRankRicherInfo[0].userName;
                uiGMMingrentang.uiInputRichName2.text = listRankRicherInfo[1].userName;
                uiGMMingrentang.uiInputRichName3.text = listRankRicherInfo[2].userName;

                uiGMMingrentang.uiInputRichHeadIcon1.text = listRankRicherInfo[0].headIcon;
                uiGMMingrentang.uiInputRichHeadIcon2.text = listRankRicherInfo[1].headIcon;
                uiGMMingrentang.uiInputRichHeadIcon3.text = listRankRicherInfo[2].headIcon;
            }
        }

        //导出文件
        LocalFileManage.ExportFile(szContent, "排行榜数据");
    }
}
