using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EMTeamBattleCamp
{
    Red,
    Blue,
}

public class UITeamBattleRoot : MonoBehaviour
{
    /// <summary>
    /// 获胜阵营
    /// </summary>
    EMTeamBattleCamp emVictoryCamp;
    [Header("决斗准备")]
    public UITeamBattleReady battleReady;
    [Header("决斗结算")]
    public UITeamBattleShow battleShow;
    /// <summary>
    /// 决斗玩家信息
    /// </summary>
    public List<SpecialCastInfo> listTeamAInfos = new List<SpecialCastInfo>();
    public List<SpecialCastInfo> listTeamBInfos = new List<SpecialCastInfo>();

    public List<SpecialCastInfo> listPlayerInfos = new List<SpecialCastInfo>();

    public int nMaxCount;
    /// <summary>
    /// 当前的门票钱
    /// </summary>
    public long nlCurPrice;
    /// <summary>
    /// 总奖励
    /// </summary>
    public long nlTotalPrice;

    /// <summary>
    /// 结束计时器
    /// </summary>
    CPropertyTimer pCheckEndTick;

    /// <summary>
    /// 是否正在结算
    /// </summary>
    public bool bResultTime;
    /// <summary>
    /// 是否已经满人
    /// </summary>
    public bool bMax;

    public void InitInfo()
    {
        Clear();
        bResultTime = false;
        bMax = false;
        pCheckEndTick = null;
        UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
        if (roomInfo != null)
        {
            
        }
        battleReady.SetActive(true);
        battleShow.SetActive(false);
        battleReady.InitInfo(nlCurPrice);
        battleReady.SetPlayerCount(nMaxCount, listPlayerInfos.Count);
        battleReady.pEndEvent = EndReady;
    }

    void EndReady()
    {
        ///ui动画处理
        battleReady.SetActive(false);
        if (listTeamAInfos.Count <= 0 ||
            listTeamBInfos.Count <= 0)
        {
            battleShow.SetActive(false);
            UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
            if (roomInfo != null)
            {
                roomInfo.HideBoard(roomInfo.uiBattleTween, 0.5f, 0f);
            }
        }
        else
        {
            battleShow.SetActive(true);
        }
        ///获得决斗用的鱼
        for (int i = 0; i < listTeamAInfos.Count; i++)
        {
            CPlayerUnit playerUnit = CPlayerMgr.Ins.GetIdleUnit(listTeamAInfos[i].playerInfo.uid);
            if (playerUnit == null) continue;
            listTeamAInfos[i].fishInfo = playerUnit.GetBattleFish();
        }
        for (int i = 0; i < listTeamBInfos.Count; i++)
        {
            CPlayerUnit playerUnit = CPlayerMgr.Ins.GetIdleUnit(listTeamBInfos[i].playerInfo.uid);
            if (playerUnit == null) continue;
            listTeamBInfos[i].fishInfo = playerUnit.GetBattleFish();
        }
        SortInfo();
        Refresh();
        ///结算倒计时
        pCheckEndTick = new CPropertyTimer();
        pCheckEndTick.Value = CGameColorFishMgr.Ins.pStaticConfig.GetInt("决斗结算倒计时");
        pCheckEndTick.FillTime();
        bResultTime = true;
        ///决斗结束后 玩家的状态处理
        CheckBattleEndState(EMTeamBattleCamp.Red);
        CheckBattleEndState(EMTeamBattleCamp.Blue);
    }

