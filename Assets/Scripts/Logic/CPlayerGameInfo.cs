using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����Ϸ����
/// </summary>
public class CPlayerGameInfo 
{
    public long lOwnerID;

    //public int nSlotIdx;    //λ��

    //public EMMapSlotColor emColor;  //λ����ɫ


    public enum EMState
    {
        Normal, //��ͨ������
        Robot,  //�������й�
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
