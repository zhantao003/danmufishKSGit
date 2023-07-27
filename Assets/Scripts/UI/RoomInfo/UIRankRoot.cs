using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UIRankRoot : MonoBehaviour
{
    public GameObject objSelf;

    public UIRankPlayerInfo[] playerInfos;

    public UIRankPlayerInfo uiCamption;

    public int nMaxCount;

    public GameObject objEmpty;

    bool bFirst = true;

    public void InitInfo()
    {
        Refresh();
    }

    public void AddPlayerInfo(SpecialCastInfo castInfo)
    {
        if (castInfo.fishInfo.emItemType == EMItemType.Other ||
            castInfo.fishInfo.emItemType == EMItemType.FishMat ||
            castInfo.fishInfo.emItemType == EMItemType.RandomEvent)
            return;

        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle &&
            CBattleModeMgr.Ins != null &&
            CBattleModeMgr.Ins.emCurState != CBattleModeMgr.EMGameState.Gaming)
        {
            return;
        }


        ///判断加入信息后是否需要刷新
        bool bNeedRefresh = COuHuangRankMgr.Ins.AddInfo(castInfo, nMaxCount);
        
        if (bNeedRefresh)
        {
            COuHuangRankMgr.Ins.Sort();
            Refresh();
        }
    }


    public void Refresh()
    {
        List<OuHuangRankInfo> listShowInfos = COuHuangRankMgr.Ins.GetRankInfos();

        if(listShowInfos.Count > 0)
        {
            uiCamption.Init(listShowInfos[0]);
        }
        else
        {
            uiCamption.Init(null);
        }

        for (int i = 0;i < listShowInfos.Count;i++)
        {
            if (i >= playerInfos.Length) break;
            playerInfos[i].SetActive(true);
            playerInfos[i].Init(listShowInfos[i]);

            if(i == 0 && 
              //(listShowInfos[i].emFishRare >= EMRare.XiYou || listShowInfos[i].bBianYi) &&
               CGameColorFishMgr.Ins.nlCurOuHuangUID != listShowInfos[i].nlUserUID)
            {
                //判断当前是否有欧皇
                CPlayerUnit pOuHuangUnit = null;
                if (!string.IsNullOrEmpty(CGameColorFishMgr.Ins.nlCurOuHuangUID))
                {
                    //CPlayerUnit pOuHuangUnit = CGameColorFishMgr.Ins.pMap.pOuHuangSlot.pBindUnit;
                    pOuHuangUnit = CPlayerMgr.Ins.GetIdleUnit(CGameColorFishMgr.Ins.nlCurOuHuangUID);
                }
                if (pOuHuangUnit != null &&
                    listShowInfos[i].nlUserUID == pOuHuangUnit.uid)
                    continue;

                CPlayerUnit pChgUnit = CPlayerMgr.Ins.GetIdleUnit(listShowInfos[i].nlUserUID);
                if (pOuHuangUnit != null
                    && pChgUnit != null)
                {
                    if (pOuHuangUnit.emCurState == CPlayerUnit.EMState.Battle)
                    {
                        if (pChgUnit.emCurState == CPlayerUnit.EMState.Battle)
                        {
                            pOuHuangUnit.pPreSlot = pChgUnit.pPreSlot;
                            if(pOuHuangUnit.pPreSlot!=null)
                            {
                                pOuHuangUnit.pPreSlot.BindPlayer(pOuHuangUnit);
                            }
                            else
                            {
                                Debug.LogError("None PreSlot Error");
                            }
                        }
                        else
                        {
                            pOuHuangUnit.pPreSlot = pChgUnit.pMapSlot;
                            if (pOuHuangUnit.pPreSlot != null)
                            {
                                pOuHuangUnit.pPreSlot.BindPlayer(pOuHuangUnit);
                            }
                            else
                            {
                                Debug.LogError("None PreSlot Error");
                            }
                        }
                    }
                    else
                    {
                        if (pChgUnit.emCurState == CPlayerUnit.EMState.Battle)
                        {
                            pOuHuangUnit.JumpTarget(pChgUnit.pPreSlot);
                        }
                        else
                        {
                            if(pChgUnit.pMapSlot == null ||
                               pChgUnit.pMapSlot.emType == CMapSlot.EMType.Duel)
                            {
                                pOuHuangUnit.JumpTarget(pChgUnit.pPreSlot);
                            }
                            else
                            {
                                pChgUnit.pMapSlot.SetBoat(101, null, 0);
                                pOuHuangUnit.JumpTarget(pChgUnit.pMapSlot);
                                pChgUnit.pMapSlot = null;
                            }
                        }
                    }
                }

                if (pChgUnit != null)
                {
                    //判断所待的格子是否为决斗格子,且占领的格子的对象是否为自己
                    if(pChgUnit.pMapSlot != null &&
                       pChgUnit.pMapSlot.pBindUnit != null &&
                       pChgUnit.pMapSlot.pBindUnit == pChgUnit &&
                       pChgUnit.pMapSlot.emType != CMapSlot.EMType.Duel)
                    {
                        pChgUnit.pMapSlot.BindPlayer(null);
                    }

                    if (pChgUnit.emCurState != CPlayerUnit.EMState.Battle)
                    {
                        //重置钓台
                        if (pChgUnit.pMapSlot != null &&
                            pChgUnit.pMapSlot.emType != CMapSlot.EMType.Duel)
                        {
                            pChgUnit.pMapSlot.SetBoat(101, null, 0);
                            pChgUnit.pMapSlot = null;
                        }

                        pChgUnit.JumpTarget(CGameColorFishMgr.Ins.pMap.pOuHuangSlot);
                    }
                    else
                    {
                        if(pOuHuangUnit == null &&
                           pChgUnit.pPreSlot != null &&
                           pChgUnit.pPreSlot.pBindUnit == pChgUnit)
                        {
                            pChgUnit.pPreSlot.BindPlayer(null);
                        }
                        pChgUnit.pMapSlot = null;

                        pChgUnit.pPreSlot = CGameColorFishMgr.Ins.pMap.pOuHuangSlot;
                        pChgUnit.pPreSlot.BindPlayer(pChgUnit);
                    }
                }
                CGameColorFishMgr.Ins.nlCurOuHuangUID = listShowInfos[i].nlUserUID;
                if(bFirst)
                {
                    bFirst = false;
                }
                else
                {
                    CPlayerBaseInfo baseInfo = CPlayerMgr.Ins.GetPlayer(CGameColorFishMgr.Ins.nlCurOuHuangUID);
                    CRankChgInfo rankChgInfo = new CRankChgInfo();
                    rankChgInfo.pPlayerInfo = baseInfo;
                    rankChgInfo.emRankChgType = EMRankChgType.OuHuang;
                    rankChgInfo.nRank = 1;
                    UIRankChg.ShowInfo(rankChgInfo);
                }
            }
        }

        for (int i = listShowInfos.Count; i < playerInfos.Length; i++)
        {
            playerInfos[i].SetActive(false);
        }
        if (objEmpty != null)
        {
            objEmpty.SetActive(listShowInfos.Count <= 0);
        }
    }

    public void OnClickReset()
    {
        UIMsgBox.Show("重置欧皇榜", "是否重置欧皇排行榜，请谨慎操作", UIMsgBox.EMType.YesNo, delegate ()
          {
              Clear();
          });
    }

    public void ShowAllPlayerName(bool show)
    {
        for (int i = 0; i < playerInfos.Length; i++)
        {
            playerInfos[i].ShowNameLabel(show);
        }
    }

    public void Clear()
    {
        bFirst = true;
        if (CBomberCountMgr.Ins!=null)
        {
            CBomberCountMgr.Ins.RandInfo();
        }
        COuHuangRankMgr.Ins.Clear();
        Refresh();
    }
}
