using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUserVipSignDaily : MonoBehaviour
{
    public UITweenPos uiTweenPos;
    public RawImage uiPlayerIcon;
    public Text uiLabelName;
    public Text uiLabelGet;
    public Image uiImgJZ;
    public Sprite[] arrResJZ;

    public void SetInfo(CPlayerBaseInfo player, int itemPack, int treasurePoint, UIUserGetAvatar.EMGetFunc getFunc)
    {
        if (player == null)
        {
            Recycle();
            return;
        }

        CAysncImageDownload.Ins.setAsyncImage(player.userFace, uiPlayerIcon);
        uiLabelName.text = player.userName;

        uiTweenPos.from = transform.localPosition;
        uiTweenPos.to = uiTweenPos.from + Vector3.left * 1300F;
        uiTweenPos.Play();
        uiTweenPos.callOver += this.Recycle;

        if(itemPack > 0)
        {
            if (getFunc == UIUserGetAvatar.EMGetFunc.Sign)
            {
                uiLabelGet.text = "签到获得 互动手柄" + itemPack + "套";
            }
            else if (getFunc == UIUserGetAvatar.EMGetFunc.Auction)
            {
                uiLabelGet.text = "竞拍获得 互动手柄" + itemPack + "套";
            }
        }
        else
        {
            if (getFunc == UIUserGetAvatar.EMGetFunc.Sign)
            {
                uiLabelGet.text = "签到获得 海盗金币" + treasurePoint + "个";
            }
            else if (getFunc == UIUserGetAvatar.EMGetFunc.Auction)
            {
                uiLabelGet.text = "竞拍获得 海盗金币" + treasurePoint + "个";
            }
        }

        if (player.guardLevel == 0)
        {
            uiImgJZ.gameObject.SetActive(false);
        }
        else if(player.guardLevel == 1)
        {
            uiImgJZ.gameObject.SetActive(true);
            uiImgJZ.sprite = arrResJZ[2];
        }
        else if(player.guardLevel == 2)
        {
            uiImgJZ.gameObject.SetActive(true);
            uiImgJZ.sprite = arrResJZ[1];
        }
        else  if(player.guardLevel == 3)
        {
            uiImgJZ.gameObject.SetActive(true);
            uiImgJZ.sprite = arrResJZ[0];
        }
    }

    void Recycle()
    {
        Destroy(gameObject);
    }
}
