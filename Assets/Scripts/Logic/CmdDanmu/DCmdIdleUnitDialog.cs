using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuCmdAttrite(CDanmuEventConst.Common_IdleUnitDialog)]
public class DCmdIdleUnitDialog : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        string uid = dm.uid;

        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if (pUnit == null)
        {
            pUnit = CPlayerMgr.Ins.GetActiveUnit(uid);

            if (pUnit == null)
                return;
        }

        ///每当玩家发送弹幕时刷新该玩家的活跃状态
        //pUnit.SetActiveState(true);
        if (dm.content == CDanmuEventConst.LaGan ||
            dm.content == CDanmuEventConst.Chaxun ||
            dm.content == CDanmuEventConst.ExitGame ||
            //dm.content == CDanmuEventConst.Buy ||
            //dm.content == CDanmuEventConst.BuyTen ||
            dm.content.StartsWith(CDanmuEventConst.Chaxun_Avatar) ||
            //dm.content.StartsWith(CDanmuEventConst.ExchangeAvatar) ||
            //dm.content.StartsWith(CDanmuEventConst.ExchangeAvatar2) ||
            dm.content.StartsWith(CDanmuEventConst.ChgAvatar) ||
            //dm.content.StartsWith(CDanmuEventConst.ChgAvatar2) ||
            dm.content.Contains(CDanmuEventConst.Chaxun_Boat) ||
            //dm.content.Contains(CDanmuEventConst.Chaxun_FishGan) ||
            //dm.content == CDanmuEventConst.Chaxun_FesInfo ||
            //dm.content == CDanmuEventConst.Chaxun_ProAdd ||
            //dm.content == CDanmuEventConst.Chaxun_FishMat ||
            //dm.content == CDanmuEventConst.Chaxun_Limit ||
            //dm.content == CDanmuEventConst.AddDuel ||
            dm.content == CDanmuEventConst.Zibeng ||
            //dm.content == CDanmuEventConst.TieTie ||
            dm.content == CDanmuEventConst.TiePlayer ||
            dm.content == CDanmuEventConst.GetOut)
        {
            return;
        }

        UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (uiGameInfo == null || !uiGameInfo.IsOpen()) return;

        UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(pUnit.uid);
        if (uiUnitInfo == null) return;

        if(uiUnitInfo.bCheckAnswer(dm.content))
        {
            uiUnitInfo.SetDmContent("验证正确");
        }
        else
        {
            uiUnitInfo.SetDmContent(dm.content);
        }
    }
}