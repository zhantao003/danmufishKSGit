using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomOwnerInfo : MonoBehaviour
{
    [Header("����ͷ��")]
    public RawImage uiIcon;
    [Header("��������")]
    public Text uiLabelName;
    [Header("��������ID")]
    public Text uiLabelRoomID;
    [Header("���㳡�ȼ�")]
    public Text uiFishRoomLevel;

    public void InitInfo()
    {
        //����ͷ��
        if(CPlayerMgr.Ins.pOwner != null)
        {
            if (uiIcon != null)
            {
                CAysncImageDownload.Ins.setAsyncImage(CPlayerMgr.Ins.pOwner.userFace, uiIcon);
            }
            if (uiLabelName != null)
            {
                uiLabelName.text = CPlayerMgr.Ins.pOwner.userName;
            }
        }
        else
        {
            if(uiLabelName != null)
            {
                uiLabelName.text = "";
            }
        }
        if (uiLabelRoomID != null)
        {
            uiLabelRoomID.text = "����ţ�" + CDanmuSDKCenter.Ins.szRoomId.ToString();
        }
        if(uiFishRoomLevel != null)
        {
            ST_SceneRate mapRate = CTBLHandlerSceneRate.Ins.GetInfo(CGameColorFishMgr.Ins.nCurRateUpLv);
            if (mapRate != null)
            {
                uiFishRoomLevel.text = mapRate.szName;
            }
        }
    }
}
