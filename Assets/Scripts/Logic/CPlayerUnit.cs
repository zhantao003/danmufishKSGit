using OpenBLive.Runtime.Data;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ҵ�λ
/// </summary>
public class CPlayerUnit : MonoBehaviour
{
    /// <summary>
    /// ը����Ϣ
    /// </summary>
    public class BoomInfo
    {
        public bool bPay;      //�Ƿ�Ϊ����ը��
    }

    /// <summary>
    /// �û�UID
    /// </summary>
    public string uid;

    //�û���Ϣ
    public CPlayerBaseInfo pInfo;

    public CPlayerUnitSpecMgr pSpecMgr;

    public CMapSlot pMapSlot;

    [HideInInspector]
    public Transform tranSelf;
    public Transform tranBody;
    [Header("��Ծ�ٶ�")]
    public float fJumpSpeed;
    [Header("��Ծ�߶�")]
    public float fJumpHeight;

    public string szBornEff;
    public enum EMState
    { 
        Idle,               //����
        Move,               //�ƶ�
        StartFish,          //��ʼ����
        Fishing,            //������
        EndFish,            //��������
        ShowFish,           //չʾ��������
        Jump,               //��Ծ
        Battle,             //����
        Drag,               //��ק
        DragCancel,         //��קȡ����״̬
        Wait,
        Start,
        Born,
        HitDrop,            //�������ϰ�

        BossEat,            //��Boss�Ե���
        BossWait,           //�ȴ�����
        BossReturn,         //����
        GunShootBoss,       //�ȱ������Boss
        RPGShootBoss,       //RPG�����Boss

        BossEatShow,            //��Boss�Ե���
        BossWaitShow,           //�ȴ�����
        BossReturnShow,         //����
    }

    /// <summary>
    /// ��ǰ��״̬
    /// </summary>
    public EMState emCurState = EMState.Idle;
    /// <summary>
    /// ״̬��
    /// </summary>
    public FSMManager pFSM;
    public Animator[] pAnimeMat;

    public CPlayerAvatar pAvatar;

    public GameObject pAvatarFishGan;

    [Header("�ƶ�������ִ�еĶ���")]
    public EMState pEndMoveState;
    [Header("�ƶ���Ŀ���")]
    public Vector3 vecMoveTarget;
    [Header("���浱ǰ�ĵ��㶯������")]
    public float fCurFinishValue;
    [Header("��ʼ����Ķ�������ʱ��")]
    public float fFinishStartTime;
    [Header("����������ʱ��")]
    public float fFinishStayTime;
    [Header("��ɵ���Ķ�������ʱ��")]
    public float fFinishEndTime;
    [Header("չʾ�������������ʱ��")]
    public float fShowFishTime;
    [Header("���㶯�����ݲ�ͬ���Ƚ��в�ͬ�ļ���")]
    public float[] fFinishCheckValues;
    [Header("����Ч��")]
    public AddInfo pAddInfo;

    [Header("����ļӿ��ٶ�")]
    public float fFishSpeed;
    [Header("��������ĸ�������")]
    public int nAddFishRate;
    [Header("�����������ĸ���")]
    public int nAddBigFishRate;
    [Header("���ӵ������ϡ����Ʒ�ĸ���")]
    public int nAddRareRate;

    [Header("�ж��Ƿ�Ϊ��Ծ���")]
    public bool bActive;
    [Header("�л���������״̬��ʱ��")]
    public Vector2 vecChgLaGanStateTimeRange = new Vector2(40, 80);
    [Header("����λ��")]
    public Transform tranGachePos;
    [Header("ը��λ��")]
    public Transform tranBoomFishPos;
    [Header("��֤�������")]
    public float fCheckYZMRate = 30;

    public GameObject objEffRich;

    /// <summary>
    /// �ж��Ƿ��������
    /// </summary>
    public bool bCanLaGan;

    /// <summary>
    /// ��Ծ״̬�ж�ʱ��
    /// </summary>
    public float fCheckActiveTime = 600;
    /// <summary>
    /// ��Ծ״̬��ʱ��
    /// </summary>
    CPropertyTimer pCheckActiveTick;

