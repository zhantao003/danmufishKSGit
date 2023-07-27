using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMapSlot : MonoBehaviour
{
    public enum EMType
    {
        Normal,
        Ouhuang,
        Duel,

        Boss,   //Boss战钓鱼台

        Survive,
    }

    public EMType emType = EMType.Normal;

    public Transform tranSelf;

    public GameObject objBoatAvatar;

    //额外鱼群
    [ReadOnly]
    public CFishPack pFishPack;

    public CPlayerUnit pBindUnit;

    CPlayerUnit pUnitInBoat;

    public void BindPlayer(CPlayerUnit player)
    {
        if(player != null)
        {
            if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive ||
               CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal ||
               CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
            {
                if(emType== EMType.Normal)
                {
                    transform.position = CGameColorFishMgr.Ins.pMap.GetRandomPos();
                }
            }
        }
        else
        {
            if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive ||
                CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal ||
                CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
            {
                if (emType == EMType.Normal)
                {
                    if(pBindUnit!=null)
                    {
                        pBindUnit.transform.SetParent(null);
                    }

                    transform.position = CGameColorFishMgr.Ins.pMap.vPosIdle;
                }    
            }
        }

        pBindUnit = player;
    }

    /// <summary>
    /// 是否有鱼群
    /// </summary>
    /// <returns></returns>
    public bool HasFishPack()
    {
        return pFishPack != null;
    }

    /// <summary>
    /// 设置鱼群
    /// </summary>
    /// <param name="fish"></param>
    public void SetFishPack(CFishPack fish)
    {
        pFishPack = fish;
    }

    public void SetBoat(int tbid, CPlayerUnit unit, long guardian, bool setPos = false)
    {
        if (emType == EMType.Duel ||
            emType == EMType.Ouhuang ||
            emType == EMType.Boss) return;

        long nWinnerPoint = 0;
        if(unit !=null && unit.pInfo!=null)
        {
            nWinnerPoint = unit.pInfo.WinnerPoint;
        }

        if(tbid == 101)
        {
            SetBoatObj(CGameColorFishMgr.Ins.pMap.GetBoatCommon(nWinnerPoint), unit, setPos);

            ////普通船
            //if (emType == EMType.Normal)
            //{
            //    SetBoatObj(CGameColorFishMgr.Ins.pMap.GetBoatCommon(guardian), unit);
            //}
            //else if(emType == EMType.Ouhuang)
            //{
            //    //if (unit != null)
            //    //{
            //    //    SetBoatObj(CGameColorFishMgr.Ins.pMap.GetBoatCommon(guardian), unit);
            //    //}
            //    //else
            //    //{
            //    //    SetBoatObj(CGameColorFishMgr.Ins.pMap.objBoartOuhuangRoot, unit);
            //    //}

            //    SetBoatObj(CGameColorFishMgr.Ins.pMap.objBoartOuhuangRoot, unit);
            //}

            return;
        }
        else 
        {
            ST_UnitFishBoat pTBLBoatInfo = CTBLHandlerUnitFishBoat.Ins.GetInfo(tbid);
            if (pTBLBoatInfo == null)
            {
                SetBoatObj(CGameColorFishMgr.Ins.pMap.GetBoatCommon(nWinnerPoint), unit, setPos);
                return;
            }

            CResLoadMgr.Inst.SynLoad(pTBLBoatInfo.szPrefab, CResLoadMgr.EM_ResLoadType.CanbeUnloadAssetbundle,
                delegate (Object res, object data, bool bSuc)
                {
                    GameObject objRoot = res as GameObject;
                    SetBoatObj(objRoot, unit, setPos);
                });
        }
    }

    public void SetBoatObj(GameObject objRoot, CPlayerUnit unit, bool setPos = false)
    {
        CBoatAvatar pBoatRoot = objRoot.GetComponent<CBoatAvatar>();

        //判断是否相同的
        if(objBoatAvatar!=null)
        {
            CBoatAvatar pOldBoat = objBoatAvatar.GetComponent<CBoatAvatar>();
            if(pOldBoat!=null && 
               pOldBoat.tbid == pBoatRoot.tbid)
            {
                return;
            }

            //删掉旧的
            if (unit != null)
            {
                unit.tranSelf.SetParent(null);
                pUnitInBoat = null;
            }
            else
            {
                if(pUnitInBoat!=null)
                {
                    pUnitInBoat.tranSelf.SetParent(null);
                    pUnitInBoat = null;
                }
            }
            
            Destroy(objBoatAvatar);
            objBoatAvatar = null;
        }

        GameObject objNewBoat = GameObject.Instantiate(objRoot) as GameObject;
        CBoatAvatar pNewBoat = objNewBoat.GetComponent<CBoatAvatar>();
        if (pNewBoat == null)
        {
            Destroy(pNewBoat);
            return;
        }

        //重新绑定
        objBoatAvatar = objNewBoat;
        Transform tranBoatAvatar = objBoatAvatar.transform;
        tranBoatAvatar.SetParent(transform);
        tranBoatAvatar.localPosition = Vector3.zero;
        tranBoatAvatar.localRotation = Quaternion.identity;
        tranBoatAvatar.localScale = Vector3.one;

        tranSelf = pNewBoat.tranTargetPos;

        if(unit!=null)
        {
            pNewBoat.SetOwner(unit.uid);

            unit.tranSelf.SetParent(tranSelf);
            unit.tranSelf.localScale = Vector3.one;
            unit.tranSelf.localRotation = Quaternion.identity;
            if(setPos)
            {
                unit.tranSelf.localPosition = Vector3.zero;
            }

            pUnitInBoat = unit;
        }

        //创建换船特效
        CEffectMgr.Instance.CreateEffSync("Effect/smoke001", tranSelf, 0);
    }
}
