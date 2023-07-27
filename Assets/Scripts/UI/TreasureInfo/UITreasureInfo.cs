using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITreasureInfo : UIBase
{
    public UITreasureSlot pRollSlot;

    public Text uiLabelBaodi;

    public Text uiLabelBroadCast;

    public UITweenPos uiTweenPos;

    public UITweenPos uiTweenSlotPos;

    public List<CGiftGachaBoxInfo> listRollInfos = new List<CGiftGachaBoxInfo>();

    public float fStayTime;

    public Vector3 vecShowByUI;
    public Vector3 vecHideByUI;

    public Vector3 vecStartPos;
    public Vector3 vecShowPos;
    public Vector3 vecHidePos;

    int nCurShowIdx;

    CPropertyTimer pStayTick;

    public GameObject uiBtnFesGachaGift;

    public GameObject objYZM;

    public Text uiYZM;

    //public CPropertyTimer pTickerUpdateBroadContent = new CPropertyTimer();

    public static void ShowUI(float fDelay)
    {
        UITreasureInfo treasureInfo = UIManager.Instance.GetUI(UIResType.TreasureInfo) as UITreasureInfo;
        if(treasureInfo != null && treasureInfo.uiTweenPos != null)
        {
            treasureInfo.uiTweenPos.from = treasureInfo.vecHideByUI;
            treasureInfo.uiTweenPos.to = treasureInfo.vecShowByUI;
            treasureInfo.uiTweenPos.delayTime = fDelay;
            treasureInfo.uiTweenPos.Play();
        }
    }

    public static void HideUI(float fDelay)
    {
        UITreasureInfo treasureInfo = UIManager.Instance.GetUI(UIResType.TreasureInfo) as UITreasureInfo;
        if (treasureInfo != null && treasureInfo.uiTweenPos != null)
        {
            treasureInfo.uiTweenPos.from = treasureInfo.vecShowByUI;
            treasureInfo.uiTweenPos.to = treasureInfo.vecHideByUI;
            treasureInfo.uiTweenPos.delayTime = fDelay;
            treasureInfo.uiTweenPos.Play();
        }
    }

    public static void SetYZM()
    {
        UITreasureInfo treasureInfo = UIManager.Instance.GetUI(UIResType.TreasureInfo) as UITreasureInfo;
        if (treasureInfo != null && treasureInfo.uiYZM != null)
        {
            treasureInfo.uiYZM.text = CGameColorFishMgr.Ins.pMap.nYZMAnswerA + "+" + CGameColorFishMgr.Ins.pMap.nYZMAnswerB + "=?";
        }
    }

    public override void OnOpen()
    {
        InitInfo();

        uiBtnFesGachaGift.SetActive(CFishFesInfoMgr.Ins.IsFesOn((int)CFishFesInfoMgr.EMFesType.TreasureTip));

        if (uiYZM != null)
        {
            uiYZM.text = CGameColorFishMgr.Ins.pMap.nYZMAnswerA + "+" + CGameColorFishMgr.Ins.pMap.nYZMAnswerB + "=?";
        }

        if(objYZM != null)
        {
            objYZM.SetActive(false);
            //objYZM.SetActive(CGameColorFishMgr.Ins.nCurRateUpLv > CGameColorFishMgr.Ins.pStaticConfig.GetInt("免验证码检测渔场等级") &&
            //                 CPlayerMgr.Ins.pOwner.uid == 559642);
        }
        //pTickerUpdateBroadContent.FillTime();
    }

    public void InitInfo()
    {
        nCurShowIdx = 0;
        listRollInfos.Clear();

        uiLabelBaodi.text = $"保底为{CFishFesInfoMgr.Ins.nGiftGachaBoxBoodiCount}次";
        uiLabelBroadCast.text = CFishFesInfoMgr.Ins.szBroadContent;

        //List<ST_TreasureInfo> unitTreasureInfos = CTBLHandlerTreasureInfo.Ins.GetInfos();
        for (int i = 0; i < CFishFesInfoMgr.Ins.listGachaGiftInfos.Count; i++)
        {
            listRollInfos.Add(CFishFesInfoMgr.Ins.listGachaGiftInfos[i]);
        }
        ShowSlot();
    }

    public void ShowSlot()
    {
        if (nCurShowIdx < 0 || nCurShowIdx >= listRollInfos.Count) return;

        pRollSlot.SetInfo(listRollInfos[nCurShowIdx]);
        nCurShowIdx++;
        if (nCurShowIdx >= listRollInfos.Count)
        {
            nCurShowIdx = 0;
        }
        uiTweenSlotPos.enabled = true;
        uiTweenSlotPos.from = vecStartPos;
        uiTweenSlotPos.to = vecShowPos;
        uiTweenSlotPos.Play(delegate ()
        {
            pStayTick = new CPropertyTimer();
            pStayTick.Value = fStayTime;
            pStayTick.FillTime();
        });
    }

    public void HideSlot()
    {
        uiTweenSlotPos.enabled = true;
        uiTweenSlotPos.from = vecShowPos;
        uiTweenSlotPos.to = vecHidePos;
        uiTweenSlotPos.Play(delegate ()
        {
            ShowSlot();
        });
    }

    private void Update()
    {
        if (pStayTick != null &&
           pStayTick.Tick(CTimeMgr.DeltaTime))
        {
            pStayTick = null;
            HideSlot();
        }

        //if(pTickerUpdateBroadContent!= null &&
        //   pTickerUpdateBroadContent.Tick(CTimeMgr.DeltaTime))
        //{
        //    CHttpMgr.Instance.SendHttpMsg(CHttpConst.GetBroadContent);

        //    pTickerUpdateBroadContent.FillTime();
        //}
    }

    public void OnClickFes()
    {
        UIManager.Instance.OpenUI(UIResType.ShowGachaGiftFes);
    }

    public void OnClickRefreshYZM()
    {
        CGameColorFishMgr.Ins.pMap.RefreshYZMAnswer(true);
    }

}
