using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerBaseInfo
{
    public enum EMUserType
    {
        Zhubo = 0,      //主播
        Guanzhong = 1,  //观众
    }

    public enum EMState
    {
        None,

        IdleQueue,       //排队待机中
        ActiveQueue,     //游戏待机中
        Gaming,          //游戏中
    }

    /// <summary>
    /// 用户UID
    /// </summary>
    public string uid;

    /// <summary>
    /// 皮肤Id
    /// </summary>
    public int avatarId;

    /// <summary>
    /// 用户昵称
    /// </summary>
    public string userName;

    /// <summary>
    /// 用户头像
    /// </summary>
    public string userFace;

    /// <summary>
    /// 粉丝勋章等级
    /// </summary>
    public long fansMedalLevel;

    /// <summary>
    /// 粉丝勋章名
    /// </summary>
    public string fansMedalName;

    /// <summary>
    /// 佩戴的粉丝勋章佩戴状态
    /// </summary>
    public bool fansMedalWearingStatus;

    /// <summary>
    /// 大航海等级
    /// </summary>
    public long guardLevel;

    public EMUserType emUserType = EMUserType.Guanzhong;

    public EMState emState = EMState.None;

    /// <summary>
    /// 弹幕接收的直播间
    /// </summary>
    public string roomId;

    public int nLv;         //游戏等级

    
    long nGameCoins; //代币积分
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

    //皮肤碎片
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


    public long nAddGameCoins = 0;  //临时添加的金币

    //宝藏积分
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
    /// 当前场次送的电池总量
    /// </summary>
    public long nBattery;

    public CPlayerGameInfo pGameInfo = null;    //游戏数据

    public CPlayerAvatarPack pAvatarPack = new CPlayerAvatarPack();    //皮肤背包

    public CPlayerBoatPack pBoatPack = new CPlayerBoatPack();   //船的背包

    public CPlayerMatPack pMatPack = new CPlayerMatPack();      //材料背包

    public CPlayerFishGanPack pFishGanPack = new CPlayerFishGanPack();   //鱼竿的背包

    public long nlBaitCount;            //鱼饵数量

    public long nlFreeBaitCount;        //免费鱼饵数量

    public long nlRobCount;             //鱼杆数量

    public long nlBuoyCount;            //浮漂数量

    public long nlFeiLunCount;          //飞轮数量

    public int nBoatAvatarId;           //船皮肤ID

    public int nFishGanAvatarId;        //钓鱼竿ID

    public long nWinnerOuhuang; //胜利点数：欧皇
    public long nWinnerRicher;  //胜利点数：有钱人
    public long nFishWinnerPoint;   //赛季积分

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
        Tidu,       //提督
        Pro,        //官方
        Zongdu,     //总督
    }

    public EMVipLv nVipPlayer;              //VIP等级

    public long nUserLv;   //用户等级
    public long nUserExp;  //用户经验
    public DelegateLLFuncCall dlgUserLvChg;

    public long nGachaGiftCount;    //盲盒计数

    //增益属性
    public Dictionary<EMAddUnitProType, int> dicAddPro = new Dictionary<EMAddUnitProType, int>();

    //星级等级
    public int nGameStarLv;

    //信用分
    public int nCreditPoint;

    //是否机器人
    public bool bIsRobot = false;

    public int nCurRank;        //当前排名

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
    /// 设置用户等级
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
    /// 判断是否为灰名
    /// </summary>
    /// <returns></returns>
    public bool CheckIsGrayName()
    {
        bool bGrayName = nVipPlayer == CPlayerBaseInfo.EMVipLv.Normal &&
                         guardLevel <= 0 &&
                         nCreditPoint <= CGameColorFishMgr.Ins.pStaticConfig.GetInt("变灰信用判断值");

        return bGrayName;
    }

    public bool CheckHaveGift()
    {
        bool haveGift = nlBaitCount > 0 ||
                        nlFeiLunCount > 0;
        return haveGift;
    }

    /// <summary>
    /// 获取当前身上的属性加成
    /// </summary>
    /// <returns></returns>
    List<CPlayerProAddInfo> listProAddRes = new List<CPlayerProAddInfo>();
    public List<CPlayerProAddInfo> GetProAddList()
    {
        listProAddRes.Clear();

        //获取鱼竿属性
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
    /// 刷新属性
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

    //是否拥有属性
    public bool HasPro(EMAddUnitProType emProType)
    {
        if(dicAddPro.ContainsKey(emProType))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 获取指定属性值
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
    /// 刷新船皮肤
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
    /// 刷新角色皮肤
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
