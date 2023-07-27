using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMNPCBase : FSMBaseState
{
    public CNPCUnit pNPCUnit;

    public override void OnReady(object obj)
    {
        pNPCUnit = obj as CNPCUnit;
    }
}

