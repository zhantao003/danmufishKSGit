using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTentacleUnitByBoss : MonoBehaviour
{
    public enum EMState
    {
        Show,       //出生
        Stay,       //等待
        Hit,        //攻击
        Recycle     //回收
    }

    public Transform tranCheckPos;
    public float fCheckRange;

    public Animation anima;

    public EMState emState;

    Vector3 vecShowStart;
    Vector3 vecShowEnd;
    Transform tranAtkPos;

    public LayerMask pCheckMask;

    public float fShowTime;
    public float fStayTime = 10f;
    public float fHitTime;
    public float fRecycleTime;
    CPropertyTimer pCheckTick;

    public float fHitPlayerTime;
    CPropertyTimer pHitTick;

    public bool bGameStartAtk;

    public string szHitEffect;

    public void Show(CTentacleShowPos cShowPos)
    {
        emState = EMState.Show;
        tranAtkPos = cShowPos.tranAtkPos;
        vecShowEnd = cShowPos.tranShowPos.position;
        vecShowStart = vecShowEnd - Vector3.up * 20f;
        transform.position = vecShowStart;
        Vector3 vecForward = (vecShowEnd - tranAtkPos.position).normalized;
        transform.forward = vecForward;
        transform.Rotate(new Vector3(0, -90, 0));
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);

        ResetState();
    }

    private void Update()
    {
        if (pCheckTick != null)
        {
            if (pCheckTick.Tick(CTimeMgr.DeltaTime))
            {
                if (emState == EMState.Show)
                {
                    emState = EMState.Stay;
                    ResetState();
                }
                else if (emState == EMState.Stay)
                {
                    emState = EMState.Hit;
                    ResetState();
                }
                else if (emState == EMState.Hit)
                {
                    emState = EMState.Recycle;
                    ResetState();
                }
                else if (emState == EMState.Recycle)
                {
                    CTentacleMgrByBoss.Ins.RecycleTentacle(this);
                    pCheckTick = null;
                }
            }
            else
            {
                if (emState == EMState.Show)
                {
                    transform.position = Vector3.Lerp(vecShowStart, vecShowEnd, 1f - pCheckTick.GetTimeLerp());
                }
                else if (emState == EMState.Recycle)
                {
                    transform.position = Vector3.Lerp(vecShowStart, vecShowEnd, 1f - pCheckTick.GetTimeLerp());
                }
            }
        }
        if (pHitTick != null &&
           pHitTick.Tick(CTimeMgr.DeltaTime))
        {
            pHitTick = null;
            CheckTarget();
        }
    }

    public void CheckTarget()
    {
        float checkRange = bGameStartAtk ? 99f : fCheckRange;
        GameObject[] objCols = CHelpTools.SphereCheck(tranAtkPos.position, checkRange, pCheckMask);
        if (objCols != null)
        {
            for (int i = 0; i < objCols.Length; i++)
            {
                CPlayerUnit pUnit = objCols[i].gameObject.GetComponent<CPlayerUnit>();
                if (pUnit == null) continue;
                if (pUnit.emCurState == CPlayerUnit.EMState.BossWait ||
                    pUnit.emCurState == CPlayerUnit.EMState.BossEat ||
                    pUnit.emCurState == CPlayerUnit.EMState.BossReturn ||
                    pUnit.emCurState == CPlayerUnit.EMState.GunShootBoss ||
                    pUnit.emCurState == CPlayerUnit.EMState.RPGShootBoss) continue;
                
                if (bGameStartAtk)
                {
                    pUnit.SetState(CPlayerUnit.EMState.BossEatShow);
                }
                else
                {
                    pUnit.SetState(CPlayerUnit.EMState.BossEat);
                }
            }
        }

        CEffectMgr.Instance.CreateEffSync(szHitEffect, tranAtkPos, 0);
    }

    /// <summary>
    /// 设置对应的状态
    /// </summary>
    public void ResetState()
    {
        if (emState == EMState.Show)
        {
            anima.Play("Tentacle|Idle01");
            anima["Tentacle|Idle01"].speed = 1f;
            pCheckTick = new CPropertyTimer();
            pCheckTick.Value = fShowTime;
            pCheckTick.FillTime();
            
        }
        else if (emState == EMState.Stay)
        {
            anima.Play("Tentacle|Idle01");
            anima["Tentacle|Idle01"].speed = 1f;
            pCheckTick = new CPropertyTimer();
            pCheckTick.Value = fStayTime;
            pCheckTick.FillTime();
        }
        else if (emState == EMState.Hit)
        {
            anima.Play("Tentacle|Attack");
            anima["Tentacle|Attack"].speed = 2f;
            pCheckTick = new CPropertyTimer();
            pCheckTick.Value = fHitTime;
            pCheckTick.FillTime();
            pHitTick = new CPropertyTimer();
            pHitTick.Value = fHitPlayerTime;
            pHitTick.FillTime();
        }
        else if (emState == EMState.Recycle)
        {
            anima.Play("Tentacle|Idle01");
            anima["Tentacle|Idle01"].speed = 1f;
            vecShowEnd = transform.position - Vector3.up * 20f;
            vecShowStart = transform.position;
            pCheckTick = new CPropertyTimer();
            pCheckTick.Value = fRecycleTime;
            pCheckTick.FillTime();
        }
    }


}
