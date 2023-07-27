using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBossBaseInfo : UIBase
{
    //Boss基本信息
    public UITweenBase uiTween;
    public Image uiImgHPVar;
    public Text uiLabelName;
    public UIRawIconLoad uiIconBoss;

    public GameObject objNormalBossTip;
    public GameObject objSpecialBossTip;

    //被吃列表
    public GameObject objPlayeEatRoot;
    public RectTransform tranPlayerEatGrid;
    List<UIBossPlayerEatSlot> listEatSlots = new List<UIBossPlayerEatSlot>();

    public GameObject[] arrBoss104AtkTip;
    public GameObject[] arrBoss204AtkTip;
    public GameObject[] arrBoss304AtkTip;

    public Transform tranBossWeakBarRoot;
    public GameObject objBossWeakBarRoot;
    List<UIRoomBossWeakHPBar> listBossWeakBars = new List<UIRoomBossWeakHPBar>();

    //伤害提示
    public RectTransform tranRootDmgTip;
    public GameObject[] arrObjDmgTip;
    protected Dictionary<EMDmgType, List<GameObject>> dicIdleDmgTips = new Dictionary<EMDmgType, List<GameObject>>();

    //倒计时
    public Text uiLabelTimeCount;
    public Text uiLabelGameStartCount;

    protected override void OnStart()
    {
        objPlayeEatRoot.SetActive(false);
        
        if (objBossWeakBarRoot != null)
        {
            objBossWeakBarRoot.SetActive(false);
        }
        for (int i=0; i<arrObjDmgTip.Length; i++)
        {
            arrObjDmgTip[i].SetActive(false);
        }
    }

   

    public override void OnOpen()
    {
        uiTween.Play();
    
        if(CGameBossMgr.Ins.pBoss!=null)
        {
            if (objNormalBossTip != null)
                objNormalBossTip.SetActive(CGameBossMgr.Ins.pBoss.emBossType == EMBossType.Normal);
            if (objSpecialBossTip != null)
                objSpecialBossTip.SetActive(CGameBossMgr.Ins.pBoss.emBossType == EMBossType.Special);

            CGameBossMgr.Ins.pBoss.dlgHPChg += this.RefreshHP;
            RefreshHP(CGameBossMgr.Ins.pBoss.nCurHp, CGameBossMgr.Ins.pBoss.nHPMax);

            uiLabelName.text = CGameBossMgr.Ins.szBossName;
            uiIconBoss.SetIconSync(CGameBossMgr.Ins.szBossIcon);
        }
    }

    protected override void OnUpdate(float dt)
    {
        if(CGameBossMgr.Ins.emCurState == CGameBossMgr.EMState.Gaming)
        {
            uiLabelTimeCount.text = CTimeMgr.SecToMMSS((long)CGameBossMgr.Ins.pTickerGame.CurValue);
        }
    }

    void RefreshHP(long cur, long max)
    {
        uiImgHPVar.fillAmount = (float)cur / (float)max;
    }

    public void AddBossWeakBar(CBossWeakBase pTarget)
    {
        if (objBossWeakBarRoot == null ||
            tranBossWeakBarRoot == null)
            return;
        GameObject objNewInfoSlot = GameObject.Instantiate(objBossWeakBarRoot) as GameObject;
        objNewInfoSlot.SetActive(true);

        Transform tranNewInfo = objNewInfoSlot.GetComponent<Transform>();
        tranNewInfo.SetParent(tranBossWeakBarRoot);
        tranNewInfo.localScale = Vector3.one;
        tranNewInfo.localRotation = Quaternion.identity;
        tranNewInfo.localPosition = Vector3.zero;

        UIRoomBossWeakHPBar pNewSlot = objNewInfoSlot.GetComponent<UIRoomBossWeakHPBar>();

        pNewSlot.SetTarget(pTarget);

        listBossWeakBars.Add(pNewSlot);
    }

    public void ActiveBossWeakBar(bool bActive)
    {
        for(int i = 0;i < listBossWeakBars.Count;i++)
        {
            if (listBossWeakBars[i] == null) continue;
            listBossWeakBars[i].SetActive(bActive);
        }
    }



    public UIBossPlayerEatSlot AddEatPlayer(CPlayerUnit unit)
    {
        if (unit == null) return null;
        UIBossPlayerEatSlot pRes = GetEatPlayer(unit.uid);
        if(pRes!=null)
        {
            pRes.InitInfo(unit);
            return pRes;
        }
        else
        {
            GameObject objNewSlot = GameObject.Instantiate(objPlayeEatRoot) as GameObject;
            objNewSlot.SetActive(true);
            Transform tranNewSlot = objNewSlot.transform;
            tranNewSlot.SetParent(tranPlayerEatGrid);
            tranNewSlot.localPosition = Vector3.zero;
            tranNewSlot.localRotation = Quaternion.identity;
            tranNewSlot.localScale = Vector3.one;
            UIBossPlayerEatSlot pNewSlot = objNewSlot.GetComponent<UIBossPlayerEatSlot>();
            pNewSlot.InitInfo(unit);

            listEatSlots.Add(pNewSlot);
            LayoutRebuilder.ForceRebuildLayoutImmediate(tranPlayerEatGrid);

            return pNewSlot;
        }
    }

    public void RemoveEatPlayer(string uid)
    {
        for(int i=0; i<listEatSlots.Count; i++)
        {
            if(listEatSlots[i].uid.Equals(uid))
            {
                Destroy(listEatSlots[i].gameObject);
                listEatSlots.RemoveAt(i);
                LayoutRebuilder.ForceRebuildLayoutImmediate(tranPlayerEatGrid);
                break;
            }
        }
    }

    UIBossPlayerEatSlot GetEatPlayer(string uid)
    {
        return listEatSlots.Find(x => x.uid.Equals(uid));
    }

    public void SetBoss104AtkTip(CBoss104.EMAtkPath path)
    {
        for(int i=0;i<arrBoss104AtkTip.Length; i++)
        {
            arrBoss104AtkTip[i].SetActive(i == (int)path);
        }
    }

    public void SetBoss204AtkTip(bool active)
    {
        for (int i = 0; i < arrBoss204AtkTip.Length; i++)
        {
            arrBoss204AtkTip[i].SetActive(active);
        }
    }

    public void SetBoss304AtkTip(bool active)
    {
        for (int i = 0; i < arrBoss204AtkTip.Length; i++)
        {
            arrBoss304AtkTip[i].SetActive(active);
        }
    }

    public void AddDmgTip(long dmg, EMDmgType dmgType, EMRare rare, Vector3 pos)
    {
        int nIdx = (int)dmgType;
        if (nIdx < 0 || nIdx >= arrObjDmgTip.Length) return;

        bool bAddNew = true;
        List<GameObject> listDmgs = null;
        GameObject objNewTip = null;
        if (dicIdleDmgTips.TryGetValue(dmgType, out listDmgs))
        {
            if(listDmgs.Count > 0)
            {
                bAddNew = false;

                objNewTip = listDmgs[0];
                listDmgs.RemoveAt(0);
            }
        }

        if(bAddNew)
        {
            objNewTip = GameObject.Instantiate(arrObjDmgTip[(int)dmgType]) as GameObject;
            objNewTip.SetActive(true);
        }

        Transform tranNewTip = objNewTip.GetComponent<Transform>();
        tranNewTip.SetParent(tranRootDmgTip);
        tranNewTip.localScale = Vector3.one;
        tranNewTip.localRotation = Quaternion.identity;

        //刷新坐标
        Vector3 vTargetScreenPos = Camera.main.WorldToScreenPoint(pos);
        vTargetScreenPos.z = 0F;

        if (UIManager.Instance == null) return;
        Vector3 vSelfWorldPos = UIManager.Instance.uiCamDefault.uiCam.ScreenToWorldPoint(vTargetScreenPos);
        tranNewTip.position = vSelfWorldPos;

        Text uiLabelDmg = objNewTip.GetComponent<Text>();
        uiLabelDmg.text = dmg.ToString();
        if (dmgType == EMDmgType.Normal)
        {
            Color pTargetColor = Color.white;
            switch (rare)
            {
                case EMRare.Normal:
                    pTargetColor = CGameColorFishMgr.Ins.pStaticConfig.GetColor("bulletR");
                    break;
                case EMRare.YouXiu:
                    pTargetColor = CGameColorFishMgr.Ins.pStaticConfig.GetColor("fishR");
                    break;
                case EMRare.XiYou:
                    pTargetColor = CGameColorFishMgr.Ins.pStaticConfig.GetColor("bulletSR");
                    break;
                case EMRare.Special:
                    pTargetColor = CGameColorFishMgr.Ins.pStaticConfig.GetColor("fishSSR");
                    break;
                case EMRare.Shisi:
                    pTargetColor = CGameColorFishMgr.Ins.pStaticConfig.GetColor("fishUR");
                    break;
            }
            uiLabelDmg.color = pTargetColor;
        }

        UITweenPos uiTweenPos = objNewTip.GetComponent<UITweenPos>();
        uiTweenPos.from = tranNewTip.localPosition;
        uiTweenPos.to = tranNewTip.localPosition + Vector3.up * 100F;
        uiTweenPos.Play();

        UITweenAlpha uiTweenAlpha = objNewTip.GetComponent<UITweenAlpha>();
        uiTweenAlpha.bInited = false;
        uiTweenAlpha.Play();

        UITweenScale uiTweenScale = objNewTip.GetComponent<UITweenScale>();
        EMDmgType curDmgType = dmgType;
        uiTweenScale.callOver = delegate () {
            RecycleDmgTip(curDmgType, objNewTip);
        };
        uiTweenScale.Play();
    }

    void RecycleDmgTip(EMDmgType dmgType, GameObject value)
    {
        value.transform.localPosition = new Vector3(10000F, 0, 0);

        List<GameObject> listDmgs = null;
        if (dicIdleDmgTips.TryGetValue(dmgType, out listDmgs))
        {
            listDmgs.Add(value);
        }
        else
        {
            listDmgs = new List<GameObject>();
            listDmgs.Add(value);
            dicIdleDmgTips.Add(dmgType, listDmgs);
        }
    }

    public void PlayGameStartCount(string content)
    {
        uiLabelGameStartCount.text = content;
        uiLabelGameStartCount.GetComponent<UITweenScale>().Play();
    }

   

}
