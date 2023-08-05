using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGMGiftGachaBoxConfigSlot : MonoBehaviour
{
    public int nShopId;

    public InputField uiInputShopId;
    public InputField uiInputItemType;
    public InputField uiInputItemId;
    public InputField uiInputWeight;
    public Toggle uiToggleRare;

    public GameObject objShopYuju;
    public GameObject objShopFeilun;
    public GameObject objYubiBoard;
    public GameObject objShopAvatar;
    public GameObject objError;

    public Text uiLabelName;
    public UIRawIconLoad uiIcon;
    public Image uiRare;
    public Image uiBG;
    public Sprite[] arrBGRare;
    public Sprite[] arrTipRare;

    public void Init(CGMGiftGachaBoxConfigInfo info)
    {
        nShopId = info.nShopId;

        uiInputShopId.text = info.nShopId.ToString();
        uiInputItemType.text = info.nItemType.ToString();
        uiInputItemId.text = info.nItemId.ToString();
        uiInputWeight.text = info.nWeight.ToString();
        uiToggleRare.isOn = (info.nIsRare > 0);

        uiInputItemType.onValueChanged.AddListener(OnInputFieldChg);
        uiInputItemId.onValueChanged.AddListener(OnInputFieldChg);

        RefreshInfo(long.Parse(uiInputItemId.text), int.Parse(uiInputItemType.text));
    }

    void RefreshInfo(long itemId, int itemType)
    {
        if (itemType == 0)         //积分
        {
            objShopYuju.SetActive(false);
            objShopFeilun.SetActive(false);
            objYubiBoard.SetActive(true);
            objShopAvatar.SetActive(false);
            objError.SetActive(false);

            uiLabelName.text = $"{itemId}积分";
        }
        else if (itemType == 1)    //渔具
        {
            objShopYuju.SetActive(true);
            objShopFeilun.SetActive(false);
            objYubiBoard.SetActive(false);
            objShopAvatar.SetActive(false);
            objError.SetActive(false);

            uiLabelName.text = $"{itemId}套互动手柄";
        }
        else if (itemType == 2)    //飞轮
        {
            objShopYuju.SetActive(false);
            objShopFeilun.SetActive(true);
            objYubiBoard.SetActive(false);
            objShopAvatar.SetActive(false);
            objError.SetActive(false);

            uiLabelName.text = $"{itemId}个初级卷轴";
        }
        else if (itemType == 3)    //角色
        {
            objShopYuju.SetActive(false);
            objShopFeilun.SetActive(false);
            objYubiBoard.SetActive(false);
            objShopAvatar.SetActive(true);
            objError.SetActive(false);

            ST_UnitAvatar pTBLInfo = CTBLHandlerUnitAvatar.Ins.GetInfo((int)itemId);
            if (pTBLInfo == null)
            {
                objShopAvatar.SetActive(false);
                objError.SetActive(true);
            }
            else
            {
                objShopAvatar.SetActive(true);
                objError.SetActive(false);

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
        }
        else if (itemType == 4)    //船
        {
            objShopYuju.SetActive(false);
            objShopFeilun.SetActive(false);
            objYubiBoard.SetActive(false);
            objShopAvatar.SetActive(true);
            objError.SetActive(false);

            ST_UnitFishBoat pTBLInfo = CTBLHandlerUnitFishBoat.Ins.GetInfo((int)itemId);
            if (pTBLInfo == null)
            {
                objShopAvatar.SetActive(false);
                objError.SetActive(true);
            }
            else
            {
                objShopAvatar.SetActive(true);
                objError.SetActive(false);

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
        }
        else if(itemType == 5)  //钓竿
        {
            objShopYuju.SetActive(false);
            objShopFeilun.SetActive(false);
            objYubiBoard.SetActive(false);
            objShopAvatar.SetActive(true);
            objError.SetActive(false);

            ST_UnitFishGan pTBLInfo = CTBLHandlerUnitFishGan.Ins.GetInfo((int)itemId);
            if (pTBLInfo == null)
            {
                objShopAvatar.SetActive(false);
                objError.SetActive(true);
            }
            else
            {
                objShopAvatar.SetActive(true);
                objError.SetActive(false);

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
        }
    }

    void OnInputFieldChg(string value)
    {
        RefreshInfo(long.Parse(uiInputItemId.text), int.Parse(uiInputItemType.text));
    }

    public void OnClickDel()
    {
        UIGMGiftGachaBoxConfig uiConfig = FindObjectOfType<UIGMGiftGachaBoxConfig>();
        if (uiConfig == null) return;

        uiConfig.DelSlot(nShopId);
    }

    public CLocalNetMsg ToJsonMsg()
    {
        CLocalNetMsg msgRes = new CLocalNetMsg();
        msgRes.SetInt("shopId", int.Parse(uiInputShopId.text));
        msgRes.SetInt("itemType", int.Parse(uiInputItemType.text));
        msgRes.SetLong("itemId", long.Parse(uiInputItemId.text));
        msgRes.SetLong("weight", long.Parse(uiInputWeight.text));
        msgRes.SetInt("rare", uiToggleRare.isOn ? 1 : 0);

        return msgRes;
    }
}
