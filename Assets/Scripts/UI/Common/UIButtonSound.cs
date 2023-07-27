using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonSound : MonoBehaviour
{
    public CAudioMgr.CAudioSlottInfo pAudioClick;

    public void OnClickEvent()
    {
        CAudioMgr.Ins.PlaySoundBySlot(pAudioClick, Vector3.zero);
    }
}
