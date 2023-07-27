using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBattleModeMgr : MonoBehaviour
{
    static CBattleModeMgr ins = null;

    public static CBattleModeMgr Ins
    {
        get
        {
            return ins;
        }
    }

    public enum EMGameState
    {
        Ready,
        Gaming,
        End,
    }

    public DelegateFFuncCall deleValueChg;

    [Header("当前的游戏状态")]
    public EMGameState emCurState;
    [Header("每局的准备时间")]
    public float fReadyTime;
    [Header("每局的游戏时间")]
    public float fGameTime;
    [Header("每局的结束时间")]
    public float fEndTime;

    CPropertyTimer pTimeTicker;


    private void Start()
    {
        ins = this;
        Init();
    }

    public void Init()
    {
        emCurState = EMGameState.Ready;
        SetGameTick();
        DoGameEvent();
    }

    private void Update()
    {
        
        if (pTimeTicker != null)
        {
            if (pTimeTicker.Tick(CTimeMgr.DeltaTime))
            {
                deleValueChg?.Invoke(0);
                pTimeTicker = null;
                CheckGameState();
            }
            else
            {
                deleValueChg?.Invoke(pTimeTicker.CurValue);
            }
        }
    }

    public void QuickStart()
    {
        if(emCurState != EMGameState.Ready)
        {
            return;
        }
        if(pTimeTicker != null)
        {
            deleValueChg?.Invoke(0);
            pTimeTicker = null;
            CheckGameState();
        }
    }

    /// <summary>
    /// 设置游戏Tick
    /// </summary>
    public void SetGameTick()
    {
        if (emCurState == EMGameState.Ready)
        {
            pTimeTicker = new CPropertyTimer();
            pTimeTicker.Value = fReadyTime;
            pTimeTicker.FillTime();
        }
        else if (emCurState == EMGameState.Gaming)
        {
            pTimeTicker = new CPropertyTimer();
            pTimeTicker.Value = fGameTime;
            pTimeTicker.FillTime();
        }
        else if (emCurState == EMGameState.End)
        {
            pTimeTicker = new CPropertyTimer();
            pTimeTicker.Value = fEndTime;
            pTimeTicker.FillTime();
        }
    }

    /// <summary>
    /// 检测游戏状态
    /// </summary>
    public void CheckGameState()
    {
        if (emCurState == EMGameState.Ready)
        {
            if (CPlayerMgr.Ins.GetAllIdleUnit().Count >= CGameColorFishMgr.Ins.pStaticConfig.GetInt("最低开始人数"))
            {
                emCurState = EMGameState.Gaming;
            }
            else
            {
                UIToast.Show("人数不够");
                emCurState = EMGameState.Ready;
            }
        }
        else if (emCurState == EMGameState.Gaming)
        {
            emCurState = EMGameState.End;
        }
        else if (emCurState == EMGameState.End)
        {
            emCurState = EMGameState.Ready;
            
        }
        SetGameTick();
        DoGameEvent();
    }

    /// <summary>
    /// 做对应状态的事情
    /// </summary>
    public void DoGameEvent()
    {
        if (emCurState == EMGameState.Ready)
        {
            if (CFishRecordMgr.Ins != null)
            {
                CFishRecordMgr.Ins.Clear();
            }
            UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
            if (roomInfo != null)
            {
                roomInfo.ClearRankInfo();
            }
        }
        else if (emCurState == EMGameState.Gaming)
        {
            if (CGameColorFishMgr.Ins.pMap != null)
            {
                CGameColorFishMgr.Ins.pMap.DoShowTick();
            }
            UIBattleModeInfo battleModeInfo = UIManager.Instance.GetUI(UIResType.BattleModeInfo) as UIBattleModeInfo;
            if (battleModeInfo != null)
            {
                battleModeInfo.StartShow();
            }
            List<CPlayerUnit> playerUnits = CPlayerMgr.Ins.GetAllIdleUnit();
            for(int i = 0;i < playerUnits.Count;i++)
            {
                if (playerUnits[i] == null) continue;
                playerUnits[i].SetState(CPlayerUnit.EMState.StartFish);
            }
        }
        else if (emCurState == EMGameState.End)
        {
            if (CGameColorFishMgr.Ins.pMap != null)
            {
                CGameColorFishMgr.Ins.pMap.EndBattleTick();
            }
            List<CPlayerUnit> playerUnits = CPlayerMgr.Ins.GetAllIdleUnit();
            for (int i = 0; i < playerUnits.Count; i++)
            {
                if (playerUnits[i] == null) continue;
                playerUnits[i].SetState(CPlayerUnit.EMState.Idle);
            }
            List<OuHuangRankInfo> listOuHuangInfos = COuHuangRankMgr.Ins.GetRankInfos();

            ///判断是否存在欧皇
            if (listOuHuangInfos.Count <= 0)
            {
                emCurState = EMGameState.Ready;
                SetGameTick();
                DoGameEvent();
            }
            else
            {
                CGetChampionRule curChampionRule = CChampionMgr.Ins.GetCurRule();

                UIBattleModeInfo battleModeInfo = UIManager.Instance.GetUI(UIResType.BattleModeInfo) as UIBattleModeInfo;
                if (battleModeInfo != null)
                {
                    battleModeInfo.InitRankInfo(curChampionRule);
                }

                List<ProfitRankInfo> listProfitInfos = CProfitRankMgr.Ins.GetRankInfos();
                if (listProfitInfos.Count > 0)
                {
                    for(int i=0; i<listProfitInfos.Count; i++)
                    {
                        CPlayerBaseInfo pShowPlayerInfo = CPlayerMgr.Ins.GetPlayer(listProfitInfos[i].nlUserUID);
                        if (pShowPlayerInfo == null) continue;

                        if(pShowPlayerInfo.avatarId == 101)
                        {
                            pShowPlayerInfo.RefreshRoleAvatar();
                        }

                        CPlayerUnit pShowUnit = CPlayerMgr.Ins.GetIdleUnit(listProfitInfos[i].nlUserUID);
                        if (pShowUnit == null) continue;
                        pShowUnit.InitAvatar();
                    }

                    if (curChampionRule != null)
                    {
                        for (int i = 0; i < curChampionRule.nGetChampion.Length; i++)
                        {
                            string szRankUID = string.Empty;
                            string szProfitUID = string.Empty;
                            if (i < listOuHuangInfos.Count && 
                                listOuHuangInfos[i] != null)
                            {
                                szRankUID = listOuHuangInfos[i].nlUserUID;
                            }
                            if (i < listProfitInfos.Count &&
                                listProfitInfos[i] != null)
                            {
                                szProfitUID = listProfitInfos[i].nlUserUID;
                            }
                            if(CHelpTools.IsStringEmptyOrNone(szRankUID) &&
                               CHelpTools.IsStringEmptyOrNone(szProfitUID))
                            {
                                continue;
                            }
                            if (szRankUID == szProfitUID)
                            {
                                CPlayerNetHelper.AddWinnerInfo(szRankUID, curChampionRule.nGetChampion[i] + 1, 0);
                            }
                            else
                            {
                                if (!CHelpTools.IsStringEmptyOrNone(szRankUID))
                                {
                                    CPlayerNetHelper.AddWinnerInfo(szRankUID, 1, 0);
                                }
                                if (!CHelpTools.IsStringEmptyOrNone(szProfitUID))
                                {
                                    CPlayerNetHelper.AddWinnerInfo(szProfitUID, curChampionRule.nGetChampion[i], 0);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (listOuHuangInfos[0].nlUserUID == listProfitInfos[0].nlUserUID)
                        {
                            CPlayerNetHelper.AddWinnerInfo(listOuHuangInfos[0].nlUserUID, 2, 0);
                        }
                        else
                        {
                            CPlayerNetHelper.AddWinnerInfo(listOuHuangInfos[0].nlUserUID, 1, 0);
                            CPlayerNetHelper.AddWinnerInfo(listProfitInfos[0].nlUserUID, 1, 0);
                        }
                    }
                }
            }

            UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
            if (roomInfo != null &&
                roomInfo.bShowRank)
            {
                roomInfo.OnClickShowRank();
            }

            UISpecialCast specialCast = UIManager.Instance.GetUI(UIResType.SpecialCast) as UISpecialCast;
            if(specialCast != null)
            {
                specialCast.Clear();
            }

            UICheckIn checkIn = UIManager.Instance.GetUI(UIResType.CheckIn) as UICheckIn;
            if (checkIn != null)
            {
                checkIn.Clear();
            }

        }
    }

    public void ForceEnd()
    {
        pTimeTicker = new CPropertyTimer();
        pTimeTicker.Value = 2f;
        pTimeTicker.FillTime();
    }
}
