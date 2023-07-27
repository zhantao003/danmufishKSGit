using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFSMBoss204Dead : CFSMBossBase
{
    CBoss204 pBoss;
    CPropertyTimer pTimeTicker = new CPropertyTimer();

    public enum EMState
    {
        Dead,
        Down,
    }

    public EMState emCurState = EMState.Dead;

    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CBossBase.EMState.Dead;
        pBoss = pUnit as CBoss204;

        CGameBossMgr.Ins.emCurState = CGameBossMgr.EMState.End;

        if (pBoss == null) return;

        emCurState = EMState.Dead;
        pBoss.pAnimeCtrl.CrossFade(CBossAnimeConst.Anime_Dead, 0.1F);
        pBoss.pAnimeCtrl.speed = 1.25f;
        pTimeTicker.Value = 2F;
        pTimeTicker.FillTime();

        pBoss.pEffDead.Play();

        //pTimeTicker.Value = pBoss.fTimeSavePower;
        //pTimeTicker.FillTime();
    }

    public override void OnFixedUpdate(object obj, float delta)
    {
        if (emCurState == EMState.Dead)
        {
            if (pTimeTicker.Tick(delta))
            {
                pBoss.pAnimeCtrl.Play(CBossAnimeConst.Anime_Idle);
                pBoss.pAnimeCtrl.speed = 1f;
                pTimeTicker.Value = pBoss.fTimeSavePower;
                pTimeTicker.FillTime();

                emCurState = EMState.Down;
            }
        }
        else if (emCurState == EMState.Down)
        {
            if (!pTimeTicker.Tick(delta))
            {
                pBoss.transform.position = Vector3.Lerp(pBoss.vPosReady, pBoss.vGameStartPos, 1f - pTimeTicker.GetTimeLerp());
            }
            else
            {
                //统计房间的变异巨鲨Boss
                //房间记录
                if (CPlayerMgr.Ins.pOwner != null)
                {
                    CHttpParam pReqRoomParams = new CHttpParam(
                        new CHttpParamSlot("uid", CPlayerMgr.Ins.pOwner.uid.ToString()),
                        new CHttpParamSlot("roomId", CDanmuSDKCenter.Ins.szRoomId.ToString()),
                        new CHttpParamSlot("fishId", 2104.ToString()),
                        new CHttpParamSlot("count", 1.ToString())
                    );

                    CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddRoomRareFishRecord, pReqRoomParams, 0, true);
                }

                pUnit.transform.position = pBoss.vGameStartPos;

                UIManager.Instance.OpenUI(UIResType.BossDmgResult);
                UIBossDmgResult uiBossRes = UIManager.Instance.GetUI(UIResType.BossDmgResult) as UIBossDmgResult;
                if (uiBossRes != null)
                {
                    uiBossRes.SetInfo(true);
                }
                pBoss.SetState(CBossBase.EMState.Ready);
            }
        }
    }
}
