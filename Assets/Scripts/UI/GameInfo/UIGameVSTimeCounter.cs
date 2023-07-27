using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameVSTimeCounter : MonoBehaviour
{
    public Text uiLabelTime;

    // Start is called before the first frame update
    void Start()
    {
        //RefreshTime((int)CGameVSGiftPoolMgr.Ins.pGameTimeTicker.CurValue);
    }

    // Update is called once per frame
    void Update()
    {
        //RefreshTime((int)CGameVSGiftPoolMgr.Ins.pGameTimeTicker.CurValue);
    }

    void RefreshTime(int time)
    {
        //uiLabelTime.text = CTimeMgr.SecToHHMMSS(time);
    }
}
