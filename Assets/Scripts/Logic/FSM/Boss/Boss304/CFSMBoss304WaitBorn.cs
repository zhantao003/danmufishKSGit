using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFSMBoss304WaitBorn : CFSMBossBase
{
    public int nTimes = 0;
    CPropertyTimer pTimerTicker = new CPropertyTimer();

    public override void OnBegin(object obj)
    {
        pTimerTicker.Value = 1F;
        pTimerTicker.FillTime();
        nTimes = 2;
        pUnit.pAnimeCtrl.Play(CBossAnimeConst.Anime_Idle);
        UIBossBaseInfo uiBossBaseInfo = UIManager.Instance.GetUI(UIResType.BossBaseInfo) as UIBossBaseInfo;
        if (uiBossBaseInfo != null)
        {
            uiBossBaseInfo.PlayGameStartCount(3.ToString());
        }
    }

    public override void OnUpdate(object obj, float delta)
    {
        if (pTimerTicker.Tick(delta))
        {
            if (nTimes <= 0)
            {
                CGameBossMgr.Ins.DoStart();
                pUnit.SetState(CBossBase.EMState.Born);

                //ȫ�帴ԭ
                List<CPlayerBaseInfo> listAllPlayers = CGameBossMgr.Ins.listActivePlayers;
                for (int i = 0; i < listAllPlayers.Count; i++)
                {
                    CPlayerUnit pPlayerUnit = CPlayerMgr.Ins.GetIdleUnit(listAllPlayers[i].uid);
                    if (pPlayerUnit == null)
                        continue;

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
