using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSpecialGiftInfo 
{
    public enum EMGiftType
    {
        Item1,          //炸弹
        Item2,          //神秘空投
        Item3,          //超级空投
        Item4,          //魔法镜
        Item5           //仙女棒
    }

    public EMGiftType emType = EMGiftType.Item1;
    public long count;
}
