using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBossWeakBase : CBossBase
{
    public bool bDead = false;
    /// <summary>
    /// ���ӹ�������
    /// </summary>
    public int nAddAtkCount = 1;
    /// <summary>
    /// ���ӵ������(���֮��)
    /// </summary>
    public int nAddRate = 260;
    /// <summary>
    /// Ѫ����λ��
    /// </summary>
    public Transform tranBarPos;

    public int nShowIdx;

    public CEffectOnlyCtrl pEffBoom;

    public GameObject objDeadEff;

    public override void Init()
    {
        bDead = false;
        nCurHp = nHPMax;
        UIBossBaseInfo bossBaseInfo = UIManager.Instance.GetUI(UIResType.BossBaseInfo) as UIBossBaseInfo;
        if(bossBaseInfo != null)
        {
            bossBaseInfo.AddBossWeakBar(this);
        }
        if(objDeadEff != null)
        {
            objDeadEff.SetActive(false);
        }
        if (pEffBoom != null)
        {
            pEffBoom.Init();
            pEffBoom.StopEffect();
        }
    }
    
    public void PlayDeadEff()
    {
        if (pEffBoom != null)
        {
            pEffBoom.Play();
        }
        if (objDeadEff != null)
        {
            objDeadEff.SetActive(true);
        }
    }



    public override void OnHit(string atkUid, long hp)
    {
        if (bDead) return;

        if (hp > nCurHp)
        {
            //ͳ��
            CGameBossMgr.Ins.AddPlayerDmg(atkUid, nCurHp);
        }
        else
        {
            //ͳ��
            CGameBossMgr.Ins.AddPlayerDmg(atkUid, hp);
        }

        nCurHp -= hp;
        if (nCurHp < 0)
        {
            nCurHp = 0;
        }
        dlgHPChg?.Invoke(nCurHp, nHPMax);

        if (nCurHp <= 0)
        {
            bDead = true;
            if (CGameBossMgr.Ins.pBoss != null)
            {
                CGameBossMgr.Ins.pBoss.SetState(EMState.OnHit);
            }
        }

    }


}
