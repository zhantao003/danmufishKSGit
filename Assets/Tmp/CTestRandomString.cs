using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTestRandomString : MonoBehaviour
{
    private void OnGUI()
    {
        if(GUILayout.Button("生成Key"))
        {
            string szRes = CHelpTools.GetRandomString(16, true, true, true, false, "");

            Debug.Log(szRes);
        }

        if (GUILayout.Button("生成iv"))
        {
            string szRes = CHelpTools.GetRandomString(16, false, true, true, false, "");

            Debug.Log(szRes);
        }

        if (GUILayout.Button("随机数"))
        {
            int nRes = Random.Range(0, 10000);
            Debug.Log(nRes);
        }
    }
}
