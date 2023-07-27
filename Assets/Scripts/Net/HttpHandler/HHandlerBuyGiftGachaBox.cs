using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.BuyGiftGachaBox)]
public class HHandlerBuyGiftGachaBox : INetEventHandler
{
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

        Debug.Log("抽奖结果：\r\n" + arrGachaContent.GetData());

        pPlayer.nGachaGiftCount = gachaCount;
        pPlayer.GameCoins = fishCoin;
        pPlayer.nlBaitCount = fishBait;
        //pPlayer.nlRobCount = fishGan;
        //pPlayer.nlBuoyCount = fishPiao;
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
        for(int i=0; i<arrGachaContent.GetSize(); i++)
        {
            CLocalNetMsg msgGachaSlot = arrGachaContent.GetNetMsg(i);
            CGiftGachaBoxInfo pGachaSlotInfo = new CGiftGachaBoxInfo();
            pGachaSlotInfo.emType = (CGiftGachaBoxInfo.EMGiftType)(msgGachaSlot.GetInt("itemType"));
            pGachaSlotInfo.nItemID = msgGachaSlot.GetInt("itemId");
            listInfos.Add(pGachaSlotInfo);

            if(pGachaSlotInfo.emType == CGiftGachaBoxInfo.EMGiftType.Role)
            {
                CPlayerAvatarInfo pAvatarInfo = new CPlayerAvatarInfo();
                pAvatarInfo.nAvatarId = pGachaSlotInfo.nItemID;
                pAvatarInfo.nPart = 0;
                pPlayer.pAvatarPack.AddInfo(pAvatarInfo);
                pPlayer.pAvatarPack.SortAvatarPack();
            }
            else if(pGachaSlotInfo.emType == CGiftGachaBoxInfo.EMGiftType.Boat)
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

        if(pUnit != null &&
           CGameColorFishMgr.Ins.pMap != null)
        {

        }

        //if (pUnit != null &&
        //   CGameColorFishMgr.Ins.pMap != null)
        //{
        //    CResLoadMgr.Inst.SynLoad("Unit/BoxGiftGacha", CResLoadMgr.EM_ResLoadType.CanbeUnloadAssetbundle,
        //    delegate (Object res, object data, bool bSuc)
        //    {
        //        GameObject objBoxRoot = res as GameObject;
        //        if (objBoxRoot == null) return;
        //        GameObject objNewBox = GameObject.Instantiate(objBoxRoot) as GameObject;
        //        Transform tranNewBox = objNewBox.GetComponent<Transform>();
        //        Transform tranTargetSlot = null;
        //        if (pUnit == null)
        //        {
        //            tranTargetSlot = CGameColorFishMgr.Ins.pMap.GetRandomGachaPos();
        //        }
        //        else
        //        {
        //            tranTargetSlot = pUnit.tranGachePos;
        //        }

        //        if (tranTargetSlot == null)
        //        {
        //            Debug.LogError("异常的宝箱点");
        //            return;
        //        }

        //        tranNewBox.SetParent(null);
        //        tranNewBox.position = tranTargetSlot.position;
        //        tranNewBox.localScale = Vector3.one;
        //        tranNewBox.rotation = Quaternion.Euler(0F, -100F, 0F);

        //        CGiftGachaBox pBox = objNewBox.GetComponent<CGiftGachaBox>();
        //        if (pBox == null)
        //        {
        //            GameObject.Destroy(objNewBox);
        //            return;
        //        }

        //        pBox.tranRoot = tranTargetSlot;
        //        pBox.Init(uid, count, listInfos);
        //    });
        //}
        //else
        //{
        //    UIUserGetAvatar uiGetAvatar = UIManager.Instance.GetUI(UIResType.UserGetAvatarTip) as UIUserGetAvatar;
        //    if (uiGetAvatar != null)
        //    {
        //        for (int i = 0; i < listInfos.Count; i++)
        //        {
        //            uiGetAvatar.AddGiftGachaBoxSlot(pPlayer, listInfos[i]);
        //        }
        //    }
        //}
    }
}
