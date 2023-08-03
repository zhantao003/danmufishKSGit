using OpenBLive.Runtime.Data;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 玩家单位
/// </summary>
public class CPlayerUnit : MonoBehaviour
{
    /// <summary>
    /// 炸弹信息
    /// </summary>
    public class BoomInfo
    {
        public bool bPay;      //是否为付费炸弹
    }

    /// <summary>
    /// 用户UID
    /// </summary>
    public string uid;

    //用户信息
    public CPlayerBaseInfo pInfo;

    public CPlayerUnitSpecMgr pSpecMgr;

    public CMapSlot pMapSlot;

    [HideInInspector]
    public Transform tranSelf;
    public Transform tranBody;
    [Header("跳跃速度")]
    public float fJumpSpeed;
    [Header("跳跃高度")]
    public float fJumpHeight;

    public string szBornEff;
    public enum EMState
    { 
        Idle,               //待机
        Move,               //移动
        StartFish,          //开始钓鱼
        Fishing,            //钓鱼中
        EndFish,            //结束钓鱼
        ShowFish,           //展示钓到的鱼
        Jump,               //跳跃
        Battle,             //决斗
        Drag,               //拖拽
        DragCancel,         //拖拽取消的状态
        Wait,
        Start,
        Born,
        HitDrop,            //被击落上岸

        BossEat,            //被Boss吃掉了
        BossWait,           //等待复活
        BossReturn,         //复活
        GunShootBoss,       //兹崩：射击Boss
        RPGShootBoss,       //RPG：射击Boss

        BossEatShow,            //被Boss吃掉了
        BossWaitShow,           //等待复活
        BossReturnShow,         //复活
    }

    /// <summary>
    /// 当前的状态
    /// </summary>
    public EMState emCurState = EMState.Idle;
    /// <summary>
    /// 状态机
    /// </summary>
    public FSMManager pFSM;
    public Animator[] pAnimeMat;

    public CPlayerAvatar pAvatar;

    public GameObject pAvatarFishGan;

    [Header("移动结束后执行的动作")]
    public EMState pEndMoveState;
    [Header("移动的目标点")]
    public Vector3 vecMoveTarget;
    [Header("保存当前的钓鱼动作进度")]
    public float fCurFinishValue;
    [Header("开始钓鱼的动作所需时间")]
    public float fFinishStartTime;
    [Header("钓鱼最大持续时间")]
    public float fFinishStayTime;
    [Header("完成钓鱼的动作所需时间")]
    public float fFinishEndTime;
    [Header("展示钓到的鱼的所需时间")]
    public float fShowFishTime;
    [Header("钓鱼动作根据不同进度进行不同的级别")]
    public float[] fFinishCheckValues;
    [Header("增益效果")]
    public AddInfo pAddInfo;

    [Header("钓鱼的加快速度")]
    public float fFishSpeed;
    [Header("钓鱼获得鱼的概率增加")]
    public int nAddFishRate;
    [Header("钓鱼钓到大鱼的概率")]
    public int nAddBigFishRate;
    [Header("增加钓鱼钓到稀有物品的概率")]
    public int nAddRareRate;

    [Header("判断是否为活跃玩家")]
    public bool bActive;
    [Header("切换到可拉杆状态的时间")]
    public Vector2 vecChgLaGanStateTimeRange = new Vector2(40, 80);
    [Header("宝箱位置")]
    public Transform tranGachePos;
    [Header("炸鱼位置")]
    public Transform tranBoomFishPos;
    [Header("验证码检测概率")]
    public float fCheckYZMRate = 30;

    public GameObject objEffRich;

    /// <summary>
    /// 判断是否可以拉杆
    /// </summary>
    public bool bCanLaGan;

    /// <summary>
    /// 活跃状态判断时间
    /// </summary>
    public float fCheckActiveTime = 600;
    /// <summary>
    /// 活跃状态计时器
    /// </summary>
    CPropertyTimer pCheckActiveTick;

    public bool bAutoExit = true;

    /// <summary>
    /// 离开状态判断时间
    /// </summary>
    public float fCheckExtTime = 3600;
    /// <summary>
    /// 离开状态计时器
    /// </summary>
    CPropertyTimer pCheckExtTick;

    public string szAnima;

    //public EPOOutline.Outlinable[] arrOutLine;

    #region UI相关

    public Transform tranNameRoot;  //名字文本

    #endregion

    #region 监听事件

    public DelegateNFuncCall dlgRecycle;            //回收事件

    public DelegateStateFuncCall dlgChgState;       //状态变化事件

    public DelegateFFuncCall dlgChgFishValue;       //改变当前钓鱼进度

    public DelegateFishFuncCall dlgGetFish;         //获取鱼

    public DelegateBFuncCall dlgChgActiveState;     //改变活跃状态

    public DelegateBFuncCall dlgChgLaGanState;     //改变可拉杆状态

    public DelegateNFuncCall dlgChgGift;            //礼物数量改变

    public DelegateNFuncCall dlgShowAddCoin;        //展示增加的鱼币

    public DelegateLLFuncCall dlgOnlySetAddCoin;     //纯展示获得鱼币

    #endregion

    public CAudioMgr.CAudioSlottInfo pStartFishAudio;

    public CAudioMgr.CAudioSlottInfo pSoldFishAudio;

    public CAudioMgr.CAudioSlottInfo pEndFishAudio;

    public string szThrowGoldEffect;

    public string szThrowGiftEffect;

    public string szGoldEffect;

    public string szBoomEffect;

    public string szTreasureBoomEffect;

    public string szDianZangEffect;

    /// <summary>
    /// 炸弹队列
    /// </summary>
    public List<BoomInfo> listNormalBooms = new List<BoomInfo>();

    /// <summary>
    /// 宝藏炸弹队列
    /// </summary>
    public List<BoomInfo> listTreasureBooms = new List<BoomInfo>();

    public float fShowBoomFishDelay = 0.05F;

    public float fCheckBoomFishLerp = 1F;

    /// <summary>
    /// 已经使用的炸弹数量
    /// </summary>
    public int nHaveUseBoom = 0;

    /// <summary>
    /// 炸弹保底计算数量
    /// </summary>
    public int nBaoDiPayBoom = 0;

    /// <summary>
    /// 检测炸鱼队列
    /// </summary>
    CPropertyTimer pCheckBoomFishTick;

    /// <summary>
    /// 检测炸鱼队列
    /// </summary>
    CPropertyTimer pCheckBoomTreasureTick;

    /// <summary>
    /// 检测决斗CD
    /// </summary>
    CPropertyTimer pCheckDuelCD;

    public bool bDuelCD;

    public bool bCheckYZM;

    public bool bTestData;

    public int nAtkIdx;

    /// <summary>
    /// 之前呆着的格子
    /// </summary>
    public CMapSlot pPreSlot;

    public DelegateNFuncCall pShowFishEndEvent;

    //临时跳跃到点事件
    public DelegateNFuncCall dlgCallOnceJumpEvent;

    /// <summary>
    /// 决斗保底目标数
    /// </summary>
    int nBattleCheckTarget;
    /// <summary>
    /// 决斗保底当前计数
    /// </summary>
    int nBattleCurValue;

    //兹蹦CD
    public float fCDZibeng;
    public CPropertyTimer pTicerCDZibeng = new CPropertyTimer();

    public int nBoomAddRate;

    /// <summary>
    /// 重置决斗保底计数
    /// </summary>
    public void ResetBattleValue()
    {
        nBattleCurValue = 0;
        nBattleCheckTarget = Random.Range(CGameColorFishMgr.Ins.pStaticConfig.GetInt("决斗保底最小值"),
                                          CGameColorFishMgr.Ins.pStaticConfig.GetInt("决斗保底最大值") + 1);

        //Debug.Log("ResetBattle ====" + nBattleCurValue);
    }

    /// <summary>
    /// 增加决斗保底计数
    /// </summary>
    public void AddBattleValue()
    {
        nBattleCurValue += 1;
        //Debug.Log("AddBattle ====" + nBattleCurValue);
    }

    /// <summary>
    /// 是否为决斗保底
    /// </summary>
    /// <returns></returns>
    public bool IsBattleBaoDi()
    {
        bool bBaoDi = nBattleCurValue >= nBattleCheckTarget;
        if(bBaoDi)
        {
            //Debug.Log("BaoDiBattle ====" + nBattleCurValue);
            ResetBattleValue();
        }

        return bBaoDi;
    }

    public void ResetDuelCD()
    {
        bDuelCD = true;
        pCheckDuelCD = new CPropertyTimer();
        pCheckDuelCD.Value = CGameColorFishMgr.Ins.pStaticConfig.GetInt("发起决斗冷却时间") +
                             CGameColorFishMgr.Ins.pStaticConfig.GetInt("决斗时间") +
                             CGameColorFishMgr.Ins.pStaticConfig.GetInt("决斗结算倒计时");
        pCheckDuelCD.FillTime();
    }

    public void SetMapSlot(CMapSlot mapSlot, bool bCancelBindUnit = true)
    {
        if(bCancelBindUnit)
        {
            if (pMapSlot != null)
            {
                pMapSlot.BindPlayer(null);
            }
        }

        if (!bCancelBindUnit)
        {
            pPreSlot = pMapSlot;
        }
        
        pMapSlot = mapSlot;
        if (pMapSlot != null)
        {
            pMapSlot.BindPlayer(this);
        }

        if(mapSlot!=null &&
           mapSlot.tranSelf != null)
        {
            tranSelf.SetParent(mapSlot.tranSelf);
            tranSelf.localScale = Vector3.one;
            tranSelf.localRotation = Quaternion.identity;
        }
        else
        {
            Debug.LogError("错误地图格子");
        }
    }

    /// <summary>
    /// 跳回之前呆着的格子
    /// </summary>
    public void BackPreSlot()
    {
        if (pPreSlot == null) return;

        JumpTarget(pPreSlot);

        if(pPreSlot != null &&
            pPreSlot.pBindUnit == this)
        {
            //pPreSlot.pBindUnit = null;
        }
        pPreSlot = null;
    }

    /// <summary>
    /// 跳向目标
    /// </summary>
    /// <param name="tranTarget"></param>
    public void JumpTarget(CMapSlot mapSlot,bool bCancelBindUnit = true)
    {
        //换船
        if (pMapSlot != null)
        {
            pMapSlot.SetBoat(101, null, 0);
        }

        SetMapSlot(mapSlot, bCancelBindUnit);

        if (mapSlot != null)
        {
            //执行占格子的动作
            CLocalNetMsg msgJumpInfo = new CLocalNetMsg();
            msgJumpInfo.SetFloat("posX", mapSlot.tranSelf.position.x);
            msgJumpInfo.SetFloat("posY", mapSlot.tranSelf.position.y);
            msgJumpInfo.SetFloat("posZ", mapSlot.tranSelf.position.z);
            if (bCancelBindUnit)
            {
                SetState(EMState.Jump, msgJumpInfo);

                //换船
                if (pMapSlot != null)
                {
                    pMapSlot.SetBoat(pInfo.nBoatAvatarId, this, pInfo.guardLevel);
                }
            }
            else
            {
                SetState(EMState.Battle, msgJumpInfo);
            }
        }
    }