    /// <summary>
    /// 检测决斗结束后玩家的状态
    /// </summary>
    /// <param name="checkCamp"></param>
    void CheckBattleEndState(EMTeamBattleCamp checkCamp)
    {
        List<SpecialCastInfo> listPlayerInfos = new List<SpecialCastInfo>();
        if(checkCamp == EMTeamBattleCamp.Red)
        {
            listPlayerInfos.AddRange(listTeamAInfos);
        }
        else if(checkCamp == EMTeamBattleCamp.Blue)
        {
            listPlayerInfos.AddRange(listTeamBInfos);
        }
        for (int i = 0; i < listPlayerInfos.Count; i++)
        {
            CPlayerUnit playerUnit = CPlayerMgr.Ins.GetIdleUnit(listPlayerInfos[i].playerInfo.uid);
            if (playerUnit == null) continue;
            ///判断是否为获胜队伍
            if (emVictoryCamp != checkCamp)
            {
                ///未获胜 跳回自己之前待着的格子
                playerUnit.AddBattleValue();
                playerUnit.SetState(CPlayerUnit.EMState.EndFish);
                playerUnit.pShowFishEndEvent = delegate ()
                {
                    CPlayerUnit targetUnit = CPlayerMgr.Ins.GetIdleUnit(listPlayerInfos[i].playerInfo.uid);
                    playerUnit.PlayGoldEffectToTarget(targetUnit.tranSelf);
                    CTimeTickMgr.Inst.PushTicker(0.7f, delegate (object[] obj)
                    {
                        playerUnit.BackPreSlot();
                    });
                };
            }
            else
            {
                ///获胜
                playerUnit.ResetBattleValue();
                CTimeTickMgr.Inst.PushTicker(playerUnit.fFinishEndTime, delegate (object[] obj)
                {
                    playerUnit.PlayAnime(CUnitAnimeConst.Anime_ShowFish);
                    UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
                    if (gameInfo != null)
                    {
                        UIShowFishInfo showFishInfo = gameInfo.GetShowFishInfoSlot(playerUnit.uid);
                        showFishInfo.ShowFishRoot();
                    }
                });
                playerUnit.PlayAnime(CUnitAnimeConst.Anime_EndFish);
                int nMengPiaoGold = (int)nlCurPrice;
                UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
                if (gameInfo != null)
                {
                    UIShowFishInfo showFishInfo = gameInfo.GetShowFishInfoSlot(listPlayerInfos[i].playerInfo.uid);
                    showFishInfo.SetAddCoin(nMengPiaoGold, 0);
                }
            }
        }
    }

    private void Update()
    {
        if (pCheckEndTick != null)
        {
            if (pCheckEndTick.Tick(CTimeMgr.DeltaTime))
            {
                pCheckEndTick = null;
                CheckFirstPlayer();
                bResultTime = false;
            }
        }
    }

    public void Clear()
    {
        CGameColorFishMgr.Ins.pMap.pDuelBoat.Clear();
        listTeamAInfos.Clear();
        listTeamBInfos.Clear();
        Refresh();
    }

    /// <summary>
    /// 检测需要调向船的玩家
    /// </summary>
    public void CheckNeedJumpBoat()
    {
        for (int i = 0; i < listTeamAInfos.Count; i++)
        {
            CPlayerUnit playerUnit = CPlayerMgr.Ins.GetIdleUnit(listTeamAInfos[i].playerInfo.uid);
            if (playerUnit == null) continue;
            CMapSlot mapSlot = CGameColorFishMgr.Ins.pMap.pDuelBoat.GetIdleSlot();
            if (mapSlot == null) continue;

            playerUnit.JumpTarget(mapSlot, false);
            mapSlot.BindPlayer(playerUnit);
        }

        for (int i = 0; i < listTeamBInfos.Count; i++)
        {
            CPlayerUnit playerUnit = CPlayerMgr.Ins.GetIdleUnit(listTeamBInfos[i].playerInfo.uid);
            if (playerUnit == null) continue;
            CMapSlot mapSlot = CGameColorFishMgr.Ins.pMap.pDuelBoat.GetIdleSlot();
            if (mapSlot == null) continue;

            playerUnit.JumpTarget(mapSlot, false);
            mapSlot.BindPlayer(playerUnit);
        }
    }


    /// <summary>
    /// 判断是否有该玩家
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    public bool bHavePlayer(string uid,EMTeamBattleCamp camp)
    {
        bool bHave = false;
        if (camp == EMTeamBattleCamp.Red)
        {
            for (int i = 0; i < listTeamAInfos.Count; i++)
            {
                if (listTeamAInfos[i].playerInfo.uid.Equals(uid))
                {
                    bHave = true;
                    break;
                }
            }
        }
        else if (camp == EMTeamBattleCamp.Blue)
        {
            for (int i = 0; i < listTeamBInfos.Count; i++)
            {
                if (listTeamBInfos[i].playerInfo.uid.Equals(uid))
                {
                    bHave = true;
                    break;
                }
            }
        }
        return bHave;
    }

