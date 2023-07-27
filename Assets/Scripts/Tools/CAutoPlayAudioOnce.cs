using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAutoPlayAudioOnce : MonoBehaviour
{
    public CAudioMgr.CAudioSlottInfo pAudioInfo;

    void Start()
    {
        if(CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.SPECIALSOUND) > 0)
        {
            CAudioMgr.Ins.PlaySoundBySlot(pAudioInfo, Vector3.zero);
        }
    }
}
