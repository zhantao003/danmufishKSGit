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
        if(GUILayout.Button("±àÂë"))
        {
            szEncodeUrl = System.Net.WebUtility.UrlEncode(szUrl);
        }

        if (GUILayout.Button("½âÂë"))
        {
            Debug.Log("½âÂëºó£º" + System.Net.WebUtility.UrlDecode(szEncodeUrl));
        }
    }
}
