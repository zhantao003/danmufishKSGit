using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMUnitDragCancel : FSMUnitBase
{
    CPropertyTimer pDownTime;
    CPropertyTimer pJumpTime;

    Vector3 vDownStartPos;
    Vector3 vDownEndPos;

    Vector3 vTarget;
    Vector3 vStartPos;
    Vector3 vEndPos;
    Vector3 vCenterPos;
    Vector3 vDir;

    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CPlayerUnit.EMState.DragCancel;

        pUnit.PlayAnime(CUnitAnimeConst.Anime_Idle);

        vDownStartPos = pUnit.tranSelf.position;
        vDownEndPos = pUnit.tranSelf.position - Vector3.up * 3.6f;
        pUnit.tranSelf.position = vDownStartPos;

        pDownTime = new CPropertyTimer();
        pDownTime.Value = (vDownEndPos - pUnit.tranSelf.position).magnitude / 6.4f;
        pDownTime.FillTime();
    }

    public override void OnFixedUpdate(object obj, float delta)
    {
        if (pDownTime != null)
        {
            if (pDownTime.Tick(delta))
            {
                pDownTime = null;
                pUnit.tranSelf.position = vDownEndPos;
                JumpBack();
            }
            else
            {
                pUnit.tranSelf.position = Vector3.Lerp(vDownStartPos, vDownEndPos, 1f - pDownTime.GetTimeLerp());
            }
        }
        if(pJumpTime != null)
        {
            if(pJumpTime.Tick(delta))
            {
                pJumpTime = null;
                pUnit.tranSelf.position = vEndPos;
                pUnit.SetState(CPlayerUnit.EMState.StartFish);
                pUnit.tranSelf.localPosition = Vector3.zero;
                pUnit.tranSelf.localRotation = Quaternion.identity;

                //检查是否有特殊的跳跃特效
                if (pUnit.pAvatar != null && !CHelpTools.IsStringEmptyOrNone(pUnit.pAvatar.szEffJumpEnd))
                {
                    CEffectMgr.Instance.CreateEffSync(pUnit.pAvatar.szEffJumpEnd, pUnit.tranSelf, 0, false);
                }
            }
            else
            {
                pUnit.tranSelf.position = CHelpTools.GetCurvePoint(vStartPos, vCenterPos, vEndPos, 1f - pJumpTime.GetTimeLerp());
                pUnit.tranSelf.forward = vDir;
            }
        }

    }

    
    void JumpBack()
    {
        pUnit.PlayAnime(CUnitAnimeConst.Anime_Jump);

        vTarget = pUnit.pMapSlot.tranSelf.position;

        float fJumpTime = Vector3.Distance(vTarget, pUnit.tranSelf.position) / pUnit.fJumpSpeed;
        fJumpTime = Mathf.Max(0.5F, fJumpTime);
        pJumpTime.Value = fJumpTime;
        pJumpTime.FillTime();
        vStartPos = pUnit.tranSelf.position;
        vEndPos = vTarget;
        vCenterPos = (vEndPos + vStartPos) * 0.5F + Vector3.up * pUnit.fJumpHeight;
        pUnit.SetDir((vTarget - pUnit.tranSelf.position).normalized * 1);

        vDir = (vEndPos - vStartPos).normalized;
        vDir.y = 0F;
        vDir = vDir.normalized;
        pUnit.tranSelf.forward = vDir;

        if (pUnit.pAvatar != null)
        {
            pUnit.pAvatar.PlayJumpEff(true);
            pUnit.pAvatar.ShowHandObj(false);
            pUnit.pAvatar.ShowJumpObj(true);
        }
    }
}

