using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFSMBoss204Ready : CFSMBossBase
{
    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CBossBase.EMState.Ready;
        CBoss204 pBoss = pUnit as CBoss204;
        if (pBoss == null) return;

        pBoss.pAnimeCtrl.Play(CBossAnimeConst.Anime_Idle);
        pBoss.transform.position = pBoss.vGameStartPos;
    }
}
