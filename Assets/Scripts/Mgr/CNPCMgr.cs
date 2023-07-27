using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CNPCMgr : MonoBehaviour
{
    static CNPCMgr ins = null;

    public static CNPCMgr Ins
    {
        get
        {
            return ins;
        }
    }

    /// <summary>
    /// NPC轮巡计时器
    /// </summary>
    public CPropertyTimer pChgTick;
    [Header("切换时间")]
    public Vector2 vChgTime;
    [Header("普通竞拍Unit")]
    public CNPCUnit pAuctionUnit;
    [Header("材料竞拍Unit")]
    public CNPCMatUnit pMatAuctionUnit;
    [Header("指导员Unit")]
    public CHelpUnit pHelpUnit;

    public int nMatNPCRate;
    
    /// <summary>
    /// 角色信息保存
    /// </summary>
    List<ST_UnitAvatar> listRollInfos = new List<ST_UnitAvatar>();

    public bool bDebug = false;

    public void Awake()
    {
        ins = this;
    }

    private void Start()
    {
        GetChgInfo();
        //pChgTick = new CPropertyTimer();
        //pChgTick.Value = Random.Range(vChgTime.x, vChgTime.y);
        //pChgTick.FillTime();
    }

    public void PlaceHelpUnit()
    {
        return;
        if (CGameColorFishMgr.Ins.nCurRateUpLv > CGameColorFishMgr.Ins.pStaticConfig.GetInt("帮助角色出现最低等级"))
            return;
        if (pHelpUnit != null)
        {
            CMapSlot mapSlot = CGameColorFishMgr.Ins.pMap.pMapSlots[CGameColorFishMgr.Ins.pMap.pMapSlots.Count - 1];
            mapSlot.BindPlayer(pHelpUnit.pBindSlot);
            pHelpUnit.transform.SetParent(mapSlot.tranSelf);
            pHelpUnit.transform.localPosition = Vector3.zero;
            pHelpUnit.transform.localRotation = Quaternion.identity;
            pHelpUnit.Init(511);
        }
    }

    private void Update()
    {
        return;
        ///判断是否开启了拍卖
        if (CTBLHandlerFucAble.Ins.GetInfo((int)EMFuncAbleType.Auction).CheckSceneLv())
        {
            return;
        }
        if (pAuctionUnit != null &&
           pAuctionUnit.emCurState == CNPCUnit.EMState.Idle &&
           pMatAuctionUnit != null &&
           pMatAuctionUnit.emCurState == CNPCUnit.EMState.Idle &&
           pChgTick != null &&
           pChgTick.Tick(CTimeMgr.DeltaTime))
        {
            AuctionUnitShow();
        }
        if(bDebug)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                if(pAuctionUnit.emCurState != CNPCUnit.EMState.Idle)
                {

                }

                AuctionUnitShow();
            }
        }
    }

    public void ShowAuction()
    {
        pAuctionUnit.Init(listRollInfos[Random.Range(0, listRollInfos.Count)].nID);
    }

    /// <summary>
    /// 展示竞拍角色
    /// </summary>
    void AuctionUnitShow()
    {
        if (listRollInfos.Count <= 0) return;

        //if (CPlayerMgr.Ins.pOwner.uid == 38367534 ||
        //    CPlayerMgr.Ins.pOwner.uid == 269522141)
        //{
            int nRate = Random.Range(1, 101);
            if (nRate > nMatNPCRate)
            {
                if (pMatAuctionUnit.emCurState != CNPCUnit.EMState.Idle)
                {
                    pMatAuctionUnit.Reset();
                    UIMatAuction auction = UIManager.Instance.GetUI(UIResType.MatAuction) as UIMatAuction;
                    if (auction != null)
                        auction.Hide();
                }
                pAuctionUnit.Init(listRollInfos[Random.Range(0, listRollInfos.Count)].nID);
            }
            else
            {
                if (pAuctionUnit.emCurState != CNPCUnit.EMState.Idle)
                {
                    pAuctionUnit.Reset();
                    UIAuction auction = UIManager.Instance.GetUI(UIResType.Auction) as UIAuction;
                    if (auction != null)
                        auction.Hide();
                }
                pMatAuctionUnit.Init(listRollInfos[Random.Range(0, listRollInfos.Count)].nID);
            }
        //}
        //else
        //{
        //    pAuctionUnit.Init(listRollInfos[Random.Range(0, listRollInfos.Count)].nID);
        //}

       
        //pChgTick = new CPropertyTimer();
        //pChgTick.Value = fChgTime;
        pChgTick.FillTime();
    }

    /// <summary>
    /// 获取替换的信息
    /// </summary>
    void GetChgInfo()
    {
        listRollInfos.Clear();
        List<ST_UnitAvatar> unitAvatar = CTBLHandlerUnitAvatar.Ins.GetInfos();
        for (int i = 0; i < unitAvatar.Count; i++)
        {
            if (unitAvatar[i].emRare == ST_UnitAvatar.EMRare.SSR)
            {
                listRollInfos.Add(unitAvatar[i]);
            }
        }
    }

}
