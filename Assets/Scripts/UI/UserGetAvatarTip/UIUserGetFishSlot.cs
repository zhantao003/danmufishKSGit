using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUserGetFishSlot : MonoBehaviour
{
    public UITweenPos uiTweenPos;
    public RawImage uiPlayerIcon;
    public UIRawIconLoad uiIconLoad;
    public Text uiLabelPlayerName;
    public Text uiLabelFunc;
    public Text uiLabelGiftName;

    public void SetInfo(CPlayerBaseInfo player, CFishInfo fishInfo)
    {
        if (player == null)
        {
            Recycle();
            return;
        }

        CAysncImageDownload.Ins.setAsyncImage(player.userFace, uiPlayerIcon);
        uiLabelPlayerName.text = player.userName;

        uiLabelGiftName.text = fishInfo.szName;

        if (!CHelpTools.IsStringEmptyOrNone(fishInfo.szIcon))
        {
            uiIconLoad.gameObject.SetActive(true);
            uiIconLoad.SetIconSync(fishInfo.szIcon);
        }
        else
        {
            uiIconLoad.gameObject.SetActive(false);
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
