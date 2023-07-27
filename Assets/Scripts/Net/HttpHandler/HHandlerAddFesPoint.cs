using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CHttpEvent(CHttpConst.AddFesPoint)]
public class HHandlerAddFesPoint : INetEventHandler
{
    public void OnErrorCode(CLocalNetMsg pMsg)
    {
        
    }

    public void OnMsgHandler(CLocalNetMsg pMsg)
    {
        string szStatus = pMsg.GetString("status");
        if (!szStatus.Equals("ok")) return;

        string uid = pMsg.GetString("uid");
        long packId = pMsg.GetLong("packId");
        int curIdx = pMsg.GetInt("curIdx");
        long viewerPoint = pMsg.GetLong("viewerPoint");
        long vtbPoint = pMsg.GetLong("vtbPoint");

        CFishFesPlayerInfo pPlayerFesInfo = CFishFesInfoMgr.Ins.GetPlayerInfo(packId, uid);
        if(pPlayerFesInfo == null)
        {
            pPlayerFesInfo = new CFishFesPlayerInfo();
            pPlayerFesInfo.nUid = uid;
            pPlayerFesInfo.nCurIdx = curIdx;
            pPlayerFesInfo.nPlayerPoint = viewerPoint;
            pPlayerFesInfo.nVtbPoint = vtbPoint;
            CFishFesInfoMgr.Ins.AddFesPlayerInfo(packId, pPlayerFesInfo);
        }
        else
        {
            pPlayerFesInfo.nCurIdx = curIdx;
            pPlayerFesInfo.nPlayerPoint = viewerPoint;
            pPlayerFesInfo.nVtbPoint = vtbPoint;
        }

        CLocalNetArrayMsg arrGiftList = pMsg.GetNetMsgArr("gift");
        if(arrGiftList == null ||
           arrGiftList.GetSize() <= 0)
        {
            return;
        }

        //结算礼物列表
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null) return;

        for (int i=0; i<arrGiftList.GetSize(); i++)
        {
            CLocalNetMsg msgGift = arrGiftList.GetNetMsg(i);
            int nGiftType = msgGift.GetInt("type");
            if(nGiftType == 0)
            {
                int nBoatId = msgGift.GetInt("boatId");

                CPlayerBoatInfo pBoatInfo = new CPlayerBoatInfo();
                pBoatInfo.nBoatId = nBoatId;
                pPlayer.pBoatPack.AddInfo(pBoatInfo);
                pPlayer.pBoatPack.SortBoatPack();

                UIUserGetAvatar uiGetAvatar = UIManager.Instance.GetUI(UIResType.UserGetAvatarTip) as UIUserGetAvatar;
                if (uiGetAvatar != null)
                {
                    uiGetAvatar.AddFesGift(pPlayer, nGiftType, nBoatId);
                }
            }
            else if(nGiftType == 1)
            {
                int nAvatarId = msgGift.GetInt("avatarId");
                int nPartId = msgGift.GetInt("partId");

                CPlayerAvatarInfo pAvatarInfo = new CPlayerAvatarInfo();
                pAvatarInfo.nAvatarId = nAvatarId;
                pAvatarInfo.nPart = nPartId;
                pPlayer.pAvatarPack.AddInfo(pAvatarInfo);
                pPlayer.pAvatarPack.SortAvatarPack();

                UIUserGetAvatar uiGetAvatar = UIManager.Instance.GetUI(UIResType.UserGetAvatarTip) as UIUserGetAvatar;
                if (uiGetAvatar != null)
                {
                    uiGetAvatar.AddFesGift(pPlayer, nGiftType, nAvatarId);
                }
            }
            else if(nGiftType == 2)
            {
                long nFishCoint = msgGift.GetLong("fishCoin");

                pPlayer.GameCoins += nFishCoint;

                UIUserGetAvatar uiGetAvatar = UIManager.Instance.GetUI(UIResType.UserGetAvatarTip) as UIUserGetAvatar;
                if (uiGetAvatar != null)
                {
                    uiGetAvatar.AddFesGift(pPlayer, nGiftType, (int)nFishCoint);
                }
            }
            else if(nGiftType == 3 ||
                    nGiftType == 4)
            {
                long addCount = msgGift.GetLong("addCount");
                long fisnBait = msgGift.GetLong("fisnBait");
                long fishGan = msgGift.GetLong("fishGan");
                long fishPiao = msgGift.GetLong("fishPiao");
                long fishLun = msgGift.GetLong("fishLun");

                pPlayer.nlBaitCount = fisnBait;
                //pPlayer.nlBaitCount = fisnBait;
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

                UIUserGetAvatar uiGetAvatar = UIManager.Instance.GetUI(UIResType.UserGetAvatarTip) as UIUserGetAvatar;
                if (uiGetAvatar != null)
                {
                    uiGetAvatar.AddFesGift(pPlayer, nGiftType, (int)addCount);
                }
            }
            else if(nGiftType == 5)
            {
                int ganId = msgGift.GetInt("ganId");
                int lv = msgGift.GetInt("lv");
                int exp = msgGift.GetInt("exp");
                EMAddUnitProType proType = (EMAddUnitProType)msgGift.GetInt("proType");
                int proAdd = msgGift.GetInt("proAdd");

                CPlayerFishGanInfo pGanInfo = new CPlayerFishGanInfo();
                pGanInfo.nGanId = ganId;
                pGanInfo.nLv = lv;
                pGanInfo.nExp = exp;
                pGanInfo.emPro = proType;
                pGanInfo.nProAdd = proAdd;
                pPlayer.pFishGanPack.AddInfo(pGanInfo);

                UIUserGetAvatar uiGetAvatar = UIManager.Instance.GetUI(UIResType.UserGetAvatarTip) as UIUserGetAvatar;
                if (uiGetAvatar != null)
                {
                    uiGetAvatar.AddFesGift(pPlayer, nGiftType, ganId);
                }
            }
        }
    }
}
