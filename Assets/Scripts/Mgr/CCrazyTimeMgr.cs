using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCrazyTimeMgr : MonoBehaviour
{
    static CCrazyTimeMgr ins = null;

    public static CCrazyTimeMgr Ins
    {
        get
        {
            return ins;
        }
    }

    /// <summary>
    /// 狂暴模式计时器
    /// </summary>
    CPropertyTimer pCheckCrazyTick;
    [Header("送礼累积狂暴时间")]
    public float fCrazyTimeGift;
    [Header("空投狂暴时间")]
    public float fCrazyTimeGacha;
    [Header("电池达标数量")]
    public long fCheckBattery;
    [Header("当前已累积电池数量")]
    public long nlTotalBatteryCount;
    public bool bCrazy = false;

    /// <summary>
    /// 空投模式计时器
    /// </summary>
    CPropertyTimer pCheckKongTouTick;
    [Header("空投累积空投模式时间")]
    public float fKongTouTimeGift;

    public bool bKongTou = false;
    

    public CPropertyTimer pEffCrazyTicker = new CPropertyTimer();

    private void Start()
    {
        ins = this;
    }

    public void AddBattery(long value)
    {
        nlTotalBatteryCount += value;
        float fTime = 0;
        while (nlTotalBatteryCount >= fCheckBattery)
        {
            nlTotalBatteryCount -= fCheckBattery;
            fTime += fCrazyTimeGift;
        }

        UICrazyGiftTip.RefreshCrazyProgress((int)nlTotalBatteryCount, (int)fCheckBattery);

        AddCrazyTime(fTime);
    }

    public float GetCrazyTime()
    {
        float fCrazyTime = 0;
        if(pCheckCrazyTick != null)
        {
            fCrazyTime = pCheckCrazyTick.CurValue;
        }
        return fCrazyTime;
    }

    public void AddCrazyTime(float fTime)
    {
        if (fTime <= 0)
            return;

        if(pCheckCrazyTick == null)
        {
            //展示UI面板
            UICrazyTime.ShowCrazyTime();
            UIBatteryTarget.HideBoard();
            pCheckCrazyTick = new CPropertyTimer();
            pCheckCrazyTick.Value = fTime;
            pCheckCrazyTick.FillTime();

            pEffCrazyTicker.FillTime();
        }
        else
        {
            pCheckCrazyTick.CurValue += fTime;
        }
        bCrazy = true;

        //创建急速特效
        if (CGameColorFishMgr.Ins.pMap != null)
        {
            CreateCrazyEff();
        }

        //关闭急速进度UI
        UICrazyGiftTip uiCrazyTip = UIManager.Instance.GetUI(UIResType.CrazyGiftTip) as UICrazyGiftTip;
        if(uiCrazyTip!=null)
        {
            UICrazyGiftTip.ShowCrazyProgress(false);
        }
    }

    public float GetKongTouTime()
    {
        float fKongTouTime = 0;
        if (pCheckKongTouTick != null)
        {
            fKongTouTime = pCheckKongTouTick.CurValue;
        }
        return fKongTouTime;
    }

    public void AddKongTouTime(float fTime)
    {
        if (fTime <= 0)
            return;

        if (pCheckKongTouTick == null)
        {
            ////展示UI面板
            //UICrazyTime.ShowCrazyTime();
            pCheckKongTouTick = new CPropertyTimer();
            pCheckKongTouTick.Value = fTime;
            pCheckKongTouTick.FillTime();
            UICrazyTime crazyTime = UIManager.Instance.GetUI(UIResType.CrazyTime) as UICrazyTime;
            if(crazyTime != null)
            {
                crazyTime.ActiveKongTou(true);
            }
        }
        else
        {
            pCheckKongTouTick.CurValue += fTime;
        }
        bKongTou = true;

        ////创建急速特效
        //if (CGameColorFishMgr.Ins.pMap != null)
        //{
        //    CreateCrazyEff();
        //}

    }

    private void Update()
    {
        if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
        {
            if(CBattleModeMgr.Ins != null &&
               CBattleModeMgr.Ins.emCurState != CBattleModeMgr.EMGameState.Gaming)
            {
                return;
            }
        }

        if(bCrazy)
        {
            if(pEffCrazyTicker.Tick(CTimeMgr.DeltaTime))
            {
                CreateCrazyEff();

                pEffCrazyTicker.FillTime();
            }
        }

        if(pCheckCrazyTick != null &&
           pCheckCrazyTick.Tick(CTimeMgr.DeltaTime))
        {
            bCrazy = false;
            pCheckCrazyTick = null;
            //关闭UI面板
            UICrazyTime.HideCrazyTime();
            UICrazyGiftTip.ShowCrazyProgress(true);
            //UIBatteryTarget.ShowBoard();
        }

        if (pCheckKongTouTick != null &&
           pCheckKongTouTick.Tick(CTimeMgr.DeltaTime))
        {
            bKongTou = false;
            pCheckKongTouTick = null;
            //关闭UI面板
            UICrazyTime crazyTime = UIManager.Instance.GetUI(UIResType.CrazyTime) as UICrazyTime;
            if (crazyTime != null)
            {
                crazyTime.ActiveKongTou(false);
            }
            //UICrazyTime.HideCrazyTime();
            //UICrazyGiftTip.ShowCrazyProgress(true);
            //UIBatteryTarget.ShowBoard();
        }
    }

    void CreateCrazyEff()
    {
        CEffectMgr.Instance.CreateEffSync("Effect/effBuyCrazyTime",
               CGameColorFishMgr.Ins.pMap.tranMapEffRoot.position,
               Quaternion.identity,
               0);
    }
}
