using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShowGiftSlot : MonoBehaviour
{
    public GameObject objSelf;

    public Text uiCount;

    public void SetActive(bool bActive)
    {
        objSelf.SetActive(bActive);
    }

    public void SetCount(long count)
    {
        long nlSuoXieCount = count / 1000;
        if (nlSuoXieCount >= 10)
        {
            uiCount.text =  nlSuoXieCount.ToString() + $"<color=#{CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("GiftBigCount")}>K</color>";
            //uiCount.color = CGameColorFishMgr.Ins.pStaticConfig.GetColor("GiftBigCount");
        }
        else
        {
            uiCount.text = count.ToString();
            uiCount.color = Color.white;
        }
        if(count <= 0)
        {
            SetActive(false);
        }
    }

}
