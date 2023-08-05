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

        //判断是否有入场特效
        string szEffBornRoot = "";
        string szContentZYM = $"世界排名第{player.nCurRank}名";//"来摸鱼啦!";
        if (player.GetVipLv() == CPlayerBaseInfo.EMVipLv.Zongdu)
        {
            szEffBornRoot = "Effect/BornEffect/effBorn001";
            //szContentZYM = "脚踏七彩祥云登场！";
        }
        else if(player.GetVipLv() == CPlayerBaseInfo.EMVipLv.Pro)
        {
            szEffBornRoot = "Effect/BornEffect/effBorn003";
            //szContentZYM = "闪亮登场！";
        }
        else if (player.GetVipLv() == CPlayerBaseInfo.EMVipLv.Tidu)
        {
            szEffBornRoot = "Effect/BornEffect/effBorn002";
        }
        else if (player.GetVipLv() == CPlayerBaseInfo.EMVipLv.Jianzhang)
        {
            szEffBornRoot = "Effect/BornEffect/effBorn004";
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
