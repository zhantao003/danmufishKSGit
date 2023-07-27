using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBroadCastSlot : MonoBehaviour
{
    public int nIdx;
    public Text uiTextContent;

    public void SetIdx(int i)
    {
        nIdx = i;

        uiTextContent.text = i.ToString();
    }
}
