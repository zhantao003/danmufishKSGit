using OpenBLive.Runtime.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuCmdAttrite(CDanmuEventConst.ExchangeBoat)]
public class DCmdExchangeBoat : CDanmuCmdAction
{
    public override void DoAction(CDanmuChat dm)
    {
        string uid = dm.uid.ToString();
        string szContent = dm.content;
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null)
        {
            //CHttpParam pParamLogin = new CHttpParam
            //(
            //   new CHttpParamSlot("uid", dm.uid.ToString()),
            //   new CHttpParamSlot("nickName", System.Net.WebUtility.UrlEncode(dm.userName)),
            //   new CHttpParamSlot("headIcon", System.Net.WebUtility.UrlEncode(dm.userFace)),
            //   new CHttpParamSlot("userType", ((int)CPlayerBaseInfo.EMUserType.Guanzhong).ToString()),
            //   new CHttpParamSlot("fansMedalLevel", dm.fansMedalLevel.ToString()),
            //   new CHttpParamSlot("fansMedalName", dm.fansMedalName),
            //   new CHttpParamSlot("fansMedalWearingStatus", dm.fansMedalWearingStatus.ToString()),
            //   new CHttpParamSlot("guardLevel", dm.guardLevel.ToString())
            //);

            //CHttpMgr.Instance.SendHttpMsg(
            //  CHttpConst.LoginViewer,
            //  new HHandlerLoginViewer(dm.userName, dm.userFace, "", null, delegate (CPlayerBaseInfo info)
            //  {
            //      ExchangeBoat(szContent, info);
            //  }),
            //  pParamLogin);

            CPlayerNetHelper.Login(uid, dm.nickName, dm.headIcon, CPlayerBaseInfo.EMUserType.Guanzhong,
                                   dm.fanLv, dm.fanName, dm.fanEquip, dm.vipLv,
                                  new HHandlerLoginViewer(dm.nickName, dm.headIcon, "", null, delegate (CPlayerBaseInfo info, CGameColorFishMgr.EMJoinType joinType)
                                  {
                                      ExchangeBoat(szContent, info);
                                  }));
        }
        else
        {
            ExchangeBoat(szContent, pPlayer);
        }
    }

    void ExchangeBoat(string content, CPlayerBaseInfo player)
    {
        string szContent = content.Trim();
        try
        {
            int nLen = content.Length;
            int nCmdIdx = content.IndexOf(CDanmuEventConst.ExchangeBoat) + CDanmuEventConst.ExchangeBoat.Length;
            szContent = content.Substring(nCmdIdx, nLen - nCmdIdx).Trim();
            if (CHelpTools.IsStringEmptyOrNone(szContent))
            {
                Debug.Log("异常兑换弹幕:" + content);
                return;
            }

            int nTargetAvatarId = 0;
            if (!int.TryParse(szContent, out nTargetAvatarId))
            {
                Debug.Log("异常的兑换ID：" + szContent);
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

            //先判断背包里有没有
            if (player.pBoatPack.GetInfo(nTargetAvatarId) != null)
            {
                Debug.Log("角色背包里已经有这个船了");
                return;
            }

            CHttpParam pReqParams = new CHttpParam(
               new CHttpParamSlot("uid", player.uid.ToString()),
               new CHttpParamSlot("boatId", nTargetAvatarId.ToString())
            );
            CHttpMgr.Instance.SendHttpMsg(CHttpConst.ExchangeBoat, new HHandlerUserExchangeBoat(), pReqParams);
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}
