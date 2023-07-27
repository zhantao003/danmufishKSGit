using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMRankType
{
    RareFish = 0,        //ϡ����
    MapLv,               //�泡�ȼ�
    MapCain,             //�泡����
    FishVtb,             //��������

    WinnerOuhuang,       //ŷ�ʰ�
    WinnerRicher,        //������

    Max,
}

public class CWorldRankInfoMgr : CSingleCompBase<CWorldRankInfoMgr>
{
    public long nGetRankInfoCD; //��ȡ������CD(����)

    //��ǰͨ��������Ϣ
    Dictionary<EMRankType, List<CPlayerRankInfo>> dicCommonRankInfo = new Dictionary<EMRankType, List<CPlayerRankInfo>>();
    Dictionary<EMRankType, CPlayerRankInfo> dicSelfCommonInfo = new Dictionary<EMRankType, CPlayerRankInfo>();

    //��ǰϡ����������Ϣ
    Dictionary<int, List<CPlayerRankInfo>> dicFishRankInfo = new Dictionary<int, List<CPlayerRankInfo>>();
    Dictionary<int, CPlayerRankInfo> dicSelfFishInfo = new Dictionary<int, CPlayerRankInfo>();

    //���һ�λ�ȡָ��������Ϣ��ʱ��
    Dictionary<EMRankType, DateTime> dicGetCommonInfoTime = new Dictionary<EMRankType, DateTime>();
    Dictionary<int, DateTime> dicGetFishInfoTime = new Dictionary<int, DateTime>();

    //����������Ϣ
    Dictionary<EMRankType, List<CPlayerResRankInfo>> dicResPlayerRank = new Dictionary<EMRankType, List<CPlayerResRankInfo>>();

    /// <summary>
    /// ����ͨ��������Ϣ
    /// </summary>
    public void SetCommonRankInfo(EMRankType rankType, List<CPlayerRankInfo> infos)
    {
        if (dicCommonRankInfo.ContainsKey(rankType))
        {
            dicCommonRankInfo[rankType] = infos;
        }
        else
        {
            dicCommonRankInfo.Add(rankType, infos);
        }
    }

    /// <summary>
    /// ���õ���������Ϣ
    /// </summary>
    /// <param name="fishId"></param>
    /// <param name="infos"></param>
    public void SetFishRankInfo(int fishId, List<CPlayerRankInfo> infos)
    {
        if (dicFishRankInfo.ContainsKey(fishId))
        {
            dicFishRankInfo[fishId] = infos;
        }
        else
        {
            dicFishRankInfo.Add(fishId, infos);
        }
    }

    /// <summary>
    /// ���ý���������Ϣ
    /// </summary>
    /// <param name="rankType"></param>
    /// <param name="infos"></param>
    public void SetResRankSlotList(EMRankType rankType, List<CPlayerResRankInfo> infos)
    {
        if (dicResPlayerRank.ContainsKey(rankType))
        {
            dicResPlayerRank[rankType] = infos;
        }
        else
        {
            dicResPlayerRank.Add(rankType, infos);
        }
    }

    //��ȡָ�������а�����
    public List<CPlayerRankInfo> GetRankInfo(EMRankType emRankType, int value)
    {
        List<CPlayerRankInfo> listCurInfos = new List<CPlayerRankInfo>();
        if (emRankType == EMRankType.RareFish)
        {
            if (dicFishRankInfo.TryGetValue(value, out listCurInfos))
            {

            }
        }
        else
        {
            if (dicCommonRankInfo.TryGetValue(emRankType, out listCurInfos))
            {

            }
        }

        return listCurInfos;
    }

    //��ȡ����ʱָ�������а�����
    public List<CPlayerResRankInfo> GetResRankSlotList(EMRankType emRankType)
    {
        List<CPlayerResRankInfo> listCurInfos = new List<CPlayerResRankInfo>();
        if (dicResPlayerRank.TryGetValue(emRankType, out listCurInfos))
        {

        }

        return listCurInfos;
    }

    /// <summary>
    /// ��ȡ��ҵ�����������Ϣ
    /// </summary>
    /// <param name="rankType"></param>
    /// <param name="uid"></param>
    /// <returns></returns>
    public CPlayerRankInfo GetPlayerRankInWorld(string uid, EMRankType rankType, int value)
    {
        List<CPlayerRankInfo> listRankInfo = GetRankInfo(rankType, value);

        CPlayerRankInfo pRes = listRankInfo.Find(x=> x.uid.Equals(uid));
        
        return pRes;
    }

    /// <summary>
    /// ��ȡ����ʱ��ҵ�����������Ϣ
    /// </summary>
    /// <param name="rankType"></param>
    /// <param name="uid"></param>
    /// <returns></returns>
    public CPlayerResRankInfo GetPlayerResRankSlotInWorld(string uid, EMRankType rankType)
    {
        List<CPlayerResRankInfo> listRankInfo = GetResRankSlotList(rankType);

        CPlayerResRankInfo pRes = listRankInfo.Find(x => x.uid.Equals(uid));

        return pRes;
    }

