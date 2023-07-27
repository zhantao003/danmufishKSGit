using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CDanmuCmdAttrite(CDanmuEventConst.Buy)]
public class DCmdBuyGacha : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        string uid = dm.uid.ToString();
        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if (pUnit == null)
        {
            pUnit = CPlayerMgr.Ins.GetActiveUnit(uid);

            if (pUnit == null)
                return;
        }
        if (pUnit.bCheckYZM ||
            pUnit.pInfo.CheckIsGrayName())
        {
            return;
        }
        int nCost = CGameColorFishMgr.Ins.pStaticConfig.GetInt("”„±“ªª±¶œ‰");
        //≈–∂œÕÊº“«Æπª≤ªπª
        if (pUnit.pInfo.GameCoins < nCost)
        {
            UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
            if (uiGameInfo != null && uiGameInfo.IsOpen())
            {
                UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(uid);
                if (uiUnitInfo != null)
                {
                    uiUnitInfo.SetDmContent("ª˝∑÷≤ª◊„");
                }
            }

            return;
        }

        if (UIManager.Instance.GetUI(UIResType.Help) as UIHelp != null &&
            pUnit.pInfo.emUserType == CPlayerBaseInfo.EMUserType.Zhubo)
        {
            if (UIHelp.bActive)
            {
                if (UIHelp.emHelpLv == EMHelpLev.Lev3_Chou)
                {
                    UIHelp.GoNextHelpLv();
                }
                else if (UIHelp.emHelpLv != EMHelpLev.Over)
                {
                    return;
                }
            }
        }

        CHttpParam pReqParams = new CHttpParam(
                    new CHttpParamSlot("uid", dm.uid.ToString()),
                    new CHttpParamSlot("roomId", CDanmuSDKCenter.Ins.szRoomId.ToString()),
                    new CHttpParamSlot("modelId", "1"),
                    new CHttpParamSlot("cost", CGameColorFishMgr.Ins.pStaticConfig.GetInt("”„±“ªª±¶œ‰").ToString()),
                    new CHttpParamSlot("gachaCount", 1.ToString()),
                    new CHttpParamSlot("isVtb", ((pUnit.pInfo.emUserType == CPlayerBaseInfo.EMUserType.Zhubo) ? 1 : 0).ToString()),
                    new CHttpParamSlot("time", CGameColorFishMgr.Ins.GetNowServerTime().ToString())
            );
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.BuyFishGacha, pReqParams, 10, true);
    }
}