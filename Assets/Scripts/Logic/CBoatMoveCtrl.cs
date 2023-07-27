using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBoatMoveCtrl : MonoBehaviour
{
    public enum SlotDirection 
    {
        Left,
        Right,
        Up,
        Down,
    }

    public CMapSlot pOwner;

    [Header("位移目标")]
    public Transform tranMoveRoot;

    public float fMoveVelocity = 5f;
    public float fMoveTime = 2.5F;

    CPropertyTimer pMoveTick;
    Vector3 vMoveVelocity;

    public float fMovePathSpeed = 4F;
    public List<Vector3> listMovePath = new List<Vector3>();
    Vector3 vMovePathDir;
    float fMovePathCurDis = 0F;
    int nCurMovePathIdx = -1;

    /// <summary>
    /// 移动
    /// </summary>
    /// <param 移动方向="direction"></param>
    public void Move(SlotDirection direction)
    {
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

        ///生成移动计时器
        pMoveTick = new CPropertyTimer();
        pMoveTick.Value = fMoveTime;
        pMoveTick.FillTime();
    }

    private void FixedUpdate()
    {
        if (listMovePath.Count <= 0)
        {
            if (pMoveTick != null)
            {
                if (pMoveTick.Tick(CTimeMgr.FixedDeltaTime))
                {
                    pMoveTick = null;
                    vMoveVelocity = Vector3.zero;
                }
                else
                {
                    vMoveVelocity = Vector3.Lerp(vMoveVelocity, Vector3.zero, 1f - pMoveTick.GetTimeLerp());
                }

                tranMoveRoot.position += vMoveVelocity * CTimeMgr.FixedDeltaTime;

                CheckBoard();
            }
        }
        else
        {
            vMovePathDir = (listMovePath[nCurMovePathIdx] - tranMoveRoot.position).normalized;
            fMovePathCurDis = fMovePathSpeed * CTimeMgr.FixedDeltaTime;
            if((listMovePath[nCurMovePathIdx] - tranMoveRoot.position).sqrMagnitude <= fMovePathCurDis*fMovePathCurDis)
            {
                tranMoveRoot.position = listMovePath[nCurMovePathIdx];
                nCurMovePathIdx++;
                if(nCurMovePathIdx >= listMovePath.Count)
                {
                    listMovePath.Clear();
                    nCurMovePathIdx = -1;
                }
            }
            else
            {
                tranMoveRoot.position += vMovePathDir * fMovePathCurDis;
            }
        }
    }

    public void SetMovePath(Vector3[] arrPos)
    {
        listMovePath.Clear();
        for (int i=0; i<arrPos.Length; i++)
        {
            listMovePath.Add(arrPos[i]);
        }

        if(listMovePath.Count > 0)
        {
            nCurMovePathIdx = 0;
        }
    }

    void CheckBoard()
    {
        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive)
        {
            if (pOwner.pBindUnit == null) return;

            if (CGameSurviveMap.Ins.emCurState == CGameSurviveMap.EMGameState.Ready)
            {
                if (tranMoveRoot.position.x < CGameSurviveMap.Ins.tranMinBoard.position.x)
                {
                    tranMoveRoot.position = new Vector3(CGameSurviveMap.Ins.tranMinBoard.position.x, tranMoveRoot.position.y, tranMoveRoot.position.z);
                }
                else if (tranMoveRoot.position.x > CGameSurviveMap.Ins.tranMaxBoard.position.x)
                {
                    tranMoveRoot.position = new Vector3(CGameSurviveMap.Ins.tranMaxBoard.position.x, tranMoveRoot.position.y, tranMoveRoot.position.z);
                }

                if (tranMoveRoot.position.z < CGameSurviveMap.Ins.tranMinBoard.position.z)
                {
                    tranMoveRoot.position = new Vector3(tranMoveRoot.position.x, tranMoveRoot.position.y, CGameSurviveMap.Ins.tranMinBoard.position.z);
                }
                else if (tranMoveRoot.position.z > CGameSurviveMap.Ins.tranMaxBoard.position.z)
                {
                    tranMoveRoot.position = new Vector3(tranMoveRoot.position.x, tranMoveRoot.position.y, CGameSurviveMap.Ins.tranMaxBoard.position.z);
                }
            }
            else if(CGameSurviveMap.Ins.emCurState == CGameSurviveMap.EMGameState.Gaming)
            {

            }
        }
        else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal ||
                 CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
        {
            if (tranMoveRoot.position.x < CGameColorFishMgr.Ins.pMap.tranBoundMin.position.x)
            {
                tranMoveRoot.position = new Vector3(CGameColorFishMgr.Ins.pMap.tranBoundMin.position.x, tranMoveRoot.position.y, tranMoveRoot.position.z);
            }
            else if (tranMoveRoot.position.x > CGameColorFishMgr.Ins.pMap.tranBoundMax.position.x)
            {
                tranMoveRoot.position = new Vector3(CGameColorFishMgr.Ins.pMap.tranBoundMax.position.x, tranMoveRoot.position.y, tranMoveRoot.position.z);
            }

            if (tranMoveRoot.position.z < CGameColorFishMgr.Ins.pMap.tranBoundMin.position.z)
            {
                tranMoveRoot.position = new Vector3(tranMoveRoot.position.x, tranMoveRoot.position.y, CGameColorFishMgr.Ins.pMap.tranBoundMin.position.z);
            }
            else if (tranMoveRoot.position.z > CGameColorFishMgr.Ins.pMap.tranBoundMax.position.z)
            {
                tranMoveRoot.position = new Vector3(tranMoveRoot.position.x, tranMoveRoot.position.y, CGameColorFishMgr.Ins.pMap.tranBoundMax.position.z);
            }
        }
    }
}
