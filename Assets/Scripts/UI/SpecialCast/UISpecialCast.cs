using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftCastInfo
{
    public CPlayerBaseInfo playerInfo;      //�����Ϣ
    public ST_GiftMenuByBoat giftInfo;      //������Ϣ
    //public 
}

public class SpecialCastInfo
{
    public CPlayerBaseInfo playerInfo;      //�����Ϣ
    public CFishInfo fishInfo;            //����Ϣ
    public string szShowText;           //չʾ�ı�
    public int nAddChampionCount;       //���ӻʹ�����
}


/// <summary>
/// �����ھ���Ϣ
/// </summary>
public class BattleCastInfo
{
    public CPlayerBaseInfo playerInfo;      //�����Ϣ
    public long nlTotalReward;              //�ܽ���
    public bool bSuc;                       //�ɹ����о���
}

/// <summary>
/// ������Ϣ
/// </summary>
public class AuctionCastInfo
{
    public CPlayerBaseInfo playerInfo;      //�����Ϣ
    public long nlTotalReward;              //�ܽ���
}

public class MatAuctionCastInfo
{
    public CPlayerBaseInfo playerInfo;      //�����Ϣ
    public EMMaterialType emMatType;        //��Ʒ����
    public int nMatNum;                     //��Ʒ����
    public int nMatID;                      //��ƷID
    public string szMatIcon;                    //��ƷͼƬ
    public EMPayType emPayType;             //�������� 
    public long nPayNum;                    //��������
    public int nPayID;                      //����ID
    public string szIcon;
}

public class UISpecialCast : UIBase
{
    public UISpecialCastRollEvent uiRoll;

    public UISpecialShowTip specialShowTip;

    public UISpecialBattleShow specialBattleShow;

    public UISpecialAuctionShow specialAuctionShow;

    public UISpecialMatAuctionShow specialMatAuctionShow;

    public UISpecialGiftShow specialGiftShow;

    [ReadOnly]
    public List<SpecialCastInfo> listWaitInfos = new List<SpecialCastInfo>();

    [Header("���������������")]
    public int nSaveCount;

    //public string[] strInfos = new string[] { "1", "2", "3", "4", "5", "6" };

    int nIdx = 0;

    public float fStayTime = 2;         //�ֶεȴ�ʱ��

    CPropertyTimer pStayTime = null;    //�ֶεȴ���ʱ��

    bool bRolling;      //�Ƿ����ֶι�����

    public string AddInfo;

    public override void OnOpen()
    {
        Init();
    }

    public void Clear()
    {
        listWaitInfos.Clear();
    }

    /// <summary>
    /// �Ƿ��ڵ�ǰ�ֶ�չʾ��
    /// </summary>
    /// <returns></returns>
    bool bShowText()
    {
        bool bShow = false;

        if (bRolling
            || pStayTime != null)
        {
            bShow = true;
        }

        return bShow;
    }

    private void Update()
    {
        if (pStayTime != null &&
            pStayTime.Tick(CTimeMgr.DeltaTime))
        {
            pStayTime = null;
            if (listWaitInfos.Count > 0)
            {
                AddNewInfo(listWaitInfos[0]);
                listWaitInfos.RemoveAt(0);
            }
        }
    }

    public static void AddCastInfo(SpecialCastInfo szInfo)
    {
        UISpecialCast specialCast = UIManager.Instance.GetUI(UIResType.SpecialCast) as UISpecialCast;
        if (specialCast != null &&
            specialCast.IsOpen() &&
            CGameColorFishMgr.Ins.bShowPlayerInfo)
        {
            specialCast.AddNewInfo(szInfo);
        }
    }

