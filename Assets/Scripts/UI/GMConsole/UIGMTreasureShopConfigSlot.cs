using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGMTreasureShopConfigSlot : MonoBehaviour
{
    public int nItemId;
    public int nItemType;
    public InputField uiInputID;
    public InputField uiInputType;
    public InputField uiInputPrice;

    public Text uiLabelName;
    public UIRawIconLoad uiIcon;
    public Image uiRare;
    public Image uiBG;
    public Sprite[] arrBGRare;
    public Sprite[] arrTipRare;

    public GameObject objRootIcon;
    public GameObject objRootError;

    public CGMTreasuresShopBaseInfo pInfo;

    void Start()
    {
        uiInputID.onValueChanged.AddListener(OnInputIDChg);
        uiInputType.onValueChanged.AddListener(OnInputTypeChg);
    }

    public void Init(CGMTreasuresShopBaseInfo info)
    {
        pInfo = info;
        nItemId = pInfo.nItemId;
        nItemType = pInfo.nItemType;
        uiInputID.text = info.nItemId.ToString();
        uiInputType.text = info.nItemType.ToString();
        uiInputPrice.text = info.nPrice.ToString();

        SetTreasureInfo(info.nItemId, pInfo.nItemType);
    }

    public void SetTreasureInfo(int tbid, int itemType)
    {
        if ((ST_ShopTreasure.EMItemType)itemType == ST_ShopTreasure.EMItemType.Boat)
        {
            ST_UnitFishBoat pTBLInfo = CTBLHandlerUnitFishBoat.Ins.GetInfo(tbid);
            if(pTBLInfo == null)
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
            if (nRareIdx > 0 && nRareIdx <= arrBGRare.Length)
            {
                uiBG.sprite = arrBGRare[nRareIdx - 1];
            }

            if (nRareIdx > 0 && nRareIdx <= arrTipRare.Length)
            {
                uiRare.sprite = arrTipRare[nRareIdx - 1];
            }
        }
        else if((ST_ShopTreasure.EMItemType)itemType == ST_ShopTreasure.EMItemType.Role)
        {
            ST_UnitAvatar pTBLInfo = CTBLHandlerUnitAvatar.Ins.GetInfo(tbid);
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
            if (nRareIdx > 0 && nRareIdx <= arrBGRare.Length)
            {
                uiBG.sprite = arrBGRare[nRareIdx - 1];
            }

            if (nRareIdx > 0 && nRareIdx <= arrTipRare.Length)
            {
                uiRare.sprite = arrTipRare[nRareIdx - 1];
            }
        }
    }

    void OnInputIDChg(string value)
    {
        int nID = int.Parse(value);
        int nItemType = int.Parse(uiInputType.text);
        SetTreasureInfo(nID,nItemType);
    }

    void OnInputTypeChg(string value)
    {
        int nID = int.Parse(uiInputID.text);
        int nItemType = int.Parse(value);
        SetTreasureInfo(nID, nItemType);
    }

    public void OnClickDel()
    {
        CHttpParam pReqParams = new CHttpParam(
             new CHttpParamSlot("itemId", uiInputID.text),
             new CHttpParamSlot("itemType", uiInputType.text)
         );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.DebugRemoveTreasureShopInfo, pReqParams);
    }

    public CLocalNetMsg ToMsg()
    {
        CLocalNetMsg msgSlot = new CLocalNetMsg();

        //ST_ShopTreasure pTBLShopInfo = CTBLHandlerTreasureShop.Ins.GetInfo(pInfo.nId);
        //if (pTBLShopInfo == null) return null;
        msgSlot.SetLong("shopId", CHelpTools.GenerateId());
        msgSlot.SetLong("price", long.Parse(uiInputPrice.text) );
        msgSlot.SetInt("itemType", int.Parse(uiInputType.text));
        msgSlot.SetLong("contentId", long.Parse(uiInputID.text));

        return msgSlot;
    }
}
