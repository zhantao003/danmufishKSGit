using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 屏幕分辨率信息
/// </summary>
public class ScreenResolutionInfo
{
    public int nReslutionX;     //分辨率X
    public int nReslutionY;     //分辨率Y

    public ScreenResolutionInfo(int nX, int nY)
    {
        nReslutionX = nX;
        nReslutionY = nY;
    }
} 

public class UISetting : UIBase
{
    public enum EMBoardType{
        Common,
        Graphycs,
    }

    public Button[] arrBtnBoard;

    public Text uiVersion;

    public UISettingBoardCommon pBoardCommon;
    public UISettingBoardGraphycs pBoardGraphycs;

    long nLastTongbuTime = 0;

    public override void OnOpen()
    {
        base.OnOpen();
        uiVersion.text = "V" + Application.version;
        SetBoard(EMBoardType.Common);
    }

    public void SetBoard(EMBoardType board)
    {
        for(int i=0; i<arrBtnBoard.Length; i++)
        {
            arrBtnBoard[i].interactable = (i != (int)board);
        }

        pBoardCommon.gameObject.SetActive(board == EMBoardType.Common);
        pBoardGraphycs.gameObject.SetActive(board == EMBoardType.Graphycs);

        if(board == EMBoardType.Common)
        {
            pBoardCommon.OnOpen();
        }
        else if(board == EMBoardType.Graphycs)
        {
            pBoardGraphycs.OnOpen();
        }
    }

    public void OnClickBoardType(int board)
    {
        SetBoard((EMBoardType)board);
    }

    /// <summary>
    /// 退出
    /// </summary>
    public void OnClickClose()
    {
        if (pBoardCommon.bSetInfo ||
            pBoardGraphycs.bSetInfo)
        {
            pBoardCommon.bSetInfo = false;
            pBoardGraphycs.bSetInfo = false;

            CSystemInfoMgr.Inst.SaveFile();
        }

        CloseSelf();
    }

    /// <summary>
    /// 保存并退出
    /// </summary>
    public void OnClickSave()
    {
        if (pBoardCommon.bSetInfo ||
            pBoardGraphycs.bSetInfo)
        {
            pBoardCommon.bSetInfo = false;
            pBoardGraphycs.bSetInfo = false;

            CSystemInfoMgr.Inst.SaveFile();
        }

        CloseSelf();
    }

    public void OnClickGoDongTai()
    {
        Application.OpenURL("https://space.bilibili.com/38367534/dynamic");
    }

    public void OnClickTongbu()
    {
        if(CTimeMgr.NowMillonsSec() - nLastTongbuTime <= 5000)
        {
            UIToast.Show("冷却中，稍后再同步");
            return;
        }

        if (CPlayerMgr.Ins.pOwner == null) return;

        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("uid", CPlayerMgr.Ins.pOwner.uid.ToString()),
            new CHttpParamSlot("roomId", CDanmuSDKCenter.Ins.szRoomId.ToString()),
            new CHttpParamSlot("isVtb", "1")
        );

