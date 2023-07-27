using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CDanmuGiftAttrite(CDanmuGiftConst.CardTreasureBoom)]
public class DGiftCardTreasureBoom : CDanmuGiftAction
{
    public override void DoAction(CDanmuGift dm)
    {
        long nGiftNum = dm.giftNum;
        string uid = dm.uid.ToString();

        //炸弹加金币
        long nGetCoin = CGameColorFishMgr.Ins.pStaticConfig.GetInt("宝藏炸弹海盗金币") * nGiftNum;
        AddCoin(uid, nGetCoin);

        if (CGameColorFishMgr.Ins.pMap == null) return;

        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null)
        {
            //先登录
            //CHttpParam pParamLogin = new CHttpParam
            //(
            //    new CHttpParamSlot("uid", uid.ToString()),
            //    new CHttpParamSlot("nickName", System.Net.WebUtility.UrlEncode(dm.userName)),
            //    new CHttpParamSlot("headIcon", System.Net.WebUtility.UrlEncode(dm.userFace)),
            //    new CHttpParamSlot("userType", ((int)CPlayerBaseInfo.EMUserType.Guanzhong).ToString()),
            //    new CHttpParamSlot("fansMedalLevel", dm.fansMedalLevel.ToString()),
            //    new CHttpParamSlot("fansMedalName", dm.fansMedalName),
            //    new CHttpParamSlot("fansMedalWearingStatus", dm.fansMedalWearingStatus.ToString()),
            //    new CHttpParamSlot("guardLevel", dm.guardLevel.ToString())
            //);

            //CHttpMgr.Instance.SendHttpMsg(
            //    CHttpConst.LoginViewer,
            //    new HHandlerLoginViewer(dm.userName, dm.userFace, "", null, delegate (CPlayerBaseInfo info)
            //    {
            //        GetBomber(nGiftNum, info); 
            //    }),
            //    pParamLogin);

            CPlayerNetHelper.Login(uid, dm.nickName, dm.headIcon, CPlayerBaseInfo.EMUserType.Guanzhong,
                                   dm.fanLv, dm.fanName, dm.fanEquip, dm.vipLv,
                    new HHandlerLoginViewer(dm.nickName, dm.headIcon, "", null, delegate (CPlayerBaseInfo info, CGameColorFishMgr.EMJoinType joinType)
                    {
                        GetBomber(nGiftNum, info);
                    }));
        }
        else
        {
            GetBomber(nGiftNum, pPlayer);
        }
    }

    void GetBomber(long num, CPlayerBaseInfo info)
    {
        CPlayerUnit pUnit = CPlayerMgr.Ins.GetIdleUnit(info.uid);
        if (pUnit == null)
        {
            if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
            {
                return;
            }

            if (CPlayerMgr.Ins.dicIdleUnits.Count >= CGameColorFishMgr.Ins.nMaxPlayerNum ||
                CGameColorFishMgr.Ins.pMap.GetRandIdleRoot() == null)
            {
                UIToast.Show("人数已满");
                return;
            }

            CGameColorFishMgr.Ins.JoinPlayer(info, CGameColorFishMgr.EMJoinType.Gift);

            pUnit = CPlayerMgr.Ins.GetIdleUnit(info.uid);
        }

        if(pUnit!=null)
        {
            pUnit.AddTreasureBoom((int)num);

            if (info.CheckHaveGift())
            {
                pUnit.ClearExitTick();
            }
            else
            {
                pUnit.ResetExitTick();
            }
        }
    }

    void AddCoin(string uid, long add)
    {
        CPlayerNetHelper.AddTreasureCoin(uid,
                                         add);
    }
}
