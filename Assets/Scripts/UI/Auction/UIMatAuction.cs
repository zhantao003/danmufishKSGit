using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMatAuction : UIBase
{
    [Header("宝箱图片")]
    public UIRawIconLoad rawIconLoad;
    [Header("宝箱名字")]
    public Text uiName;
    [Header("起拍价")]
    public Text uiBuyPrice;
    [Header("最小加价")]
    public Text uiAddPrice;
    [Header("竞拍人名字")]
    public Text uiPlayerName;
    [Header("倒计时")]
    public Text uiCountDown;

    public Color pNormalColor;
    public Color pOverColor;

    public UITweenPos tweenPos;
    public Vector3 vecShowPos;
    public Vector3 vecHidePos;


    public override void OnOpen()
    {
        base.OnOpen();
        if (CAuctionMatMgr.Ins != null)
        {
            CAuctionMatMgr.Ins.deleAuctionChgInfo = ChgInfo;
        }

        if (CNPCMgr.Ins != null)
        {
            CNPCMgr.Ins.pMatAuctionUnit.deleAuctionCountDown = SetCountDown;
        }
    }

    public void SetInfo()
    {
        if (CAuctionMatMgr.Ins.curTreasureInfo == null)
            return;
        if (CHelpTools.IsStringEmptyOrNone(CAuctionMatMgr.Ins.curTreasureInfo.szIcon))
        {
            if (CAuctionMatMgr.Ins.curTreasureInfo.emMatType == EMMaterialType.Role)
            {
                ST_UnitAvatar unitAvatar = CTBLHandlerUnitAvatar.Ins.GetInfo(CAuctionMatMgr.Ins.curTreasureInfo.nMatID);
                if (unitAvatar == null) 
                    return;
                rawIconLoad.SetIconSync(unitAvatar.szIcon);
                uiName.text = CAuctionMatMgr.Ins.curTreasureInfo.szName;
            }
            else if (CAuctionMatMgr.Ins.curTreasureInfo.emMatType == EMMaterialType.Boat)
            {
                ST_UnitFishBoat unitFishBoat = CTBLHandlerUnitFishBoat.Ins.GetInfo(CAuctionMatMgr.Ins.curTreasureInfo.nMatID);
                if (unitFishBoat == null)
                    return;
                rawIconLoad.SetIconSync(unitFishBoat.szIcon);
                uiName.text = CAuctionMatMgr.Ins.curTreasureInfo.szName;
            }
            else if(CAuctionMatMgr.Ins.curTreasureInfo.emMatType == EMMaterialType.Material)
            {
                ST_FishMat fishMat = CTBLHandlerFishMaterial.Ins.GetInfo(CAuctionMatMgr.Ins.nGetMatID);
                if (fishMat == null)
                    return;
                rawIconLoad.SetIconSync(fishMat.szIcon);
                uiName.text = fishMat.szName + "x" + CAuctionMatMgr.Ins.curTreasureInfo.nMatNum;
            }
            else if (CAuctionMatMgr.Ins.curTreasureInfo.emMatType == EMMaterialType.TargetMaterial)
            {
                ST_FishMat fishMat = CTBLHandlerFishMaterial.Ins.GetInfo(CAuctionMatMgr.Ins.nGetMatID);
                if (fishMat == null)
                    return;
                rawIconLoad.SetIconSync(fishMat.szIcon);
                uiName.text = fishMat.szName + "x" + CAuctionMatMgr.Ins.curTreasureInfo.nMatNum;
            }
        }
        else
        {
            rawIconLoad.SetIconSync(CAuctionMatMgr.Ins.curTreasureInfo.szIcon);
            if(CAuctionMatMgr.Ins.curTreasureInfo.emMatType == EMMaterialType.Role ||
               CAuctionMatMgr.Ins.curTreasureInfo.emMatType == EMMaterialType.Boat)
            {
                uiName.text = CAuctionMatMgr.Ins.curTreasureInfo.szName;
            }
            else
            {
                uiName.text = CAuctionMatMgr.Ins.curTreasureInfo.szName + "x" + CAuctionMatMgr.Ins.curTreasureInfo.nMatNum;
            }
        }
        
        SetAddPrice();
        //uiBaoDiCount.text = CAuctionMatMgr.Ins.nBaseCount + "个";
        SetBuyPrice(CAuctionMatMgr.Ins.nBuyPrice);

        uiPlayerName.text = "无";

    }

    void SetAddPrice()
    {
        if (CAuctionMatMgr.Ins.curTreasureInfo == null) return;

        switch (CAuctionMatMgr.Ins.curTreasureInfo.emPayType)
        {
            case EMPayType.FishCoin:
                {
                    uiAddPrice.text = CAuctionMatMgr.Ins.curTreasureInfo.nAddPrice + "积分";
                }
                break;
            case EMPayType.HaiDaoCoin:
                {
                    uiAddPrice.text = CAuctionMatMgr.Ins.curTreasureInfo.nAddPrice + "海盗金币";
                }
                break;
            case EMPayType.Mat:
                {
                    ST_FishMat fishMat = CTBLHandlerFishMaterial.Ins.GetInfo(CAuctionMatMgr.Ins.nPayMatID);
                    if (fishMat != null)
                    {
                        uiAddPrice.text = CAuctionMatMgr.Ins.curTreasureInfo.nAddPrice + fishMat.szName;
                    }
                }
                break;
        }

    }

    void SetBuyPrice(long value)
    {
        if (CAuctionMatMgr.Ins.curTreasureInfo == null) return;

        switch (CAuctionMatMgr.Ins.curTreasureInfo.emPayType)
        {
            case EMPayType.FishCoin:
                {
                    uiBuyPrice.text = value + "积分";
                }
                break;
            case EMPayType.HaiDaoCoin:
                {
                    uiBuyPrice.text = value + "个海盗金币";
                }
                break;
            case EMPayType.Mat:
                {
                    ST_FishMat fishMat = CTBLHandlerFishMaterial.Ins.GetInfo(CAuctionMatMgr.Ins.nPayMatID);
                    if (fishMat != null)
                    {
                        uiBuyPrice.text = value + "个" + fishMat.szName;
                    }
                }
                break;
        }
    }

    public void SetCountDown(float fValue)
    {
        uiCountDown.text = CHelpTools.GetTimeCounter((int)fValue);
        if (fValue >= 60)
        {
            uiCountDown.color = pNormalColor;
        }
        else
        {
            uiCountDown.color = pOverColor;
        }
    }

    public void ChgInfo(string uid, long price)
    {
        SetBuyPrice(price);
        CPlayerBaseInfo baseInfo = CPlayerMgr.Ins.GetPlayer(uid);
        if (baseInfo == null) return;
        uiPlayerName.text = baseInfo.userName;
    }


    public void Show()
    {
        if (tweenPos != null)
        {
            tweenPos.delayTime = 0.5f;
            tweenPos.from = vecHidePos;
            tweenPos.to = vecShowPos;
            tweenPos.Play();
        }
    }

    public void Hide()
    {
        if (tweenPos != null)
        {
            tweenPos.delayTime = 0f;
            tweenPos.from = vecShowPos;
            tweenPos.to = vecHidePos;
            tweenPos.Play();
        }
    }

}