    void AddNewInfo(SpecialCastInfo szInfo)
    {
        if (bShowText())
        {
            if (listWaitInfos.Count < nSaveCount)
            {
                listWaitInfos.Add(szInfo);
            }
        }
        else
        {
            pStayTime = null;
            bRolling = true;
            specialShowTip.InitInfo(szInfo);

            if(szInfo.fishInfo!=null &&
               szInfo.fishInfo.emRare >= EMRare.Special)
            {
                if(szInfo.fishInfo.nTBID == 1000001 ||
                   szInfo.fishInfo.nTBID == 1000002)
                {
                    if(szInfo.fishInfo.bBianYi)
                    {
                        CGameColorFishMgr.Ins.pAudioBroad.Play((CConfigFishBroad.EMBroadModel)CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.BROADMODEL), CConfigFishBroad.EMBroadType.PlayerFishBY);
                    }
                    else
                    {
                        CGameColorFishMgr.Ins.pAudioBroad.Play((CConfigFishBroad.EMBroadModel)CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.BROADMODEL), CConfigFishBroad.EMBroadType.PlayerFish);
                    }  
                }
                else if(szInfo.fishInfo.nTBID == 3000004)
                {
                    //CGameColorFishMgr.Ins.pAudioBroad.Play((CConfigFishBroad.EMBroadModel)CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.BROADMODEL), CConfigFishBroad.EMBroadType.Suipian100);
                }
                else
                {
                    if (szInfo.fishInfo.bBianYi)
                    {
                        CGameColorFishMgr.Ins.pAudioBroad.Play((CConfigFishBroad.EMBroadModel)CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.BROADMODEL), CConfigFishBroad.EMBroadType.FishBY);
                    }
                    else
                    {
                        CGameColorFishMgr.Ins.pAudioBroad.Play((CConfigFishBroad.EMBroadModel)CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.BROADMODEL), CConfigFishBroad.EMBroadType.UltraFish);
                    }
                }
            }
            //else
            //{
            //    CGameColorFishMgr.Ins.pAudioBroad.Play((CConfigFishBroad.EMBroadModel)CSystemInfoMgr.Inst.GetInt(CSystemInfoConst.BROADMODEL), CConfigFishBroad.EMBroadType.RareFish);
            //}

            //uiRoll.InitInfo(listBroadCastInfos.Count);
            //uiRoll.RollNext();
        }   
    }

    public static void ShowBattleInfo(BattleCastInfo castInfo,ShowBoardType type)
    {
        UISpecialCast specialCast = UIManager.Instance.GetUI(UIResType.SpecialCast) as UISpecialCast;
        if (specialCast != null &&
           specialCast.IsOpen())
        {
            specialCast.specialBattleShow.InitInfo(castInfo,type);
        }
        
    }

    public static void ShowAuctionInfo(AuctionCastInfo castInfo)
    {
        UISpecialCast specialCast = UIManager.Instance.GetUI(UIResType.SpecialCast) as UISpecialCast;
        if (specialCast != null &&
           specialCast.IsOpen())
        {
            specialCast.specialAuctionShow.InitInfo(castInfo);
        }
    }

    public static void ShowMatAuctionInfo(MatAuctionCastInfo castInfo)
    {
        UISpecialCast specialCast = UIManager.Instance.GetUI(UIResType.SpecialCast) as UISpecialCast;
        if (specialCast != null &&
           specialCast.IsOpen())
        {
            specialCast.specialMatAuctionShow.InitInfo(castInfo);
        }
    }

    public static void ShowGiftInfo(GiftCastInfo castInfo)
    {
        UISpecialCast specialCast = UIManager.Instance.GetUI(UIResType.SpecialCast) as UISpecialCast;
        if (specialCast != null &&
           specialCast.IsOpen())
        {
            specialCast.specialGiftShow.InitInfo(castInfo);
        }
    }

    public void Init()
    {
        //uiRoll.dlgChgSlot = this.OnRefreshSlot;
        //uiRoll.dlgRollEnd = this.OnRollEnd;
        specialShowTip.dlgTweenEnd = this.OnRollEnd;
    }

    void OnRollEnd()
    {
        pStayTime = new CPropertyTimer();
        pStayTime.Value = fStayTime;
        pStayTime.FillTime();
        bRolling = false;
    }
    //void OnRefreshSlot(UISpecialCastSlot slot, int idx)
    //{
    //    if (idx == -1)
    //    {
    //        slot.uiTextContent.text = "";
    //        slot.uiShowImg.enabled = false;
    //    }
    //    else
    //    {
    //        slot.uiTextContent.text = listBroadCastInfos[idx].playerInfo.userName + "������" + listBroadCastInfos[idx].fishInfo.szName;
    //        //if(listBroadCastInfos[idx].sprite == null)
    //        //{
    //            slot.uiShowImg.enabled = false;
    //        //}
    //        //else
    //        //{
    //        //    slot.uiShowImg.enabled = true;
    //        //    slot.uiShowImg.sprite = listBroadCastInfos[idx].sprite;
    //        //}
    //    }
    //}




}
