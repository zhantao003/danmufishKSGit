using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMUnitShowFish : FSMUnitBase
{
    CPropertyTimer pCheckTime = new CPropertyTimer();

    public override void OnBegin(object obj)
    {
        //Debug.Log("Show Fishing");

        pUnit.emCurState = CPlayerUnit.EMState.ShowFish;
        
        pUnit.PlayAnime(CUnitAnimeConst.Anime_ShowFish);

        pCheckTime.Value = pUnit.fShowFishTime;
        pCheckTime.FillTime();
    }

    public override void OnFixedUpdate(object obj, float delta)
    {
        if (pCheckTime.Tick(delta))
        {
            if (pUnit.pShowFishEndEvent != null)
            {
                //pUnit.PlaySoldFishAudio();
                pUnit.pShowFishEndEvent.Invoke();
                pUnit.pShowFishEndEvent = null;
            }
            else
            {
                if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
                {

                }
                else
                {
                    pUnit.PlaySoldFishAudio();
                }
                
                pUnit.SetState(CPlayerUnit.EMState.StartFish);
            }
            //pUnit.SetState(CPlayerUnit.EMState.Idle);
        }
    }

    public override void OnEnd(object obj)
    {
        base.OnEnd(obj);
    }

}
