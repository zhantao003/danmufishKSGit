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

        //ÅÐ¶ÏÇ®¹»²»¹»
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

        int nZiBengPrice = CGameColorFishMgr.Ins.pStaticConfig.GetInt("×Ì±À¼Û¸ñ");
        //if(bRpg)
        //{
        //    nZiBengPrice *= 2;
        //}

        //if(DateTime.Now.Year == 2022 &&
        //   DateTime.Now.Month == 11 &&
        //   DateTime.Now.Day == 11)
        //{
        //    nZiBengPrice = CGameColorFishMgr.Ins.pStaticConfig.GetInt("×Ì±À´òÕÛ¼Û¸ñ");
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
                    uiUnitInfo.SetDmContent("»ý·Ö²»×ã");
                }
            }

            return;
        }

        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
        {
            if (CGameBossMgr.Ins == null ||
                CGameBossMgr.Ins.pBoss == null ||
               !CGameBossMgr.Ins.pBoss.IsAtkAble()) return;

            ///½ûÖ¹»»Î»ÖÃµÄ×´Ì¬
            if (pUnit.emCurState == CPlayerUnit.EMState.BossEat ||
                pUnit.emCurState == CPlayerUnit.EMState.BossReturn ||
                pUnit.emCurState == CPlayerUnit.EMState.BossWait ||
                pUnit.emCurState == CPlayerUnit.EMState.GunShootBoss ||
                pUnit.emCurState == CPlayerUnit.EMState.RPGShootBoss ||
                pUnit.pShowFishEndEvent != null)
            {
                return;
            }

            //¼ì²é×È±ÄCD
            if (pUnit.IsZibengCD())
            {
                UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
                if (uiGameInfo != null && uiGameInfo.IsOpen())
                {
                    UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(pUnit.uid);
                    if (uiUnitInfo != null)
                    {
                        uiUnitInfo.SetDmContent("CDÊ£Óà" + (int)pUnit.pTicerCDZibeng.CurValue + "Ãë");
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