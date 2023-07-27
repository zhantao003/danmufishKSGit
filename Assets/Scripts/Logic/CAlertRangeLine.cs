using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAlertRangeLine : MonoBehaviour
{
    public Transform tranSelf;
    public Transform tranSetLine;
    
    public void SetAlertRange(Transform start,Transform end)
    {
        Vector3 vDir = end.position - start.position;
        vDir.y = 0F;
        vDir = vDir.normalized;
        tranSelf.forward = vDir;
        float fDis = Vector3.Distance(start.position, end.position);
        tranSetLine.localScale = new Vector3(4f, fDis, 1f);
    }

}
