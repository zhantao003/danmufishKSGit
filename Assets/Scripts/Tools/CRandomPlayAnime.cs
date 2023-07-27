using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRandomPlayAnime : MonoBehaviour
{
    public Animator pAnimeCtrl;

    public string[] arrAnime;
    public CEffectOnlyCtrl pEff;

    public CPropertyTimer pTimeTicker = new CPropertyTimer();

    // Start is called before the first frame update
    void Start()
    {
        pTimeTicker.FillTime();
    }

    // Update is called once per frame
    void Update()
    {
        if(pTimeTicker.Tick(CTimeMgr.DeltaTime))
        {
            if(arrAnime!= null && 
               arrAnime.Length > 0)
            {
                pAnimeCtrl.CrossFade(arrAnime[Random.Range(0, arrAnime.Length)], 0.1F);
            }

            if(pEff!=null)
            {
                pEff.Play();
            }

            pTimeTicker.FillTime();
        }
    }
}
