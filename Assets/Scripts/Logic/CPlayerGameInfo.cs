using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家游戏数据
/// </summary>
public class CPlayerGameInfo 
{
    public long lOwnerID;

    //public int nSlotIdx;    //位置

    //public EMMapSlotColor emColor;  //位置颜色


    public enum EMState
    {
        Normal, //普通连接中
        Robot,  //机器人托管
    }

    EMState emState = EMState.Normal;
    public void SetState(EMState state)
    {
        emState = state;
        dlgChgState?.Invoke((int)emState);
    }

    public EMState GetState()
    {
        return emState;
    }

    public DelegateIFuncCall dlgChgState;
}
