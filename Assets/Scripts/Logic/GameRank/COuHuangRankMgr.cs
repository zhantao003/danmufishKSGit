using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuHuangRankInfo
{
    public string nlUserUID;
    public string szHeadIcon;
    public string szUserName;
    public string szFishName;
    public string szFishIcon;
    public EMRare emFishRare;
    public float fFishSize;
    public long nlFishPrice;
    public bool bBianYi;
    public bool bBoom;

    public OuHuangRankInfo()
    {

    }

    public OuHuangRankInfo(SpecialCastInfo castInfo)
    {
        nlUserUID = castInfo.playerInfo.uid;
        szUserName = castInfo.playerInfo.userName;
        szHeadIcon = castInfo.playerInfo.userFace;
        szFishIcon = castInfo.fishInfo.szIcon;
        szFishName = castInfo.fishInfo.szName;
        emFishRare = castInfo.fishInfo.emRare;
        fFishSize = castInfo.fishInfo.fCurSize;
        nlFishPrice = castInfo.fishInfo.lPrice;
        bBianYi = castInfo.fishInfo.bBianYi;
        bBoom = castInfo.fishInfo.bBoom;
    }

    public void RefreshInfo(SpecialCastInfo castInfo)
    {
        nlUserUID = castInfo.playerInfo.uid;
        szUserName = castInfo.playerInfo.userName;
        szHeadIcon = castInfo.playerInfo.userFace;
        szFishIcon = castInfo.fishInfo.szIcon;
        szFishName = castInfo.fishInfo.szName;
        emFishRare = castInfo.fishInfo.emRare;
        fFishSize = castInfo.fishInfo.fCurSize;
        nlFishPrice = castInfo.fishInfo.lPrice;
        bBianYi = castInfo.fishInfo.bBianYi;
        bBoom = castInfo.fishInfo.bBoom;
    }
}

public class COuHuangRankMgr : CSingleMgrBase<COuHuangRankMgr>
{
    public List<OuHuangRankInfo> listRankInfos = new List<OuHuangRankInfo>();


    public OuHuangRankInfo GetInfo(string uid)
    {
        OuHuangRankInfo fishRecordInfo = null;

        fishRecordInfo = listRankInfos.Find(x => x.nlUserUID.Equals(uid));

        return fishRecordInfo;
    }

    /// <summary>
    /// 增加新的信息
    /// </summary>
    /// <param name="castInfo"></param>
    /// <param name="nMaxCount"></param>
    /// <returns></returns>
    public bool AddInfo(SpecialCastInfo castInfo, int nMaxCount)
    {
        OuHuangRankInfo rankInfo = null;
        bool bDel = false;
        bool bNeedRefresh = false;
        ///判断是否开启了排行榜去重的设置
        if (CGameColorFishMgr.Ins.pGameRoomConfig.bActiveRankRepeat)
        {
            rankInfo = GetInfo(castInfo.playerInfo.uid);
            if(rankInfo == null)
            {
                
                rankInfo = new OuHuangRankInfo(castInfo);
                if (listRankInfos.Count < nMaxCount)
                {
                    listRankInfos.Add(rankInfo);
                    bNeedRefresh = true;
                }
                else
                {
                    for (int i = 0; i < listRankInfos.Count; i++)
                    {
                        if (listRankInfos[i].nlFishPrice < rankInfo.nlFishPrice)
                        {
                            listRankInfos.Insert(i, rankInfo);
                            bDel = true;
                            bNeedRefresh = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                
                if (rankInfo.nlFishPrice < castInfo.fishInfo.lPrice)
                {
                    rankInfo.RefreshInfo(castInfo);
                    bNeedRefresh = true;
                }
            }
        }
        else
        {
            rankInfo = new OuHuangRankInfo(castInfo);
            if (listRankInfos.Count < nMaxCount)
            {
                listRankInfos.Add(rankInfo);
                bNeedRefresh = true;
            }
            else
            {
                for (int i = 0; i < listRankInfos.Count; i++)
                {
                    if (listRankInfos[i].nlFishPrice < rankInfo.nlFishPrice)
                    {
                        listRankInfos.Insert(i, rankInfo);
                        bDel = true;
                        bNeedRefresh = true;
                        break;
                    }
                }
            }
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

    /// <summary>
    /// 排序
    /// </summary>
    public void Sort()
    {
        listRankInfos.Sort((x, y) =>
        {
            if (y.nlFishPrice < x.nlFishPrice)
            {
                return -1;
            }
            else if (y.nlFishPrice == x.nlFishPrice)
            {
                if (x.fFishSize > y.fFishSize)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
        });
    }

    public List<OuHuangRankInfo> GetRankInfos()
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
            msgInfo.SetString("fishname", listRankInfos[i].szFishName);
            msgInfo.SetString("fishicon", listRankInfos[i].szFishIcon);
            msgInfo.SetInt("fishrare", (int)listRankInfos[i].emFishRare);
            msgInfo.SetFloat("fishsize", listRankInfos[i].fFishSize);
            msgInfo.SetLong("fishprice", listRankInfos[i].nlFishPrice);
            msgInfo.SetInt("bianyi", listRankInfos[i].bBianYi ? 1 : 0);
            msgInfo.SetInt("boom", listRankInfos[i].bBoom ? 1 : 0);
            localNetArrayMsg.AddMsg(msgInfo);
        }
        return localNetArrayMsg;
    }

    public List<OuHuangRankInfo> LoadMsg(CLocalNetArrayMsg pArrayMsg)
    {
        List<OuHuangRankInfo> listLoadInfos = new List<OuHuangRankInfo>();
        int nRankSize = pArrayMsg.GetSize();
        for (int i = 0; i < nRankSize; i++)
        {
            OuHuangRankInfo pRankInfo = new OuHuangRankInfo();
            CLocalNetMsg pMsgInfo = pArrayMsg.GetNetMsg(i);
            //UID
            pRankInfo.nlUserUID = pMsgInfo.GetString("useruid");
            //头像
            pRankInfo.szHeadIcon = pMsgInfo.GetString("headicon");
            //用户名
            pRankInfo.szUserName = pMsgInfo.GetString("username");
            //鱼ID
            pRankInfo.szFishIcon = pMsgInfo.GetString("fishicon");
            //鱼名
            pRankInfo.szFishName = pMsgInfo.GetString("fishname");
            //鱼品质
            pRankInfo.emFishRare = (EMRare)pMsgInfo.GetInt("fishrare");
            //鱼尺寸
            pRankInfo.fFishSize = pMsgInfo.GetFloat("fishsize");
            //鱼价格
            pRankInfo.nlFishPrice = pMsgInfo.GetLong("fishprice");
            //是否变异
            pRankInfo.bBianYi = pMsgInfo.GetInt("bianyi") > 0;
            //是否炸出来的
            pRankInfo.bBoom = pMsgInfo.GetInt("boom") > 0;
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
