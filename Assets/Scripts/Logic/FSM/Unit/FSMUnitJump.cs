using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMUnitJump : FSMUnitBase
{
    Vector3 vTarget;
    CPropertyTimer pJumpTime = new CPropertyTimer();

    Vector3 vStartPos;
    Vector3 vEndPos;
    Vector3 vCenterPos;
    Vector3 vDir;

    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CPlayerUnit.EMState.Jump;

        pUnit.PlayAnime(CUnitAnimeConst.Anime_Jump);

        //if (pMsgParam != null)
        //{
        //    vTarget = new Vector3(pMsgParam.GetFloat("posX"), pMsgParam.GetFloat("posY"), pMsgParam.GetFloat("posZ"));
        //}

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
            pUnit.pAvatar.PlaySpringMgr(true);
            pUnit.pAvatar.PlayJumpEff(true);
            pUnit.pAvatar.ShowHandObj(false);
            pUnit.pAvatar.ShowJumpObj(true);
        }
    }

    public override void OnFixedUpdate(object obj, float delta)
    {
        vEndPos = pUnit.pMapSlot.tranSelf.position;

        if (pJumpTime.Tick(delta))
        {
            pUnit.tranSelf.position = vEndPos;
            pUnit.SetState(CPlayerUnit.EMState.StartFish);
            pUnit.tranSelf.localPosition = Vector3.zero;
            pUnit.tranSelf.localRotation = Quaternion.identity;

            //检查是否有特殊的跳跃特效
            if (pUnit.pAvatar != null && !CHelpTools.IsStringEmptyOrNone(pUnit.pAvatar.szEffJumpEnd))
            {
                CEffectMgr.Instance.CreateEffSync(pUnit.pAvatar.szEffJumpEnd, pUnit.tranSelf, 0, false);
            }

            //检查是否有特殊的跳跃特效
            if (pUnit != null)
            {
                pUnit.dlgCallOnceJumpEvent?.Invoke();
                pUnit.dlgCallOnceJumpEvent = null;
            }

            //CEffectMgr.Instance.CreateEffSync($"Effect/boom_{pUnit.pInfo.pGameInfo.emColor.ToString().ToLower()}", vTarget.tranSelf, 0, false);
        }
        else
        {
            pUnit.tranSelf.position = CHelpTools.GetCurvePoint(vStartPos, vCenterPos, vEndPos, 1f - pJumpTime.GetTimeLerp());
            pUnit.tranSelf.forward = vDir;
        }
    }

    public override void OnEnd(object obj)
    {
        base.OnEnd(obj);

        if (pUnit.pAvatar != null)
        {
            pUnit.pAvatar.PlayJumpEff(false);
            pUnit.pAvatar.ShowHandObj(true);
            pUnit.pAvatar.ShowJumpObj(false);
        }
    }

}

