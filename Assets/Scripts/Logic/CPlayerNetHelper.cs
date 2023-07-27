using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CPlayerNetHelper
{
    public static void Login(string uid, string userName, string headIcon, CPlayerBaseInfo.EMUserType userType, long fanLv, string fanName, bool fanWear, long guarLv, INetEventHandler handler)
    {
        Debug.Log("请求登录!");

        CHttpParam pParamLogin = new CHttpParam
        (
            new CHttpParamSlot("uid", uid),
            new CHttpParamSlot("nickName", System.Net.WebUtility.UrlEncode(userName)),
            new CHttpParamSlot("headIcon", System.Net.WebUtility.UrlEncode(headIcon)),
            new CHttpParamSlot("userType", ((int)userType).ToString()),
            new CHttpParamSlot("fansMedalLevel", fanLv.ToString()),
            new CHttpParamSlot("fansMedalName", fanName),
            new CHttpParamSlot("fansMedalWearingStatus", fanWear.ToString()),
            new CHttpParamSlot("guardLevel", guarLv.ToString())
        );

        CHttpMgr.Instance.SendHttpMsg(
               CHttpConst.LoginViewer,
               handler,
               pParamLogin);
    }

    /// <summary>
    /// 加鱼币
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="roomId"></param>
    /// <param name="isVtb"></param>
    /// <param name="add"></param>
    public static void AddFishCoin(string uid, long add, EMFishCoinAddFunc func, bool addWinner, INetEventHandler handler = null)
    {
        CHttpParam pReqParams = new CHttpParam
        (
            new CHttpParamSlot("uid", uid),
            new CHttpParamSlot("roomId", CDanmuSDKCenter.Ins.szRoomId),
            new CHttpParamSlot("fishCoin", add.ToString()),
            new CHttpParamSlot("func", ((int)func).ToString()),
            new CHttpParamSlot("time", CGameColorFishMgr.Ins.GetNowServerTime().ToString())
        );

        if(handler == null)
        {
            CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddFishCoin, pReqParams, 0, true);
        }
        else
        {
            CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddFishCoin, handler, pReqParams, 0, true);
        }

        if(addWinner &&
           add > 0 &&
           func != EMFishCoinAddFunc.Duel)
        {
            AddWinnerInfo(uid, 0, add);
        }
    }

    /// <summary>
    /// 增加用户经验
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="addExp"></param>
    public static void AddUserExp(string uid, long addExp)
    {
        //暂时屏蔽经验获取
        return;

        CHttpParam pReqParams = new CHttpParam
        (
           new CHttpParamSlot("uid", uid),
           new CHttpParamSlot("exp", addExp.ToString())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddUserExp, pReqParams, 0, true);
    }

    /// <summary>
    /// 增加渔场经验
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="roomId"></param>
    /// <param name="add"></param>
    public static void AddSceneExp(string uid, string roomId, long add, bool isPay, INetEventHandler handler = null)
    {
        //暂时屏蔽经验获取
        string szTimeStamp = CGameColorFishMgr.Ins.GetNowServerTime().ToString();
        CHttpParam pAddSceneExp = new CHttpParam
           (
               new CHttpParamSlot("uid", uid),
               new CHttpParamSlot("roomId", roomId),
               new CHttpParamSlot("exp", add.ToString()),
               new CHttpParamSlot("time", CGameColorFishMgr.Ins.GetNowServerTime().ToString()),
               new CHttpParamSlot("nonce", CEncryptHelper.AesEncrypt(uid.ToString() + roomId.ToString()+ szTimeStamp))
           );

        //记录收费礼物
        if(handler == null)
        {
            CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddSceneExp, pAddSceneExp, 0, true);
        }
        else
        {
            CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddSceneExp, handler, pAddSceneExp, 0, true);
        }
    }

    public static void AddFishItemPack(string uid, long addBait, long addGan, long addPiao, long addLun)
    {
        string szTimeStamp = CGameColorFishMgr.Ins.GetNowServerTime().ToString();
        CHttpParam pReqParams = new CHttpParam
        (
           new CHttpParamSlot("uid", uid),
           new CHttpParamSlot("baitCount", addBait.ToString()),
           new CHttpParamSlot("ganCount", addGan.ToString()),
           new CHttpParamSlot("fupiaoCount", addPiao.ToString()),
           new CHttpParamSlot("lunCount", addLun.ToString()),
           new CHttpParamSlot("time", CGameColorFishMgr.Ins.GetNowServerTime().ToString()),
           new CHttpParamSlot("nonce", CEncryptHelper.AesEncrypt(uid.ToString() + CDanmuSDKCenter.Ins.szRoomId.ToString() + szTimeStamp) + CHelpTools.GetRandomString(8,true,true,true,false,""))
        ); 
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddFishItemPack, pReqParams, CHttpMgr.Instance.nReconnectTimes, true);
    }

    /// <summary>
    /// 给用户加材料
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="itemId"></param>
    /// <param name="count"></param>
    /// <param name="daily"></param>
    /// <param name="handler"></param>
    public static void AddFishMat(string uid, int itemId, long count, int daily, INetEventHandler handler = null)
    {
        CHttpParam pReqParams = new CHttpParam
        (
            new CHttpParamSlot("uid", uid),
            new CHttpParamSlot("itemId", itemId.ToString()),
            new CHttpParamSlot("count", count.ToString()),
            new CHttpParamSlot("dailyGet", daily.ToString()),
            new CHttpParamSlot("time", CGameColorFishMgr.Ins.GetNowServerTime().ToString())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddUserFishMat, handler, pReqParams, 0, true);
    }

    /// <summary>
    /// 给用户发船
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="boatId"></param>
    public static void SendUserBoat(string uid, int boatId, INetEventHandler handler = null)
    {
        //加船
        CHttpParam pReqParams = new CHttpParam
        (
            new CHttpParamSlot("uid", uid),
            new CHttpParamSlot("boatId", boatId.ToString()),
            new CHttpParamSlot("time", CGameColorFishMgr.Ins.GetNowServerTime().ToString())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugSendFishBoat, handler, pReqParams, 0, true);
    }

    /// <summary>
    /// 给用户发鱼竿
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="ganId"></param>
    /// <param name="handler"></param>
    public static void SendUserFisnGan(string uid, int ganId, INetEventHandler handler = null)
    {
        //加鱼竿
        CHttpParam pReqParams = new CHttpParam
        (
            new CHttpParamSlot("uid", uid),
            new CHttpParamSlot("ganId", ganId.ToString()),
            new CHttpParamSlot("time", CGameColorFishMgr.Ins.GetNowServerTime().ToString())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugSendFishGan, handler, pReqParams, 0, true);
    }

    /// <summary>
    /// 添加海盗金币
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="add"></param>
    /// <param name="handler"></param>
    public static void AddTreasureCoin(string uid, long add, INetEventHandler handler = null)
    {
        string szTimeStamp = CGameColorFishMgr.Ins.GetNowServerTime().ToString();
        string roomId = "";
        if(CDanmuSDKCenter.Ins!=null)
        {
            roomId = CDanmuSDKCenter.Ins.szRoomId.ToString();
        }

        CHttpParam pReqParams2 = new CHttpParam
        (
            new CHttpParamSlot("uid", uid),
            new CHttpParamSlot("treasurePoint", add.ToString()),
            new CHttpParamSlot("time", szTimeStamp.ToString()),
            new CHttpParamSlot("nonce", CEncryptHelper.AesEncrypt(uid.ToString() + roomId.ToString() + szTimeStamp) + CHelpTools.GetRandomString(8, true, true, true, false, ""))
        );

        if (handler == null)
        {
            CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddUserTreasurePoint, pReqParams2, CHttpMgr.Instance.nReconnectTimes, true);
        }
        else
        {
            CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddUserTreasurePoint, handler, pReqParams2, CHttpMgr.Instance.nReconnectTimes, true);
        }  
    }

    /// <summary>
    /// 验证码
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="suc"></param>
    public static void Validata(string uid, bool suc)
    {
        CHttpParam pParamLogin = new CHttpParam
        (
          new CHttpParamSlot("uid", uid),
          new CHttpParamSlot("check", suc.ToString()),
          new CHttpParamSlot("time", CGameColorFishMgr.Ins.GetNowServerTime().ToString())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.Validate, pParamLogin, 0, true);
    }

    //记录欧皇冠军
    public static void AddWinnerInfo(string uid, long ouhuang, long richer)
    {
        CHttpParam pParamLogin = new CHttpParam
        (
          new CHttpParamSlot("uid", uid),
          new CHttpParamSlot("roomId", CDanmuSDKCenter.Ins.szRoomId),
          new CHttpParamSlot("ouhuang", ouhuang.ToString()),
          new CHttpParamSlot("richer", richer.ToString()),
          new CHttpParamSlot("time", CGameColorFishMgr.Ins.GetNowServerTime().ToString())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddWinnerInfo, pParamLogin, 0, true);

        if(ouhuang > 0 && 
           CFishFesInfoMgr.Ins.IsFesOn((int)CFishFesInfoMgr.EMFesType.RankOuhuang))
        {
            CFishFesInfoMgr.Ins.AddFesPoint((int)CFishFesInfoMgr.EMFesType.RankOuhuang, ouhuang, uid);
        }

        if(richer > 0 &&
           CFishFesInfoMgr.Ins.IsFesOn((int)CFishFesInfoMgr.EMFesType.RankRicher))
        {
            CFishFesInfoMgr.Ins.AddFesPoint((int)CFishFesInfoMgr.EMFesType.RankRicher, richer, uid);
        }
    }


    //获取欧皇排行榜
    public static void GetWinnerOuhuangRL(string uid, INetEventHandler handler = null)
    {
        CHttpParam pParamLogin = new CHttpParam
        (
          new CHttpParamSlot("uid", uid)
        );

        if (handler == null)
        {
            CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetRankListFishWinnerOuhuang, pParamLogin, 0, true);
        }
        else
        {
            CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetRankListFishWinnerOuhuang, handler, pParamLogin, 0, true);
        } 
    }

    //获取富豪排行榜
    public static void GetWinnerRicherRL(string uid, INetEventHandler handler = null)
    {
        CHttpParam pParamLogin = new CHttpParam
        (
          new CHttpParamSlot("uid", uid)
        );

        if(handler == null)
        {
            CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetRankListFishWinnerRicher, pParamLogin, 0, true);
        }
        else
        {
            CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetRankListFishWinnerRicher, handler, pParamLogin, 0, true);
        }
    }
}
