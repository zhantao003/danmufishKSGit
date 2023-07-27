using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameSurvivePlayerInfo
{
    public int nAreaIdx;
    public int nAddHP;  //增加的血量(贡献值标的)
    public CPlayerUnit pPlayer;
}

public class CGameSurviveMap : MonoBehaviour
{
    public static CGameSurviveMap Ins
    {
        get
        {
            if(ins == null)
            {
                ins = FindObjectOfType<CGameSurviveMap>();
            }

            return ins;
        }
    }
    static CGameSurviveMap ins = null;

    public Transform tranMinBoard;
    public Transform tranMaxBoard;
    public Vector3 vPosIdle;

    public CGameSurviveNPC pNPC;
    public CGameSurviveArea[] arrArea;
    public enum EMGameState
    {
        Ready,      //等待阶段
        WaitStart,  //准备阶段
        Gaming,     //游戏阶段
        Result,     //结算阶段
        End,        //结束动画
    }

    public EMGameState emCurState = EMGameState.Ready;

    public float fTimeGameReady = 100f;
    public float fTimeGameWait = 5f;
    public float fTimeGameSurvive = 250f;

    public int nGameNeedPlayer = 10;

    public CPropertyTimer pTickerState = new CPropertyTimer();
    public DelegateFFuncCall dlgRefreshStateTime;
    public DelegateIFuncCall dlgRefrehsState;

    Dictionary<string, CGameSurvivePlayerInfo> dicPlayerEnterRoom = new Dictionary<string, CGameSurvivePlayerInfo>();

    // Start is called before the first frame update
    void Start()
    {
        SetState(EMGameState.Ready);
    }

    public void SetState(EMGameState state)
    {
        emCurState = state;

        dlgRefrehsState?.Invoke((int)state);

        if (emCurState == EMGameState.Ready)
        {
            pTickerState.Value = fTimeGameReady;
            pTickerState.FillTime();

            //开门
            for (int i=0; i<arrArea.Length; i++)
            {
                arrArea[i].OpenDoor(true);
            }

            dlgRefreshStateTime?.Invoke(pTickerState.CurValue);
        }
        else if(emCurState == EMGameState.WaitStart)
        {
            pTickerState.Value = fTimeGameWait;
            pTickerState.FillTime();

            //取出还有空位的Area
            List<CGameSurviveArea> listEmptyArea = new List<CGameSurviveArea>();
            for(int i=0; i<arrArea.Length; i++)
            {
                if(arrArea[i].HasEmptySlot())
                {
                    listEmptyArea.Add(arrArea[i]);
                }
            }

            //没有进入房间的玩家随机进入一个房间
            List<CPlayerUnit> listPlayerUnits = CPlayerMgr.Ins.GetAllIdleUnit();
            for (int i = 0; i < listPlayerUnits.Count; i++)
            {
                if (!dicPlayerEnterRoom.ContainsKey(listPlayerUnits[i].uid))
                {
                    //随机给玩家一个房间
                    int nRandAreaIdx = Random.Range(0, listEmptyArea.Count);
                    CGameSurviveArea pRandArea = listEmptyArea[nRandAreaIdx];
                    JoinRoom(listPlayerUnits[i], pRandArea.nIdx);

                    if(!pRandArea.HasEmptySlot())
                    {
                        listEmptyArea.RemoveAt(nRandAreaIdx);
                    }
                }
            }
        }
        else if(emCurState == EMGameState.Gaming)
        {
            pTickerState.Value = fTimeGameSurvive;
            pTickerState.FillTime();

            //关门
            for (int i = 0; i < arrArea.Length; i++)
            {
                arrArea[i].OpenDoor(false);
            }

            dlgRefreshStateTime?.Invoke(pTickerState.CurValue);
        }
        else if(emCurState == EMGameState.Result)
        {
            pNPC.StartAction();

            //开门
            for (int i = 0; i < arrArea.Length; i++)
            {
                arrArea[i].OpenDoor(true);
            }
        }
        else if(emCurState == EMGameState.End)
        {
            pTickerState.Value = fTimeGameWait;
            pTickerState.FillTime();

            //所有生存的玩家离开房间
            List<CPlayerUnit> listPlayerUnits = CPlayerMgr.Ins.GetAllIdleUnit();
            for(int i=0; i<listPlayerUnits.Count; i++)
            {
                CMapSlot pMapSlot = listPlayerUnits[i].pMapSlot;
                if (pMapSlot == null) continue;

                CBoatMoveCtrl pCtrl = pMapSlot.gameObject.GetComponent<CBoatMoveCtrl>();
                if (pCtrl == null) continue;

                CGameSurvivePlayerInfo pInfo = null;
                if(dicPlayerEnterRoom.TryGetValue(listPlayerUnits[i].uid, out pInfo))
                {
                    pCtrl.SetMovePath(new Vector3[] { arrArea[pInfo.nAreaIdx].tranDoor.position,
                                                      GetRandomPos() });
                }
            }

            ResetInfo();
        }
    }

