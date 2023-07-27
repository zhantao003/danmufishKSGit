using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDuelBoatAudioMgr : MonoBehaviour
{
    public CAudioMgr.CAudioSlottInfo pSmallPlayAudio;

    public CAudioMgr.CAudioSlottInfo pBigPlayAudio;
    
    public void PlaySmallAudio()
    {
        if (pSmallPlayAudio != null)
        {
            CAudioMgr.Ins.PlaySoundBySlot(pSmallPlayAudio, transform.position);
        }
    }

    public void PlayBigAudio()
    {
        if (pBigPlayAudio != null)
        {
            CAudioMgr.Ins.PlaySoundBySlot(pBigPlayAudio, transform.position);
        }
    }

}
