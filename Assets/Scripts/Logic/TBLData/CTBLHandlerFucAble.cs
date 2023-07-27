using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMFuncAbleType
{
    TodaySpecial = 1,               //�ز�
    InfoTip,                        //ָ��
    ShowInfo,                       //��Ѵ
    TopBtn,                         //���Ͻǰ�ť
    Auction,                        //����
    BattryTarget,                   //���Ŀ��
    Duel,                           //����
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
