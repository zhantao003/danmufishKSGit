using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGetGiftInfo
{
    public CPlayerBaseInfo playerInfo;      //玩家信息
    public int nGiftValue;
}


public class UIGiftRoot : MonoBehaviour
{
    [Header("收礼面板")]
    public UIGiftReady giftReady;
    
    public List<CGetGiftInfo> listShowInfos = new List<CGetGiftInfo>();

    /// <summary>
    /// 当前获取大奖数量
    /// </summary>
    public int nCurGetBig;
    /// <summary>
    /// 获取大奖上限
    /// </summary>
    public int nMaxGetBig = 2;

    /// <summary>
    /// 获得大奖的概率
    /// </summary>
    public float fGetBigRateBase = 0.01f;
    
    /// <summary>
    /// 是否正在结算
    /// </summary>
    public bool bResultTime;

    public int nNormalFishCoin = 888;
    
    public void AddGiftValue(CGetGiftInfo giftInfo)
    {
        CGetGiftInfo info = listShowInfos.Find(x => x.playerInfo.uid == giftInfo.playerInfo.uid);
        
        if (info == null)
        {
            listShowInfos.Add(giftInfo);
        }
        else
        {
            info.nGiftValue += giftInfo.nGiftValue;
        }
        giftReady.AddGift(giftInfo.nGiftValue);
    }

    public void InitInfo()
    {
        giftReady.SetActive(true);
        giftReady.InitInfo();
        giftReady.pEndEvent = EndReady;
        Clear();
        bResultTime = false;
        UIGameInfo roomInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (roomInfo != null)
        {
            roomInfo.SetGiftDialog("");
        }
    }

    void EndReady()
    {
        EndEvent();
        CTimeTickMgr.Inst.PushTicker(8f, delegate (object[] obj)
         {
             UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
             if (roomInfo != null)
             {
                 roomInfo.HideBoard(roomInfo.uiGiftsTween, 0.5f, 0f);
                 roomInfo.ShowInfo(5f, false);
             }
         });
    }

    void EndEvent()
    {
        if (giftReady.bSuc)
        {
            UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
            
            if (CGameColorFishMgr.Ins.pMap != null &&
               CGameColorFishMgr.Ins.pMap.pDuelBoat != null)
            {
                CGameColorFishMgr.Ins.pMap.pDuelBoat.PlayEffect();
            }
            ///获取本次大奖数量
            nCurGetBig = 0;
            List<float> listCheckValue = new List<float>();
            float fGetBigRate = fGetBigRateBase;
            int nCheckCount = 0;
            if (listShowInfos.Count >= nMaxGetBig)
            {
                nCheckCount = nMaxGetBig;
            }
            else
            {
                nCheckCount = listShowInfos.Count;
            }
            float fGetValue = 0f;
            for (int i = 1; i <= nCheckCount; i++)
            {
                fGetValue = 0;
                for (int j = 0; j < i; j++)
                {
                    if (j == 0)
                    {
                        fGetValue = fGetBigRate;
                    }
                    else
                    {
                        fGetValue *= fGetValue;
                    }
                }
                listCheckValue.Add(fGetValue);
            }
            float fGetRandomValue = Random.Range(0f, 1f);
            listCheckValue.Reverse();
            for (int i = 0; i < listCheckValue.Count; i++)
            {
                if (fGetRandomValue <= listCheckValue[i])
                {
                    nCurGetBig++;
                }
            }

            ///随机排序
            for (int i = 0; i < listShowInfos.Count; i++)
            {
                CGetGiftInfo temp = listShowInfos[i];
                int randomIdx = Random.Range(0, listShowInfos.Count);
                listShowInfos[i] = listShowInfos[randomIdx];
                listShowInfos[randomIdx] = temp;
            }

            ///获取大奖的玩家
            for (int i = 0; i < nCurGetBig; i++)
            {
                if (listShowInfos[i] == null) continue;
                CGetGiftInfo getGiftInfo = listShowInfos[i];
                ST_GiftMenuByBoat giftInfo = CTBLHandlerGiftMenuByBoat.pIns.GetRandomInfoByRare(EMGiftMenuRare.Special);

                if (giftInfo == null)
                {
                    Debug.LogWarning("无大奖品信息");
                    continue;
                }

                AddReward(getGiftInfo.playerInfo, giftInfo);
                GiftCastInfo castInfo = new GiftCastInfo();
                castInfo.playerInfo = getGiftInfo.playerInfo;
                castInfo.giftInfo = giftInfo;
                if (i == 0)
                {
                    UISpecialCast.ShowGiftInfo(castInfo);

                    if (roomInfo != null)
                    {
                        roomInfo.PlayBigGiftAudio();
                    }
                }
                else
                {
                    CTimeTickMgr.Inst.PushTicker(5f * i, delegate (object[] values)
                    {
                        UISpecialCast.ShowGiftInfo(castInfo);
                    });
                }
            }

            ///获取小奖的玩家
            for (int i = nCurGetBig; i < listShowInfos.Count; i++)
            {
                if (listShowInfos[i] == null) continue;
                CGetGiftInfo getGiftInfo = listShowInfos[i];
                ST_GiftMenuByBoat giftInfo = CTBLHandlerGiftMenuByBoat.pIns.GetRandomInfoByRare(EMGiftMenuRare.Normal);
               
                if(giftInfo == null)
                {
                    Debug.LogWarning("无小奖品信息");
                    continue;
                }
                
                AddReward(getGiftInfo.playerInfo, giftInfo);
            }
        }
        else
        {
            ///获取鱼币的玩家
            for (int i = 0; i < listShowInfos.Count; i++)
            {
                if (listShowInfos[i] == null) continue;
                CGetGiftInfo getGiftInfo = listShowInfos[i];
                ST_GiftMenuByBoat giftInfo = new ST_GiftMenuByBoat();
                giftInfo.emGiftMenuRare = EMGiftMenuRare.Normal;
                giftInfo.emGiftMenuType = EMGiftMenuType.FishCoin;
                giftInfo.nCount = nNormalFishCoin;
                AddReward(getGiftInfo.playerInfo, giftInfo);
            }
        }
    }

