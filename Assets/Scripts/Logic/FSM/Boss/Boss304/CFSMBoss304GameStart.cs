using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFSMBoss304GameStart : CFSMBossBase
{
    CBoss304 pBoss;
    CPropertyTimer pTimeTicker = new CPropertyTimer();

    CPropertyTimer pAtkTicker = new CPropertyTimer();

    Vector3 vPrePos;
    Vector3 vCurPos;
    Vector3 vDir;
    bool bAtk = false;

    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CBossBase.EMState.GameStart;
        pBoss = pUnit as CBoss304;
        if (pBoss == null) return;
        pUnit.pAnimeCtrl.Play(CBossAnimeConst.Anime_Atk);
        pBoss.uiTweenIdle.Stop();

        pTimeTicker.Value = pBoss.fTimeGameStart;
        pTimeTicker.FillTime();
        pAtkTicker = new CPropertyTimer();
        pAtkTicker.Value = pBoss.fTimeGameStart * 0.25f;
        pAtkTicker.FillTime();

        pBoss.transform.position = pBoss.vGameStartPos;

        //身体调整
        pBoss.tranBody.localPosition = Vector3.zero;
        pBoss.tranBody.localEulerAngles = pBoss.vAngelAtkBody;
        if (pBoss.pEffAtk != null)
        {
            pBoss.pEffAtk.SetActive(true);
        }
        vPrePos = pBoss.transform.position;
        vCurPos = pBoss.transform.position;

        UIBossBaseInfo bossBaseInfo = UIManager.Instance.GetUI(UIResType.BossBaseInfo) as UIBossBaseInfo;
        if (bossBaseInfo != null)
        {
            bossBaseInfo.ActiveBossWeakBar(false);
        }
    }

    public override void OnFixedUpdate(object obj, float delta)
    {
        if (pAtkTicker != null &&
              pAtkTicker.Tick(delta))
        {
            CheckHit(pBoss.tranCheckHitPos.position);
            pAtkTicker = null;
        }
        if (!pTimeTicker.Tick(delta))
        {
            vCurPos = Vector3.Lerp(pBoss.vGameStartPos,
                                   pBoss.vPosReady,
                                   1f - pTimeTicker.GetTimeLerp());

            pUnit.transform.position = vCurPos;

            vPrePos = pBoss.tranCheckHitPos.position;
        }
        else
        {
            vCurPos = pBoss.vPosReady;
            pUnit.transform.position = vCurPos;

            vPrePos = pBoss.tranCheckHitPos.position;

            pBoss.SetState(CBossBase.EMState.WaitBorn);
        }
        
    }

    RaycastHit pHitInfo;
    Collider[] arrHitsCol;
    void CheckHit(Vector3 end)
    {
        if (bAtk) return;

        end.y = 0;

        GameObject[] objCols = CHelpTools.SphereCheck(end, 9999f, pBoss.lAtkCheck);
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

                pUnit.SetState(CPlayerUnit.EMState.BossEatShow);
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
