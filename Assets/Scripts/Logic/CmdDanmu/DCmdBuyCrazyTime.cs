using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CDanmuCmdAttrite(CDanmuEventConst.CrazyTime)]
public class DCmdBuyCrazyTime : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        return;
        if (CCrazyTimeMgr.Ins == null) return;

        string uid = dm.uid.ToString();
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null) return;

        if (pPlayer.nVipPlayer != CPlayerBaseInfo.EMVipLv.Zongdu && 
            pPlayer.nVipPlayer != CPlayerBaseInfo.EMVipLv.Pro) return;

        //�ж�Ǯ������
        if(pPlayer.GameCoins < CGameColorFishMgr.Ins.pStaticConfig.GetInt("�ܶ����ټ۸�"))
        {
            UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
            if (uiGameInfo != null && uiGameInfo.IsOpen())
            {
                UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(uid);
                if (uiUnitInfo != null)
                {
                    uiUnitInfo.SetDmContent("���ֲ�����������");
                }
            }

            return;
        }

        //ͬ�����ʹ�õ��ߵ�����
        CPlayerNetHelper.AddFishCoin(pPlayer.uid,                                     
                                     CGameColorFishMgr.Ins.pStaticConfig.GetInt("�ܶ����ټ۸�") * -1,
                                     EMFishCoinAddFunc.Pay,
                                     true,
                                     new HHandlerVipBuyCrazyTime());
    }
}
