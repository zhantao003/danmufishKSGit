using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGMConsoleCard : MonoBehaviour
{
    public InputField uiInputIP;
    public InputField uiInputPort;

    public Text uiLabelResContent;

    public InputField uiInputCardNum;
    public InputField uiInputCard;

    public InputField uiInputRankExtime;

    public GameObject objMingrentang;

    public void OnClickCreateCard()
    {
        RefreshURL();

        int nNum = int.Parse(uiInputCardNum.text);
        Debug.Log("生成卡密" + nNum + "个");

        CLocalNetArrayMsg arrMsgCrad = new CLocalNetArrayMsg();
        for (int i = 0; i < nNum; i++)
        {
            CLocalNetMsg msgCard = new CLocalNetMsg();
            msgCard.SetInt("idx", i);
            msgCard.SetString("card", CHelpTools.GetRandomString(16, true, false, true, false, ""));

            arrMsgCrad.AddMsg(msgCard);
        }

        CLocalNetMsg msgReq = new CLocalNetMsg();
        msgReq.SetNetMsgArr("cardList", arrMsgCrad);

        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("cardList", msgReq.GetData())
            );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst_Debug.DEBUG_AddCard, pReqParams);
    }

    public void OnClickChaxunCard()
    {
        RefreshURL();

        CHttpParam pReqParams = new CHttpParam(
          new CHttpParamSlot("card", uiInputCard.text)
          );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst_Debug.DEBUG_GetCardStatus, pReqParams);
    }

    public void OnClickChaxunNum()
    {
        RefreshURL();

        CHttpMgr.Instance.SendHttpMsg(CHttpConst_Debug.DEBUG_GetActiveCard, new HGMHandlerChaxunCard());
    }

    public void OnClickCardBan()
    {
        RefreshURL();

        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("card", uiInputCard.text),
            new CHttpParamSlot("status", "2")
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst_Debug.DEBUG_SetCardStatus, new HGMHandlerChaxunCard(), pReqParams);
    }

    public void OnClickCardDeBan()
    {
        RefreshURL();

        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("card", uiInputCard.text),
            new CHttpParamSlot("status", "1") 
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst_Debug.DEBUG_SetCardStatus, new HGMHandlerChaxunCard(), pReqParams);
    }

    public void OnClickGetRankExtime()
    {
        RefreshURL();

        CHttpMgr.Instance.SendHttpMsg(CHttpConst_Debug.DEBUG_GetRankWinnerExtime, new HGMHandlerSendAvatar());
    }

    public void OnClickSetRankExtime()
    {
        RefreshURL();

        long nNewTime = long.Parse(uiInputRankExtime.text);
        CHttpParam pReqParams = new CHttpParam(
          new CHttpParamSlot("expireTime", nNewTime.ToString())
          );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst_Debug.DEBUG_SetRankRicherExtime, new HGMHandlerSendAvatar(), pReqParams);
    }

    public void OnClickResultRank() 
    {
        RefreshURL();

        CHttpMgr.Instance.SendHttpMsg(CHttpConst_Debug.DEBUG_ResultWinnerRankExtime, new HGMHandlerSendAvatar());
    }

    /// <summary>
    /// 导出最新排行榜
    /// </summary>
    public void OnClickGetLatestRankList()
    {
        CHttpMgr.Instance.SendHttpMsg(CHttpConst_Debug.DEBUG_GetLastestRankRicher, new HGMHandlerGetLatestWinnerRank());
    }

    /// <summary>
    /// 导出最新排行榜
    /// </summary>
    public void OnClickGetLatestNewRankList()
    {
        CHttpMgr.Instance.SendHttpMsg(CHttpConst_Debug.DEBUG_GetLastestRankSlotRicher, new HGMHandlerGetLatestWinnerRankSlot());
    }

    public void OnClickGetGiftPrice()
    {
        RefreshURL();

        CHttpMgr.Instance.SendHttpMsg(CHttpConst_Debug.DEBUG_GetFishGiftPrice, new HGMHandlerSendAvatar());
    }

    public void OnClickResetGiftPrice()
    {
        RefreshURL();

        CHttpMgr.Instance.SendHttpMsg(CHttpConst_Debug.DEBUG_ResetFishGiftPrice, new HGMHandlerSendAvatar());
    }

    public void OnClickSetMingrentang()
    {
        objMingrentang.SetActive(true);
    }

    public static void SetResContent(string res)
    {
        UIGMConsoleCard uiGM = FindObjectOfType<UIGMConsoleCard>();
        uiGM.uiLabelResContent.text = res;
    }

    public void RefreshURL()
    {
        CNetConfigMgr.Ins.msgContent.SetString("httpserver", uiInputIP.text);
        CNetConfigMgr.Ins.msgContent.SetInt("httpport", int.Parse(uiInputPort.text));

        CHttpMgr.Instance.Init();
    }
}
