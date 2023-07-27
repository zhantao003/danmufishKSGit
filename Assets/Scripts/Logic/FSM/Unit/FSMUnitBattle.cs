using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMUnitBattle : FSMUnitBase
{
    Vector3 vTarget;
    CPropertyTimer pJumpTime;

    Vector3 vStartPos;
    Vector3 vEndPos;
    Vector3 vCenterPos;

    public override void OnBegin(object obj)
    {
        //Debug.Log("Idle");
        pUnit.bCanLaGan = false;
        pUnit.emCurState = CPlayerUnit.EMState.Battle;
        if (pMsgParam != null)
        {
            pUnit.PlayAnime(CUnitAnimeConst.Anime_Jump);

            if (pUnit.pAvatar != null)
            {
                pUnit.pAvatar.PlaySpringMgr(true);
            }

            vTarget = new Vector3(pMsgParam.GetFloat("posX"), pMsgParam.GetFloat("posY"), pMsgParam.GetFloat("posZ"));
            float fJumpTime = Vector3.Distance(vTarget, pUnit.tranSelf.position) / pUnit.fJumpSpeed / 2f;
            fJumpTime = Mathf.Max(0.5F, fJumpTime);
            pJumpTime = new CPropertyTimer();
            pJumpTime.Value = fJumpTime;
            pJumpTime.FillTime();
            vStartPos = pUnit.tranSelf.position;
            vEndPos = vTarget;
            vCenterPos = (vEndPos + vStartPos) * 0.5F + Vector3.up * pUnit.fJumpHeight;
            pUnit.SetDir((vTarget - pUnit.tranSelf.position).normalized * 1);
            if (pUnit.pAvatar != null)
            {
                pUnit.pAvatar.PlayJumpEff(true);
                pUnit.pAvatar.ShowHandObj(false);
                pUnit.pAvatar.ShowJumpObj(true);
            }
        }
        else
        {
            pJumpTime = null;
            pUnit.PlayAnime(CUnitAnimeConst.Anime_Fishing);
        }
       
    }

    public override void OnFixedUpdate(object obj, float delta)
    {
        if (pJumpTime != null)
        {
            if (pJumpTime.Tick(delta))
            {
                pJumpTime = null;
                pUnit.tranSelf.localPosition = Vector3.zero;
                pUnit.tranSelf.localRotation = Quaternion.identity;
                //检查是否有特殊的跳跃特效
                if (pUnit.pAvatar != null && !CHelpTools.IsStringEmptyOrNone(pUnit.pAvatar.szEffJumpEnd))
                {
                    CEffectMgr.Instance.CreateEffSync(pUnit.pAvatar.szEffJumpEnd, pUnit.tranSelf, 0, false);
                }
                if (pUnit.pAvatar != null)
                {
                    pUnit.pAvatar.PlayJumpEff(false);
                    pUnit.pAvatar.ShowHandObj(true);
                    pUnit.pAvatar.ShowJumpObj(false);
                }
                pUnit.PlayAnime(CUnitAnimeConst.Anime_Fishing);
            }
            else
            {
                pUnit.tranSelf.position = CHelpTools.GetCurvePoint(vStartPos, vCenterPos, vEndPos, 1f - pJumpTime.GetTimeLerp());
            }
        }
    }

}
