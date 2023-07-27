using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMUnitBorn : FSMUnitBase
{
    CPropertyTimer pJumpTime = new CPropertyTimer();

    Vector3 vStartPos;
    Vector3 vEndPos;

    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CPlayerUnit.EMState.Born;

        pUnit.PlayAnime(CUnitAnimeConst.Anime_Idle);

        vStartPos = pUnit.tranSelf.localPosition + Vector3.up * 3.6f;
        vEndPos = pUnit.tranSelf.localPosition;
        pUnit.tranSelf.localPosition = vStartPos;

        pJumpTime.Value = (vEndPos - pUnit.tranSelf.localPosition).magnitude / 6.4f;
        pJumpTime.FillTime();
    }

    public override void OnFixedUpdate(object obj, float delta)
    {
        if (pJumpTime.Tick(delta))
        {
            //CEffectMgr.Instance.CreateEffSync(pUnit.szBornEff, pUnit.tranSelf.position, Quaternion.identity, 0);

            pUnit.tranSelf.localPosition = vEndPos;
            if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle &&
                CBattleModeMgr.Ins != null &&
                CBattleModeMgr.Ins.emCurState == CBattleModeMgr.EMGameState.Gaming)
            {
                pUnit.SetState(CPlayerUnit.EMState.StartFish);
            }
            else
            {
                pUnit.SetState(CPlayerUnit.EMState.Idle);
            }
        }
        else
        {
            pUnit.tranSelf.localPosition = Vector3.Lerp(vStartPos, vEndPos, 1f - pJumpTime.GetTimeLerp());
        }
    }
}
