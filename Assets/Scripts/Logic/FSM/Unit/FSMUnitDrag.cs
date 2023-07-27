using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMUnitDrag : FSMUnitBase
{
    public override void OnBegin(object obj)
    {
        //Debug.Log("Idle");
        pUnit.emCurState = CPlayerUnit.EMState.Drag;

        pUnit.PlayAnime(CUnitAnimeConst.Anime_Idle);
    }
}

