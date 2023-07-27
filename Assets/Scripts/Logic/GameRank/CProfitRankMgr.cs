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
    /// 增加新的信息
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
                    //判断是否大于列表中的数据
                    if (listRankInfos[i].nlTotalCoin < recordInfo.nTotalCoin)
                    {
                        listRankInfos.Insert(i, rankInfo);
                        ///判断是否超过了最大数量
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
        ///发生改变时刷新文件数据
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
    /// 排序
    /// </summary>
    public void Sort()
    {
        ///根据价格排序(从高到低)
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
            //用户名
            pRankInfo.szUserName = pMsgInfo.GetString("username");
            //头像
            pRankInfo.szHeadIcon = pMsgInfo.GetString("headicon");
            //总收益
            pRankInfo.nlTotalCoin = pMsgInfo.GetLong("totalprice");
            //加入链表
            listLoadInfos.Add(pRankInfo);
        }
        return listLoadInfos;
    }

    public void Clear()
    {
        listRankInfos.Clear();
    }
}
