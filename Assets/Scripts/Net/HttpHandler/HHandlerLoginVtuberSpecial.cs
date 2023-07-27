using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HHandlerLoginVtuberSpecial : INetEventHandler
{
    public string szName;
    public string szIcon;

    public delegate void OnDeleagteLoginSuc(CPlayerBaseInfo info);
    public OnDeleagteLoginSuc callLoginOver;
    public string szNextHttpReq;
    public CHttpParam pNextParamReq;

    public HHandlerLoginVtuberSpecial(string name, string icon, string nextReq = "", CHttpParam nextParams = null, OnDeleagteLoginSuc call = null)
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
        //long fishCoin = pMsg.GetLong("fishCoin");
        //long baitCount = pMsg.GetLong("fishItem");

        CPlayerBaseInfo pPlayerInfo = CPlayerMgr.Ins.GetPlayer(uid);
        //if (pPlayerInfo != null)
        //{
        //    pPlayerInfo.GameCoins = fishCoin;
        //}

        callLoginOver?.Invoke(pPlayerInfo);

        if (!CHelpTools.IsStringEmptyOrNone(szNextHttpReq))
        {
            CHttpMgr.Instance.SendHttpMsg(szNextHttpReq, pNextParamReq);
        }

        //请求主播房间稀有鱼数据
        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("uid", uid.ToString()),
            new CHttpParamSlot("roomId", CDanmuSDKCenter.Ins.szRoomId.ToString())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetRoomRareFishRecord, pReqParams);
    }

    public void OnErrorCode(CLocalNetMsg pMsg)
    {

    }

    void OnLoginOver()
    {
        //走完登录流程
        UIManager.Instance.CloseUI(UIResType.NetWait);
        UIManager.Instance.CloseUI(UIResType.MainMenu);
        UIManager.Instance.OpenUI(UIResType.RoomSetting);
        //UIManager.Instance.OpenUI(UIResType.Loading);
        //CSceneMgr.Instance.LoadScene((CSceneFactory.EMSceneType)101);
    }
}
