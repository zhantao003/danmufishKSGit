using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuCmdAttrite(CDanmuEventConst.BuyTreasureBoat)]
public class DCmdBuyTreasureBoat : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        string uid = dm.uid.ToString();
        string szContent = dm.content;
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null)
        {
            //先登录
            //CHttpParam pParamLogin = new CHttpParam
            //(
            //    new CHttpParamSlot("uid", dm.uid.ToString()),
            //    new CHttpParamSlot("nickName", System.Net.WebUtility.UrlEncode(dm.userName)),
            //    new CHttpParamSlot("headIcon", System.Net.WebUtility.UrlEncode(dm.userFace)),
            //    new CHttpParamSlot("userType", ((int)CPlayerBaseInfo.EMUserType.Guanzhong).ToString()),
            //    new CHttpParamSlot("fansMedalLevel", dm.fansMedalLevel.ToString()),
            //    new CHttpParamSlot("fansMedalName", dm.fansMedalName),
            //    new CHttpParamSlot("fansMedalWearingStatus", dm.fansMedalWearingStatus.ToString()),
            //    new CHttpParamSlot("guardLevel", dm.guardLevel.ToString())
            //);

            //CHttpMgr.Instance.SendHttpMsg(
            //   CHttpConst.LoginViewer,
            //   new HHandlerLoginViewer(dm.userName, dm.userFace, "", null, delegate (CPlayerBaseInfo info)
            //   {
            //       BuyTreasure(szContent, info);
            //   }),
            //   pParamLogin);

            CPlayerNetHelper.Login(uid, dm.nickName, dm.headIcon, CPlayerBaseInfo.EMUserType.Guanzhong,
                                   dm.fanLv, dm.fanName, dm.fanEquip, dm.vipLv,
                                   new HHandlerLoginViewer(dm.nickName, dm.headIcon, "", null, delegate (CPlayerBaseInfo info, CGameColorFishMgr.EMJoinType joinType)
                                   {
                                       BuyTreasure(szContent, info);
                                   }));
        }
        else
        {
            BuyTreasure(szContent, pPlayer);
        }
    }

    void BuyTreasure(string content, CPlayerBaseInfo player)
    {
        string szContent = content.Trim();
        try
        {
            int nLen = content.Length;
            int nCmdIdx = content.IndexOf(CDanmuEventConst.BuyTreasureBoat) + CDanmuEventConst.BuyTreasureBoat.Length;
            szContent = content.Substring(nCmdIdx, nLen - nCmdIdx).Trim();
            if (CHelpTools.IsStringEmptyOrNone(szContent))
            {
                Debug.Log("异常购买弹幕:" + content);
                return;
            }

            int nItemID = 0;
            if (!int.TryParse(szContent, out nItemID))
            {
                Debug.Log("异常的商品ID：" + szContent);
                return;
            }

            if (player.CheckIsGrayName())
            {
                return;
            }
            CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(player.uid);
            if (pUnit == null)
            {
                pUnit = CPlayerMgr.Ins.GetActiveUnit(player.uid);
            }
            if (pUnit != null &&
               pUnit.bCheckYZM)
            {
                return;
            }

            List<ST_ShopTreasure> listTBLInfos = CTBLHandlerTreasureShop.Ins.GetInfos();
            ST_ShopTreasure pTBLInfo = null;
            for(int i=0; i<listTBLInfos.Count; i++)
            {
                if(listTBLInfos[i].nContentID == nItemID &&
                   listTBLInfos[i].emType == ST_ShopTreasure.EMItemType.Boat)
                {
                    pTBLInfo = listTBLInfos[i];
                    break;
                }
            }

            if (pTBLInfo == null) return;

            //判断积分够不够
            if (player.nWinnerOuhuang < pTBLInfo.nPrice)
            {
                UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
                if (uiGameInfo != null && uiGameInfo.IsOpen())
                {
                    UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(player.uid);
                    if (uiUnitInfo != null)
                    {
                        uiUnitInfo.SetDmContent("皇冠数量不够");
                    }
                }

                return;
            }
            

            //if (player.TreasurePoint < pTBLInfo.nPrice)
            //{
            //    UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
            //    if (uiGameInfo != null && uiGameInfo.IsOpen())
            //    {
            //        UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(player.uid);
            //        if (uiUnitInfo != null)
            //        {
            //            uiUnitInfo.SetDmContent("海盗金币不够");
            //        }
            //    }

            //    return;
            //}

            //判断背包里有没有
            if (pTBLInfo.emType == ST_ShopTreasure.EMItemType.Role)
            {
                if(player.pAvatarPack.GetInfo(pTBLInfo.nContentID)!=null)
                {
                    UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
                    if (uiGameInfo != null && uiGameInfo.IsOpen())
                    {
                        UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(player.uid);
                        if (uiUnitInfo != null)
                        {
                            uiUnitInfo.SetDmContent("已经拥有该人");
                        }
                    }

                    return;
                }
            }
            else if(pTBLInfo.emType == ST_ShopTreasure.EMItemType.Boat)
            {
                if (player.pBoatPack.GetInfo(pTBLInfo.nContentID) != null)
                {
                    UIGameInfo uiGameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
                    if (uiGameInfo != null && uiGameInfo.IsOpen())
                    {
                        UIShowUnitDialog uiUnitInfo = uiGameInfo.GetShowDialogSlot(player.uid);
                        if (uiUnitInfo != null)
                        {
                            uiUnitInfo.SetDmContent("已经拥有这艘船");
                        }
                    }

                    return;
                }
            }
            else
            {
                return;
            }

            CHttpParam pReqParams = new CHttpParam(
               new CHttpParamSlot("uid", player.uid),
               new CHttpParamSlot("itemId", nItemID.ToString()),
               new CHttpParamSlot("itemType", ((int)ST_ShopTreasure.EMItemType.Boat).ToString())
            );
            CHttpMgr.Instance.SendHttpMsg(CHttpConst.BuyTreasureShopInfo, pReqParams);
        }
        catch(System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}
