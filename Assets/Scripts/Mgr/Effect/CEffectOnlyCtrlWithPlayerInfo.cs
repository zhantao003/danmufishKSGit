using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CEffectOnlyCtrlWithPlayerInfo : CEffectOnlyCtrl
{
    public CBoatAvatar pOwner;
    public RawImage uiIcon;

    public override void Init()
    {
        base.Init();
    }

    public override void Play(bool refresh = true)
    {
        base.Play(refresh);

        if(pOwner!=null)
        {
            CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(pOwner.ownerUID);
            if (pPlayer != null)
            {
                SetPlayerInfo(pPlayer);
            }
        }
    }

    public void SetPlayerInfo(CPlayerBaseInfo player)
    {
        if(uiIcon!=null)
        {
            CAysncImageDownload.Ins.setAsyncImage(player.userFace, uiIcon);
        }
    }
}
