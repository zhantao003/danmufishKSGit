using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpecialWarning : UIBase
{
    public CAudioMgr.CAudioSlottInfo pWarningSource;
    public CSceneRoot sceneRoot;

    CPropertyTimer pLifeTick;
    public float fLifeTime = 5f;

    public DelegateNFuncCall deleEndEvent;

    public override void OnOpen()
    {
        base.OnOpen();
        CAudioMgr.Ins.StopMusicTrac();
        CAudioMgr.Ins.PlaySoundBySlot(pWarningSource, Vector3.zero);

        pLifeTick = new CPropertyTimer();
        pLifeTick.Value = fLifeTime;
        pLifeTick.FillTime();

    }

    private void Update()
    {
        if(pLifeTick != null &&
           pLifeTick.Tick(CTimeMgr.DeltaTime))
        {
            pLifeTick = null;
            deleEndEvent?.Invoke();
            CloseSelf();
        }
    }

    public override void OnClose()
    {
        base.OnClose();
        //if (CAudioMgr.Ins != null)
        //{
        //    CAudioMgr.Ins.ClearAllSound();
        //}
    }

}