    public bool IsRankNeedReq(EMRankType rankType, int value)
    {
        bool bReqAble = false;
        EMRankType emRankType = rankType;

        //�ж��Ƿ���Ҫ����������Ϣ����Ϣ
        DateTime pLastTime;
        if (rankType == EMRankType.RareFish)
        {
            if (!dicGetFishInfoTime.TryGetValue(value, out pLastTime))
            {
                pLastTime = DateTime.Now;
                dicGetFishInfoTime.Add(value, pLastTime);
                bReqAble = true;
            }
            else
            {
                TimeSpan pTimeLerp = DateTime.Now - pLastTime;
                if (pTimeLerp.TotalMilliseconds >= nGetRankInfoCD)
                {
                    dicGetFishInfoTime[value] = DateTime.Now;
                    bReqAble = true;
                }
            }
        }
        else
        {
            if (!dicGetCommonInfoTime.TryGetValue(rankType, out pLastTime))
            {
                pLastTime = DateTime.Now;
                dicGetCommonInfoTime.Add(rankType, pLastTime);
                bReqAble = true;
            }
            else
            {
                TimeSpan pTimeLerp = DateTime.Now - pLastTime;
                if (pTimeLerp.TotalMilliseconds >= nGetRankInfoCD)
                {
                    dicGetCommonInfoTime[rankType] = DateTime.Now;
                    bReqAble = true;
                }
            }
        }

        return bReqAble;
    }

    //�������а�����
    public void ReqRankInfo(EMRankType emRankType, int value, DelegateNFuncCall suc = null)
    {
        //��ȡָ��������Ϣ
        if (emRankType == EMRankType.RareFish)
        {
            CHttpParam pReqParams = new CHttpParam(
                new CHttpParamSlot("fishId", value.ToString()),
                new CHttpParamSlot("uid", CPlayerMgr.Ins.pOwner.uid.ToString()),
                new CHttpParamSlot("roomId", CDanmuSDKCenter.Ins.szRoomId.ToString())
            );

            CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetRankListRareFish, pReqParams);

            UIManager.Instance.OpenUI(UIResType.NetWait);
        }
        else if (emRankType == EMRankType.MapLv)
        {
            CHttpParam pReqParams = new CHttpParam(
                new CHttpParamSlot("uid", CPlayerMgr.Ins.pOwner.uid.ToString()),
                new CHttpParamSlot("roomId", CDanmuSDKCenter.Ins.szRoomId.ToString())
            );

            CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetRankListMapLv, pReqParams);

            UIManager.Instance.OpenUI(UIResType.NetWait);
        }
        else if (emRankType == EMRankType.MapCain)
        {
            CHttpParam pReqParams = new CHttpParam(
                new CHttpParamSlot("uid", CPlayerMgr.Ins.pOwner.uid.ToString()),
                new CHttpParamSlot("roomId", CDanmuSDKCenter.Ins.szRoomId.ToString())
            );

            CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetRankListMapGain, pReqParams);

            UIManager.Instance.OpenUI(UIResType.NetWait);
        }
        else if (emRankType == EMRankType.FishVtb)
        {
            CHttpParam pReqParams = new CHttpParam(
                new CHttpParamSlot("uid", CPlayerMgr.Ins.pOwner.uid.ToString()),
                new CHttpParamSlot("roomId", CDanmuSDKCenter.Ins.szRoomId.ToString())
            );

            CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetRankListFishVtb, pReqParams);

            UIManager.Instance.OpenUI(UIResType.NetWait);
        }
        else if (emRankType == EMRankType.WinnerOuhuang)
        {
            CPlayerNetHelper.GetWinnerOuhuangRL("", new HHandlerWinnerOuhuangRL(suc));

            UIManager.Instance.OpenUI(UIResType.NetWait);
        }
        else if (emRankType == EMRankType.WinnerRicher)
        {
            CPlayerNetHelper.GetWinnerRicherRL("", new HHandlerWinnerRicherRL(suc));

            UIManager.Instance.OpenUI(UIResType.NetWait);
        }
    }

    public void ReqResRankInfo(EMRankType emRankType, List<string> userList, DelegateNFuncCall suc = null)
    {
        //��ȡָ��������Ϣ
        CLocalNetMsg msgUserList = new CLocalNetMsg();
        CLocalNetArrayMsg arrUserList = new CLocalNetArrayMsg();
        for(int i=0; i<userList.Count; i++)
        {
            CLocalNetMsg msgSlot = new CLocalNetMsg();
            msgSlot.SetString("uid", userList[i]);
            arrUserList.AddMsg(msgSlot);
        }
        msgUserList.SetNetMsgArr("userList", arrUserList);

        if (emRankType == EMRankType.WinnerOuhuang)
        {
            CHttpParam pReqParams = new CHttpParam(
                new CHttpParamSlot("userList", msgUserList.GetData())
            );

            CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetRLFishWinnerOuhuangBySlot, 
                new HHandlerGetRLFishOuhuangBySlot(suc),
                pReqParams);

            UIManager.Instance.OpenUI(UIResType.NetWait);
        }
        else if (emRankType == EMRankType.WinnerRicher)
        {
            CHttpParam pReqParams = new CHttpParam(
                new CHttpParamSlot("userList", msgUserList.GetData())
            );

            CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetRLFishWinnerRicherBySlot,
                new HHandlerGetRLFishRicherBySlot(suc),
                pReqParams);

            UIManager.Instance.OpenUI(UIResType.NetWait);
        }
    }

    public void SetSelfCommonInfo(EMRankType rankType, CPlayerRankInfo info)
    {
        if (dicSelfCommonInfo.ContainsKey(rankType))
        {
            dicSelfCommonInfo[rankType] = info;
        }
        else
        {
            dicSelfCommonInfo.Add(rankType, info);
        }
    }

    public void SetSelfFishInfo(int fishId, CPlayerRankInfo info)
    {
        if (dicSelfFishInfo.ContainsKey(fishId))
        {
            dicSelfFishInfo[fishId] = info;
        }
        else
        {
            dicSelfFishInfo.Add(fishId, info);
        }
    }
}
