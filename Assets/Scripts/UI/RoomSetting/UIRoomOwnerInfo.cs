using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomOwnerInfo : MonoBehaviour
{
    [Header("主播头像")]
    public RawImage uiIcon;
    [Header("主播名字")]
    public Text uiLabelName;
    [Header("主播房间ID")]
    public Text uiLabelRoomID;
    [Header("钓鱼场等级")]
    public Text uiFishRoomLevel;

    public void InitInfo()
    {
        //加载头像
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
            uiLabelRoomID.text = "房间号：" + CDanmuSDKCenter.Ins.szRoomId.ToString();
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
