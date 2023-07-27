using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CEffectSoundSlot
{
    public bool bPlay = false;
    public bool bAudioSpecCtrl = false;
    public CPropertyTimer pTimeTicker = new CPropertyTimer();
    public CAudioMgr.CAudioSlottInfo pAudio;

    public void Init()
    {
        bPlay = true;
        pTimeTicker.FillTime();
    }

    public void OnUpdate(float dt, Vector3 pos)
    {
        if(bPlay && 
           pTimeTicker.Tick(dt))
        {
            if(bAudioSpecCtrl)
            {
                if (CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.SPECIALSOUND) > 0)
                    CAudioMgr.Ins.PlaySoundBySlot(pAudio, pos);
            }
            else
            {
                CAudioMgr.Ins.PlaySoundBySlot(pAudio, pos);
            }
           
            bPlay = false;
        }
    }
}

public class CEffectBase : MonoBehaviour
{
    [ReadOnly]
    public long lGuid;

    public bool bAutoRecycle = false;
    public CPropertyTimer pLifeTicker = new CPropertyTimer();
    public CEffectSoundSlot[] arrEffAudio;

    public UITweenBase[] uiTweens;

    [ReadOnly]
    public bool bPlayAudio = false;

    [ReadOnly]
    public bool bPlaying = false;

    [ReadOnly]
    public bool bInited = false;

    protected List<SpriteRenderer> listSprites = new List<SpriteRenderer>();
    protected List<int> listSpritesOriginLayer = new List<int>();

    protected List<ParticleSystem> listParticleSys = new List<ParticleSystem>();
    protected List<int> listParticleOriginLayer = new List<int>();

    protected List<TrailRenderer> listTrail = new List<TrailRenderer>();

    protected List<Animator> listAnimator = new List<Animator>();

    public DelegateNFuncCall pEndEvent;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Update()
    {
        OnUpdate(CTimeMgr.DeltaTime);
    }

    protected virtual void OnUpdate(float dt)
    {
        if (bAutoRecycle && bPlaying)
        {
            if (pLifeTicker.Tick(dt))
            {
                bPlaying = false;
                Recycle();
            }
        }

        for(int i=0; i<arrEffAudio.Length; i++)
        {
            arrEffAudio[i].OnUpdate(dt, transform.position);
        }

        //if(bPlayAudio && pEffDelayTicker.Tick(dt))
        //{
        //    CAudioMgr.Ins.PlaySoundBySlot(pAudioPlay, transform.position);
        //    bPlayAudio = false;
        //}
    }

    public virtual void Init()
    {
        enabled = true;

        if (!bInited)
        {
            lGuid = CEffectMgr.Instance.GenerateID();

            SpriteRenderer selfSprite = GetComponent<SpriteRenderer>();
            if (selfSprite != null)
            {
                listSprites.Add(selfSprite);
                listSpritesOriginLayer.Add(selfSprite.sortingOrder);
            }

            ParticleSystem self = GetComponent<ParticleSystem>();
            if (self != null)
            {
                listParticleSys.Add(self);
                listParticleOriginLayer.Add(self.GetComponent<Renderer>().sortingOrder);
            }

            Animator selfAnime = GetComponent<Animator>();
            if (selfAnime != null)
            {
                listAnimator.Add(selfAnime);
            }

            TrailRenderer selfTrail = GetComponent<TrailRenderer>();
            if (selfTrail != null)
            {
                listTrail.Add(selfTrail);
            }

            SpriteRenderer[] arrSprites = GetComponentsInChildren<SpriteRenderer>();
            listSprites.AddRange(arrSprites);
            for (int i = 0; i < arrSprites.Length; i++)
            {
                listSpritesOriginLayer.Add(arrSprites[i].sortingOrder);
            }

            ParticleSystem[] arrChilds = GetComponentsInChildren<ParticleSystem>();
            listParticleSys.AddRange(arrChilds);
            for (int i = 0; i < arrChilds.Length; i++)
            {
                listParticleOriginLayer.Add(arrChilds[i].GetComponent<Renderer>().sortingOrder);
            }

            TrailRenderer[] arrTrailChilds = GetComponentsInChildren<TrailRenderer>();
            listTrail.AddRange(arrTrailChilds);

            Animator[] arrAnimation = GetComponentsInChildren<Animator>();
            listAnimator.AddRange(arrAnimation);

            bInited = true;
        }

        Play();
    }

    public virtual void Recycle()
    {
        pEndEvent?.Invoke();
        //Debug.Log("回收特效：" + lGuid);
        CEffectMgr.Instance.Recycle(this);
    }

    public virtual void SetLayer(int layer)
    {
        for (int i = 0; i < listSprites.Count; i++)
        {
            listSprites[i].sortingOrder = listSpritesOriginLayer[i] + layer;
        }

        for (int i = 0; i < listParticleSys.Count; i++)
        {
            listParticleSys[i].GetComponent<Renderer>().sortingOrder = listParticleOriginLayer[i] + layer;
        }
    }

    /// <summary>
    /// 暂停播放
    /// </summary>
    [ContextMenu("Stop")]
    public virtual void StopEffect()
    {
        listParticleSys.ForEach(t => t.Stop());
        listAnimator.ForEach(t =>
        {
            t.speed = 0;
        });

        listTrail.ForEach(t =>
        {
            if (t == null) return;
            t.enabled = false;
        });

        bPlaying = false;
    }

    [ContextMenu("Pause")]
    public virtual void PauseEffect()
    {
        listParticleSys.ForEach(t => t.Pause());
        listAnimator.ForEach(t =>
        {
            t.speed = 0;
        });

        bPlaying = false;
    }

    /// <summary>
    /// 播放粒子特效
    /// </summary>
    [ContextMenu("Play")]
    public virtual void Play(bool refresh = true)
    {
        bPlaying = true;

        listParticleSys.ForEach(t =>
        {
            if (t == null) return;

            if (refresh)
            {
                t.Simulate(0, false, true);
            }
            t.Play();
        });

        for(int i = 0;i < uiTweens.Length;i++)
        {
            if (uiTweens[i] == null) continue;
            uiTweens[i].Play();
        }

        listAnimator.ForEach(t =>
        {
            if (t == null) return;
            t.speed = 1;
            if (refresh)
            {
                t.Play(t.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, 0f);
            }
        });

        listTrail.ForEach(t =>
        {
            if (t == null) return;
            t.enabled = true;
        });

        if (refresh)
        {
            for (int i = 0; i < arrEffAudio.Length; i++)
            {
                arrEffAudio[i].Init();
            }

            bPlayAudio = true;
        }

        if (bAutoRecycle && refresh)
        {
            pLifeTicker.FillTime();
        }
    }
}
