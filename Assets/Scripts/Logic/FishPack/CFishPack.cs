using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFishPack : MonoBehaviour
{
    public enum EMState
    {
        MoveTarget,
        Fishing,
        MoveEnd,
    }

    public Transform tranSelf;

    public EMState emCurState = EMState.MoveTarget;

    public float fMoveTime;

    //钓鱼加成时间
    public Vector2 vRangeFishTime;

    [ReadOnly]
    public CMapSlot pTarget;

    //Vector3 vMoveDir;
    //float fMoveDis;
    Vector3 vEndPos;
    Vector3 vStartPos;

    CPropertyTimer pTickerFishing = new CPropertyTimer();

    public void SetTarget(CMapSlot slot)
    {
        pTarget = slot;
        pTarget.SetFishPack(this);

        emCurState = EMState.MoveTarget;
        pTickerFishing.Value = fMoveTime;
        pTickerFishing.FillTime();

        vStartPos = pTarget.tranSelf.position + Vector3.down * 5f;
        vEndPos = pTarget.tranSelf.position;
        tranSelf.position = pTarget.tranSelf.position + Vector3.down * 5f;
    }

    public void RefreshTime()
    {
        if(emCurState == EMState.MoveEnd)
        {
            SetTarget(pTarget);
        }
        else if(emCurState == EMState.Fishing)
        {
            float fAddTime = Random.Range(vRangeFishTime.x, vRangeFishTime.y);
            pTickerFishing.Value += fAddTime;
            pTickerFishing.CurValue += fAddTime;
        }
    }

    //void GetMoveDir()
    //{
    //    vMoveDir = (pTarget.tranSelf.position - tranSelf.position);
    //    vMoveDir.y = 0F;
    //    vMoveDir = vMoveDir.normalized;
    //}

    private void Update()
    {
        if (emCurState == EMState.Fishing)
        {
            if (pTickerFishing.Tick(CTimeMgr.DeltaTime))
            {
                pTickerFishing.Value = 3.5F;
                pTickerFishing.FillTime();

                emCurState = EMState.MoveEnd;
            }
        }
    }

    private void FixedUpdate()
    {
        if(emCurState == EMState.MoveTarget)
        {
            if(pTickerFishing.Tick(CTimeMgr.FixedDeltaTime))
            {
                tranSelf.position = vEndPos;

                emCurState = EMState.Fishing;
                pTickerFishing.Value = Random.Range(vRangeFishTime.x, vRangeFishTime.y);
                pTickerFishing.FillTime();
            }
            else
            {
                tranSelf.position = Vector3.Lerp(vStartPos, vEndPos, 1F - pTickerFishing.GetTimeLerp());
            }

            //Vector3 vLerp = (pTarget.tranSelf.position - tranSelf.position);
            //GetMoveDir();

            //fMoveDis = fSpeed * CTimeMgr.FixedDeltaTime;

            //if (vLerp.magnitude < fMoveDis)
            //{
            //    tranSelf.position = pTarget.transform.position;
            //    emCurState = EMState.Fishing;
            //    pTickerFishing.Value = Random.Range(vRangeFishTime.x, vRangeFishTime.y);
            //    pTickerFishing.FillTime();
            //    tranSelf.forward = Vector3.forward;
            //}
            //else
            //{
            //    tranSelf.position += fSpeed * CTimeMgr.FixedDeltaTime * vMoveDir;
            //    tranSelf.forward = vMoveDir;
            //}
        }
        else if(emCurState == EMState.MoveEnd)
        {
            tranSelf.position += 1F * CTimeMgr.FixedDeltaTime * Vector3.down;

            if(pTickerFishing.Tick(CTimeMgr.FixedDeltaTime))
            {
                if (pTarget != null)
                {
                    pTarget.SetFishPack(null);
                }
               
                Destroy(gameObject);
            }
        }
    }
}