    private void Update()
    {
        if(emCurState == EMGameState.Ready)
        {
            if (pTickerState.Tick(CTimeMgr.DeltaTime))
            {
                dlgRefreshStateTime?.Invoke(0);

                //判断是否够人开一局游戏
                List<CPlayerUnit> listPlayerUnits = CPlayerMgr.Ins.GetAllIdleUnit();
                if(listPlayerUnits.Count < nGameNeedPlayer)
                {
                    UIToast.Show($"至少需要{nGameNeedPlayer}个人开始游戏");
                    SetState(EMGameState.Ready);
                }
                else
                {
                    SetState(EMGameState.WaitStart);
                }  
            }
            else
            {
                dlgRefreshStateTime?.Invoke(pTickerState.CurValue);
            }
        }
        else if(emCurState == EMGameState.WaitStart)
        {
            if(pTickerState.Tick(CTimeMgr.DeltaTime))
            {
                SetState(EMGameState.Gaming);
            }
        }
        else if(emCurState == EMGameState.Gaming)
        {
            if (pTickerState.Tick(CTimeMgr.DeltaTime))
            {
                dlgRefreshStateTime?.Invoke(0);
                SetState(EMGameState.Result);
            }
            else
            {
                dlgRefreshStateTime?.Invoke(pTickerState.CurValue);
            }
        }
        else if(emCurState == EMGameState.Result)
        {

        }
        else if(emCurState == EMGameState.End)
        {
            if (pTickerState.Tick(CTimeMgr.DeltaTime))
            {
                SetState(EMGameState.Ready);
            }
        }
    }

    public CGameSurviveArea GetAreaByIdx(int idx)
    {
        if (idx < 0 || idx >= arrArea.Length) return null;

        return arrArea[idx];
    }

    //玩家加入房间
    public void JoinRoom(CPlayerUnit player, int idx)
    {
        if (IsPlayerInRoom(player.uid)) return;

        CGameSurviveArea pArea = GetAreaByIdx(idx);
        if (pArea == null) return;

        if(pArea.AddPlayerUnit(player))
        {
            dicPlayerEnterRoom.Add(player.uid, new CGameSurvivePlayerInfo() { 
                nAreaIdx = idx,
                nAddHP = 0,
                pPlayer = player
            });
        }
    }

    /// <summary>
    /// 获取指定房间的所有玩家
    /// </summary>
    public List<CGameSurvivePlayerInfo> GetAllPlayerInArea(int idx)
    {
        List<CGameSurvivePlayerInfo> listInfos = new List<CGameSurvivePlayerInfo>();
        foreach (CGameSurvivePlayerInfo info in dicPlayerEnterRoom.Values)
        {
            if(info.nAreaIdx == idx)
            {
                listInfos.Add(info);
            }
        }

        return listInfos;
    }

    /// <summary>
    /// 玩家是否在房间里
    /// </summary>
    public bool IsPlayerInRoom(string uid)
    {
        return dicPlayerEnterRoom.ContainsKey(uid);
    }

    public void ResetInfo()
    {
        dicPlayerEnterRoom.Clear();
        for(int i=0; i<arrArea.Length; i++)
        {
            arrArea[i].Reset();
        }
    }

    //获得随机点
    public Vector3 GetRandomPos()
    {
        return new Vector3(Random.Range(tranMinBoard.position.x, tranMaxBoard.position.x),
                           tranMinBoard.position.y,
                           Random.Range(tranMinBoard.position.z, tranMaxBoard.position.z));
    }

    //移除玩家
    public void RemovePlayerInRoom(string uid)
    {
        if(dicPlayerEnterRoom.ContainsKey(uid))
        {
            int nAreaIdx = dicPlayerEnterRoom[uid].nAreaIdx;
            CGameSurviveArea pArea = arrArea[nAreaIdx];
            pArea.RemovePlayer(uid);

            dicPlayerEnterRoom.Remove(uid);
        }
    }
}
