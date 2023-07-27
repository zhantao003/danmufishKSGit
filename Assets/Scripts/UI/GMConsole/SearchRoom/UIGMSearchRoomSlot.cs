using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGMSearchRoomSlot : MonoBehaviour
{
    public Text uiLabelName;
    public Text uiLabelMapLv;
    public Text uiLabelUid;
    public Text uiLabelRoomId;

    public void SetInfo(CLocalNetMsg msgInfo)
    {
        uiLabelName.text = msgInfo.GetString("name");
        uiLabelMapLv.text = msgInfo.GetInt("mapLv") + "¼¶Óæ³¡";
        uiLabelUid.text = msgInfo.GetString("uid");
        uiLabelRoomId.text = msgInfo.GetString("roomId");
    }

    public void OnClickJump()
    {
        Application.OpenURL("https://live.bilibili.com/" + uiLabelRoomId.text);
    }
}
