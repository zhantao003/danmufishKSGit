using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFishRecordMgr : CSingleMgrBase<CFishRecordMgr>
{
    public Dictionary<string, CFishRecordInfo> dicRecords = new Dictionary<string, CFishRecordInfo>();

    public void ClearAllCoin()
    {
        foreach(var recordInfo in dicRecords)
        {
            recordInfo.Value.nTotalCoin = 0;
        }
    }

    public void AddRecord(long fishCoin, CPlayerBaseInfo player)
    {
        if (player == null) return;
        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle &&
            CBattleModeMgr.Ins != null &&
            CBattleModeMgr.Ins.emCurState != CBattleModeMgr.EMGameState.Gaming)
        {
            return;
        }
        CFishRecordInfo pInfo = GetRecord(player.uid);
        if (pInfo == null)
        {
            pInfo = new CFishRecordInfo(player);
            dicRecords.Add(player.uid, pInfo);
        }
        //更新玩家本场收益
        pInfo.nTotalCoin += fishCoin;
        UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
        if (roomInfo != null)
        {
            roomInfo.profitRoot.AddPlayerInfo(pInfo);
        }
    }

    public void AddRecord(CFishInfo fish, CPlayerBaseInfo player)
    {
        if (player == null) return;
        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle &&
            CBattleModeMgr.Ins != null &&
            CBattleModeMgr.Ins.emCurState != CBattleModeMgr.EMGameState.Gaming)
        {
            return;
        }
        CFishRecordInfo pInfo = GetRecord(player.uid);
        if(pInfo == null)
        {
            pInfo = new CFishRecordInfo(player);
            dicRecords.Add(player.uid, pInfo);
        }

        //更新玩家本场收益
        pInfo.nTotalCoin += fish.lPrice;

        //记录鱼的信息
        CFishRecordSlot pSlot = pInfo.GetRecord(fish.nTBID);
        if(pSlot == null)
        {
            pSlot = new CFishRecordSlot();
            pSlot.nFishId = fish.nTBID;
            pSlot.nCount = 1;
            pSlot.fMaxSize = fish.fCurSize;
            pSlot.fMinSize = fish.fCurSize;
            pInfo.AddRecord(pSlot);
        }
        else
        {
            pSlot.nCount += 1;

            //刷新最大尺寸
            if (fish.fCurSize > pSlot.fMaxSize)
            {
                pSlot.fMaxSize = fish.fCurSize;
            }

            //刷新最小尺寸
            if (fish.fCurSize < pSlot.fMinSize)
            {
                pSlot.fMinSize = fish.fCurSize;
            }
        }

        UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
        if(roomInfo != null)
        {
            roomInfo.profitRoot.AddPlayerInfo(pInfo);
        }
    }

    /// <summary>
    /// 获取指定玩家的记录
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    public CFishRecordInfo GetRecord(string uid)
    {
        CFishRecordInfo pRes = null;
        if(dicRecords.TryGetValue(uid, out pRes))
        {

        }

        return pRes;
    }

    /// <summary>
    /// 获取所有记录
    /// </summary>
    /// <returns></returns>
    public List<CFishRecordInfo> GetAllRecords()
    {
        List<CFishRecordInfo> listInfos = new List<CFishRecordInfo>();
        listInfos.AddRange(dicRecords.Values);

        return listInfos;
    }

    public void Clear()
    {
        dicRecords.Clear();
    }
}
