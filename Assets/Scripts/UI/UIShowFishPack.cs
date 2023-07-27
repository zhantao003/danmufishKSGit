using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShowFishPack : UIBase
{
    public CPropertyTimer pTickerShow = new CPropertyTimer();

    public override void OnOpen()
    {
        pTickerShow.FillTime();
    }

    protected override void OnUpdate(float dt)
    {
        if(pTickerShow.Tick(dt))
        {
            CloseSelf();
        }
    }
}
