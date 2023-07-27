using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerUnitSpecMgr : MonoBehaviour
{
    public CPlayerUnit pOwner;

    [HideInInspector]
    public CPropertyTimer pTickerFishGan = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddFishGanTime(int delta)
    {
        if(pTickerFishGan == null)
        {
            pTickerFishGan = new CPropertyTimer();
            pTickerFishGan.Value = delta;
            pTickerFishGan.FillTime();
        }
        else
        {
            pTickerFishGan.Value += delta;
            pTickerFishGan.CurValue += delta;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(pTickerFishGan!=null)
        {
            if(pTickerFishGan.Tick(CTimeMgr.DeltaTime))
            {
                //Debug.Log("Time:" + pTickerFishGan.CurValue);

                if(pOwner!=null)
                {
                    pOwner.pInfo.nFishGanAvatarId = 101;
                    pOwner.pInfo.RefreshProAdd();
                    pOwner.InitFishGan(pOwner.pInfo.nFishGanAvatarId);

                    //Debug.Log("ªÿ ’”„∏Õ");
                }

                pTickerFishGan = null;
            }
        }
    }
}
