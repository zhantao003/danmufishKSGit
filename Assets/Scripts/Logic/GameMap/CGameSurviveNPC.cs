using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameSurviveNPC : MonoBehaviour
{
    public Transform tranSelf;
    public int nCurAreaIdx = -1;
    public Vector3 vStartPos;
    public Vector3 vEndPos;
    public Vector3[] arrAreaPos;
    public CPropertyTimer pTickerMove = new CPropertyTimer();
    public string szThrowGoldEffect;
    public bool bActive = false;

    //移动时间
    Vector3 vStartMovePos;
    Vector3 vEndMovePos;

    public void StartAction()
    {
        bActive = true;
        nCurAreaIdx = 0;
        tranSelf.position = vStartPos;

        vStartMovePos = tranSelf.position;
        vEndMovePos = arrAreaPos[nCurAreaIdx];

        pTickerMove.FillTime();
    }

    private void FixedUpdate()
    {
        if (!bActive) return;

        if(pTickerMove.Tick(CTimeMgr.FixedDeltaTime))
        {
            tranSelf.position = Vector3.Lerp(vStartMovePos, vEndMovePos, 1F);

            //给活着的人发奖励
            SendGift(nCurAreaIdx);

            pTickerMove.FillTime();
            nCurAreaIdx++;

            vStartMovePos = tranSelf.position;
            if (nCurAreaIdx < arrAreaPos.Length)
            {
                vEndMovePos = arrAreaPos[nCurAreaIdx];
            }
            else if(nCurAreaIdx == arrAreaPos.Length)
            {
                vEndMovePos = vEndPos;
            }
            else
            {
                bActive = false;
                nCurAreaIdx = -1;

                CGameSurviveMap.Ins.SetState(CGameSurviveMap.EMGameState.End);
            }
        }
        else
        {
            tranSelf.position = Vector3.Lerp(vStartMovePos, vEndMovePos, 1F - pTickerMove.GetTimeLerp());
        }
    }

    public void SendGift(int areaIdx)
    {
        if (areaIdx < 0 || areaIdx >= CGameSurviveMap.Ins.arrArea.Length) return;

        if (CHelpTools.IsStringEmptyOrNone(szThrowGoldEffect)) return;

        List<CGameSurvivePlayerInfo> listAllPlayer = CGameSurviveMap.Ins.GetAllPlayerInArea(areaIdx);
        for(int i=0; i<listAllPlayer.Count; i++)
        {
            CGameSurvivePlayerInfo pGameInfo = listAllPlayer[i];
            CPlayerUnit pTargetUnit = listAllPlayer[i].pPlayer;
            int nAddCoin = pGameInfo.nAddHP * 2 + 1000;

            if (pTargetUnit == null) continue;
            CEffectMgr.Instance.CreateEffSync("Effect/" + szThrowGoldEffect, tranSelf.position, Quaternion.identity, 0,
                delegate (GameObject value)
                {
                    CEffectBeizier pEffBeizier = value.GetComponent<CEffectBeizier>();
                    if (pEffBeizier == null) return;

                    pEffBeizier.SetTarget(tranSelf.position + Vector3.up * 1.5F,
                        pTargetUnit.tranSelf,
                        delegate() {
                            if (pTargetUnit == null) return;

                            pTargetUnit.AddCoinByHttp(nAddCoin, EMFishCoinAddFunc.Pay, false, false);
                            pTargetUnit.dlgOnlySetAddCoin?.Invoke(nAddCoin,0);
                            pTargetUnit.dlgShowAddCoin?.Invoke();
                        });
                });
        }
    }
}
