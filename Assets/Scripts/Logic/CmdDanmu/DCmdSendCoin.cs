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

        //��Ǯ������
        bool bSaQianAble = false;
        //if (uid == 38367534 ||      //�Ųʸ�
        //    uid == 1308309723 ||    //�ᶽ:130HANG130
        //    uid == 1197309192)      //�ᶽ:���ž��Ƕ��ļ׷�
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
                Debug.Log("�쳣��Ǯ��ȣ�" + content);
                return;
            }

            if(player.GameCoins < nCostCoin)
            {
                UIToast.Show("���������Ŷ");
                return;
            }

            if(nCostCoin < 1000)
            {
                UIToast.Show("����̫�ٿ�~");
                Debug.Log("���ܵ���1000");
                return;
            }

            long nMaxSendCoin = 50000;
            if(player.nVipPlayer == CPlayerBaseInfo.EMVipLv.Zongdu)
            {
                nMaxSendCoin = 200000;
            }

            if(nCostCoin > nMaxSendCoin)
            {
                UIToast.Show("����̫�࿩~");
                Debug.Log("���ܸ���200000");
                return;
            }

            //����ȿ�Ǯ
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
