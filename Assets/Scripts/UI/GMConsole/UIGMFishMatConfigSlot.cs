using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGMFishMatConfigSlot : MonoBehaviour
{
    public int nAvatarId;
    public InputField uiInputID;
    public InputField uiInputDailyMax;
    public Text uiLabelName;
    public UIRawIconLoad uiIcon;
    public Image uiRare;
    public Image uiBG;
    public Sprite[] arrBGRare;
    public Sprite[] arrTipRare;

    public GameObject objRootIcon;
    public GameObject objRootError;

    public CGMFishMatInfo pInfo;

    void Start()
    {
        uiInputID.onValueChanged.AddListener(OnInputIDChg);
    }

    public void Init(CGMFishMatInfo info)
    {
        pInfo = info;
        uiInputID.text = info.nId.ToString();
        uiInputDailyMax.text = info.nDailyMax.ToString();

        SetMatInfo(pInfo.nId);
    }

    public void SetMatInfo(int tbid)
    {
        nAvatarId = tbid;

        ST_FishMat pTBLInfo = CTBLHandlerFishMaterial.Ins.GetInfo(pInfo.nId);
        if (pTBLInfo == null)
        {
            objRootIcon.SetActive(false);
            objRootError.SetActive(true);
            return;
        }
        else
        {
            objRootIcon.SetActive(true);
            objRootError.SetActive(false);
        }

        uiLabelName.text = pTBLInfo.szName;
        uiIcon.SetIconSync(pTBLInfo.szIcon);

        int nRareIdx = (int)pTBLInfo.emRare;
        if (nRareIdx >= 0 && nRareIdx < arrBGRare.Length)
        {
            uiBG.sprite = arrBGRare[nRareIdx];
        }

        if (nRareIdx >= 0 && nRareIdx < arrTipRare.Length)
        {
            uiRare.sprite = arrTipRare[nRareIdx];
        }
    }

    void OnInputIDChg(string value)
    {
        int nID = int.Parse(value);
        SetMatInfo(nID);
    }

    public void OnClickDel()
    {
        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("itemId", nAvatarId.ToString())    
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugRemoveMatInfo, new HGMHandlerDelMatInfo(), pReqParams);
    }

    public CLocalNetMsg ToMsg()
    {
        CLocalNetMsg msgSlot = new CLocalNetMsg();
        msgSlot.SetInt("itemId", int.Parse(uiInputID.text));
        msgSlot.SetLong("maxCount", long.Parse(uiInputDailyMax.text));

        return msgSlot;
    }
}
