using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMNPCExit : FSMNPCBase
{
    Vector3 vTarget;
    CPropertyTimer pJumpTime = new CPropertyTimer();

    Vector3 vStartPos;
    Vector3 vEndPos;
    Vector3 vCenterPos;
    Vector3 vDir;

    public override void OnBegin(object obj)
    {
        pNPCUnit.emCurState = CNPCUnit.EMState.Exit;

        pNPCUnit.PlayAnime(CUnitAnimeConst.Anime_Jump);

        vTarget = pNPCUnit.tranExitPos.position;

        float fJumpTime = Vector3.Distance(vTarget, pNPCUnit.tranSelf.position) / pNPCUnit.fJumpSpeed;
        fJumpTime = Mathf.Max(0.5F, fJumpTime);
        pJumpTime = new CPropertyTimer();
        pJumpTime.Value = fJumpTime;
        pJumpTime.FillTime();
        vStartPos = pNPCUnit.tranSelf.position;
        vEndPos = vTarget;
        vCenterPos = (vEndPos + vStartPos) * 0.5F + Vector3.up * pNPCUnit.fJumpHeight;
        pNPCUnit.SetDir((vTarget - pNPCUnit.tranSelf.position).normalized * 1);

        vDir = (vEndPos - vStartPos).normalized;
        vDir.y = 0F;
        vDir = vDir.normalized;
        pNPCUnit.tranSelf.forward = vDir;

        if (pNPCUnit.pAvatar != null)
        {
            pNPCUnit.pAvatar.PlayJumpEff(true);
            pNPCUnit.pAvatar.ShowHandObj(false);
            pNPCUnit.pAvatar.ShowJumpObj(true);
        }
        CAuctionMgr.Ins.StopAuction();
    }

    public override void OnFixedUpdate(object obj, float delta)
    {
        if (pJumpTime != null)
        {
            if (pJumpTime.Tick(delta))
            {
                pJumpTime = null;
                pNPCUnit.tranSelf.position = vEndPos;
                pNPCUnit.tranSelf.rotation = pNPCUnit.tranExitPos.rotation;
                pNPCUnit.SetState(CNPCUnit.EMState.Idle);
                //pNPCUnit.tranSelf.localPosition = Vector3.zero;
                //pNPCUnit.tranSelf.localRotation = Quaternion.identity;

                //检查是否有特殊的跳跃特效
                if (pNPCUnit.pAvatar != null && !CHelpTools.IsStringEmptyOrNone(pNPCUnit.pAvatar.szEffJumpEnd))
                {
                    CEffectMgr.Instance.CreateEffSync(pNPCUnit.pAvatar.szEffJumpEnd, pNPCUnit.tranSelf, 0, false);
                }
            }
            else
            {
                pNPCUnit.tranSelf.position = CHelpTools.GetCurvePoint(vStartPos, vCenterPos, vEndPos, 1f - pJumpTime.GetTimeLerp());
                pNPCUnit.tranSelf.forward = vDir;
            }
        }
    }

    public override void OnEnd(object obj)
    {
        base.OnEnd(obj);

        if (pNPCUnit.pAvatar != null)
        {
            pNPCUnit.pAvatar.PlayJumpEff(false);
            pNPCUnit.pAvatar.ShowHandObj(true);
            pNPCUnit.pAvatar.ShowJumpObj(false);
        }
    }

}

