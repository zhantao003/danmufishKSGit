using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTestHttpReq : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CNetConfigMgr.Ins.Init();
        CHttpMgr.Instance.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGUI()
    {
        if(GUILayout.Button("Test"))
        {
            CHttpParam pReqParams = new CHttpParam(new CHttpParamSlot("roomId", "1234567890"));
            CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugTestSDK, pReqParams);
        }
    }
}
