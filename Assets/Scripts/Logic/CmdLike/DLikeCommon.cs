using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuLikeAttrite(CDanmuLikeConst.Like)]
public class DLikeCommon : CDanmuLikeAction
{
    public override void DoAction(CDanmuLike dm)
    {
        Debug.Log("收到点赞：" + dm.uid + "  次数：" + dm.likeNum);

        string uid = dm.uid.ToString();

        if (CGameColorFishMgr.Ins.pMap == null) return;

        CPlayerBaseInfo pPlayerInfo = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayerInfo == null)
        {
            Debug.Log("玩家：" + dm.nickName + " 想加入游戏");
            //Debug.Log(CPlayerMgr.Ins.dicIdleUnits.Count + "=====当前玩家人数=====" + CPlayerMgr.Ins.dicActiveUnits.Count);
            if (CPlayerMgr.Ins.dicIdleUnits.Count >= CGameColorFishMgr.Ins.nMaxPlayerNum)
            {
                UIToast.Show("人数已满");
                return;
            }

            if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal ||
               CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema ||
               CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Battle ||
               CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive ||
               CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
            {
                if (CGameColorFishMgr.Ins.pMap.GetRandIdleRoot() == null)
                {
                    UIToast.Show("没有空位了");

                    return;
                }
            }
            
            if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
            {
                CPlayerNetHelper.Login(uid, dm.nickName, dm.headIcon, CPlayerBaseInfo.EMUserType.Guanzhong,
                                       1, "", false, 0,
                                       new HHandlerLoginViewer(dm.nickName, dm.headIcon, "", null));
            }
            else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal)
            {
                CPlayerNetHelper.Login(uid, dm.nickName, dm.headIcon, CPlayerBaseInfo.EMUserType.Guanzhong,
                                       1, "", false, 0,
                                       new HHandlerLoginViewer(dm.nickName, dm.headIcon, "", null, CGameColorFishMgr.Ins.JoinPlayer));
            }
            else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
            {
                CPlayerNetHelper.Login(uid, dm.nickName, dm.headIcon, CPlayerBaseInfo.EMUserType.Guanzhong,
                                       1, "", false, 0,
                                       new HHandlerLoginViewer(dm.nickName, dm.headIcon, "", null, CGameColorFishMgr.Ins.JoinPlayer));
            }
            else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema)
            {
                CPlayerNetHelper.Login(uid, dm.nickName, dm.headIcon, CPlayerBaseInfo.EMUserType.Guanzhong,
                                       1, "", false, 0,
                                       new HHandlerLoginViewer(dm.nickName, dm.headIcon, "", null, CGameColorFishMgr.Ins.JoinPlayer));
            }
            else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive)
            {
                if (CGameSurviveMap.Ins != null &&
                   CGameSurviveMap.Ins.emCurState == CGameSurviveMap.EMGameState.Ready)
                {
                    CPlayerNetHelper.Login(uid, dm.nickName, dm.headIcon, CPlayerBaseInfo.EMUserType.Guanzhong,
                                       1, "", false, 0,
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
            CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
            if (pUnit == null)
            {
                pUnit = CPlayerMgr.Ins.GetActiveUnit(uid);
            }
            if (pUnit == null)
            {
                Debug.Log("玩家：" + dm.nickName + " 想加入游戏");
                if (CPlayerMgr.Ins.dicIdleUnits.Count >= CGameColorFishMgr.Ins.nMaxPlayerNum)
                {
                    UIToast.Show("人数已满");
                    return;
                }

                if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal ||
                   CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema ||
                   CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Battle ||
                   CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive ||
                   CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
                {
                    if (CGameColorFishMgr.Ins.pMap.GetRandIdleRoot() == null)
                    {
                        UIToast.Show("没有空位了");

                        return;
                    }
                }
                if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle &&
                CBattleModeMgr.Ins != null &&
                CBattleModeMgr.Ins.emCurState != CBattleModeMgr.EMGameState.Ready)
                {
                    return;
                }
                pPlayerInfo.guardLevel = 0;
                if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
                {
                    CGameBossMgr.Ins.AddWaitPlayer(pPlayerInfo);
                }
                else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal)
                {
                    CGameColorFishMgr.Ins.JoinPlayer(pPlayerInfo, CGameColorFishMgr.EMJoinType.Normal);
                }
                else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
                {
                    CGameColorFishMgr.Ins.JoinPlayer(pPlayerInfo, CGameColorFishMgr.EMJoinType.Normal);
                }
                else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema)
                {
                    CGameColorFishMgr.Ins.JoinPlayer(pPlayerInfo, CGameColorFishMgr.EMJoinType.Normal);
                }
                else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive)
                {
                    if (CGameSurviveMap.Ins != null &&
                       CGameSurviveMap.Ins.emCurState == CGameSurviveMap.EMGameState.Ready)
                    {
                        CGameColorFishMgr.Ins.JoinPlayer(pPlayerInfo, CGameColorFishMgr.EMJoinType.Normal);
                    }
                }
            }
            else
            {
                ///点赞减少拉杆时间
                if (pUnit.emCurState != CPlayerUnit.EMState.Fishing)
                    return;
                FSMUnitFishing fsmUnitFishing = pUnit.pFSM.GetCurState() as FSMUnitFishing;
                if (fsmUnitFishing == null)
                    return;
                if (!CHelpTools.IsStringEmptyOrNone(pUnit.szDianZangEffect))
                {
                    Vector3 vFwdDir = pUnit.tranSelf.forward + pUnit.tranSelf.right;
                    vFwdDir.y = 0;
                    vFwdDir = vFwdDir.normalized;
                    CEffectMgr.Instance.CreateEffSync(pUnit.szDianZangEffect, pUnit.tranSelf.position + Vector3.up * 3f + Vector3.right * 1f, Quaternion.LookRotation(vFwdDir, Vector3.up), 0);
                }
                pUnit.ResetExitTick();
                fsmUnitFishing.DelLaGanTime(CGameColorFishMgr.Ins.pStaticConfig.GetInt("点赞减少拉杆时间"));
            }
        }
    }
}
