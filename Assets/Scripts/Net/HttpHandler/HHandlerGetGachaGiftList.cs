using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HHandlerGetGachaGiftList : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        CLocalNetArrayMsg arrContent = pMsg.GetNetMsgArr("list");
        for (int i = 0; i < arrContent.GetSize(); i++)
        {
            CLocalNetMsg msgContent = arrContent.GetNetMsg(i);
            int modelId = msgContent.GetInt("modelId");
            long baseCount = msgContent.GetLong("baseCount");
            if (modelId != 1) continue;

            string szGachaList = msgContent.GetString("gachaContent").Replace("\\", "");
            CLocalNetArrayMsg arrGachaList = new CLocalNetArrayMsg(szGachaList);
            CFishFesInfoMgr.Ins.nGiftGachaBoxBoodiCount = baseCount;
            CFishFesInfoMgr.Ins.listGachaGiftInfos.Clear();

            if (arrGachaList == null || arrGachaList.GetSize() <= 0) continue;

            for (int j = 0; j < arrGachaList.GetSize(); j++)
            {
                CLocalNetMsg msgSlot = arrGachaList.GetNetMsg(j);
                CGiftGachaBoxInfo pInfo = new CGiftGachaBoxInfo()
                {
                    emType = (CGiftGachaBoxInfo.EMGiftType)msgSlot.GetInt("itemType"),
                    nItemID = msgSlot.GetInt("itemId")
                };

                if(pInfo.emType == CGiftGachaBoxInfo.EMGiftType.Boat ||
                   pInfo.emType == CGiftGachaBoxInfo.EMGiftType.Role ||
                   pInfo.emType == CGiftGachaBoxInfo.EMGiftType.FishGan)
                {
                    CFishFesInfoMgr.Ins.listGachaGiftInfos.Add(pInfo);
                }
            }
        }
    }
}
