using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ShowBoardType
{
    Rank,               //����
    Battle,             //����
    SpecialBattle,      //������Ҿ���
    Gifts,              //����
}

public class UIRoomInfo : UIBase
{
    [Header("�����ز����")]
    public UITodaySpecialty todaySpecialty;
    [Header("�������")]
    public UIRankRoot rankRoot;
    [Header("�������")]
    public UIProfitRoot profitRoot;
    [Header("ŷ�ʽ����������")]
    public UIOuHuangRoot ouHuangRoot;
    [Header("�������")]
    public UIBattleRoot battleRoot;
    [Header("�������")]
    public UIGiftRoot giftRoot;
    [Header("��ʾ��Ϣ���")]
    public UIRoomTipInfo uiRoomTipInfo;
    [Header("������ť��")]
    public GameObject objTopBtns;
    [Header("���ְ�����")]
    public GameObject objHelp;

    public GameObject objChgBtn;

    public UITieTieRoot uiTieTieRoot;

    public GameObject objInfoTip;
    public GameObject objBtnRank;

    public GameObject[] objShowRank;
    public GameObject[] objHideRank;

    public UITweenPos uiInfoTween;
    public UITweenPos uiTopBtnTween;
    public UITweenPos uiBattleTween;
    public UITweenPos uiGiftsTween;

    public UITweenPos uiRankTween;
    public Vector3 vRankShow;
    public Vector3 vRankHide;
    public bool bShowRank = true;
    public Image pArrowImg;

    public UITweenPos uiSpecialtyTween;
    public Vector3 vSpecialtyShow;
    public Vector3 vSpecialtyHide;

    [Header("չʾ�߶�")]
    public float fShowHeight;
    [Header("���ظ߶�")]
    public float fHideHeight;

    public ShowBoardType emCurShowType;

    public string szSaveFileName;

    public CAudioMgr.CAudioSlottInfo pGiftAudio;
    public CAudioMgr.CAudioSlottInfo pBigGiftAudio;

    CPropertyTimer pGiftShowTick;
    public float fGiftShowTime;
    public float fGiftShowCD;
    public int nGiftShowRate;
    public int nShowGiftMax;
    int nCurShowGift;

    public float fChgTime;
    CPropertyTimer pChgTick;

    public UICommonBtnShow uiChgBtn;

    //CPropertyTimer pCheckRankTick;
    //public float fCheckRankTime;

    private void Update()
    {
        if (pChgTick != null &&
           pChgTick.Tick(CTimeMgr.DeltaTime))
        {
            pChgTick = null;
            uiChgBtn.OnClickChg();
            RefreshChgTick();
        }

        //if(pGiftShowTick != null &&
        //   pGiftShowTick.Tick(CTimeMgr.DeltaTime))
        //{
        //    if(CGameColorFishMgr.Ins.nCurRateUpLv < CGameColorFishMgr.Ins.pStaticConfig.GetInt("���񴬳��ֵȼ�"))
        //    {
        //        return;
        //    }

        //    if (emCurShowType == ShowBoardType.Battle ||
        //        emCurShowType == ShowBoardType.SpecialBattle)
        //    {
        //        pGiftShowTick.Value = 120;
        //        pGiftShowTick.FillTime();
        //        return;
        //    }

        //    pGiftShowTick = null;
        //    CheckShowGift();
        //}
    }

    public void RefreshChgTick()
    {
        pChgTick = new CPropertyTimer();
        pChgTick.Value = fChgTime;
        pChgTick.FillTime();
    }

    public void CheckShowGift()
    {
        int nRandomValue = UnityEngine.Random.Range(0, 101);
        if(nRandomValue <= nGiftShowRate || nCurShowGift >= nShowGiftMax)
        {
            nCurShowGift = 0;
            ShowGifts();
        }
        else
        {
            nCurShowGift++;
            pGiftShowTick = new CPropertyTimer();
            pGiftShowTick.Value = fGiftShowTime;
            pGiftShowTick.FillTime();
        }
    }

