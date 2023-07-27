using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRoomTipInfo : MonoBehaviour
{
    public GameObject[] objDuelTip;

    public void Init()
    {
        for (int i = 0; i < objDuelTip.Length; i++)
        {
            if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal)
            {
                objDuelTip[i].SetActive(!CTBLHandlerFucAble.Ins.GetInfo((int)EMFuncAbleType.Duel).CheckSceneLv());
            }
            else if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
            {
                objDuelTip[i].SetActive(!CTBLHandlerFucAble.Ins.GetInfo((int)EMFuncAbleType.Duel).CheckSceneLv());
            }
            else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema)
            {
                objDuelTip[i].SetActive(!CTBLHandlerFucAble.Ins.GetInfo((int)EMFuncAbleType.Duel).CheckSceneLv());
            }
            else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Battle)
            {
                objDuelTip[i].SetActive(false);
            }
            else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
            {
                objDuelTip[i].SetActive(false);
            }

        }
    }
}
