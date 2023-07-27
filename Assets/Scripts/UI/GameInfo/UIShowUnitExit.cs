using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShowUnitExit : MonoBehaviour
{
    public Transform tranSelf;
    public GameObject objSelf;
    public CPlayerUnit pTargetUnit;

    public void SetActive(bool bActive)
    {
        if (objSelf == null) return;
        objSelf.SetActive(bActive);
    }

    public void OnClickExit()
    {
        if(pTargetUnit != null)
        {
            pTargetUnit.SendExitDM();
        }
        SetActive(false);
    }

    public void OnClickCopy()
    {
        if (pTargetUnit != null)
        {
            //���ֱ�Ӹ��Ƴɹ�
            CPlayerBaseInfo baseInfo = CPlayerMgr.Ins.GetPlayer(pTargetUnit.uid);
            if (baseInfo != null)
            {
                GUIUtility.systemCopyBuffer = baseInfo.userName + " " + pTargetUnit.uid;
            }
        }
        SetActive(false);
    }


}
