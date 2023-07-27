using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGiftUnit : CNPCUnit
{
    public string szThrowGiftEffect;

    public override void Init(int avatarID)
    {
        if (!bInit)
        {
            InitFSM();
            UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
            if (gameInfo != null)
            {
                gameInfo.SetGiftInfo(this);
            }
            bInit = true;
        }
        SetState(EMState.Idle);
    }

    public override void InitAvatar()
    {

    }

    /// <summary>
    /// 向目标丢红包特效
    /// </summary>
    /// <param name="tranTarget"></param>
    public void PlayGiftEffectToTarget(CPlayerUnit target,bool bBigReward,float fDelay)
    {
        if (target == null) return;
        CTimeTickMgr.Inst.PushTicker(fDelay, delegate (object[] objs)
         {
             string szAdd = string.Empty;
             if (bBigReward)
             {
                 szAdd = "Big";
             }
             if (!CHelpTools.IsStringEmptyOrNone(szThrowGiftEffect))
             {
                 CEffectMgr.Instance.CreateEffSync("Effect/" + szThrowGiftEffect + szAdd, tranSelf.position, Quaternion.identity, 0,
                 delegate (GameObject value)
                 {
                     CEffectBeizier pEffBeizier = value.GetComponent<CEffectBeizier>();
                     if (pEffBeizier == null) return;
                     if (target == null) return;
                     pEffBeizier.SetTarget(tranSelf.position + Vector3.up * 1.5F, target.tranSelf, delegate ()
                     {
                         UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
                         if (gameInfo != null &&
                             target != null)
                         {
                             UIShowFishInfo showFishInfo = gameInfo.GetShowFishInfoSlot(target.uid);
                             if (showFishInfo != null)
                             {
                                 showFishInfo.ShowGiftInfo();
                             }
                         }
                     });
                 });
             }
         });
       
    }

}