    public bool bAutoExit = true;

    /// <summary>
    /// �뿪״̬�ж�ʱ��
    /// </summary>
    public float fCheckExtTime = 3600;
    /// <summary>
    /// �뿪״̬��ʱ��
    /// </summary>
    CPropertyTimer pCheckExtTick;

    public string szAnima;

    //public EPOOutline.Outlinable[] arrOutLine;

    #region UI���

    public Transform tranNameRoot;  //�����ı�

    #endregion

    #region �����¼�

    public DelegateNFuncCall dlgRecycle;            //�����¼�

    public DelegateStateFuncCall dlgChgState;       //״̬�仯�¼�

    public DelegateFFuncCall dlgChgFishValue;       //�ı䵱ǰ�������

    public DelegateFishFuncCall dlgGetFish;         //��ȡ��

    public DelegateBFuncCall dlgChgActiveState;     //�ı��Ծ״̬

    public DelegateBFuncCall dlgChgLaGanState;     //�ı������״̬

    public DelegateNFuncCall dlgChgGift;            //���������ı�

    public DelegateNFuncCall dlgShowAddCoin;        //չʾ���ӵ����

    public DelegateLLFuncCall dlgOnlySetAddCoin;     //��չʾ������

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
    /// ը������
    /// </summary>
    public List<BoomInfo> listNormalBooms = new List<BoomInfo>();

    /// <summary>
    /// ����ը������
    /// </summary>
    public List<BoomInfo> listTreasureBooms = new List<BoomInfo>();

    public float fShowBoomFishDelay = 0.05F;

    public float fCheckBoomFishLerp = 1F;

    /// <summary>
    /// �Ѿ�ʹ�õ�ը������
    /// </summary>
    public int nHaveUseBoom = 0;

    /// <summary>
    /// ը�����׼�������
    /// </summary>
    public int nBaoDiPayBoom = 0;

    /// <summary>
    /// ���ը�����
    /// </summary>
    CPropertyTimer pCheckBoomFishTick;

    /// <summary>
    /// ���ը�����
    /// </summary>
    CPropertyTimer pCheckBoomTreasureTick;

    /// <summary>
    /// ������CD
    /// </summary>
    CPropertyTimer pCheckDuelCD;

    public bool bDuelCD;

    public bool bCheckYZM;

    public bool bTestData;

    public int nAtkIdx;

    /// <summary>
    /// ֮ǰ���ŵĸ���
    /// </summary>
    public CMapSlot pPreSlot;

    public DelegateNFuncCall pShowFishEndEvent;

    //��ʱ��Ծ�����¼�
    public DelegateNFuncCall dlgCallOnceJumpEvent;

    /// <summary>
    /// ��������Ŀ����
    /// </summary>
    int nBattleCheckTarget;
    /// <summary>
    /// �������׵�ǰ����
    /// </summary>
    int nBattleCurValue;

    //�ȱ�CD
    public float fCDZibeng;
    public CPropertyTimer pTicerCDZibeng = new CPropertyTimer();

    public int nBoomAddRate;

    /// <summary>
    /// ���þ������׼���
    /// </summary>
    public void ResetBattleValue()
    {
        nBattleCurValue = 0;
        nBattleCheckTarget = Random.Range(CGameColorFishMgr.Ins.pStaticConfig.GetInt("����������Сֵ"),
                                          CGameColorFishMgr.Ins.pStaticConfig.GetInt("�����������ֵ") + 1);

        //Debug.Log("ResetBattle ====" + nBattleCurValue);
    }

    /// <summary>
    /// ���Ӿ������׼���
    /// </summary>
    public void AddBattleValue()
    {
        nBattleCurValue += 1;
        //Debug.Log("AddBattle ====" + nBattleCurValue);
    }

