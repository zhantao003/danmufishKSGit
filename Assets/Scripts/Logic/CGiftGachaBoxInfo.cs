using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGiftGachaBoxInfo
{
    public enum EMGiftType
    {
        FishCoin = 0,   //Óã±Ò
        Yuju,           //Óæ¾ß
        Feilun,         //·ÉÂÖ
        Role,           //½ÇÉ«
        Boat,           //´¬
        FishGan,        //Óã¸Í
    }

    public EMGiftType emType = EMGiftType.FishCoin;
    public int nItemID;
}
