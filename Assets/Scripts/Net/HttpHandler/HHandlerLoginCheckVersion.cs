using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HHandlerLoginCheckVersion : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string szStatus = pMsg.GetString("status");
        if (!szStatus.Equals("ok")) return;

        string szVersion = pMsg.GetString("version");
        string szBroadContent = pMsg.GetString("broadcontent");
        long serverTimeStamp = pMsg.GetLong("timestamp");
        CEncryptHelper.KEY = pMsg.GetString("broadKey");
        CEncryptHelper.IV = pMsg.GetString("broadIv");

        CGameColorFishMgr.Ins.pTickerHeart = new CPropertyTimer();
        CGameColorFishMgr.Ins.pTickerHeart.Value = CGameColorFishMgr.Ins.nTimeHeartBeat;
        CGameColorFishMgr.Ins.pTickerHeart.FillTime();

        CGameColorFishMgr.Ins.pHeartInfo = new CGameHeartBeatInfo();
        CGameColorFishMgr.Ins.pHeartInfo.szVersion = szVersion;
        CGameColorFishMgr.Ins.pHeartInfo.szBroadContent = szBroadContent;
        CGameColorFishMgr.Ins.pHeartInfo.nServerTimeStamp = serverTimeStamp;
        CGameColorFishMgr.Ins.pHeartInfo.nRecordTimeStamp = CTimeMgr.NowMillonsSec();

        CFishFesInfoMgr.Ins.szBroadContent = CGameColorFishMgr.Ins.pHeartInfo.szBroadContent;

        if (CGameColorFishMgr.Ins.pHeartInfo.szVersion.Equals(Application.version))
        {
            //登陆
            CHttpParam pParamLoginV = new CHttpParam
            (
                new CHttpParamSlot("uid", CDanmuSDKCenter.Ins.szRoomId),
                new CHttpParamSlot("roomId", CDanmuSDKCenter.Ins.szRoomId),
                new CHttpParamSlot("code", CDanmuSDKCenter.Ins.szUid),
                new CHttpParamSlot("nickName", ""),
                new CHttpParamSlot("headIcon", ""),
                new CHttpParamSlot("userType", ((int)CPlayerBaseInfo.EMUserType.Zhubo).ToString()),
                new CHttpParamSlot("channel", CDanmuSDKCenter.Ins.emPlatform.ToString().ToLower())
            );

            CHttpMgr.Instance.SendHttpMsg(CHttpConst.LoginVtuber, pParamLoginV);
        }
        else
        {
            UIMsgBox.Show("", "有新版本！请更新至最新版本进行游戏", UIMsgBox.EMType.OK,
                delegate() {
                    CDanmuSDKCenter.Ins.EndGame(true);
                });

            return;
        }
    }
}
