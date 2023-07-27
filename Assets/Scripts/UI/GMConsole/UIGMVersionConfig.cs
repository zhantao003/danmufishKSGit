using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGMVersionConfig : MonoBehaviour
{
    public InputField uiInputSetVersion;
    public InputField uiInputSetVersionTip;
    public InputField uiInputSetBroadCast;

    public void OnClickSetVersion()
    {
        RefreshUrl();

        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("version", uiInputSetVersion.text)
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugSetVersion, new HGMHandlerSendBait(), pReqParams);
    }

    public void OnClickSetVersionTip()
    {
        RefreshUrl();

        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("version", uiInputSetVersionTip.text)
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugSetVersionTip, new HGMHandlerSendBait(), pReqParams);
    }

    public void OnClickBroadCast()
    {
        RefreshUrl();

        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("version", uiInputSetBroadCast.text)
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugSetBroadContent, new HGMHandlerSendBait(), pReqParams);
    }

    public void RefreshUrl()
    {
        UIGMConsole uiConsole = FindObjectOfType<UIGMConsole>();
        CHttpMgr.Instance.szUrl = uiConsole.uiInputUrl.text;
        CHttpMgr.Instance.nPort = int.Parse(uiConsole.uiInputPort.text);
    }
}
