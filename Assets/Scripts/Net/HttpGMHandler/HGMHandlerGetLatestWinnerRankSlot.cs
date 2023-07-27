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

        Debug.Log("ŷ�����У�" + listRankOuInfo[0].rank);
        Debug.Log("�������У�" + listRankRicherInfo[0].rank);

        string szContent = "ŷ�ʰ�\r\n";
        for (int i = 0; i < 3; i++)
        {
            szContent += $"��{listRankOuInfo[i].rank}��  " + "UID��" + listRankOuInfo[i].uid + "  �ǳƣ�" + listRankOuInfo[i].userName + "  ͷ��" + listRankOuInfo[i].headIcon + "\r\n";
        }

        szContent += "������\r\n";
        for (int i = 0; i < 3; i++)
        {
            szContent += $"��{listRankRicherInfo[i].rank}��  " + "UID��" + listRankRicherInfo[i].uid + "  �ǳƣ�" + listRankRicherInfo[i].userName + "  ͷ��" + listRankRicherInfo[i].headIcon + "\r\n";
        }

        //��ʼ���������趨��UI
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

        //�����ļ�
        LocalFileManage.ExportFile(szContent, "���а�����");
    }
}
