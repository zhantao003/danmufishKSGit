using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAvatarAnimeEvent : MonoBehaviour
{
    public UITweenBase[] arrTween;

    public CAudioMgr.CAudioSlottInfo pAudio;

    public void PlayTween()
    {
        for(int i=0; i<arrTween.Length; i++)
        {
            arrTween[i].gameObject.SetActive(true);
            arrTween[i].Play();
        }
    }

    public void PlaySound()
    {
        if (CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.SPECIALSOUND) > 0)
            CAudioMgr.Ins.PlaySoundBySlot(pAudio, transform.position);
    }
}
