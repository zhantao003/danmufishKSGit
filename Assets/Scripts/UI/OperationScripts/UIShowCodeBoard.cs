using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShowCodeBoard : UIBase
{
    public GameObject objSelf;

    public void SetActive(bool bActive)
    {
        objSelf.SetActive(bActive);
    }

}
