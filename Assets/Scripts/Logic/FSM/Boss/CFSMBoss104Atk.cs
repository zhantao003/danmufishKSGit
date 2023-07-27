using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFSMBoss104Atk : CFSMBossBase
{
    CBoss104 pBoss;
    CPropertyTimer pTimeTicker = new CPropertyTimer();
    Vector3 vPrePos;
    Vector3 vCurPos;
    Vector3 vDir;
    bool bAtk = false;

    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CBossBase.EMState.Attack;
        pBoss = pUnit as CBoss104;
        if (pBoss == null) return;

        bAtk = false;

        //身体调整
        pBoss.tranBody.localPosition = pBoss.vPosAtkBody;
        pBoss.tranBody.localEulerAngles = pBoss.vAngelAtkBody;
        pBoss.pEffAtk.Play();

        pTimeTicker.Value = pBoss.fTimeAtkDash;
        pTimeTicker.FillTime();

        vPrePos = pBoss.transform.position;
        vCurPos = pBoss.transform.position;
    }

    public override void OnFixedUpdate(object obj, float delta)
    {
        if(!pTimeTicker.Tick(delta))
        {
            vCurPos = Vector3.Lerp(pBoss.GetAtkPath(pBoss.emAtkPath)[0].position,
                                   pBoss.GetAtkPath(pBoss.emAtkPath)[1].position,
                                   1f - pTimeTicker.GetTimeLerp());

            pUnit.transform.position = vCurPos;

            CheckHit(vPrePos, vCurPos);
            vPrePos = vCurPos;
        }
        else
        {
            vCurPos = pBoss.GetAtkPath(pBoss.emAtkPath)[1].position;
            pUnit.transform.position = vCurPos;

            CheckHit(vPrePos, vCurPos);
            vPrePos = vCurPos;

            pBoss.SetState(CBossBase.EMState.EndAtk);
        }
    }

    RaycastHit pHitInfo;
    Collider[] arrHitsCol;
    void CheckHit(Vector3 start, Vector3 end)
    {
        if (bAtk) return;

        start.y = 0;
        end.y = 0;

        //vDir = end - start; 
        //vDir.y = 0F;
        //vDir = vDir.normalized;

        arrHitsCol = Physics.OverlapSphere(end, pBoss.fAtkRadius, pBoss.lAtkCheck);
        if(arrHitsCol!=null)
        {
            for(int i=0; i<arrHitsCol.Length; i++)
            {
                if (bAtk) break;

                CPlayerBossBoatCol pCol = arrHitsCol[i].gameObject.GetComponent<CPlayerBossBoatCol>();
                if (pCol == null) return;

                bAtk = true;
                //播放特效
                pCol.pBoat.BeHit();
            }
        }
    }

    public override void OnEnd(object obj)
    {
        pBoss.tranBody.localPosition = Vector3.zero;
        pBoss.tranBody.localEulerAngles = Vector3.zero;
        pBoss.pEffAtk.StopEffect();
    }
}
