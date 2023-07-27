using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUnitAnimeConst
{
    public const string Anime_Idle = "Idle";
    public const string Anime_Move = "Move";
    public const string Anime_Jump = "Jump";
    public const string Anime_StartFish = "StartFinish";
    public const string Anime_Fishing = "Finishing";
    public const string Anime_EndFish = "EndFinish";
    public const string Anime_ShowFish = "ShowFinish";
    public const string Anime_Ready = "Ready";
    public const string Anime_Win = "Win";
    public const string Anime_Win2 = "Win2";
    public const string Anime_Lose = "Lose";
}

public class CUnitMatAnimeConst
{
    public const string Anime_Idle = "Idle";
    public const string Anime_Black = "Black";
}

public class CGachaBoxAnimeConst
{
    public const string Anime_Idle = "Idle";
    public const string Anime_Born = "Born";
    public const string Anime_Open = "Open";
    public const string Anime_Close = "Close";
}

public class CBossAnimeConst
{
    public const string Anime_Idle = "Idle";
    public const string Anime_Dead = "Dead";
    public const string Anime_Escape = "Dead";
    public const string Anime_Atk = "FlyDash";
    public const string Anime_OnHit = "OnHit";
}

public class CGameItemConst
{
    public const int Item_Bomber = 101; //炸弹
    public const int Item_Rocket = 102; //火箭
    public const int Item_Rob = 103;    //掠夺
    public const int Item_Flag = 104;   //插旗
}

public enum EMDmgType
{
    Normal,
    Zibeng,
}

/// <summary>
/// 添加鱼币的途径
/// </summary>
public enum EMFishCoinAddFunc
{
    Free = 0,   //免费途径：身上没有任何付费渔具
    Pay,        //付费途径
    Duel,       //决斗途径
    Auction,    //拍卖途径
}

public enum EMAddUnitProSlot
{
    None = 0,

    Boat,
    Role,
    Gan
}

//属性增益相关
public enum EMAddUnitProType
{
    None = 0,

    FishRare = 1,   //提高出稀有鱼概率
    FishSize = 2,   //提高鱼的尺寸
    FishCoin = 3,   //提高鱼币收益（百分比）
    FishTimeWait = 4,    //减少钓鱼前等待时间
    FishTimeLagan = 5,   //减少钓鱼自动拉杆时间
    FishMat = 6,    //提高钓到材料概率

    DmgAddFish = 20,    //提高钓道具的伤害(百分比）
    DmgAddZibeng = 21,  //提高滋崩伤害

    ZibengToRPG = 100,    //滋崩升级为RPG
    ZibengToFootBall = 101,     //滋崩升级为踢球
    ZibengToFootBallG = 102,    //滋崩升级为踢金球
    ZibengToChrismas = 103,     //滋崩升级为圣诞星星
    ZibengToCuanTianHou = 104,  //滋崩升级为窜天猴
}

public class CPlayerProAddInfo
{
    public EMAddUnitProSlot emSlot;
    public EMAddUnitProType emPro;
    public int value;

    public string GetSlotString()
    {
        string szRes = "";
        switch(emSlot)
        {
            case EMAddUnitProSlot.Gan:
                {
                    szRes = "鱼竿";
                }
                break;
        }

        return szRes;
    }

    public static string GetProString(EMAddUnitProType proType, int v)
    {
        string szContent = "";
        switch (proType)
        {
            case EMAddUnitProType.FishRare:
                {
                    szContent += $"稀有鱼+{v}%";
                }
                break;
            case EMAddUnitProType.FishSize:
                {
                    szContent += $"鱼尺寸+{(v * 0.1f).ToString("F1")}%";
                }
                break;
            case EMAddUnitProType.FishCoin:
                {
                    szContent += $"钓鱼鱼币+{v}%";
                }
                break;
            case EMAddUnitProType.FishTimeWait:
                {
                    szContent += $"等待时间-{v}%";
                }
                break;
            case EMAddUnitProType.FishTimeLagan:
                {
                    szContent += $"自动拉杆-{v}%";
                }
                break;
            case EMAddUnitProType.FishMat:
                {
                    szContent += $"材料+{v}%";
                }
                break;
            case EMAddUnitProType.DmgAddFish:
                {
                    szContent += $"道具伤害+{v}%";
                }
                break;
            case EMAddUnitProType.DmgAddZibeng:
                {
                    szContent += $"滋崩伤害+{v}%";
                }
                break;
            case EMAddUnitProType.ZibengToRPG:
                {
                    szContent += $"RPG伤害:{((200 + v) * 0.01F).ToString("F2")}倍";
                }
                break;
            case EMAddUnitProType.ZibengToFootBall:
            case EMAddUnitProType.ZibengToFootBallG:
            case EMAddUnitProType.ZibengToChrismas:
            case EMAddUnitProType.ZibengToCuanTianHou:
            {
                    szContent += $"滋崩特效改变";
                }
                break;
        }

        return szContent;
    }

