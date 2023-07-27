using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CDanmuCmdAttrite(CDanmuEventConst.BuyTen)]
public class DCmdBuyGachaTen : CDanmuCmdAction
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
        int nCost = CGameColorFishMgr.Ins.pStaticConfig.GetInt("”„±“ªª±¶œ‰") * 10;
        //≈–∂œÕÊº“«Æπª≤ªπª
        if(pUnit.pInfo.GameCoins < nCost)
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

        CHttpParam pReqParams = new CHttpParam(
                    new CHttpParamSlot("uid", uid),
                    new CHttpParamSlot("roomId", CDanmuSDKCenter.Ins.szRoomId.ToString()),
                    new CHttpParamSlot("modelId", "1"),
                    new CHttpParamSlot("cost", nCost.ToString()),
                    new CHttpParamSlot("gachaCount", 10.ToString()),
                    new CHttpParamSlot("isVtb", ((pUnit.pInfo.emUserType == CPlayerBaseInfo.EMUserType.Zhubo) ? 1 : 0).ToString()),
                    new CHttpParamSlot("time", CGameColorFishMgr.Ins.GetNowServerTime().ToString()) 
            );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.BuyFishGacha, pReqParams, 10, true);
    }
}
