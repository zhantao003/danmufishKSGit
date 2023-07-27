using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CDanmuCmdAttrite(CDanmuEventConst.CrazyTime)]
public class DCmdBuyCrazyTime : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        return;
        if (CCrazyTimeMgr.Ins == null) return;

        string uid = dm.uid.ToString();
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null) return;

        if (pPlayer.nVipPlayer != CPlayerBaseInfo.EMVipLv.Zongdu && 
            pPlayer.nVipPlayer != CPlayerBaseInfo.EMVipLv.Pro) return;

        //判断钱够不够
        if(pPlayer.GameCoins < CGameColorFishMgr.Ins.pStaticConfig.GetInt("总督急速价格"))
        {
            UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
            if (uiGameInfo != null && uiGameInfo.IsOpen())
            {
                UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(uid);
                if (uiUnitInfo != null)
                {
                    uiUnitInfo.SetDmContent("积分不足启动急速");
                }
            }

            return;
        }

        //同步玩家使用道具的请求
        CPlayerNetHelper.AddFishCoin(pPlayer.uid,                                     
                                     CGameColorFishMgr.Ins.pStaticConfig.GetInt("总督急速价格") * -1,
                                     EMFishCoinAddFunc.Pay,
                                     true,
                                     new HHandlerVipBuyCrazyTime());
    }
}
