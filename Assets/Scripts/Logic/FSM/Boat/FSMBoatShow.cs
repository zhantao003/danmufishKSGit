using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMBoatShow : FSMBoatBase
{
    CPropertyTimer pAnimaTick;

    public override void OnBegin(object obj)
    {
        pBoat.emCurState = CDuelBoat.EMState.Show;
        pBoat.pAnima.Play("Show");

        pAnimaTick = new CPropertyTimer();
        pAnimaTick.Value = pBoat.fMoveShowTime;
        pAnimaTick.FillTime();
    }

    public override void OnFixedUpdate(object obj, float delta)
    {
        if (pAnimaTick != null &&
            pAnimaTick.Tick(delta))
        {
            pAnimaTick = null;
            pBoat.SetState(CDuelBoat.EMState.Stay);
        }
    }

}
