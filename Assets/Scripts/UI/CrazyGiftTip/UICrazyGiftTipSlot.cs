using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICrazyGiftTipSlot : MonoBehaviour
{
    public UITweenPos uiTweenPos;
    public RawImage uiPlayerIcon;
    public Text uiLabelName;
    public Text uiLabelFunc;
    public Text uiLabelAvatar;

    public int nTimeLerp = 100;

    public DelegateNFuncCall deleShowOverEvent;

    public void ShowInfo(CSpecialGift specialGiftInfo)
    {
        if (specialGiftInfo.baseInfo == null ||
            specialGiftInfo.giftInfo == null)
        {
            return;
        }

        CAysncImageDownload.Ins.setAsyncImage(specialGiftInfo.baseInfo.userFace, uiPlayerIcon);
        uiLabelName.text = specialGiftInfo.baseInfo.userName;

        uiLabelAvatar.text = "�����ɻ�x" + specialGiftInfo.giftInfo.count;

        uiLabelFunc.text = "����" + specialGiftInfo.giftInfo.count * nTimeLerp + "�뼱��";

        //uiTweenPos.from = transform.localPosition;
        //uiTweenPos.to = uiTweenPos.from + Vector3.left * 1300F;
        uiTweenPos.Play(delegate()
        {
            deleShowOverEvent?.Invoke();
        });
    }
}
