using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CDanmuCmdAttrite(CDanmuEventConst.Solo)]
public class DCmdSolo : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        return;
        ///判断是否开启了决斗
        if ((CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal ||
             CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema) &&
            CTBLHandlerFucAble.Ins.GetInfo((int)EMFuncAbleType.Duel).CheckSceneLv())
        {
            UIToast.Show((CTBLHandlerFucAble.Ins.GetInfo((int)EMFuncAbleType.Duel).nSecenLv + 1) + "级渔场开放决斗功能");
            return;
        }

        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Battle ||
            CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
        {
            return;
        }

        string uid = dm.uid.ToString();
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
        if (pUnit.bDuelCD)
        {
            UIToast.Show("决斗CD中");
            return;
        }

        if (pUnit.emCurState == CPlayerUnit.EMState.Jump ||
            pUnit.emCurState == CPlayerUnit.EMState.HitDrop)
        {
            return;
        }

        Duel(szContent, pUnit);
    }


    public void Duel(string content, CPlayerUnit player)
    {
        string szContent = content.Trim();
        try
        {
            int nLen = content.Length;
            int nCmdIdx = content.IndexOf(CDanmuEventConst.Solo) + CDanmuEventConst.Solo.Length;
            szContent = content.Substring(nCmdIdx, nLen - nCmdIdx).Trim();
            if (CHelpTools.IsStringEmptyOrNone(szContent))
            {
                Debug.Log("异常决斗弹幕:" + content);
                return;
            }
            long nlTargetPrice = 0;
            if (!long.TryParse(szContent, out nlTargetPrice))
            {
                Debug.Log("异常的决斗价格：" + szContent);
                return;
            }
            if (nlTargetPrice < CGameColorFishMgr.Ins.pStaticConfig.GetInt("决斗门票最小价格"))
            {
                UIToast.Show("决斗门票价格过低");
                return;
            }
            if (nlTargetPrice > CGameColorFishMgr.Ins.pStaticConfig.GetInt("决斗门票最大价格"))
            {
                UIToast.Show("决斗门票价格过高");
                return;
            }
            //CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(player.uid);
            //if (pPlayer.GameCoins < nlTargetPrice)
            //{
            //    UIToast.Show(pPlayer.userName + "的积分不够");
            //    //return;
            //}

            UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
            if (roomInfo != null && roomInfo.emCurShowType != ShowBoardType.Battle)
            {
                bool bVtb = player.pInfo.emUserType == CPlayerBaseInfo.EMUserType.Zhubo;
                //同步玩家使用道具的请求
                CPlayerNetHelper.AddFishCoin(player.uid,
                                             nlTargetPrice * -1,
                                             EMFishCoinAddFunc.Duel,
                                             false,
                                             new HHandlerCreatSolo());

                roomInfo.battleRoot.nlCurPrice = nlTargetPrice;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

}
