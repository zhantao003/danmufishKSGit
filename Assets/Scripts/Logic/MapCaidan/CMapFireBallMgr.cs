using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMapFireBallMgr : MonoBehaviour
{
    public string szLockEffect;
    public GameObject objFireBallRoot;
    public Transform[] arrFirePos;

    public CPropertyTimer pTickerAtkCD;
    public List<CMapSlot> listTargets = new List<CMapSlot>();    //ѡ�е�Ŀ��

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
        //��ѡ��һ��Ŀ��
        int nRandomIdx = 0;
        int nHitCount = Random.Range(CGameColorFishMgr.Ins.pStaticConfig.GetInt("����������Сֵ"), CGameColorFishMgr.Ins.pStaticConfig.GetInt("�����������ֵ") + 1);

        listTargets.Clear();
        List<CPlayerUnit> listPlayerUnits = new List<CPlayerUnit>();
        foreach (CPlayerUnit playerUnit in CPlayerMgr.Ins.dicIdleUnits.Values)
        {
            if (playerUnit == null)
                continue;

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

            //������Ч
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
