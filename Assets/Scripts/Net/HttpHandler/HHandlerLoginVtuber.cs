using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.LoginVtuber)]
public class HHandlerLoginVtuber : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string status = pMsg.GetString("status");
        if(!status.Equals("ok"))
        {
            UIManager.Instance.CloseUI(UIResType.NetWait);
            UIToast.Show(pMsg.GetString("msg"));
            return;
        }

        Debug.Log("������¼��Ϣ��" + pMsg.GetData());
        CHttpMgr.Instance.szToken = pMsg.GetString("token");

        //��ʼ��������Ϣ
        //CPlayerMgr.Ins.pOwner.avatarId = pMsg.GetInt("gameAvatar");
        //CPlayerMgr.Ins.pOwner.nLv = pMsg.GetInt("gameLv");
        //CPlayerMgr.Ins.pOwner.GameCoins = pMsg.GetLong("fishCoin");
        //CPlayerMgr.Ins.pOwner.AvatarSuipian = pMsg.GetLong("avatarFragments");
        //CPlayerMgr.Ins.pOwner.nlBaitCount = pMsg.GetLong("fishItem");

        //��ʱ����Ҽ�Ƥ������
        //CPlayerMgr.Ins.pOwner.pAvatarPack = new CPlayerAvatarPack();
        //CPlayerMgr.Ins.pOwner.pBoatPack = new CPlayerBoatPack();

        CGameColorFishMgr.Ins.nCurRateUpLv = pMsg.GetInt("fishMapLv");
        CGameColorFishMgr.Ins.CurMapExp = pMsg.GetLong("fishMapExp");

        ////������ȡ��ɫ����
        //CHttpParam pReqParams = new CHttpParam(
        //    new CHttpParamSlot("uid", CPlayerMgr.Ins.pOwner.uid.ToString()),
        //    new CHttpParamSlot("roomId", CDanmuSDKCenter.Ins.szRoomId.ToString()),
        //    new CHttpParamSlot("isVtb", "1")
        //);

        //CHttpParam pReqParamsOnlyUid = new CHttpParam(
        //    new CHttpParamSlot("uid", CPlayerMgr.Ins.pOwner.uid.ToString())
        //);

        //CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetAvatarList, pReqParams);
        //CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetBoatList, pReqParams);
        //CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetFishGanList, pReqParamsOnlyUid);
        //CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetUserFishMat, pReqParamsOnlyUid);
        //CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetUserFishInfo, new HHandlerGetVtbFishInfo(), pReqParams, CHttpMgr.Instance.nReconnectTimes);

        //TODO:�����û��Ļ��Ϣ(����)
        //if(CFishFesInfoMgr.Ins.IsFesOn(1))
        //{
        //    CHttpParam pReqUserFesParams = new CHttpParam(
        //        new CHttpParamSlot("uid", CPlayerMgr.Ins.pOwner.uid.ToString()),
        //        new CHttpParamSlot("packId", "1")
        //    );
        //    CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetUserFesInfo, pReqUserFesParams);
        //}
        
        //if(CFishFesInfoMgr.Ins.IsFesOn(2))
        //{
        //    CHttpParam pReqUserFes2Params = new CHttpParam(
        //        new CHttpParamSlot("uid", CPlayerMgr.Ins.pOwner.uid.ToString()),
        //        new CHttpParamSlot("packId", "2")
        //    );
        //    CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetUserFesInfo, pReqUserFes2Params);
        //}

        //ͬ��ä��
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugGetFishGachaBoxConfig, new HHandlerGetGachaGiftList());

        //������������ϡ��������
        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("uid", CDanmuSDKCenter.Ins.szRoomId),
            new CHttpParamSlot("roomId", CDanmuSDKCenter.Ins.szRoomId)
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetRoomRareFishRecord, pReqParams);
    }
}
