using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGMFishGanConfigSlot : MonoBehaviour
{
    public int nGanID;
    public InputField uiInputID;
    public InputField uiInputItemId;
    public InputField uiInputItemNum;
    public Text uiLabelName;
    public UIRawIconLoad uiIcon;
    public Image uiRare;
    public Image uiBG;
    public Sprite[] arrBGRare;
    public Sprite[] arrTipRare;

    public CGMFishGanInfo pTargetInfo;

    public EMAddUnitProType emProType;
    public int nProAddMin;
    public int nProAddMax;

    public CGMFishGanLvInfo[] arrLvInfo;

    void Start()
    {
        uiInputID.onValueChanged.AddListener(OnInputIDChg);
    }

    public void Init(CGMFishGanInfo info, float priceLerp)
    {
        pTargetInfo = info;
        uiInputID.text = info.ganId.ToString();
        uiInputItemId.text = info.itemId.ToString();
        uiInputItemNum.text = (System.Convert.ToInt32(info.itemNum * priceLerp)).ToString();
        SetGanInfo(info.ganId);
    }

    void SetGanInfo(int tbid)
    {
        nGanID = tbid;
        ST_UnitFishGan pTBLInfo = CTBLHandlerUnitFishGan.Ins.GetInfo(tbid);
        if (pTBLInfo == null) return;

        //初始化等级信息
        arrLvInfo = new CGMFishGanLvInfo[pTBLInfo.arrLvInfo.Length];
        for(int i=0; i<arrLvInfo.Length; i++)
        {
            arrLvInfo[i] = new CGMFishGanLvInfo();
            arrLvInfo[i].lv = pTBLInfo.arrLvInfo[i].nLv;
            arrLvInfo[i].exp = pTBLInfo.arrLvInfo[i].nExp;
        }

        //初始化属性信息
        emProType = pTBLInfo.emProType;
        nProAddMin = pTBLInfo.nProAddMin;
        nProAddMax = pTBLInfo.nProAddMax;

        uiLabelName.text = pTBLInfo.szName;

        if (!CHelpTools.IsStringEmptyOrNone(pTBLInfo.szIcon))
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
        SetGanInfo(nID);
    }

    public void OnClickDel()
    {
        CHttpParam pReqParam = new CHttpParam(
           new CHttpParamSlot("ganId", nGanID.ToString())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugRemoveFishGanInfo, pReqParam);
    }

    public CLocalNetMsg ToMsg()
    {
        CLocalNetMsg msgSlot = new CLocalNetMsg();
        msgSlot.SetInt("ganId", int.Parse(uiInputID.text));
        msgSlot.SetLong("price", long.Parse(uiInputItemNum.text));
        msgSlot.SetLong("itemId", long.Parse(uiInputItemId.text));
        msgSlot.SetInt("proType", (int)emProType);
        msgSlot.SetInt("proAddMin", nProAddMin);
        msgSlot.SetInt("proAddMax", nProAddMax);

        CLocalNetArrayMsg arrMsgLvInfo = new CLocalNetArrayMsg();
        for (int i = 0; i < arrLvInfo.Length; i++)
        {
            CLocalNetMsg msgLvInfo = new CLocalNetMsg();
            msgLvInfo.SetInt("lv", arrLvInfo[i].lv);
            msgLvInfo.SetInt("exp", arrLvInfo[i].exp);
            arrMsgLvInfo.AddMsg(msgLvInfo);
        }

        msgSlot.SetString("lvInfo", arrMsgLvInfo.GetData());

        return msgSlot;
    }
}
