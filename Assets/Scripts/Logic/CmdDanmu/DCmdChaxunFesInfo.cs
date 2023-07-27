using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuCmdAttrite(CDanmuEventConst.Chaxun_FesInfo)]
public class DCmdChaxunFesInfo : CDanmuCmdAction
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

        CFishFesPlayerInfo pPlayerFesInfo = CFishFesInfoMgr.Ins.GetPlayerInfo((int)CFishFesInfoMgr.EMFesType.RankOuhuang, uid);
        CFishFesPlayerInfo pPlayerFes2Info = CFishFesInfoMgr.Ins.GetPlayerInfo((int)CFishFesInfoMgr.EMFesType.RankRicher, uid);
        string szContent = "";
        if (CFishFesInfoMgr.Ins.IsFesOn((int)CFishFesInfoMgr.EMFesType.RankOuhuang))
        {
            if (pPlayerFesInfo == null)
            {
                szContent += "皇冠：暂无\r\n";
            }
            else
            {
                CFishFesInfoSlot pNextGiftInfo = CFishFesInfoMgr.Ins.GetFesInfo(1, pPlayerFesInfo.nCurIdx);

                if (pNextGiftInfo != null)
                {
                    szContent += $"皇冠：<color=#FF9500>{pPlayerFesInfo.nPlayerPoint}</color>,离下一级差{pNextGiftInfo.nPointPlayer - pPlayerFesInfo.nPlayerPoint}个\r\n";
                }
                else
                {
                    szContent += $"皇冠：奖励已领完\r\n";
                }
            }
        }
        else
        {
            szContent += "赛季未开启\r\n";
        }

        if (CFishFesInfoMgr.Ins.IsFesOn((int)CFishFesInfoMgr.EMFesType.RankRicher))
        {
            if (pPlayerFes2Info == null)
            {
                szContent += "收益积分：暂无";
            }
            else
            {
                CFishFesInfoSlot pNextGiftInfo = CFishFesInfoMgr.Ins.GetFesInfo(2, pPlayerFes2Info.nCurIdx);

                if (pNextGiftInfo != null)
                {
                    szContent += $"收益积分：<color=#FF9500>{CHelpTools.GetGoldSZ(pPlayerFes2Info.nPlayerPoint,"f1")}</color>,离下一级差{CHelpTools.GetGoldSZ(pNextGiftInfo.nPointPlayer - pPlayerFes2Info.nPlayerPoint,"f1")}\r\n";
                }
                else
                {
                    szContent += $"收益积分：奖励已领完\r\n";
                }
            }
        }
        else
        {
            szContent += "赛季未开启";
        }

        uiUnitInfo.SetDmContent(szContent);
    }
}
