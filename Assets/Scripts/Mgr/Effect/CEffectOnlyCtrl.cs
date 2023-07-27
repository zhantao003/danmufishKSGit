using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEffectOnlyCtrl : CEffectBase
{
    public GameObject[] arrShowObjs;

    public override void Init()
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

            TrailRenderer selfTrail = GetComponent<TrailRenderer>();
            if (selfTrail != null)
            {
                listTrail.Add(selfTrail);
            }

            Animator selfAnime = GetComponent<Animator>();
            if (selfAnime != null)
            {
                listAnimator.Add(selfAnime);
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

            Animator[] arrAnimation = GetComponentsInChildren<Animator>();
            listAnimator.AddRange(arrAnimation);

            TrailRenderer[] arrTrailChilds = GetComponentsInChildren<TrailRenderer>();
            listTrail.AddRange(arrTrailChilds);

            for (int i = 0; i < arrShowObjs.Length; i++)
            {
                arrShowObjs[i].SetActive(false);
            }

            bInited = true;
        }
    }

    public override void Play(bool refresh = true)
    {
        for (int i = 0; i < arrShowObjs.Length; i++)
        {
            arrShowObjs[i].SetActive(true);
        }

        base.Play(refresh);
    }

    public override void StopEffect()
    {
        base.StopEffect();

        for (int i = 0; i < arrShowObjs.Length; i++)
        {
            arrShowObjs[i].SetActive(false);
        }
    }

    public override void Recycle()
    {
        for (int i = 0; i < arrShowObjs.Length; i++)
        {
            arrShowObjs[i].SetActive(false);
        }

        base.Recycle();
    }
}
