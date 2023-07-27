using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuCmdAttrite(CDanmuEventConst.RoomD)]
public class DCmdRoomD : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        string uid = dm.uid.ToString();

        if (CGameSurviveMap.Ins == null) return;

        CPlayerUnit pPlayerUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if (pPlayerUnit == null) return;

        CGameSurviveArea pArea = CGameSurviveMap.Ins.GetAreaByIdx(0);
        if (pArea == null) return;

        if (pArea.GetPlayerCount() >= pArea.arrSlots.Length)
        {
            UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
            if (uiGameInfo == null || !uiGameInfo.IsOpen()) return;
            UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(uid);
            if (uiUnitInfo == null) return;
            uiUnitInfo.SetDmContent("·¿¼äÒÑÂú");
            return;
        }

        CGameSurviveMap.Ins.JoinRoom(pPlayerUnit, 3);
    }
}
