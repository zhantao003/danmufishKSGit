using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFSMBoss104Dead : CFSMBossBase
{
    CBoss104 pBoss;
    CPropertyTimer pTimeTicker = new CPropertyTimer();

    public enum EMState
    {
        Jump,
        Move,
    }

    public EMState emCurState = EMState.Jump;

    public override void OnBegin(object obj)
    {
        pUnit.emCurState = CBossBase.EMState.Dead;
        pBoss = pUnit as CBoss104;

        CGameBossMgr.Ins.emCurState = CGameBossMgr.EMState.End;
        
        if (pBoss == null) return;

        emCurState = EMState.Jump;
        pBoss.pAnimeCtrl.Play(CBossAnimeConst.Anime_Dead);
        pTimeTicker.Value = 0.9F;
        pTimeTicker.FillTime();

        pBoss.pEffDead.Play();

        //pTimeTicker.Value = pBoss.fTimeSavePower;
        //pTimeTicker.FillTime();
    }

    public override void OnFixedUpdate(object obj, float delta)
    {
        if(emCurState == EMState.Jump)
        {
            if(pTimeTicker.Tick(delta))
            {
                pBoss.pAnimeCtrl.Play(CBossAnimeConst.Anime_Idle);
                pTimeTicker.Value = pBoss.fTimeSavePower;
                pTimeTicker.FillTime();

                emCurState = EMState.Move;
            }
        }
        else if(emCurState == EMState.Move)
        {
            if (!pTimeTicker.Tick(delta))
            {
                pBoss.transform.position = Vector3.Lerp(pBoss.vPosIdle, pBoss.vPosReady, 1f - pTimeTicker.GetTimeLerp());
            }
            else
            {
                //ͳ�Ʒ���ı������Boss
                //�����¼
                if (CPlayerMgr.Ins.pOwner != null)
                {
                    CHttpParam pReqRoomParams = new CHttpParam(
                        new CHttpParamSlot("uid", CPlayerMgr.Ins.pOwner.uid.ToString()),
                        new CHttpParamSlot("roomId", CDanmuSDKCenter.Ins.szRoomId.ToString()),
                        new CHttpParamSlot("fishId", 1104.ToString()),
                        new CHttpParamSlot("count", 1.ToString())
                    );

                    CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddRoomRareFishRecord, pReqRoomParams, 0, true);
                }

                pUnit.transform.position = pBoss.vPosReady;

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
