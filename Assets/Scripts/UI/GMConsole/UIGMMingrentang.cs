using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGMMingrentang : MonoBehaviour
{
    public InputField uiInputSeason;

    public InputField uiInputOuUID1;
    public InputField uiInputOuName1;
    public InputField uiInputOuHeadIcon1;

    public InputField uiInputOuUID2;
    public InputField uiInputOuName2;
    public InputField uiInputOuHeadIcon2;

    public InputField uiInputOuUID3;
    public InputField uiInputOuName3;
    public InputField uiInputOuHeadIcon3;

    public InputField uiInputRichUID1;
    public InputField uiInputRichName1;
    public InputField uiInputRichHeadIcon1;

    public InputField uiInputRichUID2;
    public InputField uiInputRichName2;
    public InputField uiInputRichHeadIcon2;

    public InputField uiInputRichUID3;
    public InputField uiInputRichName3;
    public InputField uiInputRichHeadIcon3;

    public void OnClickClose()
    {
        gameObject.SetActive(false);
    }

    public void OnClickSet()
    {
        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("seasonId", uiInputSeason.text),

            new CHttpParamSlot("ouuid1", uiInputOuUID1.text),
            new CHttpParamSlot("ouname1", System.Net.WebUtility.UrlEncode(uiInputOuName1.text)),
            new CHttpParamSlot("ouheadIcon1", System.Net.WebUtility.UrlEncode(uiInputOuHeadIcon1.text)),

            new CHttpParamSlot("ouuid2", uiInputOuUID2.text),
            new CHttpParamSlot("ouname2", System.Net.WebUtility.UrlEncode(uiInputOuName2.text)),
            new CHttpParamSlot("ouheadIcon2", System.Net.WebUtility.UrlEncode(uiInputOuHeadIcon2.text)),

            new CHttpParamSlot("ouuid3", uiInputOuUID3.text),
            new CHttpParamSlot("ouname3", System.Net.WebUtility.UrlEncode(uiInputOuName3.text)),
            new CHttpParamSlot("ouheadIcon3", System.Net.WebUtility.UrlEncode(uiInputOuHeadIcon3.text)),

            new CHttpParamSlot("richuid1", uiInputRichUID1.text),
            new CHttpParamSlot("richname1", System.Net.WebUtility.UrlEncode(uiInputRichName1.text)),
            new CHttpParamSlot("richheadIcon1", System.Net.WebUtility.UrlEncode(uiInputRichHeadIcon1.text)),

            new CHttpParamSlot("richuid2", uiInputRichUID2.text),
            new CHttpParamSlot("richname2", System.Net.WebUtility.UrlEncode(uiInputRichName2.text)),
            new CHttpParamSlot("richheadIcon2", System.Net.WebUtility.UrlEncode(uiInputRichHeadIcon2.text)),

            new CHttpParamSlot("richuid3", uiInputRichUID3.text),
            new CHttpParamSlot("richname3", System.Net.WebUtility.UrlEncode(uiInputRichName3.text)),
            new CHttpParamSlot("richheadIcon3", System.Net.WebUtility.UrlEncode(uiInputRichHeadIcon3.text))
            );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst_Debug.DEBUG_SetMingrentangInfo, new HGMHandlerChaxunCard(), pReqParams);
    }
}
