using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CDanmuCmdAttrite(CDanmuEventConst.Chaxun_Record)]
public class DCmdChaxunRecord : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        string uid = dm.uid.ToString();
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        CPlayerUnit pPlayerUnit = CPlayerMgr.Ins.GetActiveUnit(uid);
        if (pPlayerUnit == null)
        {
            pPlayerUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        }

        if (pPlayer == null ||
            pPlayerUnit == null)
        {
            return;
        }

        UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (uiGameInfo == null || !uiGameInfo.IsOpen()) return;

        UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(pPlayerUnit.uid);
        if (uiUnitInfo == null) return;

        CFishRecordInfo fishRecordInfo = CFishRecordMgr.Ins.GetRecord(uid);
        if (fishRecordInfo == null) return;
        uiUnitInfo.SetRecordContent(fishRecordInfo, 4.5F);
    }
}
