using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFSMBossBase : FSMBaseState
{
    public CBossBase pUnit;

    public override void OnReady(object obj)
    {
        pUnit = obj as CBossBase;
    }
}
