using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HGMHandlerGachaGiftBox : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
       
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        UIGMConsole uiGMConsole = GameObject.FindObjectOfType<UIGMConsole>();
        if (uiGMConsole == null) return;

        string szStatus = pMsg.GetString("status");
        if(!szStatus.Equals("ok"))
        {
            uiGMConsole.uiLabelContent.text = pMsg.GetData();
            return;
        }

        string szLogRes = "抽奖结果:\r\n";
        string szContent = pMsg.GetString("gachaResult").Replace("\\", "");
        CLocalNetArrayMsg arrGachaContent = new CLocalNetArrayMsg(szContent);
        for (int i = 0; i < arrGachaContent.GetSize(); i++)
        {
            CLocalNetMsg msgGachaSlot = arrGachaContent.GetNetMsg(i);
            CGiftGachaBoxInfo pGachaSlotInfo = new CGiftGachaBoxInfo();
            pGachaSlotInfo.emType = (CGiftGachaBoxInfo.EMGiftType)(msgGachaSlot.GetInt("itemType"));
            pGachaSlotInfo.nItemID = msgGachaSlot.GetInt("itemId");
            if(pGachaSlotInfo.emType == CGiftGachaBoxInfo.EMGiftType.FishCoin)
            {
                szLogRes += "积分:" + pGachaSlotInfo.nItemID + "\r\n";
            }
            else if (pGachaSlotInfo.emType == CGiftGachaBoxInfo.EMGiftType.Yuju)
            {
                szLogRes += "加油鸭:" + pGachaSlotInfo.nItemID + "套\r\n";
            }
            else if (pGachaSlotInfo.emType == CGiftGachaBoxInfo.EMGiftType.Feilun)
            {
                szLogRes += "初级卷轴:" + pGachaSlotInfo.nItemID + "个\r\n";
            }
            else if (pGachaSlotInfo.emType == CGiftGachaBoxInfo.EMGiftType.Boat)
            {
                szLogRes += "船:" + pGachaSlotInfo.nItemID + "\r\n";
            }
            else if (pGachaSlotInfo.emType == CGiftGachaBoxInfo.EMGiftType.Role)
            {
                szLogRes += "角色:" + pGachaSlotInfo.nItemID + "\r\n";
            }
        }

        uiGMConsole.uiLabelContent.text = szLogRes;
    }
}
