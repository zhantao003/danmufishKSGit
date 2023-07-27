using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRankPlayerInfo : MonoBehaviour
{
    public GameObject objSelf;
    [Header("玩家名字")]
    public Text uiName;
    [Header("类型")]
    public Text uiType;
    [Header("大小")]
    public Text uiSize;
    [Header("总价")]
    public Text uiPrice;

    public GameObject objBianYiTIip;

    public GameObject objBoomTip;

    string szNameContent;
    bool bIsBoom;

    public void Init(OuHuangRankInfo rankInfo)
    {
        if(rankInfo == null)
        {
            uiName.text = "";
            uiType.text = "暂无欧皇";
            if (uiSize != null)
                uiSize.text = "";
            if (uiPrice != null)
                uiPrice.text = "";
            if (objBianYiTIip != null)
                objBianYiTIip.SetActive(false);
            return;
        }
        szNameContent = rankInfo.szUserName;
        bIsBoom = rankInfo.bBoom;

        ShowNameLabel(CGameColorFishMgr.Ins.bShowPlayerInfo);
        //if (uiName != null)
        //    uiName.text = rankInfo.szUserName;

        string szName = rankInfo.szFishName.Replace("[变异]", "");
        if (uiType != null)
        {
            if (rankInfo.bBianYi)
            {
                if (rankInfo.emFishRare == EMRare.YouXiu)
                {
                    szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishR"), CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishUR"));
                }
                else if (rankInfo.emFishRare == EMRare.XiYou)
                {
                    szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishSR"), CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishUR"));
                }
                else if (rankInfo.emFishRare == EMRare.Special)
                {
                    szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishSSR"), CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishUR"));
                }
                else if (rankInfo.emFishRare == EMRare.Shisi)
                {
                    szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishUR"), CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishXR"));
                }
                else if (rankInfo.emFishRare == EMRare.Normal)
                {
                    szName = "<color=#" + CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishUR") + ">" + szName + "</color>";
                }
            }
            uiType.text = szName;
        }
        if (uiSize != null)
            uiSize.text = rankInfo.fFishSize.ToString("f2") + "cm";
        if (uiPrice != null)
            uiPrice.text = ((int)rankInfo.nlFishPrice).ToString();
        if (objBianYiTIip != null)
            objBianYiTIip.SetActive(rankInfo.bBianYi);
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.F2))
    //    {
    //        ShowNameLabel(true);
    //    }

    //    if (Input.GetKeyUp(KeyCode.F2))
    //    {
    //        ShowNameLabel(false);
    //    }
    //}

    public void SetActive(bool bActive)
    {
        objSelf.SetActive(bActive);
    }

    public void ShowNameLabel(bool show)
    {
        if (uiName == null) return;

        if(show)
        {
            uiName.text = szNameContent; 
            if (objBoomTip != null)
                objBoomTip.SetActive(bIsBoom);
        }
        else
        {
            uiName.text = "";
            if (objBoomTip != null)
                objBoomTip.SetActive(false);
        }
    }
}
