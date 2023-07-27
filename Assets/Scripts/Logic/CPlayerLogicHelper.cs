using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerLogicHelper
{
    public static void CheckExit(string uid)
    {
        //�����Bossս��ʼ�����ϰ�
        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
        {
            if (CGameBossMgr.Ins != null &&
               CGameBossMgr.Ins.emCurState == CGameBossMgr.EMState.Gaming)
            {
                return;
            }
        }
        else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive)
        {
            //����ģʽ��Ϸ��ʼ�����ϰ�
            //����ģʽ���뷿������ϰ�
            if (CGameSurviveMap.Ins.emCurState != CGameSurviveMap.EMGameState.Ready)
                return;
        }

        CPlayerUnit playerUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if (playerUnit != null &&
            (playerUnit.emCurState == CPlayerUnit.EMState.Battle ||
            playerUnit.emCurState == CPlayerUnit.EMState.HitDrop ||
            playerUnit.listNormalBooms.Count > 0 ||
            playerUnit.listTreasureBooms.Count > 0))
            return;

        ///�������������޷��ϰ�
        if (CHelpTools.CheckAuctionByUID(uid))
            return;

        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive)
        {
            CGameSurviveMap.Ins.RemovePlayerInRoom(uid);
        }

        //�����ǲ�������Ϸ����
        CPlayerBaseInfo pActivePlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pActivePlayer != null)
        {
            CPlayerMgr.Ins.RemoveActivePlayer(pActivePlayer);
            UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
            if (roomInfo != null)
            {
                roomInfo.battleRoot.RemovePlayerInfo(uid);
            }

            if (pActivePlayer.emUserType == CPlayerBaseInfo.EMUserType.Guanzhong)
            {
                CPlayerMgr.Ins.RemovePlayer(uid);
            }

            if (CControlerSlotMgr.Ins != null)
            {
                CControlerSlotMgr.Ins.RecycleSlot(uid);
            }

            return;
        }

        //�����Ƿ��ڵȴ�������
        CPlayerBaseInfo pIdlePlayer = CPlayerMgr.Ins.GetIdlePlayer(uid);
        if (pIdlePlayer != null)
        {
            CPlayerMgr.Ins.RemoveIdleUnit(uid);
            CPlayerMgr.Ins.RemoveIdlePlayer(pIdlePlayer);

            UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
            if (roomInfo != null)
            {
                roomInfo.battleRoot.RemovePlayerInfo(uid);
            }
            if (pActivePlayer.emUserType == CPlayerBaseInfo.EMUserType.Guanzhong)
            {
                CPlayerMgr.Ins.RemovePlayer(uid);
            }
            if (CControlerSlotMgr.Ins != null)
            {
                CControlerSlotMgr.Ins.RecycleSlot(uid);
            }
        }
    }

}