    public void AddReward(CPlayerBaseInfo baseInfo, ST_GiftMenuByBoat giftInfo)
    {
        if (baseInfo == null)
        {
            Debug.LogError("None Player Info");
            return;
        }

        if(giftInfo == null)
        {
            Debug.LogError("None Gift Info");

            giftInfo = new ST_GiftMenuByBoat();
            giftInfo.emGiftMenuRare = EMGiftMenuRare.Normal;
            giftInfo.emGiftMenuType = EMGiftMenuType.FishCoin;
            giftInfo.nCount = nNormalFishCoin;
        }

        if(giftInfo == null)
        {
            return;
        }

        CPlayerUnit playerUnit = CPlayerMgr.Ins.GetIdleUnit(baseInfo.uid);
        if (playerUnit == null)
            return;

        if (CGameColorFishMgr.Ins.pMap.pDuelBoat != null &&
            CGameColorFishMgr.Ins.pMap.pDuelBoat.giftUnit != null)
        {
            CGameColorFishMgr.Ins.pMap.pDuelBoat.giftUnit.PlayGiftEffectToTarget(playerUnit, giftInfo.emGiftMenuRare == EMGiftMenuRare.Special, 5f);
        }
       
        UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (gameInfo != null)
        {
            UIShowFishInfo showFishInfo = gameInfo.GetShowFishInfoSlot(baseInfo.uid);
            showFishInfo.InitGiftInfo(giftInfo);
        }
        switch (giftInfo.emGiftMenuType)
        {
            case EMGiftMenuType.FishCoin:
                {
                    if (playerUnit == null) return;
                    playerUnit.AddCoinByHttp(giftInfo.nCount, EMFishCoinAddFunc.Duel, false, false);
                }
                break;
            case EMGiftMenuType.FeiLun:
                {
                    long curTimeStamp = CGameColorFishMgr.Ins.GetNowServerTime();
                    CHttpParam pReqParams = new CHttpParam
                    (
                        new CHttpParamSlot("uid", baseInfo.uid.ToString()),
                        new CHttpParamSlot("itemType", EMGiftType.fishLun.ToString()),
                        new CHttpParamSlot("count", giftInfo.nCount.ToString()),
                        new CHttpParamSlot("time", curTimeStamp.ToString()),
                        new CHttpParamSlot("nonce", CEncryptHelper.AesEncrypt(baseInfo.uid.ToString() + CDanmuSDKCenter.Ins.szRoomId.ToString() + curTimeStamp))
                    );
                    CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddGiftCount, pReqParams, CHttpMgr.Instance.nReconnectTimes, true);
                }
                break;
            case EMGiftMenuType.FishPack:
                {
                    CPlayerNetHelper.AddFishItemPack(baseInfo.uid, giftInfo.nCount, giftInfo.nCount, giftInfo.nCount, 0);
                }
                break;
            case EMGiftMenuType.HaiDaoCoin:
                {
                    CPlayerNetHelper.AddTreasureCoin(baseInfo.uid, giftInfo.nCount);
                }
                break;
        }
       

    }



    public void Clear()
    {
        CGameColorFishMgr.Ins.pMap.pDuelBoat.Clear();
        listShowInfos.Clear();
        giftReady.Clear();
    }

}
