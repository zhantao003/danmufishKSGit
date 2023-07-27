using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMapFireBall : MonoBehaviour
{
    public string szEffBoom;
    public float fSpeed;

    public CMapSlot pTargetSlot;

    public void SetTarget(CMapSlot target)
    {
        pTargetSlot = target;
        if(pTargetSlot == null)
        {
            Destroy(gameObject);
            return;
        }
    }

    float fMoveDis = 0F;
    Vector3 vMoveDir;
    private void FixedUpdate()
    {
        if(pTargetSlot == null)
        {
            Destroy(gameObject);
            return;
        }

        fMoveDis = fSpeed * CTimeMgr.FixedDeltaTime;
        vMoveDir = (pTargetSlot.transform.position - transform.position).normalized;
        if ((pTargetSlot.transform.position - transform.position).sqrMagnitude > fMoveDis * fMoveDis)
        {
            transform.position += vMoveDir * fMoveDis;
        }
        else
        {
            CEffectMgr.Instance.CreateEffSync(szEffBoom, transform, 0);

            if(pTargetSlot.pBindUnit!=null)
            {
                bool bAtkAble = true;
                CPlayerUnit pTargetUnit = pTargetSlot.pBindUnit;

                ///正在拍卖的人无法上岸
                if (CHelpTools.CheckAuctionByUID(pTargetUnit.uid))
                    bAtkAble = false;
                //if (CAuctionMgr.Ins != null &&
                //    CAuctionMgr.Ins.bAuctionByUID(pTargetUnit.uid))
                //    bAtkAble = false;

                //if (CAuctionMatMgr.Ins != null &&
                //    CAuctionMatMgr.Ins.pInfo != null &&
                //    CAuctionMatMgr.Ins.pInfo.uid == pTargetUnit.uid)
                //    bAtkAble = false;

                ///有渔具的人无法上岸
                if (pTargetUnit.pInfo != null &&
                   (pTargetUnit.pInfo.nlBaitCount > 0 ||
                    pTargetUnit.pInfo.nlFeiLunCount > 0))
                    bAtkAble = false;

                ///判断是否在欧皇船上
                if (CGameColorFishMgr.Ins.nlCurOuHuangUID == pTargetUnit.uid)
                    bAtkAble = false;

                ///判断是否是主播
                if (CPlayerMgr.Ins.pOwner != null &&
                    CPlayerMgr.Ins.pOwner.uid == pTargetUnit.uid)
                    bAtkAble = false;

                ///判断是否是官方或者总督以上
                if (pTargetUnit.pInfo.nVipPlayer >= CPlayerBaseInfo.EMVipLv.Pro)
                {
                    bAtkAble = false;
                }

                if (bAtkAble &&
                    pTargetUnit != null &&
                    (pTargetUnit.emCurState == CPlayerUnit.EMState.Idle ||
                     pTargetUnit.emCurState == CPlayerUnit.EMState.StartFish ||
                     pTargetUnit.emCurState == CPlayerUnit.EMState.Fishing ||
                     pTargetUnit.emCurState == CPlayerUnit.EMState.ShowFish ||
                     pTargetUnit.emCurState == CPlayerUnit.EMState.EndFish ||
                     pTargetUnit.emCurState == CPlayerUnit.EMState.Battle ||
                     pTargetUnit.listNormalBooms.Count > 0 ||
                     pTargetUnit.listTreasureBooms.Count > 0))
                {
                    pTargetUnit.SetState(CPlayerUnit.EMState.HitDrop);
                }
            }

            Destroy(gameObject);
        }
    }
}
