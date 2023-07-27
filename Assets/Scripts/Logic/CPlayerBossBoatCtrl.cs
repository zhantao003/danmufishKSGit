using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerBossBoatCtrl : MonoBehaviour
{
    public Transform tranSelf;
    public Vector2 vRangeMove;
    public float fMoveSpd = 1F;
    public Animator pAnimeCtrl;
    public CEffectOnlyCtrl pEffBehit;
    public Vector2 vHitPlayerRange;
    Vector3 vCurPos;

    public CAudioMgr.CAudioSlottInfo pAudioPlay;
    public CAudioMgr.CAudioSlottInfo pAudioHit;

    private void Start()
    {
        pEffBehit.Init();
        pEffBehit.StopEffect();

        CAudioMgr.Ins.PlaySoundBySlot(pAudioPlay, tranSelf.position);
    }

    private void Update()
    {
        OnMoveInput();
    }

    public void BeHit()
    {
        CAudioMgr.Ins.PlaySoundBySlot(pAudioHit, tranSelf.position);

        pAnimeCtrl.Play("Hit");
        pEffBehit.Play();

        //掉人
        int nTotalNum = CGameBossMgr.Ins.listActivePlayers.Count;
        if(nTotalNum <= 0)
        {
            Debug.Log("没有玩家可以撞");
            return;
        }

        int nHit = Random.Range((int)(nTotalNum * vHitPlayerRange.x), (int)(nTotalNum * vHitPlayerRange.y));
        List<CPlayerBaseInfo> listBeHit = CListCollectTools.GetRandomChilds(CGameBossMgr.Ins.listActivePlayers, nHit);
        for(int i=0; i<listBeHit.Count; i++)
        {
            CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(listBeHit[i].uid);
            if (pUnit == null) continue;
            if (pUnit.emCurState == CPlayerUnit.EMState.BossWait ||
                pUnit.emCurState == CPlayerUnit.EMState.BossEat ||
                pUnit.emCurState == CPlayerUnit.EMState.BossReturn ||
                pUnit.emCurState == CPlayerUnit.EMState.GunShootBoss ||
                pUnit.emCurState == CPlayerUnit.EMState.RPGShootBoss) continue;

            pUnit.SetState(CPlayerUnit.EMState.BossEat);
        }
    }

    public void BeHitShow()
    {
        CAudioMgr.Ins.PlaySoundBySlot(pAudioHit, tranSelf.position);

        pAnimeCtrl.Play("Hit");
        pEffBehit.Play();

        //掉人
        int nTotalNum = CGameBossMgr.Ins.listActivePlayers.Count;
        Debug.Log("玩家数：" + nTotalNum);
        if (nTotalNum <= 0)
        {
            return;
        }

        List<CPlayerBaseInfo> listBeHit = CGameBossMgr.Ins.listActivePlayers;
        for (int i = 0; i < listBeHit.Count; i++)
        {
            CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(listBeHit[i].uid);
            if (pUnit == null) continue;
            if (pUnit.emCurState == CPlayerUnit.EMState.BossWait ||
                pUnit.emCurState == CPlayerUnit.EMState.BossEat ||
                pUnit.emCurState == CPlayerUnit.EMState.BossReturn ||
                pUnit.emCurState == CPlayerUnit.EMState.GunShootBoss ||
                pUnit.emCurState == CPlayerUnit.EMState.RPGShootBoss) continue;

            pUnit.SetState(CPlayerUnit.EMState.BossEatShow);
        }
    }

    void OnMoveInput()
    {
        if (CGameBossMgr.Ins.emCurState != CGameBossMgr.EMState.Gaming) return;

        vCurPos = tranSelf.position;

        if (Input.GetKey(KeyCode.D) ||
           Input.GetKey(KeyCode.RightArrow))
        {
            vCurPos += Vector3.left * CTimeMgr.DeltaTime * fMoveSpd;
        }

        if (Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.LeftArrow))
        {
            vCurPos += Vector3.right * CTimeMgr.DeltaTime * fMoveSpd;
        }

        vCurPos.x = Mathf.Clamp(vCurPos.x, vRangeMove.x, vRangeMove.y);
        tranSelf.position = vCurPos;
    }
}
