using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShowImg : UIBase
{
    public string szUrl;
    public void OnClickJump()
    {
        Application.OpenURL(szUrl);
    }
}
