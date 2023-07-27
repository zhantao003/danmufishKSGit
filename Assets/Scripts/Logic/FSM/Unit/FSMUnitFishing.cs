using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMUnitFishing : FSMUnitBase
{
    CPropertyTimer pCheckTime = new CPropertyTimer();

    CPropertyTimer pCheckLaGanState = new CPropertyTimer();

    CPropertyTimer pCheckFeiLun = new CPropertyTimer();

    public override void OnBegin(object obj)
    {
        //Debug.Log("Fishing");
        pUnit.SetLaGanState(false);
        pUnit.emCurState = CPlayerUnit.EMState.Fishing;

        pUnit.PlayAnime(CUnitAnimeConst.Anime_Fishing);

        if (pUnit.pAvatar != null)
        {
            pUnit.pAvatar.PlaySpringMgr(false);
        }

        //CGameColorFishMgr.Ins.pStaticConfig.GetInt("鱼群缩短拉杆时间");
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(pUnit.uid);
        pCheckLaGanState = new CPropertyTimer();
        if (CCrazyTimeMgr.Ins != null && CCrazyTimeMgr.Ins.bCrazy)
        {
            if (pPlayer != null)
            {
                if (pPlayer.nlFeiLunCount > 0)
                {
                    //Boss战准备阶段不消耗
                    if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss &&
                        CGameBossMgr.Ins != null &&
                        CGameBossMgr.Ins.emCurState == CGameBossMgr.EMState.Ready)
                    {
                        pCheckLaGanState.Value = CGameColorFishMgr.Ins.pStaticConfig.GetInt("狂暴拉杆时间");
                    }
                    else
                    {
                        pCheckLaGanState.Value = CGameColorFishMgr.Ins.pStaticConfig.GetInt("狂暴拉杆时间") * (1 - CGameColorFishMgr.Ins.pStaticConfig.GetInt("飞轮减少咬钩时间") * 0.01f);
                    }
                }
                else
                {
                    pCheckLaGanState.Value = CGameColorFishMgr.Ins.pStaticConfig.GetInt("狂暴拉杆时间");
                }
            }
            else
            {
                pCheckLaGanState.Value = CGameColorFishMgr.Ins.pStaticConfig.GetInt("狂暴拉杆时间");
            }
        }
        else
        {
            if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
            {
                pCheckLaGanState.Value = CGameColorFishMgr.Ins.pStaticConfig.GetInt("Boss关拉杆时间");
            }
            else
            {
                float fRandomValue = Random.Range(pUnit.vecChgLaGanStateTimeRange.x, pUnit.vecChgLaGanStateTimeRange.y);
                if (pPlayer != null)
                {
                    if (pPlayer.nlFeiLunCount > 0)
                    {
                        //Boss战准备阶段不消耗
                        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss &&
                            CGameBossMgr.Ins != null &&
                            CGameBossMgr.Ins.emCurState == CGameBossMgr.EMState.Ready)
                        {
                            pCheckLaGanState.Value = fRandomValue;
                        }
                        else
                        {
                            pCheckLaGanState.Value = fRandomValue * (1 - CGameColorFishMgr.Ins.pStaticConfig.GetInt("飞轮减少咬钩时间") * 0.01f);
                        }
                    }
                    else
                    {
                        pCheckLaGanState.Value = fRandomValue;
                    }
                }
                else
                {
                    pCheckLaGanState.Value = fRandomValue;
                }
            }
        }

        
        if (pPlayer != null)
        {
            if (pPlayer.nlFeiLunCount > 0)
            {
                //Boss战准备阶段不消耗
                if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss &&
                    CGameBossMgr.Ins != null &&
                    CGameBossMgr.Ins.emCurState == CGameBossMgr.EMState.Ready)
                {

                }
                else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle &&
                         CBattleModeMgr.Ins != null &&
                         CBattleModeMgr.Ins.emCurState != CBattleModeMgr.EMGameState.Gaming)
                {
                    
                }
                else
                {
                    pUnit.AddGiftByHttp(-1, EMGiftType.fishLun);
                }
            }
        }

        pCheckLaGanState.FillTime();
        pCheckTime.Value = pUnit.fFinishStayTime + Random.Range(-2f, 2f);
        pCheckFeiLun = null;
        pCheckTime.FillTime();
    }

    public override void OnFixedUpdate(object obj, float delta)
    {
        if (CGameColorFishMgr.Ins != null &&
            CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle &&
            CBattleModeMgr.Ins != null &&
            CBattleModeMgr.Ins.emCurState != CBattleModeMgr.EMGameState.Gaming)
            return;
        ///验证码阶段不继续走该阶段
        if (pUnit.bCheckYZM ||
            pUnit.pInfo.CheckIsGrayName())
            return;
        if (pCheckFeiLun != null && 
           pCheckFeiLun.Tick(delta))
        {
            //CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(pUnit.uid);
            //if (pPlayer != null)
            //{
            //    if (pPlayer.nlFeiLunCount > 0)
            //    {
            //        //Boss战准备阶段不消耗
            //        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss &&
            //            CGameBossMgr.Ins != null &&
            //            CGameBossMgr.Ins.emCurState == CGameBossMgr.EMState.Ready)
            //        {

            //        }
            //        else
            //        {
            //            pUnit.AddGiftByHttp(-1, EMGiftType.fishLun);
            //        }   
            //    }
            //}
            pUnit.SetState(CPlayerUnit.EMState.EndFish);

            if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal ||
                CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema ||
                CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss ||
                CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive ||
                CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
            {
                pUnit.DoLagan(2.4F);
            }
            else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Battle)
            {
                pUnit.GetFishVsModel();
            }

            pCheckFeiLun = null;

            return;
        }

        if (pCheckTime.Tick(delta))
        {
            pUnit.SetState(CPlayerUnit.EMState.EndFish);

            if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal ||
                CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema ||
                CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss ||
                CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive ||
                CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
            {
                pUnit.DoLagan(2.4F);
            }
            else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Battle)
            {
                pUnit.GetFishVsModel();
            }

            pUnit.dlgChgFishValue?.Invoke(1f - pCheckTime.GetTimeLerp());

            return;
        }
        else
        {
            pUnit.dlgChgFishValue?.Invoke(1f - pCheckTime.GetTimeLerp());
        }

        if(pCheckLaGanState != null &&
           pCheckLaGanState.Tick(delta))
        {
            pUnit.SetLaGanState(true);
            pCheckLaGanState = null;
            CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(pUnit.uid);
            if (pPlayer != null)
            {
                if (pPlayer.nlFeiLunCount > 0)
                {
                    pCheckFeiLun = new CPropertyTimer();
                    pCheckFeiLun.Value = 3f;
                    pCheckFeiLun.FillTime();
                }
            }

            return;
        }
    }

    public void DelLaGanTime(float delta)
    {
        if (pUnit.bCanLaGan)
        {
            ///进行拉杆动作
            pUnit.SetState(CPlayerUnit.EMState.EndFish);

            if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal ||
                CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema ||
                CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss ||
                CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive ||
                CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
            {
                pUnit.DoLagan(2.4F);
            }
            else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Battle)
            {
                pUnit.GetFishVsModel();
            }
        }
        else
        {
            if (pCheckLaGanState != null)
            {
                pCheckLaGanState.CurValue -= delta;
            }
        }
    }

    public override void OnEnd(object obj)
    {
        base.OnEnd(obj);
        pUnit.fCurFinishValue = 1f - pCheckTime.GetTimeLerp();
        pUnit.SetLaGanState(false);
    }

}

