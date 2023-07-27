using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameSurvivePlayerInfo
{
    public int nAreaIdx;
    public int nAddHP;  //���ӵ�Ѫ��(����ֵ���)
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
        Ready,      //�ȴ��׶�
        WaitStart,  //׼���׶�
        Gaming,     //��Ϸ�׶�
        Result,     //����׶�
        End,        //��������
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

            //����
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

            //ȡ�����п�λ��Area
            List<CGameSurviveArea> listEmptyArea = new List<CGameSurviveArea>();
            for(int i=0; i<arrArea.Length; i++)
            {
                if(arrArea[i].HasEmptySlot())
                {
                    listEmptyArea.Add(arrArea[i]);
                }
            }

            //û�н��뷿�������������һ������
            List<CPlayerUnit> listPlayerUnits = CPlayerMgr.Ins.GetAllIdleUnit();
            for (int i = 0; i < listPlayerUnits.Count; i++)
            {
                if (!dicPlayerEnterRoom.ContainsKey(listPlayerUnits[i].uid))
                {
                    //��������һ������
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

            //����
            for (int i = 0; i < arrArea.Length; i++)
            {
                arrArea[i].OpenDoor(false);
            }

            dlgRefreshStateTime?.Invoke(pTickerState.CurValue);
        }
        else if(emCurState == EMGameState.Result)
        {
            pNPC.StartAction();

            //����
            for (int i = 0; i < arrArea.Length; i++)
            {
                arrArea[i].OpenDoor(true);
            }
        }
        else if(emCurState == EMGameState.End)
        {
            pTickerState.Value = fTimeGameWait;
            pTickerState.FillTime();

            //�������������뿪����
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

                //�ж��Ƿ��˿�һ����Ϸ
                List<CPlayerUnit> listPlayerUnits = CPlayerMgr.Ins.GetAllIdleUnit();
                if(listPlayerUnits.Count < nGameNeedPlayer)
                {
                    UIToast.Show($"������Ҫ{nGameNeedPlayer}���˿�ʼ��Ϸ");
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

    //��Ҽ��뷿��
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
    /// ��ȡָ��������������
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
    /// ����Ƿ��ڷ�����
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

    //��������
    public Vector3 GetRandomPos()
    {
        return new Vector3(Random.Range(tranMinBoard.position.x, tranMaxBoard.position.x),
                           tranMinBoard.position.y,
                           Random.Range(tranMinBoard.position.z, tranMaxBoard.position.z));
    }

    //�Ƴ����
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
