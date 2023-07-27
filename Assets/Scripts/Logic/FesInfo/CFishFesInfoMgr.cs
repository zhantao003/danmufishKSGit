using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFishFesInfoMgr : CSingleMgrBase<CFishFesInfoMgr>
{
    public enum EMFesType
    {
        None = 0,

        RankOuhuang = 1,    //ŷ�ʰ�
        RankRicher = 2,     //������

        FesBroadcast = 3,   //�����
        TreasureTip = 4,    //��������

        FesDaySign = 5,     //�ǩ��
    }

    //��Ӧģ�影���
    public Dictionary<long, List<CFishFesInfoSlot>> dicFesInfos = new Dictionary<long, List<CFishFesInfoSlot>>();

    //�����
    public Dictionary<long, bool> dicFesSwitch = new Dictionary<long, bool>();

    //��һ����
    public Dictionary<long, Dictionary<string, CFishFesPlayerInfo>> dicPlayerInfos = new Dictionary<long, Dictionary<string, CFishFesPlayerInfo>>();

    //ä�б�����
    public long nGiftGachaBoxBoodiCount = 0;

    //ä���ز���Ϣ
    public List<CGiftGachaBoxInfo> listGachaGiftInfos = new List<CGiftGachaBoxInfo>();

    //����������
    public string szBroadContent;

    #region ���Ʒ����ؽӿ�

    public List<CFishFesInfoSlot> GetFesPack(long packId)
    {
        List<CFishFesInfoSlot> listRes = null;
        if (dicFesInfos.TryGetValue(packId, out listRes))
        {

        }

        return listRes;
    }

    /// <summary>
    /// ��ȡָ�������Ļ����
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

        if(info.nType == 0) //��
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

    #region ������Ϣ

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

    #region �����

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
    /// ��Ƿ��
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
    /// ��ӽ��ջ���
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