        CHttpParam pReqParamsOnlyUid = new CHttpParam(
            new CHttpParamSlot("uid", CPlayerMgr.Ins.pOwner.uid.ToString())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetAvatarList, pReqParams);
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetBoatList, pReqParams);
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetFishGanList, pReqParamsOnlyUid);

        nLastTongbuTime = CTimeMgr.NowMillonsSec();
    }

    public void OnClickCopyUID()
    {
        GUIUtility.systemCopyBuffer = SystemInfo.deviceUniqueIdentifier;

        UIToast.Show("已复制");
    }

    public void OnClickExit()
    {
        CloseSelf();

        if(CSceneMgr.Instance.m_objCurScene.emSceneType == CSceneFactory.EMSceneType.MainMenu)
        {
            UIRoomSetting roomSetting = UIManager.Instance.GetUI(UIResType.RoomSetting) as UIRoomSetting;
            if(roomSetting.IsOpen())
            {
                UIMsgBox.Show("", "是否回到登录界面", UIMsgBox.EMType.YesNo, delegate ()
                {
                    UIManager.Instance.CloseUI(UIResType.RoomSetting);
                    
                    UIManager.Instance.OpenUI(UIResType.MainMenu);
                    CDanmuSDKCenter.Ins.EndGame(true);
                    UIMainMenu uiMainMenu = UIManager.Instance.GetUI(UIResType.MainMenu) as UIMainMenu;
                    if (uiMainMenu != null)
                    {
                        uiMainMenu.SetBoard(UIMainMenu.EMBoardType.Login);
                    }
                });
                //UIMsgBox.Show("", "是否回到模式选择界面", UIMsgBox.EMType.YesNo, delegate ()
                //{
                //    UIManager.Instance.CloseUI(UIResType.RoomSetting);

                //    UIManager.Instance.OpenUI(UIResType.MainMenu);
                //    UIMainMenu uiMainMenu = UIManager.Instance.GetUI(UIResType.MainMenu) as UIMainMenu;
                //    if (uiMainMenu != null)
                //    {
                //        uiMainMenu.SetBoard(UIMainMenu.EMBoardType.GameType);
                //    }
                //});
            }
            else
            {
                UIMainMenu uiMainMenu = UIManager.Instance.GetUI(UIResType.MainMenu) as UIMainMenu;
                if(uiMainMenu.IsOpen())
                {
                    if(uiMainMenu.emCurBoardType == UIMainMenu.EMBoardType.Login)
                    {
                        UIMsgBox.Show("", "是否退出到桌面", UIMsgBox.EMType.YesNo, delegate ()
                        {
                            Application.Quit();
                        });
                    }
                    else if(uiMainMenu.emCurBoardType == UIMainMenu.EMBoardType.GameType)
                    {
                        UIMsgBox.Show("", "是否回到登录界面", UIMsgBox.EMType.YesNo, delegate ()
                        {
                            CDanmuSDKCenter.Ins.EndGame(true);
                            uiMainMenu.SetBoard(UIMainMenu.EMBoardType.Login);
                        });
                    }
                }
                else
                {
                    UIMsgBox.Show("", "是否退出到桌面", UIMsgBox.EMType.YesNo, delegate ()
                    {
                        Application.Quit();
                    });
                }
            } 
        }
        else
        {
            UIMsgBox.Show("退出游戏", "要放弃本场游戏回到菜单吗？", UIMsgBox.EMType.YesNo, delegate ()
            {
                if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal ||
                   CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Battle ||
                   CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
                {
                    CPlayerMgr.Ins.ClearAllActiveUnit();
                    CPlayerMgr.Ins.ClearAllIdleUnit();
                    CPlayerMgr.Ins.ClearActivePlayer();

                    CloseSelf();

                    CSceneMainMenu.emOpenUI = UIResType.RoomInfo;
                    UIManager.Instance.OpenUI(UIResType.Loading);
                    CSceneMgr.Instance.LoadScene(CSceneFactory.EMSceneType.MainMenu);
                }
                else if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema ||
                        CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive)
                {
                    CPlayerMgr.Ins.ClearAllActiveUnit();
                    CPlayerMgr.Ins.ClearAllIdleUnit();
                    CPlayerMgr.Ins.ClearActivePlayer();

                    CloseSelf();

                    CSceneMainMenu.emOpenUI = UIResType.RoomInfo;
                    UIManager.Instance.OpenUI(UIResType.Loading);
                    CSceneMgr.Instance.LoadScene(CSceneFactory.EMSceneType.MainMenu);
                }
                else if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
                {
                    if(CGameBossMgr.Ins == null)
                    {
                        CPlayerMgr.Ins.ClearAllActiveUnit();
                        CPlayerMgr.Ins.ClearAllIdleUnit();
                        CPlayerMgr.Ins.ClearActivePlayer();
                        //CPlayerMgr.Ins.ClearAllPlayerInfos();

                        CloseSelf();

                        CSceneMainMenu.emOpenUI = UIResType.RoomInfo;
                        UIManager.Instance.OpenUI(UIResType.Loading);
                        CSceneMgr.Instance.LoadScene(CSceneFactory.EMSceneType.MainMenu);
                    }
                    else
                    {
                        if(CGameBossMgr.Ins.emCurState == CGameBossMgr.EMState.Gaming)
                        {
                            CGameBossMgr.Ins.emCurState = CGameBossMgr.EMState.End;
                            CGameBossMgr.Ins.pBoss.SetState(CBossBase.EMState.Escape);
                            CloseSelf();
                        }
                        else
                        {
                            CPlayerMgr.Ins.ClearAllActiveUnit();
                            CPlayerMgr.Ins.ClearAllIdleUnit();
                            CPlayerMgr.Ins.ClearActivePlayer();
                            //CPlayerMgr.Ins.ClearAllPlayerInfos();

                            //退出时清空隐藏boss概率
                            CGameColorFishMgr.Ins.nSpecialBossRate = 0;

                            CloseSelf();

                            CSceneMainMenu.emOpenUI = UIResType.RoomInfo;
                            UIManager.Instance.OpenUI(UIResType.Loading);
                            CSceneMgr.Instance.LoadScene(CSceneFactory.EMSceneType.MainMenu);
                        }
                    }
                }
            });
        }
    }
}
