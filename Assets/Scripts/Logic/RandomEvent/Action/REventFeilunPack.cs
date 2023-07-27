using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CRandomEventAttri(2)]
public class REventFeilunPack : CRandomEventAction
{
    public override void DoAction(CPlayerBaseInfo player)
    {
        if (player == null) return;
        int nItemCount = Random.Range(1, 3);

        long curTimeStamp = CGameColorFishMgr.Ins.GetNowServerTime();
        CHttpParam pReqParams = new CHttpParam
        (
            new CHttpParamSlot("uid", player.uid.ToString()),
            new CHttpParamSlot("itemType", "fishLun"),
            new CHttpParamSlot("count", nItemCount.ToString()),
            new CHttpParamSlot("time", curTimeStamp.ToString()),
            new CHttpParamSlot("nonce", CEncryptHelper.AesEncrypt(player.uid.ToString() + CDanmuSDKCenter.Ins.szRoomId.ToString() + curTimeStamp))
        );
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddGiftCount, pReqParams, CHttpMgr.Instance.nReconnectTimes, true);
    }
}
