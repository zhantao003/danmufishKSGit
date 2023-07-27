using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBoatEffectCtrl : MonoBehaviour
{
    public CEffectOnlyCtrl pEff;
    public Vector2 vTimeEffRange;
    public CPropertyTimer pTimeTicker = new CPropertyTimer();

    // Start is called before the first frame update
    void Start()
    {
        pTimeTicker.Value = Random.Range(vTimeEffRange.x, vTimeEffRange.y);
        pTimeTicker.ClearTime();
    }

    // Update is called once per frame
    void Update()
    {
        if(pTimeTicker.Tick(CTimeMgr.DeltaTime))
        {
            pEff.Play();

            pTimeTicker.Value = Random.Range(vTimeEffRange.x, vTimeEffRange.y);
            pTimeTicker.FillTime();
        }
    }
}
