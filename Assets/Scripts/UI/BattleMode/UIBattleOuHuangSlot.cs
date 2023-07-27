using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleOuHuangSlot : MonoBehaviour
{
    public GameObject objSelf;
    public RawImage uiPlayerIcon;
    public UIFishIconLoad iconLoad;
    public Text uiPlayerName;
    public Text uiFishName;
    public Text uiFishSize;
    public Text uiFishPrice;
    public Text uiChampionCount;
    public GameObject objBianYiTIip;

    public UIFreeDrawRoot freeDrawRoot;

    public void InitInfo(int nGetChampion)
    {
        List<OuHuangRankInfo> listShowInfos = COuHuangRankMgr.Ins.GetRankInfos();
        if (listShowInfos.Count <= 0)
        {
            objSelf.SetActive(false);
            return;
        }

        objSelf.SetActive(true);
        OuHuangRankInfo ouHuangInfo = listShowInfos[0];

        uiChampionCount.text = "+" + nGetChampion;

        CAysncImageDownload.Ins.setAsyncImage(ouHuangInfo.szHeadIcon, uiPlayerIcon);
        uiPlayerName.text = ouHuangInfo.szUserName;
        string szName = ouHuangInfo.szFishName.Replace("[±äÒì]", "");
        if (uiFishName != null)
        {
            if (ouHuangInfo.bBianYi)
            {
                if (ouHuangInfo.emFishRare == EMRare.YouXiu)
                {
                    szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishR"), CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishUR"));
                }
                else if (ouHuangInfo.emFishRare == EMRare.XiYou)
                {
                    szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishSR"), CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishUR"));
                }
                else if (ouHuangInfo.emFishRare == EMRare.Special)
                {
                    szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishSSR"), CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishUR"));
                }
                else if (ouHuangInfo.emFishRare == EMRare.Shisi)
                {
                    szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishUR"), CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishXR"));
                }
                else if (ouHuangInfo.emFishRare == EMRare.Normal)
                {
                    szName = "<color=#" + CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishUR") + ">" + szName + "</color>";
                }
            }
            uiFishName.text = szName;
        }
        if (objBianYiTIip != null)
            objBianYiTIip.SetActive(ouHuangInfo.bBianYi);
        //uiFishName.text = ouHuangInfo.szFishName;
        uiFishSize.text = ouHuangInfo.fFishSize.ToString("f2") + "cm";
        uiFishPrice.text = ((int)ouHuangInfo.nlFishPrice).ToString();
        if (iconLoad != null)
        {
            iconLoad.IconLoad(ouHuangInfo.szFishIcon);
        }

        freeDrawRoot.Hide();
        CHttpParam pReqParams = new CHttpParam(
                new CHttpParamSlot("uid", ouHuangInfo.nlUserUID.ToString()),
                new CHttpParamSlot("modelId", "1"),
                new CHttpParamSlot("gachaCount", "1")
            );
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.BuyGiftGachaBox, new HHandlerShowFreeDraw(EMFreeDrawType.OuHuang), pReqParams, 0, true);

    }

    public void SetActive(bool bActive)
    {
        if (objSelf == null) return;
        objSelf.SetActive(bActive);
    }

}
