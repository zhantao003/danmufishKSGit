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
    /// ��ģʽ��ʱ��
    /// </summary>
    CPropertyTimer pCheckCrazyTick;
    [Header("�����ۻ���ʱ��")]
    public float fCrazyTimeGift;
    [Header("��Ͷ��ʱ��")]
    public float fCrazyTimeGacha;
    [Header("��ش������")]
    public long fCheckBattery;
    [Header("��ǰ���ۻ��������")]
    public long nlTotalBatteryCount;
    public bool bCrazy = false;

    /// <summary>
    /// ��Ͷģʽ��ʱ��
    /// </summary>
    CPropertyTimer pCheckKongTouTick;
    [Header("��Ͷ�ۻ���Ͷģʽʱ��")]
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
            //չʾUI���
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

        //����������Ч
        if (CGameColorFishMgr.Ins.pMap != null)
        {
            CreateCrazyEff();
        }

        //�رռ��ٽ���UI
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
            ////չʾUI���
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

        ////����������Ч
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
            //�ر�UI���
            UICrazyTime.HideCrazyTime();
            UICrazyGiftTip.ShowCrazyProgress(true);
            //UIBatteryTarget.ShowBoard();
        }

        if (pCheckKongTouTick != null &&
           pCheckKongTouTick.Tick(CTimeMgr.DeltaTime))
        {
            bKongTou = false;
            pCheckKongTouTick = null;
            //�ر�UI���
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
