using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CRandomEventAttri(1)]
public class REventGetItemPack : CRandomEventAction
{
    public override void DoAction(CPlayerBaseInfo player)
    {
        if (player == null) return;

        int nItemCount = Random.Range(1,6);
        int nItemType = Random.Range(0, 3);

        CHttpParam pReqParams = new CHttpParam
        (
            new CHttpParamSlot("uid", player.uid.ToString()),
            new CHttpParamSlot("fishItem", nItemCount.ToString())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddFreeBait, pReqParams,0, true);

        //switch (nItemType)
        //{
        //    case 0: //Óã¶ü
        //        {
        //            CHttpParam pReqParams = new CHttpParam
        //            (
        //                new CHttpParamSlot("uid", player.uid.ToString()),
        //                new CHttpParamSlot("fishItem", nItemCount.ToString())
        //            );

        //            CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddFreeBait, pReqParams);
        //        }
        //        break;
        //    case 1: //Óã¸Í
        //        {
        //            CHttpParam pReqParams = new CHttpParam
        //            (
        //                new CHttpParamSlot("uid", player.uid.ToString()),
        //                new CHttpParamSlot("itemType", "fishGan"),
        //                new CHttpParamSlot("count", nItemCount.ToString())
        //            );
        //            CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddGiftCount, pReqParams);
        //        }
        //        break;
        //    case 2: //¸¡Æ¯
        //        {
        //            CHttpParam pReqParams = new CHttpParam
        //            (
        //                new CHttpParamSlot("uid", player.uid.ToString()),
        //                new CHttpParamSlot("itemType", "fishPiao"),
        //                new CHttpParamSlot("count", nItemCount.ToString())
        //            );
        //            CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddGiftCount, pReqParams, CHttpMgr.Instance.nReconnectTimes);
        //        }
        //        break;
        //}
    }
}
