using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIMainMenu : UIBase
{
    public GameObject objBoardLogin;
    public GameObject objBoardGameType; //游戏模式选择
    public GameObject objBoardHelp;

    public GameObject objLock;
    public Text uiLockText;

    public GameObject objLockCinema;

    public InputField uiInputCode;
    public InputField uiInputRoomId;
    public GameObject objInputRoomId;
    public Text uiLabelVersion;

    public Text uiLabelVersionTip;

    //public VideoPlayer videoPlayer;
    //public AudioSource videoAudio;
    //public float fOriginVideoSound;

    public long nSpecUID;
    public GameObject objBtnCinema;
    public GameObject objBtnVS;

    public enum EMBoardType
    { 
        Login,
        GameType,
    }

    public EMBoardType emCurBoardType = EMBoardType.Login;

    List<DelegateNFuncCall> listCallSuc = new List<DelegateNFuncCall>();

    protected override void OnStart()
    {
        
    }

    protected override void OnUpdate(float dt)
    {
        if (listCallSuc.Count > 0)
        {
            listCallSuc[0].Invoke();
            listCallSuc.RemoveAt(0);
        }
    }

    /// <summary>
    /// 请求活动信息
    /// </summary>
    void ReqFesInfo()
    {
        //TODO:活动相关接口屏蔽
        //TODO:请求活动开关
        CHttpParam pReqFesParams = new CHttpParam(
            new CHttpParamSlot("packId", "1")
        );
        CHttpParam pReqFesParams2 = new CHttpParam(
            new CHttpParamSlot("packId", "2")
        );
        //CHttpParam pReqFesParams3 = new CHttpParam(
        //   new CHttpParamSlot("packId", "3")
        //);
        //CHttpParam pReqFesParams4 = new CHttpParam(
        //   new CHttpParamSlot("packId", "4")
        //);
        //CHttpParam pReqFesParams5 = new CHttpParam(
        //   new CHttpParamSlot("packId", "5")
        //);

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetFesSwitch, new HHandlerGetFesSwitch(), pReqFesParams);
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetFesSwitch, new HHandlerGetFesSwitch(), pReqFesParams2);
        //CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetFesSwitch, new HHandlerGetFesSwitch(), pReqFesParams3);
        //CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetFesSwitch, new HHandlerGetFesSwitch(), pReqFesParams4);
        //CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetFesSwitch, new HHandlerGetFesSwitch(), pReqFesParams5);

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugLoadFesInfo, new HHandlerGetFesInfoPack((int)CFishFesInfoMgr.EMFesType.RankOuhuang), pReqFesParams);
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugLoadFesInfo, new HHandlerGetFesInfoPack((int)CFishFesInfoMgr.EMFesType.RankRicher), pReqFesParams2);
    }


    public void SetBoard(EMBoardType boardType)
    {
        emCurBoardType = boardType;
        objBoardLogin.SetActive(boardType == EMBoardType.Login);
        objBoardGameType.SetActive(boardType == EMBoardType.GameType);
        if(boardType == EMBoardType.GameType)
        {
            bool bSpecNiuRou = true;//(CPlayerMgr.Ins.pOwner.uid == nSpecUID);

            objBtnCinema.SetActive(bSpecNiuRou);
            objBtnVS.SetActive(!bSpecNiuRou);

            objLockCinema.SetActive(CGameColorFishMgr.Ins.nCurRateUpLv < 10);

            //objLock.SetActive(CGameColorFishMgr.Ins.nCurRateUpLv < CGameColorFishMgr.Ins.pStaticConfig.GetInt("炸鱼场解锁等级"));
            //uiLockText.text = CGameColorFishMgr.Ins.pStaticConfig.GetInt("炸鱼场解锁等级") + "级渔场解锁";
        }
        else if(boardType == EMBoardType.Login)
        {
            ReqFesInfo();
        }
    }

    public override void OnOpen()
    {
        SetBoard(EMBoardType.Login);

        //房间号输入栏状态刷新
        //objInputRoomId.SetActive(CDanmuSDKCenter.Ins.emPlatform == CDanmuSDKCenter.EMPlatform.DouyinOpen);

        if (CDanmuSDKCenter.Ins.emPlatform == CDanmuSDKCenter.EMPlatform.Bilibili)
        {
            string szTmpCode = "";
            if (!CHelpTools.IsStringEmptyOrNone(CGlobalInit.Ins.szArgCode))
            {
                szTmpCode = CGlobalInit.Ins.szArgCode;
            }

            string szSaveCode = CSystemInfoMgr.Inst.GetString(CSystemInfoConst.VUTBERCODE);
            if (!CHelpTools.IsStringEmptyOrNone(szSaveCode))
            {
                szTmpCode = szSaveCode;
            }

            uiInputCode.text = szTmpCode;
        }
        else if (CDanmuSDKCenter.Ins.emPlatform == CDanmuSDKCenter.EMPlatform.DouyinOpen)
        {
            //string szPreCode = CSystemInfoMgr.Inst.GetString(CSystemInfoConst.VUTBERCODE);
            //if (!CHelpTools.IsStringEmptyOrNone(szPreCode))
            //{
            //    uiInputCode.text = szPreCode;
            //}

            uiInputCode.text = SystemInfo.deviceUniqueIdentifier;
        }
        else if (CDanmuSDKCenter.Ins.emPlatform == CDanmuSDKCenter.EMPlatform.DouyinYS)
        {
            string szPreCode = CSystemInfoMgr.Inst.GetString(CSystemInfoConst.VUTBERCODE);
            if (!CHelpTools.IsStringEmptyOrNone(szPreCode))
            {
                uiInputCode.text = szPreCode;
            }
        }
        else if(CDanmuSDKCenter.Ins.emPlatform == CDanmuSDKCenter.EMPlatform.KuaiShou)
        {
            string szTmpCode = "";
            if (!CHelpTools.IsStringEmptyOrNone(CGlobalInit.Ins.szArgCode))
            {
                szTmpCode = CGlobalInit.Ins.szArgCode;
            }

            uiInputCode.text = szTmpCode;
            uiInputRoomId.text = szTmpCode;
        }

        //请求版本号
        uiLabelVersion.text = "Version：" + Application.version;
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetVersion);

        CAudioMgr.Ins.StopMusicTrac();
    }

    public override void OnClose()
    {
        //if(CAudioMgr.Ins!=null)
        //{
        //    CAudioMgr.Ins.callChgVolumMain -= this.SetVideoSound;
        //    CAudioMgr.Ins.callChgVolumMusic -= this.SetVideoSound;
        //}
    }

    //void SetVideoSound(float value)
    //{
    //    videoAudio.volume = fOriginVideoSound *
    //                 (CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.BGM) * 0.01F) *
    //                 (CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.ALLSOUND) * 0.01F);
    //}

    public void OnClickLogin()
    {
        UIManager.Instance.OpenUI(UIResType.NetWait);

        string szCode = uiInputCode.text;
        string szRoomId = uiInputRoomId.text;

        //if (CHelpTools.IsStringEmptyOrNone(szCode))
        //{
        //    if (!CDanmuSDKCenter.Ins.bDevType)
        //    {
        //        UIManager.Instance.CloseUI(UIResType.NetWait);
        //        UIToast.Show("请输入正确的身份码");
        //        return;
        //    }
        //}

        //只有抖音需要填写房间号
        if (CDanmuSDKCenter.Ins.emPlatform == CDanmuSDKCenter.EMPlatform.DouyinOpen)
        {
            if (CHelpTools.IsStringEmptyOrNone(szRoomId))
            {
                UIManager.Instance.CloseUI(UIResType.NetWait);
                UIToast.Show("请输入正确的房间号");
                return;
            }

            szCode = SystemInfo.deviceUniqueIdentifier;
        }
        else if (CDanmuSDKCenter.Ins.emPlatform == CDanmuSDKCenter.EMPlatform.KuaiShou)
        {
            szCode = uiInputRoomId.text;
        }

        if (CHelpTools.IsStringEmptyOrNone(szCode))
        {
            if (!CDanmuSDKCenter.Ins.bDevType)
            {
                UIManager.Instance.CloseUI(UIResType.NetWait);
                UIToast.Show("请输入正确的登录码");
                return;
            }
        }

        if (CDanmuSDKCenter.Ins.IsGaming())
        {
            CDanmuSDKCenter.Ins.EndGame(true, delegate ()
            {
                OnLogin(szCode, szRoomId);
            });
        }
        else
        {
            OnLogin(szCode, szRoomId);
        }

    }

    void OnLogin(string szCode, string roomId)
    {
        Debug.Log("Login==== Code:" + szCode + "   RoomId:" + uiInputRoomId.text);

        roomId = roomId.Trim('\n');

        CDanmuSDKCenter.Ins.Login(szCode, roomId, delegate (int value)
        {
            if (value!=0)
            {
                UIManager.Instance.CloseUI(UIResType.NetWait);
                UIToast.Show("连接失败:" + value);
                return;
            }

            Debug.Log("Platform:" + CDanmuSDKCenter.Ins.emPlatform.ToString() + " Connect Suc!");

            listCallSuc.Add(delegate ()
            {
                //保存唯一登录码
                CSystemInfoMgr.Inst.SetVCode(szCode);
                CSystemInfoMgr.Inst.SaveFile();

                //先获取版本号
                //CGameColorFishMgr.Ins.SendHeartBeat(new HHandlerLoginCheckVersion());
                CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetVersion, new HHandlerLoginCheckVersion());
            });
        });
    }

    public void OnClickPlayNormal()
    {
        CGameColorFishMgr.Ins.emCurGameType = CGameColorFishMgr.EMGameType.Normal;

        CloseSelf();
        UIManager.Instance.OpenUI(UIResType.RoomSetting);
    }
    
    public void OnClickPlayVS()
    {
        CGameColorFishMgr.Ins.emCurGameType = CGameColorFishMgr.EMGameType.Battle;

        CloseSelf();
        UIManager.Instance.OpenUI(UIResType.RoomSetting);
    }

    public void OnClickPlayCinema()
    {
        CGameColorFishMgr.Ins.emCurGameType = CGameColorFishMgr.EMGameType.Cinema;

        CloseSelf();

        CGameColorFishMgr.Ins.nMaxPlayerNum = 24;
        CGameColorFishMgr.Ins.pGameRoomConfig.nlBatteryTarget = 0;
        CGameColorFishMgr.Ins.pGameRoomConfig.nAutoExitTime = 120 * 60; //默认2小时离开

        //CGameColorFishMgr.Ins.pGameRoomConfig.bActiveDuel = uiActiveDuel.isOn;
        CGameColorFishMgr.Ins.pGameRoomConfig.bActiveRankRepeat = false;
        CGameColorFishMgr.Ins.pGameRoomConfig.nSelectMapID = 103;

        CSystemInfoMgr.Inst.SetBool(CSystemInfoConst.ADDSEAT, false);

        //CGameColorFishMgr.Ins.SaveRoomConfig(CGameColorFishMgr.EMGameType.Normal);
        CGameColorFishMgr.Ins.pMapConfig = CTBLHandlerGameMap.Ins.GetInfo(103);
        //if (CGameColorFishMgr.Ins.pMapConfig.emType == ST_GameMap.EMType.Normal)
        //{
        //    CGameColorFishMgr.Ins.emCurGameType = CGameColorFishMgr.EMGameType.Normal;
        //}
        //else if (CGameColorFishMgr.Ins.pMapConfig.emType == ST_GameMap.EMType.Boss)
        //{
        //    CGameColorFishMgr.Ins.emCurGameType = CGameColorFishMgr.EMGameType.Boss;
        //}

        UIManager.Instance.OpenUI(UIResType.Loading);
        UIManager.Instance.CloseUI(UIResType.RoomSetting);
        CSceneMgr.Instance.LoadScene(CSceneFactory.EMSceneType.Cinema601);
    }

    public void OnClickPlaySurvive()
    {
        CGameColorFishMgr.Ins.emCurGameType = CGameColorFishMgr.EMGameType.Survive;

        CloseSelf();

        CGameColorFishMgr.Ins.nMaxPlayerNum = 24;
        CGameColorFishMgr.Ins.pGameRoomConfig.nlBatteryTarget = 0;
        CGameColorFishMgr.Ins.pGameRoomConfig.nAutoExitTime = 120 * 60; //默认2小时离开

        CGameColorFishMgr.Ins.pGameRoomConfig.bActiveRankRepeat = false;
        CGameColorFishMgr.Ins.pGameRoomConfig.nSelectMapID = 101;

        CSystemInfoMgr.Inst.SetBool(CSystemInfoConst.ADDSEAT, false);

        CGameColorFishMgr.Ins.pMapConfig = CTBLHandlerGameMap.Ins.GetInfo(101);

        UIManager.Instance.OpenUI(UIResType.Loading);
        UIManager.Instance.CloseUI(UIResType.RoomSetting);
        CSceneMgr.Instance.LoadScene(CSceneFactory.EMSceneType.GameMap502);
    }

    public void OnClickHelp()
    {
        objBoardLogin.SetActive(false);
        objBoardHelp.SetActive(true);
    }

    public void OnClickClearCode()
    {
        uiInputCode.text = "";
    }

    public void OnClickGetCode()
    {
        Application.OpenURL("https://link.bilibili.com/p/center/index#/my-room/start-live");
    }

    public void OnClickGuide()
    {
        UIManager.Instance.OpenUI(UIResType.Guide);
    }

    public void OnClickCloseHelp()
    {
        objBoardLogin.SetActive(true);
        objBoardHelp.SetActive(false);
    }

    public void OnClickSetting()
    {
        UIManager.Instance.OpenUI(UIResType.Setting);
    }

    public void OnClickExit()
    {
        UIMsgBox.Show("", "是否退出到桌面", UIMsgBox.EMType.YesNo, delegate ()
        {
            Application.Quit();
        });
    }

    public void OnClickMMD()
    {
        Application.OpenURL("https://www.bilibili.com/video/BV1784y1b764");
    }

    public void OnClickLoginCode()
    {
        UIMsgBox.Show("", "请联系玩法运营商获取登录码", UIMsgBox.EMType.OK, delegate ()
        {
            
        });
    }

    public void OnClickRoomCode()
    {
        UIMsgBox.Show("", "请在壳程序中获取房间号", UIMsgBox.EMType.OK, delegate ()
        {

        });
    }

}
