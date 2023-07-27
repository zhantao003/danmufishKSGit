using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMUnitBossEatShow : FSMUnitBase
{
    Vector3 vStartPos;
    Vector3 vCenterPos;
    Vector3 vEndPos;
    CPropertyTimer pTimeTicker = new CPropertyTimer();

    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CPlayerUnit.EMState.BossEat;
        pUnit.PlayAnime(CUnitAnimeConst.Anime_Jump);

        //计算抛物线
        vStartPos = pUnit.tranSelf.localPosition;
        vEndPos = pUnit.tranSelf.localPosition + Vector3.forward * 4F + Vector3.down * 12F;
        vCenterPos = (vEndPos - vStartPos) * 0.5F;
        vCenterPos.y = vStartPos.y + 6.4F;

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
            pUnit.pAvatar.PlaySpringMgr(false);
            pUnit.pAvatar.PlayJumpEff(false);
            pUnit.pAvatar.ShowHandObj(false);
            pUnit.pAvatar.ShowJumpObj(false);
        }
    }

    public override void OnFixedUpdate(object obj, float delta)
    {
        if (pTimeTicker.Tick(delta))
        {
            pUnit.tranSelf.localPosition = vEndPos;
            pUnit.SetState(CPlayerUnit.EMState.BossWaitShow);
        }
        else
        {
            pUnit.tranSelf.localPosition = CHelpTools.GetCurvePoint(vStartPos, vCenterPos, vEndPos, 1f - pTimeTicker.GetTimeLerp());
            //pUnit.tranSelf.forward = vDir;
        }
    }
}
