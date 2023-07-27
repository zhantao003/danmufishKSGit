using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameItemBomber : MonoBehaviour
{
    public float fStartSpd;
    public float fGravity;

    public Vector2Int[] arrRange;
    public string szEffExplosion;
    public CAudioMgr.CAudioSlottInfo pAudioBoom;

    protected Transform tranSelf;
    protected float fSpeed;
    protected float fTargetYPos;

    CPlayerBaseInfo pOwner;
    //CGameMapSlot pTargetMapSlot;

    //public DelegateNFuncCall dlgActionOver;

    //public void SetTarget(CGameMapSlot mapSlot, CPlayerBaseInfo player)
    //{
    //    tranSelf = GetComponent<Transform>();

    //    pTargetMapSlot = mapSlot;
    //    pOwner = player;

    //    fSpeed = fStartSpd;
    //}

    //private void FixedUpdate()
    //{
    //    tranSelf.position += Vector3.up * fSpeed * CTimeMgr.FixedDeltaTime;
    //    fSpeed -= fGravity * CTimeMgr.FixedDeltaTime;

    //    if (tranSelf.position.y <= fTargetYPos)
    //    {
    //        DoAction();
    //        Destroy(gameObject);
    //    }
    //}

    //protected void DoAction()
    //{
    //    CAudioMgr.Ins.PlaySoundBySlot(pAudioBoom, tranSelf.position);

    //    for(int i=0; i<arrRange.Length; i++)
    //    {
    //        Vector2Int vCheckMapSlotIdx = new Vector2Int(pTargetMapSlot.nX + arrRange[i].x,
    //                                                     pTargetMapSlot.nY + arrRange[i].y);

    //        CGameMapSlot pCheckMapSlot = CGameColorFlagMgr.Ins.pMap.GetMapSlot(vCheckMapSlotIdx.x, vCheckMapSlotIdx.y);
    //        if (pCheckMapSlot == null) continue;

    //        //创建特效
    //        CEffectMgr.Instance.CreateEffSync(szEffExplosion + "_" + pOwner.pGameInfo.emColor.ToString().ToLower(), pCheckMapSlot.tranSelf, 0, false);

    //        //被插旗的不能炸
    //        if (pCheckMapSlot.bFlag ||
    //            pCheckMapSlot.lOwnerGuid == pOwner.uid) continue;

    //        pCheckMapSlot.SetUser(pOwner.uid, true);
    //    }

    //    dlgActionOver?.Invoke();
    //}
}
