using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[CDanmuCmdAttrite(CDanmuEventConst.SaQian)]
public class DCmdSendCoin : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        string uid = dm.uid.ToString();

        string szContent = dm.content.Trim();

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null || pPlayer.pBoatPack == null) return;

        //撒钱白名单
        bool bSaQianAble = false;
        //if (uid == 38367534 ||      //炫彩哥
        //    uid == 1308309723 ||    //提督:130HANG130
        //    uid == 1197309192)      //提督:亓九九那儿的甲方
        //{
        //    bSaQianAble = true;
        //}

        if(pPlayer.nVipPlayer >= CPlayerBaseInfo.EMVipLv.Tidu)
        {
            bSaQianAble = true;
        }

        if (!bSaQianAble) return;

        CPlayerUnit pPlayerUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if (pPlayerUnit == null) return;

        SendCoin(szContent, pPlayer);
    }

    void SendCoin(string content, CPlayerBaseInfo player)
    {
        try
        {
            int nLen = content.Length;
            int nCmdIdx = content.IndexOf(CDanmuEventConst.SaQian) + CDanmuEventConst.SaQian.Length;
            content = content.Substring(nCmdIdx, nLen - nCmdIdx).Trim();

            long nCostCoin = 0;
            if (!long.TryParse(content, out nCostCoin))
            {
                Debug.Log("异常撒钱额度：" + content);
                return;
            }

            if(player.GameCoins < nCostCoin)
            {
                UIToast.Show("不够鱼币撒哦");
                return;
            }

            if(nCostCoin < 1000)
            {
                UIToast.Show("撒得太少咯~");
                Debug.Log("不能低于1000");
                return;
            }

            long nMaxSendCoin = 50000;
            if(player.nVipPlayer == CPlayerBaseInfo.EMVipLv.Zongdu)
            {
                nMaxSendCoin = 200000;
            }

            if(nCostCoin > nMaxSendCoin)
            {
                UIToast.Show("撒得太多咯~");
                Debug.Log("不能高于200000");
                return;
            }

            //玩家先扣钱
            bool bVtb = (player.emUserType == CPlayerBaseInfo.EMUserType.Zhubo);

            CPlayerNetHelper.AddFishCoin(player.uid,
                                         -nCostCoin,
                                         EMFishCoinAddFunc.Duel,
                                         false,
                                         new HHandlerSendCoin(nCostCoin));
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}
