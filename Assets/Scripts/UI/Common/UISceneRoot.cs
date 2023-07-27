using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISceneRoot : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(UIManager.Instance.GetUI(UIResType.Setting) != null &&
               !UIManager.Instance.GetUI(UIResType.Setting).IsOpen())
            {
                UIManager.Instance.OpenUI(UIResType.Setting);
            }
        }
    }
}
