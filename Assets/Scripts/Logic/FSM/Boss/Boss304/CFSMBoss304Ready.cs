using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFSMBoss304Ready : CFSMBossBase
{
    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CBossBase.EMState.Ready;
        CBoss304 pBoss = pUnit as CBoss304;
        if (pBoss == null) return;

        pBoss.pAnimeCtrl.Play(CBossAnimeConst.Anime_Idle);
        pBoss.transform.position = pBoss.vPosReady;
    }
}