    /// <summary>
    /// 移除对应id的玩家
    /// </summary>
    /// <param name="uid"></param>
    public void RemovePlayerInfo(string uid)
    {
        for (int i = 0; i < listTeamAInfos.Count; i++)
        {
            if (listTeamAInfos[i].playerInfo.uid.Equals(uid))
            {
                listTeamAInfos.Remove(listPlayerInfos[i]);
                break;
            }
        }
        for (int i = 0; i < listTeamBInfos.Count; i++)
        {
            if (listTeamBInfos[i].playerInfo.uid.Equals(uid))
            {
                listTeamBInfos.Remove(listPlayerInfos[i]);
                break;
            }
        }
    }

    /// <summary>
    /// 增加玩家信息
    /// </summary>
    /// <param name="castInfo"></param>
    public bool AddPlayerInfo(SpecialCastInfo castInfo,EMTeamBattleCamp camp)
    {
        bool bAddSuc = false; ;
        bool bNeedRefresh = false;
        UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
        if (roomInfo == null)
            return false;
        if(camp == EMTeamBattleCamp.Red)
        {
            if(listTeamAInfos.Count < nMaxCount &&
               !bHavePlayer(castInfo.playerInfo.uid, camp))
            {
                listTeamAInfos.Add(castInfo);
                bNeedRefresh = true;
                nlTotalPrice += nlCurPrice;
                battleReady.SetPlayerCount(nMaxCount * 2, listTeamAInfos.Count + listTeamBInfos.Count);
                if (listTeamAInfos.Count > 1 && roomInfo.emCurShowType == ShowBoardType.Battle)
                {
                    battleReady.SetTotalReward(((long)System.Convert.ToInt32(nlTotalPrice * (1f - CGameColorFishMgr.Ins.pStaticConfig.GetInt("决斗抽成比例") * 0.0001f))));
                }
                else
                {
                    battleReady.SetTotalReward(nlTotalPrice);
                }
                bAddSuc = true;
            }
        }
        else if(camp == EMTeamBattleCamp.Blue)
        {
            if (listTeamBInfos.Count < nMaxCount &&
                !bHavePlayer(castInfo.playerInfo.uid, camp))
            {
                listTeamBInfos.Add(castInfo);
                bNeedRefresh = true;
                nlTotalPrice += nlCurPrice;
                battleReady.SetPlayerCount(nMaxCount * 2, listTeamAInfos.Count + listTeamBInfos.Count);
                if (listTeamBInfos.Count > 1 && roomInfo.emCurShowType == ShowBoardType.Battle)
                {
                    battleReady.SetTotalReward(((long)System.Convert.ToInt32(nlTotalPrice * (1f - CGameColorFishMgr.Ins.pStaticConfig.GetInt("决斗抽成比例") * 0.0001f))));
                }
                else
                {
                    battleReady.SetTotalReward(nlTotalPrice);
                }
                bAddSuc = true;
            }
        }
        ///判断是否需要刷新信息
        if (bNeedRefresh)
        {
            Refresh();
        }
        ///判断是否已经满人
        if (listTeamAInfos.Count >= nMaxCount &&
            listTeamBInfos.Count >= nMaxCount)
        {
            bMax = true;
        }
        return bAddSuc;
    }