    public override void OnOpen()
    {
        base.OnOpen();
        Init();
        RefreshChgTick();
        //uiRoomTipInfo.Init();

        //CLocalRankInfoMgr.Ins.RefreshFileName();
        //nCurShowGift = 0;
        //if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal)
        //{
        //    pGiftShowTick = new CPropertyTimer();
        //    pGiftShowTick.Value = fGiftShowTime;
        //    pGiftShowTick.FillTime();
        //}

        //bool bAdd = CSystemInfoMgr.Inst.GetBool(CSystemInfoConst.ADDSEAT);
        //if(bAdd)
        //{
        //    uiRankTween.tranTarget.localPosition = vRankHide;
        //    bShowRank = false;
        //    pArrowImg.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
        //}
        //else
        //{
        //    uiRankTween.tranTarget.localPosition = vRankShow;
        //    bShowRank = true;
        //    pArrowImg.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
        //}

        uiRankTween.tranTarget.localPosition = vRankHide;
        bShowRank = false;
        pArrowImg.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));

        ////TODO:20��֮ǰ�ȹر�
        //todaySpecialty.gameObject.SetActive(false);

        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema)
        {
            //objBtnRank.SetActive(false);

            uiRankTween.tranTarget.localPosition = vRankHide;
            bShowRank = false;
            pArrowImg.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }
        else
        {
            //objBtnRank.SetActive(true);
        }
    }

    public void ShowRank()
    {
        if (uiRankTween == null) return;
        uiRankTween.from = vRankHide;
        uiRankTween.to = vRankShow;
        uiRankTween.delayTime = 0f;
        uiRankTween.Play();
        bShowRank = true;
        for(int i = 0;i < objShowRank.Length;i++)
        {
            objShowRank[i].SetActive(true);
        }
        for (int i = 0; i < objHideRank.Length; i++)
        {
            objHideRank[i].SetActive(false);
        }
        if (pArrowImg == null) return;
        pArrowImg.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
    }

    public void HideRank()
    {
        if (uiRankTween == null) return;
        uiRankTween.from = vRankShow;
        uiRankTween.to = vRankHide;
        uiRankTween.delayTime = 0f;
        uiRankTween.Play();
        bShowRank = false;
        for (int i = 0; i < objShowRank.Length; i++)
        {
            objShowRank[i].SetActive(false);
        }
        for (int i = 0; i < objHideRank.Length; i++)
        {
            objHideRank[i].SetActive(true);
        }
        if (pArrowImg == null) return;
        pArrowImg.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
    }

    public void ShowSpecialty()
    {
        uiSpecialtyTween.from = vSpecialtyHide;
        uiSpecialtyTween.to = vSpecialtyShow;
        uiSpecialtyTween.delayTime = 0.5f;
        uiSpecialtyTween.Play();
    }

    public void HideSpecialty()
    {
        uiSpecialtyTween.from = vSpecialtyShow;
        uiSpecialtyTween.to = vSpecialtyHide;
        uiSpecialtyTween.delayTime = 0f;
        uiSpecialtyTween.Play();
    }

    public void ActiveTopBtn(bool bActive)
    {
        if (objTopBtns == null) return;
        objTopBtns.SetActive(bActive);
        if (objHelp == null) return;
        objHelp.SetActive(!bActive);
    }

    /// <summary>
    /// ������Ч
    /// </summary>
    public void PlayGiftAudio()
    {
        if (pGiftAudio != null)
        {
            CAudioMgr.Ins.PlaySoundBySlot(pGiftAudio, transform.position);
        }
    }

    /// <summary>
    /// ������Ч
    /// </summary>
    public void PlayBigGiftAudio()
    {
        if (pBigGiftAudio != null)
        {
            CAudioMgr.Ins.PlaySoundBySlot(pBigGiftAudio, transform.position);
        }
    }

    /// <summary>
    /// չʾ�������
    /// </summary>
    public void ShowGifts()
    {
        if (CGameColorFishMgr.Ins.pMap.pGiftsBoat == null) return;
        PlayGiftAudio();
        emCurShowType = ShowBoardType.Gifts;
        ShowBoard(uiGiftsTween, 0.5f, 0.5f);
        HideBoard(uiTopBtnTween, 0.5f, 0f);
        UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (gameInfo != null)
        {
            gameInfo.HideBatteryBoard(0.5f, 0f);
        }
        UIRollText rollText = UIManager.Instance.GetUI(UIResType.RollText) as UIRollText;
        if (rollText != null)
        {
            rollText.HideInfo();
        }

        giftRoot.InitInfo();

        UIShowRoot showRoot = UIManager.Instance.GetUI(UIResType.ShowRoot) as UIShowRoot;
        if (showRoot != null)
        {
            showRoot.HideBoard();
        }
        UITreasureInfo.HideUI(0f);

        CGameColorFishMgr.Ins.pMap.pDuelBoat = CGameColorFishMgr.Ins.pMap.pGiftsBoat;
        if (CGameColorFishMgr.Ins.pMap.pDuelBoat != null)
        {
            CGameColorFishMgr.Ins.pMap.pDuelBoat.SetState(CDuelBoat.EMState.Show);
        }

    }

    /// <summary>
    /// չʾ������Ҿ���
    /// </summary>
    public void ShowSpecialBattle()
    {
        emCurShowType = ShowBoardType.SpecialBattle;
        ShowBoard(uiBattleTween, 0.5f, 0.5f);
        HideBoard(uiTopBtnTween, 0.5f, 0f);
        UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (gameInfo != null)
        {
            gameInfo.HideBatteryBoard(0.5f, 0f);
        }
        UIRollText rollText = UIManager.Instance.GetUI(UIResType.RollText) as UIRollText;
        if (rollText != null)
        {
            rollText.HideInfo();
        }

        battleRoot.InitInfo();

        UIShowRoot showRoot = UIManager.Instance.GetUI(UIResType.ShowRoot) as UIShowRoot;
        if (showRoot != null)
        {
            showRoot.HideBoard();
        }
        UITreasureInfo.HideUI(0f);

        CGameColorFishMgr.Ins.pMap.pDuelBoat = CGameColorFishMgr.Ins.pMap.pSpecialBoat;
        if (CGameColorFishMgr.Ins.pMap.pDuelBoat != null)
        {
            CGameColorFishMgr.Ins.pMap.pDuelBoat.SetState(CDuelBoat.EMState.Show);
        }
    }

    /// <summary>
    /// չʾ�������
    /// </summary>
    public void ShowBattle()
    {
        emCurShowType = ShowBoardType.Battle;
        ShowBoard(uiBattleTween, 0.5f, 0.5f);
        HideBoard(uiTopBtnTween, 0.5f, 0f);
        UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (gameInfo != null)
        {
            gameInfo.HideBatteryBoard(0.5f, 0f);
        }
        UICrazyGiftTip crazyGiftTip = UIManager.Instance.GetUI(UIResType.CrazyGiftTip) as UICrazyGiftTip;
        if(crazyGiftTip != null)
        {
            crazyGiftTip.ShowPosByBattle(0.5f);
        }
        UICrazyTime crazyTime = UIManager.Instance.GetUI(UIResType.CrazyTime) as UICrazyTime;
        if (crazyTime != null)
        {
            crazyTime.ShowPosByBattle(0.5f);
        }
        UIRollText rollText = UIManager.Instance.GetUI(UIResType.RollText) as UIRollText;
        if(rollText != null)
        {
            rollText.HideInfo();
        }

        battleRoot.InitInfo();

        UIShowRoot showRoot = UIManager.Instance.GetUI(UIResType.ShowRoot) as UIShowRoot;
        if (showRoot != null)
        {
            showRoot.HideBoard();
        }
        UITreasureInfo.HideUI(0f);

        CGameColorFishMgr.Ins.pMap.pDuelBoat = CGameColorFishMgr.Ins.pMap.pNormalBoat;
        if (CGameColorFishMgr.Ins.pMap.pDuelBoat!= null)
        {
            CGameColorFishMgr.Ins.pMap.pDuelBoat.SetState(CDuelBoat.EMState.Show);
        }
    }

    /// <summary>
    /// չʾ��Ϣ���
    /// </summary>
    public void ShowInfo(float fDelay = 0f,bool bHideBattle = true)
    {
        if(emCurShowType == ShowBoardType.Gifts)
        {
            pGiftShowTick = new CPropertyTimer();
            pGiftShowTick.Value = fGiftShowCD;
            pGiftShowTick.FillTime();
        }
        emCurShowType = ShowBoardType.Rank;
        ShowBoard(uiTopBtnTween, 0.5f, fDelay + 0.5f);
        if (bHideBattle)
        {
            HideBoard(uiBattleTween, 0.5f, fDelay + 0f);
        }
        UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if(gameInfo != null)
        {
            gameInfo.ShowBatteryBoard(0.5f, fDelay + 0.5f);
        }
        UICrazyGiftTip crazyGiftTip = UIManager.Instance.GetUI(UIResType.CrazyGiftTip) as UICrazyGiftTip;
        if (crazyGiftTip != null)
        {
            crazyGiftTip.BackOriPos(0f);
        }
        UICrazyTime crazyTime = UIManager.Instance.GetUI(UIResType.CrazyTime) as UICrazyTime;
        if(crazyTime != null)
        {
            crazyTime.BackOriPos(0f);
        }
        UIRollText rollText = UIManager.Instance.GetUI(UIResType.RollText) as UIRollText;
        if (rollText != null)
        {
            rollText.ShowInfo(fDelay + 0.5f);
        }

        UIShowRoot showRoot = UIManager.Instance.GetUI(UIResType.ShowRoot) as UIShowRoot;
        if (showRoot != null)
        {
            showRoot.ShowBoard();
        }
        UITreasureInfo.ShowUI(fDelay);
        if (CGameColorFishMgr.Ins.pMap.pDuelBoat != null)
        {
            CGameColorFishMgr.Ins.pMap.pDuelBoat.SetState(CDuelBoat.EMState.Hide);
        }
    }

    /// <summary>
    /// չʾ��Ӧ���
    /// </summary>
    /// <param name="tweenTarget"></param>
    /// <param name="fDelay"></param>
    /// <param name="fPlay"></param>
    public void ShowBoard(UITweenPos tweenTarget, float fPlay, float fDelay)
    {
        tweenTarget.enabled = true;
        tweenTarget.from = new Vector3(tweenTarget.tranTarget.localPosition.x, fHideHeight, 0); 
        tweenTarget.to = new Vector3(tweenTarget.tranTarget.localPosition.x, fShowHeight, 0);
        tweenTarget.delayTime = fDelay;
        tweenTarget.playTime = fPlay;
        tweenTarget.Play();
    }

    /// <summary>
    /// ���ض�Ӧ���
    /// </summary>
    /// <param name="tweenTarget"></param>
    /// <param name="fDelay"></param>
    /// <param name="fPlay"></param>
    public void HideBoard(UITweenPos tweenTarget, float fPlay, float fDelay)
    {
        tweenTarget.enabled = true;
        tweenTarget.from = new Vector3(tweenTarget.tranTarget.localPosition.x, fShowHeight, 0);
        tweenTarget.to = new Vector3(tweenTarget.tranTarget.localPosition.x, fHideHeight, 0);
        tweenTarget.delayTime = fDelay;
        tweenTarget.playTime = fPlay;
        tweenTarget.Play();
    }

    //private void Update()
    //{
    //    if(pCheckRankTick != null &&
    //       pCheckRankTick.Tick(CTimeMgr.DeltaTime))
    //    {
    //        SaveInfo();
    //        pCheckRankTick = null;
    //    }
    //}

    public void Init()
    {
        emCurShowType = ShowBoardType.Rank;
        //todaySpecialty.Init();
        rankRoot.InitInfo();
        profitRoot.InitInfo();
        //ouHuangRoot.Reset();
    }

    public void ClearRankInfo()
    {
        //if(pCheckRankTick != null)
        //{
        //    pCheckRankTick = null;
        //}
        SaveInfo();
        rankRoot.Clear();
        profitRoot.Clear();
        CLocalRankInfoMgr.Ins.RefreshFileName();
    }


    public void SaveInfo()
    {
        CLocalRankInfoMgr.Ins.SaveInfo();
    }


    public void LoadByMsg(CLocalNetMsg msg)
    {
        ///��ȡʱ���
        long nlTime = msg.GetLong("time");
        ///��ȡŷ��������Ϣ
        CLocalNetArrayMsg pRankArrayMsg = msg.GetNetMsgArr("RankInfos");
        int nRankSize = pRankArrayMsg.GetSize();
        for(int i = 0;i < nRankSize;i++)
        {
            CLocalNetMsg pInfo = pRankArrayMsg.GetNetMsg(i);
            //UID
            pInfo.GetLong("useruid");
            //�û���
            pInfo.GetString("username");
            //����
            pInfo.GetString("fishname");
            //��Ʒ��
            //(EMRare)pInfo.GetInt("fishrare");
            //��ߴ�
            pInfo.GetFloat("fishsize");
            //��۸�
            pInfo.GetLong("fishprice");
            //�Ƿ����
            pInfo.GetInt("bianyi");
            //�Ƿ�ը������
            pInfo.GetInt("boom");
        }

        ///��ȡ����������Ϣ
        CLocalNetArrayMsg pProfitArrayMsg = msg.GetNetMsgArr("ProfitInfos");
        int nProfitSize = pProfitArrayMsg.GetSize();
        for(int i = 0;i < nProfitSize;i++)
        {
            CLocalNetMsg pInfo = pProfitArrayMsg.GetNetMsg(i);
            //�û���
            pInfo.GetString("username");
            //������
            pInfo.GetLong("totalprice");
        }
    }

    #region ��ť�¼�

    public void OnClickSetting()
    {
        UIManager.Instance.OpenUI(UIResType.Setting);
    }

    public void OnClickExit()
    {
        UIMsgBox.Show("�˳���Ϸ", "Ҫ����������Ϸ�ص��˵���", UIMsgBox.EMType.YesNo, delegate ()
        {
            //�����������
            //CDanmuMgr.Ins.EndGame();
            CPlayerMgr.Ins.ClearAllActiveUnit();
            CPlayerMgr.Ins.ClearAllIdleUnit();
            CPlayerMgr.Ins.ClearActivePlayer();

            CloseSelf();

            UIManager.Instance.OpenUI(UIResType.Loading);
            CSceneMgr.Instance.LoadScene(CSceneFactory.EMSceneType.MainMenu);
        });
    }

    public void OnClickUIHide()
    {
        UIManager.Instance.OpenUI(UIResType.UIHideSetting);
    }

    public void OnClickRoleShop()
    {
        //UIManager.Instance.OpenUI(UIResType.GetRole);
        UIManager.Instance.OpenUI(UIResType.GetBoat);
    }

    public void OnClickSeasonReward()
    {
        UIManager.Instance.OpenUI(UIResType.AvatarInfo);
    }

    public void OnClickTreasureShop()
    {
        UIManager.Instance.OpenUI(UIResType.ShopTreasure);
    }

    public void OnClickBoatShop()
    {
        UIManager.Instance.OpenUI(UIResType.GetBoat);
    }

    public void OnClickRankList()
    {
        UIManager.Instance.OpenUI(UIResType.RankList);
    }

    public void OnClickHelp()
    {
        UIManager.Instance.OpenUI(UIResType.Guide);
    }

    public void OnClickShowRank()
    {
        if (bShowRank)
        {
            objChgBtn.SetActive(false);
            HideRank();
        }
        else
        {
            objChgBtn.SetActive(true);
            ShowRank();
        }
    }

    #endregion

}
