using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBossPlayerEatSlot : MonoBehaviour
{
    [ReadOnly]
    public string uid;
    public RawImage uiIcon;
    public Image uiImgFill; 

    public void InitInfo(CPlayerUnit player)
    {
        if (player == null) return;

        uid = player.uid; 
        CAysncImageDownload.Ins.setAsyncImage(player.pInfo.userFace, uiIcon);
    }

    public void RefreshTime(float value)
    {
        uiImgFill.fillAmount = value;
    }
}
