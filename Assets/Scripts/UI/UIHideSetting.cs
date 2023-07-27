using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHideSetting : UIBase
{
    [Header("特产显示Tog")]
    public Toggle uiSpecialTog;
    [Header("信息显示Tog")]
    public Toggle uiInfoTog;
    [Header("渔讯显示Tog")]
    public Toggle uiShowInfoTog;

    bool bSetInfo;

    public override void OnOpen()
    {
        base.OnOpen();
        Init();
    }

    public void Init()
    {
        ///设置Tog信息
        SetTogInfo();
        bSetInfo = false;
    }

    /// <summary>
    /// 设置Tog信息
    /// </summary>
    public void SetTogInfo()
    {
        UIRoomInfo uiRoomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
        
        uiSpecialTog.onValueChanged.AddListener(SetSpecialActive);
        uiSpecialTog.isOn = uiRoomInfo.todaySpecialty.objSelf.activeSelf;

        uiInfoTog.onValueChanged.AddListener(SetInfoActive);
        uiInfoTog.isOn = uiRoomInfo.objInfoTip.activeSelf;

        UIShowRoot uiShowRoot = UIManager.Instance.GetUI(UIResType.ShowRoot) as UIShowRoot;

        uiShowInfoTog.onValueChanged.AddListener(SetShowInfoActive);
        uiShowInfoTog.isOn = uiShowRoot.objShowRoot.activeSelf;
    }

    public void SetSpecialActive(bool bValue)
    {
        UIRoomInfo uiRoomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
        uiRoomInfo.todaySpecialty.SetActive(bValue);
    }

    public void SetInfoActive(bool bValue)
    {
        UIRoomInfo uiRoomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
        uiRoomInfo.objInfoTip.SetActive(bValue);
    }

    public void SetShowInfoActive(bool bValue)
    {
        UIShowRoot uiShowRoot = UIManager.Instance.GetUI(UIResType.ShowRoot) as UIShowRoot;
        uiShowRoot.objShowRoot.SetActive(bValue);
    }

    /// <summary>
    /// 保存并退出
    /// </summary>
    public void OnClickSave()
    {
        //if (bSetInfo)
        //{
        //    bSetInfo = false;
        //    CSystemInfoMgr.Inst.SaveFile();
        //}

        CloseSelf();
    }

}
