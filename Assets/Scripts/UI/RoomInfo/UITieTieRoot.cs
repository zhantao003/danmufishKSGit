using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITieTieRoot : MonoBehaviour
{
    public float fHidePos; 
    public float fShowPos;
    public UITweenPos uiTween;

    public float fAutoHideTime = 5F;
    CPropertyTimer pTickerAutoHide;

    // Start is called before the first frame update
    void Start()
    {
        pTickerAutoHide = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(pTickerAutoHide != null &&
           pTickerAutoHide.Tick(CTimeMgr.DeltaTime))
        {
            pTickerAutoHide = null;
            Show(false);
        }
    }

    public void Show(bool value)
    {
        uiTween.from = transform.localPosition;
        uiTween.to = transform.localPosition;
        uiTween.to.y = value ? fShowPos:fHidePos;
        uiTween.Play();

        if (value)
        {
            pTickerAutoHide = new CPropertyTimer();
            pTickerAutoHide.Value = fAutoHideTime;
            pTickerAutoHide.FillTime();
        }
    }

    public void DoTitTie()
    {
        ///根据uid获取玩家单位
        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(CGameColorFishMgr.Ins.nlCurOuHuangUID);
        if (pUnit == null)
        {
            pUnit = CPlayerMgr.Ins.GetActiveUnit(CGameColorFishMgr.Ins.nlCurOuHuangUID);
            if (pUnit == null)
                return;
        }

        ///决斗中禁止贴贴
        if (pUnit.emCurState == CPlayerUnit.EMState.Battle ||
            pUnit.pShowFishEndEvent != null)
        {
            return;
        }

        //判断主播
        CPlayerUnit pOwnerUnit = CPlayerMgr.Ins.GetIdleUnit(CPlayerMgr.Ins.pOwner.uid);
        if (pOwnerUnit == null)
        {
            pOwnerUnit = CPlayerMgr.Ins.GetActiveUnit(CPlayerMgr.Ins.pOwner.uid);
            if (pOwnerUnit == null)
                return;
        }

        ///决斗中禁止贴贴
        if (pOwnerUnit.emCurState == CPlayerUnit.EMState.Battle ||
            pOwnerUnit.pShowFishEndEvent != null)
        {
            return;
        }

        //判断欧皇是否需要跳
        if (CGameColorFishMgr.Ins.pMap.pOuHuangSlot.pBindUnit == null ||
           CGameColorFishMgr.Ins.pMap.pOuHuangSlot.pBindUnit.uid != pUnit.uid)
        {
            pUnit.JumpTarget(CGameColorFishMgr.Ins.pMap.pOuHuangSlot);
        }

        //判断主播是否跳过去
        if (CGameColorFishMgr.Ins.pMap.pTietieSlot.pBindUnit == null)
        {
            pOwnerUnit.dlgCallOnceJumpEvent = delegate ()
            {
                Vector3 vCenterPos = (CGameColorFishMgr.Ins.pMap.pOuHuangSlot.tranSelf.position + CGameColorFishMgr.Ins.pMap.pTietieSlot.tranSelf.position) * 0.5F;
                CEffectMgr.Instance.CreateEffSync("Effect/effHeartBoom", vCenterPos, Quaternion.identity, 0);
            };

            pOwnerUnit.JumpTarget(CGameColorFishMgr.Ins.pMap.pTietieSlot);
        }
        else
        {
            if (CGameColorFishMgr.Ins.pMap.pTietieSlot.pBindUnit.uid != pOwnerUnit.uid)
            {
                CPlayerUnit pOldUnit = CGameColorFishMgr.Ins.pMap.pTietieSlot.pBindUnit;
                CMapSlot pOwnerSlot = pOwnerUnit.pMapSlot;

                pOwnerUnit.dlgCallOnceJumpEvent = delegate ()
                {
                    Vector3 vCenterPos = (CGameColorFishMgr.Ins.pMap.pOuHuangSlot.tranSelf.position + CGameColorFishMgr.Ins.pMap.pTietieSlot.tranSelf.position) * 0.5F;
                    CEffectMgr.Instance.CreateEffSync("Effect/effHeartBoom", vCenterPos, Quaternion.identity, 0);
                };

                //交换位置
                pOwnerUnit.JumpTarget(CGameColorFishMgr.Ins.pMap.pTietieSlot);
                pOldUnit.pMapSlot = null;
                if (pOwnerSlot != null)
                {
                    pOldUnit.JumpTarget(pOwnerSlot);
                }
            }
        }
    }

    public void OnClickOK()
    {
        Show(false);

        DoTitTie();
    }

    public void OnClickNo()
    {
        Show(false);
    }
}
