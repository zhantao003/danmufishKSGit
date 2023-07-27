using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITweenAlpha : UITweenBase
{
    public float from;
    public float to;
    public Graphic[] objTargets;
    public ParticleSystem particleSystem;

    protected Color[] colorPlay;

    public bool bInited = false;

    private void Start()
    {
        Init();
    }

    void Init()
    {
        if (bInited) return;
        int nColorLength = objTargets.Length;
        if (particleSystem != null)
        {
            nColorLength += 1;
        }
        colorPlay = new Color[nColorLength];
        for (int i = 0; i < objTargets.Length; i++)
        {
            colorPlay[i] = objTargets[i].color;
            colorPlay[i].a = from;
            objTargets[i].color = colorPlay[i];
        }

        if(particleSystem != null)
        {
            colorPlay[objTargets.Length] = particleSystem.startColor;
            colorPlay[objTargets.Length].a = from;
            particleSystem.startColor = colorPlay[objTargets.Length];
        }

        bInited = true;
    }

    public override void Play(DelegateNFuncCall call = null)
    {
        Init();

        base.Play(call);

        for(int i = 0;i < objTargets.Length;i++)
        {
            colorPlay[i] = objTargets[i].color;
            colorPlay[i].a = from;
            objTargets[i].color = colorPlay[i];
        }
        if (particleSystem != null)
        {
            colorPlay[objTargets.Length] = particleSystem.startColor;
            colorPlay[objTargets.Length].a = from;
            particleSystem.startColor = colorPlay[objTargets.Length];
        }
    }

    protected override void Refresh(float lerp)
    {
        base.Refresh(lerp);

        
        for (int i = 0; i < objTargets.Length; i++)
        {
            colorPlay[i].a = from * (1 - curValue) + to * curValue;
            objTargets[i].color = colorPlay[i];
        }

        if (particleSystem != null)
        {
            particleSystem.startColor = colorPlay[objTargets.Length];
        }
    }

    [ContextMenu("GetTarget")]
    public void FindTweenRoot()
    {
        objTargets = new Graphic[2];
        objTargets[0] = transform.GetChild(0).GetChild(0).GetComponent<Graphic>();
        objTargets[1] = transform.GetChild(0).GetChild(1).GetComponent<Graphic>();
    }
}
