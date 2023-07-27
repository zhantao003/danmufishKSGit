using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMNPCBorn : FSMNPCBase
{
    CPropertyTimer pJumpTime = new CPropertyTimer();

    Vector3 vStartPos;
    Vector3 vEndPos;

    public override void OnBegin(object obj)
    {
        pNPCUnit.emCurState = CNPCUnit.EMState.Born;

        pNPCUnit.PlayAnime(CUnitAnimeConst.Anime_Idle);

        if (pNPCUnit.tranStartPos == null)
        {
            vStartPos = pNPCUnit.transform.position + Vector3.up * 3.6f;
        }
        else
        {
            vStartPos = pNPCUnit.tranStartPos.position + Vector3.up * 3.6f;
        }
        vEndPos = pNPCUnit.tranSelf.position;
        pNPCUnit.tranSelf.position = vStartPos;

        pJumpTime.Value = (vEndPos - pNPCUnit.tranSelf.position).magnitude / 6.4f;
        pJumpTime.FillTime();
    }

    public override void OnFixedUpdate(object obj, float delta)
    {
        if (pJumpTime.Tick(delta))
        {
            pNPCUnit.tranSelf.position = vEndPos;
            pNPCUnit.SetState(pNPCUnit.emBornChgState);
        }
        else
        {
            pNPCUnit.tranSelf.position = Vector3.Lerp(vStartPos, vEndPos, 1f - pJumpTime.GetTimeLerp());
        }
    }
}
