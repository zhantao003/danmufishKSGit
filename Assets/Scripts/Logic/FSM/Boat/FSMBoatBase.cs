using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMBoatBase : FSMBaseState
{
    public CDuelBoat pBoat;

    public override void OnReady(object obj)
    {
        pBoat = obj as CDuelBoat;
    }
}

