using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEffectBeam : CEffectBase
{
    public LineRenderer pLine;
    public float fSpdUV;
    public float fLenScale = 1f;
    public float fWidth = 1F;

    public float fCurWidth = 0F;
    public AnimationCurve pLineWidthCurve;

    public void SetBeam(Vector3 start, Vector3 end)
    {
        float distance = (end - start).magnitude;
        pLine.material.mainTextureScale = new Vector2(distance / fLenScale, 1);

        fCurWidth = pLineWidthCurve.Evaluate(0) * fWidth;
        pLine.startWidth = fCurWidth;
        pLine.endWidth = fCurWidth;
        pLine.SetPosition(0, start);
        pLine.SetPosition(1, end);
    }

    protected override void OnUpdate(float dt)
    {
        if (bAutoRecycle && bPlaying)
        {
            if (pLifeTicker.Tick(dt))
            {
                pLine.startWidth = pLineWidthCurve.Evaluate(1F) * fWidth;
                pLine.endWidth = pLineWidthCurve.Evaluate(1F) * fWidth;

                bPlaying = false;
                Recycle();
            }
            else
            {
                pLine.startWidth = pLineWidthCurve.Evaluate(1F - pLifeTicker.GetTimeLerp()) * fWidth;
                pLine.endWidth = pLineWidthCurve.Evaluate(1F - pLifeTicker.GetTimeLerp()) * fWidth;
            }
        }

        for(int i=0; i<arrEffAudio.Length; i++)
        {
            arrEffAudio[i].OnUpdate(dt, transform.position);
        }
    }
}
