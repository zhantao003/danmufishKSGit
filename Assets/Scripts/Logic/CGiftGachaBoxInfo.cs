using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGiftGachaBoxInfo
{
    public enum EMGiftType
    {
        FishCoin = 0,   //���
        Yuju,           //���
        Feilun,         //����
        Role,           //��ɫ
        Boat,           //��
        FishGan,        //���
    }

    public EMGiftType emType = EMGiftType.FishCoin;
    public int nItemID;
}
