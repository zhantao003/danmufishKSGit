using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuCmdAttrite(CDanmuEventConst.RefreshPro)]
public class DCmdRefreshPro : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        return;
        string uid = dm.uid.ToString();
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null) return;

        CPlayerFishGanInfo pGanInfo = pPlayer.pFishGanPack.GetInfo(pPlayer.nFishGanAvatarId);
        if (pGanInfo == null) return;

        if (pPlayer.CheckIsGrayName())
        {
            return;
        }
        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(pPlayer.uid);
        if (pUnit == null)
        {
            pUnit = CPlayerMgr.Ins.GetActiveUnit(pPlayer.uid);
        }
        if (pUnit != null &&
            pUnit.bCheckYZM)
        {
            return;
        }

        //���˵������������������
        if (pGanInfo.emPro == EMAddUnitProType.None ||
            pGanInfo.emPro == EMAddUnitProType.ZibengToFootBall ||
            pGanInfo.emPro == EMAddUnitProType.ZibengToFootBallG ||
            pGanInfo.emPro == EMAddUnitProType.ZibengToChrismas ||
            pGanInfo.emPro == EMAddUnitProType.ZibengToCuanTianHou)
        {
            UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
            if (uiGameInfo == null || !uiGameInfo.IsOpen()) return;

            UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(pPlayer.uid);
            if (uiUnitInfo == null) return;

            uiUnitInfo.SetDmContent("��ǰ����޷�����");

            return;
        }

        //�жϺ�����ҹ�����
        if(pPlayer.TreasurePoint < 50)
        {
            UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
            if (uiGameInfo == null || !uiGameInfo.IsOpen()) return;

            UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(pPlayer.uid);
            if (uiUnitInfo == null) return;

            uiUnitInfo.SetDmContent("������Ҳ���");

            return;
        }

        CHttpParam pReqParams = new CHttpParam
        (
           new CHttpParamSlot("uid", uid.ToString()),
           new CHttpParamSlot("ganId", pPlayer.nFishGanAvatarId.ToString())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.RefreshGanPro, pReqParams, 0);
    }
}
