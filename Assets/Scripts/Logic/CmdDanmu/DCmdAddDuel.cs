using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuCmdAttrite(CDanmuEventConst.AddDuel)]
public class DCmdAddDuel : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        string uid = dm.uid.ToString();
        ///根据uid获取玩家单位
        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if (pUnit == null)
        {
            pUnit = CPlayerMgr.Ins.GetActiveUnit(uid);
            if (pUnit == null)
                return;
        }
        if (CGameColorFishMgr.Ins.emCurGameType != CGameColorFishMgr.EMGameType.Battle &&
            CGameColorFishMgr.Ins.emCurGameType != CGameColorFishMgr.EMGameType.Boss)
        {
            if (pUnit.bCheckYZM ||
            pUnit.pInfo.CheckIsGrayName())
            {
                return;
            }
        }

        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Battle ||
            CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss ||
            CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive)
        {
            return;
        }

        /////判断是否开启了决斗
        //if ((CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal ||
        //     CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema) && 
        //     CTBLHandlerFucAble.Ins.GetInfo((int)EMFuncAbleType.Duel).CheckSceneLv())
        //{
        //    UIToast.Show((CTBLHandlerFucAble.Ins.GetInfo((int)EMFuncAbleType.Duel).nSecenLv + 1) + "级渔场开放决斗功能");
        //    return;
        //}

        if (pUnit.emCurState == CPlayerUnit.EMState.Jump ||
            pUnit.emCurState == CPlayerUnit.EMState.HitDrop)
        {
            return;
        }
        
        AddDuel(pUnit);
    }


    public void AddDuel(CPlayerUnit player)
    {
        UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
        if (roomInfo == null) return;
        if (roomInfo.battleRoot.nlCurPrice <= 0) return;
        ///是否已经满人
        if (roomInfo.battleRoot.bMax) return;
        ///是否正在比赛结算时间中
        if (roomInfo.battleRoot.bResultTime) return;
        if (CGameColorFishMgr.Ins != null &&
            CGameColorFishMgr.Ins.pMap != null &&
            CGameColorFishMgr.Ins.pMap.pDuelBoat != null &&
            CGameColorFishMgr.Ins.pMap.pDuelBoat.emCurState == CDuelBoat.EMState.Hide) return;
        long nlTargetPrice = roomInfo.battleRoot.nlCurPrice;
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(player.uid);
        if (roomInfo.emCurShowType == ShowBoardType.Battle)
        {
            if (nlTargetPrice > pPlayer.GameCoins)
            {
                UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
                if (uiGameInfo == null || !uiGameInfo.IsOpen()) return;
                UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(player.uid);
                if (uiUnitInfo == null) return;
                uiUnitInfo.SetDmContent("积分不足");
                return;
            }
            //Debug.LogError(nlTargetPrice + "====Ready Send Msg====" + pPlayer.GameCoins);
            bool bVtb = player.pInfo.emUserType == CPlayerBaseInfo.EMUserType.Zhubo;
            //同步玩家使用道具的请求
            CPlayerNetHelper.AddFishCoin(player.uid,
                                         nlTargetPrice * -1,
                                         EMFishCoinAddFunc.Duel,
                                         false,
                                         new HHandlerAddDuel());
        }
        else if(roomInfo.emCurShowType == ShowBoardType.SpecialBattle)
        {
            if (nlTargetPrice > pPlayer.TreasurePoint)
            {
                UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
                if (uiGameInfo == null || !uiGameInfo.IsOpen()) return;
                UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(player.uid);
                if (uiUnitInfo == null) return;
                uiUnitInfo.SetDmContent("海盗金币不足");
                return;
            }
            //CHttpParam pReqParams = new CHttpParam
            //    (
            //        new CHttpParamSlot("uid", player.uid.ToString()),
            //        new CHttpParamSlot("treasurePoint", (-nlTargetPrice).ToString())
            //    );
            //CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddUserTreasurePoint, new HHandlerAddSpecialDuel(), pReqParams, CHttpMgr.Instance.nReconnectTimes, true);

            CPlayerNetHelper.AddTreasureCoin(player.uid, -nlTargetPrice, new HHandlerAddSpecialDuel());

            //bool bVtb = player.pInfo.emUserType == CPlayerBaseInfo.EMUserType.Zhubo;
            ////同步玩家使用道具的请求
            //CPlayerNetHelper.AddFishCoin(player.uid,
            //                             nlTargetPrice * -1,
            //                             EMFishCoinAddFunc.Duel,
            //                             new HHandlerAddDuel());
        }

    }

}
