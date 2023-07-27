using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShowUnitInfo : MonoBehaviour
{
    public string lOwnerID;
    public GameObject objNameBG;
    [Header("名字文本")]
    public Text uiLabelName;
    [Header("玩家头像")]
    public RawImage pPlayerIcon;
    [Header("鱼币数量")]
    public Text uiGold;
    [Header("挂机状态显示")]
    public GameObject objGuaJiState;
    //[Header("鱼饵信息Root")]
    //public UIShowGiftSlot uiBaitRoot;
    //[Header("鱼竿信息Root")]
    //public UIShowGiftSlot uiRobRoot;
    [Header("渔具信息Root")]
    public UIShowGiftSlot uiItemPackRoot;
    [Header("飞轮信息Root")]
    public UIShowGiftSlot uiLunRoot;

    public Image uiImgLvBG;
    public Text uiLabelLv;
    public Sprite[] arrLvBG;

    string szNameContent;
    
    [ReadOnly]
    public CPlayerUnit pTarget;
    Transform tranSelf;

    // Start is called before the first frame update
    void Start()
    {
        tranSelf = GetComponent<Transform>();
    }

    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.F2))
    //    {
    //        ShowNameLabel(true);
    //    }
        
    //    if(Input.GetKeyUp(KeyCode.F2))
    //    {
    //        ShowNameLabel(false);
    //    }
    //}

    private void LateUpdate()
    {
        ///判断绑定的目标是否还存在
        if (pTarget == null)
        {
            Debug.Log("回收了");
            Destroy(gameObject);
            return;
        }

        RefreshPos(pTarget.tranNameRoot);
    }

    public void SetUnit(CPlayerUnit unit)
    {
        lOwnerID = unit.uid;
        pTarget = unit;
        if (pTarget == null)
        {
            Debug.LogError("空的游戏对象!");
            Recycle();
            return;
        }
        pTarget.dlgRecycle += this.Recycle;
        unit.dlgChgActiveState = this.ChgActiveState;
        unit.dlgChgGift = this.CheckGiftInfo;
        //加载头像
        if (pPlayerIcon != null)
        {
            CAysncImageDownload.Ins.downloadImageAction(unit.pInfo.userFace, pPlayerIcon);
        }
        //if (unit.pInfo.guardLevel == 3)   //舰长
        //{

        //}
        //else if (unit.pInfo.guardLevel == 2) //提督
        //{

        //}
        //else if (unit.pInfo.guardLevel == 1) // 总督
        //{

        //}
        //else
        //{

        //}
        szNameContent = pTarget.pInfo.userName;
        uiLabelName.text = szNameContent;
        ShowNameLabel(CGameColorFishMgr.Ins.bShowPlayerInfo);
        //uiLabelName.text = pTarget.pInfo.userName;
        if (pTarget.pInfo.GetVipLv() == CPlayerBaseInfo.EMVipLv.Normal)
        {
            uiLabelName.color = new Color(1F, 1f, 1f);
        }
        else if (pTarget.pInfo.GetVipLv() == CPlayerBaseInfo.EMVipLv.Green)
        {
            uiLabelName.color = new Color(0.19F, 1f, 0.19f);
        }
        else if(pTarget.pInfo.GetVipLv() == CPlayerBaseInfo.EMVipLv.Jianzhang)
        {
            uiLabelName.color = new Color(0F, 1f, 1f);
        }
        else if(pTarget.pInfo.GetVipLv() == CPlayerBaseInfo.EMVipLv.Tidu)
        {
            uiLabelName.color = new Color(1F, 0.49f, 0.98f);
        }
        else if (pTarget.pInfo.GetVipLv() == CPlayerBaseInfo.EMVipLv.Pro)
        {
            uiLabelName.color = new Color(1F, 0.72f, 0.19f);
        }
        else if (pTarget.pInfo.GetVipLv() == CPlayerBaseInfo.EMVipLv.Zongdu)
        {
            uiLabelName.color = new Color(1F, 0.2f, 0.1f);
        }

        if (uiGold != null)
        {
            CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(pTarget.uid);
            if (pPlayer != null)
            {
                uiGold.text = pPlayer.GameCoins.ToString("f0");
            }
            //uiGold.text = unit.pInfo.GameCoins.ToString("f0");
        }

        RefreshUserLv(unit.pInfo.nUserLv, unit.pInfo.nUserExp);
        unit.pInfo.dlgUserLvChg += this.RefreshUserLv;

        RefreshPos(unit.tranNameRoot);
        CheckGiftInfo();
    }

    /// <summary>
    /// 检测该玩家的所持礼物状态
    /// </summary>
    void CheckGiftInfo()
    {
        ///检测玩家信息
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(pTarget.uid);
        if (pPlayer == null)
        {
            return;
        }

        /////判断鱼饵数量
        //if (pPlayer.nlBaitCount + pPlayer.nlFreeBaitCount > 0)
        //{
        //    uiBaitRoot.SetActive(true);
        //    uiBaitRoot.SetCount(pPlayer.nlBaitCount + pPlayer.nlFreeBaitCount);
        //}
        //else
        //{
        //    uiBaitRoot.SetActive(false);
        //}

        ///判断浮漂数量
        if (pPlayer.nlBaitCount + pPlayer.nlFreeBaitCount > 0)
        {
            uiItemPackRoot.SetActive(true);
            uiItemPackRoot.SetCount(pPlayer.nlBaitCount + pPlayer.nlFreeBaitCount);
        }
        else
        {
            uiItemPackRoot.SetActive(false);
        }

        /////判断鱼竿数量
        //if (pPlayer.nlRobCount > 0)
        //{
        //    uiRobRoot.SetActive(true);
        //    uiRobRoot.SetCount(pPlayer.nlRobCount);
        //}
        //else
        //{
        //    uiRobRoot.SetActive(false);
        //}

        ///判断飞轮数量
        if (pPlayer.nlFeiLunCount > 0)
        {
            uiLunRoot.SetActive(true);
            uiLunRoot.SetCount(pPlayer.nlFeiLunCount);
        }
        else
        {
            uiLunRoot.SetActive(false);
        }
    }

    /// <summary>
    /// 改变激活状态
    /// </summary>
    /// <param name="bActive"></param>
    void ChgActiveState(bool bActive)
    {
        if (objGuaJiState == null) return;
        objGuaJiState.SetActive(!bActive);
    }

    /// <summary>
    /// 刷新坐标
    /// </summary>
    void RefreshPos(Transform root)
    {
        if (tranSelf == null || root == null) return;

        Vector3 vTargetScreenPos = Camera.main.WorldToViewportPoint(root.position);
        vTargetScreenPos.z = 0F;

        Vector3 vSelfWorldPos = UIManager.Instance.uiCamDefault.uiCam.ViewportToWorldPoint(vTargetScreenPos);
        tranSelf.position = vSelfWorldPos;
        tranSelf.localPosition = new Vector3(tranSelf.localPosition.x, tranSelf.localPosition.y, 0F);
    }

    /// <summary>
    /// 是否显示名字
    /// </summary>
    /// <param name="show"></param>
    public void ShowNameLabel(bool show)
    {
        //if(show)
        //{
        //    uiLabelName.text = szNameContent;
        //}
        //else
        //{
        //    uiLabelName.text = "";
        //}

        objNameBG.SetActive(show);
        CheckGiftInfo();
    }

    /// <summary>
    /// 踢出该玩家
    /// </summary>
    public void OnClickExit()
    {
        string uid = pTarget.uid;
        //看看是不是在游戏队列
        CPlayerBaseInfo pActivePlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pActivePlayer != null)
        {
            CPlayerMgr.Ins.RemoveActivePlayer(pActivePlayer);
            CPlayerMgr.Ins.RemovePlayer(uid);
            if (CControlerSlotMgr.Ins != null)
            {
                CControlerSlotMgr.Ins.RecycleSlot(uid);
            }
            return;
        }
    }

    void RefreshUserLv(long lv, long exp)
    {
        ST_UserLvConfig pTBLInfo = CTBLHandlerUserLvConfig.Ins.GetInfo((int)lv);
        if (pTBLInfo == null)
        {
            uiLabelLv.text = "1";
            uiImgLvBG.sprite = arrLvBG[0];
            return;
        }

        uiLabelLv.text = pTBLInfo.nShowLv.ToString();
        if(pTBLInfo.nTag >= 0 && pTBLInfo.nTag < arrLvBG.Length)
        {
            uiImgLvBG.sprite = arrLvBG[pTBLInfo.nTag];
        }
        else
        {
            uiImgLvBG.sprite = arrLvBG[0];
        }
    }

    private void OnDestroy()
    {
        CPlayerBaseInfo pPlayerInfo = CPlayerMgr.Ins.GetPlayer(lOwnerID);
        if (pPlayerInfo == null) return;

        pPlayerInfo.dlgUserLvChg = null;
    }

    void Recycle()
    {
        Debug.Log("回收：" + lOwnerID);

        UIGameInfo uiShow = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (uiShow != null)
        {
            uiShow.RecycleUnitInfo(this);
        }
    }

}
