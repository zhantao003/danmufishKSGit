using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuCmdAttrite(CDanmuEventConst.ExchangeFishGan)]
public class DCmdExchangeFishGan : CDanmuCmdAction
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
            // CHttpConst.LoginViewer,
            // new HHandlerLoginViewer(dm.userName, dm.userFace, "", null, delegate (CPlayerBaseInfo info)
            // {
            //     ExchangeFishGan(szContent, info);
            // }),
            // pParamLogin);

            CPlayerNetHelper.Login(uid, dm.nickName, dm.headIcon, CPlayerBaseInfo.EMUserType.Guanzhong,
                                   dm.fanLv, dm.fanName, dm.fanEquip, dm.vipLv,
                                  new HHandlerLoginViewer(dm.nickName, dm.headIcon, "", null, delegate (CPlayerBaseInfo info, CGameColorFishMgr.EMJoinType joinType)
                                  {
                                      ExchangeFishGan(szContent, info);
                                  }));
        }
        else
        {
            ExchangeFishGan(szContent, pPlayer);
        }
    }

    void ExchangeFishGan(string content, CPlayerBaseInfo player)
    {
        string szContent = content.Trim();
        try
        {
            int nLen = content.Length;
            int nCmdIdx = content.IndexOf(CDanmuEventConst.ExchangeFishGan) + CDanmuEventConst.ExchangeFishGan.Length;
            szContent = content.Substring(nCmdIdx, nLen - nCmdIdx).Trim();
            if (CHelpTools.IsStringEmptyOrNone(szContent))
            {
                Debug.Log("�쳣�һ���Ļ:" + content);
                return;
            }

            int nTargetAvatarId = 0;
            if (!int.TryParse(szContent, out nTargetAvatarId))
            {
                Debug.Log("�쳣�Ķһ�ID��" + szContent);
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
            //���жϱ�������û��
            if (player.pFishGanPack.GetInfo(nTargetAvatarId) != null)
            {
                Debug.Log("��ɫ�������Ѿ�����������");
                return;
            }

            CHttpParam pReqParams = new CHttpParam(
               new CHttpParamSlot("uid", player.uid.ToString()),
               new CHttpParamSlot("ganId", nTargetAvatarId.ToString())
            );
            CHttpMgr.Instance.SendHttpMsg(CHttpConst.ExchangeFishGan, new HHandlerExchangeFishGan(), pReqParams);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}
