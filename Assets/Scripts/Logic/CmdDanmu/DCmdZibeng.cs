using OpenBLive.Runtime.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuCmdAttrite(CDanmuEventConst.Zibeng)]
public class DCmdZibeng : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        string uid = dm.uid.ToString();

        //�ж�Ǯ������
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null) return;

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

        //bool bRpg = pPlayer.HasPro(EMAddUnitProType.ZibengToRPG);

        int nZiBengPrice = CGameColorFishMgr.Ins.pStaticConfig.GetInt("�̱��۸�");
        //if(bRpg)
        //{
        //    nZiBengPrice *= 2;
        //}

        //if(DateTime.Now.Year == 2022 &&
        //   DateTime.Now.Month == 11 &&
        //   DateTime.Now.Day == 11)
        //{
        //    nZiBengPrice = CGameColorFishMgr.Ins.pStaticConfig.GetInt("�̱����ۼ۸�");
        //    //if(bRpg)
        //    //{
        //    //    nZiBengPrice *= 2;
        //    //}
        //}

        if (pPlayer.GameCoins <= nZiBengPrice)
        {
            UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
            if (uiGameInfo != null && uiGameInfo.IsOpen())
            {
                UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(uid);
                if (uiUnitInfo != null)
                {
                    uiUnitInfo.SetDmContent("���ֲ���");
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
                        uiUnitInfo.SetDmContent("CDʣ��" + (int)pUnit.pTicerCDZibeng.CurValue + "��");
                    }
                }

                return;
            }

            pUnit.SetState(CPlayerUnit.EMState.GunShootBoss);
        }
        else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal)
        {

        }
        else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema)
        {

        }
        else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive)
        {

        }
        else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
        {

        }
    }
}