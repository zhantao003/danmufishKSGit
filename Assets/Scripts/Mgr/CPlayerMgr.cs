using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerMgr : CSingleMgrBase<CPlayerMgr>
{
    public delegate void DelegatePlayerChg(CPlayerBaseInfo player);

    public CPlayerBaseInfo pOwner = null;   //��������

    /// <summary>
    /// ��Ϸ����
    /// </summary>
    public List<CPlayerBaseInfo> listActivePlayers = new List<CPlayerBaseInfo>();
    public Dictionary<string, CPlayerUnit> dicActiveUnits = new Dictionary<string, CPlayerUnit>();  //��Ϸʵ��
    public DelegatePlayerChg dlgActivePlayerChg;

    /// <summary>
    /// �ȴ�����
    /// </summary>
    public List<CPlayerBaseInfo> listIdlePlayers = new List<CPlayerBaseInfo>();
    public Dictionary<string, CPlayerUnit> dicIdleUnits = new Dictionary<string, CPlayerUnit>();  //��Ϸʵ��
    public DelegatePlayerChg dlgIdlePlayerChg;

    /// <summary>
    /// ��������
    /// </summary>
    public Dictionary<string, CPlayerBaseInfo> dicAllHelpers = new Dictionary<string, CPlayerBaseInfo>();

    /// <summary>
    /// �������
    /// </summary>
    public Dictionary<string, CPlayerBaseInfo> dicAllPlayers = new Dictionary<string, CPlayerBaseInfo>();
    public DelegatePlayerChg dlgAllPlayerAdd;
    public DelegatePlayerChg dlgAllPlayerRemove;

    /// <summary>
    /// ������Ϸ
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

            //���ҽ�������
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

                //���ҽ�������
                SortIdleList();

                dlgIdlePlayerChg?.Invoke(info);
                return true;
            }
        }
        
        return false;
    }

    /// <summary>
    /// ������Ϸ
    /// </summary>
    /// <param name="info"></param>
    /// <param name="maxPlayerNum"></param>
    /// <returns></returns>
    public bool JoinGame(CPlayerBaseInfo info, int maxPlayerNum, bool force = false)
    {
        //���ж��ǲ����Ѿ��ڿ�λ������
        if (GetActivePlayer(info.uid) != null) return false;

        //�ж��Ƿ��Ѿ���������
        if (GetActivePlayerNum() >= maxPlayerNum) return false;

        //�ж��ǲ������ŶӶ�����
        if (!force && GetIdlePlayer(info.uid) == null) return false;

        //�ȴ��ŶӶ������Ƴ�
        RemoveIdlePlayer(info);

        //Ȼ�������Ϸ����
        listActivePlayers.Add(info);

        info.emState = CPlayerBaseInfo.EMState.ActiveQueue;
        info.nAddGameCoins = 0;
        info.pGameInfo = new CPlayerGameInfo();
        info.pGameInfo.SetState(CPlayerGameInfo.EMState.Normal);

        ////��ʱ����
        //info.GameCoins = CGameColorFlagMgr.Ins.pStaticConfig.GetInt("��ʱ��ʼ����");

        //info.pGameInfo.Ground = 0;
        //info.pGameInfo.nSlotIdx = listActivePlayers.Count - 1;
        //info.pGameInfo.emColor = (EMMapSlotColor)(info.pGameInfo.nSlotIdx + 1);

        //����Unit
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
    /// ����
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

        Debug.Log($"��ң�{info.userName} ����ֵ��{info.nBattery}");
    }

    /// <summary>
    /// ��ȡ��ǰ����Ϸ�����е��������
    /// </summary>
    /// <returns></returns>
    public int GetActivePlayerNum()
    {
        return listActivePlayers.Count;
    }

    /// <summary>
    /// ��ȡ������Ϸ�����е����
    /// </summary>
    /// <returns></returns>
    public List<CPlayerBaseInfo> GetAllActivePlayers()
    {
        return listActivePlayers;
    }
    
    /// <summary>
    /// �Ƴ�һ����Ϸ�е����
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

        //�ж�һ���Ƿ���Unit
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
    /// ��ȡ��Ϸ�����е����
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
    /// ����������ȡ���
    /// </summary>
    /// <param name="idx"></param>
    /// <returns></returns>
    public CPlayerBaseInfo GetActivePlayerByIdx(int idx)
    {
        if (idx < 0 || idx >= listActivePlayers.Count) return null;

        return listActivePlayers[idx];
    }

    /// <summary>
    /// �����Ϸ�����е����
    /// </summary>
    public void ClearActivePlayer()
    {
        listActivePlayers.Clear();

        dlgActivePlayerChg?.Invoke(null);
    }

    /// <summary>
    /// ��ȡ��ǰ�Ŷӵ��������
    /// </summary>
    /// <returns></returns>
    public int GetIdlePlayerNum()
    {
        return listIdlePlayers.Count;
    }

    /// <summary>
    /// ��ȡ�Ŷӵ����
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
    /// �Ƴ��ŶӶ��е����
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

        //�ж�һ���Ƿ���Unit
        //RemoveIdleUnit(info.uid);

        if (!refresh) return;

        RefreshIdleList();
    }

    /// <summary>
    /// ˢ�´����б��˳λ
    /// </summary>
    public void RefreshIdleList()
    {
        SortIdleList();

        dlgIdlePlayerChg?.Invoke(null);
    }

    /// <summary>
    /// �����������
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
    /// ��ȡ�����Ŷӵ����
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
    /// ��ȡָ��ID���
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
    /// �����Ϸʵ��
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
    /// ��ȡָ����ҵĶ���
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
    /// �������ʵ��
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
    /// �����Ϸʵ��
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
                Debug.Log("�ظ������ID��" + unit.uid);
            }
           
            dicIdleUnits.Remove(unit.uid);
        }

        dicIdleUnits.Add(unit.uid, unit);
    }

    /// <summary>
    /// ��ȡָ����ҵĶ���
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
    /// �������ʵ��
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
    /// ����Ƴ�������
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
    /// ��ȡ���������������
    /// </summary>
    /// <returns></returns>
    public List<CPlayerBaseInfo> GetAllHelpers()
    {
        List<CPlayerBaseInfo> listHelpers = new List<CPlayerBaseInfo>();
        listHelpers.AddRange(dicAllHelpers.Values);

        //����
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
    /// ���������ҵ�ǰ���εĵ����
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
    /// �������������ݣ������ĳ�����
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
