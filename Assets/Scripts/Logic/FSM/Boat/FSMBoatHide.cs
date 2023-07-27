using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMBoatHide : FSMBoatBase
{
    public override void OnBegin(object obj)
    {
        pBoat.emCurState = CDuelBoat.EMState.Hide;
        pBoat.pAnima.Play("Hide");
    }

}
