using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpecialCastSlot : MonoBehaviour
{
    public int nIdx;
    public Text uiTextContent;
    public Image uiShowImg;

    public void SetIdx(int i)
    {
        nIdx = i;

        uiTextContent.text = i.ToString();
    }
}
