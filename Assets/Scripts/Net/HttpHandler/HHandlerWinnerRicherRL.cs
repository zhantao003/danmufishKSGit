
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CHttpEvent(CHttpConst.GetRankListFishWinnerRicher)]
public class HHandlerWinnerRicherRL : INetEventHandler
{
    public DelegateNFuncCall dlgCallSuc = null;

    public HHandlerWinnerRicherRL(DelegateNFuncCall call)
    {
        dlgCallSuc = call;
    }

    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        UIManager.Instance.CloseUI(UIResType.NetWait);

        string szStatus = pMsg.GetString("status");
        if (!szStatus.Equals("ok")) return;

        //string szRankContent = pMsg.GetString("list").Replace("\\", "");
        CLocalNetArrayMsg arrRankContent = pMsg.GetNetMsgArr("list"); //new CLocalNetArrayMsg(szRankContent);
        List<CPlayerRankInfo> listRankInfo = new List<CPlayerRankInfo>();
        for (int i = 0; i < arrRankContent.GetSize(); i++)
        {
            CLocalNetMsg msgRankSlot = arrRankContent.GetNetMsg(i);
            CPlayerRankInfo pRankInfo = new CPlayerRankInfo();
            pRankInfo.emRankType = EMRankType.WinnerRicher;
            pRankInfo.InitMsg(msgRankSlot);

            listRankInfo.Add(pRankInfo);
        }

        CWorldRankInfoMgr.Ins.SetCommonRankInfo(EMRankType.WinnerRicher, listRankInfo);

        dlgCallSuc?.Invoke();

        //UIRankInfo uiRank = UIManager.Instance.GetUI(UIResType.RankList) as UIRankInfo;
        //if (uiRank != null && uiRank.IsOpen())
        //{
        //    uiRank.SetCommonRankInfo(EMRankType.WinnerRicher, listRankInfo);

        //    if (uiRank.emRankType == EMRankType.WinnerRicher)
        //    {
        //        uiRank.RefreshList(listRankInfo);
        //        //uiRank.SetSelfCommonInfo(EMRankType.MapCain, pSelfInfo);
        //    }
        //}

        //TODO:¸üÐÂÅÅÐÐ°ñUI
        //UIBattleModeInfo battleModeInfo = UIManager.Instance.GetUI(UIResType.BattleModeInfo) as UIBattleModeInfo;
        //if (battleModeInfo != null)
        //{
        //    battleModeInfo.battleRankRoot.worldProfitRoot.GetRankInfo(listRankInfo);
        //}
    }
}
