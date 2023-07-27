using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClickShowGongGao : MonoBehaviour
{
    public int nFesPack;

    private void Start()
    {
        bool bShow = CFishFesInfoMgr.Ins.IsFesOn(nFesPack);
        if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss ||
           CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive)
        {
            bShow = false;
        }

        gameObject.SetActive(bShow);
    }


    public void ShowGongGao()
    {
        UIManager.Instance.OpenUI(UIResType.ShowImg);
    }
}
