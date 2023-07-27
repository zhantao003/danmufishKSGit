using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HHandlerGetVtbFishInfo : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string szStatus = pMsg.GetString("status");
        if(szStatus.Equals("error2") ||
           szStatus.Equals("error"))
        {
            UIMsgBox.Show("", "�˺���Ϣ�쳣������ϵ�ٷ��ͷ�\r\nQQ�ţ�494594996", UIMsgBox.EMType.OK, null);
            UIManager.Instance.CloseUI(UIResType.NetWait);
            return;
        }

        string uid = pMsg.GetString("uid");
        long fishBaits = pMsg.GetLong("fishBait");
        long freeFishBait = pMsg.GetLong("freeFishBait");
        //long fishGanCount = pMsg.GetLong("fishGan");
        //long fishPiaoCount = pMsg.GetLong("fishPiao");
        //long fishXianCount = pMsg.GetLong("fishXian");
        long fishLunCount = pMsg.GetLong("fishLun");
        long vipPlayer = pMsg.GetLong("vipPlayer");
        long treasurePoint = pMsg.GetLong("treasurePoint");
        //long fishLv = pMsg.GetLong("fishLv");
        //long fishExp = pMsg.GetLong("fishExp");
        //long gachaCount = pMsg.GetLong("gachaCount");
        //int gameStarLv = pMsg.GetInt("gameStarLv");
        //int creditPoint = pMsg.GetInt("crePoint");
        long winnerOuhuang = pMsg.GetLong("winnerOuhuang");
        long winnerRicher = pMsg.GetLong("winnerRicher");
        long fishWinnerPoint = pMsg.GetLong("fishWinnerPoint");

        //������Ϣͬ��
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null) return;
        pPlayer.nlBaitCount = fishBaits;
        pPlayer.nlFreeBaitCount = freeFishBait;
        //pPlayer.nlRobCount = fishGanCount;
        pPlayer.nlFeiLunCount = fishLunCount;
        //pPlayer.nlBuoyCount = fishPiaoCount;
        pPlayer.nVipPlayer = (CPlayerBaseInfo.EMVipLv)vipPlayer;
        pPlayer.TreasurePoint = treasurePoint;
        pPlayer.SetUserLv(1, 0);
        pPlayer.nGachaGiftCount = 0;
        pPlayer.nGameStarLv = 0;
        pPlayer.nCreditPoint = 20;
        pPlayer.nWinnerOuhuang = winnerOuhuang;
        pPlayer.nWinnerRicher = winnerRicher;
        pPlayer.nFishWinnerPoint = fishWinnerPoint;

        //Debug.Log(CPlayerMgr.Ins.pOwner.userName + "--��ǰ��֤�����:" + creditPoint);
        //��ȡ����������ջ�
        if (CPlayerMgr.Ins.pOwner != null &&
            uid.Equals(CPlayerMgr.Ins.pOwner.uid))
        {
            CRoomRecordInfoMgr.Ins.RoomGainCoin = pMsg.GetLong("roomGainCoin");
        }

        //��ȡ��ҵĴ�ID
        int nBoatId = pMsg.GetInt("boatAvatar");
        pPlayer.nBoatAvatarId = nBoatId;

        if(pPlayer.nBoatAvatarId == 101)
        {
            pPlayer.RefreshBoatAvatar();
        }
        
        if(pPlayer.avatarId == 101)
        {
            pPlayer.RefreshRoleAvatar();
        }

        //��ȡ��ҵ����ID
        int nFishGanID = pMsg.GetInt("ganAvatar");
        if (nFishGanID <= 0)
        {
            nFishGanID = 101;
        }
        pPlayer.nFishGanAvatarId = nFishGanID;
        pPlayer.RefreshProAdd();

        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if (pUnit != null)
        {
            pUnit.dlgChgGift?.Invoke();
        }

        //���⴦��ĵ�¼
        //CHttpParam pParamLogin = new CHttpParam
        //(
        //    new CHttpParamSlot("uid", CPlayerMgr.Ins.pOwner.uid.ToString()),
        //    new CHttpParamSlot("nickName", System.Net.WebUtility.UrlEncode(CPlayerMgr.Ins.pOwner.userName)),
        //    new CHttpParamSlot("headIcon", System.Net.WebUtility.UrlEncode(CPlayerMgr.Ins.pOwner.userFace)),
        //    new CHttpParamSlot("userType", ((int)CPlayerBaseInfo.EMUserType.Guanzhong).ToString()),
        //    new CHttpParamSlot("fansMedalLevel", ""),
        //    new CHttpParamSlot("fansMedalName", ""),
        //    new CHttpParamSlot("fansMedalWearingStatus", ""),
        //    new CHttpParamSlot("guardLevel", "")
        //);

        //CHttpMgr.Instance.SendHttpMsg(
        //    CHttpConst.LoginViewer,
        //    new HHandlerLoginVtuberSpecial(CPlayerMgr.Ins.pOwner.userName, CPlayerMgr.Ins.pOwner.userFace),
        //    pParamLogin);

        CPlayerNetHelper.Login(CPlayerMgr.Ins.pOwner.uid, CPlayerMgr.Ins.pOwner.userName, CPlayerMgr.Ins.pOwner.userFace, CPlayerBaseInfo.EMUserType.Guanzhong,
                               0, "", false, 0,
                               new HHandlerLoginVtuberSpecial(CPlayerMgr.Ins.pOwner.userName, CPlayerMgr.Ins.pOwner.userFace));
    }
}
