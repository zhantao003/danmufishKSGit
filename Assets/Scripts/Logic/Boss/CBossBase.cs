using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMBossType
{
    Normal,             //��ͨ
    Special,            //����
}


public class CBossBase : MonoBehaviour
{
    public long nHPMax;
    public long nCurHp;
    public Animator pAnimeCtrl;
    public Transform[] arrHitPoint;
    [Header("Boss����")]
    public CBossWeakBase[] pWeaks;
    public DelegateLLFuncCall dlgHPChg;

    public EMBossType emBossType;

    public FSMManager pFSM;

    public enum EMState
    {
        Ready,  //����׼��
        WaitBorn,   //�ȴ�����
        Born,   //������
        Idle,   //������
        SavePower,  //������
        WaitAtk,    //�ȴ�����
        Attack,     //������
        EndAtk,     //��������
        Dead,       //����
        Escape,    //����
        OnHit,      //�ܻ�

        GameStart,  //��Ϸ��������
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
    /// �Ƿ���Ա�����
    /// </summary>
    /// <returns></returns>
    public virtual bool IsAtkAble()
    {
        return true;
    }

    /// <summary>
    /// ��ȡ������ܻ���
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
            //ͳ��
            CGameBossMgr.Ins.AddPlayerDmg(atkUid, nCurHp);
        }
        else
        {
            //ͳ��
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
