using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfitRankInfo
{
    public string nlUserUID;
    public string szUserName;
    public string szHeadIcon;
    public long nlTotalCoin;

    public ProfitRankInfo()
    {

    }

    public ProfitRankInfo(CFishRecordInfo recordInfo)
    {
        nlUserUID = recordInfo.uid;
        szUserName = recordInfo.userName;
        szHeadIcon = recordInfo.headIcon;
        nlTotalCoin = recordInfo.nTotalCoin;
    }
}

public class CProfitRankMgr : CSingleMgrBase<CProfitRankMgr>
{
    public List<ProfitRankInfo> listRankInfos = new List<ProfitRankInfo>();

    /// <summary>
    /// �����µ���Ϣ
    /// </summary>
    /// <param name="recordInfo"></param>
    /// <param name="nMaxCount"></param>
    /// <returns></returns>
    public bool AddInfo(CFishRecordInfo recordInfo, int nMaxCount)
    {
        ProfitRankInfo fishRecordInfo = GetInfo(recordInfo.uid);
        bool bDel = false;
        bool bNeedRefresh = false;
        if (fishRecordInfo == null)
        {
            ProfitRankInfo rankInfo = new ProfitRankInfo(recordInfo);
            if (listRankInfos.Count < nMaxCount)
            {
                listRankInfos.Add(rankInfo);
                bNeedRefresh = true;
            }
            else
            {
                for (int i = 0; i < listRankInfos.Count; i++)
                {
                    //�ж��Ƿ�����б��е�����
                    if (listRankInfos[i].nlTotalCoin < recordInfo.nTotalCoin)
                    {
                        listRankInfos.Insert(i, rankInfo);
                        ///�ж��Ƿ񳬹����������
                        if (listRankInfos.Count > nMaxCount)
                        {
                            bDel = true;
                        }
                        bNeedRefresh = true;
                        break;
                    }
                }
            }
        }
        else
        {
            bNeedRefresh = true;
            fishRecordInfo.nlTotalCoin = recordInfo.nTotalCoin;
        }
        if (bDel)
        {
            listRankInfos.RemoveAt(listRankInfos.Count - 1);
        }
        ///�����ı�ʱˢ���ļ�����
        if (bNeedRefresh)
        {
            CLocalRankInfoMgr.Ins.SaveInfo();
        }
        return bNeedRefresh;
    }

    public ProfitRankInfo GetInfo(string uid)
    {
        ProfitRankInfo fishRecordInfo = null;

        fishRecordInfo = listRankInfos.Find(x => x.nlUserUID == uid);

        return fishRecordInfo;
    }

    /// <summary>
    /// ����
    /// </summary>
    public void Sort()
    {
        ///���ݼ۸�����(�Ӹߵ���)
        listRankInfos.Sort((x, y) =>
        {
            if (y.nlTotalCoin < x.nlTotalCoin)
            {
                return -1;
            }
            else if (y.nlTotalCoin == x.nlTotalCoin)
            {
                return 1;
            }
            else
            {
                return 1;
            }
        });
    }

    public List<ProfitRankInfo> GetRankInfos()
    {
        return listRankInfos;
    }

    public CLocalNetArrayMsg ToMsg()
    {
        CLocalNetArrayMsg localNetArrayMsg = new CLocalNetArrayMsg();
        for (int i = 0; i < listRankInfos.Count; i++)
        {
            CLocalNetMsg msgInfo = new CLocalNetMsg();
            msgInfo.SetString("useruid", listRankInfos[i].nlUserUID);
            msgInfo.SetString("username", listRankInfos[i].szUserName);
            msgInfo.SetString("headicon", listRankInfos[i].szHeadIcon);
            msgInfo.SetLong("totalprice", listRankInfos[i].nlTotalCoin);
            localNetArrayMsg.AddMsg(msgInfo);
        }
        return localNetArrayMsg;
    }

    public List<ProfitRankInfo> LoadMsg(CLocalNetArrayMsg pArrayMsg)
    {
        List<ProfitRankInfo> listLoadInfos = new List<ProfitRankInfo>();

        int nRankSize = pArrayMsg.GetSize();
        for (int i = 0; i < nRankSize; i++)
        {
            ProfitRankInfo pRankInfo = new ProfitRankInfo();
            CLocalNetMsg pMsgInfo = pArrayMsg.GetNetMsg(i);
            //UID
            pRankInfo.nlUserUID = pMsgInfo.GetString("useruid");
            //�û���
            pRankInfo.szUserName = pMsgInfo.GetString("username");
            //ͷ��
            pRankInfo.szHeadIcon = pMsgInfo.GetString("headicon");
            //������
            pRankInfo.nlTotalCoin = pMsgInfo.GetLong("totalprice");
            //��������
            listLoadInfos.Add(pRankInfo);
        }
        return listLoadInfos;
    }

    public void Clear()
    {
        listRankInfos.Clear();
    }
}
