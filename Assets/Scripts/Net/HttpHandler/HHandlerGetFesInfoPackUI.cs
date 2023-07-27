using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HHandlerGetFesInfoPackUI : INetEventHandler
{
    public CFishFesInfoMgr.EMFesType emFes;

    public HHandlerGetFesInfoPackUI(CFishFesInfoMgr.EMFesType fes)
    {
        emFes = fes;
    }

    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        List<CGMFesInfo> listInfos = new List<CGMFesInfo>();
        CLocalNetArrayMsg msgListContent = pMsg.GetNetMsgArr("list");
        for (int i = 0; i < msgListContent.GetSize(); i++)
        {
            CLocalNetMsg msgSlot = msgListContent.GetNetMsg(i);
            int packId = msgSlot.GetInt("packId");

            if (packId != (int)emFes) continue;

            CGMFesInfo pInfo = new CGMFesInfo();
            pInfo.nPackId = packId;
            pInfo.nIdx = msgSlot.GetInt("idx");
            pInfo.nPointVtb = msgSlot.GetInt("needVtbPoint");
            pInfo.nPointPlayer = msgSlot.GetInt("needViewerPoint");

            string szGiftContent = msgSlot.GetString("giftDetail").Replace("\\", ""); ;
            CLocalNetArrayMsg arrGiftContent = new CLocalNetArrayMsg(szGiftContent);
            if (arrGiftContent.GetSize() > 0)
            {
                CLocalNetMsg msgGiftContent = arrGiftContent.GetNetMsg(0);
                pInfo.nType = msgGiftContent.GetInt("giftType");
                pInfo.nID = msgGiftContent.GetInt("giftId");
            }
            else
            {
                pInfo.nType = 2;
                pInfo.nID = 10000;
            }

            listInfos.Add(pInfo);
        }

        if (emFes == CFishFesInfoMgr.EMFesType.RankOuhuang)
        {
            UIAvatarInfo uiAvatarInfo = UIManager.Instance.GetUI(UIResType.AvatarInfo) as UIAvatarInfo;
            if (uiAvatarInfo != null)
            {
                //uiAvatarInfo.listWinnerOuhuangInfos = listInfos;
                uiAvatarInfo.SetType(UIAvatarInfo.EMType.OuHuang);
            }
        }
        else if (emFes == CFishFesInfoMgr.EMFesType.RankRicher)
        {
            UIAvatarInfo uiAvatarInfo = UIManager.Instance.GetUI(UIResType.AvatarInfo) as UIAvatarInfo;
            if (uiAvatarInfo != null)
            {
                //uiAvatarInfo.listWinnerRicherInfos = listInfos;
                uiAvatarInfo.SetType(UIAvatarInfo.EMType.Richer);
            }
        }
    }
}
