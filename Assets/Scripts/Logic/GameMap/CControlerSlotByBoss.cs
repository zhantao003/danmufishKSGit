using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CControlerSlotByBoss : MonoBehaviour
{
    public enum SlotDirection
    {
        Left,
        Right,
        Up,
        Down,
    }

    public string nlBindUID;

    public CMapSlot pOwner;

    [Header("位移目标")]
    public Transform tranMoveRoot;

    public Rigidbody tranMoveRigi;

    public float fMoveVelocity = 5f;

    CPropertyTimer pMoveTick;

    Vector3 vMoveVelocity;

    /// <summary>
    /// 移动
    /// </summary>
    /// <param 移动方向="direction"></param>
    public void Move(SlotDirection direction)
    {
        //if (pMoveTick != null)
        //{
        //    return;
        //}
        vMoveVelocity = Vector3.zero;
        ///判断移动方向
        switch (direction)
        {
            case SlotDirection.Left:
                {
                    vMoveVelocity = new Vector3(fMoveVelocity, 0, 0);
                }
                break;
            case SlotDirection.Right:
                {
                    vMoveVelocity = new Vector3(-fMoveVelocity, 0, 0);
                }
                break;
            case SlotDirection.Up:
                {
                    vMoveVelocity = new Vector3(0, 0, -fMoveVelocity);
                }
                break;
            case SlotDirection.Down:
                {
                    vMoveVelocity = new Vector3(0, 0, fMoveVelocity);
                }
                break;
        }

        tranMoveRigi.velocity = vMoveVelocity;
        ///生成移动计时器
        pMoveTick = new CPropertyTimer();
        pMoveTick.Value = 2f;
        pMoveTick.FillTime();
    }

    private void FixedUpdate()
    {
        if (tranMoveRigi == null) return;
        if (pMoveTick != null)
        {
            if(pMoveTick.Tick(CTimeMgr.FixedDeltaTime))
            {
                pMoveTick = null;
                tranMoveRigi.velocity = Vector3.zero;
            }
            else
            {
                tranMoveRigi.velocity = Vector3.Lerp(vMoveVelocity, Vector3.zero, 1f - pMoveTick.GetTimeLerp());
            }
        }
    }

    ///// <summary>
    ///// 移动
    ///// </summary>
    ///// <param 移动方向="direction"></param>
    //public void Move(SlotDirection direction)
    //{
    //    vStartPos = tranMoveRoot.localPosition;
    //    ///判断移动方向
    //    switch (direction)
    //    {
    //        case SlotDirection.Left:
    //            {
    //                vEndPos = vStartPos + new Vector3(fMoveDistance, 0, 0);
    //            }
    //            break;
    //        case SlotDirection.Right:
    //            {
    //                vEndPos = vStartPos - new Vector3(fMoveDistance, 0, 0);
    //            }
    //            break;
    //        case SlotDirection.Up:
    //            {
    //                vEndPos = vStartPos - new Vector3(0, 0, fMoveDistance);
    //            }
    //            break;
    //        case SlotDirection.Down:
    //            {
    //                vEndPos = vStartPos + new Vector3(0, 0, fMoveDistance);
    //            }
    //            break;
    //    }
    //    ///检测X轴是否出界
    //    if(vEndPos.x < CControlerSlotMgr.Ins.vecMapRangeX.x)
    //    {
    //        vEndPos.x = CControlerSlotMgr.Ins.vecMapRangeX.x;
    //    }
    //    else if (vEndPos.x > CControlerSlotMgr.Ins.vecMapRangeX.y)
    //    {
    //        vEndPos.x = CControlerSlotMgr.Ins.vecMapRangeX.y;
    //    }
    //    ///检测Y轴是否出界
    //    if (vEndPos.z < CControlerSlotMgr.Ins.vecMapRangeY.x)
    //    {
    //        vEndPos.z = CControlerSlotMgr.Ins.vecMapRangeY.x;
    //    }
    //    else if (vEndPos.z > CControlerSlotMgr.Ins.vecMapRangeY.y)
    //    {
    //        vEndPos.z = CControlerSlotMgr.Ins.vecMapRangeY.y;
    //    }
    //    ///生成移动计时器
    //    pMoveTick = new CPropertyTimer();
    //    pMoveTick.Value = Vector3.Distance(vStartPos, vEndPos) / fMoveSpeed;
    //    pMoveTick.FillTime();

    //}

    //private void FixedUpdate()
    //{
    //    if (tranMoveRoot == null) return;
    //    if(pMoveTick != null)
    //    {
    //        if(pMoveTick.Tick(CTimeMgr.FixedDeltaTime))
    //        {
    //            tranMoveRoot.localPosition = vEndPos;
    //        }
    //        else
    //        {
    //            tranMoveRoot.localPosition = Vector3.Lerp(vStartPos, vEndPos, 1f - pMoveTick.GetTimeLerp());
    //        }
    //    }
    //}

}
