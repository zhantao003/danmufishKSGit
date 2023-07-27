using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGunRPG : MonoBehaviour
{
    [ReadOnly]
    public string nOwnerUID;

    public float fShootTime = 0.5F;
    public float fLifeTime = 1F;

    public float fDmgLerp = 4F;    //…À∫¶±∂¬ 
    public Vector2Int vDmgRange;   //…À∫¶∑∂Œß
    public string szEffShoot;

    public UITweenBase[] arrTweenBorn;
    public string szEffBullet;
    public string szEffBoom;
    public string szEffDead;

    public Transform tranFirePos;
    [ReadOnly]
    public Transform tranTarget;

    CPropertyTimer pTickerShoot;
    CPropertyTimer pTickerLife;

    public CAudioMgr.CAudioSlottInfo pAudioShoot;

    public int nTimeLimitYear;
    public int nTimeLimitMonth;
    public int nTimeLimitDay;

    int nDmgRate;
    public int nAtkIdx = 0;

    public void Init(string uid, int dmgRate, int atkIdx, Transform target)
    {
        nOwnerUID = uid;
        nDmgRate = dmgRate;
        nAtkIdx = atkIdx;

        for (int i = 0; i < arrTweenBorn.Length; i++)
        {
            arrTweenBorn[i].Play();
        }

        tranTarget = target;

        pTickerLife = new CPropertyTimer();
        pTickerLife.Value = fLifeTime;
        pTickerLife.FillTime();

        pTickerShoot = new CPropertyTimer();
        pTickerShoot.Value = fShootTime;
        pTickerShoot.FillTime();

        if (pAudioShoot != null)
        {
            CAudioMgr.Ins.PlaySoundBySlot(pAudioShoot, tranFirePos.position);
        }
    }

    void Update()
    {
        if (pTickerShoot != null &&
            pTickerShoot.Tick(CTimeMgr.DeltaTime))
        {
            CreateEff();

            System.DateTime pNowTime = System.DateTime.Now;
            if (pNowTime.Year == nTimeLimitYear &&
                pNowTime.Month == nTimeLimitMonth &&
                pNowTime.Day == nTimeLimitDay)
            {
                CPlayerNetHelper.AddFishCoin(nOwnerUID,
                                             (int)(CGameColorFishMgr.Ins.pStaticConfig.GetInt("◊Ã±¿¥Ú’€º€∏Ò") * -fDmgLerp),
                                             EMFishCoinAddFunc.Free,
                                             false);
            }
            else
            {
                CPlayerNetHelper.AddFishCoin(nOwnerUID,
                                             (int)(CGameColorFishMgr.Ins.pStaticConfig.GetInt("◊Ã±¿º€∏Ò") * -fDmgLerp),
                                             EMFishCoinAddFunc.Free,
                                             false);
            }

            //∑¢…‰◊”µØ
            int nDmg = (int)(Random.Range(vDmgRange.x, vDmgRange.y) * (fDmgLerp + nDmgRate * 0.01F));
            string nCurOwnerUID = nOwnerUID;
            CEffectMgr.Instance.CreateEffSync(szEffBullet, tranFirePos,0,false,
                delegate(GameObject eff) {
                    CEffectBeizier pEff = eff.GetComponent<CEffectBeizier>();
                    if (pEff == null) return;

                    pEff.szBoomEff = szEffBoom;
                    //string szCurEffBoom = szEffBoom;
                    pEff.SetTarget(tranFirePos.position, CGameBossMgr.Ins.pBoss.GetRandHitPos(),
                        delegate() {
                            if (nAtkIdx > 1)
                            {
                                CBossWeakBase pBossWeak = CGameBossMgr.Ins.pBoss.GetWeak(nAtkIdx);
                                if (pBossWeak != null)
                                {
                                    pBossWeak.OnHit(nCurOwnerUID, nDmg);
                                }
                                else
                                {
                                    CGameBossMgr.Ins.pBoss.OnHit(nCurOwnerUID, nDmg);
                                }
                            }
                            else
                            {
                                CGameBossMgr.Ins.pBoss.OnHit(nCurOwnerUID, nDmg);
                            }

                            Vector3 vUIPos = pEff.transform.position;
                            UIBossBaseInfo uiBossUI = UIManager.Instance.GetUI(UIResType.BossBaseInfo) as UIBossBaseInfo;
                            if (uiBossUI == null) return;
                            uiBossUI.AddDmgTip(nDmg, EMDmgType.Zibeng, EMRare.Shisi, vUIPos + Vector3.right * Random.Range(-2F, 2F));
                        });
                });

            pTickerShoot = null;
        }

        if (pTickerLife != null &&
            pTickerLife.Tick(CTimeMgr.DeltaTime))
        {
            CEffectMgr.Instance.CreateEffSync(szEffDead, transform.position, Quaternion.identity, 0);
            pTickerLife = null;

            Destroy(gameObject);
        }
    }

    void CreateEff()
    {
        if (CGameBossMgr.Ins == null ||
            CGameBossMgr.Ins.pBoss == null ||
           !CGameBossMgr.Ins.pBoss.IsAtkAble()) return;

        CEffectMgr.Instance.CreateEffSync(szEffShoot, tranFirePos.position, tranFirePos.rotation, 0);
    }
}
