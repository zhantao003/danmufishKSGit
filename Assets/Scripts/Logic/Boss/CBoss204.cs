using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CTentacleShowPos
{
    public Transform tranShowPos;
    public Transform tranAtkPos;
}

public class CBoss204 : CBossBase
{
    public Transform tranBody;
    public float fTimeShow = 1.5f;
    public float fTimeGameStart;
    public float fTimeBorn;
    public float fTimeSavePower;
    public float fTimeWaitAtk;
    public float fTimeNormalAtk;  //��ͨ����״̬ʱ��
    public float fTimeWaitTentacle;     //�ȴ����ֹ������ʱ��
    public Vector3 vPosReady;
    public Vector3 vPosIdle;
    public UITweenBase uiTweenIdle;
    public UITweenBase uiTweenMat;
    public Vector2 vAtkCDRange;
    public float fAtkRadius = 4.5F;
    public LayerMask lAtkCheck;
    [Header("��ʼ������")]
    public CTentacleShowPos[] tranStartHitPos;
    [Header("��ͨ������")]
    public CTentacleShowPos[] tranAtkHitPos;

    public Vector2Int  vNormalAtkCount = new Vector2Int(2, 4);

    public CEffectOnlyCtrl pEffAtk;
    public CEffectOnlyCtrl pEffDead;
    public Vector3 vGameStartPos;

    public float fHPLerpDanger;
    public bool bIsDanger;

    public string szAlertRangeEffect;

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
        pFSM.AddState((int)EMState.Ready, new CFSMBoss204Ready());
        pFSM.AddState((int)EMState.WaitBorn, new CFSMBoss204WaitBorn());
        pFSM.AddState((int)EMState.Born, new CFSMBoss204Born());
        pFSM.AddState((int)EMState.Idle, new CFSMBoss204Idle());
        pFSM.AddState((int)EMState.SavePower, new CFSMBoss204SavePower());
        pFSM.AddState((int)EMState.WaitAtk, new CFSMBoss204WaitAtk());
        pFSM.AddState((int)EMState.Attack, new CFSMBoss204Atk());
        pFSM.AddState((int)EMState.EndAtk, new CFSMBoss204EndAtk());
        pFSM.AddState((int)EMState.Dead, new CFSMBoss204Dead());
        pFSM.AddState((int)EMState.Escape, new CFSMBoss204Escape());

        pFSM.AddState((int)EMState.GameStart, new CFSMBoss204GameStart());
    }


    public override bool IsAtkAble()
    {
        return emCurState == EMState.Born || emCurState == EMState.Idle;
    }

    public override void OnHit(string atkUid, long hp)
    {
        //Debug.Log("�ܻ�Ѫ����" + hp);

        base.OnHit(atkUid, hp);

        if (emCurState != EMState.Dead)
        {
            uiTweenMat.Play();

            //�Ƿ����Σ��
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
    /// ��ʼ��������
    /// </summary>
    public void AtkByStart()
    {
        for(int i = 0;i < tranStartHitPos.Length;i++)
        {
            if (tranStartHitPos[i] == null) continue;
            CTentacleMgrByBoss.Ins.ShowTentacle(tranStartHitPos[i], true);
        }
    }

    /// <summary>
    /// ��ͨ����
    /// </summary>
    public void AtkByNormal()
    {
        int nRandomIdx = 0;
        int nAtkCount = Random.Range(vNormalAtkCount.x, vNormalAtkCount.y + 1);
        ///��ȡ����ص�
        List <CTentacleShowPos> listAtkPos = new List<CTentacleShowPos>();
        for(int i = 0;i < tranAtkHitPos.Length;i++)
        {
            if (tranAtkHitPos[i] == null) continue;
            listAtkPos.Add(tranAtkHitPos[i]);
        }
        ///������ص����ɴ���
        for(int i = 0;i < nAtkCount;i++)
        {
            if (listAtkPos.Count <= 0) break;
            nRandomIdx = Random.Range(0, listAtkPos.Count);
            CTentacleShowPos vAtkTargetPos = listAtkPos[nRandomIdx];
            CEffectMgr.Instance.CreateEffSync(szAlertRangeEffect, vAtkTargetPos.tranAtkPos, 0,false, delegate (GameObject value)
            {
                CEffectBase pEff = value.GetComponent<CEffectBase>();
                if (pEff != null)
                {
                    pEff.pEndEvent = delegate ()
                    {
                        CTentacleMgrByBoss.Ins.ShowTentacle(vAtkTargetPos, false);
                        //����ʾ���
                        UIBossBaseInfo uiBossInfo = UIManager.Instance.GetUI(UIResType.BossBaseInfo) as UIBossBaseInfo;
                        if (uiBossInfo != null)
                        {
                            uiBossInfo.SetBoss204AtkTip(false);
                        }
                    };
                }
            });
            listAtkPos.RemoveAt(nRandomIdx);
        }
    }

}
