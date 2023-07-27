using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.RefreshGanPro)]
public class HHandlerRefreshGanPro : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string szStatus = pMsg.GetString("status");
        string uid = pMsg.GetString("uid");
        if (!szStatus.Equals("ok"))
        {
            int code = pMsg.GetInt("errCode");
            if(code == 1)
            {
                CPlayerBaseInfo pTmpPlayer = CPlayerMgr.Ins.GetPlayer(uid);
                if (pTmpPlayer == null) return;

                UIGameInfo uiTmpGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
                if (uiTmpGameInfo == null || !uiTmpGameInfo.IsOpen()) return;

                UIShowUnitDialog uiTmpUnitInfo = uiTmpGameInfo.GetShowDialogSlot(pTmpPlayer.uid);
                if (uiTmpUnitInfo == null) return;

                uiTmpUnitInfo.SetDmContent("海盗金币不足");

                return;
            }

            return;
        }

        long treasurePoint = pMsg.GetLong("treasurePoint");
        int ganId = pMsg.GetInt("ganId");
        EMAddUnitProType proType = (EMAddUnitProType)pMsg.GetInt("proType");
        int proAdd = pMsg.GetInt("proAdd");

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        pPlayer.TreasurePoint = treasurePoint;
        if (pPlayer == null) return;

        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if (pUnit == null) return;

        CPlayerFishGanInfo pGanInfo = pPlayer.pFishGanPack.GetInfo(pPlayer.nFishGanAvatarId);
        if (pGanInfo == null) return;

        pGanInfo.emPro = proType;
        pGanInfo.nProAdd = proAdd;

        //刷新属性
        pPlayer.RefreshProAdd();

        UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (uiGameInfo == null || !uiGameInfo.IsOpen()) return;

        UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(pPlayer.uid);
        if (uiUnitInfo == null) return;

        ST_UnitFishGan pTBLGanInfo = CTBLHandlerUnitFishGan.Ins.GetInfo(pGanInfo.nGanId);
        if (pTBLGanInfo == null) return;
        bool bMax = (pTBLGanInfo.nProAddMax == pGanInfo.nProAdd);
        uiUnitInfo.SetDmContent($"<color=#FF9500>{pTBLGanInfo.szName}</color>重铸成功!\r\n" +
            CPlayerProAddInfo.GetProString(pGanInfo.emPro, pGanInfo.nProAdd) +
            (bMax ? "\r\n<color=#FF9500>已达到最高级</color>" : ("\r\n<color=#FF9500>" + CPlayerProAddInfo.GetProRangeString(pGanInfo.nGanId))+ "</color>"));

        //特效
        CEffectMgr.Instance.CreateEffSync("Effect/effRefreshPro", pUnit.tranSelf.position, Quaternion.identity, 0);

        CFishGanEffWithValue pEffRoot = pUnit.pAvatarFishGan.GetComponent<CFishGanEffWithValue>();
        if (pEffRoot != null)
        {
            pEffRoot.RefreshValue(pPlayer.GetProAddValue(pTBLGanInfo.emProType));
        }
    }
}
