using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleProfitSlot : MonoBehaviour
{
    public GameObject objSelf;
    public RawImage uiPlayerIcon;
    public Text uiPlayerName;
    public Text uiFishPrice;
    public Text uiChampionCount;
    public UIFreeDrawRoot freeDrawRoot;

    public void InitInfo(int nGetChampion)
    {
        List<ProfitRankInfo> listShowInfos = CProfitRankMgr.Ins.GetRankInfos();
        if (listShowInfos.Count <= 0)
        {
            objSelf.SetActive(false);
            return;
        }
        objSelf.SetActive(true);
        ProfitRankInfo profitRankInfo = listShowInfos[0];

        uiChampionCount.text = "+" + nGetChampion;

        CAysncImageDownload.Ins.setAsyncImage(profitRankInfo.szHeadIcon, uiPlayerIcon);
        uiPlayerName.text = profitRankInfo.szUserName;
        uiFishPrice.text = CHelpTools.GetGoldSZ(profitRankInfo.nlTotalCoin);

        freeDrawRoot.Hide();

        CHttpParam pReqParams = new CHttpParam(
                new CHttpParamSlot("uid", profitRankInfo.nlUserUID.ToString()),
                new CHttpParamSlot("modelId", "1"),
                new CHttpParamSlot("gachaCount", "1")
            );
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.BuyGiftGachaBox, new HHandlerShowFreeDraw(EMFreeDrawType.Profit), pReqParams, 0, true);
    }

    public void SetActive(bool bActive)
    {
        if (objSelf == null) return;
        objSelf.SetActive(bActive);
    }
}
