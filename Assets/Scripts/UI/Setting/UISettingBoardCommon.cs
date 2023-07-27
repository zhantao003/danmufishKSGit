using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettingBoardCommon : MonoBehaviour
{
    [Header("分辨率下拉框")]
    public Dropdown uiDropReslution;
    [Header("提示音下拉框")]
    public Dropdown uiDropTipsModel;
    /// <summary>
    /// 保存所有分辨率的信息
    /// </summary>
    List<ScreenResolutionInfo> listResolutionInfos = new List<ScreenResolutionInfo>();
    
    [Header("全屏模式Tog")]
    public Toggle uiTogFullScreen;
    [Header("特殊声音Tog")]
    public Toggle uiTogSpecialSound;
    [Header("特产显示Tog")]
    public Toggle uiSpecialTog;
    [Header("信息显示Tog")]
    public Toggle uiInfoTog;
    [Header("渔讯显示Tog")]
    public Toggle uiShowInfoTog;
    [Header("场景显示Tog")]
    public Toggle uiSceneTog;
    [Header("盲盒特产Tog")]
    public Toggle uiTreasureInfoTog;
    [Header("主音量滑动条")]
    public Slider uiSliderMasterVolume;
    [Header("音效滑动条")]
    public Slider uiSliderEffectVolume;
    [Header("音乐滑动条")]
    public Slider uiSliderBGM;
    [Header("主音量数值")]
    public Text uiLabelMaterVolume;
    [Header("音效数值")]
    public Text uiLabelEffectVolume;
    [Header("音乐数值")]
    public Text uiLabelBGM;
    [Header("离开时间")]
    public UIRoomConfigDropDown arrExitTimeConfigRoot;

    public GameObject objUIHideRoot;

    public GameObject objBtnRepairNet;

    /// <summary>
    /// 是否设置过信息
    /// </summary>
    public bool bSetInfo;

    public void OnOpen() {
        if (UIManager.Instance.GetUI(UIResType.MainMenu) != null)
        {
            objBtnRepairNet.SetActive(!UIManager.Instance.GetUI(UIResType.MainMenu).IsOpen());
            objUIHideRoot.SetActive(false);
        }
        else
        {
            objBtnRepairNet.SetActive(true);

            if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal)
            {
                objUIHideRoot.SetActive(true);
                RefreshTogInfo();
            }
            else if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
            {
                objUIHideRoot.SetActive(true);
                RefreshTogInfo();
            }
            else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema)
            {
                objUIHideRoot.SetActive(true);
                RefreshTogInfo();
            }    
            else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Battle)
            {
                objUIHideRoot.SetActive(false);
            }
            else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
            {
                objUIHideRoot.SetActive(false);
            }
        }
        arrExitTimeConfigRoot.Init();
        Init();
    }

    public void Init()
    {
        if (listResolutionInfos.Count <= 0)
        {
            ///设置下拉框信息
            SetDropDownInfo();
            SetBroadModelDropInfo();
            ///设置Tog信息
            SetTogInfo();
            ///设置滑动条信息
            SetSliderInfo();
        }
        bSetInfo = false;
    }

    /// <summary>
    /// 设置下拉框信息
    /// </summary>
    public void SetDropDownInfo()
    {
        ///获取当前屏幕的分辨率
        int nCurScreenWidth = CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.RESOLUTIONX);
        int nCurScreenHeight = CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.RESOLUTIONY);

        ///设置分辨率下拉框
        uiDropReslution.onValueChanged.AddListener(ChgResolution);
        int nCurChoice = Screen.resolutions.Length - 1;
        List<string> listStrReslution = new List<string>();
        //string szPreResolution = "";
        //for (int i = Screen.resolutions.Length - 1; i >= 0; i--)
        //{
        //    Resolution resolutions = Screen.resolutions[i];
        //    string szCurResolution = resolutions.width + "x" + resolutions.height;

        //    //过滤相同的分辨率
        //    if (i == Screen.resolutions.Length - 1)
        //    {
        //        szPreResolution = resolutions.width + "x" + resolutions.height;
        //    }
        //    else
        //    {
        //        if (szCurResolution.Equals(szPreResolution))
        //        {
        //            szPreResolution = szCurResolution;
        //            continue;
        //        }
        //    }

        //    //Debug.LogWarning("系统分辨率：" + resolutions.width + " X " + resolutions.height + " " + resolutions.refreshRate);

        //    szPreResolution = szCurResolution;
        //    ScreenResolutionInfo resolutionInfo = new ScreenResolutionInfo(resolutions.width, resolutions.height);

        //    listResolutionInfos.Add(resolutionInfo);
        //    listStrReslution.Add(resolutions.width + "x" + resolutions.height);

        //    if (resolutions.width == nCurScreenWidth &&
        //        resolutions.height == nCurScreenHeight)
        //    {
        //        nCurChoice = listResolutionInfos.Count - 1;
        //    }
        //}
        for (int i = 0; i < 4; i++)
        {
            ScreenResolutionInfo resolutionInfo = null;
            if (i == 0)
            {
                resolutionInfo = new ScreenResolutionInfo(540, 720);
            }
            else if (i == 1)
            {
                resolutionInfo = new ScreenResolutionInfo(675, 900);
            }
            else if (i == 2)
            {
                resolutionInfo = new ScreenResolutionInfo(810, 1080);
            }
            else if (i == 3)
            {
                resolutionInfo = new ScreenResolutionInfo(1080, 1440);
            }
            listResolutionInfos.Add(resolutionInfo);
            listStrReslution.Add(resolutionInfo.nReslutionX + "x" + resolutionInfo.nReslutionY);
        }

        uiDropReslution.ClearOptions();
        uiDropReslution.AddOptions(listStrReslution);
        uiDropReslution.SetValueWithoutNotify(nCurChoice);
    }

    
    /// <summary>
    /// 这是音频模板
    /// </summary>
    public void SetBroadModelDropInfo()
    {
        uiDropTipsModel.onValueChanged.AddListener(ChgBroadModel);

        if (CGameColorFishMgr.Ins.pAudioBroad == null) return;

        int nCurChoice = 0;
        int nCurValue = CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.BROADMODEL);
        List<string> listStrModel = new List<string>();
        for(int i=0; i< CGameColorFishMgr.Ins.pAudioBroad.listBroadModelName.Count; i++)
        {
            if (nCurValue == i)
            {
                nCurChoice = i;
            }

            listStrModel.Add(CGameColorFishMgr.Ins.pAudioBroad.listBroadModelName[i]);
        }

        uiDropTipsModel.ClearOptions();
        uiDropTipsModel.AddOptions(listStrModel);
        uiDropTipsModel.SetValueWithoutNotify(nCurChoice);
    }
    
    /// <summary>
    /// 设置Tog信息
    /// </summary>
    public void SetTogInfo()
    {
        

        uiTogFullScreen.onValueChanged.AddListener(SetFullScreen);
        uiTogFullScreen.isOn = CSystemInfoMgr.Inst.GetBool(CSystemInfoConst.FULLSCREEN);

        uiTogSpecialSound.onValueChanged.AddListener(SetSpecAudioSwitch);
        uiTogSpecialSound.isOn = (CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.SPECIALSOUND) > 0);

        uiSpecialTog.onValueChanged.AddListener(SetSpecialActive);
        uiInfoTog.onValueChanged.AddListener(SetInfoActive);
        uiShowInfoTog.onValueChanged.AddListener(SetShowInfoActive);
        uiSceneTog.onValueChanged.AddListener(SetSceneActive);
        uiTreasureInfoTog.onValueChanged.AddListener(SetTreasureInfoActive);
    }

    void RefreshTogInfo()
    {
        UIRoomInfo uiRoomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
        if(uiRoomInfo!=null)
        {
            uiSpecialTog.isOn = uiRoomInfo.todaySpecialty.objSelf.activeSelf;
            uiInfoTog.isOn = uiRoomInfo.objInfoTip.activeSelf;
        }

        UIShowRoot uiShowRoot = UIManager.Instance.GetUI(UIResType.ShowRoot) as UIShowRoot;
        uiShowInfoTog.isOn = uiShowRoot.objShowRoot.activeSelf;

        CSceneRoot pSceneRoot = FindObjectOfType<CSceneRoot>();
        if(pSceneRoot != null &&
           pSceneRoot.objSceneRoot!= null && 
           pSceneRoot.objSceneRoot.Length > 0 &&
           pSceneRoot.objSceneRoot[0] != null)
        {
            uiSceneTog.isOn = pSceneRoot.objSceneRoot[0].activeSelf;
        }
        else
        {
            uiSceneTog.isOn = true;
        }
    }

    /// <summary>
    /// 设置滑动条信息
    /// </summary>
    public void SetSliderInfo()
    {
        uiSliderMasterVolume.onValueChanged.AddListener(SetMaterVolume);
        uiSliderEffectVolume.onValueChanged.AddListener(SetEffectVolume);
        uiSliderBGM.onValueChanged.AddListener(SetBGM);
        uiSliderMasterVolume.value = CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.ALLSOUND) * 0.01F;
        uiLabelMaterVolume.text = CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.ALLSOUND).ToString();

        uiSliderEffectVolume.value = CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.AUDIO) * 0.01F;
        uiLabelEffectVolume.text = CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.AUDIO).ToString();

        uiSliderBGM.value = CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.BGM) * 0.01F;
        uiLabelBGM.text = CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.BGM).ToString();
    }

    #region DropDownEvent

    /// <summary>
    /// 设置分辨率
    /// </summary>
    /// <param name="nIdx"></param>
    public void ChgResolution(int nIdx)
    {
        bSetInfo = true;
        CSystemInfoMgr.Inst.SetResolution(listResolutionInfos[nIdx].nReslutionX,
                                          listResolutionInfos[nIdx].nReslutionY,
                                          CSystemInfoMgr.Inst.GetBool(CSystemInfoConst.FULLSCREEN));
    }

    /// <summary>
    /// 设置提示音模板
    /// </summary>
    /// <param name="nIdx"></param>
    public void ChgBroadModel(int nIdx)
    {
        bSetInfo = true;
        CSystemInfoMgr.Inst.SaveBroadModel(nIdx);
    }

    #endregion

    #region TogEvent

    

    public void SetFullScreen(bool bValue)
    {
        bSetInfo = true;
        CSystemInfoMgr.Inst.SetResolution(CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.RESOLUTIONX),
                                          CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.RESOLUTIONY),
                                          bValue);
    }

    public void SetSpecAudioSwitch(bool value)
    {
        bSetInfo = true;
        CSystemInfoMgr.Inst.SetInt(CSystemInfoConst.SPECIALSOUND, value ? 1 : 0);
    }

    public void SetSpecialActive(bool bValue)
    {
        UIRoomInfo uiRoomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
        if (uiRoomInfo == null) return;
        uiRoomInfo.todaySpecialty.SetActive(bValue);
    }

    public void SetInfoActive(bool bValue)
    {
        UIRoomInfo uiRoomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
        if (uiRoomInfo != null)
        {
            uiRoomInfo.objInfoTip.SetActive(bValue);
        }

        //if(bValue)
        //{
        //    UIManager.Instance.OpenUI(UIResType.RollText);
        //}
        //else
        //{
        //    UIManager.Instance.CloseUI(UIResType.RollText);
        //}
    }

    public void SetShowInfoActive(bool bValue)
    {
        UIShowRoot uiShowRoot = UIManager.Instance.GetUI(UIResType.ShowRoot) as UIShowRoot;
        if (uiShowRoot != null)
        {
            uiShowRoot.objShowRoot.SetActive(bValue);
        }
    }

    public void SetSceneActive(bool value)
    {
        if (CGameColorFishMgr.Ins != null &&
              CGameColorFishMgr.Ins.pMap != null &&
              CGameColorFishMgr.Ins.pMap.bAdd)
        {
            return;
        }
        CSceneRoot pSceneRoot = FindObjectOfType<CSceneRoot>();
        if (pSceneRoot != null)
        {
            pSceneRoot.ShowScene(value);
        }
    }

    public void SetTreasureInfoActive(bool value)
    {
        return;

        if (value)
        {
            UIManager.Instance.OpenUI(UIResType.TreasureInfo);
        }
        else
        {
            UIManager.Instance.CloseUI(UIResType.TreasureInfo);
        }
    }

    #endregion

    #region SliderEvent

    public void SetMaterVolume(float fValue)
    {
        bSetInfo = true;
        int nFinalValue = (int)(fValue * 100F);
        uiLabelMaterVolume.text = nFinalValue.ToString("f0");

        CSystemInfoMgr.Inst.SaveAllSoundSet(nFinalValue);
        //Debug.Log(CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.AUDIO) + "======Audio");
    }

    public void SetEffectVolume(float fValue)
    {
        bSetInfo = true;
        int nFinalValue = (int)(fValue * 100F);
        uiLabelEffectVolume.text = nFinalValue.ToString("f0");

        CSystemInfoMgr.Inst.SaveAudioSet(nFinalValue);
    }

    public void SetBGM(float fValue)
    {
        bSetInfo = true;
        int nFinalValue = (int)(fValue * 100F);
        uiLabelBGM.text = nFinalValue.ToString("f0");

        CSystemInfoMgr.Inst.SaveBgmSet(nFinalValue);
    }

    #endregion

    /// <summary>
    /// 试听广播提示音
    /// </summary>
    CAudioMgr.CAudioSourcePlayer pAudioPlayerBroad;
    public void OnClickPlayBroadSound()
    {
        if (CGameColorFishMgr.Ins.pAudioBroad == null) return;

        if(pAudioPlayerBroad != null &&
           pAudioPlayerBroad.pSource != null)
        {
            pAudioPlayerBroad.pSource.Stop();
        }

        int nRandType = Random.Range((int)CConfigFishBroad.EMBroadType.UltraFish, (int)CConfigFishBroad.EMBroadType.Max);
        pAudioPlayerBroad = CGameColorFishMgr.Ins.pAudioBroad.Play((CConfigFishBroad.EMBroadModel)CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.BROADMODEL),
                                                                   (CConfigFishBroad.EMBroadType)nRandType);
    }

     /// <summary>
    /// 修复网络
    /// </summary>
    public void OnClickRepairWeb()
    {
        CDanmuSDKCenter.Ins.EndGame(true);

        UIManager.Instance.OpenUI(UIResType.RepaireNet);
    }

    public void OnClickReset()
    {
        UIMsgBox.Show("重置欧皇榜", "是否重置欧皇榜，请谨慎操作", UIMsgBox.EMType.YesNo, delegate ()
        {
            UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
            if (roomInfo != null)
            {
                roomInfo.ClearRankInfo();
            }
            CFishRecordMgr.Ins.ClearAllCoin();
            UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
            if(gameInfo != null)
            {
                gameInfo.uiBatteryTarget.Clear();
            }
        });
    }
}
