using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTestTimeStamp : MonoBehaviour
{
    public long lTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        if(GUILayout.Button("��ȡʱ��"))
        {
            lTime = CHelpTools.GetTimeStampMilliseconds(DateTime.Now);
        }

        if(GUILayout.Button("ת��ʱ��"))
        {
            DateTime pTimeDat = CHelpTools.GetDateTimeMilliseconds(lTime);
            Debug.Log(pTimeDat.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
