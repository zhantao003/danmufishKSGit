using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattlePlayerInfo : MonoBehaviour
{
    public GameObject objSelf;
    [Header("玩家名字")]
    public Text uiName;
    [Header("类型")]
    public Text uiType;
    [Header("总价")]
    public Text uiPrice;

    public GameObject objBianYiTIip;

    public void Init(SpecialCastInfo castInfo, bool bSetColor = true)
    {
        if (castInfo.playerInfo == null) return;
        if (uiName != null)
            uiName.text = castInfo.playerInfo.userName;

        if (castInfo.fishInfo == null)
        {
            uiType.text = "?";
            if (uiPrice != null)
                uiPrice.text = "?";
            if (objBianYiTIip != null)
                objBianYiTIip.SetActive(false);
            return;
        }

        string szName = castInfo.fishInfo.szName.Replace("[变异]", "");
        if (uiType != null)
        {
            if (castInfo.fishInfo.bBianYi)
            {
                if (castInfo.fishInfo.emRare == EMRare.YouXiu)
                {
                    szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishR"), bSetColor ? CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishUR") : "000000");
                }
                else if (castInfo.fishInfo.emRare == EMRare.XiYou)
                {
                    szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishSR"), bSetColor ? CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishUR") : "000000");
                }
                else if (castInfo.fishInfo.emRare == EMRare.Special)
                {
                    szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishSSR"), bSetColor ? CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishUR") : "000000");
                }
                else if (castInfo.fishInfo.emRare == EMRare.Shisi)
                {
                    szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishUR"), bSetColor ? CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishXR") : "000000");
                }
                else if (castInfo.fishInfo.emRare == EMRare.Normal)
                {
                    if (bSetColor)
                    {
                        szName = "<color=#" + CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishUR") + ">" + szName + "</color>";
                    }
                    else
                    {

                    }
                }
            }
            else if (!bSetColor)
            {
                if (castInfo.fishInfo.emRare == EMRare.YouXiu)
                {
                    szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishR"), "000000");
                }
                else if (castInfo.fishInfo.emRare == EMRare.XiYou)
                {
                    szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishSR"), "000000");
                }
                else if (castInfo.fishInfo.emRare == EMRare.Special)
                {
                    szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishSSR"), "000000");
                }
                else if (castInfo.fishInfo.emRare == EMRare.Shisi)
                {
                    szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishUR"), "000000");
                }
                else if (castInfo.fishInfo.emRare == EMRare.Normal)
                {

                }
            }
            uiType.text = szName;
        }
        if (uiPrice != null)
            uiPrice.text = ((int)castInfo.fishInfo.lPrice).ToString();
        if (objBianYiTIip != null)
            objBianYiTIip.SetActive(castInfo.fishInfo.bBianYi);
    }


    public void SetActive(bool bActive)
    {
        objSelf.SetActive(bActive);
    }

}
