using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameAdvBoard : MonoBehaviour
{
    public RawImage[] arrImg;

    public void OnClickShowChoiceRole()
    {
        UIManager.Instance.OpenUI(UIResType.GetRole);
    }
}
