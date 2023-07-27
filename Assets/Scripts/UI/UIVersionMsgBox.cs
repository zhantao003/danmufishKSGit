using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVersionMsgBox : UIBase
{
    public CPropertyTimer pTicker = new CPropertyTimer();
    public Text uiLabelTime;

    public override void OnOpen()
    {
        pTicker.FillTime();
    }

    protected override void OnUpdate(float dt)
    {
        uiLabelTime.text = "πÿ±’µπº∆ ±£∫" + Mathf.RoundToInt(pTicker.CurValue).ToString() + "√Î";

        if (pTicker.Tick(dt))
        {
            Application.Quit();
            CloseSelf();
        }
    }
}
