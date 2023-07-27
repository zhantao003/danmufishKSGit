using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMUnitBossReturnShow : FSMUnitBase
{
    Vector3 vStartPos;
    Vector3 vCenterPos;
    Vector3 vEndPos;
    CPropertyTimer pTimeTicker = new CPropertyTimer();

    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CPlayerUnit.EMState.BossReturn;
        pUnit.PlayAnime(CUnitAnimeConst.Anime_Jump);

        //计算抛物线
        vStartPos = pUnit.tranSelf.localPosition;
        vEndPos = Vector3.zero;
        vCenterPos = (vEndPos - vStartPos) * 0.5F;
        vCenterPos.y = vEndPos.y + +6.4F;
        pUnit.tranSelf.forward *= -1F;

        //填充跳跃时间
        pTimeTicker.Value = 1.5F;
        pTimeTicker.FillTime();

        if (pUnit.pAvatar != null)
        {
            pUnit.pAvatar.PlaySpringMgr(true);
            pUnit.pAvatar.PlayJumpEff(true);
            pUnit.pAvatar.ShowHandObj(false);
            pUnit.pAvatar.ShowJumpObj(true);
        }
    }

    public override void OnEnd(object obj)
    {
        if (pUnit.pAvatar != null)
        {
            //pUnit.pAvatar.PlaySpringMgr(true);
            pUnit.pAvatar.PlayJumpEff(false);
            pUnit.pAvatar.ShowHandObj(true);
            pUnit.pAvatar.ShowJumpObj(false);
        }
    }

    public override void OnFixedUpdate(object obj, float delta)
    {
        if (pTimeTicker.Tick(delta))
        {
            pUnit.tranSelf.localPosition = vEndPos;
            pUnit.tranSelf.localRotation = Quaternion.identity;

            //检查是否有特殊的跳跃特效
            if (pUnit.pAvatar != null && !CHelpTools.IsStringEmptyOrNone(pUnit.pAvatar.szEffJumpEnd))
            {
                CEffectMgr.Instance.CreateEffSync(pUnit.pAvatar.szEffJumpEnd, pUnit.tranSelf, 0, false);
            }

            pUnit.SetState(CPlayerUnit.EMState.StartFish);
        }
        else
        {
            pUnit.tranSelf.localPosition = CHelpTools.GetCurvePoint(vStartPos, vCenterPos, vEndPos, 1f - pTimeTicker.GetTimeLerp());
            //pUnit.tranSelf.forward = vDir;
        }
    }
}
