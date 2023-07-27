using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMBossType
{
    Normal,             //普通
    Special,            //隐藏
}


public class CBossBase : MonoBehaviour
{
    public long nHPMax;
    public long nCurHp;
    public Animator pAnimeCtrl;
    public Transform[] arrHitPoint;
    [Header("Boss弱点")]
    public CBossWeakBase[] pWeaks;
    public DelegateLLFuncCall dlgHPChg;

    public EMBossType emBossType;

    public FSMManager pFSM;

    public enum EMState
    {
        Ready,  //出生准备
        WaitBorn,   //等待出生
        Born,   //出生中
        Idle,   //待机中
        SavePower,  //蓄力中
        WaitAtk,    //等待攻击
        Attack,     //攻击中
        EndAtk,     //结束攻击
        Dead,       //死亡
        Escape,    //逃跑
        OnHit,      //受击

        GameStart,  //游戏开场动画
    }

    public EMState emCurState = EMState.Ready;

    public virtual void Init()
    {
        nCurHp = nHPMax;
    }

    protected virtual void InitFsm()
    {

    }

    void Update()
    {
        if(pFSM!=null)
        {
            pFSM.Update(CTimeMgr.DeltaTime);
        }
    }

    void FixedUpdate()
    {
        if (pFSM != null)
        {
            pFSM.FixedUpdate(CTimeMgr.FixedDeltaTime);
        }
    }

    public void SetState(EMState state, CLocalNetMsg msgValue = null, System.Action callSuc = null)
    {
        if (pFSM == null) return;

        pFSM.ChangeMainState((int)state, msgValue, callSuc);
    }

    /// <summary>
    /// 是否可以被攻击
    /// </summary>
    /// <returns></returns>
    public virtual bool IsAtkAble()
    {
        return true;
    }

    /// <summary>
    /// 获取随机的受击点
    /// </summary>
    /// <returns></returns>
    public virtual Transform GetRandHitPos()
    {
        if (arrHitPoint == null ||
            arrHitPoint.Length <= 0) return null;

        return arrHitPoint[Random.Range(0, arrHitPoint.Length)];
    }

    public virtual void OnHit(string atkUid, long hp)
    {
        if (emCurState == EMState.Dead) return;

        if(hp > nCurHp)
        {
            //统计
            CGameBossMgr.Ins.AddPlayerDmg(atkUid, nCurHp);
        }
        else
        {
            //统计
            CGameBossMgr.Ins.AddPlayerDmg(atkUid, hp);
        }

        nCurHp -= hp;
        if(nCurHp < 0)
        {
            nCurHp = 0;
        }
        dlgHPChg?.Invoke(nCurHp, nHPMax);

        if(nCurHp <= 0)
        {
            SetState(EMState.Dead);
        }
    }

    public CBossWeakBase GetWeak(int idx)
    {
        CBossWeakBase pTarget = null;
        int nIdx = idx - 2;
        if (nIdx < pWeaks.Length &&
           !pWeaks[nIdx].bDead)
        {
            pTarget = pWeaks[nIdx];
        }
        return pTarget;
    }
}
