using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBoss304 : CBossBase
{
    public Transform tranBody;
    public float fTimeShow = 1.5f;
    public float fTimeGameStart;
    public float fTimeBorn;
    public float fTimeSavePower;
    public float fTimeWaitAtk;
    public float fTimeAtkDash;
    public Vector3 vPosReady;
    public Vector3 vPosIdle;
    public Vector3 vPosExit;
    public UITweenBase uiTweenIdle;
    public UITweenBase uiTweenMat;
    public Vector2 vAtkCDRange;
    public float fAtkRadius = 4.5F;
    public LayerMask lAtkCheck;
    [Header("普通攻击路径")]
    public CBoss304AtkPath[] pNormalAtkPaths;
    [Header("攻击检测点")]
    public Transform tranCheckHitPos;
    [HideInInspector]
    public List<CBoss304AtkPath> listAtkPaths = new List<CBoss304AtkPath>();

    public Vector2Int vNormalAtkCount = new Vector2Int(2, 4);

    public GameObject pEffAtk;
    public CEffectOnlyCtrl pEffDead;
    public Vector3 vGameStartPos;
    public Vector3 vAngelAtkBody;
    public Vector3 vAngelIdleSelf;

    public float fHPLerpDanger;
    public bool bIsDanger;

    public string szAlertRangeEffect;

    public CBossWeakBase GetRandWeak()
    {
        CBossWeakBase pTarget = null;
        List<int> listWeakIdxs = new List<int>();
        for (int i = 0; i < pWeaks.Length; i++)
        {
            if (!pWeaks[i].bDead)
            {
                listWeakIdxs.Add(i);
            } 
        }
        int nRandomIdx = 0;
        if (listWeakIdxs.Count > 0)
        {
            if(listWeakIdxs.Count > 1)
            {
                nRandomIdx = listWeakIdxs[Random.Range(0, listWeakIdxs.Count)];
            }
            else
            {
                nRandomIdx = listWeakIdxs[0];
            }
            pTarget = pWeaks[nRandomIdx];
        }
        
        return pTarget;
    }

    /// <summary>
    /// 检测弱点是否全被击破
    /// </summary>
    public void CheckWeak()
    {
        bool bWeakDead = true;
        for(int i = 0;i < pWeaks.Length;i++)
        {
            if(!pWeaks[i].bDead)
            {
                bWeakDead = false;
                break;
            }
        }
        if(bWeakDead)
        {

        }
        UIRoomBossInfo roomBossInfo = UIManager.Instance.GetUI(UIResType.RoomBossInfo) as UIRoomBossInfo;
        if (roomBossInfo != null)
        {
            roomBossInfo.RefreshDropRate();
        }
    }

    public int GetWeakAddRate()
    {
        int nAddRate = 0;
        for (int i = 0; i < pWeaks.Length; i++)
        {
            if (pWeaks[i].bDead)
            {
                nAddRate += pWeaks[i].nAddRate;
            }
        }
        return nAddRate;
    }


    public override void Init()
    {
        base.Init();
        for(int i = 0;i < pWeaks.Length;i++)
        {
            pWeaks[i].Init();
        }

        uiTweenIdle.Stop();
        if (pEffAtk != null)
        {
            pEffAtk.SetActive(false);
        }
        InitFsm();

        bIsDanger = false;

        SetState(EMState.Ready);
    }
     
    protected override void InitFsm()
    {
        pFSM = new FSMManager(this);
        pFSM.AddState((int)EMState.Ready, new CFSMBoss304Ready());
        pFSM.AddState((int)EMState.WaitBorn, new CFSMBoss304WaitBorn());
        pFSM.AddState((int)EMState.Born, new CFSMBoss304Born());
        pFSM.AddState((int)EMState.Idle, new CFSMBoss304Idle());
        pFSM.AddState((int)EMState.SavePower, new CFSMBoss304SavePower());
        pFSM.AddState((int)EMState.WaitAtk, new CFSMBoss304WaitAtk());
        pFSM.AddState((int)EMState.Attack, new CFSMBoss304Atk());
        pFSM.AddState((int)EMState.EndAtk, new CFSMBoss304EndAtk());
        pFSM.AddState((int)EMState.Dead, new CFSMBoss304Dead());
        pFSM.AddState((int)EMState.Escape, new CFSMBoss304Escape());
        pFSM.AddState((int)EMState.OnHit, new CFSMBoss304OnHit());

        pFSM.AddState((int)EMState.GameStart, new CFSMBoss304GameStart());
    }


    public override bool IsAtkAble()
    {
        return emCurState == EMState.Born || emCurState == EMState.Idle;
    }

    public override void OnHit(string atkUid, long hp)
    {
        //Debug.Log("受击血量：" + hp);

        base.OnHit(atkUid, hp);

        if (emCurState != EMState.Dead)
        {
            if (uiTweenMat != null)
            {
                uiTweenMat.Play();
            }
            //是否进入危机
            if (!bIsDanger &&
               (float)nCurHp / (float)nHPMax <= fHPLerpDanger)
            {
                bIsDanger = true;

                if (emCurState == EMState.Idle)
                {
                    SetState(EMState.SavePower);
                }
            }
        }
    }

    /// <summary>
    /// 获取普攻路径
    /// </summary>
    public void GetAtkPathByNormal()
    {
        listAtkPaths.Clear();
        int nRandomIdx = 0;
        int nAtkCount = Random.Range(vNormalAtkCount.x, vNormalAtkCount.y + 1);
        for (int i = 0; i < pWeaks.Length; i++)
        {
            if (pWeaks[i].bDead)
            {
                nAtkCount += pWeaks[i].nAddAtkCount;
            }
        }
        List<CBoss304AtkPath> listRandomPaths = new List<CBoss304AtkPath>();
        listRandomPaths.AddRange(pNormalAtkPaths);
        for (int i = 0;i < nAtkCount;i++)
        {
            if (listRandomPaths.Count <= 0)
                break;
            nRandomIdx = Random.Range(0, listRandomPaths.Count);
            listAtkPaths.Add(listRandomPaths[nRandomIdx]);
            listRandomPaths.RemoveAt(nRandomIdx);
        }
    }


}
