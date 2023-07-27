using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHideSetting : UIBase
{
    [Header("�ز���ʾTog")]
    public Toggle uiSpecialTog;
    [Header("��Ϣ��ʾTog")]
    public Toggle uiInfoTog;
    [Header("��Ѷ��ʾTog")]
    public Toggle uiShowInfoTog;

    bool bSetInfo;

    public override void OnOpen()
    {
        base.OnOpen();
        Init();
    }

    public void Init()
    {
        ///����Tog��Ϣ
        SetTogInfo();
        bSetInfo = false;
    }

    /// <summary>
    /// ����Tog��Ϣ
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
    /// ���沢�˳�
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
