using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuCmdAttrite(CDanmuEventConst.Auction)]
public class DCmdAuction : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Battle ||
            CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
        {
            return;
        }

        string uid = dm.uid;
        string szContent = dm.content;
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
        /////判断是否开启了拍卖
        //if ((CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal ||
        //     CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema) &&
        //    CTBLHandlerFucAble.Ins.GetInfo((int)EMFuncAbleType.Auction).CheckSceneLv())
        //{
        //    UIToast.Show((CTBLHandlerFucAble.Ins.GetInfo((int)EMFuncAbleType.Auction).nSecenLv + 1) + "级渔场开放拍卖功能");
        //    return;
        //}
        if (pUnit.emCurState == CPlayerUnit.EMState.HitDrop)
            return;

        Duel(szContent, pUnit);
    }


    public void Duel(string content, CPlayerUnit player)
    {
        string szContent = content.Trim();
        try
        {
            int nLen = content.Length;
            int nCmdIdx = content.IndexOf(CDanmuEventConst.Auction) + CDanmuEventConst.Auction.Length;
            szContent = content.Substring(nCmdIdx, nLen - nCmdIdx).Trim();
            if (CHelpTools.IsStringEmptyOrNone(szContent))
            {
                if (CAuctionMgr.Ins.emAuctionState != CAuctionMgr.EMAuctionState.Normal)
                {
                    szContent = (CAuctionMgr.Ins.nBuyPrice + CAuctionMgr.Ins.curTreasureInfo.treasureInfo.nAddPrice).ToString();
                }
                else if (CAuctionMatMgr.Ins.emAuctionState != CAuctionMatMgr.EMAuctionState.Normal)
                {
                    szContent = (CAuctionMatMgr.Ins.nBuyPrice + CAuctionMatMgr.Ins.curTreasureInfo.nAddPrice).ToString();
                }
            }
            if (CHelpTools.IsStringEmptyOrNone(szContent))
            {
                Debug.Log("异常出价弹幕:" + content);
                return;
            }
            //long nlTargetPrice = 0;
            //if (!long.TryParse(szContent, out nlTargetPrice))
            //{
            //    Debug.Log("异常的出价价格：" + szContent);
            //    return;
            //}
            CPlayerBaseInfo baseInfo = CPlayerMgr.Ins.GetPlayer(player.uid);
            if (baseInfo == null)
                return;
            if (CAuctionMgr.Ins.emAuctionState != CAuctionMgr.EMAuctionState.Normal)
            {
                CAuctionMgr.Ins.SetPlayerByHttp(baseInfo);
            }
            //else if (CAuctionMatMgr.Ins.emAuctionState != CAuctionMatMgr.EMAuctionState.Normal)
            //{
            //    CAuctionMatMgr.Ins.SetPlayerByHttp(baseInfo, nlTargetPrice);
            //}
            else
            {
                return;
            }


    }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }


}