    /// <summary>
    /// 信息排序
    /// </summary>
    void SortInfo()
    {
        listTeamAInfos.Sort((x, y) =>
        {
            if (x.fishInfo == null ||
                y.fishInfo == null)
                return 1;
            if (y.fishInfo.lPrice < x.fishInfo.lPrice)
            {
                return -1;
            }
            else if (y.fishInfo.lPrice == x.fishInfo.lPrice)
            {
                if (x.fishInfo.fCurSize > y.fishInfo.fCurSize)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
        });

        listTeamBInfos.Sort((x, y) =>
        {
            if (x.fishInfo == null ||
                y.fishInfo == null)
                return 1;
            if (y.fishInfo.lPrice < x.fishInfo.lPrice)
            {
                return -1;
            }
            else if (y.fishInfo.lPrice == x.fishInfo.lPrice)
            {
                if (x.fishInfo.fCurSize > y.fishInfo.fCurSize)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
        });
    }

    /// <summary>
    /// 刷新
    /// </summary>
    public void Refresh()
    {
        battleReady.RefreshPlayerInfo(listTeamAInfos, EMTeamBattleCamp.Red);
        battleReady.RefreshPlayerInfo(listTeamBInfos, EMTeamBattleCamp.Blue);

        battleShow.RefreshPlayerInfo(listTeamAInfos, EMTeamBattleCamp.Red);
        battleShow.RefreshPlayerInfo(listTeamBInfos, EMTeamBattleCamp.Blue);
    }

    /// <summary>
    /// 检测第一名
    /// </summary>
    public void CheckFirstPlayer()
    {
        UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
        CGameColorFishMgr.Ins.pMap.DoCameraTween(false);
        if (listTeamAInfos.Count <= 0 &&
            listTeamBInfos.Count <= 0)
        {
            UIRoomInfo uiRoomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
            if (uiRoomInfo != null)
                uiRoomInfo.ShowInfo(5f, false);
            nlCurPrice = 0;
            nlTotalPrice = 0;
            bMax = false;
            return;
        }
        bool bSuc = listTeamAInfos.Count > 0 && listTeamBInfos.Count > 0;
        ///判断是否成功建立了决斗（成功则抽成）
        if (roomInfo.emCurShowType == ShowBoardType.Battle)
        {
            if (bSuc)
            {
                nlTotalPrice = (long)System.Convert.ToInt32(nlTotalPrice * (1f - CGameColorFishMgr.Ins.pStaticConfig.GetInt("决斗抽成比例") * 0.0001f));
            }
        }
        else if (roomInfo.emCurShowType == ShowBoardType.SpecialBattle)
        {
            //增加宝藏积分
            //CPlayerNetHelper.AddTreasureCoin(uid, nlTotalPrice);
        }


        List<SpecialCastInfo> listPlayerInfos = new List<SpecialCastInfo>();
        if (emVictoryCamp == EMTeamBattleCamp.Red)
        {
            listPlayerInfos.AddRange(listTeamAInfos);
        }
        else if (emVictoryCamp == EMTeamBattleCamp.Blue)
        {
            listPlayerInfos.AddRange(listTeamBInfos);
        }
        for(int i = 0;i < listPlayerInfos.Count;i++)
        {
            string uid = listPlayerInfos[i].playerInfo.uid;
            CPlayerUnit playerUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
            if (playerUnit == null) return;
            
            if (roomInfo == null) return;
            if (listPlayerInfos[i].playerInfo.uid.Equals(CGameColorFishMgr.Ins.nlCurOuHuangUID))
            {
                if (playerUnit.pPreSlot != null &&
                   playerUnit.pPreSlot.pBindUnit == playerUnit)
                {
                    playerUnit.pPreSlot.BindPlayer(null);
                }
                playerUnit.JumpTarget(CGameColorFishMgr.Ins.pMap.pOuHuangSlot);
            }
            else
            {
                playerUnit.BackPreSlot();
            }
            if (roomInfo.emCurShowType == ShowBoardType.Battle)
            {
                playerUnit.AddCoinByHttp(nlTotalPrice, EMFishCoinAddFunc.Duel, false, false);
            }
            else if (roomInfo.emCurShowType == ShowBoardType.SpecialBattle)
            {
                //增加宝藏积分
                CPlayerNetHelper.AddTreasureCoin(uid, nlTotalPrice);
            }
        }

        //playerUnit.PlaySoldFishAudio();
        if (listTeamAInfos.Count > 0 &&
            listTeamBInfos.Count > 0)
        {
            roomInfo.ShowInfo(5f, true);
        }
        else
        {
            roomInfo.ShowInfo(5f, false);
        }

        nlCurPrice = 0;
        nlTotalPrice = 0;
        bMax = false;
        //Clear();
        //CGameColorFishMgr.Ins.pMap.pDuelBoat.SetState(CDuelBoat.EMState.Hide);
    }


}
