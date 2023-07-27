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
            uiLabelFunc.text = "�һ����";
        }
        else if (func == UIUserGetAvatar.EMGetFunc.Gacha)
        {
            uiLabelFunc.text = "������";
        }
        else if(func == UIUserGetAvatar.EMGetFunc.Drop)
        {
            uiLabelFunc.text = "������";
        }
        else if (func == UIUserGetAvatar.EMGetFunc.Auction)
        {
            uiLabelFunc.text = "���Ļ��";
        }

        ST_UnitFishBoat pTBLAvatar = CTBLHandlerUnitFishBoat.Ins.GetInfo(avatar.nBoatId);
        if (pTBLAvatar == null)
        {
            Debug.Log("��Ч�Ĵ���Ϣ��" + avatar.nBoatId);
            return;
        }

        //����BG
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
            uiLabelFunc.text = "�һ����";
        }
        else if (func == UIUserGetAvatar.EMGetFunc.Gacha)
        {
            uiLabelFunc.text = "������";
        }
        else if (func == UIUserGetAvatar.EMGetFunc.Drop)
        {
            uiLabelFunc.text = "������";
        }
        else if (func == UIUserGetAvatar.EMGetFunc.Auction)
        {
            uiLabelFunc.text = "���Ļ��";
        }

        ST_UnitFishGan pTBLAvatar = CTBLHandlerUnitFishGan.Ins.GetInfo(avatar.nGanId);
        if (pTBLAvatar == null)
        {
            Debug.Log("��Ч�������Ϣ��" + avatar.nGanId);
            return;
        }

        //����BG
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
