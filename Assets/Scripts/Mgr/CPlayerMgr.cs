using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerMgr : CSingleMgrBase<CPlayerMgr>
{
    public delegate void DelegatePlayerChg(CPlayerBaseInfo player);

    public CPlayerBaseInfo pOwner = null;   //主播本人

    /// <summary>
    /// 游戏队列
    /// </summary>
    public List<CPlayerBaseInfo> listActivePlayers = new List<CPlayerBaseInfo>();
    public Dictionary<string, CPlayerUnit> dicActiveUnits = new Dictionary<string, CPlayerUnit>();  //游戏实体
    public DelegatePlayerChg dlgActivePlayerChg;

    /// <summary>
    /// 等待队列
    /// </summary>
    public List<CPlayerBaseInfo> listIdlePlayers = new List<CPlayerBaseInfo>();
    public Dictionary<string, CPlayerUnit> dicIdleUnits = new Dictionary<string, CPlayerUnit>();  //游戏实体
    public DelegatePlayerChg dlgIdlePlayerChg;

    /// <summary>
    /// 助力队列
    /// </summary>
    public Dictionary<string, CPlayerBaseInfo> dicAllHelpers = new Dictionary<string, CPlayerBaseInfo>();

    /// <summary>
    /// 所有玩家
    /// </summary>
    public Dictionary<string, CPlayerBaseInfo> dicAllPlayers = new Dictionary<string, CPlayerBaseInfo>();
    public DelegatePlayerChg dlgAllPlayerAdd;
    public DelegatePlayerChg dlgAllPlayerRemove;

    /// <summary>
    /// 加入游戏
    /// </summary>
    /// <param name="info"></param>
    public bool JoinQueue(CPlayerBaseInfo info, int maxIdleNum)
    {
        if (GetIdlePlayer(info.uid) != null) return false;

        if (GetIdlePlayerNum() < maxIdleNum)
        {
            listIdlePlayers.Add(info);

            info.emState = CPlayerBaseInfo.EMState.IdleQueue;
            info.pGameInfo = null;

            //代币降序排序
            SortIdleList();

            dlgIdlePlayerChg?.Invoke(info);
            return true;
        }
        else
        {
            if(info.guardLevel > listIdlePlayers[listIdlePlayers.Count - 1].guardLevel)
            {
                CPlayerBaseInfo pRemovePlayer = listIdlePlayers[listIdlePlayers.Count - 1];
                RemoveIdleUnit(pRemovePlayer.uid);
                RemoveIdlePlayer(pRemovePlayer, false);

                listIdlePlayers.Add(info);

                info.emState = CPlayerBaseInfo.EMState.IdleQueue;
                info.pGameInfo = null;

                //代币降序排序
                SortIdleList();

                dlgIdlePlayerChg?.Invoke(info);
                return true;
            }
        }
        
        return false;
    }

    /// <summary>
    /// 加入游戏
    /// </summary>
    /// <param name="info"></param>
    /// <param name="maxPlayerNum"></param>
    /// <returns></returns>
    public bool JoinGame(CPlayerBaseInfo info, int maxPlayerNum, bool force = false)
    {
        //先判断是不是已经在空位里面了
        if (GetActivePlayer(info.uid) != null) return false;

        //判断是否已经到达上限
        if (GetActivePlayerNum() >= maxPlayerNum) return false;

        //判断是不是在排队队列中
        if (!force && GetIdlePlayer(info.uid) == null) return false;

        //先从排队队列中移除
        RemoveIdlePlayer(info);

        //然后加入游戏队列
        listActivePlayers.Add(info);

        info.emState = CPlayerBaseInfo.EMState.ActiveQueue;
        info.nAddGameCoins = 0;
        info.pGameInfo = new CPlayerGameInfo();
        info.pGameInfo.SetState(CPlayerGameInfo.EMState.Normal);

        ////临时处理
        //info.GameCoins = CGameColorFlagMgr.Ins.pStaticConfig.GetInt("临时初始代币");

        //info.pGameInfo.Ground = 0;
        //info.pGameInfo.nSlotIdx = listActivePlayers.Count - 1;
        //info.pGameInfo.emColor = (EMMapSlotColor)(info.pGameInfo.nSlotIdx + 1);

        //处理Unit
        CPlayerUnit pUnit = GetIdleUnit(info.uid);
        if (pUnit != null)
        {
            RemoveIdleUnit(info.uid, false);

            pUnit.tranSelf.SetParent(null);
            AddActiveUnit(pUnit);
        }

        dlgActivePlayerChg?.Invoke(info);

        return true;
    }

    /// <summary>
    /// 助力
    /// </summary>
    public void SendBattery(CPlayerBaseInfo info, long nBattery)
    {
        if(!dicAllHelpers.ContainsKey(info.uid))
        {
            dicAllHelpers.Add(info.uid, info);
        }
        else
        {
            dicAllHelpers[info.uid] = info;
        }

        info.nBattery += nBattery;

        Debug.Log($"玩家：{info.userName} 助力值：{info.nBattery}");
    }

    /// <summary>
    /// 获取当前在游戏队列中的玩家数量
    /// </summary>
    /// <returns></returns>
    public int GetActivePlayerNum()
    {
        return listActivePlayers.Count;
    }

    /// <summary>
    /// 获取所有游戏队列中的玩家
    /// </summary>
    /// <returns></returns>
    public List<CPlayerBaseInfo> GetAllActivePlayers()
    {
        return listActivePlayers;
    }
    
    /// <summary>
    /// 移除一个游戏中的玩家
    /// </summary>
    /// <param name="info"></param>
    /// <param name="refresh"></param>
    public void RemoveActivePlayer(CPlayerBaseInfo info, bool refresh = true)
    {
        for(int i=0; i<listActivePlayers.Count; i++)
        {
            if (listActivePlayers[i].uid != info.uid) continue;

            listActivePlayers[i].emState = CPlayerBaseInfo.EMState.None;
            listActivePlayers[i].pGameInfo = null;

            listActivePlayers.RemoveAt(i);
            break;
        }

        RemoveIdleUnit(info.uid);

        //判断一下是否有Unit
        //RemoveActiveUnit(info.uid);

        if (!refresh) return;

        //for (int i = 0; i < listActivePlayers.Count; i++)
        //{
        //    listActivePlayers[i].pGameInfo.nSlotIdx = i;
        //    listActivePlayers[i].pGameInfo.emColor = (EMMapSlotColor)(i + 1);
        //}

        dlgActivePlayerChg?.Invoke(info);
    }

    /// <summary>
    /// 获取游戏队列中的玩家
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public CPlayerBaseInfo GetActivePlayer(string id)
    {
        for(int i=0; i<listActivePlayers.Count; i++)
        {
            if(listActivePlayers[i].uid.Equals(id))
            {
                return listActivePlayers[i];
            }
        }

        return null ;
    }

    /// <summary>
    /// 按照索引获取玩家
    /// </summary>
    /// <param name="idx"></param>
    /// <returns></returns>
    public CPlayerBaseInfo GetActivePlayerByIdx(int idx)
    {
        if (idx < 0 || idx >= listActivePlayers.Count) return null;

        return listActivePlayers[idx];
    }

    /// <summary>
    /// 清空游戏队列中的玩家
    /// </summary>
    public void ClearActivePlayer()
    {
        listActivePlayers.Clear();

        dlgActivePlayerChg?.Invoke(null);
    }

    /// <summary>
    /// 获取当前排队的玩家总数
    /// </summary>
    /// <returns></returns>
    public int GetIdlePlayerNum()
    {
        return listIdlePlayers.Count;
    }

    /// <summary>
    /// 获取排队的玩家
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public CPlayerBaseInfo GetIdlePlayer(string id)
    {
        for (int i = 0; i < listIdlePlayers.Count; i++)
        {
            if (listIdlePlayers[i].uid.Equals(id))
            {
                return listIdlePlayers[i];
            }
        }

        return null;
    }

    /// <summary>
    /// 移除排队队列的玩家
    /// </summary>
    /// <param name="info"></param>
    /// <param name="refresh"></param>
    public void RemoveIdlePlayer(CPlayerBaseInfo info, bool refresh = true)
    {
        for (int i = 0; i < listIdlePlayers.Count; i++)
        {
            if (listIdlePlayers[i].uid != info.uid) continue;

            listIdlePlayers[i].emState = CPlayerBaseInfo.EMState.None;
            listIdlePlayers[i].pGameInfo = null;

            listIdlePlayers.RemoveAt(i);
            break;
        }

        //判断一下是否有Unit
        //RemoveIdleUnit(info.uid);

        if (!refresh) return;

        RefreshIdleList();
    }

    /// <summary>
    /// 刷新待机列表的顺位
    /// </summary>
    public void RefreshIdleList()
    {
        SortIdleList();

        dlgIdlePlayerChg?.Invoke(null);
    }

    /// <summary>
    /// 排序待机队伍
    /// </summary>
    protected void SortIdleList()
    {
        listIdlePlayers.Sort((x, y) =>
        {
            if(x.guardLevel < y.guardLevel)
            {
                return 1;
            }
            else if(x.guardLevel == y.guardLevel)
            {
                return 0;

                //if (x.GameCoins < y.GameCoins)
                //{
                //    return 1;
                //}
                //else if (x.GameCoins == y.GameCoins)
                //{
                //    return 0;
                //}
                //else
                //{
                //    return -1;
                //}
            }
            else
            {
                return -1;
            }
        });
    }

    /// <summary>
    /// 获取所有排队的玩家
    /// </summary>
    /// <returns></returns>
    public List<CPlayerBaseInfo> GetAllIdlePlayers()
    {
        return listIdlePlayers;
    }

    public void ClearIdlePlayers()
    {
        listIdlePlayers.Clear();

        dlgIdlePlayerChg?.Invoke(null);
    }

    public void AddPlayer(CPlayerBaseInfo player)
    {
        dicAllPlayers.Add(player.uid, player);

        //dlgAllPlayerAdd?.Invoke(player);
    }

    public void InitPlayerInfo(string uid)
    {
        CPlayerBaseInfo player = GetPlayer(uid);
        if (player == null) return;

        dlgAllPlayerAdd?.Invoke(player);
    }

    public void RemovePlayer(string uid)
    {
        if(dicAllPlayers.ContainsKey(uid))
        {
            dlgAllPlayerRemove?.Invoke(dicAllPlayers[uid]);
            dicAllPlayers.Remove(uid);
        }
    }

    /// <summary>
    /// 获取指定ID玩家
    /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    public CPlayerBaseInfo GetPlayer(string id)
    {
        CPlayerBaseInfo pInfo = null;
        if (id != null && 
            dicAllPlayers.TryGetValue(id, out pInfo))
        {

        }

        return pInfo;
    }

    /// <summary>
    /// 添加游戏实体
    /// </summary>
    /// <param name="unit"></param>
    public void AddActiveUnit(CPlayerUnit unit)
    {
        if(dicActiveUnits.ContainsKey(unit.uid))
        {
            dicActiveUnits[unit.uid].Recycle();
            GameObject.Destroy(dicActiveUnits[unit.uid].gameObject);
            dicActiveUnits.Remove(unit.uid);
        }

        dicActiveUnits.Add(unit.uid, unit);
    }

    /// <summary>
    /// 获取指定玩家的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public CPlayerUnit GetActiveUnit(string id)
    {
        CPlayerUnit pUnit = null;
        if(!dicActiveUnits.TryGetValue(id, out pUnit))
        {
            return null;
        }

        return pUnit;
    }

    public void RemoveActiveUnit(string id, bool destroy = true)
    {
        CPlayerUnit pUnit = null;
        if (dicActiveUnits.TryGetValue(id, out pUnit))
        {
            dicActiveUnits.Remove(id);

            if(destroy)
            {
                pUnit.Recycle();
                GameObject.Destroy(pUnit.gameObject);
            }
        }
    }

    /// <summary>
    /// 清空所有实体
    /// </summary>
    public void ClearAllActiveUnit()
    {
        foreach(CPlayerUnit unit in dicActiveUnits.Values)
        {
            unit.Recycle();
            GameObject.Destroy(unit.gameObject);
        }

        dicActiveUnits.Clear();
    }

    /// <summary>
    /// 添加游戏实体
    /// </summary>
    /// <param name="unit"></param>
    public void AddIdleUnit(CPlayerUnit unit)
    {
        if (dicIdleUnits.ContainsKey(unit.uid))
        {
            if(dicIdleUnits[unit.uid] != null)
            {
                GameObject.Destroy(dicIdleUnits[unit.uid].gameObject);
            }
            else
            {
                Debug.Log("重复的玩家ID：" + unit.uid);
            }
           
            dicIdleUnits.Remove(unit.uid);
        }

        dicIdleUnits.Add(unit.uid, unit);
    }

    /// <summary>
    /// 获取指定玩家的对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public CPlayerUnit GetIdleUnit(string id)
    {
        CPlayerUnit pUnit = null;
        if (!dicIdleUnits.TryGetValue(id, out pUnit))
        {
            return null;
        }

        return pUnit;
    }

    public List<CPlayerUnit> GetAllIdleUnit(string uid = "")
    {
        List<CPlayerUnit> playerUnits = new List<CPlayerUnit>();
        foreach (CPlayerUnit unit in dicIdleUnits.Values)
        {
            if (!string.IsNullOrEmpty(uid) && unit.uid.Equals(uid))
                continue;
            playerUnits.Add(unit);
        }
        return playerUnits;
    }

    /// <summary>
    /// 清空所有实体
    /// </summary>
    public void ClearAllIdleUnit()
    {
        foreach (CPlayerUnit unit in dicIdleUnits.Values)
        {
            unit.Recycle();
            GameObject.Destroy(unit.gameObject);
        }

        dicIdleUnits.Clear();
    }

    public void RemoveIdleUnit(string id, bool destroy = true)
    {
        CPlayerUnit pUnit = null;
        if (dicIdleUnits.TryGetValue(id, out pUnit))
        {
            dicIdleUnits.Remove(id);

            if (destroy)
            {
                pUnit.Recycle();
                GameObject.Destroy(pUnit.gameObject);
            }
        }
        UIRoomBossInfo roomBossInfo = UIManager.Instance.GetUI(UIResType.RoomBossInfo) as UIRoomBossInfo;
        if (roomBossInfo != null)
        {
            roomBossInfo.RefreshDropRate();
        }
    }

    /// <summary>
    /// 随机移除机器人
    /// </summary>
    public void RemoveRobot()
    {
        string uid = "";
        foreach(CPlayerUnit Unit in dicIdleUnits.Values)
        {
            if(Unit.pInfo.bIsRobot)
            {
                uid = Unit.pInfo.uid;
                break;
            }
        }
        CPlayerBaseInfo pActivePlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pActivePlayer != null)
        {
            CPlayerMgr.Ins.RemoveActivePlayer(pActivePlayer);
            if (pActivePlayer.emUserType == CPlayerBaseInfo.EMUserType.Guanzhong)
            {
                CPlayerMgr.Ins.RemovePlayer(uid);
            }
        }
    }

    /// <summary>
    /// 获取所有助力过的玩家
    /// </summary>
    /// <returns></returns>
    public List<CPlayerBaseInfo> GetAllHelpers()
    {
        List<CPlayerBaseInfo> listHelpers = new List<CPlayerBaseInfo>();
        listHelpers.AddRange(dicAllHelpers.Values);

        //排序
        listHelpers.Sort((x, y) =>
        {
            if (x.nBattery < y.nBattery)
            {
                return 1;
            }
            else if (x.nBattery == y.nBattery)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        });

        return listHelpers;
    }

    /// <summary>
    /// 清除所有玩家当前场次的电池数
    /// </summary>
    public void ClearAllPlayersBattery()
    {
        foreach(CPlayerBaseInfo player in dicAllPlayers.Values)
        {
            player.nBattery = 0;
        }

        dicAllHelpers.Clear();
    }

    /// <summary>
    /// 清空所有玩家数据（主播的除开）
    /// </summary>
    public void ClearAllPlayerInfos()
    {
        List<string> listPlayerUID = new List<string>();
        foreach (CPlayerBaseInfo player in dicAllPlayers.Values)
        {
            if(player.emUserType == CPlayerBaseInfo.EMUserType.Guanzhong)
            {
                listPlayerUID.Add(player.uid);
            }
        }
        for(int i = 0;i < listPlayerUID.Count;i++)
        {
            dicAllPlayers.Remove(listPlayerUID[i]);
        }
    }

    public void ClearAll()
    {
        ClearAllActiveUnit();
        ClearAllIdleUnit();

        ClearActivePlayer();
        ClearIdlePlayers();

        dicAllPlayers.Clear();
    }
}
