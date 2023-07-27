using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGMBoatConfigSlot : MonoBehaviour
{
    public int nBoatId;
    public InputField uiInputID;
    public InputField uiInputItemId;
    public InputField uiInputItemNum;
    public Text uiLabelName;
    public UIRawIconLoad uiIcon;
    public Image uiRare;
    public Image uiBG;
    public Sprite[] arrBGRare;
    public Sprite[] arrTipRare;

    public CGMFishBoatInfo pTargetInfo;

    void Start()
    {
        uiInputID.onValueChanged.AddListener(OnInputIDChg);
    }

    public void Init(CGMFishBoatInfo info, float priceLerp)
    {
        pTargetInfo = info;
        uiInputID.text = info.boatId.ToString();
        uiInputItemId.text = info.itemId.ToString();
        uiInputItemNum.text = (System.Convert.ToInt32(info.itemNum*priceLerp)).ToString();
        SetBoatInfo(info.boatId);
    }

    void SetBoatInfo(int tbid)
    {
        nBoatId = tbid;
        ST_UnitFishBoat pTBLInfo = CTBLHandlerUnitFishBoat.Ins.GetInfo(tbid);
        if (pTBLInfo == null) return;
        uiLabelName.text = pTBLInfo.szName;

        if(!CHelpTools.IsStringEmptyOrNone(pTBLInfo.szIcon))
        {
            uiIcon.SetIconSync(pTBLInfo.szIcon);
        }

        int nRareIdx = (int)pTBLInfo.emRare - 1;
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
        SetBoatInfo(nID);
    }

    public void OnClickDel()
    {
        CHttpParam pReqParam = new CHttpParam(
           new CHttpParamSlot("boatId", nBoatId.ToString())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugDelFishBoatInfo, pReqParam);
    }

    public CLocalNetMsg ToMsg()
    {
        CLocalNetMsg msgSlot = new CLocalNetMsg();
        msgSlot.SetInt("boatId", int.Parse(uiInputID.text));
        msgSlot.SetLong("price", long.Parse(uiInputItemNum.text));
        msgSlot.SetLong("itemId", long.Parse(uiInputItemId.text));

        return msgSlot;
    }
}
