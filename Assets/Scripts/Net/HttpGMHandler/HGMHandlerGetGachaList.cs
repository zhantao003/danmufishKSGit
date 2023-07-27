using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.DebugGetGachaInfo)]
public class HGMHandlerGetGachaList : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        UIGMGachaPool uiGachaPool = GameObject.FindObjectOfType<UIGMGachaPool>();
        if (uiGachaPool == null) return;

        CLocalNetArrayMsg arrMsgContent = pMsg.GetNetMsgArr("list");
        for(int i=0; i<arrMsgContent.GetSize(); i++)
        {
            CLocalNetMsg msgGacha = arrMsgContent.GetNetMsg(i);
            int modelId = msgGacha.GetInt("modelId");

            long curModelId = long.Parse(uiGachaPool.uiInputFieldModelId.text);
            if (curModelId != modelId) continue;

            string szGachaList = msgGacha.GetString("gachaContent").Replace("\\", "");
            CLocalNetArrayMsg arrGachaList = new CLocalNetArrayMsg(szGachaList);
            List<CGMGachaInfo> listGachaInfos = new List<CGMGachaInfo>();

            if(arrGachaList!=null && arrGachaList.GetSize() > 0)
            {
                for (int j = 0; j < arrGachaList.GetSize(); j++)
                {
                    CLocalNetMsg msgSlot = arrGachaList.GetNetMsg(j);
                    CGMGachaInfo pInfo = new CGMGachaInfo()
                    {
                        avatarId = msgSlot.GetLong("avatarId"),
                        num = msgSlot.GetLong("num"),
                        rate = msgSlot.GetLong("rate")
                    };
                    listGachaInfos.Add(pInfo);
                }
            }
            else
            {
                Debug.Log($"ë´½ð³Ø{modelId}¿ÕÊý¾Ý");
            }

            uiGachaPool.Init(modelId, msgGacha.GetInt("baseCount"), 2500, 1, 1, listGachaInfos);
        }
    }
}
