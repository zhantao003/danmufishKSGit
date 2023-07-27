using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRankTreasureSlot
{
    public string nlUserUID;
    public long nBomberCount;

    public int nLv1_count;
    public int nLv2_count;
    public int nLv3_count;
    public int nLv4_count;

    public string szHeadIcon;
    public string szUserName;

    public CRankTreasureSlot()
    {

    }

    public CRankTreasureSlot(SpecialCastInfo castInfo)
    {
        nlUserUID = castInfo.playerInfo.uid;
        szUserName = castInfo.playerInfo.userName;
        szHeadIcon = castInfo.playerInfo.userFace;

        nBomberCount = 1;

        if (castInfo.fishInfo.nTreasureLv == 1)
        {
            nLv1_count += 1;
        }
        else if (castInfo.fishInfo.nTreasureLv == 2)
        {
            nLv2_count += 1;
        }
        else if (castInfo.fishInfo.nTreasureLv == 3)
        {
            nLv3_count += 1;
        }
        else if (castInfo.fishInfo.nTreasureLv == 4)
        {
            nLv4_count += 1;
        }
    }

    public void AddInfo(SpecialCastInfo castInfo)
    {
        nlUserUID = castInfo.playerInfo.uid;
        szUserName = castInfo.playerInfo.userName;
        szHeadIcon = castInfo.playerInfo.userFace;

        nBomberCount += 1;

        if (castInfo.fishInfo.nTreasureLv == 1)
        {
            nLv1_count += 1;
        }
        else if (castInfo.fishInfo.nTreasureLv == 2)
        {
            nLv2_count += 1;
        }
        else if (castInfo.fishInfo.nTreasureLv == 3)
        {
            nLv3_count += 1;
        }
        else if (castInfo.fishInfo.nTreasureLv == 4)
        {
            nLv4_count += 1;
        }
    }
}

public class CRankTreasureMgr : CSingleMgrBase<CRankTreasureMgr>
{
    public DelegateNFuncCall dlgRefreshInfo;

    public List<CRankTreasureSlot> listRankInfos = new List<CRankTreasureSlot>();

    public CRankTreasureSlot GetInfo(string uid)
    {
        CRankTreasureSlot fishRecordInfo = null;

        fishRecordInfo = listRankInfos.Find(x => x.nlUserUID.Equals(uid));

        return fishRecordInfo;
    }

    public List<CRankTreasureSlot> GetRankInfos()
    {
        return listRankInfos;
    }

    public bool AddInfo(SpecialCastInfo castInfo)
    {
        CRankTreasureSlot rankInfo = GetInfo(castInfo.playerInfo.uid); //new CRankTreasureSlot(castInfo);
        if(rankInfo == null)
        {
            rankInfo = new CRankTreasureSlot(castInfo);
            listRankInfos.Add(rankInfo);
        }
        else
        {
            rankInfo.AddInfo(castInfo);
        }
       
        Sort();

        ///发生改变时刷新文件数据
        CLocalRankInfoMgr.Ins.SaveVSInfo();

        dlgRefreshInfo?.Invoke();

        return true;
    }

    /// <summary>
    /// 排序
    /// </summary>
    public void Sort()
    {
        listRankInfos.Sort((x, y) =>
        {
            if(x.nLv4_count != y.nLv4_count)
            {
                return y.nLv4_count.CompareTo(x.nLv4_count);
            }
            else
            {
                if(x.nLv3_count!=y.nLv3_count)
                {
                    return y.nLv3_count.CompareTo(x.nLv3_count);
                }
                else
                {
                    if (x.nLv2_count != y.nLv2_count)
                    {
                        return y.nLv2_count.CompareTo(x.nLv2_count);
                    }
                    else
                    {
                        return y.nLv1_count.CompareTo(x.nLv1_count);
                    }
                }
            }
        });
    }

    public void Clear(bool reset = true)
    {
        if(!reset)
        {
            dlgRefreshInfo = null;
        }
        
        listRankInfos.Clear();
    }

    public CLocalNetArrayMsg ToMsg()
    {
        CLocalNetArrayMsg localNetArrayMsg = new CLocalNetArrayMsg();
        for (int i = 0; i < listRankInfos.Count; i++)
        {
            CLocalNetMsg msgInfo = new CLocalNetMsg();
            msgInfo.SetString("useruid", listRankInfos[i].nlUserUID);
            msgInfo.SetLong("bomber", listRankInfos[i].nBomberCount);
            msgInfo.SetString("username", listRankInfos[i].szUserName);
            msgInfo.SetString("headicon", listRankInfos[i].szHeadIcon);

            msgInfo.SetInt("lv1count", listRankInfos[i].nLv1_count);
            msgInfo.SetInt("lv2count", listRankInfos[i].nLv2_count);
            msgInfo.SetInt("lv3count", listRankInfos[i].nLv3_count);
            msgInfo.SetInt("lv4count", listRankInfos[i].nLv4_count);
            localNetArrayMsg.AddMsg(msgInfo);
        }
        return localNetArrayMsg;
    }

    public List<CRankTreasureSlot> LoadMsg(CLocalNetArrayMsg pArrayMsg)
    {
        List<CRankTreasureSlot> listLoadInfos = new List<CRankTreasureSlot>();
        int nRankSize = pArrayMsg.GetSize();
        for (int i = 0; i < nRankSize; i++)
        {
            CRankTreasureSlot pRankInfo = new CRankTreasureSlot();
            CLocalNetMsg pMsgInfo = pArrayMsg.GetNetMsg(i);
            //UID
            pRankInfo.nlUserUID = pMsgInfo.GetString("useruid");
            pRankInfo.nBomberCount = pMsgInfo.GetLong("bomber");
            //头像
            pRankInfo.szHeadIcon = pMsgInfo.GetString("headicon");
            //用户名
            pRankInfo.szUserName = pMsgInfo.GetString("username");
            //宝藏数量
            pRankInfo.nLv1_count = pMsgInfo.GetInt("lv1count");
            pRankInfo.nLv2_count = pMsgInfo.GetInt("lv2count");
            pRankInfo.nLv3_count = pMsgInfo.GetInt("lv3count");
            pRankInfo.nLv4_count = pMsgInfo.GetInt("lv4count");
            //加入链表
            listLoadInfos.Add(pRankInfo);
        }
        return listLoadInfos;
    }
}
