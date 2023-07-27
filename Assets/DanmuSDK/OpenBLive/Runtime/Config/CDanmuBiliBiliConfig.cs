using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[UnityEngine.CreateAssetMenu(fileName = "DanmuConfig", menuName = "Bilibili/DanmuConfig")]
public class CDanmuBiliBiliConfig : ScriptableObject
{
    public string accessKeySecret;
    public string accessKeyId;
    public string devUid;
    public string roomId;
    public string appId;
    public string code;
}
