using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUserGiftGachaBoxSlot : MonoBehaviour
{
    public UITweenPos uiTweenPos;
    public RawImage uiPlayerIcon;
    public Text uiLabelName;
    public Text uiLabelFunc;
    public Text uiLabelAvatar;

    public Image uiImgBG;
    public Sprite[] arrImgBG;

    public void SetInfo(CPlayerBaseInfo player, CGiftGachaBoxInfo info)
    {
        if (player == null ||
            info == null)
        {
            Recycle();
            return;
        }

        CAysncImageDownload.Ins.setAsyncImage(player.userFace, uiPlayerIcon);
        uiLabelName.text = player.userName;

        uiImgBG.sprite = arrImgBG[0];
        if (info.emType == CGiftGachaBoxInfo.EMGiftType.FishCoin)
        {
            uiLabelAvatar.text = info.nItemID + "积分";
        }
        else if(info.emType == CGiftGachaBoxInfo.EMGiftType.Yuju)
        {
            uiLabelAvatar.text = info.nItemID + "套加油鸭";
        }
        else if(info.emType == CGiftGachaBoxInfo.EMGiftType.Feilun)
        {
            uiLabelAvatar.text = info.nItemID + "个初级卷轴";
        }
        else if(info.emType == CGiftGachaBoxInfo.EMGiftType.Role)
        {
            uiImgBG.sprite = arrImgBG[1];
            ST_UnitAvatar pTBLAvatarInfo = CTBLHandlerUnitAvatar.Ins.GetInfo(info.nItemID);
            if(pTBLAvatarInfo!=null)
            {
                uiLabelAvatar.text = pTBLAvatarInfo.szName + "#" + pTBLAvatarInfo.nID;
            }
        }
        else if(info.emType == CGiftGachaBoxInfo.EMGiftType.Boat)
        {
            uiImgBG.sprite = arrImgBG[1];
            ST_UnitFishBoat pTBLAvatarInfo = CTBLHandlerUnitFishBoat.Ins.GetInfo(info.nItemID);
            if (pTBLAvatarInfo != null)
            {
                uiLabelAvatar.text = pTBLAvatarInfo.szName + "#" + pTBLAvatarInfo.nID;
            }
        }
        else if(info.emType == CGiftGachaBoxInfo.EMGiftType.FishGan)
        {
            uiImgBG.sprite = arrImgBG[1];
            ST_UnitFishGan pTBLAvatarInfo = CTBLHandlerUnitFishGan.Ins.GetInfo(info.nItemID);
            if (pTBLAvatarInfo != null)
            {
                uiLabelAvatar.text = pTBLAvatarInfo.szName + "#" + pTBLAvatarInfo.nID;
            }
        }

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
