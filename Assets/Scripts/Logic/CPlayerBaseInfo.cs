using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerBaseInfo
{
    public enum EMUserType
    {
        Zhubo = 0,      //����
        Guanzhong = 1,  //����
    }

    public enum EMState
    {
        None,

        IdleQueue,       //�ŶӴ�����
        ActiveQueue,     //��Ϸ������
        Gaming,          //��Ϸ��
    }

    /// <summary>
    /// �û�UID
    /// </summary>
    public string uid;

    /// <summary>
    /// Ƥ��Id
    /// </summary>
    public int avatarId;

    /// <summary>
    /// �û��ǳ�
    /// </summary>
    public string userName;

    /// <summary>
    /// �û�ͷ��
    /// </summary>
    public string userFace;

    /// <summary>
    /// ��˿ѫ�µȼ�
    /// </summary>
    public long fansMedalLevel;

    /// <summary>
    /// ��˿ѫ����
    /// </summary>
    public string fansMedalName;

    /// <summary>
    /// ����ķ�˿ѫ�����״̬
    /// </summary>
    public bool fansMedalWearingStatus;

    /// <summary>
    /// �󺽺��ȼ�
    /// </summary>
    public long guardLevel;

    public EMUserType emUserType = EMUserType.Guanzhong;

    public EMState emState = EMState.None;

    /// <summary>
    /// ��Ļ���յ�ֱ����
    /// </summary>
    public string roomId;

    public int nLv;         //��Ϸ�ȼ�

    
    long nGameCoins; //���һ���
    public DelegateLFuncCall dlgChgCoins;
    public long GameCoins
    {
        get
        {
            return nGameCoins;
        }
        set
        {
            nGameCoins = value;
            if(nGameCoins < 0)
            {
                nGameCoins = 0;
            }

            dlgChgCoins?.Invoke(nGameCoins);
        }
    }

    //Ƥ����Ƭ
    long nAvatarSuipian;
    public DelegateLFuncCall dlgChgAvatarSuipian;
    public long AvatarSuipian
    {
        get
        {
            return nAvatarSuipian;
        }
        set
        {
            nAvatarSuipian = value;
            if (nAvatarSuipian < 0)
            {
                nAvatarSuipian = 0;
            }

            dlgChgAvatarSuipian?.Invoke(nAvatarSuipian);
        }
    }


    public long nAddGameCoins = 0;  //��ʱ��ӵĽ��

    //���ػ���
    long nTreasurePoint;
    public DelegateLFuncCall dlgChgTreasurePoint;
    public long TreasurePoint
    {
        get
        {
            return nTreasurePoint;
        }
        set
        {
            nTreasurePoint = value;
            if (nTreasurePoint < 0)
            {
                nTreasurePoint = 0;
            }

            dlgChgTreasurePoint?.Invoke(nTreasurePoint);
        }
    }

    /// <summary>
    /// ��ǰ�����͵ĵ������
    /// </summary>
    public long nBattery;

    public CPlayerGameInfo pGameInfo = null;    //��Ϸ����

    public CPlayerAvatarPack pAvatarPack = new CPlayerAvatarPack();    //Ƥ������

    public CPlayerBoatPack pBoatPack = new CPlayerBoatPack();   //���ı���

    public CPlayerMatPack pMatPack = new CPlayerMatPack();      //���ϱ���

    public CPlayerFishGanPack pFishGanPack = new CPlayerFishGanPack();   //��͵ı���

    public long nlBaitCount;            //�������

    public long nlFreeBaitCount;        //����������

    public long nlRobCount;             //�������

    public long nlBuoyCount;            //��Ư����

    public long nlFeiLunCount;          //��������

    public int nBoatAvatarId;           //��Ƥ��ID

    public int nFishGanAvatarId;        //�����ID

    public long nWinnerOuhuang; //ʤ��������ŷ��
    public long nWinnerRicher;  //ʤ����������Ǯ��
    public long nFishWinnerPoint;   //��������

    public long WinnerPoint
    {
        get
        {
            return nFishWinnerPoint;
        }
    }

    public enum EMVipLv
    {
        Normal = 0,

        Green,
        Jianzhang,
        Tidu,       //�ᶽ
        Pro,        //�ٷ�
        Zongdu,     //�ܶ�
    }

    public EMVipLv nVipPlayer;              //VIP�ȼ�

    public long nUserLv;   //�û��ȼ�
    public long nUserExp;  //�û�����
    public DelegateLLFuncCall dlgUserLvChg;

    public long nGachaGiftCount;    //ä�м���

    //��������
    public Dictionary<EMAddUnitProType, int> dicAddPro = new Dictionary<EMAddUnitProType, int>();

    //�Ǽ��ȼ�
    public int nGameStarLv;

    //���÷�
    public int nCreditPoint;

    //�Ƿ������
    public bool bIsRobot = false;

    public int nCurRank;        //��ǰ����

    public CPlayerBaseInfo(string _uid, string _userName, string _userFace, long _fansMedalLevel, string _fansMedalName ,bool _fansMedalWearingStatus, long _guardLevel, string _roomId, EMUserType userType)
    {
        Init(_uid, _userName, _userFace, _fansMedalLevel, _fansMedalName, _fansMedalWearingStatus, _guardLevel, _roomId, userType);
    }

    public void Init(string _uid, string _userName, string _userFace, long _fansMedalLevel, string _fansMedalName, bool _fansMedalWearingStatus, long _guardLevel, string _roomId, EMUserType userType)
    {
        uid = _uid;
        userName = _userName;
        userFace = _userFace;
        fansMedalLevel = _fansMedalLevel;
        fansMedalName = _fansMedalName;
        fansMedalWearingStatus = _fansMedalWearingStatus;
        guardLevel = _guardLevel;
        roomId = _roomId;
        emUserType = userType;

        avatarId = 101;
        nBoatAvatarId = 101;
        nFishGanAvatarId = 101;
        nLv = 1;
    }

    /// <summary>
    /// �����û��ȼ�
    /// </summary>
    /// <param name="lv"></param>
    /// <param name="exp"></param>
    public void SetUserLv(long lv, long exp)
    {
        nUserLv = lv;
        nUserExp = exp;

        dlgUserLvChg?.Invoke(nUserLv, nUserExp);
    }

    /// <summary>
    /// �ж��Ƿ�Ϊ����
    /// </summary>
    /// <returns></returns>
    public bool CheckIsGrayName()
    {
        bool bGrayName = nVipPlayer == CPlayerBaseInfo.EMVipLv.Normal &&
                         guardLevel <= 0 &&
                         nCreditPoint <= CGameColorFishMgr.Ins.pStaticConfig.GetInt("��������ж�ֵ");

        return bGrayName;
    }

    public bool CheckHaveGift()
    {
        bool haveGift = nlBaitCount > 0 ||
                        nlFeiLunCount > 0;
        return haveGift;
    }

    /// <summary>
    /// ��ȡ��ǰ���ϵ����Լӳ�
    /// </summary>
    /// <returns></returns>
    List<CPlayerProAddInfo> listProAddRes = new List<CPlayerProAddInfo>();
    public List<CPlayerProAddInfo> GetProAddList()
    {
        listProAddRes.Clear();

        //��ȡ�������
        CPlayerFishGanInfo pGanInfo = pFishGanPack.GetInfo(nFishGanAvatarId);
        if(pGanInfo != null)
        {
            if(pGanInfo.emPro != EMAddUnitProType.None)
            {
                CPlayerProAddInfo pProInfo = new CPlayerProAddInfo();
                pProInfo.emSlot = EMAddUnitProSlot.Gan;
                pProInfo.emPro = pGanInfo.emPro;
                pProInfo.value = pGanInfo.nProAdd;
                listProAddRes.Add(pProInfo);
            }
        }

        return listProAddRes;
    }

    /// <summary>
    /// ˢ������
    /// </summary>
    public void RefreshProAdd()
    {
        dicAddPro.Clear();
        List<CPlayerProAddInfo> listPro = GetProAddList();
        for(int i=0; i<listPro.Count; i++)
        {
            if(dicAddPro.ContainsKey(listPro[i].emPro))
            {
                dicAddPro[listPro[i].emPro] += listPro[i].value;
            }
            else
            {
                dicAddPro.Add(listPro[i].emPro, listPro[i].value);
            }
        }
    }

    //�Ƿ�ӵ������
    public bool HasPro(EMAddUnitProType emProType)
    {
        if(dicAddPro.ContainsKey(emProType))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// ��ȡָ������ֵ
    /// </summary>
    /// <param name="emProType"></param>
    /// <returns></returns>
    public int GetProAddValue(EMAddUnitProType emProType)
    {
        int value = 0;
        if(dicAddPro.TryGetValue(emProType, out value))
        {

        }

        return value;
    }

    public EMVipLv GetVipLv()
    {
        if (nCurRank == 1)
        {
            return EMVipLv.Zongdu;
        }
        else if (nCurRank <= 5)
        {
            return EMVipLv.Pro;
        }
        else if (nCurRank <= 20)
        {
            return EMVipLv.Tidu;
        }
        else if (nCurRank <= 50)
        {
            return EMVipLv.Jianzhang;
        }
        else
        {
            return EMVipLv.Green;
        }
        //if (WinnerPoint >= 200)
        //{
        //    return EMVipLv.Zongdu;
        //}
        //else if (WinnerPoint >= 100)  
        //{
        //    return EMVipLv.Pro;
        //}
        //else if (WinnerPoint >= 55)  
        //{
        //    return EMVipLv.Tidu;
        //}
        //else if (WinnerPoint >= 25) 
        //{
        //    return EMVipLv.Jianzhang;
        //}
        //else if (WinnerPoint >= 5) 
        //{
        //    return EMVipLv.Green;
        //}

        return (EMVipLv)nVipPlayer;
    }

    /// <summary>
    /// ˢ�´�Ƥ��
    /// </summary>
    public void RefreshBoatAvatar()
    {
        return;

        List<ST_WinnerBoat> listBoats = CTBLHandlerWinnerBoat.Ins.GetInfos();
        listBoats.Sort((x, y) => { return y.nID.CompareTo(x.nID); });
        for(int i=0; i< listBoats.Count; i++)
        {
            if(nWinnerOuhuang >= listBoats[i].count)
            {
                nBoatAvatarId = listBoats[i].boat;
                break;
            }
        }
    }

    /// <summary>
    /// ˢ�½�ɫƤ��
    /// </summary>
    public void RefreshRoleAvatar()
    {
        return;

        List<ST_RicherBoat> listBoats = CTBLHandlerRicherBoat.Ins.GetInfos();
        listBoats.Sort((x, y) => { return y.nID.CompareTo(x.nID); });
        for (int i = 0; i < listBoats.Count; i++)
        {
            if (GameCoins >= listBoats[i].count)
            {
                avatarId = listBoats[i].roleId;
                break;
            }
        }
    }
}
