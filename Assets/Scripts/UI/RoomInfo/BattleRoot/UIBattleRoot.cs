using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleRoot : MonoBehaviour
{
    [Header("����׼��")]
    public UIBattleReady battleReady;
    [Header("��������")]
    public UIBattleShow battleShow;
    /// <summary>
    /// ���������Ϣ
    /// </summary>
    public List<SpecialCastInfo> listPlayerInfos = new List<SpecialCastInfo>();

    public GameObject objNormalImg;
    public GameObject objSpecialImg;

    public int nMaxCount;
    /// <summary>
    /// ��ǰ����ƱǮ
    /// </summary>
    public long nlCurPrice;
    /// <summary>
    /// �ܽ���
    /// </summary>
    public long nlTotalPrice;

    /// <summary>
    /// ������ʱ��
    /// </summary>
    CPropertyTimer pCheckEndTick;

    /// <summary>
    /// �Ƿ����ڽ���
    /// </summary>
    public bool bResultTime;
    /// <summary>
    /// �Ƿ��Ѿ�����
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
        ///ui��������
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
        ///��þ����õ���
        for (int i = 0; i < listPlayerInfos.Count; i++)
        {
            CPlayerUnit playerUnit = CPlayerMgr.Ins.GetIdleUnit(listPlayerInfos[i].playerInfo.uid);
            if (playerUnit == null) continue;
            listPlayerInfos[i].fishInfo = playerUnit.GetBattleFish();
        }
        SortInfo();
        Refresh();
        ///���㵹��ʱ
        pCheckEndTick = new CPropertyTimer();
        pCheckEndTick.Value = CGameColorFishMgr.Ins.pStaticConfig.GetInt("�������㵹��ʱ");
        pCheckEndTick.FillTime();
        bResultTime = true;
        ///���������� ��ҵ�״̬����
        for (int i = 0; i < listPlayerInfos.Count; i++)
        {
            CPlayerUnit playerUnit = CPlayerMgr.Ins.GetIdleUnit(listPlayerInfos[i].playerInfo.uid);
            if (playerUnit == null) continue;
            ///�����ھ����� �����������Լ�֮ǰ���ŵĸ���
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
    /// �����Ҫ���򴬵����
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
    /// �ж��Ƿ��и����
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
    /// �Ƴ���Ӧid�����
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
    /// ���������Ϣ
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
                battleReady.SetTotalReward(((long)System.Convert.ToInt32(nlTotalPrice * (1f - CGameColorFishMgr.Ins.pStaticConfig.GetInt("������ɱ���") * 0.0001f))));
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
        ///���ݵ�������ļ۸�����(�Ӹߵ���)
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
    /// ��Ϣ����
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
    /// ˢ��
    /// </summary>
    public void Refresh()
    {
        battleReady.RefreshPlayerInfo(listPlayerInfos);
        battleShow.RefreshPlayerInfo(listPlayerInfos);
    }

    /// <summary>
    /// ����һ��
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
        ///�ж��Ƿ�ɹ������˾������ɹ����ɣ�
        
        if (roomInfo.emCurShowType == ShowBoardType.Battle)
        {
            if (bSuc)
            {
                nlTotalPrice = (long)System.Convert.ToInt32(nlTotalPrice * (1f - CGameColorFishMgr.Ins.pStaticConfig.GetInt("������ɱ���") * 0.0001f));
            }
            BattleCastInfo castInfo = new BattleCastInfo();
            castInfo.playerInfo = listPlayerInfos[0].playerInfo;
            castInfo.nlTotalReward = nlTotalPrice;
            castInfo.bSuc = bSuc;
            ///�жϻ�ȡ����Ʒ�Ƿ�Ϊϡ������
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
            ///�жϻ�ȡ����Ʒ�Ƿ�Ϊϡ������
            UISpecialCast.ShowBattleInfo(castInfo, ShowBoardType.SpecialBattle);

            //���ӱ��ػ���
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
