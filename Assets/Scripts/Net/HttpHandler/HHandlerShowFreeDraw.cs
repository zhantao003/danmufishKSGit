using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMFreeDrawType
{
    OuHuang,
    Profit,
    Boss,
}

public class HHandlerShowFreeDraw : INetEventHandler
{
    EMFreeDrawType emFreeDrawType;
    public HHandlerShowFreeDraw(EMFreeDrawType freeDrawType)
    {
        emFreeDrawType = freeDrawType;
    }

    public void OnErrorCode(CLocalNetMsg pMsg)
    {

    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string szStatus = pMsg.GetString("status");
        if (!szStatus.Equals("ok")) return;

        string uid = pMsg.GetString("uid");
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null) return;

        long count = pMsg.GetLong("count");
        long gachaCount = pMsg.GetLong("gachaCount");
        long fishCoin = pMsg.GetLong("fishCoin");
        long fishGan = pMsg.GetLong("fishGan");
        long fishPiao = pMsg.GetLong("fishPiao");
        long fishLun = pMsg.GetLong("fishLun");
        long fishBait = pMsg.GetLong("fishBait");
        string szContent = pMsg.GetString("gachaResult").Replace("\\", "");
        CLocalNetArrayMsg arrGachaContent = new CLocalNetArrayMsg(szContent);

        Debug.Log("³é½±½á¹û£º\r\n" + arrGachaContent.GetData());

        pPlayer.nGachaGiftCount = gachaCount;
        pPlayer.GameCoins = fishCoin;
        pPlayer.nlBaitCount = fishBait;
        pPlayer.nlFeiLunCount = fishLun;

        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(uid);
        if (pUnit != null)
        {
            pUnit.dlgChgGift?.Invoke();
            if (pPlayer.CheckHaveGift())
            {
                pUnit.ClearExitTick();
            }
            else
            {
                pUnit.ResetExitTick();
            }
        }

        List<CGiftGachaBoxInfo> listInfos = new List<CGiftGachaBoxInfo>();
        for (int i = 0; i < arrGachaContent.GetSize(); i++)
        {
            CLocalNetMsg msgGachaSlot = arrGachaContent.GetNetMsg(i);
            CGiftGachaBoxInfo pGachaSlotInfo = new CGiftGachaBoxInfo();
            pGachaSlotInfo.emType = (CGiftGachaBoxInfo.EMGiftType)(msgGachaSlot.GetInt("itemType"));
            pGachaSlotInfo.nItemID = msgGachaSlot.GetInt("itemId");
            listInfos.Add(pGachaSlotInfo);

            if (pGachaSlotInfo.emType == CGiftGachaBoxInfo.EMGiftType.Role)
            {
                CPlayerAvatarInfo pAvatarInfo = new CPlayerAvatarInfo();
                pAvatarInfo.nAvatarId = pGachaSlotInfo.nItemID;
                pAvatarInfo.nPart = 0;
                pPlayer.pAvatarPack.AddInfo(pAvatarInfo);
                pPlayer.pAvatarPack.SortAvatarPack();
            }
            else if (pGachaSlotInfo.emType == CGiftGachaBoxInfo.EMGiftType.Boat)
            {
                CPlayerBoatInfo pBoatInfo = new CPlayerBoatInfo();
                pBoatInfo.nBoatId = pGachaSlotInfo.nItemID;
                pPlayer.pBoatPack.AddInfo(pBoatInfo);
            }
            else if (pGachaSlotInfo.emType == CGiftGachaBoxInfo.EMGiftType.FishGan)
            {
                int ganLv = msgGachaSlot.GetInt("lv");
                int exp = msgGachaSlot.GetInt("exp");
                EMAddUnitProType proType = (EMAddUnitProType)msgGachaSlot.GetInt("proType");
                int proAdd = msgGachaSlot.GetInt("proAdd");

                CPlayerFishGanInfo pGanInfo = new CPlayerFishGanInfo();
                pGanInfo.nGanId = pGachaSlotInfo.nItemID;
                pGanInfo.nLv = ganLv;
                pGanInfo.nExp = exp;
                pGanInfo.emPro = proType;
                pGanInfo.nProAdd = proAdd;
                pPlayer.pFishGanPack.AddInfo(pGanInfo);
            }
        }

        if (pUnit != null &&
           CGameColorFishMgr.Ins.pMap != null)
        {
            if (listInfos.Count <= 0)
                return;

            CDrawRewardInfo rewardInfo = new CDrawRewardInfo();
            rewardInfo.emDrawRewardType = listInfos[0].emType;
            rewardInfo.nRewardID = listInfos[0].nItemID;
            rewardInfo.nlRewardCount = listInfos[0].nItemID;
            if (emFreeDrawType == EMFreeDrawType.OuHuang)
            {
                UIBattleModeInfo battleModeInfo = UIManager.Instance.GetUI(UIResType.BattleModeInfo) as UIBattleModeInfo;
                if(battleModeInfo != null)
                {
                    battleModeInfo.battleRankRoot.pOuHuangSlot.freeDrawRoot.Show(rewardInfo);
                }
            }
            else if (emFreeDrawType == EMFreeDrawType.Profit)
            {
                UIBattleModeInfo battleModeInfo = UIManager.Instance.GetUI(UIResType.BattleModeInfo) as UIBattleModeInfo;
                if (battleModeInfo != null)
                {
                    battleModeInfo.battleRankRoot.pProfitSlot.freeDrawRoot.Show(rewardInfo);
                }
            }
            else if (emFreeDrawType == EMFreeDrawType.Boss)
            {
                UIBossDmgResult bossDmgResult = UIManager.Instance.GetUI(UIResType.BossDmgResult) as UIBossDmgResult;
                if(bossDmgResult != null)
                {
                    bossDmgResult.freeDrawRoot.Show(rewardInfo);
                }
            }
        }
    }
}


