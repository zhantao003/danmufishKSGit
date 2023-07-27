using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSceneGame : CSceneBase
{
    public override void OnSceneStart()
    {
        CProfitRankMgr.Ins.Clear();
        COuHuangRankMgr.Ins.Clear();

        CTBLInfo.Inst.Init();
        CGameColorFishMgr.Ins.pMap = GameObject.FindObjectOfType<CGameMap>();
        CGameColorFishMgr.Ins.Init();

        CGameColorFishMgr.Ins.pGachaMgr = GameObject.FindObjectOfType<CGachaMgr>();
        CGameColorFishMgr.Ins.pGachaMgr.Init(CGameColorFishMgr.Ins.pMap.arrTranGachaRoots.Length);

        CGameColorFishMgr.Ins.pRandomEventMgr.Init();
        CGameColorFishMgr.Ins.nlCurOuHuangUID = "";

        UIManager.Instance.RefreshUI();
        UIManager.Instance.OpenUI(UIResType.GameInfo);
        UIManager.Instance.OpenUI(UIResType.SpecialCast);

        UICrazyTime.ResetPos();

        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal)
        {
            UIManager.Instance.OpenUI(UIResType.RoomInfo);
            //TODO:20大之前先关闭
            //UIManager.Instance.OpenUI(UIResType.ShowRoot);
            //UIManager.Instance.OpenUI(UIResType.TreasureInfo);
        }
        else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
        {
            UIManager.Instance.OpenUI(UIResType.RoomInfo);
            UIManager.Instance.OpenUI(UIResType.BattleModeInfo);
            UIBattleModeInfo battleModeInfo = UIManager.Instance.GetUI(UIResType.BattleModeInfo) as UIBattleModeInfo;
            if(battleModeInfo != null)
            {
                battleModeInfo.Init();
            }

            UIManager.Instance.OpenUI(UIResType.CheckIn);
            UIManager.Instance.OpenUI(UIResType.RankChg);
            UIManager.Instance.OpenUI(UIResType.CrazyGiftTip);
        }
        else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema)
        {
            UIManager.Instance.OpenUI(UIResType.RoomInfo);
            //UIManager.Instance.OpenUI(UIResType.ShowRoot);
            //UIManager.Instance.OpenUI(UIResType.TreasureInfo);

            //主播自动加入
            if (CPlayerMgr.Ins.pOwner != null)
            {
                CGameColorFishMgr.Ins.JoinPlayer(CPlayerMgr.Ins.pOwner, CGameColorFishMgr.EMJoinType.Normal);
            }
        }
        else if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive)
        {
            //UIManager.Instance.OpenUI(UIResType.TreasureInfo);
            UIManager.Instance.OpenUI(UIResType.RoomSurvive);
        }
        else if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Battle)
        {
            UIManager.Instance.OpenUI(UIResType.RoomVSInfo);

            ////奖池结束
            //CGameVSGiftPoolMgr.Ins.dlgPoolEnd += delegate ()
            //{
            //    CTimeTickMgr.Inst.PushTicker(4F, delegate (object[] values)
            //    {
            //        UIManager.Instance.OpenUI(UIResType.GameVSResult);
            //    });
            //};
        }
        else if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
        {
            CGameBossMgr.Ins.Init();

            UIManager.Instance.OpenUI(UIResType.RoomBossInfo);

            //主播自动加入
            if(CPlayerMgr.Ins.pOwner != null)
            {
                CGameColorFishMgr.Ins.JoinPlayer(CPlayerMgr.Ins.pOwner, CGameColorFishMgr.EMJoinType.Normal);
                CGameBossMgr.Ins.AddActivePlayer(CPlayerMgr.Ins.pOwner);
            }
        }
        
        UIManager.Instance.CloseUI(UIResType.Loading);

        if (CNPCMgr.Ins != null)
        {
            UIManager.Instance.OpenUI(UIResType.Auction);
            UIManager.Instance.OpenUI(UIResType.MatAuction);
            //if (CGameColorFishMgr.Ins.pGameRoomConfig.bActiveAuction)
            //{
            //    UIManager.Instance.OpenUI(UIResType.Auction);
            //}
            CNPCMgr.Ins.PlaceHelpUnit();
        }

        //UIRoomInfo uiRoomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
        //if (uiRoomInfo != null)
        //{
        //    uiRoomInfo.todaySpecialty.SetActive(!CTBLHandlerFucAble.Ins.GetInfo((int)EMFuncAbleType.TodaySpecial).CheckSceneLv());
        //    if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema)
        //    {
        //        uiRoomInfo.objInfoTip.SetActive(false);
        //    }
        //    else
        //    {
        //        uiRoomInfo.objInfoTip.SetActive(!CTBLHandlerFucAble.Ins.GetInfo((int)EMFuncAbleType.InfoTip).CheckSceneLv());
        //    }
        //    uiRoomInfo.ActiveTopBtn(!CTBLHandlerFucAble.Ins.GetInfo((int)EMFuncAbleType.TopBtn).CheckSceneLv());
        //}

        UIShowRoot uiShowRoot = UIManager.Instance.GetUI(UIResType.ShowRoot) as UIShowRoot;
        if (uiShowRoot != null)
        {
            if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema)
            {
                uiShowRoot.objShowRoot.SetActive(false);
            }
            else
            {
                uiShowRoot.objShowRoot.SetActive(true);
                //uiShowRoot.objShowRoot.SetActive(!CTBLHandlerFucAble.Ins.GetInfo((int)EMFuncAbleType.ShowInfo).CheckSceneLv());
            }
        }

        //if (CGameColorFishMgr.Ins.nCurRateUpLv == 1)
        //{
        //    UIManager.Instance.OpenUI(UIResType.Help);
        //}
    }

    public override void OnSceneUpdate()
    {
        //if(Input.GetKeyDown(KeyCode.F8))
        //{
        //    CHttpParam pReqParams = new CHttpParam
        //    (
        //        new CHttpParamSlot("roomId", CDanmuSDKCenter.Ins.szRoomId)
        //    );

        //    CHttpMgr.Instance.SendHttpMsg(CHttpConst.StartKongtou, pReqParams, 0, true);
        //}
    }

    public override void OnSceneLeave()
    {
        CFishRecordMgr.Ins.Clear();
        CRankTreasureMgr.Ins.Clear(false);

        UIManager.Instance.ClearUI();

        CPlayerMgr.Ins.ClearAllIdleUnit();
        CPlayerMgr.Ins.ClearAllActiveUnit();

        CAudioMgr.Ins.ClearAllAudio();
    }
}
