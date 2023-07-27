using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMUnitIdle : FSMUnitBase
{
    public override void OnBegin(object obj)
    {
        //Debug.Log("Idle");
        pUnit.emCurState = CPlayerUnit.EMState.Idle;

        pUnit.PlayAnime(CUnitAnimeConst.Anime_Idle);
    }
}
