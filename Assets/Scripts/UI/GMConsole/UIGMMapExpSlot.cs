using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGMMapExpSlot : MonoBehaviour
{
    public Text uiLev;
    public InputField uiInputExp;


    public void InitInfo(CMapExpInfo info)
    {
        uiLev.text = info.nLv.ToString();
        uiInputExp.text = info.nExp.ToString();
    }

    public CLocalNetMsg GetMsg()
    {
        CLocalNetMsg msg = new CLocalNetMsg();
        long nLev = int.Parse(uiLev.text);
        long nExp = int.Parse(uiInputExp.text);
        msg.SetLong("level", nLev);
        msg.SetLong("exp", nExp);
        return msg;
    }
}
