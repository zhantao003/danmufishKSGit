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
        uiLabelTime.text = "�رյ���ʱ��" + Mathf.RoundToInt(pTicker.CurValue).ToString() + "��";

        if (pTicker.Tick(dt))
        {
            Application.Quit();
            CloseSelf();
        }
    }
}
