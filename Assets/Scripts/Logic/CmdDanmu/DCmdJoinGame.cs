using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuCmdAttrite(CDanmuEventConst.JoinQueue)]
public class DCmdJoinGame : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        ////影院模式不能正常加入
        //if(CPlayerMgr.Ins.pOwner.uid == 559642 ||
        //   CPlayerMgr.Ins.pOwner.uid == dm.uid)
        //{

        //}
        //else
        //{
        //    if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema) return;
        //}

        string uid = dm.uid.ToString();

        if (CGameColorFishMgr.Ins.pMap == null) return;

        Debug.Log("玩家：" + dm.nickName + " 想加入游戏");
        //Debug.Log(CPlayerMgr.Ins.dicIdleUnits.Count + "=====当前玩家人数=====" + CPlayerMgr.Ins.dicActiveUnits.Count);
        if(CPlayerMgr.Ins.dicIdleUnits.Count >= CGameColorFishMgr.Ins.nMaxPlayerNum)
        {
            UIToast.Show("人数已满");
            return;
        }

        if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal ||
           CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema ||
           CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Battle ||
           CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive ||
           CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
        {
            if(CGameColorFishMgr.Ins.pMap.GetRandIdleRoot() == null)
            {
                UIToast.Show("没有空位了");

                return;
            }
        }

        CPlayerBaseInfo pPlayerInfo = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayerInfo == null)
        {
            if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
            {
                CPlayerNetHelper.Login(uid, dm.nickName, dm.headIcon, CPlayerBaseInfo.EMUserType.Guanzhong,
                                       dm.fanLv, dm.fanName, dm.fanEquip, dm.vipLv,
                                       new HHandlerLoginViewer(dm.nickName, dm.headIcon, "", null));
            }
            else if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal)
            {
                CPlayerNetHelper.Login(uid, dm.nickName, dm.headIcon, CPlayerBaseInfo.EMUserType.Guanzhong,
                                       dm.fanLv, dm.fanName, dm.fanEquip, dm.vipLv,
                                       new HHandlerLoginViewer(dm.nickName, dm.headIcon, "", null, CGameColorFishMgr.Ins.JoinPlayer));
            }
            else if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
            {
                CPlayerNetHelper.Login(uid, dm.nickName, dm.headIcon, CPlayerBaseInfo.EMUserType.Guanzhong,
                                       dm.fanLv, dm.fanName, dm.fanEquip, dm.vipLv,
                                       new HHandlerLoginViewer(dm.nickName, dm.headIcon, "", null, CGameColorFishMgr.Ins.JoinPlayer));
            }
            else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema)
            {
                CPlayerNetHelper.Login(uid, dm.nickName, dm.headIcon, CPlayerBaseInfo.EMUserType.Guanzhong,
                                       dm.fanLv, dm.fanName, dm.fanEquip, dm.vipLv,
                                       new HHandlerLoginViewer(dm.nickName, dm.headIcon, "", null, CGameColorFishMgr.Ins.JoinPlayer));
            }
            else if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive)
            {
                if(CGameSurviveMap.Ins!=null && 
                   CGameSurviveMap.Ins.emCurState == CGameSurviveMap.EMGameState.Ready)
                {
                    CPlayerNetHelper.Login(uid, dm.nickName, dm.headIcon, CPlayerBaseInfo.EMUserType.Guanzhong,
                                       dm.fanLv, dm.fanName, dm.fanEquip, dm.vipLv,
                                       new HHandlerLoginViewer(dm.nickName, dm.headIcon, "", null, CGameColorFishMgr.Ins.JoinPlayer));
                }
            }
            else
            {
                //CHttpMgr.Instance.SendHttpMsg(CHttpConst.LoginViewer,
                //                             new HHandlerLoginViewer(dm.userName, dm.userFace),
                //                             pParamLogin);
            }
        }
        else
        {
            pPlayerInfo.guardLevel = dm.vipLv;

            if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
            {
                CGameBossMgr.Ins.AddWaitPlayer(pPlayerInfo);
            }
            else if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal)
            {
                CGameColorFishMgr.Ins.JoinPlayer(pPlayerInfo, CGameColorFishMgr.EMJoinType.Normal);
            }
            else if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
            {
                CGameColorFishMgr.Ins.JoinPlayer(pPlayerInfo, CGameColorFishMgr.EMJoinType.Normal);
            }
            else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema)
            {
                CGameColorFishMgr.Ins.JoinPlayer(pPlayerInfo, CGameColorFishMgr.EMJoinType.Normal);
            }
            else if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive)
            {
                if (CGameSurviveMap.Ins != null &&
                   CGameSurviveMap.Ins.emCurState == CGameSurviveMap.EMGameState.Ready)
                {
                    CGameColorFishMgr.Ins.JoinPlayer(pPlayerInfo, CGameColorFishMgr.EMJoinType.Normal);
                }
            }

            if (UIManager.Instance.GetUI(UIResType.Help) as UIHelp != null &&
                UIHelp.emHelpLv == EMHelpLev.Lev1_MoYu &&
                pPlayerInfo.emUserType == CPlayerBaseInfo.EMUserType.Zhubo)
            {
                UIHelp.GoNextHelpLv();
            }
        }
    }

}
