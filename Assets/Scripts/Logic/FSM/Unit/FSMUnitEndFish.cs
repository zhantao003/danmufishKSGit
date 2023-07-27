using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMUnitEndFish : FSMUnitBase
{
    CPropertyTimer pCheckTime;

    public override void OnBegin(object obj)
    {
        pUnit.SetLaGanState(false);
        pUnit.PlayEndFishAudio();
        //Debug.Log("End Fishing=====" + pUnit.fCurFinishValue + "=====" + pUnit.GetCurFinishValue());
        pUnit.emCurState = CPlayerUnit.EMState.EndFish; 

        pUnit.PlayAnime(CUnitAnimeConst.Anime_EndFish);

        pCheckTime = new CPropertyTimer();
        pCheckTime.Value = pUnit.fFinishEndTime;
        pCheckTime.FillTime();
    }

    public override void OnFixedUpdate(object obj, float delta)
    {
        if (pCheckTime != null &&
            pCheckTime.Tick(delta))
        {
            pUnit.SetState(CPlayerUnit.EMState.ShowFish);
            pCheckTime = null;
        }
    }

    public override void OnEnd(object obj)
    {
        base.OnEnd(obj);
    }

}

