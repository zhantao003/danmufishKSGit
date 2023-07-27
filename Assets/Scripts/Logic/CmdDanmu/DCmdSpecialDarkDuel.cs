using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CDanmuCmdAttrite(CDanmuEventConst.SpecialDarkDuel)]
public class DCmdSpecialDarkDuel : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        return;
        ///�ж��Ƿ����˾���
        if ((CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal ||
             CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema) &&
            CTBLHandlerFucAble.Ins.GetInfo((int)EMFuncAbleType.Duel).CheckSceneLv())
        {
            UIToast.Show((CTBLHandlerFucAble.Ins.GetInfo((int)EMFuncAbleType.Duel).nSecenLv + 1) + "���泡���ź�������");
            return;
        }

        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Battle ||
            CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss ||
            CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive)
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
        if (pUnit.bCheckYZM ||
            pUnit.pInfo.CheckIsGrayName())
        {
            return;
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
            int nCmdIdx = content.IndexOf(CDanmuEventConst.SpecialDarkDuel) + CDanmuEventConst.SpecialDarkDuel.Length;
            szContent = content.Substring(nCmdIdx, nLen - nCmdIdx).Trim();
            if (CHelpTools.IsStringEmptyOrNone(szContent))
            {
                Debug.Log("�쳣������Ļ:" + content);
                return;
            }
            long nlTargetPrice = 0;
            if (!long.TryParse(szContent, out nlTargetPrice))
            {
                Debug.Log("�쳣�ĺ����۸�" + szContent);
                return;
            }
            if (nlTargetPrice < CGameColorFishMgr.Ins.pStaticConfig.GetInt("������Ʊ��С�۸�"))
            {
                UIToast.Show("������Ʊ�۸����");
                return;
            }

            //�����������ޣ�������
            bool bDuelCoinMaxTupo = false;
            if (player.pInfo.nVipPlayer >= CPlayerBaseInfo.EMVipLv.Tidu)
            {
                bDuelCoinMaxTupo = true;
            }

            //if (player.uid == 38367534 ||      //�Ųʸ�
            //    player.uid == 1308309723 ||    //�ᶽ:130HANG130
            //    player.uid == 1197309192)      //�ᶽ:���ž��Ƕ��ļ׷�
            //{
            //    bDuelCoinMaxTupo = true;
            //}

            if (bDuelCoinMaxTupo)
            {
                if (nlTargetPrice > CGameColorFishMgr.Ins.pStaticConfig.GetInt("������Ʊ���۸�2"))
                {
                    UIToast.Show("������Ʊ�۸����");
                    return;
                }
            }
            else
            {
                if (nlTargetPrice > CGameColorFishMgr.Ins.pStaticConfig.GetInt("������Ʊ���۸�"))
                {
                    UIToast.Show("������Ʊ�۸����");
                    return;
                }
            }

            CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(player.uid);
            if (nlTargetPrice > pPlayer.TreasurePoint)
            {
                UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
                if (uiGameInfo == null || !uiGameInfo.IsOpen()) return;
                UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(player.uid);
                if (uiUnitInfo == null) return;
                uiUnitInfo.SetDmContent("������Ҳ���");
                return;
            }

            UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
            if (roomInfo != null &&
                roomInfo.emCurShowType != ShowBoardType.SpecialBattle &&
                roomInfo.emCurShowType != ShowBoardType.Battle &&
                roomInfo.emCurShowType != ShowBoardType.Gifts)
            {
                bool bVtb = player.pInfo.emUserType == CPlayerBaseInfo.EMUserType.Zhubo;
                //ͬ�����ʹ�õ��ߵ�����
                //���ػ���
                CPlayerNetHelper.AddTreasureCoin(player.uid, -nlTargetPrice, new HHandlerCreatSpecialDuel(nlTargetPrice));

                roomInfo.battleRoot.nlCurPrice = nlTargetPrice;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

}
