using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMNPCShow : FSMNPCBase
{
    Vector3 vTarget;
    //public float fJumpTime = 5F;
    CPropertyTimer pJumpTime = new CPropertyTimer();

    Vector3 vStartPos;
    Vector3 vEndPos;
    Vector3 vCenterPos;
    Vector3 vDir;

    public override void OnBegin(object obj)
    {
        pNPCUnit.emCurState = CNPCUnit.EMState.Show;

        pNPCUnit.PlayAnime(CUnitAnimeConst.Anime_Jump);
        UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (gameInfo != null)
        {
            gameInfo.showAuctionInfo.SetDmContent(gameInfo.showAuctionInfo.szShowContent);
        }
        vTarget = pNPCUnit.tranStayPos.position;
        pNPCUnit.tranSelf.position = pNPCUnit.tranStartPos.position;
        float fJumpTime = Vector3.Distance(vTarget, pNPCUnit.tranSelf.position) / pNPCUnit.fJumpSpeed;
        fJumpTime = Mathf.Max(0.5F, fJumpTime);
        pJumpTime.Value = fJumpTime;
        pJumpTime.FillTime();
        vStartPos = pNPCUnit.tranStartPos.position;
        vEndPos = vTarget;
        vCenterPos = (vEndPos + vStartPos) * 0.5F + Vector3.up * pNPCUnit.fJumpHeight;
        pNPCUnit.SetDir((vTarget - pNPCUnit.tranSelf.position).normalized * 1);

        vDir = (vEndPos - vStartPos).normalized;
        vDir.y = 0F;
        vDir = vDir.normalized;
        pNPCUnit.tranSelf.forward = vDir;

        if (pNPCUnit.pAvatar != null)
        {
            pNPCUnit.pAvatar.PlayJumpEff(true);
            pNPCUnit.pAvatar.ShowHandObj(false);
            pNPCUnit.pAvatar.ShowJumpObj(true);
        }
    }

    public override void OnFixedUpdate(object obj, float delta)
    {
        if (pJumpTime.Tick(delta))
        {
            pNPCUnit.tranSelf.position = vEndPos;
            pNPCUnit.tranSelf.rotation = pNPCUnit.tranStayPos.rotation;
            pNPCUnit.SetState(CNPCUnit.EMState.Stay);
            //检查是否有特殊的跳跃特效
            if (pNPCUnit.pAvatar != null && !CHelpTools.IsStringEmptyOrNone(pNPCUnit.pAvatar.szEffJumpEnd))
            {
                CEffectMgr.Instance.CreateEffSync(pNPCUnit.pAvatar.szEffJumpEnd, pNPCUnit.tranSelf, 0, false);
            }
        }
        else
        {
            pNPCUnit.tranSelf.position = CHelpTools.GetCurvePoint(vStartPos, vCenterPos, vEndPos, 1f - pJumpTime.GetTimeLerp());
            pNPCUnit.tranSelf.forward = vDir;
        }
    }

    public override void OnEnd(object obj)
    {
        base.OnEnd(obj);

        if (pNPCUnit.pAvatar != null)
        {
            pNPCUnit.pAvatar.PlayJumpEff(false);
            pNPCUnit.pAvatar.ShowHandObj(true);
            pNPCUnit.pAvatar.ShowJumpObj(false);
        }
    }

}