    public static string GetProRangeString(int nGanId)
    {
        string szContent = "";
        ST_UnitFishGan pTBLGanInfo = CTBLHandlerUnitFishGan.Ins.GetInfo(nGanId);
        if (pTBLGanInfo == null) return "";

        switch(pTBLGanInfo.emProType)
        {
            case EMAddUnitProType.FishRare:
                {
                    szContent += $"{pTBLGanInfo.nProAddMin * 0.1F}%-{pTBLGanInfo.nProAddMax * 0.1F}%";
                }
                break;
            case EMAddUnitProType.FishSize:
                {
                    szContent += $"{(pTBLGanInfo.nProAddMin * 0.1F).ToString("F1")}%-{(pTBLGanInfo.nProAddMax * 0.1F).ToString("F1")}%";
                }
                break;
            case EMAddUnitProType.FishCoin:
                {
                    szContent += $"{pTBLGanInfo.nProAddMin * 0.1F}%-{pTBLGanInfo.nProAddMax * 0.1F}%";
                }
                break;
            case EMAddUnitProType.FishTimeWait:
                {
                    szContent += $"{pTBLGanInfo.nProAddMin * 0.1F}%-{pTBLGanInfo.nProAddMax * 0.1F}%";
                }
                break;
            case EMAddUnitProType.FishTimeLagan:
                {
                    szContent += $"{pTBLGanInfo.nProAddMin * 0.1F}%-{pTBLGanInfo.nProAddMax * 0.1F}%";
                }
                break;
            case EMAddUnitProType.FishMat:
                {
                    szContent += $"{pTBLGanInfo.nProAddMin * 0.1F}%-{pTBLGanInfo.nProAddMax * 0.1F}%";
                }
                break;
            case EMAddUnitProType.DmgAddFish:
                {
                    szContent += $"{pTBLGanInfo.nProAddMin * 0.1F}%-{pTBLGanInfo.nProAddMax * 0.1F}%";
                }
                break;
            case EMAddUnitProType.DmgAddZibeng:
                {
                    szContent += $"{pTBLGanInfo.nProAddMin * 0.1F}%-{pTBLGanInfo.nProAddMax * 0.1F}%";
                }
                break;
            case EMAddUnitProType.ZibengToRPG:
                {
                    szContent += $"{((200 + pTBLGanInfo.nProAddMin)*0.01F).ToString("F2")}倍-{((200 + pTBLGanInfo.nProAddMax) * 0.01F).ToString("F2")}倍";
                }
                break;
            case EMAddUnitProType.ZibengToFootBall:
            case EMAddUnitProType.ZibengToFootBallG:
            case EMAddUnitProType.ZibengToChrismas:
            case EMAddUnitProType.ZibengToCuanTianHou:
                {
                    szContent += "";
                }
                break;
        }

        return szContent;
    }

    public static string GetProTypeString(EMAddUnitProType proType)
    {
        string szContent = "";
        switch (proType)
        {
            case EMAddUnitProType.FishRare:
                {
                    szContent += $"稀有鱼概率提升";
                }
                break;
            case EMAddUnitProType.FishSize:
                {
                    szContent += $"鱼尺寸提升";
                }
                break;
            case EMAddUnitProType.FishCoin:
                {
                    szContent += $"钓鱼鱼币提升";
                }
                break;
            case EMAddUnitProType.FishTimeWait:
                {
                    szContent += $"等待时间减少";
                }
                break;
            case EMAddUnitProType.FishTimeLagan:
                {
                    szContent += $"自动拉杆时间减少";
                }
                break;
            case EMAddUnitProType.FishMat:
                {
                    szContent += $"材料概率增加%";
                }
                break;
            case EMAddUnitProType.DmgAddFish:
                {
                    szContent += $"道具伤害增加%";
                }
                break;
            case EMAddUnitProType.DmgAddZibeng:
                {
                    szContent += $"滋崩伤害增加%";
                }
                break;
            case EMAddUnitProType.ZibengToRPG:
                {
                    szContent += $"可以发射RPG";
                }
                break;
            case EMAddUnitProType.ZibengToFootBall:
            case EMAddUnitProType.ZibengToFootBallG:
            case EMAddUnitProType.ZibengToChrismas:
            case EMAddUnitProType.ZibengToCuanTianHou:
                {
                    szContent += $"滋崩特效改变";
                }
                break;
        }

        return szContent;
    }
}