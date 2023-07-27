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
                szContent += "�ʹڣ�����\r\n";
            }
            else
            {
                CFishFesInfoSlot pNextGiftInfo = CFishFesInfoMgr.Ins.GetFesInfo(1, pPlayerFesInfo.nCurIdx);

                if (pNextGiftInfo != null)
                {
                    szContent += $"�ʹڣ�<color=#FF9500>{pPlayerFesInfo.nPlayerPoint}</color>,����һ����{pNextGiftInfo.nPointPlayer - pPlayerFesInfo.nPlayerPoint}��\r\n";
                }
                else
                {
                    szContent += $"�ʹڣ�����������\r\n";
                }
            }
        }
        else
        {
            szContent += "����δ����\r\n";
        }

        if (CFishFesInfoMgr.Ins.IsFesOn((int)CFishFesInfoMgr.EMFesType.RankRicher))
        {
            if (pPlayerFes2Info == null)
            {
                szContent += "������֣�����";
            }
            else
            {
                CFishFesInfoSlot pNextGiftInfo = CFishFesInfoMgr.Ins.GetFesInfo(2, pPlayerFes2Info.nCurIdx);

                if (pNextGiftInfo != null)
                {
                    szContent += $"������֣�<color=#FF9500>{CHelpTools.GetGoldSZ(pPlayerFes2Info.nPlayerPoint,"f1")}</color>,����һ����{CHelpTools.GetGoldSZ(pNextGiftInfo.nPointPlayer - pPlayerFes2Info.nPlayerPoint,"f1")}\r\n";
                }
                else
                {
                    szContent += $"������֣�����������\r\n";
                }
            }
        }
        else
        {
            szContent += "����δ����";
        }

        uiUnitInfo.SetDmContent(szContent);
    }
}
