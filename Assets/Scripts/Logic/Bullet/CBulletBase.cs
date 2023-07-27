using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBulletBase : MonoBehaviour
{
    [ReadOnly]
    public string uid;        //发送玩家
    [ReadOnly]
    public long dmg;        //伤害值
    [ReadOnly]
    public EMRare emRare;   //稀有度

    public Transform tranSelf;

    [ReadOnly]
    public CBossBase pBossTarget;

    [ReadOnly]
    public bool bActive = false;

    public virtual void SetInfo(CHitBossInfo info)
    {
        uid = info.uid;
        dmg = info.dmg;
        emRare = info.emRare;
        bActive = true;
    }

    public virtual void SetTarget(Vector3 start, Transform target, CBossBase boss)
    {
        pBossTarget = boss;
        tranSelf.position = start;
    }

    public virtual void DoHit()
    { 
        if(pBossTarget == null)
        {
            Recycle();
            return;
        }

        pBossTarget.OnHit(uid, dmg);
        Recycle();
    }

    void Update()
    {
        if(bActive)
        {
            OnUpdate(CTimeMgr.DeltaTime);
        }
    }

    void FixedUpdate()
    {
        if (bActive)
        {
            OnFixedUpdate(CTimeMgr.FixedDeltaTime);
        }
    }

    public virtual void OnUpdate(float delta)
    { 

    }

    public virtual void OnFixedUpdate(float delta)
    {

    }

    public virtual void Recycle()
    {
        bActive = false;
        CBossBulletMgr.Ins.RecycleBullet(this);
    }
}
