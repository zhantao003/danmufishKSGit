using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFSMBoss104GameStart : CFSMBossBase
{
    CBoss104 pBoss;
    CPropertyTimer pTimeTicker = new CPropertyTimer();

    Vector3 vPrePos;
    Vector3 vCurPos;
    Vector3 vDir;
    bool bAtk = false;

    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CBossBase.EMState.GameStart;
        pBoss = pUnit as CBoss104;
        if (pBoss == null) return;

        pBoss.uiTweenIdle.Stop();

        pTimeTicker.Value = pBoss.fTimeGameStart;
        pTimeTicker.FillTime();

        pBoss.transform.position = pBoss.vGameStartPos;

        //身体调整
        pBoss.tranBody.localPosition = pBoss.vPosAtkBody;
        pBoss.tranBody.localEulerAngles = pBoss.vAngelAtkBody;
        pBoss.pEffAtk.Play();

        vPrePos = pBoss.transform.position;
        vCurPos = pBoss.transform.position;
    }

    public override void OnFixedUpdate(object obj, float delta)
    {
        if (!pTimeTicker.Tick(delta))
        {
            vCurPos = Vector3.Lerp(pBoss.vGameStartPos,
                                   pBoss.vPosReady,
                                   1f - pTimeTicker.GetTimeLerp());

            pUnit.transform.position = vCurPos;

            CheckHit(vPrePos, vCurPos);
            vPrePos = vCurPos;
        }
        else
        {
            vCurPos = pBoss.vPosReady;
            pUnit.transform.position = vCurPos;

            CheckHit(vPrePos, vCurPos);
            vPrePos = vCurPos;

            pBoss.SetState(CBossBase.EMState.WaitBorn);
        }
    }

    RaycastHit pHitInfo;
    Collider[] arrHitsCol;
    void CheckHit(Vector3 start, Vector3 end)
    {
        if (bAtk) return;

        start.y = 0;
        end.y = 0;

        arrHitsCol = Physics.OverlapSphere(end, pBoss.fAtkRadius, pBoss.lAtkCheck);
        if (arrHitsCol != null)
        {
            for (int i = 0; i < arrHitsCol.Length; i++)
            {
                if (bAtk) break;

                CPlayerBossBoatCol pCol = arrHitsCol[i].gameObject.GetComponent<CPlayerBossBoatCol>();
                if (pCol == null) return;

                bAtk = true;
                //播放特效
                pCol.pBoat.BeHitShow();
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
