using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUsetGetGiftSlot : MonoBehaviour
{
    public UITweenPos uiTweenPos;
    public RawImage uiPlayerIcon;
    public Text uiLabelPlayerName;
    public Text uiLabelFunc;
    public Text uiLabelGiftName;

    public void SetInfo(CPlayerBaseInfo player, UIUserGetAvatar.EMGetGiftType func,int nValue)
    {
        if (player == null)
        {
            Recycle();
            return;
        }

        CAysncImageDownload.Ins.setAsyncImage(player.userFace, uiPlayerIcon);
        uiLabelPlayerName.text = player.userName;

        if (func == UIUserGetAvatar.EMGetGiftType.Boat)
        {
            uiLabelFunc.text = "��ô�ֻ";
            ST_UnitFishBoat unitFishBoat = CTBLHandlerUnitFishBoat.Ins.GetInfo(nValue);
            if(unitFishBoat != null)
            {
                uiLabelGiftName.text = unitFishBoat.szName + unitFishBoat.nID;
            }
        }
        else if (func == UIUserGetAvatar.EMGetGiftType.Role)
        {
            uiLabelFunc.text = "��ý�ɫ";
            ST_UnitAvatar unitAvatar = CTBLHandlerUnitAvatar.Ins.GetInfo(nValue);
            if(unitAvatar != null)
            {
                uiLabelGiftName.text = unitAvatar.szName + unitAvatar.nID;
            }
        }
        else if (func == UIUserGetAvatar.EMGetGiftType.FishCoin)
        {
            uiLabelFunc.text = "��û���";
            uiLabelGiftName.text = nValue.ToString();
        }
        else if(func == UIUserGetAvatar.EMGetGiftType.FishItem)
        {
            uiLabelFunc.text = "��ü���Ѽ";
            uiLabelGiftName.text = nValue.ToString() + "��";
        }
        else if(func == UIUserGetAvatar.EMGetGiftType.FishLun)
        {
            uiLabelFunc.text = "��ó�������";
            uiLabelGiftName.text = nValue.ToString() + "��";
        }
        else if(func == UIUserGetAvatar.EMGetGiftType.FishGan)
        {
            uiLabelFunc.text = "������";
            ST_UnitFishGan unitAvatar = CTBLHandlerUnitFishGan.Ins.GetInfo(nValue);
            if (unitAvatar != null)
            {
                uiLabelGiftName.text = unitAvatar.szName + unitAvatar.nID;
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
