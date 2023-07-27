using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFSMBoss204Atk : CFSMBossBase
{
    CBoss204 pBoss;
    CPropertyTimer pTimeTicker = new CPropertyTimer();
    Vector3 vPrePos;
    Vector3 vCurPos;
    Vector3 vDir;
    bool bAtk = false;

    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CBossBase.EMState.Attack;
        pBoss = pUnit as CBoss204;
        if (pBoss == null) return;

        bAtk = false;

        //身体调整
        //pBoss.tranBody.localPosition = pBoss.vPosAtkBody;
        //pBoss.tranBody.localEulerAngles = pBoss.vAngelAtkBody;
        //pBoss.pEffAtk.Play();

        pTimeTicker.Value = pBoss.fTimeNormalAtk;
        pTimeTicker.FillTime();

        //vPrePos = pBoss.transform.position;
        //vCurPos = pBoss.transform.position;
    }

    public override void OnFixedUpdate(object obj, float delta)
    {
        if (!pTimeTicker.Tick(delta))
        {
            //vCurPos = Vector3.Lerp(pBoss.vGameStartPos,
                                   //pBoss.vPosReady,
                                   //1f - pTimeTicker.GetTimeLerp());

            //pUnit.transform.position = vCurPos;
            //vPrePos = vCurPos;
        }
        else
        {
            //vCurPos = pBoss.vPosReady;
            //pUnit.transform.position = vCurPos;
            //打开显示面板
            UIBossBaseInfo uiBossInfo = UIManager.Instance.GetUI(UIResType.BossBaseInfo) as UIBossBaseInfo;
            if (uiBossInfo != null)
            {
                uiBossInfo.SetBoss204AtkTip(true);
            }
            pBoss.AtkByNormal();
            //vPrePos = vCurPos;
            

            
            pBoss.SetState(CBossBase.EMState.EndAtk);
        }
    }


    public override void OnEnd(object obj)
    {
        pBoss.tranBody.localPosition = Vector3.zero;
        pBoss.tranBody.localEulerAngles = Vector3.zero;
        pBoss.pEffAtk.StopEffect();
    }
}
