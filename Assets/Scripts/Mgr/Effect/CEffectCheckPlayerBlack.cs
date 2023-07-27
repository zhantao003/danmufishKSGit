using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEffectCheckPlayerBlack : CEffectBase
{
    public float fCheckRadius = 0.1F;
    public LayerMask lCheckMsk;

    Collider[] arrColUnit;

    public override void Play(bool refresh = true)
    {
        base.Play(refresh);

        DoCheck();
    }

    void DoCheck()
    {
        arrColUnit = Physics.OverlapSphere(transform.position, fCheckRadius, lCheckMsk);
        if (arrColUnit == null || arrColUnit.Length == 0) return;

        for (int i = 0; i < arrColUnit.Length; i++)
        {
            CPlayerUnit pUnit = arrColUnit[i].gameObject.GetComponent<CPlayerUnit>();
            if (pUnit == null) return;

            //pUnit.PlayMatAnime(CUnitMatAnimeConst.Anime_Black);
        }
    }
}
