using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBulletBeizierTarget : CBulletBase
{
    public CRendererTextureLoader pIcon;

    protected List<ParticleSystem> listParticleSys = new List<ParticleSystem>();

    [ReadOnly]
    public Vector3 vStart;
    [ReadOnly]
    public Transform tranEnd;
    [ReadOnly]
    public Vector3 vCenter;
    public float fSpd;
    public float fCenterHeight;
    public float fTargetAddHeight;
    CPropertyTimer pMoveTicker = null;

    public string szBoomEff;

    bool bInited = false;

    void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        if (!bInited)
        {
            ParticleSystem self = GetComponent<ParticleSystem>();
            if (self != null)
            {
                listParticleSys.Add(self);
            }

            ParticleSystem[] arrChilds = GetComponentsInChildren<ParticleSystem>();
            listParticleSys.AddRange(arrChilds);

            StopEffect();

            bInited = true;
        }

        PlayEffect();
    }

    public virtual void PlayEffect(bool refresh = true)
    {
        listParticleSys.ForEach(t =>
        {
            if (t == null) return;

            if (refresh)
            {
                t.Simulate(0, false, true);
            }
            t.Play();
        });
    }

    public virtual void StopEffect()
    {
        listParticleSys.ForEach(t => t.Stop());
    }

    public override void SetInfo(CHitBossInfo info)
    {
        base.SetInfo(info);
        if (!CHelpTools.IsStringEmptyOrNone(info.szIcon))
        {
            pIcon.InitIcon(info.szIcon);
        }
        Init();
    }

    public override void SetTarget(Vector3 start, Transform target, CBossBase boss)
    {
        base.SetTarget(start, target, boss);

        vStart = start;
        tranEnd = target;
        vCenter = (tranEnd.position + vStart) * 0.5F + Vector3.up * fCenterHeight;

        float fMoveTime = (tranEnd.position - vStart).magnitude / fSpd;
        pMoveTicker = new CPropertyTimer();
        pMoveTicker.Value = fMoveTime;
        pMoveTicker.FillTime();
    }

    public override void OnFixedUpdate(float delta)
    {
        if (pMoveTicker == null) return;

        if (tranEnd == null)
        {
            pMoveTicker = null;
            Recycle();
            return;
        }

        if (pMoveTicker.Tick(CTimeMgr.FixedDeltaTime))
        {
            pMoveTicker = null;

            tranSelf.position = (tranEnd.position + Vector3.up * fTargetAddHeight);

            CEffectMgr.Instance.CreateEffSync(szBoomEff, tranSelf, 0);

            //´´ÔìÉËº¦Êý×ÖUI
            ShowDmgUI();
            DoHit();

            //Recycle();
        }
        else
        {
            tranSelf.position = CHelpTools.GetCurvePoint(vStart, vCenter, (tranEnd.position + Vector3.up * fTargetAddHeight), 1F - pMoveTicker.GetTimeLerp());
        }
    }

    void ShowDmgUI()
    {
        Vector3 vUIPos = tranEnd.position + Vector3.up * fTargetAddHeight;
        UIBossBaseInfo uiBossUI = UIManager.Instance.GetUI(UIResType.BossBaseInfo) as UIBossBaseInfo;
        if (uiBossUI == null) return;
        uiBossUI.AddDmgTip(dmg, EMDmgType.Normal, emRare, vUIPos + Vector3.right * Random.Range(-2F, 2F));
    }

    public override void Recycle()
    {
        StopEffect();
        base.Recycle();
    }
}
