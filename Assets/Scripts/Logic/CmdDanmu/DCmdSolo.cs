using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CDanmuCmdAttrite(CDanmuEventConst.Solo)]
public class DCmdSolo : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        return;
        ///�ж��Ƿ����˾���
        if ((CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal ||
             CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema) &&
            CTBLHandlerFucAble.Ins.GetInfo((int)EMFuncAbleType.Duel).CheckSceneLv())
        {
            UIToast.Show((CTBLHandlerFucAble.Ins.GetInfo((int)EMFuncAbleType.Duel).nSecenLv + 1) + "���泡���ž�������");
            return;
        }

        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Battle ||
            CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
        {
            return;
        }

        string uid = dm.uid.ToString();
        string szContent = dm.content;
        ///����uid��ȡ��ҵ�λ
        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if (pUnit == null)
        {
            pUnit = CPlayerMgr.Ins.GetActiveUnit(uid);
            if (pUnit == null)
                return;
        }
        if (CGameColorFishMgr.Ins.emCurGameType != CGameColorFishMgr.EMGameType.Battle &&
            CGameColorFishMgr.Ins.emCurGameType != CGameColorFishMgr.EMGameType.Boss)
        {
            if (pUnit.bCheckYZM ||
            pUnit.pInfo.CheckIsGrayName())
            {
                return;
            }
        }
        if (pUnit.bDuelCD)
        {
            UIToast.Show("����CD��");
            return;
        }

        if (pUnit.emCurState == CPlayerUnit.EMState.Jump ||
            pUnit.emCurState == CPlayerUnit.EMState.HitDrop)
        {
            return;
        }

        Duel(szContent, pUnit);
    }


    public void Duel(string content, CPlayerUnit player)
    {
        string szContent = content.Trim();
        try
        {
            int nLen = content.Length;
            int nCmdIdx = content.IndexOf(CDanmuEventConst.Solo) + CDanmuEventConst.Solo.Length;
            szContent = content.Substring(nCmdIdx, nLen - nCmdIdx).Trim();
            if (CHelpTools.IsStringEmptyOrNone(szContent))
            {
                Debug.Log("�쳣������Ļ:" + content);
                return;
            }
            long nlTargetPrice = 0;
            if (!long.TryParse(szContent, out nlTargetPrice))
            {
                Debug.Log("�쳣�ľ����۸�" + szContent);
                return;
            }
            if (nlTargetPrice < CGameColorFishMgr.Ins.pStaticConfig.GetInt("������Ʊ��С�۸�"))
            {
                UIToast.Show("������Ʊ�۸����");
                return;
            }
            if (nlTargetPrice > CGameColorFishMgr.Ins.pStaticConfig.GetInt("������Ʊ���۸�"))
            {
                UIToast.Show("������Ʊ�۸����");
                return;
            }
            //CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(player.uid);
            //if (pPlayer.GameCoins < nlTargetPrice)
            //{
            //    UIToast.Show(pPlayer.userName + "�Ļ��ֲ���");
            //    //return;
            //}

            UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
            if (roomInfo != null && roomInfo.emCurShowType != ShowBoardType.Battle)
            {
                bool bVtb = player.pInfo.emUserType == CPlayerBaseInfo.EMUserType.Zhubo;
                //ͬ�����ʹ�õ��ߵ�����
                CPlayerNetHelper.AddFishCoin(player.uid,
                                             nlTargetPrice * -1,
                                             EMFishCoinAddFunc.Duel,
                                             false,
                                             new HHandlerCreatSolo());

                roomInfo.battleRoot.nlCurPrice = nlTargetPrice;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

}
