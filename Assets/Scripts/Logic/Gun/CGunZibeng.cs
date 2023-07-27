using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGunZibeng : MonoBehaviour
{
    [ReadOnly]
    public string nOwnerUID;

    public float fShootTime = 0.5F;
    public float fHitTime = 0.5F;
    public float fLifeTime = 1F;
    public Vector2Int vDmgRange;   //ÉËº¦·¶Î§
    public string szEffShoot;
    public string szBeam;
    public string szEffHit;

    public UITweenBase[] arrTweenBorn;
    public string szEffDead;

    public Transform tranFirePos;
    [ReadOnly]
    public Transform tranTarget;

    CPropertyTimer pTickerShoot;
    CPropertyTimer pTickerHit;
    CPropertyTimer pTickerLife;

    public CAudioMgr.CAudioSlottInfo pAudioShoot;

    public int nTimeLimitYear;
    public int nTimeLimitMonth;
    public int nTimeLimitDay;

    public int nAtkIdx = 0;

    public void Init(string uid, int atkIdx, Transform target)
    {
        nOwnerUID = uid;
        nAtkIdx = atkIdx;

        for(int i=0; i<arrTweenBorn.Length; i++)
        {
            arrTweenBorn[i].Play();
        }

        tranTarget = target;

        pTickerLife = new CPropertyTimer();
        pTickerLife.Value = fLifeTime;
        pTickerLife.FillTime();

        pTickerHit = new CPropertyTimer();
        pTickerHit.Value = fHitTime;
        pTickerHit.FillTime();

        pTickerShoot = new CPropertyTimer();
        pTickerShoot.Value = fShootTime;
        pTickerShoot.FillTime();

        if (pAudioShoot != null)
        {
            CAudioMgr.Ins.PlaySoundBySlot(pAudioShoot, tranFirePos.position);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(pTickerShoot != null &&
           pTickerShoot.Tick(CTimeMgr.DeltaTime))
        {
            CreateEff();

            pTickerShoot = null;
        }

        if(pTickerHit != null &&
           pTickerHit.Tick(CTimeMgr.DeltaTime))
        {
            DoHit();

            pTickerHit = null;
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
        CEffectMgr.Instance.CreateEffSync(szEffHit, tranTarget.position, Quaternion.identity, 0);
        CEffectMgr.Instance.CreateEffSync(szBeam, tranFirePos.position, tranFirePos.rotation, 0,
            delegate (GameObject effObj)
            {
                CEffectBeam pBeam = effObj.GetComponent<CEffectBeam>();
                pBeam.SetBeam(tranFirePos.position, tranTarget.position);
            });
    }

    void DoHit()
    {
        if (CGameBossMgr.Ins == null ||
           CGameBossMgr.Ins.pBoss == null ||
          !CGameBossMgr.Ins.pBoss.IsAtkAble()) return;

        int nDmg = Random.Range(vDmgRange.x, vDmgRange.y);

        if(nAtkIdx > 1)
        {
            CBossWeakBase pBossWeak = CGameBossMgr.Ins.pBoss.GetWeak(nAtkIdx);
            if(pBossWeak!=null)
            {
                pBossWeak.OnHit(nOwnerUID, nDmg);
            }
            else
            {
                CGameBossMgr.Ins.pBoss.OnHit(nOwnerUID, nDmg);
            }
        }
        else
        {
            CGameBossMgr.Ins.pBoss.OnHit(nOwnerUID, nDmg);
        }

        Vector3 vUIPos = tranTarget.position;
        UIBossBaseInfo uiBossUI = UIManager.Instance.GetUI(UIResType.BossBaseInfo) as UIBossBaseInfo;
        if (uiBossUI == null) return;
        uiBossUI.AddDmgTip(nDmg, EMDmgType.Zibeng, EMRare.Shisi, vUIPos + Vector3.right * Random.Range(-2F, 2F));

        System.DateTime pNowTime = System.DateTime.Now;
        if (pNowTime.Year == nTimeLimitYear &&
            pNowTime.Month == nTimeLimitMonth &&
            pNowTime.Day == nTimeLimitDay)
        {
            CPlayerNetHelper.AddFishCoin(nOwnerUID,
                                         CGameColorFishMgr.Ins.pStaticConfig.GetInt("×Ì±À´òÕÛ¼Û¸ñ") * -1,
                                         EMFishCoinAddFunc.Free,
                                         false);
        }
        else
        {
            CPlayerNetHelper.AddFishCoin(nOwnerUID,
                                         CGameColorFishMgr.Ins.pStaticConfig.GetInt("×Ì±À¼Û¸ñ") * -1,
                                         EMFishCoinAddFunc.Free,
                                         false);
        }
    }
}
