using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CDanmuCmdAttrite(CDanmuEventConst.Chaxun_ProAdd)]
public class DCmdChaxunProAdd : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        string uid = dm.uid.ToString();
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null) return;

        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if (pUnit == null) return;

        UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (uiGameInfo == null || !uiGameInfo.IsOpen()) return;

        UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(pUnit.uid);
        if (uiUnitInfo == null) return;

        List<CPlayerProAddInfo> listProInfo = pPlayer.GetProAddList();
        string szContent = "";
        if(listProInfo.Count <= 0)
        {
            uiUnitInfo.SetDmContent("暂无增益属性");
            return;
        }

        for(int i=0; i<listProInfo.Count; i++)
        {
            //szContent += listProInfo[i].GetSlotString() +
            //            ":" +
            //            CPlayerProAddInfo.GetProString(listProInfo[i].emPro, listProInfo[i].value) + "\r\n";
            szContent += CPlayerProAddInfo.GetProString(listProInfo[i].emPro, listProInfo[i].value) + "\r\n";
        }
        uiUnitInfo.SetDmContent(szContent);
    }
}
