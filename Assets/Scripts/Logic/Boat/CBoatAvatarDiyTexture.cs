using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CBoatDiyTexture
{
    public string nUid;
    public Texture pTexture;
}

public class CBoatAvatarDiyTexture : CBoatAvatar
{
    public CBoatDiyTexture[] arrDiyTex;
    public Renderer pRender;


    public override void SetOwner(string uid)
    {
        base.SetOwner(uid);

        if (pRender == null) return;
        for (int i = 0; i < arrDiyTex.Length; i++)
        {
            if(arrDiyTex[i].nUid.Equals(uid))
            {
                pRender.material.mainTexture = arrDiyTex[i].pTexture;
                pRender.material.SetTexture("_EmissionMap", arrDiyTex[i].pTexture);
                break;
            }
        }
    }
}
