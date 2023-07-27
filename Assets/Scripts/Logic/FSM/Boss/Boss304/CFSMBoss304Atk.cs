using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFSMBoss304Atk : CFSMBossBase
{
    CBoss304 pBoss;
    CPropertyTimer pTimeTicker = new CPropertyTimer();
    Vector3 vPrePos;
    Vector3 vCurPos;
    Vector3 vDir;
    bool bAtk = false;

    Transform tranStart;
    Transform tranEnd;

    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CBossBase.EMState.Attack;
        pBoss = pUnit as CBoss304;
        if (pBoss == null) return;
        pUnit.pAnimeCtrl.Play(CBossAnimeConst.Anime_Atk);
        bAtk = false;
        UIBossBaseInfo bossBaseInfo = UIManager.Instance.GetUI(UIResType.BossBaseInfo) as UIBossBaseInfo;
        if (bossBaseInfo != null)
        {
            bossBaseInfo.ActiveBossWeakBar(false);
        }
        ResetBody();
    }

    void ResetBody()
    {
        if(Vector3.Distance(pBoss.transform.position, pBoss.listAtkPaths[0].tranStartPos.position) >
           Vector3.Distance(pBoss.transform.position, pBoss.listAtkPaths[0].tranEndPos.position))
        {
            tranStart = pBoss.listAtkPaths[0].tranEndPos;
            tranEnd = pBoss.listAtkPaths[0].tranStartPos;
        }
        else
        {
            tranStart = pBoss.listAtkPaths[0].tranStartPos; 
            tranEnd = pBoss.listAtkPaths[0].tranEndPos;
        }

        //确定路径
        pBoss.transform.position = tranStart.position;

        //调整转向
        Vector3 vDir = tranEnd.position - tranStart.position;
        vDir.y = 0F;
        vDir = vDir.normalized;
        pBoss.transform.forward = vDir;

        //身体调整
        pBoss.tranBody.localPosition = Vector3.zero;
        pBoss.tranBody.localEulerAngles = pBoss.vAngelAtkBody;
        if (pBoss.pEffAtk != null)
        {
            pBoss.pEffAtk.SetActive(true);
        }
        pTimeTicker.Value = pBoss.fTimeAtkDash;
        pTimeTicker.FillTime();

        vPrePos = pBoss.transform.position;
        vCurPos = pBoss.transform.position;
    }

    public override void OnFixedUpdate(object obj, float delta)
    {
        if (!pTimeTicker.Tick(delta))
        {
            vCurPos = Vector3.Lerp(tranStart.position,
                                   tranEnd.position,
                                   1f - pTimeTicker.GetTimeLerp());

            pUnit.transform.position = vCurPos;

            CheckHit(vPrePos, pBoss.tranCheckHitPos.position);
            vPrePos = pBoss.tranCheckHitPos.position;
        }
        else
        {
            vCurPos = tranEnd.position;
            pUnit.transform.position = vCurPos;

            CheckHit(vPrePos, pBoss.tranCheckHitPos.position);
            vPrePos = pBoss.tranCheckHitPos.position;
            pBoss.listAtkPaths.RemoveAt(0);
            if (pBoss.listAtkPaths.Count > 0)
            {
                ResetBody();
            }
            else
            {
                pBoss.SetState(CBossBase.EMState.EndAtk);
            }
        }
    }

    RaycastHit pHitInfo;
    Collider[] arrHitsCol;
    void CheckHit(Vector3 start, Vector3 end)
    {
        if (bAtk) return;

        start.y = 0;
        end.y = 0;


        GameObject[] objCols = CHelpTools.SphereCheck(end, pBoss.fAtkRadius, pBoss.lAtkCheck);
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

                pUnit.SetState(CPlayerUnit.EMState.BossEat);
            }
        }
    }

    public override void OnEnd(object obj)
    {
        pBoss.tranBody.localPosition = Vector3.zero;
        pBoss.tranBody.localEulerAngles = Vector3.zero;
        if (pBoss.pEffAtk != null)
        {
            pBoss.pEffAtk.SetActive(false);
        }
    }
}
