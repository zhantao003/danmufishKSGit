using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICrazyTime : UIBase
{
    public UITweenPos tweenPos;

    public Vector3 vecHidePos;
    public Vector3 vecShowPos;

    public Vector3 vShowPosByBattle;
    public Vector3 vShowPosByAuction;

    public Text uiCrazyCountDown;

    public GameObject objKongTou;

    bool bShow;

    public void ActiveKongTou(bool bActive)
    {
        if (objKongTou == null) return;

        objKongTou.SetActive(bActive);
    }

    public void ShowPosByBattle(float fDelay)
    {
        if (!bShow) return;
        tweenPos.from = tweenPos.tranTarget.localPosition;
        tweenPos.to = vShowPosByBattle;
        tweenPos.delayTime = fDelay;
        tweenPos.Play();
    }

    public void ShowPosByAuction(float fDelay)
    {
        if (!bShow) return;
        tweenPos.from = tweenPos.tranTarget.localPosition;
        tweenPos.to = vShowPosByAuction;
        tweenPos.delayTime = fDelay;
        tweenPos.Play();
    }

    public void BackOriPos(float fDelay)
    {
        if (!bShow) return;
        tweenPos.from = tweenPos.tranTarget.localPosition;
        tweenPos.to = vecShowPos;
        tweenPos.delayTime = fDelay;
        tweenPos.Play();
    }

    public override void OnOpen()
    {
        base.OnOpen(); 
        tweenPos.enabled = false;
        tweenPos.transform.position = vecHidePos;
        objKongTou.SetActive(false);
    }

    /// <summary>
    /// 展示面板
    /// </summary>
    public static void ShowCrazyTime()
    {
        UICrazyTime crazyTime = UIManager.Instance.GetUI(UIResType.CrazyTime) as UICrazyTime;
        if(crazyTime != null)
        {
            crazyTime.SetBoard(true, 0.5f);
        }
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    public static void HideCrazyTime()
    {
        UICrazyTime crazyTime = UIManager.Instance.GetUI(UIResType.CrazyTime) as UICrazyTime;
        if (crazyTime != null)
        {
            crazyTime.SetBoard(false);
        }
    }

    public static void ResetPos()
    {
        UICrazyTime crazyTime = UIManager.Instance.GetUI(UIResType.CrazyTime) as UICrazyTime;
        if (crazyTime != null)
        {
            crazyTime.tweenPos.enabled = false;
            crazyTime.tweenPos.transform.position = crazyTime.vecHidePos;
        }
    }



    public void SetBoard(bool show,float fDelay = 0)
    {
        if (tweenPos == null)
            return;
        tweenPos.enabled = true;
        bShow = show;
        if (show)
        {
            tweenPos.from = vecHidePos;
            if (CGameColorFishMgr.Ins.pMap.pDuelBoat.emCurState == CDuelBoat.EMState.Show ||
                CGameColorFishMgr.Ins.pMap.pDuelBoat.emCurState == CDuelBoat.EMState.Stay)
            {
                tweenPos.to = vShowPosByBattle;
            }
            else if (CAuctionMgr.Ins.emAuctionState != CAuctionMgr.EMAuctionState.Normal)
            {
                tweenPos.to = vShowPosByAuction;
            }
            else
            {
                tweenPos.to = vecShowPos;
            }
        }
        else
        {
            tweenPos.from = vecShowPos;
            tweenPos.to = vecHidePos;
        }
        tweenPos.delayTime = fDelay;
        tweenPos.Play();
    }

    private void Update()
    {
        if (bShow)
        {
            uiCrazyCountDown.text = CCrazyTimeMgr.Ins.GetCrazyTime().ToString("f0") + "s";
        }
    }


}
