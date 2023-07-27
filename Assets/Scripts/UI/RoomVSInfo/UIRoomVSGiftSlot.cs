using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomVSGiftSlot : MonoBehaviour
{
    public int nFishID;
    public int nNum;

    public Text uiName;
    //public Text uiCount;
    public UIFishIconLoad iconLoad;
    public Image pRareBG;
    public Sprite[] pRareBGSprite;

    public void Init(int fishId, int num)
    {
        nFishID = fishId;

        ST_FishInfo fishInfo = CGameColorFishMgr.Ins.pMap.pTBLHandlerBoomFishInfo.GetInfo(fishId);

        string szName = fishInfo.szName;
        //if (fishInfo.emRare == EMRare.YouXiu)
        //{
        //    szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishR"), "000000");
        //}
        //else if (fishInfo.emRare == EMRare.XiYou)
        //{
        //    szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishSR"), "000000");
        //}
        //else if (fishInfo.emRare == EMRare.Special)
        //{
        //    szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishSSR"), "000000");
        //}
        //else if (fishInfo.emRare == EMRare.Shisi)
        //{
        //    szName = szName.Replace(CGameColorFishMgr.Ins.pStaticConfig.GetColorHex("fishUR"), "000000");
        //}
        
        uiName.text = szName;

        if (iconLoad != null)
        {
            iconLoad.IconLoad(fishInfo.szIcon);
        }

        if (pRareBG != null)
        {
            pRareBG.sprite = pRareBGSprite[(int)fishInfo.emRare];
        }

        //SetCount(num);
    }

    //public void SetCount(int value)
    //{
    //    //uiCount.text = "Ê£" + value.ToString() +"Ìõ";
    //    nNum = value;
    //    uiCount.text = value.ToString();
    //}
}
