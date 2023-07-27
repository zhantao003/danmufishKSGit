using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CSpecialGift
{
    public CPlayerBaseInfo baseInfo;
    public CSpecialGiftInfo giftInfo;
}


public class UICrazyGiftTip : UIBase
{
    public UICrazyGiftTipSlot giftTipSlot;

    /// <summary>
    /// 保存数据
    /// </summary>
    public List<CSpecialGift> listSaveUnits = new List<CSpecialGift>();
    /// <summary>
    /// 保存数据的最大数量
    /// </summary>
    public int nMaxSaveCount = 50;

    bool bShowInfo;

    public GameObject objCrazyProgress;
    public Text uiLabelCrazyProgress;
    public Image uiImgCrazyProgress;

    public UITweenPos tweenPos;

    public Vector3 vOriPos;
    public Vector3 vShowPosByBattle;
    public Vector3 vShowPosByAuction;

    public void ShowPosByBattle(float fDelay)
    {
        tweenPos.from = vOriPos;
        tweenPos.to = vShowPosByBattle;
        tweenPos.delayTime = fDelay;
        tweenPos.Play();
    }

    public void ShowPosByAuction(float fDelay)
    {
        tweenPos.from = vOriPos;
        tweenPos.to = vShowPosByAuction;
        tweenPos.delayTime = fDelay;
        tweenPos.Play();
    }

    public void BackOriPos(float fDelay)
    {
        tweenPos.from = tweenPos.tranTarget.localPosition;
        tweenPos.to = vOriPos;
        tweenPos.delayTime = fDelay;
        tweenPos.Play();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        InitInfo();

        ShowCrazyProgress(true);
        RefreshCrazyProgress((int)CCrazyTimeMgr.Ins.nlTotalBatteryCount, (int)CCrazyTimeMgr.Ins.fCheckBattery);
    }

    void InitInfo()
    {
        bShowInfo = false;
        giftTipSlot.deleShowOverEvent = ShowNext;
    }

    public static void ShowInfo(CSpecialGift specialGiftInfo)
    {
        UICrazyGiftTip rankChg = UIManager.Instance.GetUI(UIResType.CrazyGiftTip) as UICrazyGiftTip;
        if (rankChg != null)
        {
            rankChg.ShowPlayerInfo(specialGiftInfo);
        }
    }

    public void ShowPlayerInfo(CSpecialGift specialGiftInfo)
    {
        if (bShowInfo)
        {
            if (listSaveUnits.Count < nMaxSaveCount)
            {
                listSaveUnits.Add(specialGiftInfo);
            }
        }
        else
        {
            bShowInfo = true;
            giftTipSlot.ShowInfo(specialGiftInfo);
        }
    }

    public void ShowNext()
    {
        if (listSaveUnits.Count > 0)
        {
            giftTipSlot.ShowInfo(listSaveUnits[0]);
            listSaveUnits.RemoveAt(0);
        }
        else
        {
            bShowInfo = false;
        }
    }

    public static void ShowCrazyProgress(bool show)
    {
        return;

        UICrazyGiftTip uiCrazyTip = UIManager.Instance.GetUI(UIResType.CrazyGiftTip) as UICrazyGiftTip;
        if (uiCrazyTip != null)
        {
            uiCrazyTip.objCrazyProgress.SetActive(show);
        }
    }

    //刷新急速进度
    public static void RefreshCrazyProgress(int cur, int max)
    {
        UICrazyGiftTip uiCrazyTip = UIManager.Instance.GetUI(UIResType.CrazyGiftTip) as UICrazyGiftTip;
        if (uiCrazyTip != null)
        {
            uiCrazyTip.uiImgCrazyProgress.fillAmount = (float)cur / (float)max;
            uiCrazyTip.uiLabelCrazyProgress.text = "急速能量  " + cur + "/" + max;
        }
    }
}
