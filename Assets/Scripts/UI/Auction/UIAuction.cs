using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAuction : UIBase
{
    [Header("����ͼƬ")]
    public UIRawIconLoad rawIconLoad;
    [Header("��������")]
    public Text uiName;
    [Header("�ο���ֵ")]
    public Text uiNormalPrice;
    [Header("��������")]
    public Text uiBaoDiCount;
    [Header("���ļ�")]
    public Text uiBuyPrice;
    [Header("��С�Ӽ�")]
    public Text uiAddPrice;
    [Header("����������")]
    public Text uiPlayerName;
    [Header("����ʱ")]
    public Text uiCountDown;
    [Header("�������ͷ��")]
    public UIAuctionWaitPlayer[] uiAuctionImgs;

    public GameObject objSingleRoot;
    public GameObject objMultipleRoot;

    public Color pNormalColor;
    public Color pOverColor;

    public UITweenPos tweenPos;
    public Vector3 vecShowPos;
    public Vector3 vecHidePos;


    public override void OnOpen()
    {
        base.OnOpen();
        if(CAuctionMgr.Ins!=null)
        {
            CAuctionMgr.Ins.deleAuctionChgInfo = ChgInfo;
        }
        
        if(CNPCMgr.Ins!=null)
        {
            CNPCMgr.Ins.pAuctionUnit.deleAuctionCountDown = SetCountDown;
        }
    }

    public void SetInfo()
    {
        if (CAuctionMgr.Ins.curTreasureInfo != null)
        {
            rawIconLoad.SetIconSync(CAuctionMgr.Ins.curTreasureInfo.treasureInfo.szIcon);
            uiName.text = CAuctionMgr.Ins.curTreasureInfo.treasureInfo.szName;
            uiAddPrice.text = CAuctionMgr.Ins.curTreasureInfo.treasureInfo.nAddPrice.ToString();
            objSingleRoot.SetActive(CAuctionMgr.Ins.curTreasureInfo.treasureInfo.emAuctionLimitType == EMAuctionLimitType.Single);
            objMultipleRoot.SetActive(CAuctionMgr.Ins.curTreasureInfo.treasureInfo.emAuctionLimitType == EMAuctionLimitType.Multiple);
        }
        uiNormalPrice.text = CAuctionMgr.Ins.nNormalPrice.ToString();
        uiBaoDiCount.text = CAuctionMgr.Ins.nBaseCount + "��";
        uiBuyPrice.text = CAuctionMgr.Ins.nBuyPrice.ToString();

        uiPlayerName.text = "��";
        for (int i = 0; i < uiAuctionImgs.Length; i++)
        {
            uiAuctionImgs[i].SetActive(false);
        }
    }
     
    public void SetCountDown(float fValue)
    {
        uiCountDown.text = CHelpTools.GetTimeCounter((int)fValue);
        if(fValue >= 60)
        {
            uiCountDown.color = pNormalColor;
        }
        else
        {
            uiCountDown.color = pOverColor;
        }
    }

    public void ChgInfo(string uid,long price)
    {
        uiBuyPrice.text = price.ToString();

        List<CPlayerBaseInfo> listInfos = CAuctionMgr.Ins.listAuctionPlayer;
        for (int i = 0; i < listInfos.Count; i++)
        {
            if (i >= uiAuctionImgs.Length) break;
            string szHeadIcon = listInfos[i].userFace;
            uiAuctionImgs[i].SetActive(true);
            uiAuctionImgs[i].SetIcon(szHeadIcon);
        }
        if (listInfos.Count < uiAuctionImgs.Length)
        {
            for (int i = listInfos.Count; i < uiAuctionImgs.Length; i++)
            {
                uiAuctionImgs[i].SetActive(false);
            }
        }

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
