using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CDanmuCmdAttrite(CDanmuEventConst.Chaxun_Limit)]
public class DCmdChaxunLimit : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        string uid = dm.uid;

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null) return;

        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if (pUnit == null) return;

        CHttpParam pReqParams = new CHttpParam(
                new CHttpParamSlot("uid", uid.ToString())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.ChaxunDailyLimit, pReqParams);
    }
}
