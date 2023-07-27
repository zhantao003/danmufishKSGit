using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuCmdAttrite(CDanmuEventConst.GunRpg)]
public class DCmdGunRPG : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        return;
        string uid = dm.uid.ToString();

        //�ж�Ǯ������
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null) return;

        //�ж��Ƿ���Я��RPG
        if(!pPlayer.HasPro(EMAddUnitProType.ZibengToRPG))
        {
            return;
        }

        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if (pUnit == null) return;

        if (pPlayer.CheckIsGrayName())
        {
            return;
        }
        if (pUnit != null &&
           pUnit.bCheckYZM)
        {
            return;
        }

        int nZiBengPrice = CGameColorFishMgr.Ins.pStaticConfig.GetInt("�̱��۸�") * 2;

        if (pPlayer.GameCoins <= nZiBengPrice)
        {
            UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
            if (uiGameInfo != null && uiGameInfo.IsOpen())
            {
                UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(uid);
                if (uiUnitInfo != null)
                {
                    uiUnitInfo.SetDmContent("���ֲ����Է���RPG");
                }
            }

            return;
        }

        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
        {
            if (CGameBossMgr.Ins == null ||
                CGameBossMgr.Ins.pBoss == null ||
               !CGameBossMgr.Ins.pBoss.IsAtkAble()) return;

            ///��ֹ��λ�õ�״̬
            if (pUnit.emCurState == CPlayerUnit.EMState.BossEat ||
                pUnit.emCurState == CPlayerUnit.EMState.BossReturn ||
                pUnit.emCurState == CPlayerUnit.EMState.BossWait ||
                pUnit.emCurState == CPlayerUnit.EMState.GunShootBoss ||
                pUnit.emCurState == CPlayerUnit.EMState.RPGShootBoss ||
                pUnit.pShowFishEndEvent != null)
            {
                return;
            }

            //����ȱ�CD
            if (pUnit.IsZibengCD())
            {
                UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
                if (uiGameInfo != null && uiGameInfo.IsOpen())
                {
                    UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(pUnit.uid);
                    if (uiUnitInfo != null)
                    {
                        uiUnitInfo.SetDmContent("RPG CDʣ��" + (int)pUnit.pTicerCDZibeng.CurValue + "��");
                    }
                }

                return;
            }

            pUnit.SetState(CPlayerUnit.EMState.RPGShootBoss);
        }
        else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal)
        {

        }
        else if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
        {

        }
    }
}
