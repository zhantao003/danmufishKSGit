using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMNPCIdle : FSMNPCBase
{
    public override void OnBegin(object obj)
    {
        pNPCUnit.emCurState = CNPCUnit.EMState.Idle;
        pNPCUnit.PlayAnime(CUnitAnimeConst.Anime_Idle);
    }
}
