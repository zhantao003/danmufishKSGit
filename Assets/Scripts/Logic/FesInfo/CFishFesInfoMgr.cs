using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFishFesInfoMgr : CSingleMgrBase<CFishFesInfoMgr>
{
    public enum EMFesType
    {
        None = 0,

        RankOuhuang = 1,    //欧皇榜
        RankRicher = 2,     //富豪榜

        FesBroadcast = 3,   //活动公告
        TreasureTip = 4,    //音符公告

        FesDaySign = 5,     //活动签到
    }

    //对应模板奖金池
    public Dictionary<long, List<CFishFesInfoSlot>> dicFesInfos = new Dictionary<long, List<CFishFesInfoSlot>>();

    //活动开关
    public Dictionary<long, bool> dicFesSwitch = new Dictionary<long, bool>();

    //玩家活动积分
    public Dictionary<long, Dictionary<string, CFishFesPlayerInfo>> dicPlayerInfos = new Dictionary<long, Dictionary<string, CFishFesPlayerInfo>>();

    //盲盒保底数
    public long nGiftGachaBoxBoodiCount = 0;

    //盲盒特产信息
    public List<CGiftGachaBoxInfo> listGachaGiftInfos = new List<CGiftGachaBoxInfo>();

    //服务器公告
    public string szBroadContent;

    #region 活动奖品池相关接口

    public List<CFishFesInfoSlot> GetFesPack(long packId)
    {
        List<CFishFesInfoSlot> listRes = null;
        if (dicFesInfos.TryGetValue(packId, out listRes))
        {

        }

        return listRes;
    }

    /// <summary>
    /// 获取指定索引的活动奖励
    /// </summary>
    public CFishFesInfoSlot GetFesInfo(long packId, int idx)
    {
        List<CFishFesInfoSlot> listRes = GetFesPack(packId);
        if (listRes == null || listRes.Count <= 0) return null;

        CFishFesInfoSlot pRes = null;
        for (int i = 0; i < listRes.Count; i++)
        {
            if (listRes[i].nIdx == idx)
            {
                pRes = listRes[i];
                break;
            }
        }

        return pRes;
    }

    public void AddFesInfo(CFishFesInfoSlot info)
    {
        List<CFishFesInfoSlot> listRes = GetFesPack(info.nPackId);
        if (listRes == null)
        {
            listRes = new List<CFishFesInfoSlot>();
            dicFesInfos.Add(info.nPackId, listRes);
        }

        if (GetFesInfo(info.nPackId, info.nIdx) != null)
        {
            for (int i = 0; i < listRes.Count; i++)
            {
                if (listRes[i].nIdx == info.nIdx)
                {
                    listRes[i] = info;
                    break;
                }
            }
        }
        else
        {
            listRes.Add(info);
        }

        if(info.nType == 0) //船
        {
            ST_UnitFishBoat pTBLBoatInfo = CTBLHandlerUnitFishBoat.Ins.GetInfo(info.nID);
            if(pTBLBoatInfo!=null)
            {
                pTBLBoatInfo.bIsSeason = true;
            }
        }
        else if(info.nType == 1)
        {
            ST_UnitAvatar pTBLAvatarInfo = CTBLHandlerUnitAvatar.Ins.GetInfo(info.nID);
            if (pTBLAvatarInfo != null)
            {
                pTBLAvatarInfo.bIsSeason = true;
            }
        }
    }

    #endregion

    #region 活动玩家信息

    public void AddFesPlayerInfo(long packId, CFishFesPlayerInfo info)
    {
        Dictionary<string, CFishFesPlayerInfo> dicContent = null;
        if (dicPlayerInfos.TryGetValue(packId, out dicContent))
        {
            if(dicContent.ContainsKey(info.nUid))
            {
                dicContent[info.nUid] = info;
            }
            else
            {
                dicContent.Add(info.nUid, info);
            }
        }
        else
        {
            dicContent = new Dictionary<string, CFishFesPlayerInfo>();
            dicContent.Add(info.nUid, info);
            dicPlayerInfos.Add(packId, dicContent);
        }
    }

    public CFishFesPlayerInfo GetPlayerInfo(long packId, string playerId)
    {
        CFishFesPlayerInfo pRes = null;
        Dictionary<string, CFishFesPlayerInfo> dicContent = null;
        if(dicPlayerInfos.TryGetValue(packId, out dicContent))
        {
            if(dicContent.TryGetValue(playerId, out pRes))
            {

            }
        }

        return pRes;
    }

    #endregion

    #region 活动开关

    public void SetFesSwitch(long id, bool isOn)
    {
        if(dicFesSwitch.ContainsKey(id))
        {
            dicFesSwitch[id] = isOn;
        }
        else
        {
            dicFesSwitch.Add(id, isOn);
        }
    }

    /// <summary>
    /// 活动是否打开
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool IsFesOn(long id)
    {
        bool bIsOn = false;
        if(dicFesSwitch.TryGetValue(id, out bIsOn))
        {

        }
        return bIsOn;
    }

    #endregion

    /// <summary>
    /// 添加节日积分
    /// </summary>
    public void AddFesPoint(long packId, long num, string uid)
    {
        if (!IsFesOn(packId)) return;

        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("uid", uid),
            new CHttpParamSlot("packId", packId.ToString()),
            new CHttpParamSlot("point", num.ToString()),
            new CHttpParamSlot("isVtb", "0"),
            new CHttpParamSlot("time", CGameColorFishMgr.Ins.GetNowServerTime().ToString())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddFesPoint, pReqParams, 10, true);
    }
}
