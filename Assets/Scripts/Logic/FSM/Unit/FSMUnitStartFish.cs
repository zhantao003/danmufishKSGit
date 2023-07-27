using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMUnitStartFish : FSMUnitBase
{
    CPropertyTimer pCheckTime = new CPropertyTimer();

    public override void OnBegin(object obj)
    {
        //Debug.Log("Start Fishing");
        pUnit.PlayStartFishAudio();
        pUnit.emCurState = CPlayerUnit.EMState.StartFish;

        pUnit.PlayAnime(CUnitAnimeConst.Anime_StartFish);

        pCheckTime.Value = pUnit.fFinishStartTime;
        pCheckTime.FillTime();
    }

    public override void OnFixedUpdate(object obj, float delta)
    {
        if (pCheckTime.Tick(delta))
        {
            pUnit.SetState(CPlayerUnit.EMState.Fishing);
        }
    }
}

