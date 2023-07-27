using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HHandlerGetFesInfoPack : INetEventHandler
{
    long nPackId = 0;

    public HHandlerGetFesInfoPack(long packId)
    {
        nPackId = packId;
    }

    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        CLocalNetArrayMsg msgListContent = pMsg.GetNetMsgArr("list");
        for (int i = 0; i < msgListContent.GetSize(); i++)
        {
            CLocalNetMsg msgSlot = msgListContent.GetNetMsg(i);
            long packId = msgSlot.GetLong("packId");
            if (packId != nPackId) continue;

            CFishFesInfoSlot pInfo = new CFishFesInfoSlot();
            pInfo.nPackId = packId;
            pInfo.nIdx = msgSlot.GetInt("idx");
            pInfo.nPointVtb = msgSlot.GetInt("needVtbPoint");
            pInfo.nPointPlayer = msgSlot.GetInt("needViewerPoint");

            string szGiftContent = msgSlot.GetString("giftDetail").Replace("\\", "");
            CLocalNetArrayMsg arrGiftList = new CLocalNetArrayMsg(szGiftContent);
            if(arrGiftList!=null && arrGiftList.GetSize() > 0)
            {
                CLocalNetMsg msgGiftContent = arrGiftList.GetNetMsg(0);
                pInfo.nType = msgGiftContent.GetInt("giftType");
                pInfo.nID = msgGiftContent.GetInt("giftId");

                CFishFesInfoMgr.Ins.AddFesInfo(pInfo);
            }
        }
    }
}
