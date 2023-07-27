using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUserGetVtbAddSuipian : MonoBehaviour
{
    public UITweenPos uiTweenPos;
    public RawImage uiPlayerIcon;
    public Text uiLabelName;
    public Text uiLabelSuipian;

    public void SetInfo(CPlayerBaseInfo player, long num)
    {
        CAysncImageDownload.Ins.setAsyncImage(player.userFace, uiPlayerIcon);
        uiLabelName.text = player.userName;
        uiLabelSuipian.text = "X" + num;

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