    /// <summary>
    /// �Ƿ�Ϊ��������
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
        pCheckDuelCD.Value = CGameColorFishMgr.Ins.pStaticConfig.GetInt("���������ȴʱ��") +
                             CGameColorFishMgr.Ins.pStaticConfig.GetInt("����ʱ��") +
                             CGameColorFishMgr.Ins.pStaticConfig.GetInt("�������㵹��ʱ");
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
            Debug.LogError("�����ͼ����");
        }
    }

    /// <summary>
    /// ����֮ǰ���ŵĸ���
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
    /// ����Ŀ��
    /// </summary>
    /// <param name="tranTarget"></param>
    public void JumpTarget(CMapSlot mapSlot,bool bCancelBindUnit = true)
    {
        //����
        if (pMapSlot != null)
        {
            pMapSlot.SetBoat(101, null, 0);
        }

        SetMapSlot(mapSlot, bCancelBindUnit);

        if (mapSlot != null)
        {
            //ִ��ռ���ӵĶ���
            CLocalNetMsg msgJumpInfo = new CLocalNetMsg();
            msgJumpInfo.SetFloat("posX", mapSlot.tranSelf.position.x);
            msgJumpInfo.SetFloat("posY", mapSlot.tranSelf.position.y);
            msgJumpInfo.SetFloat("posZ", mapSlot.tranSelf.position.z);
            if (bCancelBindUnit)
            {
                SetState(EMState.Jump, msgJumpInfo);

                //����
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

    #region ��ͨը��

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

        //�����Bossս,Boss���ڲ��ɹ���״̬��ֹͣ��ը��
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
            bool goldBoom = Random.Range(0, 101) <= CGameColorFishMgr.Ins.pStaticConfig.GetInt("��ը�����ָ���");
            bool pay = listNormalBooms[0].bPay;
            int Count = CGameColorFishMgr.Ins.pStaticConfig.GetInt("��ը����С����");// Random.Range(CGameColorFishMgr.Ins.pStaticConfig.GetInt("��ը����С����"), CGameColorFishMgr.Ins.pStaticConfig.GetInt("��ը���������") + 1);
            ShowBoomFish(Count, goldBoom, pay);
            return;
        }

        if (string.IsNullOrEmpty(szBoomEffect)) return;
        
        bool bGoldBoom = Random.Range(0, 101) <= CGameColorFishMgr.Ins.pStaticConfig.GetInt("��ը�����ָ���");
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
                if(nBaoDiPayBoom >= CGameColorFishMgr.Ins.pStaticConfig.GetInt("��ը�����ֱ�������"))
                {
                    nBaoDiPayBoom = 0;
                    bGoldBoom = true;
                }
            }
        }
        //������Ч
        Vector3 vFwdDir = tranSelf.forward;
        vFwdDir.y = 0;
        vFwdDir = vFwdDir.normalized;
        CEffectMgr.Instance.CreateEffSync(szBoomEffect + (bGoldBoom ? "Gold" : ""), tranSelf.position, Quaternion.LookRotation(vFwdDir, Vector3.up), 0);

        int nCount = CGameColorFishMgr.Ins.pStaticConfig.GetInt("��ը����С����");// Random.Range(CGameColorFishMgr.Ins.pStaticConfig.GetInt("��ը����С����"), CGameColorFishMgr.Ins.pStaticConfig.GetInt("��ը���������") + 1);
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
        bool bGetChengSeFish = nHaveUseBoom >= CGameColorFishMgr.Ins.pStaticConfig.GetInt("ը�����׳���ɫ����");
        //if (bGetChengSeFish)
        //{
        //    Debug.LogError("Must Get ChengSe");
        //    //nHaveUseBoom = 0;
        //}
        bool bHaveBianYi = false;

        CBomberCountInfo bomberCountInfo = null;        //ը��������Ϣ
        if(bPay)
        {
            bomberCountInfo = CBomberCountMgr.Count(1);
            if (bomberCountInfo != null)
            {
                Debug.Log($"��{bomberCountInfo.nIdx}��  ������<color=#F02000>{bomberCountInfo.szName}</color>");
            }
        }

        int nGetFishMatByGold = 0;
        if(bGoldBoom)
        {
            nGetFishMatByGold = CGameColorFishMgr.Ins.pStaticConfig.GetInt("��ը���س����ϴ���");
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

        //�����
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

        //�Ӿ���
        if(nFinalAddExp > 0)
        {
            CPlayerNetHelper.AddUserExp(uid, nFinalAddExp);
        }
    }

    /// <summary>
    /// ը��ģʽ�µ�ը������
    /// </summary>
    /// <param name="nCount"></param>
    /// <param name="bGoldBoom"></param>
    /// <param name="bPay"></param>
    void ShowBoomFishVsModel(int nCount, bool bGoldBoom, bool bPay)
    {
        UIGameInfo gameInfo = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (gameInfo == null) return;

        int nGiftTBID = 0;        //ը��������Ϣ
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

                //�ж��Ƿ���Ҫ���ü�����
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

            //��ӱ��ػ���
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
            //���ӱ��ػ���
            //CHttpParam pReqParams = new CHttpParam
            //(
            //    new CHttpParamSlot("uid", uid.ToString()),
            //    new CHttpParamSlot("treasurePoint", nAddTreasurePoint.ToString())
            //);
            //CHttpMgr.Instance.SendHttpMsg(CHttpConst.AddUserTreasurePoint, pReqParams, 10, true);

            CPlayerNetHelper.AddTreasureCoin(uid, nAddTreasurePoint);

            //����ͬ�����ӱ��ػ���
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

    #region ����ը��

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
        bool bGoldBoom = Random.Range(0, 101) <= CGameColorFishMgr.Ins.pStaticConfig.GetInt("��ը�����ָ���");
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
                if (nBaoDiPayBoom >= CGameColorFishMgr.Ins.pStaticConfig.GetInt("��ը�����ֱ�������"))
                {
                    nBaoDiPayBoom = 0;
                    bGoldBoom = true;
                }
            }
        }

        //������Ч
        Vector3 vFwdDir = tranSelf.forward + tranSelf.right;
        vFwdDir.y = 0;
        vFwdDir = vFwdDir.normalized;
        CEffectMgr.Instance.CreateEffSync(szTreasureBoomEffect, tranSelf.position, Quaternion.LookRotation(vFwdDir, Vector3.up), 0);

        int nCount = CGameColorFishMgr.Ins.pStaticConfig.GetInt("��ը����С����");// Random.Range(CGameColorFishMgr.Ins.pStaticConfig.GetInt("��ը����С����"), CGameColorFishMgr.Ins.pStaticConfig.GetInt("��ը���������") + 1);
        CTimeTickMgr.Inst.PushTicker(0.9F, delegate (object[] values)
        {
            ShowBoomFishVsModel(nCount - 1, bGoldBoom, bPay);
        });
    }

    #endregion

    #region PlayAudio

    /// <summary>
    /// ���ſ�ʼ�������Ч
    /// </summary>
    public void PlayStartFishAudio()
    {
        if (pStartFishAudio != null)
        {
            CAudioMgr.Ins.PlaySoundBySlot(pStartFishAudio, transform.position);
        }
    }

    /// <summary>
    /// ��Ŀ�궪������Ч
    /// </summary>
    /// <param name="tranTarget"></param>
    public void PlayGiftEffectToTarget(int nValue)
    {
        if (CGameColorFishMgr.Ins.pMap == null ||
            CGameColorFishMgr.Ins.pMap.pDuelBoat == null ||
            CGameColorFishMgr.Ins.pMap.pDuelBoat.giftUnit == null)
            return;
        if (CGameColorFishMgr.Ins.pMap.pDuelBoat.emCurState == CDuelBoat.EMState.Hide) //���������ؽ׶�
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
    /// ��Ŀ�궪�����Ч
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
    /// �������������Ч�Ͷ�Ӧ����Ч
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
    /// ���Ž����������Ч
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
    /// ��������״̬
    /// </summary>
    /// <param name="laGan"></param>
    public void SetLaGanState(bool laGan)
    {
        bCanLaGan = laGan;
        dlgChgLaGanState?.Invoke(bCanLaGan);
    }

    /// <summary>
    /// ��ȡ�����������Ϣ
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
            //pAddInfo.nAddRandomRate += CGameColorFishMgr.Ins.pStaticConfig.GetInt("�����װ������¼�����"); 
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
    /// �����(����)
    /// </summary>
    public CFishInfo GetBattleFish()
    {
        ///�ж��Ƿ�Ϊ�������
        bool bPayPlayer = false;
        bPayPlayer = GetGiftAddInfo(true);
        //pAddInfo.Clear();
        ///�����ܼӳ�
        int addFishRate = pAddInfo.nAddFishRate + CGameColorFishMgr.Ins.pStaticConfig.GetInt("���������ȡ��ĸ���");
        int addFishRareRate = pAddInfo.nAddFishRareRate;
        int addZawuRareRate = pAddInfo.nAddZawuRareRate;
        int addFishMatRareRate = pAddInfo.nAddFishMatRareRate;
        int addBigFishRate = pAddInfo.nAddBigFishRate;
        int addFishMatRate = pAddInfo.nAddFishMatRate + CGameColorFishMgr.Ins.pStaticConfig.GetInt("���������ȡ���ϵĸ���");
        int addRandomEvent = pAddInfo.nAddRandomRate + CGameColorFishMgr.Ins.pStaticConfig.GetInt("���������ȡ����¼��ĸ���");

        if (CCrazyTimeMgr.Ins != null)
        {
            if (CCrazyTimeMgr.Ins.bCrazy)
            {
                addFishRareRate += CGameColorFishMgr.Ins.pStaticConfig.GetInt("��ʱ���ϡ�������");
            }
            if (CCrazyTimeMgr.Ins.bKongTou)
            {
                addFishRareRate += CGameColorFishMgr.Ins.pStaticConfig.GetInt("��Ͷʱ���ϡ�������");
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
        ///��ȡ�ôε���Ļ�ȡ������
        //EMItemType emItemType = GetFishType(addFishRate, addPiFuRate, addRandomEvent);
        EMItemType emItemType = EMItemType.Fish;
        ///��ȡ��������Ϣ
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
            ///�������ӱ��ʣ��ж��Ƿ�����Ⱥ��
            float fBaseAddRate = ((pMapSlot != null && pMapSlot.HasFishPack()) ? CGameColorFishMgr.Ins.pStaticConfig.GetInt("��Ⱥ���ӵı���") : 100) * 0.01f;

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

        ////���⴦��㲥
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
        //Debug.Log("����㣺" + fishInfo.szName + "," + fishInfo.fCurSize + "cm," + fishInfo.GetPrice() + "���===" + castInfo.fishInfo.fCurSize);
    }


    /// <summary>
    /// �����
    /// </summary>
    public CFishInfo GetFish(bool bShowFishInfo = true, bool bIsBomber = false, bool bGoldBoom = false, bool bMustGetChengSe = false, bool bHaveBianYi = false, bool bMustNoBianYi = false,bool bPay = false,bool bMustFishMat = false, bool bFirstBoom =false)
    {
        ///�ж��Ƿ�Ϊ�������
        bool bPayPlayer = false;

        bool bAddFishPay = pInfo.nlBaitCount > 0 ||
                           pInfo.nlFeiLunCount > 0;

        bool bAddMatPay = pInfo.nlBaitCount > 0;

        if (bShowFishInfo)
        {
            //Bossս׼���׶β�����
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

        ///�����ܼӳ�
        int addFishRate = pAddInfo.nAddFishRate + CGameColorFishMgr.Ins.pStaticConfig.GetInt("���������ȡ��ĸ���");
        int addFishRareRate = pAddInfo.nAddFishRareRate;
        int addZawuRareRate = pAddInfo.nAddZawuRareRate;
        int addFishMatRareRate = pAddInfo.nAddFishMatRareRate;
        int addBigFishRate = pAddInfo.nAddBigFishRate;
        int addFishMatRate = pAddInfo.nAddFishMatRate + CGameColorFishMgr.Ins.pStaticConfig.GetInt("���������ȡ���ϵĸ���");
        int addRandomEvent = pAddInfo.nAddRandomRate + CGameColorFishMgr.Ins.pStaticConfig.GetInt("���������ȡ����¼��ĸ���");

        if (CCrazyTimeMgr.Ins != null)
        {
            if (CCrazyTimeMgr.Ins.bCrazy)
            {
                addFishRareRate += CGameColorFishMgr.Ins.pStaticConfig.GetInt("��ʱ���ϡ�������");
            }
            if (CCrazyTimeMgr.Ins.bKongTou)
            {
                addFishRareRate += CGameColorFishMgr.Ins.pStaticConfig.GetInt("��Ͷʱ���ϡ�������");
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

        ///��ȡ�ôε���Ļ�ȡ������
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
        ///��ȡ��������Ϣ

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
            ///�������ӱ��ʣ��ж��Ƿ�����Ⱥ��
            float fBaseAddRate = (pMapSlot != null && pMapSlot.HasFishPack() ? CGameColorFishMgr.Ins.pStaticConfig.GetInt("��Ⱥ���ӵı���") : 100) * 0.01f;
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

        //�ж��Ƿ�ϡ����
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

        //����Ǻ���ͼӼ�������
        if(info.emRare >= EMRare.Special)
        {
            if(CCrazyTimeMgr.Ins!=null)
            {
                CCrazyTimeMgr.Ins.AddBattery(1);
            }
        }

        return info;
        //Debug.Log("����㣺" + fishInfo.szName + "," + fishInfo.fCurSize + "cm," + fishInfo.GetPrice() + "���===" + castInfo.fishInfo.fCurSize);
    }

    public CFishInfo GetFishVsModel(bool bShowFishInfo = true, bool bGoldBoom = false, bool bMustGetChengSe = false, bool bHaveBianYi = false, bool bMustNoBianYi = false, bool bPay = false)
    {
        ///�ж��Ƿ�Ϊ�������
        bool bPayPlayer = false;

        EMItemType emItemType = EMItemType.Fish;

        ///��ȡ��������Ϣ
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
            ///�������ӱ��ʣ��ж��Ƿ�����Ⱥ��
            float fBaseAddRate = (pMapSlot != null && pMapSlot.HasFishPack() ? CGameColorFishMgr.Ins.pStaticConfig.GetInt("��Ⱥ���ӵı���") : 100) * 0.01f;

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
    /// �����ӵ�
    /// </summary>
    /// <param name="info"></param>
    /// <param name="createBullet"></param>
    public void DoBullet(CFishInfo info, float createBullet)
    {
        if (CBossBulletMgr.Ins == null ||
            CGameBossMgr.Ins == null) return;

        //�����ӵ�
        CTimeTickMgr.Inst.PushTicker(createBullet + Random.Range(-0.125f, 0.125f), delegate (object[] values)
        {
            if (CGameBossMgr.Ins == null ||
                CGameBossMgr.Ins.pBoss == null ||
               !CGameBossMgr.Ins.pBoss.IsAtkAble())
            {
                return;
            }

            ///�ж��Ƿ������
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

        //�ж��Ƿ��¼
        if(pTBLInfo == null ||
          !pTBLInfo.bRecord)
        {
            return;
        }

        //�ж��Ƿ��¼������Ϣ
        if (info.emItemType == EMItemType.Fish &&
            info.emRare >= EMRare.Special)
        {
            //�û��Լ���¼
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

        //�ж��Ƿ��¼��������
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
    /// ��ȡ�����ĸ���
    /// </summary>
    /// <returns></returns>
    EMItemType GetFishType(int nFishRate, int nPiFuRate, int nRandomRate, bool bMustGetChengSe = false,bool bIsBomber = false)
    {
        EMItemType getItemType = EMItemType.Fish;
        int nRandomValue = Random.Range(0, 101);
        //Debug.LogError(nRandomRate + "====Value =====" + nFishRate + "=====" + nPiFuRate + "=====" + nRandomValue);
        ///�жϻ�ȡ�ĸ���
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
    /// չʾ�����Ϣ������ȡ��Ӧ�����
    /// </summary>
    /// <param name="info"></param>
    public void ShowFishInfo(CFishInfo info, bool bAddCoin, bool bShowRank, 
        EMFishCoinAddFunc emAddFunc, bool matPay, DelegateNFuncCall deleTest = null,bool bRecord = true)
    {
        SpecialCastInfo castInfo = new SpecialCastInfo();
        castInfo.playerInfo = pInfo;
        castInfo.fishInfo = info;

        ///�жϻ�ȡ����Ʒ�Ƿ�Ϊϡ������
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
                    nAddChampionCount = CGameColorFishMgr.Ins.pStaticConfig.GetInt("�������ӻʹ�����");
                }
                else
                {
                    nAddChampionCount = CGameColorFishMgr.Ins.pStaticConfig.GetInt("����ӻʹ�����");
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
            ///���ôε�����Ϣ�����������б�
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
                AddCoinByHttp((matPay ? info.lPrice * CGameColorFishMgr.Ins.pStaticConfig.GetInt("�����װ���ⱶ��") : info.lPrice),
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

            ///�ж��Ƿ��ȡ���泡����
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

            //�ж��Ƿ������ϡ����
            if (info.nFesPack > 0 &&
                info.nFesPoint > 0)
            {
            //    CFishFesInfoMgr.Ins.AddFesPoint(info.nFesPack, info.nFesPoint, pInfo.uid);
            }
        }

        if (bShowRank)
        {
            ///���ô���Ϣ���뵽�Ҳ����Ϣչʾ�б�
            UIShowRoot.AddCastInfo(castInfo);
        }

        /////�ж��Ƿ������Ƥ�����͵���Ʒ
        //if (info.emItemType == EMItemType.FishMat)
        //{
        //    AddPifuChip((int)info.fCurSize);
        //}

        //��¼�嵥
        CFishRecordMgr.Ins.AddRecord(info, pInfo);
    }

    /// <summary>
    /// չʾ�����Ϣ������ȡ��Ӧ�����
    /// </summary>
    /// <param name="info"></param>
    void ShowVSFishInfo(CFishInfo info)
    {
        SpecialCastInfo castInfo = new SpecialCastInfo();
        castInfo.playerInfo = pInfo;
        castInfo.fishInfo = info;

        ///�жϻ�ȡ����Ʒ�Ƿ�Ϊϡ������
        if ((int)info.emRare >= (int)EMRare.Special)
        {
            UISpecialCast.AddCastInfo(castInfo);

            if(CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Battle)
            {
                CRankTreasureMgr.Ins.AddInfo(castInfo);
            }
        }

        //��¼�嵥
        if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.Battle)
        {
            CFishRecordMgr.Ins.AddRecord(info, pInfo);
        }   
    }

    #region ����Э��

    /// <summary>
    /// ����Ƥ����Ƭ������Э�飩
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
    /// ���ӽ�ң�����Э�飩
    /// </summary>
    /// <param name="nlValue"></param>
    public void AddCoinByHttp(long nlValue, EMFishCoinAddFunc func, bool addWinner, bool record = true)
    {
        //if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle)
        //    return;

        bool bVtb = pInfo.emUserType == CPlayerBaseInfo.EMUserType.Zhubo;

        //ͬ�����ʹ�õ��ߵ�����
        CPlayerNetHelper.AddFishCoin(uid,
                                     nlValue,
                                     func,
                                     addWinner);

        //��¼��������
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
    /// �������������Э�飩
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
    /// �������������Э�飩
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
    /// �������/��Ư/����/���ߣ�����Э�飩
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
    /// ��ȡ��ǰ���㶯���ļ���
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
            fCheckExtTime = CGameColorFishMgr.Ins.pStaticConfig.GetInt("�ܶ�����ʱ��") * 60;
        }
        else if (pInfo.nVipPlayer == CPlayerBaseInfo.EMVipLv.Tidu)
        {
            fCheckExtTime = CGameColorFishMgr.Ins.pStaticConfig.GetInt("�ᶽ����ʱ��") * 60;
        }
        else if (pInfo.nVipPlayer == CPlayerBaseInfo.EMVipLv.Pro)
        {
            fCheckExtTime = CGameColorFishMgr.Ins.pStaticConfig.GetInt("�ᶽ����ʱ��") * 60;
        }
        else
        {
            fCheckExtTime = CGameColorFishMgr.Ins.pGameRoomConfig.nAutoExitTime;
        }
        uid = pInfo.uid;
        tranSelf = GetComponent<Transform>();
        SetLaGanState(false);
        InitFSM();

        //����Avatar
        InitAvatar();

        SetState(EMState.Idle);
        ///�ճ���Ĭ��Ϊ��Ծ״̬
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
    /// ��֤����
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
        if (pInfo.GameCoins <= CGameColorFishMgr.Ins.pStaticConfig.GetInt("����֤�����������"))
        {
            return;
        }
        if (pInfo.nLv <= CGameColorFishMgr.Ins.pStaticConfig.GetInt("����֤�����ɫ�ȼ�"))
        {
            return;
        }
        if (CGameColorFishMgr.Ins.nCurRateUpLv <= CGameColorFishMgr.Ins.pStaticConfig.GetInt("����֤�����泡�ȼ�"))
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
        //���뵥λ��UI��Ϣ
        UIGameInfo uiShow = UIManager.Instance.GetUI(UIResType.GameInfo) as UIGameInfo;
        if (uiShow == null)
        {
            return;
        }
        UIShowUnitDialog uiUnitInfo = uiShow.GetShowDialogSlot(uid);
        if (uiUnitInfo == null)
            return; 
        
        ///�ж��Ƿ������֤����
        int nRandomRate = Random.Range(1, 101);
        if (nRandomRate <= fCheckYZMRate)
        {
            uiUnitInfo.SetDmContentByCheck();
        }
    }

    /// <summary>
    /// ��ʼ��Avatar
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
            Debug.LogError("�����Avatar��Ϣ:" + pInfo.avatarId);
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

        //�Ƴ����еĵ����
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
            Debug.LogError("�����FishGan��Ϣ:" + id);
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
    /// ���û�Ծ״̬
    /// </summary>
    /// <param name="active"></param>
    public void SetActiveState(bool active)
    {
        bActive = active;
        if(bActive)
        {
            fFinishStayTime = CGameColorFishMgr.Ins.pStaticConfig.GetInt("��Ծ����ʱ��");
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
            fFinishStayTime = CGameColorFishMgr.Ins.pStaticConfig.GetInt("�ǻ�Ծ����ʱ��");
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
               CPlayerMgr.Ins.GetAllIdleUnit().Count < CGameColorFishMgr.Ins.pStaticConfig.GetInt("�������ж�����"))
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
                        ///�жϸ���������Ƿ񻹴�������
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

                    ///�������������޷��ϰ�
                    if (CHelpTools.CheckAuctionByUID(uid))
                    {
                        ResetExitTick();
                        return;
                    }

                    if (CGameColorFishMgr.Ins.emCurGameType == CGameColorFishMgr.EMGameType.TimeBattle &&
                        CPlayerMgr.Ins.GetAllIdleUnit().Count < CGameColorFishMgr.Ins.pStaticConfig.GetInt("�������ж�����"))
                    {
                        ResetExitTick();
                        return;
                    }

                    ClearExitTick();
                    //ʱ�䵽���Զ������뿪��Ϸ�ĵ�Ļ
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
                //ʱ�䵽���Զ������뿪��Ϸ�ĵ�Ļ
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
    /// �ȱĽ�CD
    /// </summary>
    public void DoZibengCD()
    {
        pTicerCDZibeng = new CPropertyTimer();
        pTicerCDZibeng.Value = fCDZibeng;
        pTicerCDZibeng.FillTime();
    }

    /// <summary>
    /// �ж��ȱ��Ƿ���CD��
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
