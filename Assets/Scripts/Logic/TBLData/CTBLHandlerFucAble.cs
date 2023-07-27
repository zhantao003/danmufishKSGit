using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMFuncAbleType
{
    TodaySpecial = 1,               //特产
    InfoTip,                        //指令
    ShowInfo,                       //鱼汛
    TopBtn,                         //右上角按钮
    Auction,                        //拍卖
    BattryTarget,                   //电池目标
    Duel,                           //决斗
}

public class ST_FucAble : CTBLConfigSlot
{
    public int nSecenLv;

    public override void InitByLoader(CTBLLoader loader)
    {
        nID = loader.GetIntByName("id");
        nSecenLv = loader.GetIntByName("scenelv");
    }

    public bool CheckSceneLv()
    {
        bool bInSceneLv = false;
        if (CGameColorFishMgr.Ins.nCurRateUpLv <= nSecenLv)
        {
            bInSceneLv = true;
        }

        return bInSceneLv;
    }

}

[CTBLConfigAttri("FucAble")]
public class CTBLHandlerFucAble : CTBLConfigBaseWithDic<ST_FucAble>
{

}