    #region 普通炸弹

    public void UpdateBoom()
    {
        if(emCurState == EMState.GunShootBoss ||
           emCurState == EMState.RPGShootBoss ||
           emCurState == EMState.BossEat ||
           emCurState == EMState.BossReturn ||
           emCurState == EMState.BossWait)
        {
            return;
        }

        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
        {
            if(CBattleModeMgr.Ins != null &&
                CBattleModeMgr.Ins.emCurState != CBattleModeMgr.EMGameState.Gaming)
            {
                return;
            }
        }

        //如果是Boss战,Boss处于不可攻击状态则停止丢炸弹
        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
        {
            if(CGameBossMgr.Ins ==null || 
               CGameBossMgr.Ins.pBoss == null ||
               !CGameBossMgr.Ins.pBoss.IsAtkAble())
            {
                return;
            }
        }

        if (pCheckBoomFishTick != null &&
            pCheckBoomFishTick.Tick(CTimeMgr.DeltaTime))
        {
            if (listNormalBooms.Count > 0)
            {
                PlayBoom();
            } 

            if (listNormalBooms.Count > 0)
            {
                pCheckBoomFishTick = new CPropertyTimer();
                pCheckBoomFishTick.Value = fCheckBoomFishLerp;
                pCheckBoomFishTick.FillTime();
            }
            else
            {
                pCheckBoomFishTick = null;
            }
        }
    }

    public void AddBoom(int num,bool pay = true)
    {
        BoomInfo boomInfo = new BoomInfo();
        boomInfo.bPay = pay;

        if (bTestData)
        {
            for (int i = 0; i < num; i++)
            {
                listNormalBooms.Add(boomInfo);
                PlayBoom();
            }
            return;
        }

        for (int i = 0; i < num; i++)
        {
            listNormalBooms.Add(boomInfo);
        }

        if (pCheckBoomFishTick != null)
        {
            //nBoomCount += num;
            return;
        }

        pCheckBoomFishTick = new CPropertyTimer();
        pCheckBoomFishTick.Value = fCheckBoomFishLerp;
        pCheckBoomFishTick.FillTime();

        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
        {
            if (CBattleModeMgr.Ins != null &&
                CBattleModeMgr.Ins.emCurState != CBattleModeMgr.EMGameState.Gaming)
            {
                return;
            }
        }

        if (emCurState == EMState.BossEat ||
           emCurState == EMState.BossReturn ||
           emCurState == EMState.BossWait ||
           emCurState == EMState.GunShootBoss ||
           emCurState == EMState.RPGShootBoss)
        {

        }
        else
        {
            if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
            {
                if(CGameBossMgr.Ins.pBoss!=null &&
                   CGameBossMgr.Ins.pBoss.IsAtkAble())
                {
                    PlayBoom();
                }
            }
            else
            {
                PlayBoom();
            } 
        }
    }

