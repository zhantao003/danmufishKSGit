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
    public const int Item_Bomber = 101; //ը��
    public const int Item_Rocket = 102; //���
    public const int Item_Rob = 103;    //�Ӷ�
    public const int Item_Flag = 104;   //����
}

public enum EMDmgType
{
    Normal,
    Zibeng,
}

/// <summary>
/// �����ҵ�;��
/// </summary>
public enum EMFishCoinAddFunc
{
    Free = 0,   //���;��������û���κθ������
    Pay,        //����;��
    Duel,       //����;��
    Auction,    //����;��
}

public enum EMAddUnitProSlot
{
    None = 0,

    Boat,
    Role,
    Gan
}

//�����������
public enum EMAddUnitProType
{
    None = 0,

    FishRare = 1,   //��߳�ϡ�������
    FishSize = 2,   //�����ĳߴ�
    FishCoin = 3,   //���������棨�ٷֱȣ�
    FishTimeWait = 4,    //���ٵ���ǰ�ȴ�ʱ��
    FishTimeLagan = 5,   //���ٵ����Զ�����ʱ��
    FishMat = 6,    //��ߵ������ϸ���

    DmgAddFish = 20,    //��ߵ����ߵ��˺�(�ٷֱȣ�
    DmgAddZibeng = 21,  //����̱��˺�

    ZibengToRPG = 100,    //�̱�����ΪRPG
    ZibengToFootBall = 101,     //�̱�����Ϊ����
    ZibengToFootBallG = 102,    //�̱�����Ϊ�߽���
    ZibengToChrismas = 103,     //�̱�����Ϊʥ������
    ZibengToCuanTianHou = 104,  //�̱�����Ϊ�����
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
                    szRes = "���";
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
                    szContent += $"ϡ����+{v}%";
                }
                break;
            case EMAddUnitProType.FishSize:
                {
                    szContent += $"��ߴ�+{(v * 0.1f).ToString("F1")}%";
                }
                break;
            case EMAddUnitProType.FishCoin:
                {
                    szContent += $"�������+{v}%";
                }
                break;
            case EMAddUnitProType.FishTimeWait:
                {
                    szContent += $"�ȴ�ʱ��-{v}%";
                }
                break;
            case EMAddUnitProType.FishTimeLagan:
                {
                    szContent += $"�Զ�����-{v}%";
                }
                break;
            case EMAddUnitProType.FishMat:
                {
                    szContent += $"����+{v}%";
                }
                break;
            case EMAddUnitProType.DmgAddFish:
                {
                    szContent += $"�����˺�+{v}%";
                }
                break;
            case EMAddUnitProType.DmgAddZibeng:
                {
                    szContent += $"�̱��˺�+{v}%";
                }
                break;
            case EMAddUnitProType.ZibengToRPG:
                {
                    szContent += $"RPG�˺�:{((200 + v) * 0.01F).ToString("F2")}��";
                }
                break;
            case EMAddUnitProType.ZibengToFootBall:
            case EMAddUnitProType.ZibengToFootBallG:
            case EMAddUnitProType.ZibengToChrismas:
            case EMAddUnitProType.ZibengToCuanTianHou:
            {
                    szContent += $"�̱���Ч�ı�";
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
                    szContent += $"{((200 + pTBLGanInfo.nProAddMin)*0.01F).ToString("F2")}��-{((200 + pTBLGanInfo.nProAddMax) * 0.01F).ToString("F2")}��";
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
                    szContent += $"ϡ�����������";
                }
                break;
            case EMAddUnitProType.FishSize:
                {
                    szContent += $"��ߴ�����";
                }
                break;
            case EMAddUnitProType.FishCoin:
                {
                    szContent += $"�����������";
                }
                break;
            case EMAddUnitProType.FishTimeWait:
                {
                    szContent += $"�ȴ�ʱ�����";
                }
                break;
            case EMAddUnitProType.FishTimeLagan:
                {
                    szContent += $"�Զ�����ʱ�����";
                }
                break;
            case EMAddUnitProType.FishMat:
                {
                    szContent += $"���ϸ�������%";
                }
                break;
            case EMAddUnitProType.DmgAddFish:
                {
                    szContent += $"�����˺�����%";
                }
                break;
            case EMAddUnitProType.DmgAddZibeng:
                {
                    szContent += $"�̱��˺�����%";
                }
                break;
            case EMAddUnitProType.ZibengToRPG:
                {
                    szContent += $"���Է���RPG";
                }
                break;
            case EMAddUnitProType.ZibengToFootBall:
            case EMAddUnitProType.ZibengToFootBallG:
            case EMAddUnitProType.ZibengToChrismas:
            case EMAddUnitProType.ZibengToCuanTianHou:
                {
                    szContent += $"�̱���Ч�ı�";
                }
                break;
        }

        return szContent;
    }
}