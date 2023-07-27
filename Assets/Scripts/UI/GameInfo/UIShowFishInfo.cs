using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShowFishInfo : MonoBehaviour
{
    public string lOwnerID;

    [Header("钓鱼状态Obj")]
    public GameObject objFishValue;
    [Header("钓鱼中不可拉杆Obj")]
    public GameObject objFishStay;
    [Header("钓鱼中可拉杆Obj")]
    public GameObject[] objFishCanLaGan;
    [Header("展示鱼信息的Root")]
    public GameObject objShowFishRoot;
    [Header("礼物信息的Root")]
    public UIGiftInfo giftInfo;
    [Header("鱼的信息")]
    public UIFishInfo fishInfo;
    [Header("增加鱼币的文本")]
    public Text uiAddCoinText;
    [Header("额外增加鱼币的文本")]
    public Text uiAddCoinTextByGift;
    public GameObject objAddCoinTextByGift;
    [Header("增加鱼币的数量Obj")]
    public GameObject objAddCoin;
    [Header("决斗图标")]
    public GameObject objBattleImg;

    [ReadOnly]
    public CPlayerUnit pTarget;
    Transform tranSelf;

    CFishInfo pCurShowFishInfo;

    CPropertyTimer pCheckTick;
    public float fShowTime = 5f;

    void Start()
    {
        tranSelf = GetComponent<Transform>();
    }

    private void Update()
    {
        if (pCheckTick != null &&
           pCheckTick.Tick(CTimeMgr.DeltaTime))
        {
            pCheckTick = null;
            giftInfo.SetActive(false);
        }
    }

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

        unit.dlgChgState = this.CheckFishStateByFish;
        unit.dlgGetFish = this.ChgFishInfo;
        unit.dlgChgLaGanState = this.ChgLaGanState;
        unit.dlgShowAddCoin = this.ShowAddCoin;
        unit.dlgOnlySetAddCoin = this.SetAddCoin;

        objFishValue.SetActive(false);
        objShowFishRoot.SetActive(false);
        giftInfo.SetActive(false);
        objBattleImg.SetActive(false);
        objAddCoin.SetActive(false);
        RefreshPos(unit.tranNameRoot);
    }

    public void InitGiftInfo(ST_GiftMenuByBoat giftMenuByBoat)
    {
        giftInfo.Init(giftMenuByBoat);
    }

    /// <summary>
    /// 展示礼物信息
    /// </summary>
    public void ShowGiftInfo()
    {
        giftInfo.SetActive(true);
        pCheckTick = new CPropertyTimer();
        pCheckTick.Value = fShowTime;
        pCheckTick.FillTime();
    }

    public void ShowAddCoin()
    {
        objAddCoin.SetActive(true);
        UITweenPos pTweenPos = objAddCoin.GetComponent<UITweenPos>();
        pTweenPos.Play();
        UITweenAlpha pTweenAlpha = objAddCoin.GetComponent<UITweenAlpha>();
        pTweenAlpha.Play();
        pTweenAlpha.callOver = delegate ()
        {
            objAddCoin.SetActive(false);
        };
        if(pCurShowFishInfo != null && 
           pCurShowFishInfo.emItemType == EMItemType.RandomEvent && 
           pCurShowFishInfo.nRandomID > 0)
        {
            CPlayerBaseInfo baseInfo = CPlayerMgr.Ins.GetPlayer(pTarget.uid);
            CGameColorFishMgr.Ins.DoRandomEvent(pCurShowFishInfo.nRandomID, baseInfo);
        }
    }


    /// <summary>
    /// 变换拉杆状态
    /// </summary>
    /// <param name="bLaGan"></param>
    void ChgLaGanState(bool bLaGan)
    {
        objFishStay.SetActive(!bLaGan);
        for (int i = 0; i < objFishCanLaGan.Length; i++)
        {
            objFishCanLaGan[i].SetActive(bLaGan);
        }
    }


    /// <summary>
    /// 替换钓鱼信息
    /// </summary>
    /// <param name="info"></param>
    void ChgFishInfo(CFishInfo info)
    {
        pCurShowFishInfo = info;
        if (fishInfo == null) return;
        fishInfo.Init(info);

        if(info.emItemType == EMItemType.BattleItem)
        {

        }
        else
        {
            SetAddCoin(info.lPrice,info.lAddPrice);
        }
        
        //uiAddCoinText.text = "+" + ((int)info.lPrice).ToString();
    }

    public void SetAddCoin(long add,long addCoin)
    {
        uiAddCoinText.text = "+" + add.ToString();
        objAddCoinTextByGift.SetActive(addCoin > 0);
        if (addCoin <= 0) return;
        uiAddCoinTextByGift.text = "+" + addCoin.ToString();
        uiAddCoinText.text = "+" + (add - addCoin).ToString();
    }

    /// <summary>
    /// 检测是否显示钓鱼进度
    /// </summary>
    /// <param name="state"></param>
    void CheckFishStateByFish(CPlayerUnit.EMState state)
    {
        if (objFishValue != null)
        {
            bool bActive = state == CPlayerUnit.EMState.Fishing;
            objFishValue.SetActive(bActive);
        }
        if (objShowFishRoot != null)
        {
            bool bShowFish = state == CPlayerUnit.EMState.ShowFish;
            objShowFishRoot.SetActive(bShowFish);
        }
        if(objBattleImg != null)
        {
            bool bShowBattle = state == CPlayerUnit.EMState.Battle;
            objBattleImg.SetActive(bShowBattle);
        }
    }

    public void ShowFishRoot()
    {
        objShowFishRoot.SetActive(true);
    }

    /// <summary>
    /// 刷新坐标
    /// </summary>
    void RefreshPos(Transform root)
    {
        if (tranSelf == null || root == null) return;

        Vector3 vTargetScreenPos = Camera.main.WorldToScreenPoint(root.position);
        vTargetScreenPos.z = 0F;

        Vector3 vSelfWorldPos = UIManager.Instance.uiCamDefault.uiCam.ScreenToWorldPoint(vTargetScreenPos);
        tranSelf.position = vSelfWorldPos;
        tranSelf.localPosition = new Vector3(tranSelf.localPosition.x, tranSelf.localPosition.y, 0F);
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



    void Recycle()
    {
        Debug.Log("回收：" + lOwnerID);

        UIGameInfo uiShow = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (uiShow != null)
        {
            uiShow.RecycleUnitFishInfoInfo(this);
        }
    }

}