    public void PlayBoom()
    {
        if(bTestData)
        {
            bool goldBoom = Random.Range(0, 101) <= CGameColorFishMgr.Ins.pStaticConfig.GetInt("金炸弹出现概率");
            bool pay = listNormalBooms[0].bPay;
            int Count = CGameColorFishMgr.Ins.pStaticConfig.GetInt("金炸弹最小鱼数");// Random.Range(CGameColorFishMgr.Ins.pStaticConfig.GetInt("金炸弹最小鱼数"), CGameColorFishMgr.Ins.pStaticConfig.GetInt("金炸弹最大鱼数") + 1);
            ShowBoomFish(Count, goldBoom, pay);
            return;
        }

        if (string.IsNullOrEmpty(szBoomEffect)) return;
        
        bool bGoldBoom = Random.Range(0, 101) <= CGameColorFishMgr.Ins.pStaticConfig.GetInt("金炸弹出现概率");
        bool bPay = listNormalBooms[0].bPay;
        listNormalBooms.RemoveAt(0);
        if(bPay)
        {
            if(bGoldBoom)
            {
                nBaoDiPayBoom = 0;
            }
            else
            {
                nBaoDiPayBoom++;
                if(nBaoDiPayBoom >= CGameColorFishMgr.Ins.pStaticConfig.GetInt("金炸弹出现保底数量"))
                {
                    nBaoDiPayBoom = 0;
                    bGoldBoom = true;
                }
            }
        }
        //创建特效
        Vector3 vFwdDir = tranSelf.forward;
        vFwdDir.y = 0;
        vFwdDir = vFwdDir.normalized;
        CEffectMgr.Instance.CreateEffSync(szBoomEffect + (bGoldBoom ? "Gold" : ""), tranSelf.position, Quaternion.LookRotation(vFwdDir, Vector3.up), 0);

        int nCount = CGameColorFishMgr.Ins.pStaticConfig.GetInt("金炸弹最小鱼数");// Random.Range(CGameColorFishMgr.Ins.pStaticConfig.GetInt("金炸弹最小鱼数"), CGameColorFishMgr.Ins.pStaticConfig.GetInt("金炸弹最大鱼数") + 1);
        CTimeTickMgr.Inst.PushTicker(0.9F, delegate (object[] values)
        {
            nHaveUseBoom++;
            if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal ||
                CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema ||
                CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss ||
                CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive ||
                CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
            {
                ShowBoomFish(nCount, bGoldBoom, bPay);
            }
            else if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Battle)
            {
                ShowBoomFishVsModel(nCount, bGoldBoom, bPay);
            } 
        });
    }

    void ShowBoomFish(int nCount, bool bGoldBoom, bool bPay)
    {
        UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (gameInfo == null) return;
        bool bGetChengSeFish = nHaveUseBoom >= CGameColorFishMgr.Ins.pStaticConfig.GetInt("炸弹保底出橙色次数");
        //if (bGetChengSeFish)
        //{
        //    Debug.LogError("Must Get ChengSe");
        //    //nHaveUseBoom = 0;
        //}
        bool bHaveBianYi = false;

        CBomberCountInfo bomberCountInfo = null;        //炸弹保底信息
        if(bPay)
        {
            bomberCountInfo = CBomberCountMgr.Count(1);
            if (bomberCountInfo != null)
            {
                Debug.Log($"第{bomberCountInfo.nIdx}次  出现了<color=#F02000>{bomberCountInfo.szName}</color>");
            }
        }

        int nGetFishMatByGold = 0;
        if(bGoldBoom)
        {
            nGetFishMatByGold = CGameColorFishMgr.Ins.pStaticConfig.GetInt("金炸弹必出材料次数");
        }
        bool bFirstBoom = true;
        long nFinalAddCoin = 0;
        long nFinalAddExp = 0;
        for (int i = 0;i < nCount;i++)
        {
            CFishInfo fishInfo = null;
            if(bomberCountInfo != null && i == 0)
            {
                ST_FishInfo info = CGameColorFishMgr.Ins.pMap.pTBLHandlerFishInfo.GetInfo(bomberCountInfo.nFishId);
                fishInfo = new CFishInfo(info, bomberCountInfo.bBianyi, pInfo.GetProAddValue(EMAddUnitProType.FishSize));
                RecordFishInfo(fishInfo);
            }
            else if(i == nCount - 1 &&
                    bGoldBoom &&
                    CGameColorFishMgr.Ins.emCurGameType != CGameColorFishMgr.EMGameType.Boss)
            {
                ST_FishInfo info = CGameColorFishMgr.Ins.pMap.pTBLHandlerBoomFishInfo.GetInfo(1000003);
                fishInfo = new CFishInfo(info, false, pInfo.GetProAddValue(EMAddUnitProType.FishSize));
                RecordFishInfo(fishInfo);

            }
            else
            {
                if (nGetFishMatByGold > 0 && 
                    CGameColorFishMgr.Ins.emCurGameType != CGameColorFishMgr.EMGameType.Boss)
                {
                    fishInfo = GetFish(false, true, bGoldBoom, bGetChengSeFish, bHaveBianYi, true, bPay, true, bFirstBoom);
                    nGetFishMatByGold--;
                }
                else
                {
                    fishInfo = GetFish(false, true, bGoldBoom, bGetChengSeFish, bHaveBianYi, true, bPay ,false, bFirstBoom);
                    if (bGetChengSeFish)
                    {
                        //Debug.LogError("Must Chengse =====" + nHaveUseBoom);
                        bGetChengSeFish = false;
                    }
                    
                }
                if (bFirstBoom)
                {
                    bFirstBoom = false;
                }
            }

            if (fishInfo.bBianYi)
            {
                bHaveBianYi = true;
            }

            fishInfo.bBoom = bPay;
            //if (bTestData)
            //{
            //    Debug.Log("Boom Fish Data:{Name" + fishInfo.szName +
            //              ",Size:" + fishInfo.fCurSize.ToString("f2") +
            //              "cm,Price:" + fishInfo.lPrice.ToString("f2") + "}");
            //}
            //Debug.LogError("Boom Fish Data:{Name" + fishInfo.szName +
            //              ",Size:" + fishInfo.fCurSize.ToString("f2") +
            //              "cm,Price:" + fishInfo.lPrice.ToString("f2") + "}");
            if (fishInfo.emItemType == EMItemType.Fish &&
                fishInfo.emRare >= EMRare.Special)
            {
                //Debug.LogError("Reset====" + fishInfo.szName +"=====" + i + "=====" + bGetChengSeFish);
                nHaveUseBoom = 0;
            }

            nFinalAddCoin += fishInfo.lPrice;
            nFinalAddExp += Mathf.CeilToInt(fishInfo.nAddExp * fishInfo.fCurSizeRate);

            if (i > 0)
            {
                CTimeTickMgr.Inst.PushTicker(i * fShowBoomFishDelay, delegate (object[] values)
                {
                    if (tranBoomFishPos != null)
                    {
                        gameInfo.AddBoomFish(uid, fishInfo, tranBoomFishPos.position, false, delegate ()
                          {
                              if (fishInfo.emItemType == EMItemType.RandomEvent &&
                                  fishInfo.nRandomID > 0)
                              {
                                  CPlayerBaseInfo baseInfo = CPlayerMgr.Ins.GetPlayer(uid);
                                  CGameColorFishMgr.Ins.DoRandomEvent(fishInfo.nRandomID, baseInfo);
                                  ShowFishInfo(fishInfo, false, true, EMFishCoinAddFunc.Free, true, null, false);
                              }
                              else
                              {
                                  ShowFishInfo(fishInfo, false, true, EMFishCoinAddFunc.Pay, true, null, true);
                              }
                              DoBullet(fishInfo, 0F);
                          });
                    }
                });
            }
            else
            {
                if (tranBoomFishPos != null)
                {
                    gameInfo.AddBoomFish(uid, fishInfo, tranBoomFishPos.position, false, delegate ()
                    {
                        if (fishInfo.emItemType == EMItemType.RandomEvent &&
                            fishInfo.nRandomID > 0)
                        {
                            CPlayerBaseInfo baseInfo = CPlayerMgr.Ins.GetPlayer(uid);
                            CGameColorFishMgr.Ins.DoRandomEvent(fishInfo.nRandomID, baseInfo);
                            ShowFishInfo(fishInfo, false, true, EMFishCoinAddFunc.Free, true, null, false);
                        }
                        else
                        {
                            ShowFishInfo(fishInfo, false, true, EMFishCoinAddFunc.Pay, true, null, true);
                        }

                        DoBullet(fishInfo, 0F);
                    });
                }
            }
        }

        //加鱼币
        if (nFinalAddCoin > 0)
        {
            if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal ||
               CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema ||
               CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive)
            {
                AddCoinByHttp(nFinalAddCoin, EMFishCoinAddFunc.Pay, true, true);
            }
            else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
            {
                AddCoinByHttp(nFinalAddCoin, EMFishCoinAddFunc.Pay, true, true);
            }
        }

        //加经验
        if(nFinalAddExp > 0)
        {
            CPlayerNetHelper.AddUserExp(uid, nFinalAddExp);
        }
    }

    /// <summary>
    /// 炸鱼模式下的炸弹出货
    /// </summary>
    /// <param name="nCount"></param>
    /// <param name="bGoldBoom"></param>
    /// <param name="bPay"></param>
    void ShowBoomFishVsModel(int nCount, bool bGoldBoom, bool bPay)
    {
        UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (gameInfo == null) return;

        int nGiftTBID = 0;        //炸弹保底信息
        if (bPay)
        {
            nGiftTBID = CGameVSGiftPoolMgr.Ins.Count(uid, 1);
        }

        int nAddTreasurePoint = 0;
        long nAddCoin = 0;
        long nAddExp = 0;
        for (int i = 0; i < nCount; i++)
        {
            CFishInfo fishInfo = null;
            if (nGiftTBID>0 && i == 0)
            {
                ST_FishInfo info = CGameColorFishMgr.Ins.pMap.pTBLHandlerTreasureBombFishInfo.GetInfo(nGiftTBID);
                fishInfo = new CFishInfo(info, false);
            }
            else
            {
                fishInfo = GetFishVsModel(false, true, false, false, true, bPay);

                //判断是否需要重置计数器
                CGameVSGiftCountPlayerInfo pCountInfo = CGameVSGiftPoolMgr.Ins.GetPlayerCountInfo(uid);
                if(pCountInfo!=null)
                {
                    CGameVSGiftCountSlot pCountSlot = pCountInfo.GetInfo(fishInfo.nTBID);
                    if(pCountSlot!=null)
                    {
                        pCountSlot.Reset();
                    }
                }
            }

            fishInfo.bBoom = bPay;
            //if (bTestData)
            //{
            //    Debug.Log("Boom Fish Data:{Name" + fishInfo.szName +
            //              ",Size:" + fishInfo.fCurSize.ToString("f2") +
            //              "cm,Price:" + fishInfo.lPrice.ToString("f2") + "}");
            //}

            if (fishInfo.emRare >= EMRare.Special)
            {
                nHaveUseBoom = 0;
            }

            //添加宝藏积分
            nAddTreasurePoint += fishInfo.nTreasurePoint;
            nAddCoin += fishInfo.lPrice;
            nAddExp += fishInfo.nAddExp;

            if (bTestData)
            {
                ShowVSFishInfo(fishInfo);
            }
            else
            {
                if (i > 0)
                {
                    CTimeTickMgr.Inst.PushTicker(i * fShowBoomFishDelay, delegate (object[] values)
                    {
                        if (tranBoomFishPos != null)
                        {
                            gameInfo.AddBoomFish(uid, fishInfo, tranBoomFishPos.position, false, delegate ()
                            {
                                ShowVSFishInfo(fishInfo);
                            });
                        }
                    });
                }
                else
                {
                    if (tranBoomFishPos != null)
                    {
                        gameInfo.AddBoomFish(uid, fishInfo, tranBoomFishPos.position, true, delegate ()
                        {
                            ShowVSFishInfo(fishInfo);
                        });
                    }
                }
            }
        }
   
        if(nAddTreasurePoint > 0)
        {
            //增加宝藏积分
            //CHttpParam pReqParams = new CHttpParam
            //(
            //    new CHttpParamSlot("uid", uid.ToString()),
            //    new CHttpParamSlot("treasurePoint", nAddTreasurePoint.ToString())
            //);
            //CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddUserTreasurePoint, pReqParams, 10, true);

            CPlayerNetHelper.AddTreasureCoin(uid, nAddTreasurePoint);

            //主播同步增加宝藏积分
            //CHttpParam pReqParams2 = new CHttpParam
            //(
            //    new CHttpParamSlot("uid", CPlayerMgr.Ins.pOwner.uid.ToString()),
            //    new CHttpParamSlot("treasurePoint", (Mathf.CeilToInt(nAddTreasurePoint * 0.25F)).ToString())
            //);
            //CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddUserTreasurePoint, pReqParams2, 10, true);

            CPlayerNetHelper.AddTreasureCoin(CPlayerMgr.Ins.pOwner.uid, Mathf.CeilToInt(nAddTreasurePoint * 0.25F));
            
            //CPlayerNetHelper.AddTreasureCoin(CPlayerMgr.Ins.pOwner.uid, Mathf.CeilToInt(nAddTreasurePoint * 0.25F));
        }

        if(nAddCoin > 0)
        {
            AddCoinByHttp(nAddCoin,EMFishCoinAddFunc.Pay, true, true);
        }
    
        if(nAddExp > 0)
        {
            CPlayerNetHelper.AddUserExp(uid, nAddExp);
        }
    }

    #endregion

    #region 宝藏炸弹

    public void UpdateTreasureBoom()
    {
        if (emCurState == EMState.GunShootBoss ||
            emCurState == EMState.RPGShootBoss ||
            emCurState == EMState.BossEat ||
            emCurState == EMState.BossReturn ||
            emCurState == EMState.BossWait)
        {
            return;
        }

        if (pCheckBoomTreasureTick != null &&
            pCheckBoomTreasureTick.Tick(CTimeMgr.DeltaTime))
        {
            if (listTreasureBooms.Count > 0)
            {
                PlayTreasureBoom();
            }

            if (listTreasureBooms.Count > 0)
            {
                pCheckBoomTreasureTick = new CPropertyTimer();
                pCheckBoomTreasureTick.Value = fCheckBoomFishLerp;
                pCheckBoomTreasureTick.FillTime();
            }
            else
            {
                pCheckBoomTreasureTick = null;
            }
        }
    }

    public void AddTreasureBoom(int num)
    {
        BoomInfo boomInfo = new BoomInfo();
        boomInfo.bPay = true;

        if (bTestData)
        {
            for (int i = 0; i < num; i++)
            {
                listTreasureBooms.Add(boomInfo);
                PlayTreasureBoom();
            }
            return;
        }

        for (int i = 0; i < num; i++)
        {
            listTreasureBooms.Add(boomInfo);
        }

        if (pCheckBoomTreasureTick != null)
        {
            //nBoomCount += num;
            return;
        }

        pCheckBoomTreasureTick = new CPropertyTimer();
        pCheckBoomTreasureTick.Value = fCheckBoomFishLerp;
        pCheckBoomTreasureTick.FillTime();

        if (emCurState == EMState.BossEat ||
            emCurState == EMState.BossReturn ||
            emCurState == EMState.BossWait ||
            emCurState == EMState.GunShootBoss ||
            emCurState == EMState.RPGShootBoss)
        {

        }
        else
        {
            if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
            {
                if (CGameBossMgr.Ins.pBoss != null &&
                    CGameBossMgr.Ins.pBoss.IsAtkAble())
                {
                    PlayTreasureBoom();
                }
            }
            else
            {
                PlayTreasureBoom();
            }
        }
    }

    public void PlayTreasureBoom()
    {
        if (string.IsNullOrEmpty(szBoomEffect)) return;
        nHaveUseBoom++;
        bool bGoldBoom = Random.Range(0, 101) <= CGameColorFishMgr.Ins.pStaticConfig.GetInt("金炸弹出现概率");
        bool bPay = listTreasureBooms[0].bPay;
        listTreasureBooms.RemoveAt(0);
        if (bPay)
        {
            if (bGoldBoom)
            {
                nBaoDiPayBoom = 0;
            }
            else
            {
                nBaoDiPayBoom++;
                if (nBaoDiPayBoom >= CGameColorFishMgr.Ins.pStaticConfig.GetInt("金炸弹出现保底数量"))
                {
                    nBaoDiPayBoom = 0;
                    bGoldBoom = true;
                }
            }
        }

        //创建特效
        Vector3 vFwdDir = tranSelf.forward + tranSelf.right;
        vFwdDir.y = 0;
        vFwdDir = vFwdDir.normalized;
        CEffectMgr.Instance.CreateEffSync(szTreasureBoomEffect, tranSelf.position, Quaternion.LookRotation(vFwdDir, Vector3.up), 0);

        int nCount = CGameColorFishMgr.Ins.pStaticConfig.GetInt("金炸弹最小鱼数");// Random.Range(CGameColorFishMgr.Ins.pStaticConfig.GetInt("金炸弹最小鱼数"), CGameColorFishMgr.Ins.pStaticConfig.GetInt("金炸弹最大鱼数") + 1);
        CTimeTickMgr.Inst.PushTicker(0.9F, delegate (object[] values)
        {
            ShowBoomFishVsModel(nCount - 1, bGoldBoom, bPay);
        });
    }

    #endregion

    #region PlayAudio

    /// <summary>
    /// 播放开始钓鱼的音效
    /// </summary>
    public void PlayStartFishAudio()
    {
        if (pStartFishAudio != null)
        {
            CAudioMgr.Ins.PlaySoundBySlot(pStartFishAudio, transform.position);
        }
    }

    /// <summary>
    /// 向目标丢礼物特效
    /// </summary>
    /// <param name="tranTarget"></param>
    public void PlayGiftEffectToTarget(int nValue)
    {
        if (CGameColorFishMgr.Ins.pMap == null ||
            CGameColorFishMgr.Ins.pMap.pDuelBoat == null ||
            CGameColorFishMgr.Ins.pMap.pDuelBoat.giftUnit == null)
            return;
        if (CGameColorFishMgr.Ins.pMap.pDuelBoat.emCurState == CDuelBoat.EMState.Hide) //船正在隐藏阶段
        {
            return;
        }

        Transform target = CGameColorFishMgr.Ins.pMap.pDuelBoat.giftUnit.tranSelf;
        if (!CHelpTools.IsStringEmptyOrNone(szThrowGiftEffect))
        {
            CEffectMgr.Instance.CreateEffSync("Effect/" + szThrowGiftEffect, tranSelf.position, Quaternion.identity, 0,
            delegate (GameObject value)
            {
                CEffectBeizier pEffBeizier = value.GetComponent<CEffectBeizier>();
                if (pEffBeizier == null) return;

                pEffBeizier.SetTarget(tranSelf.position + Vector3.up * 1.5F, target, delegate ()
                {
                    CGetGiftInfo getGiftInfo = new CGetGiftInfo();
                    getGiftInfo.playerInfo = pInfo;
                    getGiftInfo.nGiftValue = nValue;

                    UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
                    if (roomInfo != null)
                    {
                        roomInfo.giftRoot.AddGiftValue(getGiftInfo);
                    }
                });
            });
        }
    }

    /// <summary>
    /// 向目标丢金币特效
    /// </summary>
    /// <param name="tranTarget"></param>
    public void PlayGoldEffectToTarget(Transform tranTarget, DelegateNFuncCall callSuc = null)
    {
        if (!CHelpTools.IsStringEmptyOrNone(szThrowGoldEffect))
        {
            CEffectMgr.Instance.CreateEffSync("Effect/" + szThrowGoldEffect, tranSelf.position, Quaternion.identity, 0,
            delegate (GameObject value)
            {
                CEffectBeizier pEffBeizier = value.GetComponent<CEffectBeizier>();
                if (pEffBeizier == null) return;

                pEffBeizier.SetTarget(tranSelf.position + Vector3.up * 1.5F, tranTarget, callSuc);
            });
        }
    }

    /// <summary>
    /// 播放售卖鱼的音效和对应的特效
    /// </summary>
    public void PlaySoldFishAudio(bool showui = true)
    {
        if (pSoldFishAudio != null)
        {
            CAudioMgr.Ins.PlaySoundBySlot(pSoldFishAudio, transform.position);
        }

        if (!CHelpTools.IsStringEmptyOrNone(szGoldEffect))
        {
            CEffectMgr.Instance.CreateEffSync("Effect/" + szGoldEffect, tranSelf.position - Vector3.up * 2F, Quaternion.identity, 0);
        }

        if(showui)
            dlgShowAddCoin?.Invoke();
    }
    /// <summary>
    /// 播放结束钓鱼的音效
    /// </summary>
    public void PlayEndFishAudio()
    {
        if (pEndFishAudio != null)
        {
            CAudioMgr.Ins.PlaySoundBySlot(pEndFishAudio, transform.position);
        }
    }

    #endregion

    /// <summary>
    /// 设置拉杆状态
    /// </summary>
    /// <param name="laGan"></param>
    public void SetLaGanState(bool laGan)
    {
        bCanLaGan = laGan;
        dlgChgLaGanState?.Invoke(bCanLaGan);
    }

    /// <summary>
    /// 获取礼物的增益信息
    /// </summary>
    public bool GetGiftAddInfo(bool bCheckCount = true)
    {
        pAddInfo.Clear();
        CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
        if (pPlayer == null) return false;

        bool bPlayer = false;

        if((pPlayer.nlBaitCount + pPlayer.nlFreeBaitCount) > 0)
        {
            bPlayer = true;
            //pAddInfo.nAddRandomRate += CGameColorFishMgr.Ins.pStaticConfig.GetInt("渔具套装加随机事件概率"); 
        }

        bool bReqAble = false;
        CHttpParam pReqCostParams = new CHttpParam();
        pReqCostParams.AddSlot(new CHttpParamSlot("uid", uid.ToString()));

        if(!bCheckCount ||
           (pPlayer.nlBaitCount + pPlayer.nlFreeBaitCount) > 0)
        {
            //ST_GiftInfo giftInfo = CTBLHandlerGiftInfo.Ins.GetInfo((int)EMGiftType.fishBait);
            //pAddInfo.AddNewInfo(giftInfo.pAddInfo);

            //ST_GiftInfo giftInfo2 = CTBLHandlerGiftInfo.Ins.GetInfo((int)EMGiftType.fishGan);
            //pAddInfo.AddNewInfo(giftInfo2.pAddInfo);

            //ST_GiftInfo giftInfo3 = CTBLHandlerGiftInfo.Ins.GetInfo((int)EMGiftType.fishPiao);
            //pAddInfo.AddNewInfo(giftInfo3.pAddInfo);

            if (bCheckCount)
            {
                pReqCostParams.AddSlot(new CHttpParamSlot("baitCount", "-1"));
                bReqAble = true;
            }
        }

        //if (!bCheckCount || 
        //    (pPlayer.nlBaitCount + pPlayer.nlFreeBaitCount) > 0)
        //{
        //    ST_GiftInfo giftInfo = CTBLHandlerGiftInfo.Ins.GetInfo((int)EMGiftType.fishBait);
        //    pAddInfo.AddNewInfo(giftInfo.pAddInfo);

        //    if (bCheckCount)
        //    {
        //        pReqCostParams.AddSlot(new CHttpParamSlot("baitCount", "-1"));
        //        bReqAble = true;

        //        //AddBaitByHttp(-1);
        //    }
        //}

        //if (!bCheckCount || pPlayer.nlRobCount > 0)
        //{
        //    ST_GiftInfo giftInfo = CTBLHandlerGiftInfo.Ins.GetInfo((int)EMGiftType.fishGan);
        //    pAddInfo.AddNewInfo(giftInfo.pAddInfo);

        //    if (bCheckCount)
        //    {
        //        //AddGiftByHttp(-1, EMGiftType.fishGan);
        //        pReqCostParams.AddSlot(new CHttpParamSlot("ganCount", "-1"));
        //        bReqAble = true;
        //    }
        //}

        //if (!bCheckCount || pPlayer.nlBuoyCount > 0)
        //{
        //    ST_GiftInfo giftInfo = CTBLHandlerGiftInfo.Ins.GetInfo((int)EMGiftType.fishPiao);
        //    pAddInfo.AddNewInfo(giftInfo.pAddInfo);

        //    if (bCheckCount)
        //    {
        //        //AddGiftByHttp(-1, EMGiftType.fishPiao);
        //        pReqCostParams.AddSlot(new CHttpParamSlot("fupiaoCount", "-1"));
        //        bReqAble = true;
        //    }
        //}

        if (!bCheckCount)
        {
            pAddInfo.nAddFishRate += 3;
        }
        else
        {
            if(bReqAble)
            {
                CHttpMgr.Instance.SendHttpMsg(CHttpConst.CostFishItems, pReqCostParams, CHttpMgr.Instance.nReconnectTimes, true);
            }
        }

        return bPlayer;
    }

    /// <summary>
    /// 获得鱼(决斗)
    /// </summary>
    public CFishInfo GetBattleFish()
    {
        ///判断是否为付费玩家
        bool bPayPlayer = false;
        bPayPlayer = GetGiftAddInfo(true);
        //pAddInfo.Clear();
        ///计算总加成
        int addFishRate = pAddInfo.nAddFishRate + CGameColorFishMgr.Ins.pStaticConfig.GetInt("基础钓鱼获取鱼的概率");
        int addFishRareRate = pAddInfo.nAddFishRareRate;
        int addZawuRareRate = pAddInfo.nAddZawuRareRate;
        int addFishMatRareRate = pAddInfo.nAddFishMatRareRate;
        int addBigFishRate = pAddInfo.nAddBigFishRate;
        int addFishMatRate = pAddInfo.nAddFishMatRate + CGameColorFishMgr.Ins.pStaticConfig.GetInt("基础钓鱼获取材料的概率");
        int addRandomEvent = pAddInfo.nAddRandomRate + CGameColorFishMgr.Ins.pStaticConfig.GetInt("基础钓鱼获取随机事件的概率");

        if (CCrazyTimeMgr.Ins != null)
        {
            if (CCrazyTimeMgr.Ins.bCrazy)
            {
                addFishRareRate += CGameColorFishMgr.Ins.pStaticConfig.GetInt("狂暴时间加稀有鱼概率");
            }
            if (CCrazyTimeMgr.Ins.bKongTou)
            {
                addFishRareRate += CGameColorFishMgr.Ins.pStaticConfig.GetInt("空投时间加稀有鱼概率");
            }
        }
        //if (CGameColorFishMgr.Ins.nCurRateUpLv > 1)
        //{
        //    ST_SceneRate sceneRate = CTBLHandlerSceneRate.Ins.GetInfo(CGameColorFishMgr.Ins.nCurRateUpLv);
        //    if (sceneRate != null)
        //    {
        //        addFishRate += sceneRate.nAddFishRate;
        //        addFishRareRate += sceneRate.nAddRareRate;
        //        addZawuRareRate += sceneRate.nAddRareRate;
        //        addFishMatRareRate += sceneRate.nAddRareRate;
        //        addBigFishRate += sceneRate.nAddBigFishRate;
        //    }
        //}
        ///获取该次钓鱼的获取的类型
        //EMItemType emItemType = GetFishType(addFishRate, addPiFuRate, addRandomEvent);
        EMItemType emItemType = EMItemType.Fish;
        ///获取钓到的信息
        ST_FishInfo fishInfo = null;
        ST_RandomEvent randomEvent = null;
        CFishInfo info = null;
        if (emItemType == EMItemType.RandomEvent)
        {
            randomEvent = CGameColorFishMgr.Ins.RandomEvent();
            info = new CFishInfo(randomEvent);
        }
        else
        {
            bool bMustBattleBaoDi = IsBattleBaoDi();
            ///基础增加倍率（判断是否有鱼群）
            float fBaseAddRate = ((pMapSlot != null && pMapSlot.HasFishPack()) ? CGameColorFishMgr.Ins.pStaticConfig.GetInt("鱼群增加的倍率") : 100) * 0.01f;

            if (bPayPlayer)
            {
                fishInfo = CGameColorFishMgr.Ins.pMap.pTBLHandlerFishInfo.
                           GetRandomFishInfo(emItemType,
                                             addFishRareRate,
                                             addZawuRareRate,
                                             addFishMatRareRate,
                                             fBaseAddRate,
                                             EMRare.XiYou);
            }
            else
            {
                fishInfo = CGameColorFishMgr.Ins.pMap.pTBLHandlerFishInfo.
                           GetRandomFishInfo(emItemType,
                                             addFishRareRate,
                                             addZawuRareRate,
                                             addFishMatRareRate,
                                             fBaseAddRate,
                                             EMRare.Normal);
                                             //bMustBattleBaoDi ? EMRare.XiYou : EMRare.Normal);
            }
            info = new CFishInfo(fishInfo, addBigFishRate, false, false, false, pInfo.GetProAddValue(EMAddUnitProType.FishSize));
        }

        ShowFishInfo(info, true, true, EMFishCoinAddFunc.Duel, false);
        dlgGetFish?.Invoke(info);

        ////特殊处理广播
        //if (info.nTBID == 2000109)
        //{
        //    CGameColorFishMgr.Ins.pAudioBroad.Play((CConfigFishBroad.EMBroadModel)CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.BROADMODEL), CConfigFishBroad.EMBroadType.ChouWazi);
        //}
        //else if (info.nTBID == 2000105)
        //{
        //    CGameColorFishMgr.Ins.pAudioBroad.Play((CConfigFishBroad.EMBroadModel)CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.BROADMODEL), CConfigFishBroad.EMBroadType.Pangci);
        //}

        if (info.emRare == EMRare.XiYou)
        {
            UIUserGetAvatar uiGetAvatar = UIManager.Instance.GetUI(UIResType.UserGetAvatarTip) as UIUserGetAvatar;
            if (uiGetAvatar != null)
            {
                uiGetAvatar.AddFishInfoSlot(pInfo, info);
            }
        }

        RecordFishInfo(info);

        return info;
        //Debug.Log("获得鱼：" + fishInfo.szName + "," + fishInfo.fCurSize + "cm," + fishInfo.GetPrice() + "鱼币===" + castInfo.fishInfo.fCurSize);
    }


    /// <summary>
    /// 获得鱼
    /// </summary>
    public CFishInfo GetFish(bool bShowFishInfo = true, bool bIsBomber = false, bool bGoldBoom = false, bool bMustGetChengSe = false, bool bHaveBianYi = false, bool bMustNoBianYi = false,bool bPay = false,bool bMustFishMat = false, bool bFirstBoom =false)
    {
        ///判断是否为付费玩家
        bool bPayPlayer = false;

        bool bAddFishPay = pInfo.nlBaitCount > 0 ||
                           pInfo.nlFeiLunCount > 0;

        bool bAddMatPay = pInfo.nlBaitCount > 0;

        if (bShowFishInfo)
        {
            //Boss战准备阶段不消耗
            if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss && 
               CGameBossMgr.Ins != null &&
               CGameBossMgr.Ins.emCurState == CGameBossMgr.EMState.Ready)
            {
                bPayPlayer = GetGiftAddInfo(false);
            }
            else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle &&
                     CBattleModeMgr.Ins != null &&
                     CBattleModeMgr.Ins.emCurState != CBattleModeMgr.EMGameState.Gaming)
            {
                bPayPlayer = GetGiftAddInfo(false);
            }
            else
            {
                bPayPlayer = GetGiftAddInfo(true);
            }
            
        }
        else
        {
            bPayPlayer = bPay;
            if (bGoldBoom)
            {
                GetGiftAddInfo(false);
            }
            else
            {
                if(bIsBomber)
                {
                    GetGiftAddInfo(false);
                }
                else
                {
                    pAddInfo.Clear();
                }
            }
            //GetGiftAddInfo(false);
        }

        ///计算总加成
        int addFishRate = pAddInfo.nAddFishRate + CGameColorFishMgr.Ins.pStaticConfig.GetInt("基础钓鱼获取鱼的概率");
        int addFishRareRate = pAddInfo.nAddFishRareRate;
        int addZawuRareRate = pAddInfo.nAddZawuRareRate;
        int addFishMatRareRate = pAddInfo.nAddFishMatRareRate;
        int addBigFishRate = pAddInfo.nAddBigFishRate;
        int addFishMatRate = pAddInfo.nAddFishMatRate + CGameColorFishMgr.Ins.pStaticConfig.GetInt("基础钓鱼获取材料的概率");
        int addRandomEvent = pAddInfo.nAddRandomRate + CGameColorFishMgr.Ins.pStaticConfig.GetInt("基础钓鱼获取随机事件的概率");

        if (CCrazyTimeMgr.Ins != null)
        {
            if (CCrazyTimeMgr.Ins.bCrazy)
            {
                addFishRareRate += CGameColorFishMgr.Ins.pStaticConfig.GetInt("狂暴时间加稀有鱼概率");
            }
            if (CCrazyTimeMgr.Ins.bKongTou)
            {
                addFishRareRate += CGameColorFishMgr.Ins.pStaticConfig.GetInt("空投时间加稀有鱼概率");
            }
        }

        //if (CGameColorFishMgr.Ins.nCurRateUpLv > 1)
        //{
        //    ST_SceneRate sceneRate = CTBLHandlerSceneRate.Ins.GetInfo(CGameColorFishMgr.Ins.nCurRateUpLv);
        //    if(sceneRate != null)
        //    {
        //        addFishRate += sceneRate.nAddFishRate;
        //        addFishRareRate += sceneRate.nAddRareRate;
        //        addZawuRareRate += sceneRate.nAddRareRate;
        //        addFishMatRareRate += sceneRate.nAddRareRate;
        //        addBigFishRate += sceneRate.nAddBigFishRate;
        //        //Debug.Log("Add Rate ====" + sceneRate.nAddFishRate + "=====" + sceneRate.nAddRareRate + "====" + sceneRate.nAddBigFishRate);
        //    }
        //}

        ///获取该次钓鱼的获取的类型
        EMItemType emItemType = EMItemType.Fish;
        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal ||
            CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema ||
            CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive ||
            CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
        {
            emItemType = GetFishType(addFishRate, addFishMatRate, addRandomEvent, bMustGetChengSe, bIsBomber);
        }
        else if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
        {
            emItemType = EMItemType.BattleItem;
        }
        if(bMustFishMat)
        {
            emItemType = EMItemType.FishMat;
        }
        //emItemType = EMItemType.FishMat;
        //Debug.LogError(addFishRate + "====Value =====" + addPiFuRate + "=====" + addRandomEvent + "====" + emItemType);
        ///获取钓到的信息

        ST_FishInfo fishInfo = null;
        ST_RandomEvent randomEvent = null;
        CFishInfo info = null;
        if (emItemType == EMItemType.RandomEvent)
        {
            randomEvent = CGameColorFishMgr.Ins.RandomEvent();
            info = new CFishInfo(randomEvent);
        }
        else
        { 
            ///基础增加倍率（判断是否有鱼群）
            float fBaseAddRate = (pMapSlot != null && pMapSlot.HasFishPack() ? CGameColorFishMgr.Ins.pStaticConfig.GetInt("鱼群增加的倍率") : 100) * 0.01f;
            if (bIsBomber)
            {
                EMRare emNormalRare = EMRare.Normal;
                if(bFirstBoom)
                {
                    if(bGoldBoom)
                    {
                        emNormalRare = EMRare.Special;
                    }
                    else
                    {
                        emNormalRare = EMRare.XiYou;
                    }
                }
                fishInfo = CGameColorFishMgr.Ins.pMap.pTBLHandlerBoomFishInfo.
                               GetRandomFishInfo(emItemType,
                                                 addFishRareRate,
                                                 addZawuRareRate,
                                                 addFishMatRareRate,
                                                 fBaseAddRate,
                                                 (bMustGetChengSe && emItemType == EMItemType.Fish) ? EMRare.Special : emNormalRare, bGoldBoom);
            }
            else
            {
                if(bPayPlayer)
                {
                    if (CGameColorFishMgr.Ins.emCurGameType != CGameColorFishMgr.EMGameType.Boss)
                    {
                        emItemType = EMItemType.Fish;
                    }
                    fishInfo = CGameColorFishMgr.Ins.pMap.pTBLHandlerFishInfo.
                               GetRandomFishInfo(emItemType,
                                                 addFishRareRate,
                                                 addZawuRareRate,
                                                 addFishMatRareRate,
                                                 fBaseAddRate,
                                                (bMustGetChengSe && emItemType == EMItemType.Fish) ? EMRare.Special : EMRare.XiYou, bGoldBoom);
                }
                else
                {
                    fishInfo = CGameColorFishMgr.Ins.pMap.pTBLHandlerFishInfo.
                               GetRandomFishInfo(emItemType,
                                                 addFishRareRate,
                                                 addZawuRareRate,
                                                 addFishMatRareRate,
                                                 fBaseAddRate,
                                                 (bMustGetChengSe && emItemType == EMItemType.Fish) ? EMRare.Special : EMRare.Normal, bGoldBoom);
                }
            }
           
            info = new CFishInfo(fishInfo, addBigFishRate, bHaveBianYi, bMustNoBianYi, bPayPlayer, pInfo.GetProAddValue(EMAddUnitProType.FishSize));
            if (bIsBomber)
            {
                info.emGetTypeByFish = EMGetTypeByFish.Boom;
            }
            else if(bPayPlayer)
            {
                info.emGetTypeByFish = EMGetTypeByFish.FishPack;
            }
            else
            {
                info.emGetTypeByFish = EMGetTypeByFish.Normal;
            }
        }
        //if (bMustGetChengSe)
        //{
        //    Debug.LogError(emItemType + "=== Must===" + info.emRare + "=====" + info.szName);
        //}
        if (bShowFishInfo)
        {
            if(emItemType == EMItemType.RandomEvent)
            {
                ShowFishInfo(info, true, true, EMFishCoinAddFunc.Free, bAddMatPay, null, false);
            }
            else
            {
                ShowFishInfo(info, true, true, bAddFishPay ? EMFishCoinAddFunc.Pay : EMFishCoinAddFunc.Free, bAddMatPay, null, true);
            }

            dlgGetFish?.Invoke(info);

            //if(info != null)
            //{
            //    if (info.nTBID == 2000111 ||
            //        info.nTBID == 2000115 ||
            //        info.nTBID == 2000120)
            //    {
            //        CGameColorFishMgr.Ins.pAudioBroad.Play((CConfigFishBroad.EMBroadModel)CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.BROADMODEL), CConfigFishBroad.EMBroadType.Duck);
            //    }
            //}
        }

        RecordFishInfo(info);

        //判断是否稀有鱼
        if(info.emRare >= EMRare.Special)
        {
            CEffectMgr.Instance.CreateEffSync("Effect/effOuhuangBuff", tranSelf, 0, false);
        }
        else if(info.emRare == EMRare.XiYou &&
            CGameColorFishMgr.Ins.emCurGameType != CGameColorFishMgr.EMGameType.Boss)
        {
            UIUserGetAvatar uiGetAvatar = UIManager.Instance.GetUI(UIResType.UserGetAvatarTip) as UIUserGetAvatar;
            if(uiGetAvatar!=null)
            {
                uiGetAvatar.AddFishInfoSlot(pInfo, info);
            }
        }

        //如果是红鱼就加急速能量
        if(info.emRare >= EMRare.Special)
        {
            if(CCrazyTimeMgr.Ins!=null)
            {
                CCrazyTimeMgr.Ins.AddBattery(1);
            }
        }

        return info;
        //Debug.Log("获得鱼：" + fishInfo.szName + "," + fishInfo.fCurSize + "cm," + fishInfo.GetPrice() + "鱼币===" + castInfo.fishInfo.fCurSize);
    }

    public CFishInfo GetFishVsModel(bool bShowFishInfo = true, bool bGoldBoom = false, bool bMustGetChengSe = false, bool bHaveBianYi = false, bool bMustNoBianYi = false, bool bPay = false)
    {
        ///判断是否为付费玩家
        bool bPayPlayer = false;

        EMItemType emItemType = EMItemType.Fish;

        ///获取钓到的信息
        ST_FishInfo fishInfo = null;
        ST_RandomEvent randomEvent = null;
        CFishInfo info = null;
        if (emItemType == EMItemType.RandomEvent)
        {
            randomEvent = CGameColorFishMgr.Ins.RandomEvent();
            info = new CFishInfo(randomEvent);
        }
        else
        {
            ///基础增加倍率（判断是否有鱼群）
            float fBaseAddRate = (pMapSlot != null && pMapSlot.HasFishPack() ? CGameColorFishMgr.Ins.pStaticConfig.GetInt("鱼群增加的倍率") : 100) * 0.01f;

            if (bGoldBoom)
            {
                fishInfo = CGameColorFishMgr.Ins.pMap.pTBLHandlerTreasureBombFishInfo.GetRandomVSFishInfo(emItemType, 0, 0, 0, fBaseAddRate, bMustGetChengSe);
            }
            else
            {
                fishInfo = CGameColorFishMgr.Ins.pMap.pTBLHandlerTreasureFishInfo.GetRandomVSFishInfo(emItemType, 0, 0, 0, fBaseAddRate, bMustGetChengSe);
            }

            info = new CFishInfo(fishInfo, 0, bHaveBianYi, bMustNoBianYi, bPayPlayer);
        }

        if (bShowFishInfo)
        {
            ShowVSFishInfo(info);
            dlgGetFish?.Invoke(info);
        }

        //RecordFishInfo(info);

        return info;
    }

    public void DoLagan(float createBullet = 0f)
    {
        if (UIManager.Instance.GetUI(UIResType.Help) as UIHelp != null &&
            UIHelp.emHelpLv == EMHelpLev.Lev2_Mo &&
            pInfo.emUserType == CPlayerBaseInfo.EMUserType.Zhubo)
        {
            UIHelp.GoNextHelpLv();
        }
        CFishInfo info = GetFish();

        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
        {
            DoBullet(info, createBullet);
        }
    }

    /// <summary>
    /// 创建子弹
    /// </summary>
    /// <param name="info"></param>
    /// <param name="createBullet"></param>
    public void DoBullet(CFishInfo info, float createBullet)
    {
        if (CBossBulletMgr.Ins == null ||
            CGameBossMgr.Ins == null) return;

        //发射子弹
        CTimeTickMgr.Inst.PushTicker(createBullet + Random.Range(-0.125f, 0.125f), delegate (object[] values)
        {
            if (CGameBossMgr.Ins == null ||
                CGameBossMgr.Ins.pBoss == null ||
               !CGameBossMgr.Ins.pBoss.IsAtkAble())
            {
                return;
            }

            ///判断是否打弱点
            if (nAtkIdx > 1)
            {
                CBossWeakBase boss304Weak = CGameBossMgr.Ins.pBoss.GetWeak(nAtkIdx);
                if (boss304Weak == null)
                {
                    CBossBulletMgr.Ins.AddBullet(
                                       new CHitBossInfo()
                                       {
                                           uid = this.uid,
                                           dmg = info.lPrice,
                                           szIcon = info.szIcon,
                                           emRare = info.emRare
                                       },
                                       tranNameRoot.position,
                                       CGameBossMgr.Ins.pBoss.GetRandHitPos(),
                                       CGameBossMgr.Ins.pBoss);
                }
                else
                {
                    CBossBulletMgr.Ins.AddBullet(
                                       new CHitBossInfo()
                                       {
                                           uid = this.uid,
                                           dmg = info.lPrice,
                                           szIcon = info.szIcon,
                                           emRare = info.emRare
                                       },
                                       tranNameRoot.position,
                                       boss304Weak.GetRandHitPos(),
                                       boss304Weak);
                }
            }
            else
            {
                CBossBulletMgr.Ins.AddBullet(
                                        new CHitBossInfo()
                                        {
                                            uid = this.uid,
                                            dmg = info.lPrice,
                                            szIcon = info.szIcon,
                                            emRare = info.emRare
                                        },
                                        tranNameRoot.position,
                                        CGameBossMgr.Ins.pBoss.GetRandHitPos(),
                                        CGameBossMgr.Ins.pBoss);
            }
        });
    }

    public void RecordFishInfo(CFishInfo info)
    {
        ST_FishInfo pTBLInfo = CGameColorFishMgr.Ins.pMap.pTBLHandlerFishInfo.GetInfo(info.nTBID);

        //判断是否记录
        if(pTBLInfo == null ||
          !pTBLInfo.bRecord)
        {
            return;
        }

        //判断是否记录大鱼信息
        if (info.emItemType == EMItemType.Fish &&
            info.emRare >= EMRare.Special)
        {
            //用户自己记录
            CHttpParam pReqUserParams = new CHttpParam(
                new CHttpParamSlot("uid", pInfo.uid.ToString()),
                new CHttpParamSlot("fishId", info.nTBID.ToString()),
                new CHttpParamSlot("count", 1.ToString())
            );

            CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddUserRareFishRecord, pReqUserParams);

            CHttpParam pReqRoomParams = new CHttpParam(
                    new CHttpParamSlot("uid", CDanmuSDKCenter.Ins.szRoomId.ToString()),
                    new CHttpParamSlot("roomId", CDanmuSDKCenter.Ins.szRoomId.ToString()),
                    new CHttpParamSlot("fishId", info.nTBID.ToString()),
                    new CHttpParamSlot("count", 1.ToString())
                );

            CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddRoomRareFishRecord, pReqRoomParams, 0, true);
        }

        //判断是否记录钓到主播
        if (CPlayerMgr.Ins.pOwner != null &&
            info.nTBID == 1000001)
        {
            CHttpParam pReqParams = new CHttpParam(
               new CHttpParamSlot("uid", pInfo.uid.ToString()),
               new CHttpParamSlot("userName", pInfo.userName),
               new CHttpParamSlot("userIcon", pInfo.userFace),
               new CHttpParamSlot("vtbUid", CPlayerMgr.Ins.pOwner.uid.ToString()),
               new CHttpParamSlot("vtbName", CPlayerMgr.Ins.pOwner.userName),
               new CHttpParamSlot("vtbIcon", CPlayerMgr.Ins.pOwner.userFace),
               new CHttpParamSlot("size", ((int)(info.fCurSize * 100F)).ToString())
           );

            CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddUserFishVtbRecord, pReqParams);
        }
    }

    /// <summary>
    /// 获取钓到的概率
    /// </summary>
    /// <returns></returns>
    EMItemType GetFishType(int nFishRate, int nPiFuRate, int nRandomRate, bool bMustGetChengSe = false,bool bIsBomber = false)
    {
        EMItemType getItemType = EMItemType.Fish;
        int nRandomValue = Random.Range(0, 101);
        //Debug.LogError(nRandomRate + "====Value =====" + nFishRate + "=====" + nPiFuRate + "=====" + nRandomValue);
        ///判断获取的概率
        nRandomValue -= nFishRate;
        if (nRandomValue <= 0)
        {
            getItemType = EMItemType.Fish;
            return getItemType;
        }
        nRandomValue -= nPiFuRate;
        if (nRandomValue <= 0)
        {
            getItemType = EMItemType.FishMat;
            if (bMustGetChengSe)
            {
                getItemType = EMItemType.Fish;
            }
            return getItemType;
        }
        if (nRandomRate > 0)
        {
            nRandomValue -= nRandomRate;
            if (nRandomValue <= 0)
            {
                getItemType = EMItemType.RandomEvent;
            }
            else
            {
                if (bIsBomber)
                {
                    getItemType = EMItemType.Fish;
                }
                else
                {
                    getItemType = EMItemType.Other;
                }
            }
        }
        else
        {
            if (bIsBomber)
            {
                getItemType = EMItemType.Fish;
            }
            else
            {
                getItemType = EMItemType.Other;
            }
        }
        if (bMustGetChengSe)
        {
            getItemType = EMItemType.Fish;
        }
        return getItemType;
    }

    /// <summary>
    /// 展示鱼的信息，并获取对应的鱼币
    /// </summary>
    /// <param name="info"></param>
    public void ShowFishInfo(CFishInfo info, bool bAddCoin, bool bShowRank, 
        EMFishCoinAddFunc emAddFunc, bool matPay, DelegateNFuncCall deleTest = null,bool bRecord = true)
    {
        SpecialCastInfo castInfo = new SpecialCastInfo();
        castInfo.playerInfo = pInfo;
        castInfo.fishInfo = info;

        ///判断获取的物品是否为稀有类型
        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
        {
            if ((int)info.emRare >= (int)EMRare.Shisi)
            {
                UISpecialCast.AddCastInfo(castInfo);
            }
        }
        else if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal ||
                CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema ||
                CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive ||
                CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
        {
            int nAddChampionCount = 0;
            if ((int)info.emRare >= (int)EMRare.Special &&
                bShowRank)
            {
                if (info.bBianYi)
                {
                    nAddChampionCount = CGameColorFishMgr.Ins.pStaticConfig.GetInt("变异红鱼加皇冠数量");
                }
                else
                {
                    nAddChampionCount = CGameColorFishMgr.Ins.pStaticConfig.GetInt("红鱼加皇冠数量");
                }
                CPlayerNetHelper.AddWinnerInfo(uid, nAddChampionCount, 0);
            }
            castInfo.nAddChampionCount = nAddChampionCount;
            if ((int)info.emRare >= (int)EMRare.Special &&
                bShowRank)
            {
                UISpecialCast.AddCastInfo(castInfo);
            }

            
        }

        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal ||
            CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema ||
            CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive ||
            CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
        {
            ///将该次钓鱼信息加入排名的列表
            UIRoomInfo roomInfo = UIManager.Instance.GetUI(UIResType.RoomInfo) as UIRoomInfo;
            if (roomInfo != null &&
                info.nTBID != 1000003)
            {
                roomInfo.rankRoot.AddPlayerInfo(castInfo);
            }

            if (bAddCoin && 
                info.lPrice > 0 && 
                !pInfo.bIsRobot)
            {
                AddCoinByHttp((matPay ? info.lPrice * CGameColorFishMgr.Ins.pStaticConfig.GetInt("渔具套装额外倍率") : info.lPrice),
                               EMFishCoinAddFunc.Pay, 
                               true,
                               bRecord);
            }

            if (bAddCoin &&
               info.nAddExp > 0 &&
               !pInfo.bIsRobot)
            {
                CPlayerNetHelper.AddUserExp(uid, Mathf.CeilToInt(info.nAddExp*info.fCurSizeRate));
            }

            ///判断是否获取了渔场材料
            if (info.emItemType == EMItemType.FishMat)
            {
                //CHttpParam pReqParams = new CHttpParam
                //(
                //   new CHttpParamSlot("uid", uid.ToString()),
                //   new CHttpParamSlot("itemId", info.nItemID.ToString()),
                //   new CHttpParamSlot("count", ((long)info.fCurSize).ToString()),
                //   new CHttpParamSlot("dailyGet", (matPay) ? "0":"1")
                //);

                //CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddUserFishMat, new HHandlerAddFishMat(), pReqParams, 0, true);
                CPlayerNetHelper.AddFishMat(uid, info.nItemID, (long)info.fCurSize, (matPay) ? 0 : 1, new HHandlerAddFishMat());
            }

            //判断是否钓到了稀有鱼
            if (info.nFesPack > 0 &&
                info.nFesPoint > 0)
            {
            //    CFishFesInfoMgr.Ins.AddFesPoint(info.nFesPack, info.nFesPoint, pInfo.uid);
            }
        }

        if (bShowRank)
        {
            ///将该次信息加入到右侧的信息展示列表
            UIShowRoot.AddCastInfo(castInfo);
        }

        /////判断是否钓到了皮肤类型的物品
        //if (info.emItemType == EMItemType.FishMat)
        //{
        //    AddPifuChip((int)info.fCurSize);
        //}

        //记录清单
        CFishRecordMgr.Ins.AddRecord(info, pInfo);
    }

    /// <summary>
    /// 展示鱼的信息，并获取对应的鱼币
    /// </summary>
    /// <param name="info"></param>
    void ShowVSFishInfo(CFishInfo info)
    {
        SpecialCastInfo castInfo = new SpecialCastInfo();
        castInfo.playerInfo = pInfo;
        castInfo.fishInfo = info;

        ///判断获取的物品是否为稀有类型
        if ((int)info.emRare >= (int)EMRare.Special)
        {
            UISpecialCast.AddCastInfo(castInfo);

            if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Battle)
            {
                CRankTreasureMgr.Ins.AddInfo(castInfo);
            }
        }

        //记录清单
        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Battle)
        {
            CFishRecordMgr.Ins.AddRecord(info, pInfo);
        }   
    }

    #region 网络协议

    /// <summary>
    /// 增加皮肤碎片（网络协议）
    /// </summary>
    /// <param name="nChipCount"></param>
    public void AddPifuChip(int nChipCount)
    {
        if (nChipCount <= 0) return;
        
        CLocalNetArrayMsg arrMsgUserList = new CLocalNetArrayMsg();
        CLocalNetMsg msgUserInfo = new CLocalNetMsg();
        msgUserInfo.SetString("uid", uid);
        msgUserInfo.SetLong("isVtb", (pInfo.emUserType == CPlayerBaseInfo.EMUserType.Zhubo) ? 1 : 0);

        long nFinnalAdd = nChipCount;
        msgUserInfo.SetLong("count", nFinnalAdd);
        arrMsgUserList.AddMsg(msgUserInfo);
        CLocalNetMsg msgReqContent = new CLocalNetMsg();
        msgReqContent.SetNetMsgArr("userList", arrMsgUserList);

        CHttpParam pReqParams = new CHttpParam(
            new CHttpParamSlot("roomId", CDanmuSDKCenter.Ins.szRoomId.ToString()),
            new CHttpParamSlot("userList", msgReqContent.GetData())
        );

        CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddAvatarSuipianArr, pReqParams, CHttpMgr.Instance.nReconnectTimes, true);
    }


    /// <summary>
    /// 增加金币（网络协议）
    /// </summary>
    /// <param name="nlValue"></param>
    public void AddCoinByHttp(long nlValue, EMFishCoinAddFunc func, bool addWinner, bool record = true)
    {
        //if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
        //    return;

        bool bVtb = pInfo.emUserType == CPlayerBaseInfo.EMUserType.Zhubo;

        //同步玩家使用道具的请求
        CPlayerNetHelper.AddFishCoin(uid,
                                     nlValue,
                                     func,
                                     addWinner);

        //记录房间收益
        if (record)
        {
            if (CPlayerMgr.Ins.pOwner != null)
            {
                CHttpParam pReqAddCoinParams = new CHttpParam(
                    new CHttpParamSlot("uid", CPlayerMgr.Ins.pOwner.uid.ToString()),
                    new CHttpParamSlot("count", nlValue.ToString())
                );

                CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddRoomFishGainCoin, pReqAddCoinParams);
            }
        }
    }

    /// <summary>
    /// 增加鱼饵（网络协议）
    /// </summary>
    /// <param name="nlValue"></param>
    public void AddBaitByHttp(long nlValue)
    {
        CHttpParam pReqParams = new CHttpParam
        (
           new CHttpParamSlot("uid", uid.ToString()),
           new CHttpParamSlot("fishItem", nlValue.ToString())
        );
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddBait, pReqParams, CHttpMgr.Instance.nReconnectTimes, true);
    }

    /// <summary>
    /// 增加鱼饵（网络协议）
    /// </summary>
    /// <param name="nlValue"></param>
    public void AddFreeBaitByHttp(long nlValue)
    {
        CHttpParam pReqParams = new CHttpParam
       (
           new CHttpParamSlot("uid", uid.ToString()),
           new CHttpParamSlot("fishItem", nlValue.ToString())
       );
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddFreeBait, pReqParams, CHttpMgr.Instance.nReconnectTimes, true);
    }

    /// <summary>
    /// 增加鱼杆/浮漂/飞轮/鱼线（网络协议）
    /// </summary>
    /// <param name="nlValue"></param>
    public void AddGiftByHttp(long nlValue,EMGiftType giftType)
    {
        long curTimeStamp = CGameColorFishMgr.Ins.GetNowServerTime();
        CHttpParam pReqParams = new CHttpParam
        (
           new CHttpParamSlot("uid", uid.ToString()),
           new CHttpParamSlot("itemType", giftType.ToString()),
           new CHttpParamSlot("count", nlValue.ToString()),
           new CHttpParamSlot("time", curTimeStamp.ToString()),
            new CHttpParamSlot("nonce", CEncryptHelper.AesEncrypt(uid.ToString() + CDanmuSDKCenter.Ins.szRoomId.ToString() + curTimeStamp))
        );
        CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddGiftCount, pReqParams, CHttpMgr.Instance.nReconnectTimes, true);
    }

    #endregion

    /// <summary>
    /// 获取当前钓鱼动作的级别
    /// </summary>
    /// <returns></returns>
    public int GetCurFinishValue()
    {
        int nIdx = 0;
        for(int i = 0;i < fFinishCheckValues.Length;i++)
        {
            if(fCurFinishValue <= fFinishCheckValues[i])
            {
                nIdx = i;
                break;
            }
        }
        return nIdx;
    }

    public void Init(CPlayerBaseInfo info)
    {
        nBoomAddRate = 0;
        pPreSlot = null;
        bDuelCD = false;
        pCheckDuelCD = null;
        ResetBattleValue();
        //fCheckExtTime = CGameColorFishMgr.Ins.pGameRoomConfig.nAutoExitTime;
        pInfo = info;
        if (pInfo.nVipPlayer == CPlayerBaseInfo.EMVipLv.Zongdu)
        {
            fCheckExtTime = CGameColorFishMgr.Ins.pStaticConfig.GetInt("总督待机时间") * 60;
        }
        else if (pInfo.nVipPlayer == CPlayerBaseInfo.EMVipLv.Tidu)
        {
            fCheckExtTime = CGameColorFishMgr.Ins.pStaticConfig.GetInt("提督待机时间") * 60;
        }
        else if (pInfo.nVipPlayer == CPlayerBaseInfo.EMVipLv.Pro)
        {
            fCheckExtTime = CGameColorFishMgr.Ins.pStaticConfig.GetInt("提督待机时间") * 60;
        }
        else
        {
            fCheckExtTime = CGameColorFishMgr.Ins.pGameRoomConfig.nAutoExitTime;
        }
        uid = pInfo.uid;
        tranSelf = GetComponent<Transform>();
        SetLaGanState(false);
        InitFSM();

        //加载Avatar
        InitAvatar();

        SetState(EMState.Idle);
        ///刚出生默认为活跃状态
        SetActiveState(true);

        objEffRich.SetActive(false);

        List<ProfitRankInfo> listShowInfos = CProfitRankMgr.Ins.GetRankInfos();
        if (listShowInfos.Count > 0)
        {
            if(uid == listShowInfos[0].nlUserUID)
            {
                objEffRich.SetActive(true);
            }
        }
        //pTicerCDTicker.Value = fCDZibeng;

        pSpecMgr.pOwner = this;
    }

    /// <summary>
    /// 验证码检测
    /// </summary>
    public void CheckYZM()
    {
        return;

        //if (CPlayerMgr.Ins.pOwner.uid != 559642)
        //    return;

        if (pInfo.CheckIsGrayName())
        {
            UIGameInfo show = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
            if (show != null)
            {
                UIShowUnitDialog unitInfo = show.GetShowDialogSlot(uid);
                if (unitInfo != null)
                {
                    unitInfo.SetGrayShow();
                }
            }
            
            return;
        }
        if (CPlayerMgr.Ins.pOwner.uid == uid)
            return;
        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Battle ||
            CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss)
        {
            return;
        }
        if (pInfo.GameCoins <= CGameColorFishMgr.Ins.pStaticConfig.GetInt("免验证码检测鱼币数量"))
        {
            return;
        }
        if (pInfo.nLv <= CGameColorFishMgr.Ins.pStaticConfig.GetInt("免验证码检测角色等级"))
        {
            return;
        }
        if (CGameColorFishMgr.Ins.nCurRateUpLv <= CGameColorFishMgr.Ins.pStaticConfig.GetInt("免验证码检测渔场等级"))
        {
            return;
        }

        if (pInfo.guardLevel == 1 ||
            pInfo.guardLevel == 2 ||
            pInfo.guardLevel == 3 ||
            pInfo.nVipPlayer == CPlayerBaseInfo.EMVipLv.Pro ||
            pInfo.nVipPlayer == CPlayerBaseInfo.EMVipLv.Tidu ||
            pInfo.nVipPlayer == CPlayerBaseInfo.EMVipLv.Zongdu)
        {
            return;
        }

        if (pInfo.CheckHaveGift())
        {
            return;
        }
        //加入单位的UI信息
        UIGameInfo uiShow = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (uiShow == null)
        {
            return;
        }
        UIShowUnitDialog uiUnitInfo = uiShow.GetShowDialogSlot(uid);
        if (uiUnitInfo == null)
            return; 
        
        ///判断是否出现验证码检测
        int nRandomRate = Random.Range(1, 101);
        if (nRandomRate <= fCheckYZMRate)
        {
            uiUnitInfo.SetDmContentByCheck();
        }
    }

    /// <summary>
    /// 初始化Avatar
    /// </summary>
    public virtual void InitAvatar()
    {
        if (pAvatar != null)
        {
            Destroy(pAvatar.gameObject);
            pAvatar = null;
            pAvatarFishGan = null;
        }

        ST_UnitAvatar pAvatarInfo = CTBLHandlerUnitAvatar.Ins.GetInfo(pInfo.avatarId);
        if (pAvatarInfo == null)
        {
            pAvatarInfo = CTBLHandlerUnitAvatar.Ins.GetInfo(101);
        }

        if (pAvatarInfo == null)
        {
            Debug.LogError("错误的Avatar信息:" + pInfo.avatarId);
            return;
        }

        CResLoadMgr.Inst.SynLoad(pAvatarInfo.szPrefab, CResLoadMgr.EM_ResLoadType.Role,
            delegate (Object res, object data, bool bSuc)
            {
                GameObject objAvatarRoot = res as GameObject;
                if (objAvatarRoot == null) return;

                GameObject objNewRole = GameObject.Instantiate(objAvatarRoot) as GameObject;
                Transform tranNewRole = objNewRole.GetComponent<Transform>();
                tranNewRole.SetParent(tranSelf);
                tranNewRole.localScale = Vector3.one * CGameColorFishMgr.Ins.fAvatarScale;
                tranNewRole.localPosition = Vector3.zero; ;
                tranNewRole.localRotation = Quaternion.identity;

                pAvatar = objNewRole.GetComponent<CPlayerAvatar>();
                //pAvatar.PlayJumpEff(emCurState == EMState.Jump);

                tranBody = pAvatar.transform;

                if(!string.IsNullOrEmpty(szAnima))
                {
                    PlayAnime(szAnima);

                    if(emCurState == EMState.Fishing)
                    {
                        pAvatar.PlaySpringMgr(false);
                    }
                }

                InitFishGan(pInfo.nFishGanAvatarId);
            });
    }

    public virtual void InitFishGan(int id)
    {
        if (pAvatar == null) return;

        //移除旧有的钓鱼竿
        if (pAvatarFishGan != null)
        {
            pAvatar.RemoveHandObj(pAvatarFishGan);
            pAvatarFishGan = null;
        }

        ST_UnitFishGan pTBLGanInfo = CTBLHandlerUnitFishGan.Ins.GetInfo(id);
        if (pTBLGanInfo == null)
        {
            pTBLGanInfo = CTBLHandlerUnitFishGan.Ins.GetInfo(101);
        }

        if (pTBLGanInfo == null)
        {
            Debug.LogError("错误的FishGan信息:" + id);
            return;
        }

        CResLoadMgr.Inst.SynLoad(pTBLGanInfo.szPrefabPath + pTBLGanInfo.GetLv(0).szPrefab, CResLoadMgr.EM_ResLoadType.Role,
           delegate (Object res, object data, bool bSuc)
           {
               GameObject objAvatarRoot = res as GameObject;
               if (objAvatarRoot == null) return;

               GameObject objNewRole = GameObject.Instantiate(objAvatarRoot) as GameObject;
               Transform tranNewRole = objNewRole.GetComponent<Transform>();
               tranNewRole.SetParent(pAvatar.tranMainHandRoot);
               tranNewRole.localScale = Vector3.one;
               tranNewRole.localPosition = Vector3.zero; ;
               tranNewRole.localRotation = Quaternion.identity;

               pAvatar.AddHandObj(objNewRole);
               pAvatarFishGan = objNewRole;

               CFishGanEffWithValue pEffRoot = pAvatarFishGan.GetComponent<CFishGanEffWithValue>();
               if(pEffRoot!=null)
               {
                   pEffRoot.RefreshValue(pInfo.GetProAddValue(pTBLGanInfo.emProType));
               }
           });
    }

    /// <summary>
    /// 设置活跃状态
    /// </summary>
    /// <param name="active"></param>
    public void SetActiveState(bool active)
    {
        bActive = active;
        if(bActive)
        {
            fFinishStayTime = CGameColorFishMgr.Ins.pStaticConfig.GetInt("活跃钓鱼时间");
            //if(pCheckActiveTick == null)
            //{
            //    pCheckActiveTick = new CPropertyTimer();
            //    pCheckActiveTick.Value = fCheckActiveTime;
            //    pCheckActiveTick.FillTime();
            //}
            //else
            //{
            //    pCheckActiveTick.FillTime();
            //}
            CPlayerBaseInfo pPlayer = CPlayerMgr.Ins.GetPlayer(uid);
            if (pPlayer != null)
            {
                if (pPlayer.CheckHaveGift())
                {
                    ClearExitTick();
                }
                else
                {
                    ResetExitTick();
                }
            }
        }
        else
        {
            fFinishStayTime = CGameColorFishMgr.Ins.pStaticConfig.GetInt("非活跃钓鱼时间");
        }
        dlgChgActiveState?.Invoke(bActive);
    }

    protected virtual void InitFSM()
    {
        pFSM = new FSMManager(this);
        pFSM.AddState((int)EMState.Born, new FSMUnitBorn());
        pFSM.AddState((int)EMState.Idle, new FSMUnitIdle());
        pFSM.AddState((int)EMState.Jump, new FSMUnitJump());
        pFSM.AddState((int)EMState.Move, new FSMUnitMove());
        pFSM.AddState((int)EMState.StartFish, new FSMUnitStartFish());
        pFSM.AddState((int)EMState.Fishing, new FSMUnitFishing());
        pFSM.AddState((int)EMState.EndFish, new FSMUnitEndFish());
        pFSM.AddState((int)EMState.ShowFish, new FSMUnitShowFish());
        pFSM.AddState((int)EMState.Battle, new FSMUnitBattle());
        pFSM.AddState((int)EMState.Drag, new FSMUnitDrag());
        pFSM.AddState((int)EMState.DragCancel, new FSMUnitDragCancel());
        pFSM.AddState((int)EMState.HitDrop, new FSMUnitHitByTentacle());

        pFSM.AddState((int)EMState.BossEat, new FSMUnitBossEat());
        pFSM.AddState((int)EMState.BossWait, new FSMUnitBossWait());
        pFSM.AddState((int)EMState.BossReturn, new FSMUnitBossReturn());
        pFSM.AddState((int)EMState.GunShootBoss, new FSMUnitGunShootBoss());
        pFSM.AddState((int)EMState.RPGShootBoss, new FSMUnitRPGShootBoss());

        pFSM.AddState((int)EMState.BossEatShow, new FSMUnitBossEatShow());
        pFSM.AddState((int)EMState.BossWaitShow, new FSMUnitBossWaitShow());
        pFSM.AddState((int)EMState.BossReturnShow, new FSMUnitBossReturnShow());
    }

    private void Update()
    {
        if (pInfo == null)
            return;
        if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal ||
           CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema ||
           CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss ||
           CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive ||
           CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
        {
            if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle &&
               CPlayerMgr.Ins.GetAllIdleUnit().Count < CGameColorFishMgr.Ins.pStaticConfig.GetInt("可踢人判断数量"))
            {

            }
            else
            {
                if (pCheckExtTick != null &&
                    pCheckExtTick.Tick(CTimeMgr.DeltaTime))
                {
                    if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Normal ||
                        CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Cinema ||
                        CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
                    {
                        ///判断该玩家身上是否还存在礼物
                        if (pInfo != null &&
                            pInfo.CheckHaveGift())
                        {
                            ResetExitTick();
                            return;
                        }
                    }
                    else if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Boss ||
                             CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Survive)
                    {
                        ResetExitTick();
                        return;
                    }

                    if (listNormalBooms.Count > 0 ||
                        listTreasureBooms.Count > 0)
                    {
                        ResetExitTick();
                        return;
                    }

                    if (pInfo.nFishGanAvatarId != 101)
                    {
                        ResetExitTick();
                        return;
                    }

                    ///正在拍卖的人无法上岸
                    if (CHelpTools.CheckAuctionByUID(uid))
                    {
                        ResetExitTick();
                        return;
                    }

                    if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle &&
                        CPlayerMgr.Ins.GetAllIdleUnit().Count < CGameColorFishMgr.Ins.pStaticConfig.GetInt("可踢人判断数量"))
                    {
                        ResetExitTick();
                        return;
                    }

                    ClearExitTick();
                    //时间到了自动发送离开游戏的弹幕
                    SendExitDM();
                }
            }
        }
        else if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Battle)
        {
            if (listNormalBooms.Count <= 0 &&
                pCheckExtTick == null)
            {
                ResetExitTick();
                return;
            }

            if(pCheckExtTick!=null && 
               pCheckExtTick.Tick(CTimeMgr.DeltaTime))
            {
                if (listNormalBooms.Count > 0 ||
                    listTreasureBooms.Count > 0)
                {
                    ResetExitTick();
                    return;
                }

                ClearExitTick();
                //时间到了自动发送离开游戏的弹幕
                SendExitDM();
            }
        }
      
        UpdateBoom();

        UpdateTreasureBoom();

        if(pCheckDuelCD != null &&
           pCheckDuelCD.Tick(CTimeMgr.DeltaTime))
        {
            pCheckDuelCD = null;
            bDuelCD = false;
        }

        if(pTicerCDZibeng != null &&
           pTicerCDZibeng.Tick(CTimeMgr.DeltaTime))
        {
            pTicerCDZibeng = null;
        }

        //if(bActive && pCheckActiveTick != null)
        //{
        //    if(pCheckActiveTick.Tick(CTimeMgr.DeltaTime))
        //    {
        //        SetActiveState(false);
        //        pCheckActiveTick = null;
        //    }
        //}
        //CheckContinuosGain(CTimeMgr.DeltaTime);

        if (pFSM!=null)
        {
            pFSM.Update(CTimeMgr.DeltaTime);
        }
    }

    public void SendExitDM()
    {
        if (listNormalBooms.Count > 0 ||
            listTreasureBooms.Count > 0)
            return;

        CDanmuChat dm = new CDanmuChat();
        dm.uid = uid;
        dm.nickName = pInfo.userName;
        dm.headIcon = pInfo.userFace;
        dm.content = CDanmuEventConst.ExitGame;
        dm.roomId = CDanmuSDKCenter.Ins.szRoomId;
        dm.Mock();
    }

    public void ResetExitTick()
    {
        if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Battle)
        {
            pCheckExtTick = new CPropertyTimer();
            pCheckExtTick.Value = 6F;
            pCheckExtTick.FillTime();
        }
        else if(fCheckExtTime > 0 && bAutoExit)
        {
            pCheckExtTick = new CPropertyTimer();
            pCheckExtTick.Value = fCheckExtTime;
            pCheckExtTick.FillTime();
        }
    }

    public void SetExitTick(int nExitTime)
    {
        if (pCheckExtTick == null) return;
        if(pCheckExtTick.CurValue > nExitTime)
        {
            pCheckExtTick.CurValue = nExitTime;
        }
    }

    public void ClearExitTick()
    {
        pCheckExtTick = null;
    }

    private void FixedUpdate()
    {
        if (pFSM != null)
        {
            pFSM.FixedUpdate(CTimeMgr.FixedDeltaTime);
        }
    }

    public virtual void SetState(EMState state, CLocalNetMsg msg = null)
    {
        dlgChgState?.Invoke(state);
        pFSM.ChangeMainState((int)state, msg);
    }

    public virtual void PlayAnime(string anime, float crossfade = 0.1F, float startlerp = 0F)
    {
        if (pAvatar == null) return;

        szAnima = anime;
        pAvatar.PlayAnime(anime, crossfade, startlerp);
        //if (pAnime == null) return;

        //pAnime.CrossFade(anime, crossfade, 0, startlerp);
    }

    public virtual void PlayMatAnime(string anime)
    {
        if (pAvatar == null) return;

        pAvatar.PlayMatAnime(anime);
        //if (pAnimeMat == null) return;

        //for(int i=0; i<pAnimeMat.Length; i++)
        //{
        //    pAnimeMat[i].Play(anime);
        //}
    }

    public void SetDir(Vector3 dir)
    {
        transform.forward = dir;
    }

    /// <summary>
    /// 兹蹦进CD
    /// </summary>
    public void DoZibengCD()
    {
        pTicerCDZibeng = new CPropertyTimer();
        pTicerCDZibeng.Value = fCDZibeng;
        pTicerCDZibeng.FillTime();
    }

    /// <summary>
    /// 判断兹蹦是否在CD中
    /// </summary>
    /// <returns></returns>
    public bool IsZibengCD()
    {
        return (pTicerCDZibeng != null);
    }

    public virtual void Recycle()
    {
        if(pMapSlot!=null)
        {
            pMapSlot.SetBoat(101, null, 0);
        }

        if (pMapSlot != null &&
            pMapSlot.pBindUnit == this)
        {
            pMapSlot.BindPlayer(null);
        }

        if(pPreSlot != null &&
           pPreSlot.pBindUnit == this)
        {
            pPreSlot.BindPlayer(null);
        }

        dlgRecycle?.Invoke();
    }
}
