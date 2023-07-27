using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAutoUVAnime : MonoBehaviour
{
    public float fXSpd;
    public float fYSpd;
    public string szTexName;
    public Renderer pRenderer;
    public Vector2 vCurOffset;

    private void FixedUpdate()
    {
        vCurOffset.x += fXSpd * CTimeMgr.FixedDeltaTime;
        if(vCurOffset.x > 1F)
        {
            vCurOffset.x -= 1F;
        }
        else if(vCurOffset.x < -1F)
        {
            vCurOffset.x += 1F;
        }

        vCurOffset.y += fYSpd * CTimeMgr.FixedDeltaTime;
        if (vCurOffset.y > 1F)
        {
            vCurOffset.y -= 1F;
        }
        else if (vCurOffset.y < -1F)
        {
            vCurOffset.y += 1F;
        }

        pRenderer.material.SetTextureOffset(szTexName, vCurOffset);
    }
}
