using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBoss104 : CBossBase
{
    public Transform tranBody;
    public float fTimeGameStart;
    public float fTimeBorn;
    public float fTimeSavePower;
    public float fTimeWaitAtk;
    public float fTimeAtkDash;  //攻击冲刺的时间
    public Vector3 vPosReady;
    public Vector3 vPosIdle;
    public UITweenBase uiTweenIdle;
    public UITweenBase uiTweenMat;
    public Vector2 vAtkCDRange;
    public float fAtkRadius = 4.5F;
    public LayerMask lAtkCheck;

    public enum EMAtkPath
    {
        Left = 0,
        Right,

        Max,
    }
    [HideInInspector]
    public EMAtkPath emAtkPath;

    public Vector3 vPosAtkBody;
    public Vector3 vAngelAtkBody;
    public Vector3 vAngelIdleSelf;
    public CEffectOnlyCtrl pEffAtk;
    public CEffectOnlyCtrl pEffDead;
    public Transform[] arrAtkPathL;
    public Transform[] arrAtkPathR;
    public Vector3 vGameStartPos;

    public float fHPLerpDanger;
    public bool bIsDanger;

    public override void Init()
    {
        base.Init();

        uiTweenIdle.Stop();
        pEffAtk.Init();
        pEffAtk.StopEffect();

        InitFsm();

        bIsDanger = false;

        SetState(EMState.Ready);
    }

    protected override void InitFsm()
    {
        pFSM = new FSMManager(this);
        pFSM.AddState((int)EMState.Ready, new CFSMBoss104Ready());
        pFSM.AddState((int)EMState.WaitBorn, new CFSMBoss104WaitBorn());
        pFSM.AddState((int)EMState.Born, new CFSMBoss104Born());
        pFSM.AddState((int)EMState.Idle, new CFSMBoss104Idle());
        pFSM.AddState((int)EMState.SavePower, new CFSMBoss104SavePower());
        pFSM.AddState((int)EMState.WaitAtk, new CFSMBoss104WaitAtk());
        pFSM.AddState((int)EMState.Attack, new CFSMBoss104Atk());
        pFSM.AddState((int)EMState.EndAtk, new CFSMBoss104EndAtk());
        pFSM.AddState((int)EMState.Dead, new CFSMBoss104Dead());
        pFSM.AddState((int)EMState.Escape, new CFSMBoss104Escape());

        pFSM.AddState((int)EMState.GameStart, new CFSMBoss104GameStart());
    }

    public override bool IsAtkAble()
    {
        return emCurState == EMState.Born || emCurState == EMState.Idle;
    }

    public override void OnHit(string atkUid, long hp)
    {
        //Debug.Log("受击血量：" + hp);

        base.OnHit(atkUid, hp);

        if(emCurState != EMState.Dead)
        {
            uiTweenMat.Play();

            //是否进入危机
            if(!bIsDanger &&
               (float)nCurHp / (float)nHPMax <= fHPLerpDanger)
            {
                bIsDanger = true;

                if(emCurState == EMState.Idle)
                {
                    SetState(EMState.SavePower);
                }
            }
        }
    }

    /// <summary>
    /// 获取Boss的攻击路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public Transform[] GetAtkPath(EMAtkPath path)
    {
        if(path == EMAtkPath.Left)
        {
            return arrAtkPathL;
        }
        else if(path == EMAtkPath.Right)
        {
            return arrAtkPathR;
        }

        return null;
    }
}
