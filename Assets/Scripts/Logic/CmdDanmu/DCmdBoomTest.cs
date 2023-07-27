using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CDanmuCmdAttrite(CDanmuEventConst.TestBomber)]
public class DCmdBoomTest : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        string uid = dm.uid.ToString();
        if (CGameColorFishMgr.Ins.pMap == null) return;

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null)
        {
            //先登录
            CPlayerNetHelper.Login(uid, dm.nickName, dm.headIcon, CPlayerBaseInfo.EMUserType.Guanzhong,
                                   dm.fanLv, dm.fanName, dm.fanEquip, dm.vipLv,
                                    new HHandlerLoginViewer(dm.nickName, dm.headIcon, "", null, delegate (CPlayerBaseInfo info, CGameColorFishMgr.EMJoinType joinType)
                                    {
                                        GetBomber(1, info);
                                    }));
        }
        else
        {
            GetBomber(1, pPlayer);
        }
    }

    void GetBomber(long num, CPlayerBaseInfo info)
    {
        ///根据uid获取玩家单位
        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(info.uid);
        if (pUnit == null)
        {
            //Boss战不能直接上船
            if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
            {
                CGameBossMgr.Ins.AddWaitPlayer(info);

                ////给主播加炸弹
                //CPlayerUnit pUnitOwner = CPlayerMgr.Ins.GetIdleUnit(CPlayerMgr.Ins.pOwner.uid);
                //if (pUnitOwner != null)
                //{
                //    CGameBossMgr.Ins.AddPlayerBomber(info.uid, num);

                //    pUnitOwner.AddBoom((int)num);
                //}

                return;
            }

            pUnit = CPlayerMgr.Ins.GetActiveUnit(info.uid);
            if (pUnit == null)
            {
                if (CPlayerMgr.Ins.dicIdleUnits.Count >= CGameColorFishMgr.Ins.nMaxPlayerNum ||
                    CGameColorFishMgr.Ins.pMap.GetRandIdleRoot() == null)
                {
                    UIToast.Show("人数已满");
                    return;
                }

                CGameColorFishMgr.Ins.JoinPlayer(info, CGameColorFishMgr.EMJoinType.Normal);
            }

            pUnit = CPlayerMgr.Ins.GetIdleUnit(info.uid);
            if (pUnit != null)
            {
                if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
                {
                    CGameBossMgr.Ins.AddPlayerBomber(info.uid, num);
                }

                pUnit.AddBoom((int)num);
            }
        }
        else
        {
            if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
            {
                CGameBossMgr.Ins.AddPlayerBomber(info.uid, num);
            }

            pUnit.AddBoom((int)num);
        }

        if (info.CheckHaveGift())
        {
            pUnit.ClearExitTick();
        }
        else
        {
            pUnit.ResetExitTick();
        }
    }
}
