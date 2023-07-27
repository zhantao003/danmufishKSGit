using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUserGetBoatSlot : MonoBehaviour
{
    public UITweenPos uiTweenPos;
    public RawImage uiPlayerIcon;
    public Text uiLabelName;
    public Text uiLabelFunc;
    public Text uiLabelAvatar;

    public Image uiImgBG;
    public Sprite[] arrImgBG;

    public void SetInfo(CPlayerBaseInfo player, CPlayerBoatInfo avatar, UIUserGetAvatar.EMGetFunc func)
    {
        if (player == null ||
            avatar == null)
        {
            Recycle();
            return;
        }

        CAysncImageDownload.Ins.setAsyncImage(player.userFace, uiPlayerIcon);
        uiLabelName.text = player.userName;

        if (func == UIUserGetAvatar.EMGetFunc.Exchange)
        {
            uiLabelFunc.text = "兑换获得";
        }
        else if (func == UIUserGetAvatar.EMGetFunc.Gacha)
        {
            uiLabelFunc.text = "开箱获得";
        }
        else if(func == UIUserGetAvatar.EMGetFunc.Drop)
        {
            uiLabelFunc.text = "掉落获得";
        }
        else if (func == UIUserGetAvatar.EMGetFunc.Auction)
        {
            uiLabelFunc.text = "竞拍获得";
        }

        ST_UnitFishBoat pTBLAvatar = CTBLHandlerUnitFishBoat.Ins.GetInfo(avatar.nBoatId);
        if (pTBLAvatar == null)
        {
            Debug.Log("无效的船信息：" + avatar.nBoatId);
            return;
        }

        //更换BG
        if (pTBLAvatar.emRare == ST_UnitFishBoat.EMRare.SSR)
        {
            uiImgBG.sprite = arrImgBG[1];
        }
        else
        {
            uiImgBG.sprite = arrImgBG[0];
        }

        uiLabelAvatar.text = pTBLAvatar.szName + "#" + pTBLAvatar.nID;
        uiTweenPos.from = transform.localPosition;
        uiTweenPos.to = uiTweenPos.from + Vector3.left * 1300F;
        uiTweenPos.Play();
        uiTweenPos.callOver += this.Recycle;
    }

    public void SetGanInfo(CPlayerBaseInfo player, CPlayerFishGanInfo avatar, UIUserGetAvatar.EMGetFunc func)
    {
        if (player == null ||
            avatar == null)
        {
            Recycle();
            return;
        }

        CAysncImageDownload.Ins.setAsyncImage(player.userFace, uiPlayerIcon);
        uiLabelName.text = player.userName;

        if (func == UIUserGetAvatar.EMGetFunc.Exchange)
        {
            uiLabelFunc.text = "兑换获得";
        }
        else if (func == UIUserGetAvatar.EMGetFunc.Gacha)
        {
            uiLabelFunc.text = "开箱获得";
        }
        else if (func == UIUserGetAvatar.EMGetFunc.Drop)
        {
            uiLabelFunc.text = "掉落获得";
        }
        else if (func == UIUserGetAvatar.EMGetFunc.Auction)
        {
            uiLabelFunc.text = "竞拍获得";
        }

        ST_UnitFishGan pTBLAvatar = CTBLHandlerUnitFishGan.Ins.GetInfo(avatar.nGanId);
        if (pTBLAvatar == null)
        {
            Debug.Log("无效的鱼竿信息：" + avatar.nGanId);
            return;
        }

        //更换BG
        if (pTBLAvatar.emRare == ST_UnitFishGan.EMRare.SSR)
        {
            uiImgBG.sprite = arrImgBG[1];
        }
        else
        {
            uiImgBG.sprite = arrImgBG[0];
        }

        uiLabelAvatar.text = pTBLAvatar.szName + "#" + pTBLAvatar.nID;
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
