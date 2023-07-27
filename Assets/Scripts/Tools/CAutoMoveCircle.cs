using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAutoMoveCircle : MonoBehaviour
{
    public float fSpd;
    public float fRadius;
    public float fHeight;
    public Transform tranSelf;
    public Transform tranCenter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        tranSelf.RotateAround(tranCenter.position, Vector3.up, fSpd * CTimeMgr.FixedDeltaTime);
    }
}
