using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGMFishUserLvConfigSlot : MonoBehaviour
{
    public Text uiLabelLevel;
    public InputField uiInputExp;

    public void SetInfo(CGMFishUserLvConfig info)
    {
        uiLabelLevel.text = info.lv.ToString();
        uiInputExp.text = info.exp.ToString(); 
    }

    public CLocalNetMsg ToJsonMsg()
    {
        CLocalNetMsg msgRes = new CLocalNetMsg();
        msgRes.SetLong("lv", long.Parse(uiLabelLevel.text));
        msgRes.SetLong("exp", long.Parse(uiInputExp.text));

        return msgRes;
    }
}
