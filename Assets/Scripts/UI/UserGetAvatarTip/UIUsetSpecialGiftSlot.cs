using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUsetSpecialGiftSlot : MonoBehaviour
{
    public UITweenPos uiTweenPos;
    public RawImage uiPlayerIcon;
    public Text uiLabelName;
    public Text uiLabelFunc;
    public Text uiLabelAvatar;

    public GameObject[] objImgGift;

    public void SetInfo(CPlayerBaseInfo player, CSpecialGiftInfo info)
    {
        if (player == null ||
            info == null)
        {
            Recycle();
            return;
        }

        CAysncImageDownload.Ins.setAsyncImage(player.userFace, uiPlayerIcon);
        uiLabelName.text = player.userName;

        for(int i = 0;i < objImgGift.Length;i++)
        {
            objImgGift[i].SetActive(i == (int)info.emType);
        }
        uiLabelAvatar.text = "x" + info.count;
        //if (info.emType == CSpecialGiftInfo.EMGiftType.Item1)
        //{
        //    uiLabelAvatar.text = info.count + "积分";
        //}
        //else if (info.emType == CSpecialGiftInfo.EMGiftType.Item2)
        //{
        //    uiLabelAvatar.text = info.count + "套互动手柄";
        //}
        //else if (info.emType == CSpecialGiftInfo.EMGiftType.Item3)
        //{
        //    uiLabelAvatar.text = info.count + "个初级卷轴";
        //}

        uiTweenPos.from = transform.localPosition;
        uiTweenPos.to = uiTweenPos.from + Vector3.left * 1300F;
        uiTweenPos.Play();
        uiTweenPos.callOver += this.Recycle;
    }

    void Recycle()
    {
        Destroy(gameObject);
    }

}
