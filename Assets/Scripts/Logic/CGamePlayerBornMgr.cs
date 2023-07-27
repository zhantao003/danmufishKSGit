using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGamePlayerBornMgr : MonoBehaviour
{
    public static CGamePlayerBornMgr Ins = null;

    public Transform tranEffRoot;
    bool bActive = false;

    public List<CPlayerBaseInfo> listBornInfo = new List<CPlayerBaseInfo>();

    void Start()
    {
        Ins = this;
    }

    public void CreateBornEff(CPlayerBaseInfo player)
    {
        if(bActive)
        {
            listBornInfo.Add(player);
            return;
        }

        //≈–∂œ «∑Ò”–»Î≥°Ãÿ–ß
        string szEffBornRoot = "";
        string szContentZYM = "¿¥√˛”„¿≤!";
        if (player.GetVipLv() == CPlayerBaseInfo.EMVipLv.Zongdu)
        {
            szEffBornRoot = "Effect/BornEffect/effBorn001";
            szContentZYM = "Ω≈Ã§∆ﬂ≤ œÈ‘∆µ«≥°£°";
        }
        else if(player.GetVipLv() == CPlayerBaseInfo.EMVipLv.Pro)
        {
            szEffBornRoot = "Effect/BornEffect/effBorn003";
            szContentZYM = "…¡¡¡µ«≥°£°";
        }
        else if (player.GetVipLv() == CPlayerBaseInfo.EMVipLv.Tidu)
        {
            szEffBornRoot = "Effect/BornEffect/effBorn002";
        }
        else
        {
            return;
        }

        if (CHelpTools.IsStringEmptyOrNone(szEffBornRoot)) return;
        if (tranEffRoot == null) return;

        bActive = true;

        CEffectMgr.Instance.CreateEffSync(szEffBornRoot, tranEffRoot, 0, true,
        delegate (GameObject value)
        {
            CEffectBornPlayer pEffBorn = value.GetComponent<CEffectBornPlayer>();
            if (pEffBorn != null)
            {
                pEffBorn.szContentZYM = szContentZYM;
                pEffBorn.SetPlayerInfo(player);

                pEffBorn.callRecycleEvent = PopEffect;
            }
        });
    }

    void PopEffect()
    {
        bActive = false;
        if (listBornInfo.Count <= 0) return;

        CreateBornEff(listBornInfo[0]);
        listBornInfo.RemoveAt(0);
    }
}
