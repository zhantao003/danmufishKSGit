using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUserGetSign : MonoBehaviour
{
    public UITweenPos uiTweenPos;
    public RawImage uiPlayerIcon;
    public Text uiLabelName;
    public void SetInfo(CPlayerBaseInfo player)
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
    }

    void Recycle()
    {
        Destroy(gameObject);
    }
}
