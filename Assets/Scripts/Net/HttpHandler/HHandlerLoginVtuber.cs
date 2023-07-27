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

        Debug.Log("主播登录信息：" + pMsg.GetData());
        CHttpMgr.Instance.szToken = pMsg.GetString("token");

        //初始化主播信息
        //CPlayerMgr.Ins.pOwner.avatarId = pMsg.GetInt("gameAvatar");
        //CPlayerMgr.Ins.pOwner.nLv = pMsg.GetInt("gameLv");
        //CPlayerMgr.Ins.pOwner.GameCoins = pMsg.GetLong("fishCoin");
        //CPlayerMgr.Ins.pOwner.AvatarSuipian = pMsg.GetLong("avatarFragments");
        //CPlayerMgr.Ins.pOwner.nlBaitCount = pMsg.GetLong("fishItem");

        //临时给玩家加皮肤背包
        //CPlayerMgr.Ins.pOwner.pAvatarPack = new CPlayerAvatarPack();
        //CPlayerMgr.Ins.pOwner.pBoatPack = new CPlayerBoatPack();

        CGameColorFishMgr.Ins.nCurRateUpLv = pMsg.GetInt("fishMapLv");
        CGameColorFishMgr.Ins.CurMapExp = pMsg.GetLong("fishMapExp");

        ////继续获取角色背包
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

        //TODO:请求用户的活动信息(国庆活动)
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

        //同步盲盒
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugGetFishGachaBoxConfig, new HHandlerGetGachaGiftList());

        //请求主播房间稀有鱼数据
        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("uid", CDanmuSDKCenter.Ins.szRoomId),
            new CHttpParamSlot("roomId", CDanmuSDKCenter.Ins.szRoomId)
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetRoomRareFishRecord, pReqParams);
    }
}
