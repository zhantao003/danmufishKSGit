using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTentacleUnit : MonoBehaviour
{
    public enum EMState
    {
        Show,       //出生
        Stay,       //等待
        Hit,        //攻击
        Recycle     //回收
    }

    public CPlayerUnit pTarget;

    public Animation anima;

    public EMState emState;

    Vector3 vecShowStart;
    Vector3 vecShowEnd;

    public float fShowTime;
    public float fStayTime = 10f;
    public float fHitTime;
    public float fRecycleTime;
    CPropertyTimer pCheckTick;

    public float fHitPlayerTime;
    CPropertyTimer pHitTick;

    public void Show(CPlayerUnit target)
    {
        pTarget = target;
        emState = EMState.Show;
        if (pTarget != null && pTarget.pMapSlot != null)
        {
            vecShowEnd = pTarget.pMapSlot.tranSelf.position + pTarget.tranBody.forward * 2f;
            vecShowStart = vecShowEnd - Vector3.up * 2f;
            transform.position = vecShowStart;
            Vector3 vecForward = (vecShowEnd - pTarget.tranBody.position).normalized;
            
            transform.forward = vecForward;
            transform.Rotate(new Vector3(0, -90, 0));
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
        }
        
        ResetState();
    }

    private void Update()
    {
        if(pCheckTick != null)
        {
            if(pCheckTick.Tick(CTimeMgr.DeltaTime))
            {
                if (emState == EMState.Show)
                {
                    emState = EMState.Stay;
                    ResetState();
                }
                else if(emState == EMState.Stay)
                {
                    emState = EMState.Hit;
                    ResetState();
                }
                else if (emState == EMState.Hit)
                {
                    emState = EMState.Recycle;
                    ResetState();
                }
                else if (emState == EMState.Recycle)
                {
                    CTentacleMgr.Ins.RecycleTentacle(this);
                    pCheckTick = null;
                }
            }
            else
            {
                if (emState == EMState.Show)
                {
                    transform.position = Vector3.Lerp(vecShowStart, vecShowEnd, 1f - pCheckTick.GetTimeLerp());
                }
                else if (emState == EMState.Recycle)
                {
                    transform.position = Vector3.Lerp(vecShowStart, vecShowEnd, 1f - pCheckTick.GetTimeLerp());
                }
            }
        }
        if(pHitTick != null &&
           pHitTick.Tick(CTimeMgr.DeltaTime))
        {
            pHitTick = null;
            if (pTarget == null)
                return;
            ///正在拍卖的人无法上岸
            if (CHelpTools.CheckAuctionByUID(pTarget.uid))
                return;
            ///有渔具的人无法上岸
            if (pTarget.pInfo != null &&
               (pTarget.pInfo.nlBaitCount > 0 ||
                pTarget.pInfo.nlFeiLunCount > 0))
                return;
            ///判断是否在欧皇船上
            if (CGameColorFishMgr.Ins.nlCurOuHuangUID == pTarget.uid)
                return;
            ///判断是否是主播
            if (CPlayerMgr.Ins.pOwner != null &&
                CPlayerMgr.Ins.pOwner.uid == pTarget.uid)
                return;
            ///判断是否是官方或者总督以上
            if (pTarget.pInfo.nVipPlayer >= CPlayerBaseInfo.EMVipLv.Pro)
            {
                return;
            }
            if (pTarget != null &&
                pTarget.emCurState == CPlayerUnit.EMState.Idle ||
                pTarget.emCurState == CPlayerUnit.EMState.StartFish ||
                pTarget.emCurState == CPlayerUnit.EMState.Fishing ||
                pTarget.emCurState == CPlayerUnit.EMState.ShowFish ||
                pTarget.emCurState == CPlayerUnit.EMState.EndFish ||
                pTarget.emCurState == CPlayerUnit.EMState.Battle ||
                pTarget.listNormalBooms.Count > 0 ||
                pTarget.listTreasureBooms.Count > 0)
            {
                pTarget.SetState(CPlayerUnit.EMState.HitDrop);
            }
        }
    }

    /// <summary>
    /// 设置对应的状态
    /// </summary>
    public void ResetState()
    {
        if(emState == EMState.Show)
        {
            anima.Play("Tentacle|Idle01");
            anima["Tentacle|Idle01"].speed = 1f;
            pCheckTick = new CPropertyTimer();
            pCheckTick.Value = fShowTime;
            pCheckTick.FillTime();
        }
        else if (emState == EMState.Stay)
        {
            anima.Play("Tentacle|Idle01");
            anima["Tentacle|Idle01"].speed = 1f;
            pCheckTick = new CPropertyTimer();
            pCheckTick.Value = fStayTime;
            pCheckTick.FillTime();
        }
        else if (emState == EMState.Hit)
        {
            anima.Play("Tentacle|Attack");
            anima["Tentacle|Attack"].speed = 2f;
            pCheckTick = new CPropertyTimer();
            pCheckTick.Value = fHitTime;
            pCheckTick.FillTime();
            pHitTick = new CPropertyTimer();
            pHitTick.Value = fHitPlayerTime;
            pHitTick.FillTime();
            
        }
        else if (emState == EMState.Recycle)
        {
            anima.Play("Tentacle|Idle01");
            anima["Tentacle|Idle01"].speed = 1f;
            vecShowEnd = transform.position - Vector3.up * 2f;
            vecShowStart = transform.position;
            pCheckTick = new CPropertyTimer();
            pCheckTick.Value = fRecycleTime;
            pCheckTick.FillTime();
        }
    }


}
