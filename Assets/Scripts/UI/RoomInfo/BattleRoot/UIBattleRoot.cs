using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleRoot : MonoBehaviour
{
    [Header("决斗准备")]
    public UIBattleReady battleReady;
    [Header("决斗结算")]
    public UIBattleShow battleShow;
    /// <summary>
    /// 决斗玩家信息
    /// </summary>
    public List<SpecialCastInfo> listPlayerInfos = new List<SpecialCastInfo>();

    public GameObject objNormalImg;
    public GameObject objSpecialImg;

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
            objNormalImg.SetActive(roomInfo.emCurShowType == ShowBoardType.Battle);
            objSpecialImg.SetActive(roomInfo.emCurShowType == ShowBoardType.SpecialBattle);
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
        if (listPlayerInfos.Count <= 1)
        {
            battleShow.SetActive(false);
            UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
            if (roomInfo != null)
            {
                roomInfo.HideBoard(roomInfo.uiBattleTween, 0.5f,0f);
            }
        }
        else
        {
            battleShow.SetActive(true);
        }
        ///获得决斗用的鱼
        for (int i = 0; i < listPlayerInfos.Count; i++)
        {
            CPlayerUnit playerUnit = CPlayerMgr.Ins.GetIdleUnit(listPlayerInfos[i].playerInfo.uid);
            if (playerUnit == null) continue;
            listPlayerInfos[i].fishInfo = playerUnit.GetBattleFish();
        }
        SortInfo();
        Refresh();
        ///结算倒计时
        pCheckEndTick = new CPropertyTimer();
        pCheckEndTick.Value = CGameColorFishMgr.Ins.pStaticConfig.GetInt("决斗结算倒计时");
        pCheckEndTick.FillTime();
        bResultTime = true;
        ///决斗结束后 玩家的状态处理
        for (int i = 0; i < listPlayerInfos.Count; i++)
        {
            CPlayerUnit playerUnit = CPlayerMgr.Ins.GetIdleUnit(listPlayerInfos[i].playerInfo.uid);
            if (playerUnit == null) continue;
            ///除开冠军以外 其他人跳回自己之前待着的格子
            if (i > 0)
            {
                playerUnit.AddBattleValue();
                playerUnit.SetState(CPlayerUnit.EMState.EndFish);
                playerUnit.pShowFishEndEvent = delegate ()
                {
                    CPlayerUnit targetUnit = CPlayerMgr.Ins.GetIdleUnit(listPlayerInfos[0].playerInfo.uid);
                    playerUnit.PlayGoldEffectToTarget(targetUnit.tranSelf);
                    CTimeTickMgr.Inst.PushTicker(0.7f, delegate (object[] obj)
                     {
                         playerUnit.BackPreSlot();
                     });
                };
            }
            else
            {
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
                    UIShowFishInfo showFishInfo = gameInfo.GetShowFishInfoSlot(listPlayerInfos[0].playerInfo.uid);
                    showFishInfo.SetAddCoin(nMengPiaoGold, 0);
                }
                //playerUnit.PlaySoldFishAudio();
            }
        }
    }

    private void Update()
    {
        if(pCheckEndTick!= null)
        {
            if(pCheckEndTick.Tick(CTimeMgr.DeltaTime))
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
        listPlayerInfos.Clear();
        Refresh();
    }


    /// <summary>
    /// 检测需要调向船的玩家
    /// </summary>
    public void CheckNeedJumpBoat()
    {
        for (int i = 0; i < listPlayerInfos.Count; i++)
        {
            CPlayerUnit playerUnit = CPlayerMgr.Ins.GetIdleUnit(listPlayerInfos[i].playerInfo.uid);
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
    public bool bHavePlayer(string uid)
    {
        bool bHave = false;
        for (int i = 0; i < listPlayerInfos.Count; i++)
        {
            if (listPlayerInfos[i].playerInfo.uid.Equals(uid))
            {
                bHave = true;
                break;
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
        for (int i = 0; i < listPlayerInfos.Count; i++)
        {
            if (listPlayerInfos[i].playerInfo.uid.Equals(uid))
            {
                listPlayerInfos.Remove(listPlayerInfos[i]);
                break;
            }
        }
    }

    /// <summary>
    /// 增加玩家信息
    /// </summary>
    /// <param name="castInfo"></param>
    public bool AddPlayerInfo(SpecialCastInfo castInfo)
    {
        bool bAddSuc = false; ;
        bool bNeedRefresh = false;
        UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
        if (roomInfo == null)
            return false;
        if (listPlayerInfos.Count < nMaxCount &&
            !bHavePlayer(castInfo.playerInfo.uid))
        {
            listPlayerInfos.Add(castInfo);
            bNeedRefresh = true;
            nlTotalPrice += nlCurPrice;
            battleReady.SetPlayerCount(nMaxCount, listPlayerInfos.Count);
            if(listPlayerInfos.Count > 1 && roomInfo.emCurShowType == ShowBoardType.Battle)
            {
                battleReady.SetTotalReward(((long)System.Convert.ToInt32(nlTotalPrice * (1f - CGameColorFishMgr.Ins.pStaticConfig.GetInt("决斗抽成比例") * 0.0001f))));
            }
            else
            {
                battleReady.SetTotalReward(nlTotalPrice);
            }
            bAddSuc = true;
        }
        else
        {
            
        }
        ///根据钓到的鱼的价格排序(从高到低)
        //SortInfo();
        if (bNeedRefresh)
        {
            Refresh();
        }
        if(listPlayerInfos.Count >= nMaxCount)
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
        listPlayerInfos.Sort((x, y) =>
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
        battleReady.RefreshPlayerInfo(listPlayerInfos);
        battleShow.RefreshPlayerInfo(listPlayerInfos);
    }

    /// <summary>
    /// 检测第一名
    /// </summary>
    public void CheckFirstPlayer()
    {
        CGameColorFishMgr.Ins.pMap.DoCameraTween(false);
        if (listPlayerInfos.Count <= 0)
        {
            UIRoomInfo uiRoomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
            if (uiRoomInfo != null)
                uiRoomInfo.ShowInfo(5f, false);
            nlCurPrice = 0;
            nlTotalPrice = 0;
            bMax = false;
            return;
        }
        string uid = listPlayerInfos[0].playerInfo.uid;
        CPlayerUnit playerUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if (playerUnit == null) return;
        UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
        if (roomInfo == null) return;
        if (listPlayerInfos[0].playerInfo.uid.Equals(CGameColorFishMgr.Ins.nlCurOuHuangUID))
        {
            if(playerUnit.pPreSlot != null &&
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
        bool bSuc = listPlayerInfos.Count > 1;
        ///判断是否成功建立了决斗（成功则抽成）
        
        if (roomInfo.emCurShowType == ShowBoardType.Battle)
        {
            if (bSuc)
            {
                nlTotalPrice = (long)System.Convert.ToInt32(nlTotalPrice * (1f - CGameColorFishMgr.Ins.pStaticConfig.GetInt("决斗抽成比例") * 0.0001f));
            }
            BattleCastInfo castInfo = new BattleCastInfo();
            castInfo.playerInfo = listPlayerInfos[0].playerInfo;
            castInfo.nlTotalReward = nlTotalPrice;
            castInfo.bSuc = bSuc;
            ///判断获取的物品是否为稀有类型
            UISpecialCast.ShowBattleInfo(castInfo, ShowBoardType.Battle);
            playerUnit.AddCoinByHttp(nlTotalPrice, EMFishCoinAddFunc.Duel, false, false);
            //CFishRecordMgr.Ins.AddRecord(nlTotalPrice, listShowInfos[0].playerInfo);
        }
        else if(roomInfo.emCurShowType == ShowBoardType.SpecialBattle)
        {
            BattleCastInfo castInfo = new BattleCastInfo();
            castInfo.playerInfo = listPlayerInfos[0].playerInfo;
            castInfo.nlTotalReward = nlTotalPrice;
            castInfo.bSuc = bSuc;
            ///判断获取的物品是否为稀有类型
            UISpecialCast.ShowBattleInfo(castInfo, ShowBoardType.SpecialBattle);

            //增加宝藏积分
            //CHttpParam pReqParams = new CHttpParam
            //(
            //    new CHttpParamSlot("uid", uid.ToString()),
            //    new CHttpParamSlot("treasurePoint", nlTotalPrice.ToString())
            //);
            //CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddUserTreasurePoint, pReqParams, CHttpMgr.Instance.nReconnectTimes, true);
            CPlayerNetHelper.AddTreasureCoin(uid, nlTotalPrice);
        }
        //playerUnit.PlaySoldFishAudio();
        if (listPlayerInfos.Count > 1)
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
