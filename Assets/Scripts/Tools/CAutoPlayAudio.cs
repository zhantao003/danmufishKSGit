using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAutoPlayAudio : MonoBehaviour
{
    public CAudioMgr.CAudioSlottInfo pAudioInfo;
    public float fMaxTime;
    public float fCurTime;
    float fLerp;
    public bool bAudioSpecCtrl = false;

    // Start is called before the first frame update
    void Start()
    {
        fCurTime = fMaxTime;
        PlaySound();
    }

    // Update is called once per frame
    void Update()
    {
        fLerp = fCurTime - CTimeMgr.DeltaTime;
        if(fLerp <= 0F)
        {
            fCurTime = fMaxTime;
            fCurTime += fLerp;

            PlaySound();
        }
        else
        {
            fCurTime = fLerp;
        }
    }

    void PlaySound()
    {
        if (bAudioSpecCtrl)
        {
            if (CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.SPECIALSOUND) > 0)
                CAudioMgr.Ins.PlaySoundBySlot(pAudioInfo, Vector3.zero);
        }
        else
        {
            CAudioMgr.Ins.PlaySoundBySlot(pAudioInfo, Vector3.zero);
        }
    }
}
