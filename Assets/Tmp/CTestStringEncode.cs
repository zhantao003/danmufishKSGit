using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTestStringEncode : MonoBehaviour
{
    public string szUrl;
    public string szEncodeUrl;

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
        if(GUILayout.Button("����"))
        {
            szEncodeUrl = System.Net.WebUtility.UrlEncode(szUrl);
        }

        if (GUILayout.Button("����"))
        {
            Debug.Log("�����" + System.Net.WebUtility.UrlDecode(szEncodeUrl));
        }
    }
}
