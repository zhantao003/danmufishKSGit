using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSpecialGiftInfo 
{
    public enum EMGiftType
    {
        Item1,          //ը��
        Item2,          //���ؿ�Ͷ
        Item3,          //������Ͷ
        Item4,          //ħ����
        Item5           //��Ů��
    }

    public EMGiftType emType = EMGiftType.Item1;
    public long count;
}
