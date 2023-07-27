using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAutoRonateZero : MonoBehaviour
{
    public Transform tranSelf;

    private void LateUpdate()
    {
        tranSelf.rotation = Quaternion.identity;
    }
}
