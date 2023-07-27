using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUserGetAvatarSlot : MonoBehaviour
{
    public UITweenPos uiTweenPos;
    public RawImage uiPlayerIcon;
    public Text uiLabelName;
    public Text uiLabelFunc;
    public Text uiLabelAvatar;
    public GameObject objFraments;
    public Text uiLabelFraments;

    public Image uiImgBG;
    public Sprite[] arrImgBG;
    
    public void SetInfo(CPlayerBaseInfo player, CPlayerAvatarInfo avatar, UIUserGetAvatar.EMGetFunc func, int returnNum)
    {
        if(player == null ||
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
        else if(func == UIUserGetAvatar.EMGetFunc.Gacha)
        {
            uiLabelFunc.text = "开盒获得";
        }
        else if(func == UIUserGetAvatar.EMGetFunc.Auction)
        {
            uiLabelFunc.text = "竞拍获得";
        }

        ST_UnitAvatar pTBLAvatar = CTBLHandlerUnitAvatar.Ins.GetInfo(avatar.nAvatarId);
        if(pTBLAvatar == null)
        {
            Debug.Log("无效的角色信息：" + avatar.nAvatarId);
            return;
        }

        objFraments.SetActive(returnNum > 0);
        uiLabelFraments.text = "X" + returnNum.ToString();

        //更换BG
        if(pTBLAvatar.emRare == ST_UnitAvatar.EMRare.SSR)
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
