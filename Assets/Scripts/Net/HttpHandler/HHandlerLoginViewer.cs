using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HHandlerLoginViewer : INetEventHandler
{
    public string szName;
    public string szIcon;

    public OnDeleagteLoginByTypeSuc callLoginOver;
    public string szNextHttpReq;
    public CHttpParam pNextParamReq;

    public HHandlerLoginViewer(string name, string icon, string nextReq = "", CHttpParam nextParams = null, OnDeleagteLoginByTypeSuc call = null)
    {
        szName = name;
        szIcon = icon;

        callLoginOver = call;
        szNextHttpReq = nextReq;
        pNextParamReq = nextParams;
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        Debug.Log("观众登录信息：" + pMsg.GetData());

        string uid = pMsg.GetString("uid");
        if (string.IsNullOrEmpty(uid)) return;

        string szToken = pMsg.GetString("token");
        int lv = pMsg.GetInt("gameLv");
        int avatarId = pMsg.GetInt("gameAvatar");
        int crown = pMsg.GetInt("gameScore");
       // long coin = pMsg.GetLong("fishCoin");
        long avatarSuiPian = pMsg.GetLong("avatarFragments");
        long fansMedalLv = pMsg.GetLong("fansMedalLevel");
        string fansMedalName = pMsg.GetString("fansMedalName");
        bool fansMedalWear = pMsg.GetBool("fansMedalWearingStatus");
        long guardLv = pMsg.GetLong("guardLevel");
        //int gameStarLv = pMsg.GetInt("gameStarLv");
        //int creditPoint = pMsg.GetInt("crePoint");

        //long baitCount = pMsg.GetLong("fishItem");

        CPlayerBaseInfo pPlayerInfo = CPlayerMgr.Ins.GetPlayer(uid); 
        if(pPlayerInfo == null)
        {
            pPlayerInfo = new CPlayerBaseInfo(uid, szName, szIcon, fansMedalLv, fansMedalName, fansMedalWear, guardLv, CDanmuSDKCenter.Ins.szRoomId.ToString(), CPlayerBaseInfo.EMUserType.Guanzhong);
            pPlayerInfo.avatarId = avatarId; // Random.Range(101, 109); //avatarId;
            pPlayerInfo.nBattery = 0;
            pPlayerInfo.AvatarSuipian = avatarSuiPian;
            //pPlayerInfo.nlBaitCount = baitCount;
            //pPlayerInfo.GameCoins = coin;
            //pPlayerInfo.nGameStarLv = gameStarLv;
            //pPlayerInfo.nCreditPoint = creditPoint;
            CPlayerMgr.Ins.AddPlayer(pPlayerInfo);
        }
        else
        {
            pPlayerInfo.userName = szName;
            pPlayerInfo.userFace = szIcon;
            pPlayerInfo.fansMedalLevel = fansMedalLv;
            pPlayerInfo.fansMedalName = fansMedalName;
            pPlayerInfo.fansMedalWearingStatus = fansMedalWear;
            pPlayerInfo.guardLevel = guardLv;
            pPlayerInfo.roomId = CDanmuSDKCenter.Ins.szRoomId.ToString();
            //pPlayerInfo.GameCoins = coin;
            pPlayerInfo.avatarId = avatarId; // Random.Range(101, 109); //avatarId;
            pPlayerInfo.nBattery = 0;
            //pPlayerInfo.nlBaitCount = baitCount;
            pPlayerInfo.AvatarSuipian = avatarSuiPian;
            //pPlayerInfo.nGameStarLv = gameStarLv;
            //pPlayerInfo.nCreditPoint = creditPoint;
        }

        //继续获取角色背包
        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("uid", pPlayerInfo.uid),
            new CHttpParamSlot("roomId", CDanmuSDKCenter.Ins.szRoomId.ToString()),
            new CHttpParamSlot("isVtb", "0")
        );

        CHttpParam pReqParamsOnlyUid = new CHttpParam(
            new CHttpParamSlot("uid", pPlayerInfo.uid.ToString())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetAvatarList, pReqParams);
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetBoatList, pReqParams);
        //CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetFishGanList, pReqParamsOnlyUid);
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetUserFishMat, pReqParamsOnlyUid);
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetUserFishInfo, new HHandlerGetUserFishInfo(this.LoginOver), pReqParams, CHttpMgr.Instance.nReconnectTimes);

        //TODO:请求用户的活动信息
        if (CFishFesInfoMgr.Ins.IsFesOn(1))
        {
            CHttpParam pReqUserFesParams = new CHttpParam(
                new CHttpParamSlot("uid", pPlayerInfo.uid.ToString()),
                new CHttpParamSlot("packId", ((int)CFishFesInfoMgr.EMFesType.RankOuhuang).ToString())
            );
            CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetUserFesInfo, pReqUserFesParams);
        }

        if (CFishFesInfoMgr.Ins.IsFesOn(2))
        {
            CHttpParam pReqUserFes2Params = new CHttpParam(
                new CHttpParamSlot("uid", pPlayerInfo.uid.ToString()),
                new CHttpParamSlot("packId", ((int)CFishFesInfoMgr.EMFesType.RankRicher).ToString())
            );
            CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetUserFesInfo, pReqUserFes2Params);
        }
    }

    void LoginOver(CPlayerBaseInfo player)
    {
        callLoginOver?.Invoke(player, CGameColorFishMgr.EMJoinType.Normal);

        if (!CHelpTools.IsStringEmptyOrNone(szNextHttpReq))
        {
            CHttpMgr.Instance.SendHttpMsg(szNextHttpReq, pNextParamReq);
        }
    }

    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }
}
