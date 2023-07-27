using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShowPlayerInfo : MonoBehaviour
{
    public GameObject objSelf;

    public float fLifeTime;
    CPropertyTimer pLifeTick;

    public RawImage uiPlayerIcon;
    public Text uiPlayerName;

    public void SetInfo(CPlayerBaseInfo info)
    {
        if(info == null)
        {
            return;
        }
        objSelf.SetActive(true);
        if (uiPlayerIcon != null)
        {
            CAysncImageDownload.Ins.setAsyncImage(info.userFace, uiPlayerIcon);
        }
        if(uiPlayerName != null)
        {
            uiPlayerName.text = info.userName;
        }
        pLifeTick = new CPropertyTimer();
        pLifeTick.Value = fLifeTime;
        pLifeTick.FillTime();
    }

    private void Update()
    {
        if(pLifeTick != null &&
            pLifeTick.Tick(CTimeMgr.DeltaTime))
        {
            pLifeTick = null;
            objSelf.SetActive(false);
        }
    }



}
