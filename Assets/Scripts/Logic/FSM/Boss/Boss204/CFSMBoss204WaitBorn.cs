using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFSMBoss204WaitBorn : CFSMBossBase
{
    CBoss204 pBoss;
    public int nTimes = 0;
    CPropertyTimer pTimerTicker = new CPropertyTimer();

    CPropertyTimer pWaitTicker = new CPropertyTimer();
    CPropertyTimer pShowTicker = new CPropertyTimer();

    public override void OnBegin(object obj)
    {
        pBoss = pUnit as CBoss204;
        if (pBoss == null) return;

        pWaitTicker = new CPropertyTimer();
        pWaitTicker.Value = pBoss.fTimeWaitTentacle - 15f;
        pWaitTicker.FillTime();

        pTimerTicker = null;
        pShowTicker = null;
    }

    public override void OnUpdate(object obj, float delta)
    {
        if (pWaitTicker != null)
        {
            if (pWaitTicker.Tick(delta))
            {
                pWaitTicker = null;
                pShowTicker = new CPropertyTimer();
                pShowTicker.Value = pBoss.fTimeShow;
                pShowTicker.FillTime();

                pTimerTicker = new CPropertyTimer();
                pTimerTicker.Value = 1F;
                pTimerTicker.FillTime();
                nTimes = 2;
                UIBossBaseInfo uiBossBaseInfo = UIManager.Instance.GetUI(UIResType.BossBaseInfo) as UIBossBaseInfo;
                if (uiBossBaseInfo != null)
                {
                    uiBossBaseInfo.PlayGameStartCount((nTimes + 1).ToString());
                }
            }
        }
        if (pShowTicker != null)
        {
            if (pShowTicker.Tick(delta))
            {
                pBoss.transform.position = pBoss.vPosReady;
                pShowTicker = null;
            }
            else
            {
                pBoss.transform.position = Vector3.Lerp(pBoss.vGameStartPos, pBoss.vPosReady, 1f - pShowTicker.GetTimeLerp());
            }
        }
        if (pTimerTicker != null)
        {
            if (pTimerTicker.Tick(delta))
            {
                if (nTimes <= 0)
                {
                    CGameBossMgr.Ins.DoStart();
                    pUnit.SetState(CBossBase.EMState.Born);

                    //全体复原
                    List<CPlayerBaseInfo> listAllPlayers = CGameBossMgr.Ins.listActivePlayers;
                    Debug.Log("复原玩家数量：" + listAllPlayers.Count);
                    for (int i = 0; i < listAllPlayers.Count; i++)
                    {
                        CPlayerUnit pPlayerUnit = CPlayerMgr.Ins.GetIdleUnit(listAllPlayers[i].uid);
                        if (pPlayerUnit == null)
                        {
                            Debug.Log("uid:" + listAllPlayers[i].uid + "  IdleUnit位空");
                            pPlayerUnit = CPlayerMgr.Ins.GetActiveUnit(listAllPlayers[i].uid);
                            if (pPlayerUnit == null)
                            {
                                Debug.Log("uid:" + listAllPlayers[i].uid + "  ActiveUnit位空");
                                continue;
                            }
                        }

                        pPlayerUnit.SetState(CPlayerUnit.EMState.BossReturnShow);
                    }

                    UIBossBaseInfo uiBossBaseInfo = UIManager.Instance.GetUI(UIResType.BossBaseInfo) as UIBossBaseInfo;
                    if (uiBossBaseInfo != null)
                    {
                        uiBossBaseInfo.PlayGameStartCount("");
                    }
                }
                else
                {
                    pTimerTicker.FillTime();
                    nTimes--;

                    UIBossBaseInfo uiBossBaseInfo = UIManager.Instance.GetUI(UIResType.BossBaseInfo) as UIBossBaseInfo;
                    if (uiBossBaseInfo != null)
                    {
                        uiBossBaseInfo.PlayGameStartCount((nTimes + 1).ToString());
                    }
                }
            }
        }
    }
}
