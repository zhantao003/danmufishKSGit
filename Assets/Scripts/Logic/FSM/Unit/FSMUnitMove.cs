using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMUnitMove : FSMUnitBase
{
    CPropertyTimer pMoveTick = new CPropertyTimer();

    Vector3 vStartPos;
    Vector3 vEndPos;

    public override void OnBegin(object obj)
    {
        Debug.Log("Move");
        pUnit.emCurState = CPlayerUnit.EMState.Move;

        //pUnit.PlayAnime(CUnitAnimeConst.Anime_Jump);

        vStartPos = pUnit.tranSelf.position;
        vEndPos = pUnit.vecMoveTarget;
        pUnit.tranSelf.position = vStartPos;

        pUnit.tranSelf.forward = (vEndPos - vStartPos).normalized;

        pMoveTick.Value = Vector3.Distance(vStartPos,vEndPos) / pUnit.fJumpSpeed;
        pMoveTick.FillTime();
    }

    public override void OnFixedUpdate(object obj, float delta)
    {
        if (pMoveTick.Tick(delta))
        {
            //CEffectMgr.Instance.CreateEffSync(pUnit.szBornEff, pUnit.tranSelf.position, Quaternion.identity, 0);

            pUnit.tranSelf.position = vEndPos;
            pUnit.SetState(pUnit.pEndMoveState);
        }
        else
        {
            pUnit.tranSelf.position = Vector3.Lerp(vStartPos, vEndPos, 1f - pMoveTick.GetTimeLerp());
        }
    }
}
