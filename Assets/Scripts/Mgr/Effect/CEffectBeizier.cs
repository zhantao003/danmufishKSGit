using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEffectBeizier : CEffectBase
{
    Transform tranSelf;
    [ReadOnly]
    public Vector3 vStart;
    [ReadOnly]
    public Transform tranEnd;
    [ReadOnly]
    public Vector3 vCenter;
    public float fSpd;
    public float fCenterHeight;
    public float fTargetAddHeight;
    CPropertyTimer pMoveTicker = null;

    public string szBoomEff;

    public bool bRecyleMoveEnd = true;
    public bool bFwdReset = false;

    public ParticleSystemRenderer pEff;

    public void SetRenderTexture(Texture texture)
    {
        pEff.material.SetTexture("_MainTex", texture);
    }

    public void SetTarget(Vector3 start, Transform end, DelegateNFuncCall endEvent = null)
    {
        pEndEvent = endEvent;
        vStart = start;
        tranEnd = end;
        vCenter = (tranEnd.position + vStart) * 0.5F + Vector3.up * fCenterHeight;

        float fMoveTime = (tranEnd.position - vStart).magnitude / fSpd;
        pMoveTicker = new CPropertyTimer();
        pMoveTicker.Value = fMoveTime;
        pMoveTicker.FillTime();

        tranSelf = gameObject.GetComponent<Transform>();

        if(bFwdReset)
        {
            tranSelf.forward = (vCenter - vStart).normalized;
        }
    }

    Vector3 vPrePos;
    private void FixedUpdate()
    {
        if (pMoveTicker == null || !bPlaying) return;

        if(tranEnd == null)
        {
            pMoveTicker = null;
            Recycle();
            return;
        }

        if(pMoveTicker.Tick(CTimeMgr.FixedDeltaTime))
        {
            pMoveTicker = null;

            tranSelf.position = (tranEnd.position + Vector3.up * fTargetAddHeight);

            if(!CHelpTools.IsStringEmptyOrNone(szBoomEff))
            {
                CEffectMgr.Instance.CreateEffSync(szBoomEff, tranSelf, 0);
            }
            
            if (bRecyleMoveEnd)
                Recycle();
        }
        else
        {
            vPrePos = tranSelf.position;
            tranSelf.position = CHelpTools.GetCurvePoint(vStart, vCenter, (tranEnd.position + Vector3.up * fTargetAddHeight), 1F - pMoveTicker.GetTimeLerp());
        
            if(bFwdReset)
            {
                tranSelf.forward = (tranSelf.position - vPrePos).normalized;
            }
        }
    }
}
