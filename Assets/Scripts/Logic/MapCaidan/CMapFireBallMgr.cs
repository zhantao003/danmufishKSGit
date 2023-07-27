using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMapFireBallMgr : MonoBehaviour
{
    public string szLockEffect;
    public GameObject objFireBallRoot;
    public Transform[] arrFirePos;

    public CPropertyTimer pTickerAtkCD;
    public List<CMapSlot> listTargets = new List<CMapSlot>();    //选中的目标

    CPropertyTimer pTickerAtk;

    private void Start()
    {
        pTickerAtk = null;
        pTickerAtkCD.FillTime();

        objFireBallRoot.SetActive(false);
    }
    private void Update()
    {
        if(pTickerAtkCD.Tick(CTimeMgr.DeltaTime))
        {
            StartAtk();
            pTickerAtkCD.FillTime();
        }

        if(pTickerAtk!=null && pTickerAtk.Tick(CTimeMgr.DeltaTime))
        {
            DoAtk();
            pTickerAtk = null;
        }
    }

    void StartAtk()
    {
        //先选择一个目标
        int nRandomIdx = 0;
        int nHitCount = Random.Range(CGameColorFishMgr.Ins.pStaticConfig.GetInt("触手拍人最小值"), CGameColorFishMgr.Ins.pStaticConfig.GetInt("触手拍人最大值") + 1);

        listTargets.Clear();
        List<CPlayerUnit> listPlayerUnits = new List<CPlayerUnit>();
        foreach (CPlayerUnit playerUnit in CPlayerMgr.Ins.dicIdleUnits.Values)
        {
            if (playerUnit == null)
                continue;

            ///决斗和丢炸弹的无法上岸
            if (playerUnit != null &&
               (playerUnit.emCurState == CPlayerUnit.EMState.Battle ||
                playerUnit.listNormalBooms.Count > 0 ||
                playerUnit.listTreasureBooms.Count > 0))
                continue;

            ///有渔具的人无法上岸
            if (playerUnit.pInfo != null &&
               (playerUnit.pInfo.nlBaitCount > 0 ||
                playerUnit.pInfo.nlFeiLunCount > 0))
                continue;

            ///判断是否是主播
            if (CPlayerMgr.Ins.pOwner != null &&
                CPlayerMgr.Ins.pOwner.uid == playerUnit.uid)
                continue;

            ///正在拍卖的人无法上岸
            if (CHelpTools.CheckAuctionByUID(playerUnit.uid))
                continue;

            if (CGameColorFishMgr.Ins.nlCurOuHuangUID == playerUnit.uid)
                continue;

            listPlayerUnits.Add(playerUnit);
        }

        if (listPlayerUnits.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < nHitCount; i++)
        {
            if (listPlayerUnits.Count <= 0) break;

            nRandomIdx = Random.Range(0, listPlayerUnits.Count);
            if (listPlayerUnits[nRandomIdx].pMapSlot == null) continue;

            listTargets.Add(listPlayerUnits[nRandomIdx].pMapSlot);

            //创建特效
            CEffectMgr.Instance.CreateEffSync(szLockEffect, listPlayerUnits[nRandomIdx].pMapSlot.transform, 0);

            listPlayerUnits.RemoveAt(nRandomIdx);
        }

        pTickerAtk = new CPropertyTimer();
        pTickerAtk.Value = 15F;
        pTickerAtk.FillTime();
    }

    void DoAtk()
    {
        for(int i=0; i<listTargets.Count; i++)
        {
            Transform tranFirePos = arrFirePos[Random.Range(0, arrFirePos.Length)];
            GameObject objNewFireBall = GameObject.Instantiate(objFireBallRoot) as GameObject;
            objNewFireBall.SetActive(true);
            Transform tranNewFireBall = objNewFireBall.GetComponent<Transform>();
            tranNewFireBall.SetParent(null);
            tranNewFireBall.position = tranFirePos.position;
            tranNewFireBall.localScale = Vector3.one;

            CMapFireBall pNewFireBall = objNewFireBall.GetComponent<CMapFireBall>();
            pNewFireBall.SetTarget(listTargets[i]);
        }
    }
}
