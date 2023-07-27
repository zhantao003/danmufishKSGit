using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameIdleExpMgr : MonoBehaviour
{
    public CPropertyTimer pTimeTicker = new CPropertyTimer();

    // Start is called before the first frame update
    void Start()
    {
        pTimeTicker.Value = CGameColorFishMgr.Ins.pStaticConfig.GetInt("�һ����Ӿ���ʱ��");
        pTimeTicker.FillTime();
    }

    // Update is called once per frame
    void Update()
    {
        if(pTimeTicker.Tick(CTimeMgr.DeltaTime))
        {
            //if (CGameColorFishMgr.Ins.nCurRateUpLv > 1)
            //{
                CPlayerNetHelper.AddSceneExp(CDanmuSDKCenter.Ins.szRoomId,
                                             CDanmuSDKCenter.Ins.szRoomId,
                                             CGameColorFishMgr.Ins.pStaticConfig.GetInt("�һ����Ӿ���"),
                                             false);
            //}
            pTimeTicker.FillTime();
        }
    }
}
