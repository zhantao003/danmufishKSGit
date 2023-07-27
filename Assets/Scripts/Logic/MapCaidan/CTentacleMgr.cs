using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTentacleMgr : MonoBehaviour
{
    static CTentacleMgr ins = null;

    public static CTentacleMgr Ins
    {
        get
        {
            return ins;
        }
    }

    public Vector2 vCheckTime = new Vector2(900f, 1800f);
    CPropertyTimer pCheckTick;

    public List<CTentacleUnit> listIdleTentacleUnits = new List<CTentacleUnit>();   //�����

    public int nCheckCount;

    private void Start()
    {
        ins = this;
        ResetTick();
    }

    public void ShowTentacle(CPlayerUnit pTarget)
    {
        if (pTarget == null) return;
        CTentacleUnit pNewSlot = null;
        if (listIdleTentacleUnits.Count > 0)
        {
            pNewSlot = listIdleTentacleUnits[0];
            listIdleTentacleUnits.RemoveAt(0);
            pNewSlot.Show(pTarget);
        }
        else
        {
            CResLoadMgr.Inst.SynLoad("Unit/TentacleUnit", CResLoadMgr.EM_ResLoadType.Role,
            delegate (Object res, object data, bool bSuc)
            {
                GameObject objRoleRoot = res as GameObject;
                if (objRoleRoot == null) return;
                GameObject objNewRole = GameObject.Instantiate(objRoleRoot) as GameObject;
                Transform tranNewRole = objNewRole.GetComponent<Transform>();
                tranNewRole.SetParent(null);
                tranNewRole.localScale = Vector3.one;

                CTentacleUnit pNewUnit = objNewRole.GetComponent<CTentacleUnit>();
                pNewUnit.Show(pTarget);
            });
        }
    }
    

    public void RecycleTentacle(CTentacleUnit unit)
    {
        listIdleTentacleUnits.Add(unit);
        unit.transform.localPosition = new Vector3(10000F, 0F, 0F);

        ResetTick();
    }

    public void Update()
    {
        if(pCheckTick != null &&
           pCheckTick.Tick(CTimeMgr.DeltaTime))
        {
            pCheckTick = null;
            RandomHitPlayer();
        }
    }

    /// <summary>
    /// ����������
    /// </summary>
    public void RandomHitPlayer()
    {
        int nPlayerCount = 0;
        int nRandomIdx = 0;
        List<CPlayerUnit> listPlayerUnits = new List<CPlayerUnit>();
        int nHitCount = Random.Range(CGameColorFishMgr.Ins.pStaticConfig.GetInt("����������Сֵ"), CGameColorFishMgr.Ins.pStaticConfig.GetInt("�����������ֵ") + 1);
        foreach(CPlayerBaseInfo baseInfo in CPlayerMgr.Ins.dicAllPlayers.Values)
        {
            CPlayerUnit playerUnit = CPlayerMgr.Ins.GetIdleUnit(baseInfo.uid);
            if (playerUnit == null)
                continue;
            nPlayerCount++;
            ///�����Ͷ�ը�����޷��ϰ�
            if (playerUnit != null &&
               (playerUnit.emCurState == CPlayerUnit.EMState.Battle ||
                playerUnit.listNormalBooms.Count > 0 ||
                playerUnit.listTreasureBooms.Count > 0))
                continue;
            ///����ߵ����޷��ϰ�
            if (playerUnit.pInfo != null &&
               (playerUnit.pInfo.nlBaitCount > 0 ||
                playerUnit.pInfo.nlFeiLunCount > 0))
                continue;
            ///�ж��Ƿ�������
            if (CPlayerMgr.Ins.pOwner != null &&
                CPlayerMgr.Ins.pOwner.uid == playerUnit.uid)
                continue;
            ///�������������޷��ϰ�
            if (CHelpTools.CheckAuctionByUID(baseInfo.uid))
                continue;
            
            if (CGameColorFishMgr.Ins.nlCurOuHuangUID == baseInfo.uid)
                continue;
            
            listPlayerUnits.Add(playerUnit);
        }
        if (nPlayerCount < nCheckCount)
        {
            ResetTick();
            return;
        }
        for (int i = 0;i < nHitCount;i++)
        {
            if (listPlayerUnits.Count <= 0) break;
            nRandomIdx = Random.Range(0, listPlayerUnits.Count);
            ShowTentacle(listPlayerUnits[nRandomIdx]);
            listPlayerUnits.RemoveAt(nRandomIdx);
        }
        
    }

    public void ResetTick()
    {
        if (pCheckTick != null) return;
        pCheckTick = new CPropertyTimer();
        pCheckTick.Value = Random.Range(vCheckTime.x, vCheckTime.y);
        pCheckTick.FillTime();
    }

}
