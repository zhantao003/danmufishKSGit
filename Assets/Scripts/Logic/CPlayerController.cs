using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CPlayerController : MonoBehaviour
{
    public Camera mainCam;
    public Camera uiCam;
    public LayerMask lCheckMsk;
    public LayerMask lColMsk;   //阻挡面板

    public float fDragCheckTime;

    CPropertyTimer pDragTick;

    Ray pCheckRay;
    RaycastHit pHitInfo;


    CPlayerUnit pClickUnit;
    bool bDrag;
    

    void Update()
    {
        //if (CPlayerMgr.Ins.pOwner == null) return;

        //if(pDragTick != null &&
        //   pDragTick.Tick(fDragCheckTime))
        //{
        //    pDragTick = null;
        //    bDrag = true;
        //    pClickUnit.SetState(CPlayerUnit.EMState.Drag);
        //}

        //if (bDrag)
        //{

        //}
        //else
        //{
        //    if (Input.GetKeyDown(KeyCode.Mouse0))
        //    {
        //        //检查是否有阻挡
        //        if (EventSystem.current.IsPointerOverGameObject())
        //        {
        //            Debug.Log("被阻挡了");
        //            return;
        //        }
        //        pCheckRay = new Ray();
        //        pCheckRay = mainCam.ScreenPointToRay(Input.mousePosition);
        //        if (Physics.Raycast(pCheckRay, out pHitInfo, 999F, lCheckMsk))
        //        {
        //            CPlayerUnit playerUnit = pHitInfo.collider.gameObject.GetComponent<CPlayerUnit>();
        //            if (pClickUnit == null)
        //            {
        //                pClickUnit = playerUnit;
        //            }
        //            if (playerUnit != null && playerUnit.pInfo.emUserType == CPlayerBaseInfo.EMUserType.Zhubo)
        //            {

        //            }
        //            else
        //            {
        //                if (playerUnit != null)
        //                {
        //                    if(pDragTick == null)
        //                    {
        //                        pDragTick = new CPropertyTimer();
        //                        pDragTick.Value = fDragCheckTime;
        //                        pDragTick.FillTime();
        //                    }
        //                    UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        //                    if (gameInfo != null)
        //                    {
        //                        gameInfo.ShowUnitExitInfo(playerUnit);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //if(Input.GetKeyUp(KeyCode.Mouse0))
        //{
        //    pDragTick = null;
        //    if (pClickUnit != null)
        //    {
        //        if (bDrag)
        //        {
        //            pClickUnit.SetState(CPlayerUnit.EMState.Idle);
        //            bDrag = false;
        //        }
        //        else
        //        {
        //            if (pClickUnit.pInfo.emUserType == CPlayerBaseInfo.EMUserType.Zhubo)
        //            {
        //                Dm dm = new Dm();
        //                dm.uid = pClickUnit.uid;
        //                dm.userName = pClickUnit.pInfo.userName;
        //                dm.userFace = pClickUnit.pInfo.userFace;
        //                dm.msg = CDanmuEventConst.LaGan;
        //                dm.roomId = CDanmuSDKCenter.Ins.szRoomId;
        //                CDanmuMgr.Ins.WebSocketBLiveClient.Mock(dm);
        //            }
        //            else
        //            {
        //                UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        //                if (gameInfo != null)
        //                {
        //                    gameInfo.ShowUnitExitInfo(pClickUnit);
        //                }
        //            }
        //        }
        //    }
        //    pClickUnit = null;
        //}

        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //检查是否有阻挡
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //Debug.Log("被阻挡了");
                return;
            }

            pCheckRay = new Ray();
            pCheckRay = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(pCheckRay, out pHitInfo, 999F, lCheckMsk))
            {
                CPlayerUnit playerUnit = pHitInfo.collider.gameObject.GetComponent<CPlayerUnit>();
                if(playerUnit != null &&
                   playerUnit.bCanLaGan)
                {
                    bool bAbleLagan = false;
                    //舰长或者主播自己才可以点
                    bAbleLagan = (playerUnit.pInfo != null && playerUnit.pInfo.guardLevel > 0);
                    bAbleLagan = bAbleLagan || (playerUnit.pInfo != null && playerUnit.pInfo.nVipPlayer > 0);
                    //bAbleLagan = bAbleLagan || (playerUnit.uid == CPlayerMgr.Ins.pOwner.uid);

                    if (bAbleLagan)
                    {
                        PlayerLaGan(playerUnit);
                        //Dm dm = new Dm();
                        //dm.uid = playerUnit.uid;
                        //dm.userName = playerUnit.pInfo.userName;
                        //dm.userFace = playerUnit.pInfo.userFace;
                        //dm.msg = CDanmuEventConst.LaGan;
                        //dm.roomId = CDanmuSDKCenter.Ins.szRoomId;
                        //CDanmuMgr.Ins.WebSocketBLiveClient.Mock(dm);

                        CEffectMgr.Instance.CreateEffSync("Effect/Common/ctrlClick",
                            playerUnit.tranSelf.position,
                            playerUnit.tranSelf.rotation, 0);
                    }                }
            }
        }
    
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            //检查是否有阻挡
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("被阻挡了");
                return;
            }
            pCheckRay = new Ray();
            pCheckRay = mainCam.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(pCheckRay, out pHitInfo, 999F, lCheckMsk))
            {
                
                CPlayerUnit playerUnit = pHitInfo.collider.gameObject.GetComponent<CPlayerUnit>();
                if (playerUnit != null &&
                    playerUnit.pInfo != null &&
                    playerUnit.pInfo.emUserType == CPlayerBaseInfo.EMUserType.Zhubo)
                {

                }
                else
                {
                    if (playerUnit == null ||
                        playerUnit.pInfo == null)
                    {

                    }
                    else
                    {
                        //展示强行抱走的按钮
                        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
                        {
                            if (CGameBossMgr.Ins.emCurState == CGameBossMgr.EMState.Ready)
                            {
                                UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
                                if (gameInfo != null)
                                {
                                    gameInfo.ShowUnitExitInfo(playerUnit);
                                }
                            }
                        }
                        //else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
                        //{
                        //    if(CBattleModeMgr.Ins != null &&
                        //       CBattleModeMgr.Ins.emCurState == CBattleModeMgr.EMGameState.Ready)
                        //    {
                        //        UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
                        //        if (gameInfo != null)
                        //        {
                        //            gameInfo.ShowUnitExitInfo(playerUnit);
                        //        }
                        //    }
                        //}
                        else
                        {
                            UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
                            if (gameInfo != null)
                            {
                                gameInfo.ShowUnitExitInfo(playerUnit);
                            }
                        }
                    }
                }
            }
        }

        //PlayerControllerByBoss204();

    }

    /// <summary>
    /// Boss204场景下的主播操作
    /// </summary>
    void PlayerControllerByBoss204()
    {
        //如果是Boss战开始后不让上岸
        if (CGameColorFishMgr.Ins.emCurGameType != CGameColorFishMgr.EMGameType.Boss)
        {
            return;
        }
        if (CGameBossMgr.Ins != null &&
            CGameBossMgr.Ins.emCurState != CGameBossMgr.EMState.Gaming)
        {
            return;
        }

        if (CPlayerMgr.Ins == null ||
            CPlayerMgr.Ins.pOwner == null)
        {
            return;
        }

        string uid = CPlayerMgr.Ins.pOwner.uid;

        CPlayerBaseInfo pPlayerInfo = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayerInfo == null)
            return;

        if (CControlerSlotMgr.Ins == null)
            return;

        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if (pUnit == null)
        {
            pUnit = CPlayerMgr.Ins.GetActiveUnit(uid);
            if (pUnit == null)
                return;
        }

        if (pUnit.emCurState == CPlayerUnit.EMState.BossEat ||
            pUnit.emCurState == CPlayerUnit.EMState.BossWait ||
            pUnit.emCurState == CPlayerUnit.EMState.BossReturn)
            return;

        CControlerSlotByBoss slot = CControlerSlotMgr.Ins.GetSlot(uid);
        if (slot == null)
            return;

        

        if (Input.GetKeyDown(KeyCode.D) ||
           Input.GetKeyDown(KeyCode.RightArrow))
        {
            slot.Move(CControlerSlotByBoss.SlotDirection.Right);
        }

        if (Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.LeftArrow))
        {
            slot.Move(CControlerSlotByBoss.SlotDirection.Left);
        }

        if (Input.GetKeyDown(KeyCode.W) ||
          Input.GetKeyDown(KeyCode.UpArrow))
        {
            slot.Move(CControlerSlotByBoss.SlotDirection.Up);
        }

        if (Input.GetKeyDown(KeyCode.S) ||
          Input.GetKeyDown(KeyCode.DownArrow))
        {
            slot.Move(CControlerSlotByBoss.SlotDirection.Down);
        }

    }


    void PlayerLaGan(CPlayerUnit pUnit)
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
    }

}